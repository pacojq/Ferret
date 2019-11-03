using System;

namespace FerretEngine.Graphics
{
    public class TileMap
    {
        public int Width { get; }
        public int Height { get; }

        public SpriteSheet SpriteSheet { get; }
        private int[] _indices;
        
        public TileMap(SpriteSheet sprites, int width, int height, int[] indices)
        {
            if (width * height != indices.Length)
                throw new ArgumentException($"Failed creating TileMap: invalid sizes. Width * Height = {width * height}. Indices length = {indices.Length}");

            Width = width;
            Height = height;
            _indices = indices;
            SpriteSheet = sprites;
        }


        public Sprite Evaluate(int x, int y)
        {
            int sprite = _indices[y * Width + x];
            return SpriteSheet.GetSprite(sprite);
        }
        
    }
}