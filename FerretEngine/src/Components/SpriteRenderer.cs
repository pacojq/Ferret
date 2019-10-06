﻿using FerretEngine.Core;
using FerretEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Components
{
    public class SpriteRenderer : Component
    {
        public Sprite Sprite { get; set; }

        public Rectangle ClipRect { get; set; }

        public float Rotation { get; set; }

        public Vector2 Scale { get; set; }

        public SpriteEffects Flip { get; set; }
        
        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            ClipRect = new Rectangle(0, 0, sprite.Width, sprite.Height);
            Scale = Vector2.One;
            Rotation = 0;
            Flip = SpriteEffects.None;
        }


        public override void Draw(float deltaTime)
        {
            if (Sprite == null)
                return;

            FeDraw.SpriteExt(Sprite, Position, Color.White, Rotation, Vector2.Zero, Scale, Flip, 0);
        }
    }
}