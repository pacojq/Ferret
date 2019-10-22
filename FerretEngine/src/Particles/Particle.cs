using Microsoft.Xna.Framework;

namespace FerretEngine.Particles
{
    internal struct Particle
    {
        public bool Active;
        public float LifeTime;
        
        public Color Color;
        
        public Vector2 Position;
        public Vector2 Speed;
        
        public float Size;

        public float Rotation;
    }
}