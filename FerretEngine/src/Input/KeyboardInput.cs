using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
    public class KeyboardInput
    {
        private KeyboardState _previous;
        private KeyboardState _current;

        private MouseState _mousePrevious;
        private MouseState _mouseCurrent;

        internal KeyboardInput()
        {
            
        }
        
        internal void Update()
        {
            _previous = _current;
            _current = Keyboard.GetState();

            _mousePrevious = _mouseCurrent;
            _mouseCurrent = Mouse.GetState();
        }
        
        
        
        
        public bool IsKeyHeld(Keys key)
        {
            return _current.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return _current.IsKeyDown(key) && !_previous.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return !_current.IsKeyDown(key) && _previous.IsKeyDown(key);
        }


        public Vector2 GetMousePosition()
        {
            return new Vector2(_mouseCurrent.X, _mouseCurrent.Y);
        }
    }
}