
namespace Moa.AST;

using Moa.Scanner;

abstract class Statement
{
	public interface IVisitor<R>
	{
		R VisitExpressionStmtStatement(ExpressionStmt expr);
		R VisitPrintStmtStatement(PrintStmt expr);
		R VisitVarStmtStatement(VarStmt expr);
	}
	public abstract R Accept<R>(IVisitor<R> visitor);
}


class ExpressionStmt : Statement
{
	public Expression Expr;

	public ExpressionStmt(Expression Expr)
	{
		this.Expr = Expr;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitExpressionStmtStatement(this);
	}
}


class PrintStmt : Statement
{
	public Expression Expr;

	public PrintStmt(Expression Expr)
	{
		this.Expr = Expr;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitPrintStmtStatement(this);
	}
}


class VarStmt : Statement
{
	public Token Name;
	public Expression? Initializer;

	public VarStmt(Token Name, Expression? Initializer)
	{
		this.Name = Name;
		this.Initializer = Initializer;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitVarStmtStatement(this);
	}
}
