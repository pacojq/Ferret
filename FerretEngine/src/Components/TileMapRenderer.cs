using FerretEngine.Core;
using FerretEngine.Graphics;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components
{
    public class TileMapRenderer : Component
    {
        private TileMap _tileMap;
        
        public TileMapRenderer(TileMap tileMap)
        {
            _tileMap = tileMap;
        }
        
        public override void Draw(float deltaTime)
        {
            if (_tileMap == null)
                return;

            FeDraw.SetMaterial(Material.Default);
            
            for (int i = 0; i < _tileMap.Width; i++)
            {
                Vector2 pos = Position + new Vector2(i * _tileMap.SpriteSheet.SpriteWidth,0);
                
                for (int j = 0; j < _tileMap.Height; j++)
                {
                    FeDraw.Sprite(_tileMap.Evaluate(i, j), pos);
                    pos.Y += _tileMap.SpriteSheet.SpriteHeight;
                }
            }
        }
    }
}