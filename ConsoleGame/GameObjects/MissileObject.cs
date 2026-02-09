using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ConsoleGame.GameObjects;

namespace ConsoleGame
{
    public class MissileObject : BaseObject
    {
        private List<string> _canDestroy = new List<string>
        { "SimpleObject", "SineObject", "CosineObject" };

        public MissileObject(GameManager gameManager, int x, int y)
            : base(gameManager, x, y)
        {
            var textures = new List<Texture2D> { GameManager.Textures["missile"] };
            ObjectDisplay = new ObjectDisplay(textures, Color.Yellow);
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
