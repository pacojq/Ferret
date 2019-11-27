using FerretEngine.Utils;

namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleSpeed
    {
        public static ParticleSpeed Fixed(float lifetime)
        {
            return new ParticleSpeed(lifetime, lifetime);
        }

        public static ParticleSpeed RandomRange(float min, float max)
        {
            return new ParticleSpeed(min, max);
        }
        
        
        internal float Value => FeRandom.Range(_min, _max);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleSpeed(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}