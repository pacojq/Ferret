using FerretEngine.Core;
using FerretEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Sandbox.Player
{
    public class PlayerComponent : Component
    {
        private KeyboardInput _input;

        private float _pxPerSecond = 96;
        
        public PlayerComponent()
        {
            _input = FeInput.Keyboard;
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
        }
    }
}