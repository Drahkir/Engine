using System;
using Tao.Sdl;

namespace Engine.Input
{
    public class ControlTrigger
    {
        private readonly int _index;
        private readonly IntPtr _joystick;
        private readonly bool _top; // The triggers are treated as axes and need splitting up
        private float _deadZone = 0.24f;

        public ControlTrigger(IntPtr joystick, int index, bool top)
        {
            _joystick = joystick;
            _index = index;
            _top = top;
        }

        public float Value { get; private set; }

        public void Update()
        {
            Value = MapZeroToOne(Sdl.SDL_JoystickGetAxis(_joystick, _index));
        }

        private float MapZeroToOne(short value)
        {
            float output = ((float) value/short.MaxValue);

            if (!_top)
            {
                if (output > 0)
                {
                    output = 0;
                }

                output = Math.Abs(output);
            }

            // Be careful of rounding error
            output = Math.Min(output, 1.0f);
            output = Math.Max(output, 0.0f);

            if (Math.Abs(output) < _deadZone)
            {
                output = 0;
            }

            return output;
        }
    }
}