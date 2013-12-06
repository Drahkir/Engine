using Tao.Sdl;

namespace Engine.Input
{
    public class Input
    {
        private readonly bool _usingController;

        public Input()
        {
            Sdl.SDL_InitSubSystem(Sdl.SDL_INIT_JOYSTICK);

            if (Sdl.SDL_NumJoysticks() > 0)
            {
                Controller = new XboxController(0);
                _usingController = true;
            }
        }

        public Mouse Mouse { get; set; }
        public Keyboard Keyboard { get; set; }
        public Point MousePosition { get; set; }
        public XboxController Controller { get; set; }

        public void Update(double elapsedTime)
        {
            if (_usingController)
            {
                Sdl.SDL_JoystickUpdate();
                Controller.Update();
            }
            Mouse.Update(elapsedTime);
            Keyboard.Process();
        }
    }
}