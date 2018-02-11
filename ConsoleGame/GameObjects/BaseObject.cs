using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGame.GameObjects;

namespace ConsoleGame
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
        protected List<ConsoleKey> KeyList = new List<ConsoleKey>();
        protected ulong SlowFactor { get; set; }


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

        public virtual bool CanBeDestroyed
        {
            get { return canBeDestroyed; }
        }
        public virtual bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }
        public virtual int Points
        {
            get { return points; }
        }
        public virtual List<string> CanDestroy => CanDestroyList;
        public virtual List<ConsoleKey> RegisteredKeys => KeyList;

        public virtual void ProcessKey(ConsoleKeyInfo keyInfo)
        {
        }

        public virtual void Erase()
        {
            ObjectDisplay.Erase(CurrentY, CurrentX);
        }

        public virtual void Draw()
        {
            if (!IsDead)
            {
                var dimensions = ObjectDisplay.Draw(CurrentY, CurrentX);
                CurrentWidth = dimensions.Item1;
                CurrentHeight = dimensions.Item2;
            }
        }

        public virtual void Process(ulong counter)
        {
            CheckAlive();
            if (!isDead && counter % SlowFactor == 0)
            {
                Erase();
                Update();
                Draw();
            }
        }
        public abstract void Update();

        protected void CheckAlive()
        {
            foreach (var otherObjAtMyPosition in GameManager.GetObjectsIntersecting(
                CurrentX, CurrentY, CurrentWidth, CurrentHeight))
            {
                var myType = GetType().ToString().Split('.')[1];
                if (otherObjAtMyPosition.CanDestroy.Contains(myType))
                {
                    isDead = true;
                    Erase();
                    GameManager.Beep(300);
                    otherObjAtMyPosition.IsDead = true;
                }
            }
        }

        public void Dispose()
        {
            Erase();
        }
    }
}
