
namespace Moa.AST;

using Moa.Scanner;

abstract class Statement
{
	public interface IVisitor<R>
	{
		R VisitExpressionStmtStatement(ExpressionStmt expr);
		R VisitPrintStmtStatement(PrintStmt expr);
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
