

namespace Moa.Utils.Errors;

using Moa.Scanner;

class RuntimeException(Token token, String message) : SystemException(message)
{
    public Token token = token;
}