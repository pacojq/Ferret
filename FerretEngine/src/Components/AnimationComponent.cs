using System;
using FerretEngine.Core;
using FerretEngine.Graphics;

namespace FerretEngine.Components
{
    public class AnimationComponent : Component
    {
        public SpriteRenderer Renderer { get; set; }
        public AnimationController Controller { get; set; }
        public Animation CurrentAnimation { get; set; }
        

        public Action OnAnimationUpdate;
        
        
        
        public AnimationComponent(SpriteRenderer renderer, AnimationController controller)
        {
            Renderer = renderer;
            Controller = controller;
            CurrentAnimation = controller.CurrentAnimation;
        }


        public override void Update(float dt)
        {
            Controller.Update(dt);
            UpdateSprite(CurrentAnimation[Controller.ImageIndex]);
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