using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleGame
{ 

    public class SineObject : DestroyableObject
    {
        public SineObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 50;
            var textures = new List<Texture2D> { GameManager.Textures["sine"] };
            ObjectDisplay = new ObjectDisplay(textures, Color.Cyan);
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
