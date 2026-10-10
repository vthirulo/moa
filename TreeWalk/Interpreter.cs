
namespace Moa.TreeWalk;

using Moa.TreeWalk.AST;
using Moa.TreeWalk.Utils.Errors;

class Interpreter : Expression.IVisitor<Object?>, Statement.IVisitor<Object?>
{
    internal Environment _globals;
    private Environment _env;

    class ClockFunction : MoaCallable
    {
        public int arity() => 0;

        public string callToString()
        {
            return "native function <clock>";
        }

        public object? call(Interpreter interpreter, List<object?> arguments)
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000.0;
        }
    };

    public Interpreter()
    {
        _globals = new();
        _env = _globals;

        _globals.Define("clock", new ClockFunction());
    }

    public void interpret(List<Statement> statements)
    {
        try
        {
            foreach (Statement stmt in statements)
            {
                _execute(stmt);
            }
        }
        catch (RuntimeException runtime_err)
        {
            Error.ReportRuntimeError(runtime_err);
        }
    }

    public object? VisitBlockStatement(Block block)
    {
        _executeBlock(block.Statements, new Environment(_env));
        return null;
    }

    public object? VisitIfStmtStatement(IfStmt ifStmt)
    {
        if (_isTruthness(_evaluate(ifStmt.Condition)))
        {
            _execute(ifStmt.ThenBranch);
        }
        else if (ifStmt.ElseBranch != null)
        {
            _execute(ifStmt.ElseBranch);
        }

        return null;
    }

    public object? VisitWhileStmtStatement(WhileStmt whileStmt)
    {
        while (_isTruthness(_evaluate(whileStmt.Condition)))
        {
            _execute(whileStmt.Body);
        }

        return null;
    }

    public object? VisitExpressionStmtStatement(ExpressionStmt expr)
    {
        _evaluate(expr.Expr);
        return null;
    }

    public object? VisitPrintStmtStatement(PrintStmt expr)
    {
        Object? value = _evaluate(expr.Expr);
        Console.WriteLine(_value_to_string_conv(value));
        return null;
    }

    public object? VisitVarStmtStatement(VarStmt expr)
    {
        object? value = null;

        if (expr.Initializer != null)
        {
            value = _evaluate(expr.Initializer);
        }

        _env.Define(expr.Name.lexeme, value);
        return value;
    }

    public object? VisitFuncStmtStatement(FuncStmt stmt)
    {
        MoaFunction function = new(stmt, _env);

        _env.Define(stmt.Name.lexeme, function);

        return null;
    }

    public object? VisitReturnStmtStatement(ReturnStmt stmt)
    {
        object? value = null;
        if (stmt.Value is not null) value = _evaluate(stmt.Value);

        throw new Return(value);
    }

    public object? VisitVariableExpression(Variable expr) => _env.Get(expr.Name);

    public object? VisitLiteralExpression(Literal expr) => expr.Value;

    public object? VisitGroupingExpression(Grouping expr) => _evaluate(expr.Expr);

    public object? VisitAssignExpression(Assign expr)
    {
        object? value = _evaluate(expr.Value);
        _env.Assign(expr.Name, value);
        return value;
    }

    public object? VisitLogicalExpression(Logical expr)
    {
        object? left = _evaluate(expr.Left);

        if (expr.Operator.type == TokenType.OR)
        {
            if (_isTruthness(left)) return left;
        }
        else
        {
            if (!_isTruthness(left)) return left;
        }

        return _evaluate(expr.Right);
    }

    public object? VisitUnaryExpression(Unary expr)
    {
        object? right = _evaluate(expr.Right);

        switch (expr.Operator.type)
        {
            case TokenType.BANG:
                return right == null ? null : !_isTruthness(right);
            case TokenType.MINUS:
                _checkOperandNumber(expr.Operator, right);
                return right == null ? null : -(double)right;
        }

        return null;
    }

    public object? VisitCallExpression(Call expr)
    {
        object calle = _evaluate(expr.Calle)!;

        List<object?> arguments = [];
        foreach (Expression single_arg in expr.arguments)
        {
            arguments.Add(_evaluate(single_arg));
        }

        if (calle is not MoaCallable)
        {
            throw new RuntimeException(expr.Paren, "Can only call functions and classes");
        }

        MoaCallable function = (MoaCallable)calle;

        if (arguments.Count != function.arity())
        {
            throw new RuntimeException(
                expr.Paren, "Expected " + function.arity() + " arguments but got " + arguments.Count + "."
            );
        }

        return function.call(this, arguments);
    }

    public object? VisitBinaryExpression(Binary expr)
    {
        object? left = _evaluate(expr.Left);
        object? right = _evaluate(expr.Right);

        switch (expr.Operator.type)
        {
            case TokenType.PLUS:
                if (left is string vleft && right is string vright)
                {
                    return vleft + vright;
                }
                else if (left is double && right is double)
                {
                    _checkOperandsNumber(expr.Operator, left, right);
                    return (left == null || right == null) ? null : (double)left + (double)right;
                }

                throw new RuntimeException(expr.Operator, "Operands must be two numbers or two strings !");
            case TokenType.MINUS:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left - (double)right;
            case TokenType.SLASH:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left / (double)right;
            case TokenType.STAR:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left * (double)right;

            case TokenType.GREATER:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left > (double)right;
            case TokenType.GREATER_EQUAL:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left >= (double)right;
            case TokenType.LESS:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left < (double)right;
            case TokenType.LESS_EQUAL:
                _checkOperandsNumber(expr.Operator, left, right);
                return (left == null || right == null) ? null : (double)left <= (double)right;

            case TokenType.BANG_EQUAL:
                return !_isEqual(left, right);
            case TokenType.EQUAL_EQUAL:
                return _isEqual(left, right);
        }

        return null;
    }

    public object? VisitCommaExpression(Comma expr)
    {
        throw new NotImplementedException();
    }

    internal void _executeBlock(List<Statement> statements, Environment environment)
    {
        Environment _previous = this._env;

        try
        {
            this._env = environment;

            foreach (Statement stmt in statements)
            {
                _execute(stmt);
            }
        }
        finally
        {
            this._env = _previous;
        }
    }

    private object? _execute(Statement stmt) => stmt.Accept(this);

    private object? _evaluate(Expression expr) => expr.Accept(this);

    private bool _isTruthness(object? value)
    {
        if (value == null) return false;
        if (value is bool) return (bool)value;

        return true;
    }

    private bool _isEqual(object? left, object? right)
    {
        if (left == null && right == null) return true;
        if (left == null) return false;

        return left.Equals(right);
    }

    private void _checkOperandNumber(Token @operator, object? operand)
    {
        if (operand is double) return;

        throw new RuntimeException(@operator, "Operand must be a number !");
    }

    private void _checkOperandsNumber(Token @operator, object? left, object? right)
    {
        if (left is double && right is double) return;

        throw new RuntimeException(@operator, "Operands must be a number !");
    }

    private string? _value_to_string_conv(object? value)
    {
        if (value == null) return "null";

        if (value is double)
        {
            string text = value.ToString() ?? "null";

            if (text.EndsWith(".0")) text = text.Substring(0, text.Length - 2);

            return text;
        }

        return value.ToString();
    }

}