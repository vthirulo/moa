
namespace Moa.TreeWalk.Utils.Errors;

class Error
{
    public static void Report(int line, string message)
    {
        Console.Error.WriteLine("[line " + line + "] " + message);
        _hadError = true;
    }

    public static void ReportWhere(int line, string where, string message)
    {
        Console.Error.WriteLine("[line " + line + "] Error " + where + ": " + message);
        _hadError = true;
    }

    public static void ReportRuntimeError(RuntimeException err)
    {
        Console.Error.WriteLine("[line " + err.token.line + "] " + err.Message);
        _hadRuntimeError = true;
    }

    private static bool _hadError = false;

    private static bool _hadRuntimeError = false;

    public static bool Status => _hadError;

    public static bool RuntimeStatus => _hadRuntimeError;
}