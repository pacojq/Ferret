using FerretEngine.Utils;

namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleDirection
    {
        public static ParticleDirection Fixed(float lifetime)
        {
            return new ParticleDirection(lifetime, lifetime);
        }

        public static ParticleDirection RandomRange(float min, float max)
        {
            return new ParticleDirection(min, max);
        }
        
        
        internal float Value => FeRandom.Range(_min, _max);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleDirection(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}