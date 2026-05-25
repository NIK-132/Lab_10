using System;

namespace Компилятор
{
    class LexicalAnalyzer
    {
        public const byte
            star = 21,
            slash = 60,
            equal = 16,
            comma = 20,
            semicolon = 14,
            colon = 5,
            point = 61,
            arrow = 62,
            leftpar = 9,
            rightpar = 4,
            lbracket = 11,
            rbracket = 12,
            flpar = 63,
            frpar = 64,
            later = 65,
            greater = 66,
            laterequal = 67,
            greaterequal = 68,
            latergreater = 69,
            plus = 70,
            minus = 71,
            lcomment = 72,
            rcomment = 73,
            assign = 51,
            twopoints = 74,
            ident = 2,
            floatc = 82,
            intc = 15,
            charc = 30,
            casesy = 31,
            elsesy = 32,
            filesy = 57,
            gotosy = 33,
            thensy = 52,
            typesy = 34,
            untilsy = 53,
            dosy = 54,
            withsy = 37,
            ifsy = 56,
            insy = 100,
            ofsy = 101,
            orsy = 102,
            tosy = 103,
            endsy = 104,
            varsy = 105,
            divsy = 106,
            andsy = 107,
            notsy = 108,
            forsy = 109,
            modsy = 110,
            nilsy = 111,
            setsy = 112,
            beginsy = 113,
            whilesy = 114,
            arraysy = 115,
            constsy = 116,
            labelsy = 117,
            downtosy = 118,
            packedsy = 119,
            recordsy = 120,
            repeatsy = 121,
            programsy = 122,
            functionsy = 123,
            procedurensy = 124;

        private byte _symbol;
        private TextPosition _tokenPos;
        private string _identName;
        private int _intValue;
        private float _floatValue;
        private char _charValue;

        private Keywords _keywords;

        public LexicalAnalyzer()
        {
            _keywords = new Keywords();
            _tokenPos = new TextPosition();
        }

        public byte Symbol => _symbol;
        public TextPosition TokenPosition => _tokenPos;
        public string IdentName => _identName;
        public int IntValue => _intValue;
        public float FloatValue => _floatValue;
        public char CharValue => _charValue;

        public byte NextSym()
        {
            while (InputOutput.Ch == ' ')
            {
                InputOutput.NextCh();
            }

            _tokenPos = InputOutput.PositionNow;

            if (InputOutput.EndOfFile)
            {
                _symbol = 0;
                return 0;
            }

            char ch = InputOutput.Ch;

            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
            {
                string name = "";
                while ((InputOutput.Ch >= 'a' && InputOutput.Ch <= 'z') ||
                       (InputOutput.Ch >= 'A' && InputOutput.Ch <= 'Z') ||
                       (InputOutput.Ch >= '0' && InputOutput.Ch <= '9'))
                {
                    name += InputOutput.Ch;
                    InputOutput.NextCh();
                }
                string lower = name.ToLower();
                int len = lower.Length;
                if (len >= 2 && len <= 9 &&
                    _keywords.KeywordsTable.ContainsKey((byte)len) &&
                    _keywords.KeywordsTable[(byte)len].ContainsKey(lower))
                {
                    _symbol = _keywords.KeywordsTable[(byte)len][lower];
                }
                else
                {
                    _symbol = ident;
                    _identName = name;
                }
                return _symbol;
            }

            if (ch >= '0' && ch <= '9')
            {
                TextPosition startPos = _tokenPos;
                int maxint = 32767;
                _intValue = 0;
                while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                {
                    byte digit = (byte)(InputOutput.Ch - '0');
                    if (_intValue < maxint / 10 ||
                        (_intValue == maxint / 10 && digit <= maxint % 10))
                    {
                        _intValue = 10 * _intValue + digit;
                    }
                    else
                    {
                        InputOutput.Error(203, startPos);
                        _intValue = 0;
                        while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                            InputOutput.NextCh();
                        _symbol = intc;
                        return _symbol;
                    }
                    InputOutput.NextCh();
                }
                if (InputOutput.Ch == '.')
                {
                    TextPosition dotPos = InputOutput.PositionNow;
                    InputOutput.NextCh();
                    if (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                    {
                        float fraction = 0;
                        float divisor = 1;
                        while (InputOutput.Ch >= '0' && InputOutput.Ch <= '9')
                        {
                            fraction = fraction * 10 + (InputOutput.Ch - '0');
                            divisor *= 10;
                            InputOutput.NextCh();
                        }
                        _floatValue = _intValue + fraction / divisor;
                        _symbol = floatc;
                    }
                    else
                    {
                        InputOutput.Error(147, dotPos);
                        _symbol = intc;
                    }
                    return _symbol;
                }
                _symbol = intc;
                return _symbol;
            }

            if (ch == '\'')
            {
                TextPosition startPos = _tokenPos;
                InputOutput.NextCh(); 

                if (InputOutput.EndOfFile)
                {
                    InputOutput.Error(100, startPos);
                    _symbol = ident;
                    return _symbol;
                }

                if (InputOutput.Ch == '\'')
                {
                    InputOutput.Error(100, startPos);
                    InputOutput.NextCh();
                    _symbol = ident;
                    return _symbol;
                }

                _charValue = InputOutput.Ch;
                InputOutput.NextCh();

                if (InputOutput.EndOfFile || InputOutput.Ch != '\'')
                {
                    InputOutput.Error(100, startPos);
                    while (!InputOutput.EndOfFile &&
                           InputOutput.Ch != ' ' && InputOutput.Ch != ';' &&
                           InputOutput.Ch != ':' && InputOutput.Ch != '=' &&
                           InputOutput.Ch != ',' && InputOutput.Ch != '(' &&
                           InputOutput.Ch != ')' && InputOutput.Ch != '\n' &&
                           InputOutput.Ch != '\r')
                    {
                        InputOutput.NextCh();
                    }
                    _symbol = ident;
                    return _symbol;
                }

                InputOutput.NextCh();

                if (!InputOutput.EndOfFile && 
                    (char.IsLetterOrDigit(InputOutput.Ch) || 
                    InputOutput.Ch == '\''))
                {
                    InputOutput.Error(100, startPos);
                    while (!InputOutput.EndOfFile &&
                           (char.IsLetterOrDigit(InputOutput.Ch) || 
                           InputOutput.Ch == '\'' || 
                           InputOutput.Ch == ' '))
                    {
                        InputOutput.NextCh();
                    }
                    _symbol = ident;
                    return _symbol;
                }

                _symbol = charc;
                return _symbol;
            }

            switch (ch)
            {
                case '<':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '=')
                    {
                        _symbol = laterequal;
                        InputOutput.NextCh();
                    }
                    else if (InputOutput.Ch == '>')
                    {
                        _symbol = latergreater;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = later;
                    break;
                case '>':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '=')
                    {
                        _symbol = greaterequal;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = greater;
                    break;
                case ':':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '=')
                    {
                        _symbol = assign;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = colon;
                    break;
                case '=':
                    _symbol = equal;
                    InputOutput.NextCh();
                    break;
                case '+':
                    _symbol = plus;
                    InputOutput.NextCh();
                    break;
                case '-':
                    _symbol = minus;
                    InputOutput.NextCh();
                    break;
                case '*':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == ')')
                    {
                        _symbol = rcomment;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = star;
                    break;
                case '/':
                    _symbol = slash;
                    InputOutput.NextCh();
                    break;
                case '(':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '*')
                    {
                        _symbol = lcomment;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = leftpar;
                    break;
                case ')':
                    _symbol = rightpar;
                    InputOutput.NextCh();
                    break;
                case ';':
                    _symbol = semicolon;
                    InputOutput.NextCh();
                    break;
                case '.':
                    InputOutput.NextCh();
                    if (InputOutput.Ch == '.')
                    {
                        _symbol = twopoints;
                        InputOutput.NextCh();
                    }
                    else
                        _symbol = point;
                    break;
                case ',':
                    _symbol = comma;
                    InputOutput.NextCh();
                    break;
                case '[':
                    _symbol = lbracket;
                    InputOutput.NextCh();
                    break;
                case ']':
                    _symbol = rbracket;
                    InputOutput.NextCh();
                    break;
                case '{':
                    _symbol = flpar;
                    InputOutput.NextCh();
                    break;
                case '}':
                    _symbol = frpar;
                    InputOutput.NextCh();
                    break;
                case '^':
                    _symbol = arrow;
                    InputOutput.NextCh();
                    break;
                default:
                    InputOutput.Error(100, InputOutput.PositionNow);
                    InputOutput.NextCh();
                    _symbol = ident;
                    break;
            }
            return _symbol;
        }
    }
}