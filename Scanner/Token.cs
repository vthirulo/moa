
namespace Moa;

class Token
{
    private TokenType type;
    private String? lexeme;
    private Object? literal;

    private int line;

    public Token(TokenType type, String lexeme, Object? literal, int line)
    {
        this.type = type;
        this.lexeme = lexeme;
        this.literal = literal;
        this.line = line;
    }

    public String toString()
    {
        return "TYPE - " + type + " | Lexeme - " + lexeme + " | Literal - " + literal;
    }
}


enum TokenType
{
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SLASH, STAR, SEMICOLON,

    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,

    IDENTIFIER, STRING, NUMBER, NULL,

    AND, OR, NOT, IF, ELSE, WHILE, VAR, FOR, FUNC,
    PRINT, RETURN,

    TRUE, FALSE,

    EOF
};
