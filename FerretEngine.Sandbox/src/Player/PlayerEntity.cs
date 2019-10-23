using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Graphics.Loading;
using FerretEngine.Logging;
using FerretEngine.Particles;
using FerretEngine.Particles.ParticleAttributes;
using Microsoft.Xna.Framework;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerEntity : Entity
    {
        public PlayerEntity()
        {
            Sprite sprite = SpriteLoader.LoadSprites("character/charAtlas.feAsset")[0];
            SpriteRenderer renderer = new SpriteRenderer(sprite);
            
            renderer.Material = new Material(SandboxGame.TestEffect);
            
            Bind(renderer);

            BoxCollider col = new BoxCollider(sprite.Width, sprite.Height, Vector2.Zero);
            Bind(col);

            col.OnCollisionEnter += o => FeLog.Debug($"PLAYER ENTER: {this}");
            col.OnCollisionExit += o => FeLog.Debug($"PLAYER EXIT: {this}");


            ParticleType partType = new ParticleType(FeGraphics.LoadSprite("box"))
            {
                Lifetime = ParticleLifetime.RandomRange(0.5f, 1f),
                StartAngle = ParticleAngle.RandomRange(0, 360),
                StartSpeed = ParticleSpeed.RandomRange(-2f, 2f),
                StartDirection = ParticleDirection.RandomRange(0, 360)
            };

            ParticleEmitter emitter = new ParticleEmitter(partType)
            {
                AutoEmit = true,
                BurstCunt = 5,
                EmitDelay = 3,
                BurstSteps = 8,
                BurstStepDelay = .1f
            };

            Bind(emitter);
            
            
            
            Bind(new PlayerComponent());
        }
    }
}