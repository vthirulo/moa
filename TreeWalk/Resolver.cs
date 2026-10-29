
using Moa.TreeWalk;
using Moa.TreeWalk.AST;
using Moa.TreeWalk.Utils.Errors;
using MoaInterpreter = Moa.TreeWalk.Interpreter;

class Resolver(MoaInterpreter interpreter) : Expression.IVisitor<Object?>, Statement.IVisitor<Object?>
{
    private MoaInterpreter _interpreter = interpreter;
    private Stack<Dictionary<string, bool>> _scopes = new();

    private Dictionary<string, bool> _globalstates = [];

    private FunctionType _currentFunction = FunctionType.NONE;

    public object? VisitBlockStatement(Block expr)
    {
        _beginScope();
        _resolve(expr.Statements);
        _endScope();

        return null;
    }

    private void _beginScope() => _scopes.Push(new Dictionary<string, bool>());

    private void _endScope() => _scopes.Pop();

    public void _resolve(List<Statement> statements)
    {
        foreach (Statement stmt in statements)
        {
            _resolve(stmt);
        }
    }

    private void _resolve(Statement statement)
    {
        statement.Accept(this);
    }

    private void _resolve(Expression expression)
    {
        expression.Accept(this);
    }

    public object? VisitVarStmtStatement(VarStmt varStmt)
    {
        _declare(varStmt.Name);
        if (varStmt.Initializer is not null)
        {
            _resolve(varStmt.Initializer);
        }
        _define(varStmt.Name);
        return null;
    }

    private void _declare(Token varName)
    {
        if (_scopes.Count is 0)
        {
            if (_globalstates.ContainsKey(varName.lexeme))
            {
                Error.Report(varName.line, "Redeclaring global variable '" + varName.lexeme + "' is not allowed !");
            }
            _globalstates[varName.lexeme] = false;
            return;
        }

        Dictionary<string, bool> scope = _scopes.Peek();

        if (scope.ContainsKey(varName.lexeme))
        {
            Error.Report(varName.line, "Redeclaring variable '" + varName.lexeme + "' is not allowed !");
        }

        scope[varName.lexeme] = false;
    }

    private void _define(Token varName)
    {
        if (_scopes.Count is 0)
        {
            _globalstates[varName.lexeme] = true;
            return;
        }

        _scopes.Peek()[varName.lexeme] = true;
    }

    public object? VisitVariableExpression(Variable expr)
    {
        if (_scopes.Count is not 0)
        {
            if (_scopes.Peek().TryGetValue(expr.Name.lexeme, out bool val) && val == false) Error.Report(
                expr.Name.line, "Can't read local variable in its own initializer"
            );
        }
        else
        {
            if (_globalstates.TryGetValue(expr.Name.lexeme, out bool val) && val == false) Error.Report(
                expr.Name.line, "Can't read global variable in its own initializer"
            );
            return null;
        }

        _resolveLocal(expr, expr.Name);
        return null;
    }

    public object? VisitAssignExpression(Assign expr)
    {
        _resolve(expr.Value);
        _resolveLocal(expr, expr.Name);
        return null;
    }

    public object? VisitFuncStmtStatement(FuncStmt stmt)
    {
        _declare(stmt.Name);
        _define(stmt.Name);

        _resolveFunction(stmt, FunctionType.FUNCTION);
        return null;
    }

    private void _resolveLocal(Expression varExpr, Token varName)
    {
        for (int i = 0; i < _scopes.Count; ++i)
        {
            if (_scopes.ElementAt(i).ContainsKey(varName.lexeme))
            {
                _interpreter.resolve(varExpr, i);
                return;
            }
        }
    }

    private void _resolveFunction(FuncStmt stmt, FunctionType functionType)
    {
        FunctionType enclosingFunction = _currentFunction;
        _currentFunction = functionType;

        _beginScope();

        foreach (Token parameter in stmt.Params)
        {
            _declare(parameter);
            _define(parameter);
        }
        _resolve(stmt.FuncBody);
        _endScope();

        _currentFunction = enclosingFunction;
    }

    public object? VisitExpressionStmtStatement(ExpressionStmt exprStmt)
    {
        _resolve(exprStmt.Expr);
        return null;
    }

    public object? VisitIfStmtStatement(IfStmt IfElseStmt)
    {
        _resolve(IfElseStmt.Condition);
        _resolve(IfElseStmt.ThenBranch);
        if (IfElseStmt.ElseBranch is not null) _resolve(IfElseStmt.ElseBranch);
        return null;
    }


    public object? VisitPrintStmtStatement(PrintStmt printStmt)
    {
        _resolve(printStmt.Expr);
        return null;
    }

    public object? VisitReturnStmtStatement(ReturnStmt returnStmt)
    {
        if (_currentFunction is FunctionType.NONE)
        {
            Error.Report(returnStmt.Keyword.line, "Can't return from a block");
        }

        if (returnStmt.Value is not null) _resolve(returnStmt.Value);
        return null;
    }

    public object? VisitWhileStmtStatement(WhileStmt whileStmt)
    {
        _resolve(whileStmt.Condition);
        _resolve(whileStmt.Body);
        return null;
    }

    public object? VisitBinaryExpression(Binary expr)
    {
        _resolve(expr.Left);
        _resolve(expr.Right);
        return null;
    }

    public object? VisitCallExpression(Call expr)
    {
        _resolve(expr.Calle);

        foreach (Expression argument in expr.arguments)
        {
            _resolve(argument);
        }

        return null;
    }

    public object? VisitGroupingExpression(Grouping expr)
    {
        _resolve(expr.Expr);
        return null;
    }

    public object? VisitUnaryExpression(Unary expr)
    {
        _resolve(expr.Right);
        return null;
    }


    public object? VisitLogicalExpression(Logical expr)
    {
        _resolve(expr.Left);
        _resolve(expr.Right);

        return null;
    }

    public object? VisitLiteralExpression(Literal expr) => null;

    public object? VisitCommaExpression(Comma expr)
    {
        throw new NotImplementedException();
    }

}

enum FunctionType
{
    NONE, FUNCTION
};
