
namespace Moa.TreeWalk;

using Moa.TreeWalk.Utils.Errors;

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

    public void AssignAt(int distance, Token name, object? value) => _ancestor(
        distance
    )._values[name.lexeme] = value;

    public object? Get(Token name)
    {
        if (_values.ContainsKey(name.lexeme))
        {
            if (_values[name.lexeme] is null) throw new RuntimeException(
                name, "Accessing an uninitialized variable '" + name.lexeme + "' is forbidden !"
            );

            return _values[name.lexeme];
        }

        if (_enclosing is not null) return _enclosing.Get(name);

        throw new RuntimeException(name, "Undefined variable '" + name.lexeme + "'");
    }

    public object? GetAt(int distance, string name) => _ancestor(distance)._values[name];

    private Environment _ancestor(int distance)
    {
        Environment environment = this;

        for (int i = 0; i < distance; ++i)
        {
            environment = environment._enclosing!;
        }
        return environment;
    }

}