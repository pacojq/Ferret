using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Graphics
{
    /// <summary>
    /// Wrapper class for a <see cref="Microsoft.Xna.Framework.Graphics.Texture2D"/>
    /// </summary>
    public class Sprite
    {
        public Texture2D Texture { get; }
        public Rectangle ClipRect { get; }
            
        public Vector2 DrawOffset { get; set; }
        public int Width { get; }
        public int Height { get; }
        
        
        
        public Sprite(Texture2D texture)
        {
            Texture = texture;
            ClipRect = new Rectangle(0, 0, texture.Width, texture.Height);
            DrawOffset = Vector2.Zero;
            Width = ClipRect.Width;
            Height = ClipRect.Height;
        }
        
        
        public Sprite(int width, int height, Color color)
        {
            GraphicsDevice gd = FeGame.Instance.Graphics.GraphicsDevice;
            Texture = new Texture2D(gd, width, height);
            
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                pixels[i] = color;
            Texture.SetData(pixels);
            
            ClipRect = new Rectangle(0, 0, width, height);
            DrawOffset = Vector2.Zero;
            Width = width;
            Height = height;
        }
    }
}