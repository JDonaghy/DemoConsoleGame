using System;

namespace ConsoleGame
{
    public class SimpleObject : DestoyableObject
    {
        private int _x = 1;
        private int _y = 1;

        public SimpleObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 20;
            ObjectDisplay = new ObjectDisplay
            {
                BackgroundColor = ConsoleColor.Yellow,
                ForegroundColor = ConsoleColor.Yellow,
                CharMap = new char[,] { { ' ' } }
            };
            Draw();
        }

        public override void Update()
        {
            if ((_x > 0 && CurrentX + _x > GameManager.NumColumns) ||
                (_x < 0 && CurrentX + _x < 0))
            {
                _x *= -1;
                GameManager.Beep(500);
            }
            if ((_y > 0 && CurrentY + _y > GameManager.NumRows) ||
                (_y < 0 && CurrentY + _y < 0))
            {
                _y *= -1;
                GameManager.Beep(800);
            }
            CurrentX += _x;
            CurrentY += _y;
        }
    }
}
