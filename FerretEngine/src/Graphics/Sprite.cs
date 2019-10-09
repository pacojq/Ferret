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
        
        
        public Sprite(Texture2D texture, Rectangle clipRect)
        {
            Texture = texture;
            ClipRect = clipRect;
            DrawOffset = Vector2.Zero;
            Width = ClipRect.Width;
            Height = ClipRect.Height;
        }
        
        public Sprite(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height))
        {
        }
        
        
        
        
        
        
        public static Sprite PlainColor(int width, int height, Color color)
        {
            GraphicsDevice gd = FeGame.Instance.Graphics.GraphicsDevice;
            
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                pixels[i] = color;
            
            Texture2D tex = new Texture2D(gd, width, height);
            tex.SetData(pixels);

            return new Sprite(tex);
        }
    }
}