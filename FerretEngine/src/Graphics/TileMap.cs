using System;

namespace FerretEngine.Graphics
{
    public class TileMap
    {
        
        public TileMap(SpriteSheet sprites, int width, int height, int[] indices)
        {
            if (width * height != indices.Length)
                throw new ArgumentException($"Failed creating TileMap: invalid sizes. Width * Height = {width * height}. Indices length = {indices.Length}");
            
            // TODO
        }
    }
}