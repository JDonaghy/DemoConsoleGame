using System;
using System.Collections.Generic;

namespace ConsoleGame.GameObjects
{
    public interface IBaseObject:  IDisposable
    {
        List<string> CanDestroy { get; }
        bool CanBeDestroyed { get; }
        int Points { get; }
        List<ConsoleKey> RegisteredKeys { get; }
        bool IsDead { get; set; }
        int CurrentX { get; }
        int CurrentY { get; }

        void Erase();
        void Update();
        void Draw();
        void Process(ulong counter);
        void ProcessKey(ConsoleKeyInfo keyInfo);
    }
}
