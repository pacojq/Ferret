using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics.Fonts
{
    internal class Glyph
    {
        public Texture2D Texture { get; }
        public Rectangle Bounds { get; }
        public Vector2 Position { get; }
        
        public Glyph(Texture2D texture, Rectangle bounds, Vector2 position)
        {
            Texture = texture;
            Bounds = bounds;
            Position = position;
        }
    }
}