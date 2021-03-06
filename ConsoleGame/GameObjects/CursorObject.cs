﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame
{
    public class CursorObject : DestoyableObject
    {
        private List<ConsoleKey> _rightKeys = new List<ConsoleKey>()
        { ConsoleKey.D, ConsoleKey.RightArrow };
        private List<ConsoleKey> _leftKeys = new List<ConsoleKey>()
        { ConsoleKey.A, ConsoleKey.LeftArrow };
        private List<ConsoleKey> _shootKeys = new List<ConsoleKey>()
        { ConsoleKey.Spacebar };
        private int ticksSinceLastFire = -1;

        public CursorObject(GameManager gameManager)
            : base(gameManager, 1, 28)
        {
            foreach (var k in _rightKeys.Union(_leftKeys).Union(_shootKeys))
            {
                RegisteredKeys.Add(k);
            }
            CanDestroyList.Clear();

            var images = new List<char[,]> { new char[,] { { '_', '^', '_' } } };
            ObjectDisplay = new ObjectDisplay(images,
                ConsoleColor.Yellow, ConsoleColor.DarkBlue,
                gameManager.GameForeground, GameManager.GameBackground);
            Draw();
        }

        public override void ProcessKey(ConsoleKeyInfo keyInfo)
        {
            CheckAlive();
            if (isDead)
            {
                return;
            }
            
            if (_rightKeys.Contains(keyInfo.Key) &&
                CurrentX < GameManager.NumColumns - 1)
            {
                CurrentX++;
            }
            else if (_leftKeys.Contains(keyInfo.Key) &&
                CurrentX > 1)
            {
                CurrentX--;
            }
            else if (_shootKeys.Contains(keyInfo.Key) && ticksSinceLastFire == -1)
            {
                GameManager.AddObject("MissileObject", CurrentX, CurrentY - 1);
                ObjectDisplay.CharMaps[0][0, 1] = '_';
                ticksSinceLastFire = 0;
            }
        }

        public override void Update()
        {
            if (ticksSinceLastFire >= 0)
            {
                ticksSinceLastFire++;
                if (ticksSinceLastFire > 1)
                {
                    ticksSinceLastFire = -1;
                    ObjectDisplay.CharMaps[0][0, 1] = '^';
                }
            }
        }
    }
}
