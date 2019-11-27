using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Input;
using FerretEngine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerComponent : Component
    {
        private ParticleEmitter _emitter;
        private KeyboardInput _input;

        private float _pxPerSecond = 96;
        
        public PlayerComponent()
        {
            _input = FeInput.Keyboard;
        }


        protected override void OnBinding(Entity entity)
        {
            base.OnBinding(entity);
            _emitter = entity.Get<ParticleEmitter>();
        }

        public override void Update(float deltaTime)
        {
            Vector2 pos = this.Entity.Position;


            int xSpd = 0;
            int ySpd = 0;
            
            if (_input.IsKeyHeld(Keys.Right)) xSpd++;
            if (_input.IsKeyHeld(Keys.Left))  xSpd--;
            
            if (_input.IsKeyHeld(Keys.Down))  ySpd++;
            if (_input.IsKeyHeld(Keys.Up))    ySpd--;

            pos.X += xSpd * (_pxPerSecond * deltaTime);
            pos.Y += ySpd * (_pxPerSecond * deltaTime);

            this.Entity.Position = pos;
            this.Entity.Scene.MainCamera.Position = pos;

            if (_input.IsKeyPressed(Keys.Space))
                _emitter.Emit();
        }


        public override void DrawGUI(float deltaTime)
        {
            base.DrawGUI(deltaTime);

            //FeDraw.Rect(0, 0, FeGame.Width, FeGame.Height, true);
            
            FeDraw.SetHAlign(FeDraw.HAlign.Left);
            FeDraw.SetVAlign(FeDraw.VAlign.Top);
            FeDraw.Text("This is GUI text", new Vector2(-FeGame.Width/2f, -FeGame.Height/2f));
            
            
            FeDraw.SetHAlign(FeDraw.HAlign.Centre);
            FeDraw.SetVAlign(FeDraw.VAlign.Centre);
            FeDraw.Text("Hi there :D", new Vector2(96, 24));
        }
    }
}