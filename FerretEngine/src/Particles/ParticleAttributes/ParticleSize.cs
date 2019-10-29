namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleSize
    {
        public static ParticleSize Fixed(float lifetime)
        {
            return new ParticleSize(lifetime, lifetime);
        }

        public static ParticleSize RandomRange(float min, float max)
        {
            return new ParticleSize(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleSize(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}