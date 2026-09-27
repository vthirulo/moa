
namespace Moa;

class Interpret : Expression.IVisitor<Object?>
{

    public Interpret(Expression expr)
    {
        try
        {
            object? value = _evaluate(expr);
            Console.WriteLine(_value_to_string_conv(value));
        }
        catch (RuntimeException runtime_err)
        {
            Error.report(runtime_err.token.line, runtime_err.Message);
        }
    }

    public object? VisitLiteralExpression(Literal expr)
    {
        return expr.Value;
    }

    public object? VisitGroupingExpression(Grouping expr)
    {
        return _evaluate(expr.Expr);
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

    public object? VisitBinaryExpression(Binary expr)
    {
        object? left = _evaluate(expr.Left);
        object? right = _evaluate(expr.Right);

        switch (expr.Operator.type)
        {
            case TokenType.PLUS:
                if (left is string && right is string)
                {
                    _checkOperandsNumber(expr.Operator, left, right);
                    return (left == null || right == null) ? null : (string)left + (string)right;
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

    private string _value_to_string_conv(object? value)
    {
        if (value == null) return "null";

        if (value is double)
        {
            string text = value.ToString();

            if (text.EndsWith(".0")) text = text.Substring(0, text.Length - 2);

            return text;
        }

        return value.ToString();
    }

}