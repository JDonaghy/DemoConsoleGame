using System;
using System.Collections.Generic;

namespace ConsoleGame.GameObjects
{
    public class GameOverObject : SimpleObject
    {
        private int inc = 1;
        private const string _gameOver = "G A M E    O V E R";

        public GameOverObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            RegisteredKeys.Add(ConsoleKey.Spacebar);
            var images = new List<char[,]> { new char[,] { { 'G', ' ', 'A', ' ', 'M', ' ', 'E', ' ', ' ', ' ', 'O', ' ', 'V', ' ', 'E', ' ', 'R' } } };
            ObjectDisplay = new ObjectDisplay(images,
                ConsoleColor.Yellow, ConsoleColor.DarkBlue,
                gameManager.GameForeground, GameManager.GameBackground);
            Draw();
            beepFreq = 0;
            Draw();
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            IsDead = true;
        }
    }
}
