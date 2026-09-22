
/*
expression     → equality ;
comma          → equality ((",") equality)* ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
*/

namespace Moa
{

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

        public Expression Parse()
        {
            try
            {
                return expression();
            }
            catch (ParseError parse_error)
            {
                return null;
            }
        }

        private Expression expression()
        {
            return comma();
        }

        private Expression comma()
        {
            Expression expr = equality();

            while (_match(TokenType.COMMA))
            {
                Token @operator = _previous();
                Expression right = equality();
                expr = new Comma(right);
            }

            return expr;
        }

        private Expression equality()
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
            Expression expr = term();

            while (_match(
                TokenType.LESS, TokenType.LESS_EQUAL,
                TokenType.GREATER, TokenType.GREATER_EQUAL
            ))
            {
                Token @operator = _previous();
                Expression right = term();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression term()
        {
            Expression expr = factor();

            while (_match(TokenType.PLUS, TokenType.MINUS))
            {
                Token @operator = _previous();
                Expression right = factor();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression factor()
        {
            Expression expr = unary();

            while (_match(TokenType.SLASH, TokenType.STAR))
            {
                Token @operator = _previous();
                Expression right = unary();
                expr = new Binary(expr, @operator, right);
            }

            return expr;
        }

        private Expression unary()
        {
            if (_match(TokenType.BANG, TokenType.MINUS))
            {
                Token @operator = _previous();
                Expression right = unary();
                return new Unary(@operator, right);
            }

            return primary();
        }

        private Expression primary()
        {
            if (_match(TokenType.FALSE)) return new Literal(false);
            if (_match(TokenType.TRUE)) return new Literal(true);
            if (_match(TokenType.NULL)) return new Literal(null);

            if (_match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Literal(_previous().literal);
            }

            if (_match(TokenType.LEFT_PAREN))
            {
                Expression expr = expression();
                _consume(TokenType.RIGHT_PAREN, "Expected ')' after expression");
                return new Grouping(expr);
            }

            throw _error(_peek(), "Expect expression");
        }

        private Token _consume(TokenType type, String message)
        {
            if (_check(type)) return _advance();

            throw _error(_peek(), message);
        }

        private ParseError _error(Token token, String message)
        {
            if (token.type == TokenType.EOF) Error.reportWhere(token.line, "at the end", message);
            else Error.reportWhere(token.line, "at '" + token.literal + "' ", message);

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

    }
}

