

namespace Moa;

class RuntimeException : SystemException
{
    public Token token;

    public RuntimeException(Token token, String message) : base(message)
    {
        this.token = token;
    }
}