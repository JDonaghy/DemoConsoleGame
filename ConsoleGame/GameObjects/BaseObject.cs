using System;
using System.Collections.Generic;
using ConsoleGame.GameObjects;

namespace ConsoleGame
{
    public abstract class BaseObject : IBaseObject
    {
        public int CurrentY { get; protected set; }
        public int CurrentX { get; protected set; }
        protected ObjectDisplay ObjectDisplay { get; set; }
        protected int points = 0;
        protected bool isDead = false;
        protected bool canBeDestroyed = false;
        protected GameManager GameManager;
        protected List<string> CanDestroyList = new List<string>();
        protected List<ConsoleKey> KeyList = new List<ConsoleKey>();
        private ObjectDisplay eraseObj = null;
        protected ulong SlowFactor { get; set; }


        public BaseObject(GameManager gameManager, int x, int y)
        {
            GameManager = gameManager;
            CurrentY = y;
            CurrentX = x;
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
            if (eraseObj != null)
            {
                GameManager.DrawObject(eraseObj, CurrentY, CurrentX);
            }
        }

        public virtual void Draw()
        {
            if (eraseObj == null)
            {
                eraseObj = new ObjectDisplay
                {
                    BackgroundColor = GameManager.GameBackground,
                    ForegroundColor = GameManager.GameForeground,
                    CharMap = new char[ObjectDisplay.CharMap.GetLength(0), ObjectDisplay.CharMap.GetLength(1)]
                };
                for (var i = 0; i < eraseObj.CharMap.GetLength(0); i++)
                    for (var j = 0; j < eraseObj.CharMap.GetLength(1); j++)
                        eraseObj.CharMap[i, j] = ' ';
            }
            if (!IsDead)
            {
                GameManager.DrawObject(ObjectDisplay, CurrentY, CurrentX);
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
            foreach (var otherObjAtMyPosition in GameManager.GetObjectsAt(CurrentX, CurrentY))
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
