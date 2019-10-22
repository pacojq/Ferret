using System;
using FerretEngine.Core;

namespace FerretEngine.Particles
{
    public sealed class ParticleEmitter : Component
    {
        public int MaxParticles
        {
            get => _system.MaxParticles;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Max particle count must be grater than zero.");
                _system.MaxParticles = value;
            }
        }

        public ParticleType ParticleType
        {
            get => _system.ParticleType;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _system.ParticleType = value;   
            }
        }
        
        /// <summary>
        /// Whether the Emitter uses <see cref="EmitDelay"/>
        /// to auto-emit particle bursts.
        /// </summary>
        public bool AutoEmit { get; set; }
        
        /// <summary>
        /// Time in seconds between emissions.
        /// </summary>
        public float EmitDelay { get; set; }
        
        /// <summary>
        /// Time in seconds between each emitted particle in a single
        /// burst.
        /// </summary>
        public float Interval { get; set; }
        
        
        
        
        
        private readonly ParticleSystem _system;

        private float _emitCoundown;
        
        
        
        public ParticleEmitter(ParticleType particleType, int maxParticles)
        {
            _system = new ParticleSystem(particleType, maxParticles);
        }

        public ParticleEmitter(ParticleType particleType)
            : this(particleType, 128)
        {
        }


        
        /// <summary>
        /// Emits a single burst of particles.
        /// If this method is called, it's recommendable to set
        /// <see cref="AutoEmit"/> to false.
        /// </summary>
        public void Emit()
        {
            
        }
        
        

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _system.Update(deltaTime);

            if (AutoEmit)
            {
                this.Emit();
            }
        }
        
        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
            _system.Draw(deltaTime);
        }
    }
}