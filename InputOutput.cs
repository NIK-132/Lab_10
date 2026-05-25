using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    public static class InputOutput
    {
        private const byte MAX_ERRORS = 9;

        private static string[]? _sourceLines;
        private static int _currentLine;
        private static int _currentPos;

        private static uint _globalErrorCount;
        private static List<Err> _lineErrors;

        private static TextPosition _pos;
        private static char _ch;
        private static bool _endOfFile;

        public static char Ch
        {
            get 
            { 
                return _ch; 
            }
        }

        public static TextPosition PositionNow
        {
            get 
            { 
                return _pos; 
            }
        }

        public static bool EndOfFile
        {
            get { return _endOfFile; }
        }

        public static List<Err> Errors
        {
            get { return _lineErrors; }
        }

        static InputOutput()
        {
            _lineErrors = new List<Err>();
            _pos = new TextPosition(1, 0);
            _sourceLines = Array.Empty<string>();
        }

        public static void LoadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Файл не найден: " + filePath);
            }

            _sourceLines = File.ReadAllLines(filePath);
            if (_sourceLines.Length == 0)
            {
                _endOfFile = true;
                return;
            }

            _currentLine = 0;
            _currentPos = -1;
            _globalErrorCount = 0;
            _lineErrors.Clear();
            _endOfFile = false;
            _pos = new TextPosition(1, 0);
            NextCh();
        }

        public static void NextCh()
        {
            if (_endOfFile)
            {
                return;
            }
            if (_sourceLines == null)
            {
                return;
            }

            _currentPos++;
            _pos.CharNumber = (byte)(_currentPos + 1);

            if (_currentPos >= _sourceLines[_currentLine].Length)
            {
                PrintCurrentLine();
                if (_lineErrors.Count > 0)
                    PrintLineErrors();

                _currentLine++;
                if (_currentLine >= _sourceLines.Length)
                {
                    _endOfFile = true;
                    _ch = '\0';
                    FinalizeCompilation();
                    return;
                }

                _currentPos = -1;
                _pos.LineNumber = (uint)(_currentLine + 1);
                _pos.CharNumber = 0;
                _lineErrors.Clear();
                NextCh();
                return;
            }

            _ch = _sourceLines[_currentLine][_currentPos];
        }

        public static void Error(byte code, TextPosition position)
        {
            if (_lineErrors.Count < MAX_ERRORS)
            {
                _lineErrors.Add(new Err(position, code));
            }
        }

        private static void PrintCurrentLine()
        {
            if (_sourceLines != null && _currentLine < _sourceLines.Length)
            {
                Console.WriteLine($"{_pos.LineNumber,4}  " +
                    $"{_sourceLines[_currentLine]}");
            }
        }

        private static void PrintLineErrors()
        {
            const int lineNumberWidth = 4;
            const int spaceAfterNumber = 1;
            int indentBase = lineNumberWidth + spaceAfterNumber;

            foreach (var err in _lineErrors)
            {
                _globalErrorCount++;
                string marker = "**" + (_globalErrorCount < 10 ? "0" : "") 
                    + _globalErrorCount + "**";
                int column = err.ErrorPosition.CharNumber - 1;
                if (column < 1) column = 1;
                int totalIndent = indentBase + column + 1;

                string arrowLine = new string(' ', totalIndent - marker.Length)
                    + "^ ошибка код " + err.ErrorCode;
                Console.WriteLine(marker + arrowLine);

                string desc = ErrorTable.GetDescription(err.ErrorCode);
                if (!string.IsNullOrEmpty(desc))
                {
                    Console.WriteLine(new string(' ', totalIndent + 1) + desc);
                }
            }
        }

        private static void FinalizeCompilation()
        {
            Console.WriteLine($"\nКомпиляция окончена: " +
                $"ошибок — {_globalErrorCount}!");
        }
    }
}