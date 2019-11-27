using System;
using System.Collections.Generic;

namespace FerretEngine.Graphics
{
    public delegate void AnimationEvent(Animation anim);
    
    
    public class AnimationController
    {
        public Animation CurrentAnimation { get; set; }

        private readonly IDictionary<string, Animation> _animations;

        
        public int ImageIndex { get; private set; }
        private float _actualImageIndex;

        
        public AnimationEvent OnAnimationEnd { get; set; }
        
        private Action _SetAnimation_OnAnimationEnd;

        public AnimationController(params Animation[] animations)
        {
            // TODO check size > 0
            
            CurrentAnimation = animations[0];
            
            _animations = new Dictionary<string, Animation>();
            foreach (var anim in animations)
                AddAnimation(anim);
        }


        internal void Update(float dt)
        {
            SetFrame(_actualImageIndex + CurrentAnimation.Speed);
        }


        public void SetAnimation(string name)
        {
            SetAnimation(name, null);
        }

        public void AddAnimation(Animation anim)
        {
            _animations.Add(anim.Name, anim);
        }
        
        
        public void SetAnimation(string name, Action onAnimationEnd)
        {
            if (!_animations.ContainsKey(name))
                throw new Exception($"Animation '{name}' does not exist in the controller.");

            if (name == CurrentAnimation.Name)
                return;
            
            CurrentAnimation = _animations[name];
            _SetAnimation_OnAnimationEnd = onAnimationEnd;
        }


        public void SetFrame(float index)
        {
            if (index < 0)
                index = 0;
            
            _actualImageIndex = index;
            
            if (index >= CurrentAnimation.FrameCount)
            {
                if (OnAnimationEnd != null)
                    OnAnimationEnd(CurrentAnimation);
                
                if (_SetAnimation_OnAnimationEnd != null)
                    _SetAnimation_OnAnimationEnd();
            }
            
            _actualImageIndex %= CurrentAnimation.FrameCount;
            ImageIndex = (int) Math.Floor(_actualImageIndex);
        }
    }
}