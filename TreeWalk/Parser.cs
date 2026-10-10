
namespace Moa.TreeWalk;

using Moa.TreeWalk.AST;
using Moa.TreeWalk.Utils.Errors;

class ParseError : SystemException
{

}

class Parser
{
    private readonly List<Token> tokens;
    private int current;

    public Parser(List<Token> Tokens)
    {
        this.tokens = Tokens;
        this.current = 0;
    }

    public List<Statement> Parse()
    {
        List<Statement> statements = [];

        while (!_isEOF())
        {
            if (_declaration() is Statement stmt)
            {
                statements.Add(stmt);
            }
        }

        return statements;
    }

    private Statement? _declaration()
    {
        try
        {
            if (_match(TokenType.FUNC)) return _function("function");
            if (_match(TokenType.VAR)) return _varDeclaration();

            return _statement();
        }
        catch (ParseError)
        {
            _synchronize();
            return null;
        }
    }

    private Statement _varDeclaration()
    {
        Token Name = _consume(TokenType.IDENTIFIER, "Expect variable name");

        Expression? initializer = null;

        if (_match(TokenType.EQUAL))
        {
            initializer = _expression();
        }

        _consume(TokenType.SEMICOLON, "Expect ';' after variable declaration");
        return new VarStmt(Name, initializer);
    }

    private FuncStmt _function(string kind)
    {
        Token funcName = _consume(TokenType.IDENTIFIER, "Expect " + kind + " name.");

        _consume(TokenType.LEFT_PAREN, "Expect '(' after " + kind + " name.");

        List<Token> parameters = [];

        if (!_check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (parameters.Count >= 255)
                {
                    Error.Report(_peek().line, "Can't have more than 255 parameters.");
                }

                parameters.Add(_consume(TokenType.IDENTIFIER, "Expect parameters Name"));
            } while (_match(TokenType.COMMA));
        }

        _consume(TokenType.RIGHT_PAREN, "Expect ')' after parameters");

        _consume(TokenType.LEFT_BRACE, "Expect '{' after " + kind + " body.");

        List<Statement> funcBody = _block();

        return new FuncStmt(funcName, parameters, funcBody);
    }

    private Statement _statement()
    {
        if (_match(TokenType.IF)) return _ifStatement();

        if (_match(TokenType.WHILE)) return _whileStatement();

        if (_match(TokenType.FOR)) return _forStatement();

        if (_match(TokenType.PRINT)) return _printStatement();

        if (_match(TokenType.RETURN)) return _returnStatement();

        if (_match(TokenType.LEFT_BRACE)) return new Block(_block());

        return _expressionStatement();
    }

    private List<Statement> _block()
    {
        List<Statement> statements = [];

        while (!_check(TokenType.RIGHT_BRACE) && !_isEOF())
        {
            if (_declaration() is Statement stmt)
            {
                statements.Add(stmt);
            }
        }

        _consume(TokenType.RIGHT_BRACE, "Expect '}' after block");
        return statements;
    }

    private Statement _ifStatement()
    {
        _consume(TokenType.LEFT_PAREN, "Expect '(' after if keyword");
        Expression condition = _expression();
        _consume(TokenType.RIGHT_PAREN, "Expect ')' after if condition");

        Statement thenBranch = _statement();
        Statement? elseBranch = null;
        if (_match(TokenType.ELSE))
        {
            elseBranch = _statement();
        }

        return new IfStmt(condition, thenBranch, elseBranch);
    }

    private Statement _whileStatement()
    {
        _consume(TokenType.LEFT_PAREN, "Expect '(' after while keyword");
        Expression condition = _expression();
        _consume(TokenType.RIGHT_PAREN, "Expect ')' after while condition");

        Statement while_body = _statement();

        return new WhileStmt(condition, while_body);
    }

    private Statement _forStatement()
    {
        _consume(TokenType.LEFT_PAREN, "Expect '(' after for keyword");

        Statement? initializer;

        if (_match(TokenType.SEMICOLON)) initializer = null;
        else if (_match(TokenType.VAR)) initializer = _varDeclaration();
        else initializer = _expressionStatement();

        Expression? condition = null;

        if (!_match(TokenType.SEMICOLON))
        {
            condition = _expression();
        }
        _consume(TokenType.SEMICOLON, "Expect ';' after loop condition");

        Expression? incrementer = null;

        if (!_match(TokenType.RIGHT_PAREN))
        {
            incrementer = _expression();
        }

        _consume(TokenType.RIGHT_PAREN, "Expect ')' after for component");

        Statement for_body = _statement();

        if (incrementer is not null)
        {
            for_body = new Block([
                for_body,
                new ExpressionStmt(incrementer)
            ]);
        }

        condition ??= new Literal(true);

        for_body = new WhileStmt(condition, for_body);

        if (initializer is not null)
        {
            for_body = new Block([
                initializer,
                for_body
            ]);
        }

        return for_body;
    }

    private Statement _printStatement()
    {
        Expression value = _expression();
        _consume(TokenType.SEMICOLON, "Expect ';' after value");

        return new PrintStmt(value);
    }

    private Statement _returnStatement()
    {
        Token keyword = _previous();
        Expression? value = null;

        if (!_check(TokenType.SEMICOLON))
        {
            value = _expression();
        }

        _consume(TokenType.SEMICOLON, "Expect ';' after an expression.");

        return new ReturnStmt(keyword, value);
    }

    private Statement _expressionStatement()
    {
        Expression expr = _expression();
        _consume(TokenType.SEMICOLON, "Expect ';' after expression");

        return new ExpressionStmt(expr);
    }

    private Expression _expression()
    {
        return _assignment();
    }

    private Expression _assignment()
    {
        Expression expr = _or();

        if (_match(TokenType.EQUAL))
        {
            Token equal_to = _previous();
            Expression value = _assignment();

            if (expr is Variable target_var)
            {
                Token Name = target_var.Name;
                return new Assign(Name, value);
            }

            Error.Report(equal_to.line, "Invalid assignment target");
        }

        return expr;
    }

    private Expression _comma()
    {
        Expression expr = _equality();

        while (_match(TokenType.COMMA))
        {
            Token @operator = _previous();
            Expression right = _equality();
            expr = new Comma(right);
        }

        return expr;
    }

    private Expression _or()
    {
        Expression expr = _and();

        while (_match(TokenType.OR))
        {
            Token @operator = _previous();
            Expression right = _and();

            expr = new Logical(expr, @operator, right);
        }

        return expr;
    }

    private Expression _and()
    {
        Expression expr = _equality();

        while (_match(TokenType.AND))
        {
            Token @operator = _previous();
            Expression right = _equality();

            expr = new Logical(expr, @operator, right);
        }

        return expr;
    }

    private Expression _equality()
    {
        Expression expr = comparison();

        while (_match(TokenType.BANG_EQUAL, TokenType.EQUAL_EQUAL))
        {
            Token @operator = _previous();
            Expression right = comparison();
            expr = new Binary(expr, @operator, right);
        }

        return expr;
    }

    private Expression comparison()
    {
        Expression expr = _term();

        while (_match(
            TokenType.LESS, TokenType.LESS_EQUAL,
            TokenType.GREATER, TokenType.GREATER_EQUAL
        ))
        {
            Token @operator = _previous();
            Expression right = _term();
            expr = new Binary(expr, @operator, right);
        }

        return expr;
    }

    private Expression _term()
    {
        Expression expr = _factor();

        while (_match(TokenType.PLUS, TokenType.MINUS))
        {
            Token @operator = _previous();
            Expression right = _factor();
            expr = new Binary(expr, @operator, right);
        }

        return expr;
    }

    private Expression _factor()
    {
        Expression expr = _unary();

        while (_match(TokenType.SLASH, TokenType.STAR))
        {
            Token @operator = _previous();
            Expression right = _unary();
            expr = new Binary(expr, @operator, right);
        }

        return expr;
    }

    private Expression _unary()
    {
        if (_match(TokenType.BANG, TokenType.MINUS))
        {
            Token @operator = _previous();
            Expression right = _unary();
            return new Unary(@operator, right);
        }

        return _call();
    }

    private Expression _call()
    {
        Expression expr = _primary();

        for (; ; )
        {
            if (_match(TokenType.LEFT_PAREN))
            {
                expr = _finishCall(expr);
            }
            else
            {
                break;
            }
        }
        return expr;
    }

    private Expression _primary()
    {
        if (_match(TokenType.FALSE)) return new Literal(false);
        if (_match(TokenType.TRUE)) return new Literal(true);
        if (_match(TokenType.NULL)) return new Literal(null);

        if (_match(TokenType.NUMBER, TokenType.STRING))
        {
            return new Literal(_previous().literal);
        }

        if (_match(TokenType.IDENTIFIER)) return new Variable(_previous());

        if (_match(TokenType.LEFT_PAREN))
        {
            Expression expr = _expression();
            _consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
            return new Grouping(expr);
        }

        throw _error(_peek(), "Expect expression");
    }

    private Expression _finishCall(Expression expr)
    {
        List<Expression> arguments = [];

        if (!_check(TokenType.RIGHT_PAREN))
        {
            do
            {
                if (arguments.Count > 255)
                {
                    Error.Report(_peek().line, "Can't have more than 255 arguments");
                }
                arguments.Add(_expression());
            } while (_match(TokenType.COMMA));
        }

        Token paren = _consume(TokenType.RIGHT_PAREN, "Expect ')' after arguments");

        return new Call(expr, paren, arguments);
    }

    private Token _consume(TokenType type, String message)
    {
        if (_check(type)) return _advance();

        throw _error(_peek(), message);
    }

    private ParseError _error(Token token, String message)
    {
        if (token.type == TokenType.EOF) Error.ReportWhere(token.line, "at the end", message);
        else Error.ReportWhere(token.line, "at '" + token.literal + "' ", message);

        return new ParseError();
    }

    private bool _match(params TokenType[] types)
    {
        foreach (TokenType type in types)
        {
            if (_check(type))
            {
                _advance();
                return true;
            }
        }
        return false;
    }

    private bool _check(TokenType type)
    {
        if (_isEOF()) return false;
        return _peek().type == type;
    }

    private Token _advance()
    {
        if (!_isEOF()) current++;
        return _previous();
    }

    private bool _isEOF()
    {
        if (_peek().type == TokenType.EOF) return true;
        return false;
    }

    private Token _peek() => tokens[current];

    private Token _previous() => tokens[current - 1];

    private void _synchronize()
    {
        _advance();

        while (!_isEOF())
        {
            if (_previous().type == TokenType.SEMICOLON) return;

            switch (_peek().type)
            {
                case TokenType.IF:
                case TokenType.ELSE:
                case TokenType.FOR:
                case TokenType.WHILE:
                case TokenType.PRINT:
                case TokenType.VAR:
                case TokenType.FUNC:
                case TokenType.RETURN:
                    return;
            }

            _advance();
        }
    }

}
