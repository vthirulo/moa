

namespace Moa.TreeWalk.Utils.Errors;

using Moa.TreeWalk;

class RuntimeException(Token token, String message) : SystemException(message)
{
    public Token token = token;
}