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
            ObjectDisplay = new ObjectDisplay
            {
                BackgroundColor = ConsoleColor.DarkBlue,
                ForegroundColor = ConsoleColor.Yellow,
                CharMap = new char[,] { { '|' } }
            };
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
