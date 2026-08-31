
using System;

public class MoaProgram
{
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: dotnet run [file]");
            Console.WriteLine("Usage: dotnet run");
        }
        else if (args.Length == 1)
        {
            Moa.Scanner.ReadFile(args[0]);
        }
        else{
            Moa.Scanner.Prompt();
        }
    }
}
