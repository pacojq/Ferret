namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleRotation
    {
        public static ParticleRotation Fixed(float lifetime)
        {
            return new ParticleRotation(lifetime, lifetime);
        }

        public static ParticleRotation RandomRange(float min, float max)
        {
            return new ParticleRotation(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleRotation(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}