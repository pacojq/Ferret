using System;
using System.Collections.Generic;

namespace FerretEngine.Graphics
{
    public class AnimationController
    {
        public Animation CurrentAnimation { get; set; }

        private readonly IDictionary<string, Animation> _animations;

        
        public int ImageIndex { get; private set; }
        private float _actualImageIndex;

        public AnimationController(params Animation[] animations)
        {
            // TODO check size > 0
            
            CurrentAnimation = animations[0];
            
            _animations = new Dictionary<string, Animation>();
            foreach (var anim in animations)
            {
                _animations.Add(anim.Name, anim);
            }
        }


        internal void Update(float dt)
        {
            SetFrame(_actualImageIndex + CurrentAnimation.Speed);
        }


        public void SetAnimation(string name)
        {
            CurrentAnimation = _animations[name];
        }


        public void SetFrame(float index)
        {
            if (index < 0)
                index = 0;
            
            _actualImageIndex = index;
            _actualImageIndex %= CurrentAnimation.FrameCount;
            ImageIndex = (int) Math.Floor(_actualImageIndex);
        }
    }
}