using System;

namespace Компилятор
{
    class LexicalAnalyzer
    {
        private const byte star = 21;
        private const byte slash = 60;
        private const byte equal = 16;
        private const byte comma = 20;
        private const byte semicolon = 14;
        private const byte colon = 5;
        private const byte point = 61;
        private const byte arrow = 62;
        private const byte leftpar = 9;
        private const byte rightpar = 4;
        private const byte lbracket = 11;
        private const byte rbracket = 12;
        private const byte flpar = 63;
        private const byte frpar = 64;
        private const byte later = 65;
        private const byte greater = 66;
        private const byte laterequal = 67;
        private const byte greaterequal = 68;
        private const byte latergreater = 69;
        private const byte plus = 70;
        private const byte minus = 71;
        private const byte assign = 51;
        private const byte twopoints = 74;
        private const byte ident = 2;
        private const byte intc = 15;
        private const byte floatc = 82;
        private const byte charc = 80;

        private const byte casesy = 31;
        private const byte elsesy = 32;
        private const byte filesy = 57;
        private const byte gotosy = 33;
        private const byte thensy = 52;
        private const byte typesy = 34;
        private const byte untilsy = 53;
        private const byte dosy = 54;
        private const byte withsy = 37;
        private const byte ifsy = 56;
        private const byte insy = 100;
        private const byte ofsy = 101;
        private const byte orsy = 102;
        private const byte tosy = 103;
        private const byte endsy = 104;
        private const byte varsy = 105;
        private const byte divsy = 106;
        private const byte andsy = 107;
        private const byte notsy = 108;
        private const byte forsy = 109;
        private const byte modsy = 110;
        private const byte nilsy = 111;
        private const byte setsy = 112;
        private const byte beginsy = 113;
        private const byte whilesy = 114;
        private const byte arraysy = 115;
        private const byte constsy = 116;
        private const byte labelsy = 117;
        private const byte downtosy = 118;
        private const byte packedsy = 119;
        private const byte recordsy = 120;
        private const byte repeatsy = 121;
        private const byte programsy = 122;
        private const byte functionsy = 123;
        private const byte procedurensy = 124;

        public static byte Star => star;
        public static byte Slash => slash;
        public static byte Equal => equal;
        public static byte Comma => comma;
        public static byte Semicolon => semicolon;
        public static byte Colon => colon;
        public static byte Point => point;
        public static byte Arrow => arrow;
        public static byte Leftpar => leftpar;
        public static byte Rightpar => rightpar;
        public static byte Lbracket => lbracket;
        public static byte Rbracket => rbracket;
        public static byte Flpar => flpar;
        public static byte Frpar => frpar;
        public static byte Later => later;
        public static byte Greater => greater;
        public static byte Laterequal => laterequal;
        public static byte Greaterequal => greaterequal;
        public static byte Latergreater => latergreater;
        public static byte Plus => plus;
        public static byte Minus => minus;
        public static byte Assign => assign;
        public static byte Twopoints => twopoints;
        public static byte Ident => ident;
        public static byte Intc => intc;
        public static byte Floatc => floatc;
        public static byte Charc => charc;

        public static byte Casesy => casesy;
        public static byte Elsesy => elsesy;
        public static byte Filesy => filesy;
        public static byte Gotosy => gotosy;
        public static byte Thensy => thensy;
        public static byte Typesy => typesy;
        public static byte Untilsy => untilsy;
        public static byte Dosy => dosy;
        public static byte Withsy => withsy;
        public static byte Ifsy => ifsy;
        public static byte Insy => insy;
        public static byte Ofsy => ofsy;
        public static byte Orsy => orsy;
        public static byte Tosy => tosy;
        public static byte Endsy => endsy;
        public static byte Varsy => varsy;
        public static byte Divsy => divsy;
        public static byte Andsy => andsy;
        public static byte Notsy => notsy;
        public static byte Forsy => forsy;
        public static byte Modsy => modsy;
        public static byte Nilsy => nilsy;
        public static byte Setsy => setsy;
        public static byte Beginsy => beginsy;
        public static byte Whilesy => whilesy;
        public static byte Arraysy => arraysy;
        public static byte Constsy => constsy;
        public static byte Labelsy => labelsy;
        public static byte Downtosy => downtosy;
        public static byte Packedsy => packedsy;
        public static byte Recordsy => recordsy;
        public static byte Repeatsy => repeatsy;
        public static byte Programsy => programsy;
        public static byte Functionsy => functionsy;
        public static byte Procedurensy => procedurensy;

        private byte _symbol;
        private TextPosition _token;
        private string _addrName;
        private int _nmb_int;
        private float _nmb_float;
        private char _charValue;

        private static int _level = 0;
        private static TextPosition _unclosedParenPos =
            new TextPosition(0, 0);

        public LexicalAnalyzer()
        {
            _token = new TextPosition();
            _addrName = "";
            _nmb_int = 0;
            _nmb_float = 0;
            _charValue = '\0';
        }

        public byte Symbol
        {
            get
            {
                return _symbol;
            }
            set
            {
                _symbol = value;
            }
        }

        public TextPosition Token
        {
            get
            {
                return _token;
            }
            set
            {
                _token = value;
            }
        }

        public string AddrName
        {
            get
            {
                return _addrName;
            }
            set
            {
                _addrName = value;
            }
        }

        public int NmbInt
        {
            get
            {
                return _nmb_int;
            }
            set
            {
                _nmb_int = value;
            }
        }

        public float NmbFloat
        {
            get
            {
                return _nmb_float;
            }
            set
            {
                _nmb_float = value;
            }
        }

        public char CharValue
        {
            get
            {
                return _charValue;
            }
            set
            {
                _charValue = value;
            }
        }

        public static void CheckParenBalance()
        {
            if (_level > 0)
            {
                InputOutput.Error(105, _unclosedParenPos);
            }
        }

        public byte NextSym()
        {
            while (!InputOutput.EndOfFile && InputOutput.Ch == ' ')
            {
                InputOutput.NextCh();
            }

            if (InputOutput.EndOfFile)
            {
                return 0;
            }

            _token = InputOutput.PositionNow;
            char ch = InputOutput.Ch;

            if (char.IsLetter(ch) || ch == '_')
            {
                string name = "";
                while (!InputOutput.EndOfFile &&
                       (char.IsLetterOrDigit(InputOutput.Ch)
                       || InputOutput.Ch == '_'))
                {
                    name += InputOutput.Ch;
                    InputOutput.NextCh();
                }
                byte code = Keywords.GetCode(name.ToLower());
                if (code != 0)
                {
                    _symbol = code;
                }
                else
                {
                    _symbol = ident;
                    _addrName = name;
                }
                return _symbol;
            }

            if (char.IsDigit(ch))
            {
                const int maxint = 32767;
                _nmb_int = 0;
                bool overflow = false;
                TextPosition startPos = _token;

                while (!InputOutput.EndOfFile && char.IsDigit(InputOutput.Ch))
                {
                    int digit = InputOutput.Ch - '0';
                    if (_nmb_int > (maxint - digit) / 10)
                    {
                        InputOutput.Error(203, startPos);
                        overflow = true;
                        while (!InputOutput.EndOfFile && char.IsDigit(InputOutput.Ch))
                        {
                            InputOutput.NextCh();
                        }
                        _nmb_int = 0;
                        break;
                    }
                    _nmb_int = _nmb_int * 10 + digit;
                    InputOutput.NextCh();
                }

                if (!overflow && !InputOutput.EndOfFile
                    && InputOutput.Ch == '.')
                {
                    TextPosition dotPos = InputOutput.PositionNow;
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && char.IsDigit(InputOutput.Ch))
                    {
                        float fraction = 0;
                        float divisor = 1;
                        while (!InputOutput.EndOfFile
                            && char.IsDigit(InputOutput.Ch))
                        {
                            fraction = fraction * 10 + (InputOutput.Ch - '0');
                            divisor *= 10;
                            InputOutput.NextCh();
                        }
                        _nmb_float = _nmb_int + fraction / divisor;
                        _symbol = floatc;
                        return _symbol;
                    }
                    else
                    {
                        InputOutput.Error(147, dotPos);
                        _symbol = intc;
                        return _symbol;
                    }
                }
                _symbol = intc;
                return _symbol;
            }

            switch (ch)
            {
                case ',':
                    _symbol = comma;
                    InputOutput.NextCh();
                    break;
                case ';':
                    _symbol = semicolon;
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
                case '=':
                    _symbol = equal;
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
                case '^':
                    _symbol = arrow;
                    InputOutput.NextCh();
                    break;
                case '/':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '/')
                    {
                        while (!InputOutput.EndOfFile
                            && InputOutput.PositionNow.CharNumber
                            < InputOutput.LastInLine)
                        {
                            InputOutput.NextCh();
                        }
                        if (!InputOutput.EndOfFile)
                        {
                            InputOutput.NextCh();
                        }
                        return NextSym();
                    }
                    else
                    {
                        _symbol = slash;
                    }
                    break;
                case '*':
                    _symbol = star;
                    InputOutput.NextCh();
                    break;
                case '(':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '*')
                    {
                        InputOutput.NextCh();
                        while (!InputOutput.EndOfFile)
                        {
                            if (InputOutput.Ch == '*')
                            {
                                InputOutput.NextCh();
                                if (!InputOutput.EndOfFile
                                    && InputOutput.Ch == ')')
                                {
                                    InputOutput.NextCh();
                                    break;
                                }
                            }
                            else
                            {
                                InputOutput.NextCh();
                            }
                        }
                        return NextSym();
                    }
                    else
                    {
                        _symbol = leftpar;
                        if (_level == 0)
                        {
                            _unclosedParenPos = _token;
                        }
                        _level++;
                    }
                    break;
                case ')':
                    if (_level == 0)
                    {
                        InputOutput.Error(106, InputOutput.PositionNow);
                    }
                    else
                    {
                        _level--;
                        if (_level == 0)
                        {
                            _unclosedParenPos = new TextPosition(0, 0);
                        }
                    }
                    _symbol = rightpar;
                    InputOutput.NextCh();
                    break;
                case ':':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '=')
                    {
                        _symbol = assign;
                        InputOutput.NextCh();
                    }
                    else
                    {
                        _symbol = colon;
                    }
                    break;
                case '<':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '=')
                    {
                        _symbol = laterequal;
                        InputOutput.NextCh();
                    }
                    else if (!InputOutput.EndOfFile && InputOutput.Ch == '>')
                    {
                        _symbol = latergreater;
                        InputOutput.NextCh();
                    }
                    else
                    {
                        _symbol = later;
                    }
                    break;
                case '>':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '=')
                    {
                        _symbol = greaterequal;
                        InputOutput.NextCh();
                    }
                    else
                    {
                        _symbol = greater;
                    }
                    break;
                case '.':
                    InputOutput.NextCh();
                    if (!InputOutput.EndOfFile && InputOutput.Ch == '.')
                    {
                        _symbol = twopoints;
                        InputOutput.NextCh();
                    }
                    else
                    {
                        _symbol = point;
                    }
                    break;
                case '{':
                    InputOutput.NextCh();
                    while (!InputOutput.EndOfFile && InputOutput.Ch != '}')
                    {
                        InputOutput.NextCh();
                    }
                    if (InputOutput.Ch == '}')
                    {
                        InputOutput.NextCh();
                    }
                    return NextSym();
                case '}':
                    InputOutput.NextCh();
                    return NextSym();
                case '\'':
                    {
                        TextPosition startPos = _token;
                        InputOutput.NextCh();

                        if (InputOutput.EndOfFile)
                        {
                            InputOutput.Error(103, startPos);
                            _symbol = ident;
                            return _symbol;
                        }

                        if (InputOutput.Ch == '\'')
                        {
                            InputOutput.Error(102, startPos);
                            InputOutput.NextCh();
                            _symbol = ident;
                            return _symbol;
                        }

                        _charValue = InputOutput.Ch;
                        InputOutput.NextCh();

                        if (InputOutput.EndOfFile || InputOutput.Ch != '\'')
                        {
                            InputOutput.Error(103, startPos);
                            while (!InputOutput.EndOfFile
                                && InputOutput.Ch != ' '
                                && InputOutput.Ch != ';'
                                && InputOutput.Ch != ':'
                                && InputOutput.Ch != '='
                                && InputOutput.Ch != ','
                                && InputOutput.Ch != '('
                                && InputOutput.Ch != ')'
                                && InputOutput.Ch != '\n')
                            {
                                InputOutput.NextCh();
                            }
                            _symbol = ident;
                            return _symbol;
                        }

                        InputOutput.NextCh();

                        if (!InputOutput.EndOfFile
                            && (char.IsLetterOrDigit(InputOutput.Ch)
                            || InputOutput.Ch == '\''))
                        {
                            InputOutput.Error(104, startPos);
                            while (!InputOutput.EndOfFile
                                && (char.IsLetterOrDigit(InputOutput.Ch)
                                || InputOutput.Ch == '\''
                                || InputOutput.Ch == ' '))
                            {
                                InputOutput.NextCh();
                            }
                            _symbol = ident;
                            return _symbol;
                        }

                        _symbol = charc;
                        return _symbol;
                    }
                default:
                    InputOutput.Error(1, InputOutput.PositionNow);
                    InputOutput.NextCh();
                    return NextSym();
            }
            return _symbol;
        }
    }
}