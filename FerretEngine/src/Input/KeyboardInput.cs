using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
    public class KeyboardInput
    {
        private KeyboardState _previous;
        private KeyboardState _current;


        internal KeyboardInput()
        {
            
        }
        
        internal void Update()
        {
            _previous = _current;
            _current = Keyboard.GetState();
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
        
    }
}