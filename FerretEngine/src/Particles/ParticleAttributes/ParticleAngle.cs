namespace FerretEngine.Particles.ParticleAttributes
{
    public class ParticleAngle
    {
        public static ParticleAngle Fixed(float lifetime)
        {
            return new ParticleAngle(lifetime, lifetime);
        }

        public static ParticleAngle RandomRange(float min, float max)
        {
            return new ParticleAngle(min, max);
        }
        
        
        internal float Value => _min + (float) FeGame.Random.NextDouble() * (_max - _min);
        
        private readonly float _min;
        private readonly float _max;

        private ParticleAngle(float min, float max)
        {
            _min = min;
            _max = max;
        }
    }
}