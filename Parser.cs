using System;
using System.Collections.Generic;

namespace Компилятор
{
    class Parser
    {
        private LexicalAnalyzer _lex;
        private SymbolTable _symTable;

        public Parser(LexicalAnalyzer lex)
        {
            _lex = lex;
            _symTable = new SymbolTable();
        }

        private byte Sym => _lex.Symbol;

        public void Parse()
        {
            _lex.NextSym();

            if (Sym == LexicalAnalyzer.Programsy)
            {
                _lex.NextSym();
                if (Sym == LexicalAnalyzer.Ident)
                {
                    _lex.NextSym();
                }
                else
                {
                    InputOutput.Error(2, _lex.Token);
                }

                Accept(LexicalAnalyzer.Semicolon, 14);
            }
            else
            {
                InputOutput.Error(25, _lex.Token);
                SkipTo(LexicalAnalyzer.Varsy, 
                    LexicalAnalyzer.Procedurensy, LexicalAnalyzer.Beginsy);
            }

            Block();

            Accept(LexicalAnalyzer.Point, 26);
        }

        private void Block()
        {
            if (Sym == LexicalAnalyzer.Varsy)
            {
                _lex.NextSym();
                while (Sym == LexicalAnalyzer.Ident)
                {
                    VarDeclaration();
                    Accept(LexicalAnalyzer.Semicolon, 14);
                }
            }

            while (Sym == LexicalAnalyzer.Procedurensy)
            {
                ProcedureDeclaration();
                Accept(LexicalAnalyzer.Semicolon, 14);
            }

            CompoundStatement();
        }

        private void VarDeclaration()
        {
            List<string> varNames = new List<string>();

            while (Sym == LexicalAnalyzer.Ident)
            {
                varNames.Add(_lex.AddrName);
                _lex.NextSym();

                if (Sym == LexicalAnalyzer.Comma)
                {
                    _lex.NextSym();
                }
                else
                {
                    break;
                }
            }

            Accept(LexicalAnalyzer.Colon, 13);
            DataType type = ParseType();

            foreach (var name in varNames)
            {
                if (!_symTable.AddSymbol(new Symbol 
                { Name = name, Kind = SymbolKind.Variable, Type = type }))
                {
                    InputOutput.Error(100, _lex.Token); 
                }
            }
        }

        private DataType ParseType()
        {
            DataType t = DataType.Unknown;
            if (Sym == LexicalAnalyzer.Ident)
            {
                string typeName = _lex.AddrName.ToLower();
                switch (typeName)
                {
                    case "integer": t = DataType.Integer; break;
                    case "real": t = DataType.Real; break;
                    case "char": t = DataType.Char; break;
                    case "boolean": t = DataType.Boolean; break;
                    default:
                        InputOutput.Error(15, _lex.Token);
                        break;
                }
                _lex.NextSym();
            }
            else
            {
                InputOutput.Error(15, _lex.Token);
                SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Rightpar);
            }
            return t;
        }

        private void ProcedureDeclaration()
        {
            _lex.NextSym();

            if (Sym != LexicalAnalyzer.Ident)
            {
                InputOutput.Error(2, _lex.Token);
                SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Beginsy);
                return;
            }

            Symbol procSym = new Symbol { Name = _lex.AddrName, 
                Kind = SymbolKind.Procedure, Type = DataType.Unknown };
            if (!_symTable.AddSymbol(procSym))
            {
                InputOutput.Error(100, _lex.Token);
            }

            _lex.NextSym();

            _symTable.PushScope();

            if (Sym == LexicalAnalyzer.Leftpar)
            {
                _lex.NextSym();
                while (Sym == LexicalAnalyzer.Ident
                    || Sym == LexicalAnalyzer.Varsy)
                {
                    bool isVar = false;
                    if (Sym == LexicalAnalyzer.Varsy)
                    {
                        isVar = true;
                        _lex.NextSym();
                    }

                    List<string> paramNames = new List<string>();
                    while (Sym == LexicalAnalyzer.Ident)
                    {
                        paramNames.Add(_lex.AddrName);
                        _lex.NextSym();
                        if (Sym == LexicalAnalyzer.Comma) _lex.NextSym();
                        else break;
                    }

                    Accept(LexicalAnalyzer.Colon, 13);
                    DataType pType = ParseType();

                    foreach (var pName in paramNames)
                    {
                        procSym.Parameters.Add(new ParameterInfo
                        { Name = pName, Type = pType, IsVar = isVar });
                        _symTable.AddSymbol(new Symbol
                        {
                            Name = pName,
                            Kind = SymbolKind.Parameter,
                            Type = pType
                        });
                    }

                    if (Sym == LexicalAnalyzer.Semicolon)
                    {
                        _lex.NextSym();
                    }
                    else
                    {
                        break;
                    }
                }
                    Accept(LexicalAnalyzer.Rightpar, 10);
            }

            Accept(LexicalAnalyzer.Semicolon, 14);

            Block();

            _symTable.PopScope();
        }

        private void CompoundStatement()
        {
            Accept(LexicalAnalyzer.Beginsy, 17);

            while (Sym != LexicalAnalyzer.Endsy && !InputOutput.EndOfFile)
            {
                Statement();
                if (Sym == LexicalAnalyzer.Semicolon)
                {
                    _lex.NextSym();
                }
                else if (Sym != LexicalAnalyzer.Endsy)
                {
                    InputOutput.Error(14, _lex.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, 
                        LexicalAnalyzer.Endsy, LexicalAnalyzer.Beginsy);
                    if (Sym == LexicalAnalyzer.Semicolon) _lex.NextSym();
                }
            }

            Accept(LexicalAnalyzer.Endsy, 22);
        }

        private void Statement()
        {
            if (Sym == LexicalAnalyzer.Ident)
            {
                string idName = _lex.AddrName;
                TextPosition idPos = _lex.Token;
                _lex.NextSym();

                Symbol s = _symTable.Find(idName);
                if (s == null)
                {
                    InputOutput.Error(100, idPos);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Endsy);
                    return;
                }

                if (Sym == LexicalAnalyzer.Assign)
                {
                    _lex.NextSym();

                    if (Sym == LexicalAnalyzer.Ident)
                    {
                        Symbol rightSide = _symTable.Find(_lex.AddrName);
                        if (rightSide != null && rightSide.Kind 
                            == SymbolKind.Procedure)
                        {
                            InputOutput.Error(100, _lex.Token);
                        }
                    }
                    DataType exprType = Expression();

                    if (s.Type != exprType && s.Type 
                        != DataType.Unknown && exprType != DataType.Unknown)
                    {
                        if (!(s.Type == DataType.Real 
                            && exprType == DataType.Integer))
                        {
                            InputOutput.Error(18, idPos);
                        }
                    }
                }
                else if (Sym == LexicalAnalyzer.Leftpar 
                    || Sym == LexicalAnalyzer.Semicolon)
                {
                    if (s.Kind != SymbolKind.Procedure)
                        InputOutput.Error(100, idPos);

                    if (Sym == LexicalAnalyzer.Leftpar)
                    {
                        _lex.NextSym();
                        int paramIdx = 0;
                        while (Sym != LexicalAnalyzer.Rightpar 
                            && !InputOutput.EndOfFile)
                        {
                            DataType argType = Expression();
                            if (paramIdx < s.Parameters.Count)
                            {
                                var expectedType = s.Parameters[paramIdx].Type;
                                if (argType != expectedType && !(expectedType 
                                    == DataType.Real && argType 
                                    == DataType.Integer))
                                {
                                    InputOutput.Error(100, _lex.Token);
                                }
                            }
                            paramIdx++;

                            if (Sym == LexicalAnalyzer.Comma)
                            {
                                _lex.NextSym();
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (paramIdx != s.Parameters.Count)
                        {
                            InputOutput.Error(100, _lex.Token);
                        }

                        Accept(LexicalAnalyzer.Rightpar, 10);
                    }
                }
                else
                {
                    InputOutput.Error(18, _lex.Token);
                    SkipTo(LexicalAnalyzer.Semicolon, LexicalAnalyzer.Endsy);
                }
            }
            else if (Sym == LexicalAnalyzer.Beginsy)
            {
                CompoundStatement();
            }
        }

        private DataType Expression()
        {
            DataType t1 = Term();
            while (Sym == LexicalAnalyzer.Plus || Sym == LexicalAnalyzer.Minus)
            {
                _lex.NextSym();
                DataType t2 = Term();
                t1 = GetWiderType(t1, t2);
            }
            return t1;
        }

        private DataType Term()
        {
            DataType t1 = Factor();
            while (Sym == LexicalAnalyzer.Star || Sym == LexicalAnalyzer.Slash)
            {
                _lex.NextSym();
                DataType t2 = Factor();
                t1 = GetWiderType(t1, t2);
            }
            return t1;
        }

        private DataType Factor()
        {
            DataType t = DataType.Unknown;
            if (Sym == LexicalAnalyzer.Ident)
            {
                string name = _lex.AddrName.ToLower();
                if (name == "true" || name == "false")
                {
                    t = DataType.Boolean;
                    _lex.NextSym();
                }
                else
                {
                    Symbol s = _symTable.Find(_lex.AddrName);
                    if (s == null) InputOutput.Error(100, _lex.Token);
                    else t = s.Type;
                    _lex.NextSym();
                }
            }
            else if (Sym == LexicalAnalyzer.Intc)
            {
                t = DataType.Integer;
                _lex.NextSym();
            }
            else if (Sym == LexicalAnalyzer.Floatc)
            {
                t = DataType.Real;
                _lex.NextSym();
            }
            else if (Sym == LexicalAnalyzer.Leftpar)
            {
                _lex.NextSym();
                t = Expression();
                Accept(LexicalAnalyzer.Rightpar, 10);
            }
            else
            {
                InputOutput.Error(18, _lex.Token);
                SkipTo(LexicalAnalyzer.Semicolon, 
                    LexicalAnalyzer.Rightpar, LexicalAnalyzer.Endsy);
            }
            return t;
        }

        private DataType GetWiderType(DataType t1, DataType t2)
        {
            if (t1 == DataType.Real || t2 == DataType.Real)
            {
                return DataType.Real;
            }
            return DataType.Integer;
        }


        private void Accept(byte expectedToken, byte errorCode)
        {
            if (Sym == expectedToken)
            {
                _lex.NextSym();
            }
            else
            {
                InputOutput.Error(errorCode, _lex.Token);
            }
        }

        private void SkipTo(params byte[] syncTokens)
        {
            while (!InputOutput.EndOfFile)
            {
                foreach (var t in syncTokens)
                {
                    if (Sym == t) return;
                }
                _lex.NextSym();
            }
        }
    }
}