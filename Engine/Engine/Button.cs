using System;

namespace Engine
{
    public class Button
    {
        private readonly Text _label;
        private readonly EventHandler _onPressEvent;
        private Vector _position;

        public Button(EventHandler onPressEvent, Text label)
        {
            _onPressEvent = onPressEvent;
            _label = label;
            _label.SetColor(new Color(0, 0, 0, 1));
            UpdatePosition();
        }

        public Vector Position
        {
            get { return _position; }
            set
            {
                _position = value;
                UpdatePosition();
            }
        }

        public void UpdatePosition()
        {
            // Center label text on position.
            _label.SetPosition(_position.X - (_label.Width/2), _position.Y + (_label.Height/2));
        }

        public void OnGainFocus()
        {
            _label.SetColor(new Color(1, 0, 0, 1));
        }

        public void OnLoseFocus()
        {
            _label.SetColor(new Color(0, 0, 0, 1));
        }

        public void OnPress()
        {
            _onPressEvent(this, EventArgs.Empty);
        }

        public void Render(Renderer renderer)
        {
            renderer.DrawText(_label);
        }
    }
}