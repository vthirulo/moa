
namespace Moa.TreeWalk;

using Moa.TreeWalk.Utils.Errors;

class Scanner
{
    private readonly string _source;
    private readonly List<Token> _tokens = [];
    private static readonly Dictionary<string, TokenType> _keywords;

    private int start, current;
    private int line;

    static Scanner()
    {
        _keywords = new Dictionary<string, TokenType>
            {
                { "and",    TokenType.AND },
                { "or",     TokenType.OR },
                { "not",    TokenType.NOT },
                { "if",     TokenType.IF },
                { "else",   TokenType.ELSE },
                { "while",  TokenType.WHILE },
                { "for",    TokenType.FOR },
                { "func",   TokenType.FUNC },
                { "print",  TokenType.PRINT },
                { "return", TokenType.RETURN },
                { "null",   TokenType.NULL },
                { "var",    TokenType.VAR },
                { "true",   TokenType.TRUE },
                { "false",   TokenType.FALSE },
            };
    }

    public Scanner(String source)
    {
        this._source = source;

        start = current = 0;
        line = 1;
    }

    public List<Token> ScanTokens()
    {
        while (!this.isAtEnd())
        {
            start = current;
            this.scanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, line));
        return _tokens;
    }


    private void scanToken()
    {
        char c = this.advance();

        switch (c)
        {
            case '(': addToken(TokenType.LEFT_PAREN); break;
            case ')': addToken(TokenType.RIGHT_PAREN); break;
            case '{': addToken(TokenType.LEFT_BRACE); break;
            case '}': addToken(TokenType.RIGHT_BRACE); break;
            case ',': addToken(TokenType.COMMA); break;
            case '.': addToken(TokenType.DOT); break;
            case '-': addToken(TokenType.MINUS); break;
            case '+': addToken(TokenType.PLUS); break;
            case '*': addToken(TokenType.STAR); break;
            case ';': addToken(TokenType.SEMICOLON); break;

            case '!': addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG); break;
            case '=': addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL); break;
            case '<': addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS); break;
            case '>': addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER); break;

            case ' ':
            case '\t':
            case '\r':
                break;

            case '\n':
                line++;
                break;

            case '/':
                if (match('/'))
                {
                    while (peek() != '\n' && !isAtEnd()) advance();
                }
                else if (match('*'))
                {
                    while (!isAtEnd())
                    {
                        if (peek() == '*' && peekNext() == '/')
                        {
                            advance(); // *
                            advance(); // /
                            break;
                        }
                        if (peek() == '\n') line++;
                        advance();

                        if (isAtEnd()) Error.Report(line, "C style comment not terminated");
                    }

                }
                else
                {
                    addToken(TokenType.SLASH);
                }
                break;

            case '"':
                stringLiteral();
                break;

            default:
                if (isDigit(c))
                {
                    number();
                }
                else if (isAlpha(c))
                {
                    identifier();
                }
                else
                {
                    Error.Report(line, "Unexpected Char - " + c); break;
                }
                break;
        }
    }

    private bool isAtEnd() => current >= _source.Length;

    private char advance() => _source[current++];

    private void addToken(TokenType type) => addToken(type, null);

    private void addToken(TokenType type, Object? literal)
    {
        string lexeme = _source.Substring(start, current - start);
        _tokens.Add(new Token(type, lexeme, literal, line));
    }

    private bool match(char expected)
    {
        if (isAtEnd()) return false;
        if (_source[current] != expected) return false;

        current++;
        return true;
    }

    private char peek()
    {
        if (isAtEnd()) return '\0';
        return _source[current];
    }

    private char peekNext()
    {
        if (current + 1 >= _source.Length) return '\0';
        return _source[current + 1];
    }

    private void stringLiteral()
    {
        while (peek() != '"' && !isAtEnd())
        {
            if (peek() == '\n') line++;

            advance();
        }

        if (isAtEnd())
        {
            Error.Report(line, "Unterminated String");
            return;
        }

        advance();

        int literal_length = current - start; // includes " and the char after "
        addToken(TokenType.STRING, _source.Substring(start + 1, literal_length - 2));
    }

    private bool isDigit(char c) => c >= '0' && c <= '9';

    private void number()
    {
        while (isDigit(peek())) advance();

        if (peek() == '.' && isDigit(peekNext()))
        {
            advance();
            while (isDigit(peek())) advance();
        }

        int literal_length = current - start; // includes " and the char after "
        addToken(TokenType.NUMBER, Double.Parse(_source.Substring(start, literal_length)));
    }

    private bool isAlpha(char c) =>
        (c >= 'a' && c <= 'z') ||
        (c >= 'A' && c <= 'Z') ||
        (c == '_');

    private bool isAlphaNumeric(char c) => isAlpha(c) || isDigit(c);

    private void identifier()
    {
        while (isAlphaNumeric(peek())) advance();

        String identifier = _source.Substring(start, current - start);

        TokenType identifier_type;

        if (!_keywords.TryGetValue(identifier, out identifier_type))
        {
            identifier_type = TokenType.IDENTIFIER;
        }

        addToken(identifier_type);
    }

}
