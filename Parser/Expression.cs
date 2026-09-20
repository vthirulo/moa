
namespace Moa;

abstract class Expression
{
	public interface IVisitor<R>
	{
		R VisitUnaryExpression(Unary expr);
		R VisitBinaryExpression(Binary expr);
		R VisitGroupingExpression(Grouping expr);
		R VisitLiteralExpression(Literal expr);
	}
	public abstract R Accept<R>(IVisitor<R> visitor);
}


class Unary : Expression
{
	public Token Operator;
	public Expression Right;

	public Unary(Token Operator, Expression Right)
	{
		this.Operator = Operator;
		this.Right = Right;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitUnaryExpression(this);
	}
}


class Binary : Expression
{
	public Expression Left;
	public Token Operator;
	public Expression Right;

	public Binary(Expression Left, Token Operator, Expression Right)
	{
		this.Left = Left;
		this.Operator = Operator;
		this.Right = Right;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitBinaryExpression(this);
	}
}


class Grouping : Expression
{
	public Expression Expr;

	public Grouping(Expression Expr)
	{
		this.Expr = Expr;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitGroupingExpression(this);
	}
}


class Literal : Expression
{
	public Object? Value;

	public Literal(Object? Value)
	{
		this.Value = Value;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitLiteralExpression(this);
	}
}
