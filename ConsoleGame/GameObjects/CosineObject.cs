using System;

namespace ConsoleGame
{

    public class CosineObject : DestoyableObject
    {
        public CosineObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 30;
            ObjectDisplay = new ObjectDisplay
            {
                BackgroundColor = ConsoleColor.Red,
                ForegroundColor = ConsoleColor.Red,
                CharMap = new char[,] { { ' ' } }
            };
            Draw();
        }

        public override void Update()
        {
            _updateColumnIndex();
            var x = CurrentX * (Math.PI * 2 / GameManager.NumColumns);
            var y = (Math.Cos(x) * -1 * GameManager.NumRows / 2) + (GameManager.NumRows / 2);
            CurrentY = (int) Math.Floor(y);
        }

        private void _updateColumnIndex()
        {
            CurrentX++;
            if (CurrentX >= GameManager.NumColumns)
            {
                CurrentX = 0;
            }
        }
    }
}
