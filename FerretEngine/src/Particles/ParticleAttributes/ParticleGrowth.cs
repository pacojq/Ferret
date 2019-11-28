using FerretEngine.Utils;

namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleGrowth
    {
        public static ParticleGrowth Fixed(float growth)
        {
            return new ParticleGrowth(growth, growth);
        }

        public static ParticleGrowth RandomRange(float min, float max)
        {
            return new ParticleGrowth(min, max);
        }
        
        
        internal float Value => FeRandom.Range(_min, _max);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleGrowth(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}