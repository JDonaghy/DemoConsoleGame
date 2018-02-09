using System;

namespace ConsoleGame.GameObjects
{
    public class GameOverObject : BaseObject
    {
        private int inc = 1;
        private const string _gameOver = "G A M E    O V E R";

        public GameOverObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            RegisteredKeys.Add(ConsoleKey.Spacebar);
            ObjectDisplay = new ObjectDisplay
            {
                BackgroundColor = ConsoleColor.DarkBlue,
                ForegroundColor = ConsoleColor.Yellow,
                CharMap = new char[,] { { 'G', ' ', 'A', ' ', 'M', ' ', 'E', ' ', ' ', ' ', 'O', ' ', 'V', ' ', 'E', ' ', 'R' } }
            };
            Draw();
        }

        public override void Update()
        {
            
            CurrentY += inc;
            if (CurrentY > GameManager.NumRows - 5)
            {
                inc = -1;
            }
            else if (CurrentY < 5)
            {
                inc = 1;
            }
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            IsDead = true;
        }
    }
}
