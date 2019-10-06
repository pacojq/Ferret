using Microsoft.Xna.Framework;

namespace FerretEngine.Graphics
{
    /// <summary>
    /// TODO
    /// </summary>
    public class Camera
    {
        public Vector2 Position { get; set; }
        public Vector2 Center { get; set; }
        
        public int Width { get; set; }

        public int Height { get; set; }
        
        public bool Active { get; set; }


        public Camera(Vector2 position, int width, int height)
        {
            Active = true;
            Position = position;
            Center = new Vector2(width/2, height/2);
            Width = width;
            Height = height;
        }
        
        public Camera(int width, int height) : this(Vector2.Zero, width, height) { }


    }
}