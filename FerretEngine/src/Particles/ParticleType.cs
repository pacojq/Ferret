using FerretEngine.Graphics;
using FerretEngine.Particles.ParticleAttributes;
using Microsoft.Xna.Framework;

namespace FerretEngine.Particles
{
    public class ParticleType
    {
        
        public Sprite Sprite { get; set; }
        
        
        /// <summary>
        /// The lifetime, in seconds, of the particle.
        /// By default, <see cref="ParticleLifetime.Fixed"/>, with
        /// one second.
        /// </summary>
        public ParticleLifetime Lifetime { get; set; }
        
        /// <summary>
        /// The color of the particle when it's created.
        /// By default, white.
        /// </summary>
        public Color StartColor { get; set; }
        
        /// <summary>
        /// Speed per-frame of a particle.
        /// By default, <see cref="ParticleSpeed.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleSpeed StartSpeed { get; set; }
        
        /// <summary>
        /// Increase, per-frame, of a particle speed.
        /// By default, <see cref="ParticleAcceleration.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleAcceleration Acceleration { get; set; }
        
        /// <summary>
        /// The scale of the particle when it's created.
        /// By default, <see cref="ParticleSize.Fixed"/>, with
        /// a value of one.
        /// </summary>
        public ParticleSize StartSize { get; set; }
        
        /// <summary>
        /// Increase, per-frame, of a particle size.
        /// By default, <see cref="ParticleGrowth.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleGrowth Growth { get; set; }

        /// <summary>
        /// The rotation, in degrees, of a particle when it's created.
        /// By default, <see cref="ParticleAngle.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleAngle StartAngle { get; set; }
        
        /// <summary>
        /// Increase, per-frame, of a particle angle.
        /// By default, <see cref="ParticleRotation.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleRotation Rotation { get; set; }

        /// <summary>
        /// The direction, in degrees, a particle if facing when it's created.
        /// By default, <see cref="ParticleDirection.Fixed"/>, with
        /// a value of zero.
        /// </summary>
        public ParticleDirection StartDirection { get; set; }

        
        
        public ParticleType(Sprite sprite)
        {
            Sprite = sprite;
            StartColor = Color.White;
            
            Lifetime = ParticleLifetime.Fixed(1f);
            
            StartSpeed = ParticleSpeed.Fixed(0f); 
            Acceleration = ParticleAcceleration.Fixed(0f);

            StartSize = ParticleSize.Fixed(1f);
            Growth = ParticleGrowth.Fixed(0f);

            StartAngle = ParticleAngle.Fixed(0f);
            Rotation = ParticleRotation.Fixed(0f);
            
            StartDirection = ParticleDirection.Fixed(0f);
        }
        
        

        internal Particle CreateParticle(Particle part)
        {
            part.LifeTime = Lifetime.Value;
            
            part.Color = StartColor;
            
            part.Speed = StartSpeed.Value;
            part.Acceleration = Acceleration.Value;
            
            part.Size = StartSize.Value;
            part.Growth = Growth.Value;
            
            part.Angle = StartAngle.Value;
            part.Rotation = Rotation.Value;

            part.Direction = StartDirection.Value;

            return part;
        }
    }
}