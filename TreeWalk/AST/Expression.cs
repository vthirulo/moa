
namespace Moa.TreeWalk.AST;

using Moa.TreeWalk;

abstract class Expression
{
	public interface IVisitor<R>
	{
		R VisitCommaExpression(Comma expr);
		R VisitAssignExpression(Assign expr);
		R VisitUnaryExpression(Unary expr);
		R VisitLogicalExpression(Logical expr);
		R VisitBinaryExpression(Binary expr);
		R VisitGroupingExpression(Grouping expr);
		R VisitLiteralExpression(Literal expr);
		R VisitVariableExpression(Variable expr);
	}
	public abstract R Accept<R>(IVisitor<R> visitor);
}


class Comma : Expression
{
	public Expression Right;

	public Comma(Expression Right)
	{
		this.Right = Right;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitCommaExpression(this);
	}
}


class Assign : Expression
{
	public Token Name;
	public Expression Value;

	public Assign(Token Name, Expression Value)
	{
		this.Name = Name;
		this.Value = Value;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitAssignExpression(this);
	}
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


class Logical : Expression
{
	public Expression Left;
	public Token Operator;
	public Expression Right;

	public Logical(Expression Left, Token Operator, Expression Right)
	{
		this.Left = Left;
		this.Operator = Operator;
		this.Right = Right;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitLogicalExpression(this);
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


class Variable : Expression
{
	public Token Name;

	public Variable(Token Name)
	{
		this.Name = Name;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitVariableExpression(this);
	}
}
