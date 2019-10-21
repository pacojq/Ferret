using System;
using System.Runtime.CompilerServices;
using FerretEngine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace FerretEngine.Input
{
    public class GamepadInput
    {
        public PlayerIndex PlayerIndex { get; private set; }
        public bool IsConnected { get; private set; }
        
        public float Deadzone
        {
            get => _Deadzone;
            set => _Deadzone = FeMath.Clamp(value, 0f, 1f);
        }
        private float _Deadzone;

        
        
        
        
        // DPad //
        
        public bool DPadLeftHeld => _current.DPad.Left == ButtonState.Pressed;

        public bool DPadLeftPressed => _current.DPad.Left == ButtonState.Pressed && _previous.DPad.Left == ButtonState.Released;

        public bool DPadLeftReleased => _current.DPad.Left == ButtonState.Released && _previous.DPad.Left == ButtonState.Pressed;

        public bool DPadRightHeld => _current.DPad.Right == ButtonState.Pressed;

        public bool DPadRightPressed => _current.DPad.Right == ButtonState.Pressed && _previous.DPad.Right == ButtonState.Released;

        public bool DPadRightReleased => _current.DPad.Right == ButtonState.Released && _previous.DPad.Right == ButtonState.Pressed;

        public bool DPadUpHeld => _current.DPad.Up == ButtonState.Pressed;

        public bool DPadUpPressed => _current.DPad.Up == ButtonState.Pressed && _previous.DPad.Up == ButtonState.Released;

        public bool DPadUpReleased => _current.DPad.Up == ButtonState.Released && _previous.DPad.Up == ButtonState.Pressed;

        public bool DPadDownHeld => _current.DPad.Down == ButtonState.Pressed;

        public bool DPadDownPressed => _current.DPad.Down == ButtonState.Pressed && _previous.DPad.Down == ButtonState.Released;

        public bool DPadDownReleased => _current.DPad.Down == ButtonState.Released && _previous.DPad.Down == ButtonState.Pressed;
    
        
        
        
        
        

        // Utility shortcuts //
        private Vector2 StickLeft => _current.ThumbSticks.Left;
        private Vector2 StickRight => _current.ThumbSticks.Right;
        private Vector2 PrevStickLeft => _previous.ThumbSticks.Left;
        private Vector2 PrevStickRight => _previous.ThumbSticks.Right;
        
        
        
        
        private GamePadState _previous;
        private GamePadState _current;
        
        private float _rumbleStrength;
        private float _rumbleTime;

        
        internal GamepadInput(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
            _current = GamePad.GetState(PlayerIndex);
        }
        
        
        
        
        
        public void Update()
        {
            _previous = _current;
            _current = GamePad.GetState(PlayerIndex);
            IsConnected = _current.IsConnected;

            if (_rumbleTime > 0)
            {
                _rumbleTime -= FeGame.DeltaTime;
                if (_rumbleTime <= 0)
                    GamePad.SetVibration(PlayerIndex, 0, 0);
            }
        }
        
        
        public void Rumble(float strength, float time)
        {
            if (_rumbleTime <= 0 || strength > _rumbleStrength || (strength == _rumbleStrength && time > _rumbleTime))
            {
                GamePad.SetVibration(PlayerIndex, strength, strength);
                _rumbleStrength = strength;
                _rumbleTime = time;
            }
        }
        
        public void StopRumble()
        {
            GamePad.SetVibration(PlayerIndex, 0, 0);
            _rumbleTime = 0;
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsButtonHeld(Buttons button)
        {
            return _current.IsButtonDown(button);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsButtonPressed(Buttons button)
        {
            return _current.IsButtonDown(button) && !_previous.IsButtonDown(button);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsButtonReleased(Buttons button)
        {
            return !_current.IsButtonDown(button) && _previous.IsButtonDown(button);
        }
        
        
        
        
        
        
        public Vector2 GetLeftStick()
        {
            Vector2 result = StickLeft;
            
            if (result.LengthSquared() < Deadzone*Deadzone)
                return Vector2.Zero;
            
            result.Y = -result.Y;
            return result;
        }
        
        public float LeftStickHorizontal()
        {
            float h = StickLeft.X;
            if (Math.Abs(h) < Deadzone)
                return 0;
            return h;
        }

        public float LeftStickVertical()
        {
            float v = StickLeft.Y;
            if (Math.Abs(v) < Deadzone)
                return 0;
            return -v;
        }
        
        
        public bool LeftStickLeftHeld()
        {
            return StickLeft.X <= -Deadzone;
        }

        public bool LeftStickLeftPressed()
        {
            return StickLeft.X <= -Deadzone && _previous.ThumbSticks.Left.X > -Deadzone;
        }

        public bool LeftStickLeftReleased()
        {
            return StickLeft.X > -Deadzone && PrevStickLeft.X <= -Deadzone;
        }

        public bool LeftStickRightHeld()
        {
            return StickLeft.X >= Deadzone;
        }

        public bool LeftStickRightPressed()
        {
            return StickLeft.X >= Deadzone && PrevStickLeft.X < Deadzone;
        }

        public bool LeftStickRightReleased()
        {
            return StickLeft.X < Deadzone && PrevStickLeft.X >= Deadzone;
        }

        public bool LeftStickDownHeld()
        {
            return StickLeft.Y <= -Deadzone;
        }

        public bool LeftStickDownPressed()
        {
            return StickLeft.Y <= -Deadzone && PrevStickLeft.Y > -Deadzone;
        }

        public bool LeftStickDownReleased()
        {
            return StickLeft.Y > -Deadzone && PrevStickLeft.Y <= -Deadzone;
        }

        public bool LeftStickUpHeld()
        {
            return StickLeft.Y >= Deadzone;
        }

        public bool LeftStickUpPressed()
        {
            return StickLeft.Y >= Deadzone && PrevStickLeft.Y < Deadzone;
        }

        public bool LeftStickUpReleased()
        {
            return StickLeft.Y < Deadzone && PrevStickLeft.Y >= Deadzone;
        }
    
    
        
        
        
        
        
        
        public Vector2 GetRightStick()
        {
            Vector2 result = _current.ThumbSticks.Right;
            
            if (result.LengthSquared() < Deadzone*Deadzone)
                return Vector2.Zero;
            
            result.Y = -result.Y;
            return result;
        }
        
        public float RightStickHorizontal()
        {
            float h = _current.ThumbSticks.Right.X;
            if (Math.Abs(h) < Deadzone)
                return 0;
            return h;
        }

        public float RightStickVertical()
        {
            float v = _current.ThumbSticks.Right.Y;
            if (Math.Abs(v) < Deadzone)
                return 0;
            return -v;
        }
        
        public bool RightStickLeftHeld()
        {
            return StickRight.X <= -Deadzone;
        }

        public bool RightStickLeftPressed()
        {
            return StickRight.X <= -Deadzone && PrevStickRight.X > -Deadzone;
        }

        public bool RightStickLeftReleased()
        {
            return StickRight.X > -Deadzone && PrevStickRight.X <= -Deadzone;
        }

        public bool RightStickRightHeld()
        {
            return StickRight.X >= Deadzone;
        }

        public bool RightStickRightPressed()
        {
            return StickRight.X >= Deadzone && PrevStickRight.X < Deadzone;
        }

        public bool RightStickRightReleased()
        {
            return StickRight.X < Deadzone && PrevStickRight.X >= Deadzone;
        }

        public bool RightStickUpHeld()
        {
            return StickRight.Y <= -Deadzone;
        }

        public bool RightStickUpPressed()
        {
            return StickRight.Y <= -Deadzone && PrevStickRight.Y > -Deadzone;
        }

        public bool RightStickUpReleased()
        {
            return StickRight.Y > -Deadzone && PrevStickRight.Y <= -Deadzone;
        }

        public bool RightStickDownHeld()
        {
            return StickRight.Y >= Deadzone;
        }

        public bool RightStickDownPressed()
        {
            return StickRight.Y >= Deadzone && PrevStickRight.Y < Deadzone;
        }

        public bool RightStickDownReleased()
        {
            return StickRight.Y < Deadzone && PrevStickRight.Y >= Deadzone;
        }
    

        
        
        
        public bool LeftTriggerHeld(float threshold)
        {
            return _current.Triggers.Left >= threshold;
        }

        public bool LeftTriggerPressed(float threshold)
        {
            return _current.Triggers.Left >= threshold && _previous.Triggers.Left < threshold;
        }

        public bool LeftTriggerReleased(float threshold)
        {
            return _current.Triggers.Left < threshold && _previous.Triggers.Left >= threshold;
        }

        public bool RightTriggerHeld(float threshold)
        {
            return _current.Triggers.Right >= threshold;
        }

        public bool RightTriggerPressed(float threshold)
        {
            return _current.Triggers.Right >= threshold && _previous.Triggers.Right < threshold;
        }

        public bool RightTriggerReleased(float threshold)
        {
            return _current.Triggers.Right < threshold && _previous.Triggers.Right >= threshold;
        }
        
    }
}