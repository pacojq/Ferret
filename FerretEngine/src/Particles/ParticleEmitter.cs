using System;
using System.Collections;
using System.Collections.Generic;
using FerretEngine.Core;
using FerretEngine.Logging;

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
        /// Time in seconds between each step in a burst.
        /// </summary>
        public float BurstStepDelay { get; set; }
        
        /// <summary>
        /// The number of emissions in a single burst.
        /// </summary>
        public int BurstSteps { get; set; }
        
        /// <summary>
        /// The number of emitted particles in a burst emission.
        /// </summary>
        public int BurstCunt { get; set; }
        
        
        
        
        
        private readonly ParticleSystem _system;

        private float _emitCountdown;
        
        
        private IEnumerator _coroutine;
        private float _coroutineDelay;
        
        
        public ParticleEmitter(ParticleType particleType, int maxParticles)
        {
            _system = new ParticleSystem(particleType, maxParticles);
        }

        /// <summary>
        /// Creates a <see cref="ParticleEmitter"/> with a maximum
        /// particle capacity of 128 particles.
        /// </summary>
        /// <param name="particleType"></param>
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
            _emitCountdown = EmitDelay;
            _coroutine = Burst();
        }

        private IEnumerator Burst()
        {
            for (int i = 0; i < BurstSteps-1; i++)
            {
                CreateParticles();
                yield return BurstStepDelay;
            }
            CreateParticles();
        }

        private void CreateParticles()
        {
            for (int i = 0; i < BurstCunt; i++)
            {
                _system.CreateParticle(Position);
            }
        }
        
        

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _system.Update(deltaTime);


            if (_coroutine != null)
            {
                if (_coroutineDelay > 0)
                {
                    _coroutineDelay -= deltaTime;
                }
                else
                {
                    bool result = _coroutine.MoveNext();
                    
                    if (_coroutine.Current is float)
                        _coroutineDelay = (float)_coroutine.Current;
                    
                    if (!result)
                        _coroutine = null;
                }
            }
            else if (AutoEmit)
            {
                if (_emitCountdown > 0)
                {
                    _emitCountdown -= deltaTime;
                }
                else
                {
                    this.Emit();
                }
            }
        }
        
        
        
        
        public override void Draw(float deltaTime)
        {
            base.Draw(deltaTime);
            _system.Draw(deltaTime);
        }
    }
}