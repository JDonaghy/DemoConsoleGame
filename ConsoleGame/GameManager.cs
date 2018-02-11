using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGame.GameObjects;

namespace ConsoleGame
{
    struct Rect
    {
        public int x;
        public int y;
        public int width;
        public int height;
    };

    public class GameManager
    {
        private int _numRows = 29;
        private int _numColumns = 118;
        private ObjectDisplay[,] charMap;
        private bool _proccessing = false;
        private Random _rand = new Random();
        private int _score;
        private int _livesLeft;

        public bool GameOver { get; set; }
        public ConsoleColor GameBackground => ConsoleColor.DarkBlue;
        public ConsoleColor GameForeground => ConsoleColor.Cyan;
        public int NumRows { get => _numRows; set => _numRows = value; }
        public int NumColumns { get => _numColumns; set => _numColumns = value; }
        
        public List<IBaseObject> objList { get; set; }
        private List<IBaseObject> _newObjects = new List<IBaseObject>();
        List<string> IndependentOjects = 
            new List<string> { "SineObject", "CosineObject", "SimpleObject" };

        public GameManager()
        {
            objList = new List<IBaseObject>();
            charMap = new ObjectDisplay[_numRows,_numColumns];
            GameOver = false;
            _clearConsole();
            _writeStatus();
        }

        public void AddObject(string type, int x, int y)
        {
            var listToAddTo = _proccessing ? _newObjects : objList;
            switch (type)
            {
                case "SineObject":
                    listToAddTo.Add(new SineObject(this, x, y));
                    break;
                case "CosineObject":
                    listToAddTo.Add(new CosineObject(this, x, y));
                    break;
                case "SimpleObject":
                    listToAddTo.Add(new SimpleObject(this, x, y));
                    break;
                case "MissileObject":
                    listToAddTo.Add(new MissileObject(this, x, y));
                    break;
                case "GameOverObject":
                    listToAddTo.Add(new GameOverObject(this, x, y));
                    break;
                case "ExplosionObject":
                    _newObjects.Add( new ExplosionObject(this, x, y));
                    break;
            }
        }

        public void AddRandomObject()
        {
            AddObject(IndependentOjects[
                _rand.Next(0, 3)], _rand.Next(0, _numColumns), _rand.Next(0, _numRows));
        }

        public IEnumerable<IBaseObject> GetObjectsIntersecting(int x, int y, int width, int height)
        {
            var result = new List<IBaseObject>();
            foreach (var obj in objList)
            {
                Rect r1 = new Rect()
                {
                    x = x,
                    y = y, width = 
                    width,
                    height = height
                };
                Rect r2 = new Rect()
                {
                    x = obj.CurrentX,
                    y = obj.CurrentY,
                    width = obj.CurrentWidth,
                    height = obj.CurrentHeight
                };
                
                if (_rectanglesOverlap(r1, r2))
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        public bool ProcessKeys()
        {
            var result = true;
            while (Console.KeyAvailable)
            {
                var keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    result = false;
                }
                else
                {
                    var firstObj = objList.FirstOrDefault(x => x.RegisteredKeys.Contains(keyInfo.Key));
                    if (firstObj != null)
                    {
                        firstObj.Erase();
                        firstObj.ProcessKey(keyInfo);
                        firstObj.Draw();
                    }
                }
            }
            return result;
        }

        public void InitGame()
        {
            _score = 0;
            _livesLeft = 2;
            GameOver = false;
            _clearItems();
            AddRandomObject();
            objList.Add(new CursorObject(this));
        }

        public void ProcessObjects(ulong counter)
        {
            foreach (var obj in objList)
            {
                obj.Process(counter);
            }
            if (!GameOver)
            {
                if (!objList.Any(x => x.GetType().ToString() == "ConsoleGame.CursorObject"))
                {
                    if (_livesLeft > 0)
                    {
                        _livesLeft--;
                        objList.Add(new CursorObject(this));
                    }
                    else
                    {
                        EndGame();
                    }
                }
                else
                {
                    _proccessing = true;
                    foreach (var obj in objList.Where(x => x.IsDead))
                    {
                        if (obj.Points != 0)
                        {
                            _score += obj.Points;
                        }
                        if (obj.CanBeDestroyed)
                        {
                            AddObject("ExplosionObject", obj.CurrentX, obj.CurrentY);
                        }
                        obj.Dispose();
                    }
                    objList.RemoveAll(x => x.IsDead);
                    objList.AddRange(_newObjects);
                    _newObjects.Clear();
                    _proccessing = false;
                }
            }
            else
            {
                if (objList.Any(x => x.IsDead))
                {
                    InitGame();
                }
            }
            _writeStatus();
            if (counter % 100 == 0)
            {
                makeDemBuggers();
            }
        }

        public void Beep(int freq)
        {
            Console.Beep(freq, 100);
        }

        public void EndGame()
        {
            _clearItems();
            _clearConsole();
            _writeStatus();
            AddObject("GameOverObject", NumColumns / 2 - 8, 5);
            GameOver = true;
        }

        public void WriteText(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
        }

        //
        // Source: https://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
        //
        private bool _valueInRange(int value, int min, int max)
        {
            return (value >= min) && (value <= max);
        }

        private bool _rectanglesOverlap(Rect A, Rect B)
        {
            bool xOverlap = _valueInRange(A.x, B.x, B.x + B.width-1) ||
                            _valueInRange(B.x, A.x, A.x + A.width-1);

            bool yOverlap = _valueInRange(A.y, B.y, B.y + B.height-1) ||
                            _valueInRange(B.y, A.y, A.y + A.height-1);

            return xOverlap && yOverlap;
        }
        //
        // End source
        //

        private void _writeStatus()
        {
            WriteText(NumColumns - 10, 1, $"{_score.ToString("D5")}     ");
            for (int i=0;i<_livesLeft;i++)
            {
                WriteText(NumColumns - 1 -i, 1, "^");
            }
        }
        
        private void _clearItems()
        {
            foreach(var obj in objList)
            {
                obj.Dispose();
            }
            objList.Clear();
        }

        private void _clearConsole()
        {
            Console.CursorVisible = false;
            Console.ForegroundColor = GameForeground;
            Console.BackgroundColor = GameBackground;
            Console.Clear();
        }

        private void makeDemBuggers()
        {
            var destroyableObjectCount = objList.Where(
                obj => obj.CanBeDestroyed).ToList().Count;

            if (!GameOver && destroyableObjectCount < 5)
            {
                AddRandomObject();
            }
        }
    }
}
