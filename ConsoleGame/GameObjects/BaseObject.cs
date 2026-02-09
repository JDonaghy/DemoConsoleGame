using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace ConsoleGame.GameObjects
{
    public abstract class BaseObject : IBaseObject
    {
        public int CurrentY { get; protected set; }
        public int CurrentX { get; protected set; }
        public int CurrentWidth { get; protected set; }
        public int CurrentHeight { get; protected set; }
        public ObjectDisplay ObjectDisplay { get; set; }
        protected int points = 0;
        protected bool isDead = false;
        protected bool canBeDestroyed = false;
        protected GameManager GameManager;
        protected List<string> CanDestroyList = new List<string>();
        protected List<Keys> KeyList = new List<Keys>();
        protected double UpdateInterval { get; set; } = 100;
        protected double TimeSinceLastUpdate { get; set; }

        // Backward compatibility wrapper for SlowFactor
        protected ulong SlowFactor 
        { 
            set { UpdateInterval = value * 10; }
        }

        public BaseObject(GameManager gameManager, int x, int y)
        {
            GameManager = gameManager;
            CurrentY = y;
            CurrentX = x;
            CurrentWidth = 1;
            CurrentHeight = 1;
            CanDestroyList.Add("CursorObject");
            SlowFactor = 10;
        }

        public virtual bool CanBeDestroyed => canBeDestroyed;
        public virtual bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }
        public virtual int Points => points;
        public virtual List<string> CanDestroy => CanDestroyList;
        public virtual List<Keys> RegisteredKeys => KeyList;

        public virtual void ProcessInput(KeyboardState keyState)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch, float scale)
        {
            if (!IsDead)
            {
                // We assume 16px is the standard "cell size" for rendering purposes at scale 1.0
                int cellSize = 16;
                Vector2 position = new Vector2(CurrentX * cellSize * scale, CurrentY * cellSize * scale);
                
                // Pass scale to object display
                var size = ObjectDisplay.Draw(spriteBatch, position, scale);
                
                // Update logical width/height based on drawn size
                // If sprite is larger than one cell, it occupies multiple logical cells
                CurrentWidth = (int)Math.Max(1, Math.Round(size.X / (cellSize * scale)));
                CurrentHeight = (int)Math.Max(1, Math.Round(size.Y / (cellSize * scale)));
            }
        }

        public virtual void Process(GameTime gameTime)
        {
            CheckAlive();
            if (!isDead)
            {
                TimeSinceLastUpdate += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (TimeSinceLastUpdate >= UpdateInterval)
                {
                    Update();
                    TimeSinceLastUpdate = 0;
                }
            }
        }
        public abstract void Update();

        protected void CheckAlive()
        {
            var intersectingObjects = GameManager.GetObjectsIntersecting(
                CurrentX, CurrentY, CurrentWidth, CurrentHeight).ToList();

            foreach (var otherObjAtMyPosition in intersectingObjects)
            {
                if (otherObjAtMyPosition == this) continue;

                // Logic: ConsoleGame.MissileObject -> MissileObject
                var typeName = GetType().ToString();
                var parts = typeName.Split('.');
                var myType = parts.Length > 1 ? parts[parts.Length - 1] : parts[0];

                if (otherObjAtMyPosition.CanDestroy.Contains(myType))
                {
                    isDead = true;
                    // Erase() is gone
                    GameManager.Beep(300); // We'll stub this in GameManager
                    otherObjAtMyPosition.IsDead = true;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}
