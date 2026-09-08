
namespace Moa;

class Error
{
    public static void report(int line, string message)
    {
        Console.Error.WriteLine("[line " + line + "] " + message);
        hadError = true;
    }

    private static bool hadError = false;
}