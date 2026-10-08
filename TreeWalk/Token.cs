
namespace Moa.TreeWalk;

class Token(TokenType type, String lexeme, Object? literal, int line)
{
    public TokenType type = type;
    public string lexeme = lexeme;
    public object? literal = literal;

    public int line = line;
}


public enum TokenType
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
