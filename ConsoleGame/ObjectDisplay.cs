using System;
using System.Collections.Generic;

namespace ConsoleGame
{
    public class ObjectDisplay
    {
        private int _charMapsIndex=0;
        private List<char[,]> _eraseMaps = new List<char[,]>();
        private List<char[,]> _charMaps;
        public List<char[,]> CharMaps
        {
            get
            {
                return _charMaps;
            }
            set
            {
                _charMaps = value;
                _eraseMaps.Clear();
                foreach (var charMap in _charMaps)
                {
                    var eraseMap = new char[charMap.GetLength(0), charMap.GetLength(1)];
                    for (var i = 0; i < eraseMap.GetLength(0); i++)
                        for (var j = 0; j < eraseMap.GetLength(1); j++)
                            eraseMap[i, j] = ' ';
                    _eraseMaps.Add(eraseMap);
                }

            }
        }
        public char[,] CurrentCharMap
        {
            get
            {
                return _charMaps[_charMapsIndex];
            }
        }

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }
        public ConsoleColor GameForegroundColor { get; set; }
        public ConsoleColor GameBackgroundColor { get; set; }

        private ObjectDisplay()
        {
        }

        public ObjectDisplay(
            List<char[,]> charMaps, ConsoleColor fg, ConsoleColor bg, 
            ConsoleColor gameFg, ConsoleColor gameBg)
        {
            CharMaps = charMaps;
            ForegroundColor = fg;
            BackgroundColor = bg;
            GameForegroundColor = gameFg;
            GameBackgroundColor = gameBg;
        }

        public void Draw(int row, int col)
        {
            Console.ForegroundColor = ForegroundColor;
            Console.BackgroundColor = BackgroundColor;
            _drawImage(row, col, CharMaps[_charMapsIndex]);
            _charMapsIndex++;
            if (_charMapsIndex > CharMaps.Count - 1)
            {
                _charMapsIndex = 0;
            }
        }

        public void Erase(int row, int col)
        {
            Console.ForegroundColor = GameForegroundColor;
            Console.BackgroundColor = GameBackgroundColor;
            _drawImage(row, col, _eraseMaps[_charMapsIndex]);
        }

        private void _drawImage(int row, int col, char[,] img)
        {
            if (row < 0 || col < 0)
                return;

            for (var i = 0; i < img.GetLength(0); i++)
            {
                Console.SetCursorPosition(col, row + i);
                for (var j = 0; j < img.GetLength(1); j++)
                {
                    Console.Write(img[i, j]);
                }
            }
        }
    }
}
