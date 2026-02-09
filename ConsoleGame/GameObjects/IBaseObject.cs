using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleGame.GameObjects
{
    public interface IBaseObject:  IDisposable
    {
        ObjectDisplay ObjectDisplay { get; set; }
        List<string> CanDestroy { get; }
        bool CanBeDestroyed { get; }
        int Points { get; }
        List<Keys> RegisteredKeys { get; }
        bool IsDead { get; set; }
        int CurrentX { get; }
        int CurrentY { get; }
        int CurrentWidth { get; }
        int CurrentHeight { get; }

        void Update();
        void Draw(SpriteBatch spriteBatch, float scale);
        void Process(GameTime gameTime);
        void ProcessInput(KeyboardState keyState);
    }
}
