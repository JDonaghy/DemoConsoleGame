using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGame.GameObjects;

namespace ConsoleGame
{
    public class GameManager
    {
        private int _numRows;
        private int _numColumns;
        private ObjectDisplay[,] charMap;
        private bool _proccessing = false;
        private Random _rand = new Random();
        private int _score;
        private int _livesLeft;

        public bool GameOver { get; set; }
        public int NumRows { get => _numRows; set => _numRows = value; }
        public ConsoleColor GameBackground => ConsoleColor.DarkBlue;
        public ConsoleColor GameForeground => ConsoleColor.Cyan;

        public int NumColumns { get => _numColumns; set => _numColumns = value; }
        
        public List<IBaseObject> objList { get; set; }
        private List<IBaseObject> _newObjects = new List<IBaseObject>();
        List<string> IndependentOjects = 
            new List<string> { "SineObject", "CosineObject", "SimpleObject" };

        public GameManager(int numRows, int numColumns)
        {
            objList = new List<IBaseObject>();
            NumColumns = numColumns;
            NumRows = numRows;
            charMap = new ObjectDisplay[numRows,numColumns];
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

        public IEnumerable<IBaseObject> GetObjectsAt(int x, int y)
        {
            return objList.Where(obj => 
                obj.CurrentX - 1 < x && obj.CurrentX + 1 > x &&
                obj.CurrentY - 1 < y && obj.CurrentY + 1 > y);
        }

        public void DrawObject(ObjectDisplay objectDisplay, int row, int col)
        {
            objectDisplay.Draw(row, col);
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
            _clearItems();
            GameOver = false;
            _score = 0;
            AddRandomObject();
            objList.Add(new CursorObject(this));
            _livesLeft = 2;
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
    }
}
