using System.Collections.Generic;

namespace Компилятор
{
    public enum DataType 
    { Unknown, Integer, Real, Char, Boolean 
    }

    public enum SymbolKind 
    { Variable, Procedure, Parameter 
    }

    public class Symbol
    {
        private string _name;
        private SymbolKind _kind;
        private DataType _type;
        private List<ParameterInfo> _parameters = new List<ParameterInfo>();

        public string Name
        {
            get 
            { 
                return _name; 
            }
            set 
            { 
                _name = value?.ToLower(); 
            }
        }

        public SymbolKind Kind
        {
            get 
            { 
                return _kind; 
            }
            set 
            { 
                _kind = value; 
            }
        }

        public DataType Type
        {
            get 
            { 
                return _type;
            }
            set 
            { 
                _type = value; 
            }
        }

        public List<ParameterInfo> Parameters
        {
            get 
            {
                return _parameters; 
            }
            set 
            {
                _parameters = value; 
            }
        }
    }

    public class ParameterInfo
    {
        private string _name;
        private DataType _type;
        private bool _isVar;

        public string Name
        {
            get 
            {
                return _name;
            }
            set
            { 
                _name = value?.ToLower();
            }
        }

        public DataType Type
        {
            get 
            { 
                return _type; 
            }
            set 
            { 
                _type = value; 
            }
        }

        public bool IsVar
        {
            get 
            { 
                return _isVar; 
            }
            set 
            { 
                _isVar = value;
            }
        }
    }

    public class SymbolTable
    {
        private Stack<Dictionary<string, Symbol>> _scopes 
            = new Stack<Dictionary<string, Symbol>>();

        public SymbolTable()
        {
            PushScope();
        }

        public void PushScope() => 
            _scopes.Push(new Dictionary<string, Symbol>
                (System.StringComparer.OrdinalIgnoreCase));
        public void PopScope() => _scopes.Pop();

        public bool AddSymbol(Symbol sym)
        {
            var currentScope = _scopes.Peek();
            if (currentScope.ContainsKey(sym.Name))
            {
                return false;
            }

            currentScope[sym.Name] = sym;
            return true;
        }

        public Symbol Find(string name)
        {
            if (name == null) return null;
            string searchName = name.ToLower();

            foreach (var scope in _scopes)
            {
                if (scope.TryGetValue(searchName, out Symbol sym))
                {
                    return sym;
                }
            }
            return null;
        }
    }
}