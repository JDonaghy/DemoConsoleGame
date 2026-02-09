using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class ObjectDisplay
    {
        private int _textureIndex = 0;
        public List<Texture2D> Textures { get; set; }
        public Color Color { get; set; }

        public int TextureIndex
        {
            get => _textureIndex;
            set => _textureIndex = value % Textures.Count;
        }
        public bool AutoAnimate { get; set; } = true;

        public ObjectDisplay(List<Texture2D> textures, Color color)
        {
            Textures = textures;
            Color = color;
        }

        public Vector2 Draw(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            var tex = Textures[_textureIndex];
            spriteBatch.Draw(tex, position, null, Color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            if (AutoAnimate)
            {
                _textureIndex++;
                if (_textureIndex > Textures.Count - 1)
                {
                    _textureIndex = 0;
                }
            }
            return new Vector2(tex.Width * scale, tex.Height * scale);
        }
    }
}
