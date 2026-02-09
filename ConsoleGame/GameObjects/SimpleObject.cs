using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleGame
{
    public class SimpleObject : DestroyableObject
    {
        private int _xChange = 1;
        private int _yChange = 1;
        protected int beepFreq=0;

        public SimpleObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 20;
            beepFreq = 800;
            var textures = new List<Texture2D> { GameManager.Textures["simple"] };
            ObjectDisplay = new ObjectDisplay(textures, Color.Yellow);
        }

        public override void Update()
        {
            int height = CurrentHeight;
            int width = CurrentWidth;

            if ((_xChange > 0 && CurrentX + _xChange + width > GameManager.NumColumns) ||
                (_xChange < 0 && CurrentX + _xChange < 0))
            {
                _xChange *= -1;
                if (beepFreq > 0)
                    GameManager.Beep(beepFreq);
            }
            if ((_yChange > 0 && CurrentY + _yChange + height > GameManager.NumRows) ||
                (_yChange < 0 && CurrentY + _yChange < 0))
            {
                _yChange *= -1;
                if (beepFreq > 0)
                    GameManager.Beep(beepFreq);
            }
            CurrentX += _xChange;
            CurrentY += _yChange;
        }
    }
}
