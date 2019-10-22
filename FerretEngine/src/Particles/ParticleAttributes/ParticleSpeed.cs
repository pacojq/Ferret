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
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleSpeed(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}