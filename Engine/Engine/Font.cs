using System;
using System.Collections.Generic;

namespace Engine
{
    public class Font
    {
        private readonly Dictionary<char, CharacterData> _characterData;
        private Texture _texture;

        public Font(Texture texture, Dictionary<char, CharacterData> characterData)
        {
            _texture = texture;
            _characterData = characterData;
        }

        public Vector MeasureFont(string text)
        {
            return MeasureFont(text, -1);
        }

        public Vector MeasureFont(string text, double maxWidth)
        {
            var dimensions = new Vector();

            foreach (char c in text)
            {
                CharacterData data = _characterData[c];
                dimensions.X += data.XAdvance;
                dimensions.Y = Math.Max(dimensions.Y, data.Height + data.YOffset);
            }
            return dimensions;
        }

        public CharacterSprite CreateSprite(char c)
        {
            CharacterData charData = _characterData[c];
            var sprite = new Sprite();
            sprite.Texture = _texture;

            // Setup UVs
            var topLeft = new Point(charData.X/(float) _texture.Width,
                charData.Y/(float) _texture.Height);
            var bottomRight = new Point(topLeft.X + (charData.Width/(float) _texture.Width),
                topLeft.Y + (charData.Height/(float) _texture.Height));
            sprite.SetUVs(topLeft, bottomRight);
            sprite.SetWidth(charData.Width);
            sprite.SetHeight(charData.Height);
            sprite.SetColor(new Color(1, 1, 1, 1));

            return new CharacterSprite(sprite, charData);
        }
    }
}