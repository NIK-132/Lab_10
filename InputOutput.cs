using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    class InputOutput
    {
        const byte ERRMAX = 9;

        private static string _line;
        private static byte _lastInLine;
        private static StreamReader _file;
        private static uint _errCount;
        private static TextPosition _positionNow;
        private static List<Err> _errors;

        private static char _ch;
        private static bool _endOfFile;
        private static bool _finished;

        static InputOutput()
        {
            _lastInLine = 0;
            _errCount = 0;
            _positionNow = new TextPosition();
            _errors = new List<Err>();
            _finished = false;
        }

        public static char Ch
        {
            get
            {
                return _ch;
            }
            private set
            {
                _ch = value;
            }
        }

        public static TextPosition PositionNow
        {
            get
            {
                return _positionNow;
            }
            private set
            {
                _positionNow = value;
            }
        }

        public static bool EndOfFile
        {
            get
            {
                return _endOfFile;
            }
            private set
            {
                _endOfFile = value;
            }
        }

        public static List<Err> Errors
        {
            get
            {
                return _errors;
            }
        }
        public static byte LastInLine => _lastInLine;

        public static void Initialize(string filePath)
        {
            _file = new StreamReader(filePath, System.Text.Encoding.Default);
            ReadNextLine();
            if (!EndOfFile)
            {
                _positionNow = new TextPosition(1, 0);
                Ch = _line[0];
            }
        }

        public static void NextCh()
        {
            if (EndOfFile)
            {
                return;
            }

            if (_positionNow.CharNumber == _lastInLine)
            {
                ListThisLine();
                if (_errors.Count > 0)
                {
                    ListErrors();
                }
                ReadNextLine();
                if (EndOfFile)
                {
                    return;
                }
                else
                {
                    _positionNow =
                        new TextPosition(_positionNow.LineNumber + 1, 0);
                    Ch = _line[0];
                }
            }
            else
            {
                _positionNow = new TextPosition(_positionNow.LineNumber,
                    (byte)(_positionNow.CharNumber + 1));
                Ch = _line[_positionNow.CharNumber];
            }
        }

        public static void Error(byte errorCode, TextPosition position)
        {
            if (_errors.Count <= ERRMAX)
            {
                _errors.Add(new Err(position, errorCode));
            }
        }

        public static void FlushErrors()
        {
            if (_errors.Count > 0)
            {
                ListErrors();
                _errors.Clear();
            }
        }

        public static void Finish()
        {
            if (!_finished)
            {
                if (_file != null)
                {
                    _file.Close();
                    Console.WriteLine($"\nКомпиляция окончена: " +
                        $"ошибок — {_errCount}!");
                    _finished = true;
                }
            }
        }

        static void ListThisLine()
        {
            Console.WriteLine($"{_positionNow.LineNumber,4}  {_line}");
        }

        static void ReadNextLine()
        {
            while (!_file.EndOfStream)
            {
                _line = _file.ReadLine();
                if (_line == null)
                {
                    break;
                }
                if (_line.Length > 0)
                {
                    _lastInLine = (byte)(_line.Length - 1);
                    _errors = new List<Err>();
                    return;
                }
                Console.WriteLine();
            }
            EndOfFile = true;
            _line = string.Empty;
            _lastInLine = 0;
        }

        static void ListErrors()
        {
            string marker;
            string pointerLine;
            string desc;
            int spaces;
            foreach (Err item in _errors)
            {
                _errCount++;
                marker = "**" + (_errCount < 10 ? "0" : "") + _errCount + "**";
                spaces = item.ErrorPosition.CharNumber;
                pointerLine = marker + new string(' ', spaces)
                    + "^ ошибка код " + item.ErrorCode;
                Console.WriteLine(pointerLine);
                desc = ErrorTable.GetDescription(item.ErrorCode);
                if (!string.IsNullOrEmpty(desc))
                {
                    Console.WriteLine(new string(' ',
                        marker.Length + spaces + 1) + desc);
                }
            }
        }
    }
}