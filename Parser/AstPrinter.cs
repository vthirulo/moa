
using System.Text;

namespace Moa;

class AstPrinter : Expression.IVisitor<String>
{
    public string VisitCommaExpression(Comma expr) => parenthesize("comma", expr.Right);

    public string VisitBinaryExpression(Binary expr) =>
        parenthesize(
            expr.Operator.lexeme,
            expr.Left,
            expr.Right
        );

    public string VisitGroupingExpression(Grouping expr) =>
        parenthesize(
            "group",
            expr.Expr
        );

    public string VisitLiteralExpression(Literal expr) => expr.Value == null ? "null" : expr.Value.ToString();

    public string VisitUnaryExpression(Unary expr) =>
        parenthesize(
            expr.Operator.lexeme,
            expr.Right
        );

    public String print(Expression expr) => expr.Accept(this);

    String parenthesize(String name, params Expression[] exprs)
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("(").Append(name);

        foreach (Expression expr in exprs)
        {
            builder.Append(" ");
            builder.Append(expr.Accept(this));
        }

        builder.Append(")");

        return builder.ToString();
    }

}