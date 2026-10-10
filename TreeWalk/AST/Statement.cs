
namespace Moa.TreeWalk.AST;

using Moa.TreeWalk;

abstract class Statement
{
	public interface IVisitor<R>
	{
		R VisitBlockStatement(Block expr);
		R VisitIfStmtStatement(IfStmt expr);
		R VisitWhileStmtStatement(WhileStmt expr);
		R VisitExpressionStmtStatement(ExpressionStmt expr);
		R VisitPrintStmtStatement(PrintStmt expr);
		R VisitVarStmtStatement(VarStmt expr);
		R VisitFuncStmtStatement(FuncStmt expr);
		R VisitReturnStmtStatement(ReturnStmt expr);
	}
	public abstract R Accept<R>(IVisitor<R> visitor);
}


class Block : Statement
{
	public List<Statement> Statements;

	public Block(List<Statement> Statements)
	{
		this.Statements = Statements;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitBlockStatement(this);
	}
}


class IfStmt : Statement
{
	public Expression Condition;
	public Statement ThenBranch;
	public Statement? ElseBranch;

	public IfStmt(Expression Condition, Statement ThenBranch, Statement? ElseBranch)
	{
		this.Condition = Condition;
		this.ThenBranch = ThenBranch;
		this.ElseBranch = ElseBranch;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitIfStmtStatement(this);
	}
}


class WhileStmt : Statement
{
	public Expression Condition;
	public Statement Body;

	public WhileStmt(Expression Condition, Statement Body)
	{
		this.Condition = Condition;
		this.Body = Body;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitWhileStmtStatement(this);
	}
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


class FuncStmt : Statement
{
	public Token Name;
	public List<Token> Params;
	public List<Statement> FuncBody;

	public FuncStmt(Token Name, List<Token> Params, List<Statement> FuncBody)
	{
		this.Name = Name;
		this.Params = Params;
		this.FuncBody = FuncBody;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitFuncStmtStatement(this);
	}
}


class ReturnStmt : Statement
{
	public Token Keyword;
	public Expression? Value;

	public ReturnStmt(Token Keyword, Expression? Value)
	{
		this.Keyword = Keyword;
		this.Value = Value;
	}

	public override R Accept<R>(IVisitor<R> visitor)
	{
		return visitor.VisitReturnStmtStatement(this);
	}
}
