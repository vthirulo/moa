
namespace Moa;

class Error
{
    public static void report(int line, string message)
    {
        Console.Error.WriteLine("[line " + line + "] " + message);
        _hadError = true;
    }

    public static void reportWhere(int line, string where, string message)
    {
        Console.Error.WriteLine("[line " + line + "] Error " + where + ": " + message);
        _hadError = true;
    }

    private static bool _hadError = false;

    public static bool Status => _hadError;
}