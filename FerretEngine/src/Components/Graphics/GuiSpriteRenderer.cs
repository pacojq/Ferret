using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Components.Graphics
{
    public class GuiSpriteRenderer : Component
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

        public float LocalRotation { get; set; }

        public float Rotation
        {
            get
            {
                Assert.IsNotNull(Entity, "Cannot calculate a SpriteRenderer Rotation when its Entity is null.");
                if (Entity != null)
                    return Entity.Rotation + LocalRotation;
                return LocalRotation;
            }
        }

        public Vector2 Scale { get; set; }

        public SpriteEffects Flip { get; set; }
        
        
        public Material Material { get; set; }

        public GuiSpriteRenderer(Sprite sprite)
        {
            Sprite = sprite;
            Alpha = 1f;
            BlendColor = Color.White;
            Scale = Vector2.One;
            LocalRotation = 0;
            Flip = SpriteEffects.None;
            Material = Material.Default;
        }


        public override void DrawGUI(float deltaTime)
        {
            if (Sprite == null)
                return;

            Vector2 pos = Position - Entity.Scene.MainCamera.Position;
            
            FeDraw.SetMaterial(Material);
            FeDraw.SpriteExt(Sprite, pos, new Color(BlendColor, Alpha), Rotation, Scale, Flip, 0);
            FeDraw.SetMaterial(Material.Default);
        }
    }
}