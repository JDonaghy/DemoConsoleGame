using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleGame
{
    public class CursorObject : DestroyableObject
    {
        private List<Keys> _rightKeys = new List<Keys> { Keys.D, Keys.Right };
        private List<Keys> _leftKeys = new List<Keys> { Keys.A, Keys.Left };
        private List<Keys> _shootKeys = new List<Keys> { Keys.Space };
        private int ticksSinceLastFire = -1;
        private int _inputDelay = 0;

        public CursorObject(GameManager gameManager)
            : base(gameManager, 1, 40) // Adjusted Y lower to catch objects
        {
            foreach (var k in _rightKeys.Union(_leftKeys).Union(_shootKeys))
            {
                RegisteredKeys.Add(k);
            }
            CanDestroyList.Clear();

            var textures = new List<Texture2D> 
            { 
                GameManager.Textures["cursor"], 
                GameManager.Textures["cursor_fire"] 
            };
            ObjectDisplay = new ObjectDisplay(textures, Color.Yellow);
            ObjectDisplay.AutoAnimate = false;
        }

        public override void ProcessInput(KeyboardState keyState)
        {
            CheckAlive();
            if (isDead) return;
            
            if (_inputDelay > 0) 
            {
                _inputDelay--;
            }
            else 
            {
                bool moved = false;
                if (_rightKeys.Any(k => keyState.IsKeyDown(k)) && CurrentX < GameManager.NumColumns - 1)
                {
                    CurrentX++;
                    moved = true;
                }
                else if (_leftKeys.Any(k => keyState.IsKeyDown(k)) && CurrentX > 1)
                {
                    CurrentX--;
                    moved = true;
                }
                if (moved) _inputDelay = 3; // Throttle movement
            }

            if (_shootKeys.Any(k => keyState.IsKeyDown(k)) && ticksSinceLastFire == -1)
            {
                GameManager.AddObject("MissileObject", CurrentX, CurrentY - 1);
                ObjectDisplay.TextureIndex = 1;
                ticksSinceLastFire = 0;
            }
        }

        public override void Update()
        {
            if (ticksSinceLastFire >= 0)
            {
                ticksSinceLastFire++;
                if (ticksSinceLastFire > 2)
                {
                    ticksSinceLastFire = -1;
                    ObjectDisplay.TextureIndex = 0;
                }
            }
        }
    }
}
