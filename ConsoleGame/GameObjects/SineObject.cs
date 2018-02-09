using System;

namespace ConsoleGame
{ 

    public class SineObject : DestoyableObject
    {
        public SineObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 10;
            ObjectDisplay = new ObjectDisplay
            {
                BackgroundColor = ConsoleColor.Cyan,
                ForegroundColor = ConsoleColor.Cyan,
                CharMap = new char[,] { { ' ' } }
            };
            Draw();
        }

        public override void Update()
        {
            _updateColumnIndex();
            var x = CurrentX * (Math.PI * 2 / GameManager.NumColumns);
            var y = (Math.Sin(x) * -1 * GameManager.NumRows / 2) + GameManager.NumRows / 2;
            CurrentY = (int)Math.Floor(y);
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
