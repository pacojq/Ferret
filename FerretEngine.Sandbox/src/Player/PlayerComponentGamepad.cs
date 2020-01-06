using FerretEngine.Core;
using FerretEngine.Graphics;
using FerretEngine.Input;
using FerretEngine.Particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerComponentGamepad : Component
    {
        private ParticleEmitter _emitter;
        private GamepadInput _input;

        private float _pxPerSecond = 96;
        
        public PlayerComponentGamepad()
        {
            _input = FeInput.Gamepad;
        }


        protected override void OnBinding(Entity entity)
        {
            base.OnBinding(entity);
            _emitter = entity.Get<ParticleEmitter>();
        }

        public override void Update(float deltaTime)
        {
            Vector2 pos = this.Entity.Position;


            float xSpd = _input.GetLeftStick().X;
            float ySpd = _input.GetLeftStick().Y;
            
            pos.X += xSpd * (_pxPerSecond * deltaTime);
            pos.Y += ySpd * (_pxPerSecond * deltaTime);

            this.Entity.Position = pos;
            this.Entity.Scene.MainCamera.Position = pos;

            if (_input.IsButtonPressed(Buttons.A))
                _emitter.Emit();
        }


        public override void DrawGUI(float deltaTime)
        {
            base.DrawGUI(deltaTime);

            FeDraw.Rect(0, 0, FeGame.Width, FeGame.Height, true);
            
            FeDraw.SetHAlign(FeDraw.HAlign.Left);
            FeDraw.SetVAlign(FeDraw.VAlign.Top);
            FeDraw.Text("This is GUI text", new Vector2(-FeGame.Width/2f, -FeGame.Height/2f));
            
            
            FeDraw.SetHAlign(FeDraw.HAlign.Centre);
            FeDraw.SetVAlign(FeDraw.VAlign.Centre);
            FeDraw.Text("Hi there :D", new Vector2(0, 24));
        }
    }
}