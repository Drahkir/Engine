using System;
using Tao.Sdl;

namespace Engine.Input
{
    public class ControlStick
    {
        private readonly int _axisIdX;
        private readonly int _axisIdY;
        private readonly IntPtr _joystick;
        private float _deadZone = 0.2f;

        public ControlStick(IntPtr joystick, int axisIdX, int axisIdY)
        {
            _joystick = joystick;
            _axisIdX = axisIdX;
            _axisIdY = axisIdY;
        }

        public float X { get; private set; }
        public float Y { get; private set; }

        public void Update()
        {
            X = MapMinusOneToOne(Sdl.SDL_JoystickGetAxis(_joystick, _axisIdX));
            Y = MapMinusOneToOne(Sdl.SDL_JoystickGetAxis(_joystick, _axisIdY));
        }

        private float MapMinusOneToOne(short value)
        {
            float output = ((float) value/short.MaxValue);

            // Be careful of rounding error
            output = Math.Min(output, 1.0f);
            output = Math.Max(output, -1.0f);

            if (Math.Abs(output) < _deadZone)
            {
                output = 0;
            }

            return output;
        }
    }
}