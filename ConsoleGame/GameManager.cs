using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGame.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleGame
{
    public struct Rect // Changed to public so it can be used if needed, though MonoGame has Rectangle
    {
        public int x;
        public int y;
        public int width;
        public int height;
    };

    public class GameManager
    {
        private int _numRows = 45; // Approx 720 / 16
        private int _numColumns = 80; // Approx 1280 / 16
        private bool _proccessing = false;
        private Random _rand = new Random();
        private int _score;
        private int _livesLeft;

        public bool GameOver { get; set; }
        public Color GameBackground => Color.DarkBlue;
        public Color GameForeground => Color.Cyan;
        public int NumRows { get => _numRows; set => _numRows = value; }
        public int NumColumns { get => _numColumns; set => _numColumns = value; }
        
        public List<IBaseObject> objList { get; set; }
        private List<IBaseObject> _newObjects = new List<IBaseObject>();
        List<string> IndependentOjects = 
            new List<string> { "SineObject", "CosineObject", "SimpleObject" };

        public Dictionary<string, Texture2D> Textures { get; set; } = new Dictionary<string, Texture2D>();

        public GameManager()
        {
            objList = new List<IBaseObject>();
            GameOver = false;
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
            Rect areaToCheck = new Rect()
            {
                x = x,
                y = y, 
                width = width,
                height = height
            };
            foreach (var obj in objList)
            {
                Rect objRectangle = new Rect()
                {
                    x = obj.CurrentX,
                    y = obj.CurrentY,
                    width = obj.CurrentWidth,
                    height = obj.CurrentHeight
                };
                
                if (_rectanglesOverlap(areaToCheck, objRectangle))
                {
                    result.Add(obj);
                }
            }
            return result;
        }

        public bool ProcessInput()
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape))
            {
                return false;
            }
            
            // Allow objects to process input
            // Iterate over a copy to allow modification of the collection (e.g. adding missiles)
            var currentObjects = objList.ToList();
            foreach(var obj in currentObjects)
            {
                obj.ProcessInput(state);
            }
            return true;
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

        public void ProcessObjects(GameTime gameTime)
        {
            foreach (var obj in objList)
            {
                obj.Process(gameTime);
            }
            if (!GameOver)
            {
                if (!objList.Any(x => x.GetType().ToString().EndsWith("CursorObject")))
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
            
            if (_score > 0 && (int)gameTime.TotalGameTime.TotalMilliseconds % 2000 < 20) // Roughly every 2 seconds
            {
                 // makeDemBuggers logic needs to be called
                 makeDemBuggers();
            }
            // Actually original logic was counter % 100 == 0. 
            // 100 * 10ms = 1000ms = 1 second.
            // So I should call it periodically.
        }
        
        // Helper to run periodically
        private double _timeSinceLastSpawn;
        public void Update(GameTime gameTime)
        {
             _timeSinceLastSpawn += gameTime.ElapsedGameTime.TotalMilliseconds;
             if (_timeSinceLastSpawn > 1000)
             {
                 makeDemBuggers();
                 _timeSinceLastSpawn = 0;
             }
             ProcessObjects(gameTime);
        }

        public void Beep(int freq)
        {
            // Optional: Play sound
        }

        public void EndGame()
        {
            _clearItems();
            AddObject("GameOverObject", NumColumns / 2 - 8, 5);
            GameOver = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
             // Draw background
             if (Textures.ContainsKey("background"))
             {
                 spriteBatch.Draw(Textures["background"], new Rectangle(0, 0, 1280, 720), Color.White);
             }

             foreach(var obj in objList)
             {
                 obj.Draw(spriteBatch, 1.0f);
             }
             
             // Draw score
             string scoreStr = _score.ToString();
             int x = 10;
             int y = 10;
             foreach(char c in scoreStr)
             {
                 if (char.IsDigit(c))
                 {
                     var tex = Textures[$"num_{c}"];
                     spriteBatch.Draw(tex, new Vector2(x, y), Color.White);
                     x += tex.Width + 2;
                 }
             }

             // Draw lives
             var shipTex = Textures["cursor"];
             int livesX = 1280 - 40; // Top right
             for (int i = 0; i < _livesLeft; i++)
             {
                 spriteBatch.Draw(shipTex, new Vector2(livesX, 10), null, Color.White, 0f, Vector2.Zero, 0.7f, SpriteEffects.None, 0f); // Draw slightly smaller
                 livesX -= (int)(shipTex.Width * 0.7f) + 5;
             }
        }

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

        private void _clearItems()
        {
            foreach(var obj in objList)
            {
                obj.Dispose();
            }
            objList.Clear();
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
