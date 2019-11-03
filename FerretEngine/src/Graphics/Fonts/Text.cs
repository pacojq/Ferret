using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Fonts
{
    internal class Text
    {
        public float Width { get; }

        /// <summary>Height of the given string</summary>
        public float Height { get; }
            
        /// <summary>The string that produced this text object</summary>
        public string String { get; }

        
        private readonly List<Glyph> _glyphs;
        
        
        public Text(string text, List<Glyph> glyphs, float width, float height)
        {
            String = text;
            _glyphs = glyphs;
            Width = width;
            Height = height;
        }
        
        
        /// <summary>
        /// Render this text with the given SpriteBatch
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to render to</param>
        /// <param name="position">Position at which to render the text</param>
        /// <param name="color">Color to render the text with</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            foreach (var glyph in _glyphs)
                spriteBatch.Draw(glyph.Texture, glyph.Position + position, glyph.Bounds, color);
        }
    }
}