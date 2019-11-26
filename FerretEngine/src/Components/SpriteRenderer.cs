using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Components
{
    public class SpriteRenderer : Component
    {
        public Sprite Sprite
        {
            get => _sprite;
            set
            {
                _sprite = value;
                if (_sprite != null)
                    ClipRect = new Rectangle(0, 0, _sprite.Width, _sprite.Height);
                else ClipRect = new Rectangle();
            }
        }
        private Sprite _sprite;
        
        
        public Color BlendColor { get; set; }
        
        public float Alpha { get; set; }

        public Rectangle ClipRect { get; set; }

        public float Rotation { get; set; }

        public Vector2 Scale { get; set; }

        public SpriteEffects Flip { get; set; }
        
        
        public Material Material { get; set; }

        public SpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            Alpha = 1f;
            BlendColor = Color.White;
            Scale = Vector2.One;
            Rotation = 0;
            Flip = SpriteEffects.None;
            Material = Material.Default;
        }


        public override void Draw(float deltaTime)
        {
            if (Sprite == null)
                return;

            FeDraw.SetMaterial(Material);
            FeDraw.SpriteExt(Sprite, Position, new Color(BlendColor, Alpha), Rotation, Scale, Flip, 0);
        }
    }
}