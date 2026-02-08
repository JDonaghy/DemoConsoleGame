using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame
{
    public class ExplosionObject : BaseObject
    {
        private int _ticks=0;
        public ExplosionObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            var images = new List<char[,]>
            {
                new char[,] 
                { 
                    { '*', ' ', '*' } , 
                    { ' ', ' ', ' ' } , 
                    { '*', ' ', '*' }
                },
                new char[,]
                {
                    { ' ', '.', ' ' } ,
                    { '.', ' ', '.' } ,
                    { ' ', '.', ' ' }
                }
            };
            ObjectDisplay = new ObjectDisplay(images,
                ConsoleColor.Yellow, ConsoleColor.DarkBlue,
                gameManager.GameForeground, GameManager.GameBackground);
            Draw();
        }

        public override void Update()
        {
            _ticks++;
            if (_ticks > 1)
                isDead = true;
        }
    }
}
