using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class SimpleObject : DestoyableObject
    {
        private int _xChange = 1;
        private int _yChange = 1;
        protected int beepFreq=0;

        public SimpleObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 20;
            beepFreq = 800;
            var images = new List<char[,]> { new char[,] { { ' ' } } };
            ObjectDisplay = new ObjectDisplay(images,
                ConsoleColor.Yellow, ConsoleColor.Yellow,
                gameManager.GameForeground, GameManager.GameBackground);
            Draw();
        }

        public override void Update()
        {
            var height = ObjectDisplay.CurrentCharMap.GetLength(0);
            var width = ObjectDisplay.CurrentCharMap.GetLength(1);
            if ((_xChange > 0 && CurrentX + _xChange * width > GameManager.NumColumns) ||
                (_xChange < 0 && CurrentX + _xChange < 0))
            {
                _xChange *= -1;
                if (beepFreq > 0)
                    GameManager.Beep(beepFreq);
            }
            if ((_yChange > 0 && CurrentY + _yChange * height > GameManager.NumRows) ||
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
