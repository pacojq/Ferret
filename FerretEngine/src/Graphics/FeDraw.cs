using System;
using System.Runtime.CompilerServices;
using FerretEngine.Graphics.Effects;
using FerretEngine.Graphics.Fonts;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    public static class FeDraw
    {
        /// <summary>
        /// The types of horizontal alignment a text can have. 
        /// </summary>
        public enum HAlign { Left, Centre, Right }
        
        /// <summary>
        /// The types of vertical alignment a text can have. 
        /// </summary>
        public enum VAlign { Top, Centre, Bottom }
        
        
        
        
        public static Font Font
        {
            get => _font;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _font = value;
            }
        }
        private static Font _font;

        public static Color Color
        {
            get => _color;
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                _color = value;
            }
        }
        private static Color _color;


        private static HAlign _hAlign;
        private static VAlign _vAlign;
        
        
        
        
        
        
        
        public static void SetMaterial(Material material)
        {
            FeGraphics.SetMaterial(material);
        }


        public static void SetHAlign(HAlign align)
        {
            _hAlign = align;
        }

        public static void SetVAlign(VAlign align)
        {
            _vAlign = align;
        }
        
        


        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sprite(Sprite sprite, Vector2 position)
        {
            Assert.IsTrue(FeGraphics.IsRendering);
            if (!WillRender(sprite, position, Vector2.One))
                return;
            
            FeGraphics.BindMaterial();
            
            FeGraphics.SpriteBatch.Draw(
                    sprite.Texture,
                    position - sprite.Origin,
                    sprite.ClipRect,
                    Color.White
                );
        }
        
        /// <summary>
        /// Draw a sprite on screen.
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        /// <param name="rotation"></param>
        /// <param name="origin"></param>
        /// <param name="scale"></param>
        /// <param name="effects"></param>
        /// <param name="layerDepth"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SpriteExt(Sprite sprite, Vector2 position, Color color, float rotation,
                Vector2 scale, SpriteEffects effects, float layerDepth)
        {
            Assert.IsTrue(FeGraphics.IsRendering);
            if (!WillRender(sprite, position, scale))
                return;

            FeGraphics.BindMaterial();
            
            FeGraphics.SpriteBatch.Draw(
                    sprite.Texture,
                    position,
                    sprite.ClipRect,
                    color,
                    FeMath.DegToRad(rotation),
                    sprite.Origin,
                    scale,
                    effects,
                    layerDepth
                );
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool WillRender(Sprite sprite, Vector2 worldPos, Vector2 scale)
        {
            return true;
            
            /* TODO culling
            return FeGraphics.CurrentRenderer.Camera
                .GetRenderRect()
                .Intersects(
                    new Rectangle(
                        (int) worldPos.X, 
                        (int) worldPos.Y,
                        (int) (sprite.Width * scale.X), 
                        (int) (sprite.Height * scale.Y))
                 );
            */
        }
        
        
        
        
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Line(float x0, float y0, float x1, float y1, int width = 1)
        {
            Line(new Vector2(x0, y0), new Vector2(x1, y1), width);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Line(Vector2 p0, Vector2 p1, int width = 1)
        {
            Assert.IsTrue(FeGraphics.IsRendering);
            
            // Draw a scaled pixel
            SpriteExt(
                    FeGraphics.Pixel, 
                    FeMath.Floor(p0),
                    _color,
                    FeMath.Direction(p0, p1),
                    new Vector2(FeMath.Distance(p0, p1), width), 
                    SpriteEffects.None, 
                    0
                );
        }

        
        
        
        
        
        
        public static void Rect(float x, float y, float width, float height, bool outline, int borderWidth = 1)
        {
            Rectangle rect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            RectExt(rect, Color, outline, borderWidth);
        }
        
        public static void Rect(Rectangle rect, bool outline, int borderWidth = 1)
        {
            RectExt(rect, Color, outline, borderWidth);
        }
        
        public static void RectExt(float x, float y, float width, float height, Color color, bool outline, int borderWidth = 1)
        {
            Rectangle rect = new Rectangle((int)x, (int)y, (int)width, (int)height);
            RectExt(rect, color, outline, borderWidth);
        }
        
        public static void RectExt(Rectangle rect, Color color, bool outline, int borderWidth = 1)
        {
            Assert.IsTrue(FeGraphics.IsRendering);
            if (outline) RectOutline(rect, color, borderWidth);
            else RectFilled(rect, color);
        }

        
        private static void RectFilled(Rectangle rect, Color color)
        {
            Sprite px = FeGraphics.Pixel;
            
            Vector2 rel = FeMath.Floor(new Vector2(rect.X, rect.Y));
            rect.X = (int) rel.X;
            rect.Y = (int) rel.Y;
            
            FeGraphics.SpriteBatch.Draw(px.Texture, rect, px.ClipRect, color);
        }
        
        private static void RectOutline(Rectangle rect, Color color, int border)
        {
            Sprite px = FeGraphics.Pixel;
            Vector2 rel = FeMath.Floor(new Vector2(rect.X, rect.Y));

            int xx = FeMath.FloorToInt(rel.X - border / 2f);
            int yy = FeMath.FloorToInt(rel.Y - border / 2f);
            
            var top = new Rectangle(xx, yy, rect.Width + border, border);
            var bot = new Rectangle(xx, yy + rect.Height, rect.Width + border, border);
            var right = new Rectangle(xx + rect.Width, yy, border, rect.Height + border);
            var left = new Rectangle(xx, yy, border, rect.Height + border);
            
            FeGraphics.SpriteBatch.Draw(px.Texture, top, px.ClipRect, color);
            FeGraphics.SpriteBatch.Draw(px.Texture, bot, px.ClipRect, color);
            FeGraphics.SpriteBatch.Draw(px.Texture, right, px.ClipRect, color);
            FeGraphics.SpriteBatch.Draw(px.Texture, left, px.ClipRect, color);
        }
        
        
        
        
        
        
        
        
        /// <summary>
        /// Draws a string and returns its width and height.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Text(string text, Vector2 position)
        {
            return TextExt(text, position, Color);
        }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 TextExt(string text, Vector2 position, Color color)
        {
            Assert.IsTrue(FeGraphics.IsRendering);

            Text tx = Font.MakeText(text);
            
            Vector2 offset = Vector2.Zero;
            
            if (_hAlign == HAlign.Centre)
                offset.X = -tx.Width / 2f;
            else if (_hAlign == HAlign.Right)
                offset.X = -tx.Width;
            
            if (_vAlign == VAlign.Centre)
                offset.Y = -tx.Height / 2f;
            else if (_vAlign == VAlign.Bottom)
                offset.Y = -tx.Height;

            //Vector2 pos = position + offset;
            // TODO split \n and draw different texts
            
            tx.Draw(FeGraphics.SpriteBatch, position + offset, color);
            
            return new Vector2(tx.Width, tx.Height);
        }
        
    }
}