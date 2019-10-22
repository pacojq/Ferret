namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleGrowth
    {
        public static ParticleGrowth Fixed(float lifetime)
        {
            return new ParticleGrowth(lifetime, lifetime);
        }

        public static ParticleGrowth RandomRange(float min, float max)
        {
            return new ParticleGrowth(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleGrowth(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}