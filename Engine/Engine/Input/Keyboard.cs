using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Engine.Input
{
    public class Keyboard
    {
        private readonly Dictionary<Keys, KeyState> _keyStates = new Dictionary<Keys, KeyState>();
        public KeyPressEventHandler KeyPressEvent;
        private Control _openGLControl;

        public Keyboard(Control openGLControl)
        {
            _openGLControl = openGLControl;
            _openGLControl.KeyDown += OnKeyDown;
            _openGLControl.KeyUp += OnKeyUp;
            _openGLControl.KeyPress += OnKeyPress;
        }

        [DllImport("User32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

        private void OnKeyPress(object sender, KeyPressEventArgs e)
        {
            if (KeyPressEvent != null)
            {
                KeyPressEvent(sender, e);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            EnsureKeyStateExists(e.KeyCode);
            _keyStates[e.KeyCode].OnUp();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            EnsureKeyStateExists(e.KeyCode);
            _keyStates[e.KeyCode].OnDown();
        }

        private void EnsureKeyStateExists(Keys key)
        {
            if (!_keyStates.Keys.Contains(key))
            {
                _keyStates.Add(key, new KeyState());
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            EnsureKeyStateExists(key);
            return _keyStates[key].Pressed;
        }

        public bool IsKeyHeld(Keys key)
        {
            EnsureKeyStateExists(key);
            return _keyStates[key].Held;
        }

        public void Process()
        {
            ProcessControlKeys();
            foreach (KeyState state in _keyStates.Values)
            {
                // Reset state.
                state.Pressed = false;
                state.Process();
            }
        }

        private bool PollKeyPress(Keys key)
        {
            return (GetAsyncKeyState((int) key) != 0);
        }

        private void ProcessControlKeys()
        {
            UpdateControlKey(Keys.Left);
            UpdateControlKey(Keys.Right);
            UpdateControlKey(Keys.Up);
            UpdateControlKey(Keys.Down);
            UpdateControlKey(Keys.LMenu); // this is the left alt key
        }

        private void UpdateControlKey(Keys keys)
        {
            if (PollKeyPress(keys))
            {
                OnKeyDown(this, new KeyEventArgs(keys));
            }
            else
            {
                OnKeyUp(this, new KeyEventArgs(keys));
            }
        }

        private class KeyState
        {
            private bool _keyPressDetected;

            public KeyState()
            {
                Held = false;
                Pressed = false;
            }

            public bool Held { get; set; }
            public bool Pressed { get; set; }

            internal void OnDown()
            {
                if (Held == false)
                {
                    _keyPressDetected = true;
                }
                Held = true;
            }

            internal void OnUp()
            {
                Held = false;
            }

            internal void Process()
            {
                Pressed = false;
                if (_keyPressDetected)
                {
                    Pressed = true;
                    _keyPressDetected = false;
                }
            }
        }
    }
}