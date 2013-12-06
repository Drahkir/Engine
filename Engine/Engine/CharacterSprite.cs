namespace Engine
{
    public class CharacterSprite
    {
        public CharacterSprite(Sprite sprite, CharacterData data)
        {
            Data = data;
            Sprite = sprite;
        }

        public Sprite Sprite { get; set; }
        public CharacterData Data { get; set; }
    }
}