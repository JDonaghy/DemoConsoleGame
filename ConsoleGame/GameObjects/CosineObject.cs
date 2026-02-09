using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleGame
{
    public class CosineObject : DestroyableObject
    {
        public CosineObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            points = 100;
            var textures = new List<Texture2D> { GameManager.Textures["cosine"] };
            ObjectDisplay = new ObjectDisplay(textures, Color.Red);
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
