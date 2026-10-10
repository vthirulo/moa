

using Moa.TreeWalk;

interface MoaCallable
{
    public object? call(Interpreter interpreter, List<object?> arguments);

    public int arity();

    public string callToString();
}