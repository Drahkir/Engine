﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Engine
{
    public class VerticalMenu
    {
        private readonly List<Button> _buttons = new List<Button>();
        private readonly Input.Input _input;
        private int _currentFocus;

        // Input Handling
        private bool _inDown;
        private bool _inUp;
        private Vector _position;

        public VerticalMenu(double x, double y, Input.Input input)
        {
            _input = input;
            _position = new Vector(x, y, 0);
            Spacing = 50;
        }

        public double Spacing { get; set; }

        public void AddButton(Button button)
        {
            double _currentY = _position.Y;

            if (_buttons.Count != 0)
            {
                _currentY = _buttons.Last().Position.Y;
                _currentY -= Spacing;
            }
            else
            {
                // It's the first button added it shoud have
                // focus
                button.OnGainFocus();
            }

            button.Position = new Vector(_position.X, _currentY, 0);
            _buttons.Add(button);
        }

        public void HandleInput()
        {
            bool controlPadDown = false;
            bool controlPadUp = false;

            if (_input.Controller != null)
            {
                float invertY = _input.Controller.LeftControlStick.Y*-1;

                if (invertY < -0.2)
                {
                    // The control stick is pulled down
                    if (_inDown == false)
                    {
                        controlPadDown = true;
                        _inDown = true;
                    }
                }
                else
                {
                    _inDown = false;
                }

                if (invertY > 0.2)
                {
                    if (_inUp == false)
                    {
                        controlPadUp = true;
                        _inUp = true;
                    }
                }

                else
                {
                    _inUp = false;
                }
            }
            if (_input.Keyboard.IsKeyPressed(Keys.Down) || controlPadDown)
            {
                OnDown();
            }
            else if (_input.Keyboard.IsKeyPressed(Keys.Up) || controlPadUp)
            {
                OnUp();
            }

            else if (_input.Keyboard.IsKeyPressed(Keys.Enter) ||
                     (_input.Controller != null && _input.Controller.ButtonA.Pressed))
            {
                OnButtonPress();
            }
        }

        private void OnButtonPress()
        {
            _buttons[_currentFocus].OnPress();
        }

        private void OnDown()
        {
            int oldFocus = _currentFocus;
            _currentFocus++;

            if (_currentFocus == _buttons.Count)
            {
                _currentFocus = 0;
            }
            ChangeFocus(oldFocus, _currentFocus);
        }

        private void OnUp()
        {
            int oldFocus = _currentFocus;
            _currentFocus--;

            if (_currentFocus == -1)
            {
                _currentFocus = (_buttons.Count - 1);
            }

            ChangeFocus(oldFocus, _currentFocus);
        }

        private void ChangeFocus(int from, int to)
        {
            if (from != to)
            {
                _buttons[from].OnLoseFocus();
                _buttons[to].OnGainFocus();
            }
        }

        public void Render(Renderer renderer)
        {
            _buttons.ForEach(x => x.Render(renderer));
        }
    }
}