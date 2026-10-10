

using Moa.TreeWalk;
using Moa.TreeWalk.AST;
using MoaEnvironment = Moa.TreeWalk.Environment;

class MoaFunction(FuncStmt declaration, MoaEnvironment closure) : MoaCallable
{
    private FuncStmt _declaration = declaration;
    private MoaEnvironment _closure = closure;

    public int arity() => _declaration.Params.Count;

    public string callToString() => "fn <" + _declaration.Name.lexeme + ">";

    public object? call(Interpreter interpreter, List<object?> arguments)
    {
        MoaEnvironment _env = new(_closure);

        for (int i = 0; i < _declaration.Params.Count; ++i)
        {
            _env.Define(_declaration.Params[i].lexeme, arguments[i]);
        }

        try
        {
            interpreter._executeBlock(_declaration.FuncBody, _env);
        }
        catch (Return ret)
        {
            return ret.value;
        }

        return null;
    }

}