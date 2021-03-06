﻿using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using Microsoft.Xna.Framework;

namespace FerretEngine.Components.Graphics
{
    public class TileMapRenderer : Component
    {
        private TileMap _tileMap;
        
        public Material Material { get; set; }
        
        public TileMapRenderer(TileMap tileMap)
        {
            _tileMap = tileMap;
            Material = Material.Default;
        }
        
        public override void Draw(float deltaTime)
        {
            if (_tileMap == null)
                return;

            FeDraw.SetMaterial(Material);
            
            for (int i = 0; i < _tileMap.Width; i++)
            {
                Vector2 pos = Position + new Vector2(i * _tileMap.SpriteSheet.SpriteWidth,0);
                
                for (int j = 0; j < _tileMap.Height; j++)
                {
                    FeDraw.Sprite(_tileMap.Evaluate(i, j), pos);
                    pos.Y += _tileMap.SpriteSheet.SpriteHeight;
                }
            }
            
            FeDraw.SetMaterial(Material.Default);
        }
    }
}