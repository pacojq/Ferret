using System;
using FerretEngine.Core;
using FerretEngine.Graphics;

namespace FerretEngine.Components.Graphics
{
    public class AnimationComponent : Component
    {
        public SpriteRenderer Renderer { get; set; }
        public AnimationController Controller { get; set; }

        public Action OnAnimationUpdate;
        
        
        public AnimationComponent(SpriteRenderer renderer, AnimationController controller)
        {
            Renderer = renderer;
            Controller = controller;
        }


        public override void Update(float dt)
        {
            Controller.Update(dt);
            UpdateSprite(Controller.CurrentAnimation[Controller.ImageIndex]);
        }

        private void UpdateSprite(Sprite frame)
        {
            if (Renderer.Sprite == frame)
                return;

            Renderer.Sprite = frame;
            if (OnAnimationUpdate != null)
                OnAnimationUpdate();
        }
    }
}