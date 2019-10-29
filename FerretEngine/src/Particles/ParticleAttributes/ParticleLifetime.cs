namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleLifetime
    {
        public static ParticleLifetime Fixed(float lifetime)
        {
            return new ParticleLifetime(lifetime, lifetime);
        }

        public static ParticleLifetime RandomRange(float min, float max)
        {
            return new ParticleLifetime(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleLifetime(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}