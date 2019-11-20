using FerretEngine.Components;
using FerretEngine.Components.Colliders;
using FerretEngine.Content;
using FerretEngine.Core;
using FerretEngine.Graphics;
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
            Sprite[] sprites = FeContent.LoadSpriteSheet("character/charAtlas.feAsset");
            
            Animation anim = new Animation(sprites, "walk", .1f);
            AnimationController contr = new AnimationController(anim);
            
            SpriteRenderer renderer = new SpriteRenderer(sprites[0]);
            renderer.Material = new Material(SandboxGame.TestEffect);
            
            AnimationComponent animComponent = new AnimationComponent(renderer, contr);

            Bind(renderer);
            Bind(animComponent);
            
            
            

            BoxCollider col = new BoxCollider(sprites[0].Width, sprites[0].Height, Vector2.Zero);
            Bind(col);

            col.OnCollisionEnter += o => FeLog.Debug($"PLAYER ENTER: {this}");
            col.OnCollisionExit += o => FeLog.Debug($"PLAYER EXIT: {this}");


            ParticleType partType = new ParticleType(FeContent.LoadSprite("box.png"))
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
            
            
            
            //Bind(new PlayerComponent());
            Bind(new PlayerComponentGamepad());
        }
    }
}