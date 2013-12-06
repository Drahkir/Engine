using System;
using Tao.Sdl;

namespace Engine.Input
{
    public class ControllerButton
    {
        private readonly int _buttonId;
        private readonly IntPtr _joystick;

        private bool _wasHeld;

        public ControllerButton(IntPtr joystick, int buttonId)
        {
            _joystick = joystick;
            _buttonId = buttonId;
        }

        public bool Held { get; private set; }
        public bool Pressed { get; private set; }

        public void Update()
        {
            // Reset the pressed value
            Pressed = false;

            byte buttonState = Sdl.SDL_JoystickGetButton(_joystick, _buttonId);
            Held = (buttonState == 1);

            if (Held)
            {
                if (_wasHeld == false)
                {
                    Pressed = true;
                }
                _wasHeld = true;
            }
            else
            {
                _wasHeld = false;
            }
        }
    }
}