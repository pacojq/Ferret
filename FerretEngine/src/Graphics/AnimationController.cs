using System.Collections.Generic;

namespace FerretEngine.Graphics
{
    public class AnimationController
    {
        public Animation CurrentAnimation { get; set; }

        private readonly IDictionary<string, Animation> _animations;


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


        public void SetAnimation(string name)
        {
            CurrentAnimation = _animations[name];
        }
    }
}