using Microsoft.Xna.Framework;

namespace FerretEngine.Particles
{
    internal struct Particle
    {
        public bool Active;
        public float LifeTime;
        
        public Color Color;
        
        public Vector2 Position;
        
        public float Speed;
        public float Acceleration;
        
        public float Direction;
        
        public float Size;
        public float Growth;

        public float Angle;
        public float Rotation;
    }
}