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
        
        
        public int ImageIndex { get; private set; }
        private float _actualImageIndex;

        public Action OnAnimationUpdate;
        
        
        
        public AnimationComponent(SpriteRenderer renderer, AnimationController controller)
        {
            Renderer = renderer;
            Controller = controller;
            CurrentAnimation = controller.CurrentAnimation;
            
            ImageIndex = 0;
            _actualImageIndex = 0;
        }


        public override void Update(float dt)
        {
            _actualImageIndex += CurrentAnimation.Speed;
            _actualImageIndex %= CurrentAnimation.FrameCount;
            ImageIndex = (int) Math.Floor(_actualImageIndex);

            UpdateSprite(CurrentAnimation[ImageIndex]);
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