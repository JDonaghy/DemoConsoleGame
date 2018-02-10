using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class MissileObject : BaseObject
    {
        private List<string> _canDestroy = new List<string>(
            new string[] { "SimpleObject", "SineObject", "CosineObject" });

        public MissileObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            var images = new List<char[,]> { new char[,] { { '|' } } };
            ObjectDisplay = new ObjectDisplay(images,
                ConsoleColor.Yellow, ConsoleColor.DarkBlue,
                gameManager.GameForeground, GameManager.GameBackground);
            Draw();
            SlowFactor = 2;
        }

        public override void Update()
        {
            if (CurrentY > 0)
            {
                CurrentY--;
            }
            else
            {
                isDead = true;
            }
        }

        public override List<string> CanDestroy => _canDestroy;
    }
}
