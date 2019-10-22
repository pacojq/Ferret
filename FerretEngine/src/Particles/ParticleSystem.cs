using System;
using System.Runtime.CompilerServices;
using FerretEngine.Core;
using Microsoft.Xna.Framework;

namespace FerretEngine.Particles
{
    internal class ParticleSystem
    {

        public int MaxParticles
        {
            get => _maxParticles;
            set => SetMaxParticles(value);
        }
        private int _maxParticles;
        
        public ParticleType ParticleType { get; set; }
        

        private Particle[] _particles;
        private int _seek;


        
        
        
        
        public ParticleSystem(ParticleType particleType, int maxParticles)
        {
            _particles = new Particle[maxParticles];
            _maxParticles = maxParticles;
            _seek = 0;
        }


        internal void Add(Particle particle)
        {
            particle.Active = true;
            _particles[_seek] = particle;

            
            // TODO improve seek increase
            // to actually have _maxParticles number of particles available
            
            _seek++;
            if (_seek >= _maxParticles)
                _seek = 0;
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Update(float deltaTime)
        {
            foreach (var p in _particles)
                if (p.Active)
                    Update(p, deltaTime);
        }


        private void Update(Particle part, float deltaTime)
        {
            
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Draw(float deltaTime)
        {
            foreach (var p in _particles)
                if (p.Active)
                    Draw(p, deltaTime);
        }
        
        private void Draw(Particle part, float deltaTime)
        {
            
        }
        
        
        
        
        
        

        private void SetMaxParticles(int maxParticles)
        {
            Particle[] part = new Particle[maxParticles];

            int min = Math.Min(_maxParticles, maxParticles);
            for (int i = 0; i < min; i++)
                part[i] = _particles[i];

            _particles = part;
            _maxParticles = maxParticles;
        }

    }
}