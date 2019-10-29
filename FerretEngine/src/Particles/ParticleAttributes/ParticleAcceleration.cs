namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleAcceleration
    {
        public static ParticleAcceleration Fixed(float lifetime)
        {
            return new ParticleAcceleration(lifetime, lifetime);
        }

        public static ParticleAcceleration RandomRange(float min, float max)
        {
            return new ParticleAcceleration(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleAcceleration(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}