using System;
using System.Collections.Generic;

namespace ConsoleGame.GameObjects
{
    public interface IBaseObject:  IDisposable
    {
        ObjectDisplay ObjectDisplay { get; set; }
        List<string> CanDestroy { get; }
        bool CanBeDestroyed { get; }
        int Points { get; }
        List<ConsoleKey> RegisteredKeys { get; }
        bool IsDead { get; set; }
        int CurrentX { get; }
        int CurrentY { get; }
        int CurrentWidth { get; }
        int CurrentHeight { get; }

        void Erase();
        void Update();
        void Draw();
        void Process(ulong counter);
        void ProcessKey(ConsoleKeyInfo keyInfo);
    }
}
