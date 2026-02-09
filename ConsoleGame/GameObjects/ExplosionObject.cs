using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConsoleGame.GameObjects;

namespace ConsoleGame
{
    public class ExplosionObject : BaseObject
    {
        private int _ticks=0;
        public ExplosionObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            var textures = new List<Texture2D> 
            { 
                GameManager.Textures["explosion"],
                GameManager.Textures["explosion2"]
            };
            ObjectDisplay = new ObjectDisplay(textures, Color.Yellow);
        }

        public override void Update()
        {
            _ticks++;
            if (_ticks > 4) // Slower explosion
                isDead = true;
        }
    }
}
