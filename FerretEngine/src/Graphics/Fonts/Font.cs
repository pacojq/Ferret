using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpFont;

namespace FerretEngine.Graphics.Fonts
{
    public class Font
    {
        private static int TextureSize = 1024;
        
        
        public char? DefaultCharacter
        {
            get { return _defaultChar; }
            set
            {
                if (value == null)
                {
                    _defaultChar = null;
                    _defaultGlyph = null;
                    return;
                }

                var c = value.Value;
                if (!TryGetGlyph(c, out var glyph))
                    throw new Exception($"Invalid character '{c}'");

                _defaultChar = c;
                _defaultGlyph = glyph;
            }
        }

        public int TabSpaces { get; set; }

        public int Size { get; }
        public int LineHeight { get; }

        

        private GlyphInfo? _defaultGlyph;
        private char? _defaultChar;
        private readonly GraphicsDevice graphicsDevice;
        private readonly byte[] fontBytes;
        private readonly int ascender;
        private readonly int advanceSpace;
        private readonly Face face;

        private readonly List<GlyphPage> pages = new List<GlyphPage>();
        private readonly Dictionary<char, GlyphInfo> glyphs = new Dictionary<char, GlyphInfo>();
        private readonly Dictionary<char, Rectangle> boundsByChar = new Dictionary<char, Rectangle>();
        private readonly Dictionary<char, Texture2D> texturesByChar = new Dictionary<char, Texture2D>();
        private readonly Dictionary<char, Dictionary<char, int>> kerning = new Dictionary<char, Dictionary<char, int>>();
        
        
        
        
        public Font(int size, byte[] fontBytes, GraphicsDevice graphicsDevice)
        {
            Size = size;
            this.fontBytes = fontBytes;
            this.graphicsDevice = graphicsDevice;
                
            face = new Face(FontLibrary.Lib, fontBytes, 0);

            face.SetCharSize(0, Size, 0, 96);
            LineHeight = face.Size.Metrics.NominalHeight;
            ascender = (int)face.Size.Metrics.Ascender;

            face.LoadGlyph(face.GetCharIndex(32), LoadFlags.Default, LoadTarget.Normal);
            advanceSpace = (int)face.Glyph.Metrics.HorizontalAdvance;
        }

        public void PreheatCache(IEnumerable<char> chars)
        {
            foreach (var c in chars)
                TryGetGlyph(c, out _);
        }
        
        
        internal Text MakeText(string text)
        {
            //  Heavily modified from https://gist.github.com/suzusime/b644eedffba87001427291d79f36a955
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            var set = new HashSet<Texture2D>();
            var result = new List<Glyph>();
            float maxWidth = 0, maxHeight = 0;

            if (text.Length > 0)
            {
                int currLineWidth = 0, currLineHeight = LineHeight;

                bool firstInLine = true;
                float underrun = 0, overrun = 0;
                var pen = new Vector2();

                for (int i = 0; i < text.Length; i++)
                {
                    #region Load character
                    char c = text[i];
                    switch (c)
                    {
                        case ' ':
                            pen.X += advanceSpace;
                            continue;
                        case '\t':
                            pen.X += advanceSpace * TabSpaces;
                            continue;
                        case '\r':  //  ignore
                            continue;
                        case '\n':
                            firstInLine = true;
                            underrun = overrun = 0;

                            pen.X = 0;
                            pen.Y += LineHeight;

                            maxWidth = Math.Max(maxWidth, currLineWidth);
                            maxHeight = Math.Max(maxHeight, currLineHeight);
                            continue;
                    }

                    if (!TryGetGlyph(c, out var glyph))
                    {
                        if (_defaultGlyph == null)
                            throw new Exception($"Invalid character '{c}'");

                        glyph = _defaultGlyph.Value;
                    }

                    float gAdvanceX = glyph.Advance;
                    float gBearingX = glyph.BearingX;
                    float gWidth = glyph.Width;

                    #endregion

                    #region Underrun
                    // Negative bearing would cause clipping of the first character
                    // at the left boundary, if not accounted for.
                    // A positive bearing would cause empty space.
                    underrun += -(gBearingX);
                    if (pen.X == 0)
                        pen.X += underrun;

                    if (firstInLine && underrun <= 0)
                    {
                        underrun = 0;
                        firstInLine = false;
                    }
                    #endregion

                    var texture = GetTexturePageForGlyph(glyph, out var bounds);

                    int x = (int)Math.Round(pen.X + glyph.BitmapLeft);
                    int y = (int)Math.Round(pen.Y + ascender - glyph.BearingY);

                    result.Add(new Glyph(texture, bounds, new Vector2(x, y)));
                    set.Add(texture);

                    currLineWidth = x + glyph.Width;
                    currLineHeight = Math.Max(currLineHeight, y + glyph.Height);

                    #region Overrun
                    // Accumulate overrun, which could cause clipping at the right side of characters near
                    // the end of the string (typically affects fonts with slanted characters)
                    // Overrun will be added at the end of the line
                    if (gBearingX + gWidth > 0 || gAdvanceX > 0)
                    {
                        overrun -= Math.Max(gBearingX + gWidth, gAdvanceX);
                        if (overrun < 0) overrun = 0;
                    }

                    overrun += (gBearingX == 0 && gWidth == 0 ? 0 : gBearingX + gWidth - gAdvanceX);

                    #endregion

                    pen.X += gAdvanceX;

                    #region Kerning (for next character)
                    // Calculate kern for the NEXT character (if any)
                    // The kern value adjusts the origin of the next character (positive or negative).
                    if (i < text.Length - 1)
                    {
                        char cNext = text[i + 1];
                        if (TryGetGlyph(cNext, out var next))
                        {
                            int kern = GetKerning(glyph, next);
                            // sanity check for some fonts that have kern way out of whack
                            if (kern > gAdvanceX * 5 || kern < -(gAdvanceX * 5))
                                kern = 0;

                            pen.X += kern;
                        }
                    }

                    #endregion
                }
                
                maxWidth = Math.Max(maxWidth, currLineWidth);
                maxHeight = Math.Max(maxHeight, currLineHeight);

                if (set.Count > 1)
                    result.Sort((a, b) => a.Texture != b.Texture ? 1 : 0);
            }

            return new Text(text, result, maxWidth, maxHeight);
        }
        
        
        
        
        
        
        private Texture2D GetTexturePageForGlyph(GlyphInfo glyph, out Rectangle bounds)
        {
            if (!texturesByChar.TryGetValue(glyph.Character, out var texture))
            {
                var page = FindPage(glyph.Width, glyph.Height, out var packX, out var packY);
                page.RenderGlyph(glyph.Width, glyph.Height, glyph.BufferData, packX, packY);

                boundsByChar[glyph.Character] = bounds = new Rectangle(packX, packY, glyph.Width, glyph.Height);
                return texturesByChar[glyph.Character] = texture = page.Texture;
            }

            bounds = boundsByChar[glyph.Character];
            return texture;
        }

        private GlyphPage FindPage(int width, int height, out int packX, out int packY)
        {
            foreach (var page in pages)
            {
                if (page.Pack(width, height, out var rect))
                {
                    packX = rect.X;
                    packY = rect.Y;
                    return page;
                }
            }

            //  none found, make a new page
            {
                var result = new GlyphPage(graphicsDevice, TextureSize);
                pages.Add(result);
                if (!result.Pack(width, height, out var rect))
                    throw new Exception("Failed to pack item even in new page");

                packX = rect.X;
                packY = rect.Y;
                return result;
            }
        }
        
        
        private bool TryGetGlyph(char c, out GlyphInfo glyph)
        {
            if (glyphs.TryGetValue(c, out glyph))
                return true;
                
            var glyphIndex = face.GetCharIndex(c);
            if (glyphIndex == 0)
                return false;
                
            byte[] bufferData;
            face.LoadGlyph(glyphIndex, LoadFlags.Default, LoadTarget.Normal);
            if (face.Glyph.Metrics.Width == 0)
            {
                bufferData = new byte[0];
            }
            else
            {
                face.Glyph.RenderGlyph(RenderMode.Normal);
                bufferData = face.Glyph.Bitmap.BufferData;
            }
                    
            glyphs[c] = glyph = new GlyphInfo(c, glyphIndex, face.Glyph.BitmapLeft, face.Glyph.Metrics, bufferData);
            return true;
        }
        
        
        private int GetKerning(GlyphInfo l, GlyphInfo r)
        {
            if (!face.HasKerning)
                return 0;

            if (!kerning.TryGetValue(l.Character, out var subKerning))
                kerning[l.Character] = subKerning = new Dictionary<char, int>();

            if (!subKerning.TryGetValue(r.Character, out var kern))
                subKerning[r.Character] = kern = (int) face.GetKerning(l.Index, r.Index, KerningMode.Default).X;

            return kern;
        }

        public void Dispose()
        {
            foreach (var page in pages)
                page.Dispose();
        }
        
    }
}