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


        public Camera(int width, int height, Vector2 center)
        {
            Active = true;
            Position = Vector2.Zero;
            Center = center;
            Width = width;
            Height = height;
        }

        public Camera(int width, int height) : this(width, height, new Vector2(width / 2, height / 2))
        {
        }


        /// <summary>
        /// Transforms a world position to its relative position in camera.
        /// </summary>
        /// <param name="worldPos"></param>
        /// <returns></returns>
        public Vector2 GetRelativePosition(Vector2 worldPos)
        {
            return worldPos - this.Position + this.Center;
        }
    }
}