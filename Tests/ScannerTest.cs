
namespace Moa.Tests;

using MoaScanner = TreeWalk.Scanner;
using MoaTokenType = TreeWalk.TokenType;

public class ScannerTest
{
    private MoaScanner _scanner = null!;

    [Theory]
    [InlineData("(", MoaTokenType.LEFT_PAREN)]
    [InlineData(")", MoaTokenType.RIGHT_PAREN)]
    [InlineData("{", MoaTokenType.LEFT_BRACE)]
    [InlineData("}", MoaTokenType.RIGHT_BRACE)]
    [InlineData(",", MoaTokenType.COMMA)]
    [InlineData(".", MoaTokenType.DOT)]
    [InlineData("-", MoaTokenType.MINUS)]
    [InlineData("+", MoaTokenType.PLUS)]
    [InlineData("/", MoaTokenType.SLASH)]
    [InlineData("*", MoaTokenType.STAR)]
    [InlineData(";", MoaTokenType.SEMICOLON)]
    [InlineData("!", MoaTokenType.BANG)]
    [InlineData("=", MoaTokenType.EQUAL)]
    [InlineData(">", MoaTokenType.GREATER)]
    [InlineData("<", MoaTokenType.LESS)]
    public void SingleCharacterTest(string test_string, MoaTokenType expected_type)
    {
        _scanner = new(test_string);

        var _tokens = _scanner.ScanTokens();

        Assert.Equal(2, _tokens.Count);

        Assert.Multiple(
            () => Assert.Equal(expected_type, _tokens[0].type),
            () => Assert.Equal(test_string, _tokens[0].lexeme),
            () => Assert.Equal(MoaTokenType.EOF, _tokens[1].type)
        );
    }

    [Theory]
    [InlineData("!=", MoaTokenType.BANG_EQUAL)]
    [InlineData("==", MoaTokenType.EQUAL_EQUAL)]
    [InlineData(">=", MoaTokenType.GREATER_EQUAL)]
    [InlineData("<=", MoaTokenType.LESS_EQUAL)]
    public void DoubleCharacterTest(string test_string, MoaTokenType expected_type)
    {
        _scanner = new(test_string);

        var _tokens = _scanner.ScanTokens();

        Assert.Equal(2, _tokens.Count);
        Assert.Equal(2, _tokens[0].lexeme.Length);

        Assert.Multiple(
            () => Assert.Equal(test_string[0], _tokens[0].lexeme[0]),
            () => Assert.Equal(test_string[1], _tokens[0].lexeme[1]),
            () => Assert.Equal(expected_type, _tokens[0].type),
            () => Assert.Equal(MoaTokenType.EOF, _tokens[1].type)
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Whitespace(string test_whitespace)
    {
        _scanner = new(test_whitespace);

        var _tokens = _scanner.ScanTokens();

        Assert.Single(_tokens);
        Assert.Equal(MoaTokenType.EOF, _tokens[0].type);
    }
}
