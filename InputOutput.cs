using System;
using System.IO;

namespace Компилятор
{
    public struct TextPosition
    {
        public uint lineNumber;
        public byte charNumber;
        public TextPosition(uint ln = 0, byte c = 0)
        {
            lineNumber = ln;
            charNumber = c;
        }
    }

    public static class InputOutput
    {
        private static string[] _lines;
        private static int _currentLineIdx;
        private static int _currentCharPos;

        public static char Ch { get; private set; }
        public static TextPosition positionNow = new TextPosition(1, 0);
        public static bool IsEndOfFile { get; private set; } = true;

        public static void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл не найден: " + filePath);
            _lines = File.ReadAllLines(filePath);
            if (_lines.Length == 0)
            {
                IsEndOfFile = true;
                return;
            }
            _currentLineIdx = 0;
            _currentCharPos = -1;
            positionNow = new TextPosition(1, 0);
            IsEndOfFile = false;
            Ch = '\0';
        }

        public static void NextCh()
        {
            if (IsEndOfFile) return;

            _currentCharPos++;
            positionNow.charNumber = (byte)(_currentCharPos + 1);

            if (_currentCharPos >= _lines[_currentLineIdx].Length)
            {
                _currentLineIdx++;
                if (_currentLineIdx >= _lines.Length)
                {
                    IsEndOfFile = true;
                    Ch = '\0';
                    return;
                }
                _currentCharPos = -1;
                positionNow.lineNumber = (uint)(_currentLineIdx + 1);
                positionNow.charNumber = 0;
                NextCh();
                return;
            }

            Ch = _lines[_currentLineIdx][_currentCharPos];
        }

        public static string GetLine(int lineNumber) =>
            (lineNumber >= 1 && lineNumber <= _lines.Length) 
            ? _lines[lineNumber - 1] : "";
    }
}