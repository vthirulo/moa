
using Moa.Scanner;
using Moa.Utils.Errors;

namespace Moa.Interpreter;

class Environment
{
    private Dictionary<string, object?> _values = [];
    private Environment? _enclosing;

    public Environment() => _enclosing = null;

    public Environment(Environment enclosing) => _enclosing = enclosing;

    public void Define(string name, object? value)
    {
        _values.Add(name, value);
    }

    public void Assign(Token name, object? value)
    {
        if (_values.ContainsKey(name.lexeme))
        {
            _values[name.lexeme] = value;
            return;
        }

        if (_enclosing is not null)
        {
            _enclosing.Assign(name, value);
            return;
        }

        throw new RuntimeException(name, "Undefined variable '" + name.lexeme + "'");
    }

    public object? Get(Token name)
    {
        if (_values.ContainsKey(name.lexeme))
        {
            return _values[name.lexeme];
        }

        if (_enclosing is not null) return _enclosing.Get(name);

        throw new RuntimeException(name, "Undefined variable '" + name.lexeme + "'");
    }

}