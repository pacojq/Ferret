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
            
        public Vector2 Origin { get; set; }
        public int Width { get; }
        public int Height { get; }
        
        
        public Sprite(Texture2D texture, Rectangle clipRect, Vector2 origin)
        {
            Texture = texture;
            ClipRect = clipRect;
            Origin = origin;
            Width = ClipRect.Width;
            Height = ClipRect.Height;
        }
        
        public Sprite(Texture2D texture) 
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height))
        {
        }
        
        public Sprite(Texture2D texture, Rectangle clipRect) 
            : this(texture, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero)
        {
        }
        
        
        
        
        
        
        public static Sprite PlainColor(int width, int height, Color color)
        {
            GraphicsDevice gd = FeGraphics.GraphicsDevice;
            
            Color[] pixels = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                pixels[i] = color;
            
            Texture2D tex = new Texture2D(gd, width, height);
            tex.SetData(pixels);

            return new Sprite(tex);
        }
    }
}