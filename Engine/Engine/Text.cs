﻿using System.Collections.Generic;

namespace Engine
{
    public class Text
    {
        private readonly List<CharacterSprite> _bitmapText = new List<CharacterSprite>();
        private readonly Font _font;
        private readonly int _maxWidth = -1;
        private readonly string _text;
        private Color _color = new Color(1, 1, 1, 1);
        private Vector _dimensions;

        public Text(string text, Font font) : this(text, font, -1)
        {
        }

        public Text(string text, Font font, int maxWidth)
        {
            _text = text;
            _font = font;
            _maxWidth = maxWidth;
            CreateText(0, 0, _maxWidth);
        }

        public double Width
        {
            get { return _dimensions.X; }
        }

        public double Height
        {
            get { return _dimensions.Y; }
        }

        public List<CharacterSprite> CharacterSprites
        {
            get { return _bitmapText; }
        }


        private void CreateText(double x, double y)
        {
            CreateText(x, y, _maxWidth);
        }

        private void CreateText(double x, double y, double maxWidth)
        {
            _bitmapText.Clear();
            double currentX = 0;
            double currentY = 0;

            string[] words = _text.Split(' ');

            foreach (string word in words)
            {
                Vector nextWordLength = _font.MeasureFont(word);

                if (maxWidth != -1 &&
                    (currentX + nextWordLength.X) > maxWidth)
                {
                    currentX = 0;
                    currentY += nextWordLength.Y;
                }

                string wordWithSpace = word + " "; // add the space character that was removed.

                foreach (char c in wordWithSpace)
                {
                    CharacterSprite sprite = _font.CreateSprite(c);
                    float xOffset = ((float) sprite.Data.XOffset)/2;
                    float yOffset = (sprite.Data.Height*0.5f) + sprite.Data.YOffset;
                    sprite.Sprite.SetPosition(x + currentX + xOffset, y - currentY - yOffset);
                    currentX += sprite.Data.XAdvance;
                    _bitmapText.Add(sprite);
                }
            }
            _dimensions = _font.MeasureFont(_text, _maxWidth);
            _dimensions.Y = currentY;
            SetColor(_color);
        }


        public void SetColor()
        {
            foreach (CharacterSprite s in _bitmapText)
            {
                s.Sprite.SetColor(_color);
            }
        }

        public void SetColor(Color color)
        {
            _color = color;
            foreach (CharacterSprite s in _bitmapText)
            {
                s.Sprite.SetColor(color);
            }
        }

        public void SetPosition(double x, double y)
        {
            CreateText(x, y);
        }
    }
}