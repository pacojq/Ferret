using Microsoft.Xna.Framework;

namespace FerretEngine.Particles
{
    public class ParticleType
    {
        public float LifeTime { get; set; }
        
        public Color StartColor { get; set; }
        
        public Vector2 StartSpeed { get; set; }
        public Vector2 Acceleration { get; set; }
        
        public float StartSize { get; set; }
        public float Growth { get; set; }

        public float StartAngle { get; set; }
        public float Rotation { get; set; }
    }
}