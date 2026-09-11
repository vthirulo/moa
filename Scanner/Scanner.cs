
using System;
using System.IO;

namespace Moa
{
    class Scanner
    {
        private String source;
        private List<Token> tokens = new List<Token>();

        private int start, current;
        private int line;

        public Scanner(String source)
        {
            this.source = source;

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

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
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
                    else
                    {
                        Error.report(line, "Unexpected Char - " + c); break;
                    }
                    break;
            }
        }

        private bool isAtEnd() => current >= source.Length;

        private char advance() => source[current++];

        private void addToken(TokenType type) => addToken(type, null);

        private void addToken(TokenType type, Object? literal)
        {
            string lexeme = source.Substring(start, current - start);
            tokens.Add(new Token(type, lexeme, literal, line));
        }

        private bool match(char expected)
        {
            if (isAtEnd()) return false;
            if (source[current] != expected) return false;

            current++;
            return true;
        }

        private char peek()
        {
            if (isAtEnd()) return '\0';
            return source[current];
        }

        private char peekNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
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
                Error.report(line, "Unterminated String");
                return;
            }

            advance();

            int literal_length = current - start; // includes " and the char after "
            addToken(TokenType.STRING, source.Substring(start + 1, literal_length - 2));
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
            addToken(TokenType.NUMBER, Double.Parse(source.Substring(start, literal_length)));
        }
    }
}

