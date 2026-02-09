using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleGame.GameObjects
{
    public class GameOverObject : SimpleObject
    {
        public GameOverObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            RegisteredKeys.Add(Keys.Space);
            var textures = new List<Texture2D> { GameManager.Textures["gameover"] };
            ObjectDisplay = new ObjectDisplay(textures, Color.Yellow);
            beepFreq = 0;
        }

        public override void ProcessInput(KeyboardState keyState)
        {
             if (keyState.IsKeyDown(Keys.Space))
             {
                 IsDead = true;
             }
        }
    }
}
