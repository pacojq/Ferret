using System;
using System.Runtime.CompilerServices;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Effects;
using FerretEngine.Logging;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FerretEngine.Particles
{
    public class ParticleSystem
    {

        public int MaxParticles
        {
            get => _maxParticles;
            internal set => SetMaxParticles(value);
        }
        private int _maxParticles;
        
        public ParticleType ParticleType { get; internal set; }

        /// <summary>
        /// Origin of the particle system.
        /// </summary>
        public Vector2 Origin { get; set; }
        
        
        public Material Material { get; set; }
        

        private Particle[] _particles;
        
        /// <summary>
        /// Pointer to the next free particle
        /// </summary>
        private int _seek;


        
        
        
        
        public ParticleSystem(ParticleType particleType, int maxParticles)
        {
            ParticleType = particleType;
            Origin = Vector2.Zero;
            Material = Material.Default;
            _particles = new Particle[maxParticles];
            _maxParticles = maxParticles;
            _seek = 0;
        }


        internal void CreateParticle(Vector2 position)
        {
            _particles[_seek] = ParticleType.CreateParticle(_particles[_seek]);
            
            Particle particle = _particles[_seek];
            particle.Active = true;
            particle.Position = position;
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
            for (int i = 0; i < _maxParticles; i++)
            {
                if (_particles[i].Active)
                    Update(ref _particles[i], deltaTime);
            }
        }


        private void Update(ref Particle part, float deltaTime)
        {
            part.LifeTime -= deltaTime;
            if (part.LifeTime <= 0)
            {
                part.Active = false;
                return;
            }

            Vector2 delta = new Vector2( 
                    FeMath.Cos(part.Direction) * part.Speed,
                    FeMath.Sin(part.Direction) * part.Speed
                );
            
            part.Position += delta;
            part.Speed += part.Acceleration;

            part.Angle += part.Rotation;
            
            part.Size += part.Growth;
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Draw(float deltaTime)
        {
            foreach (var p in _particles)
                if (p.Active)
                    Draw(p, deltaTime);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Draw(Particle part, float deltaTime)
        {
            FeDraw.SetMaterial(Material);
            FeDraw.SpriteExt(
                    ParticleType.Sprite,
                    Origin + part.Position,
                    part.Color,
                    part.Angle,
                    Vector2.One * part.Size,
                    SpriteEffects.None,
                    1
                );
            FeDraw.SetMaterial(Material.Default);
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