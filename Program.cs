
using Moa.AST;
using Moa.Utils.Errors;

using MoaScanner = Moa.Scanner.Scanner;
using MoaParser = Moa.Parser.Parser;
using MoaInterpreter = Moa.Interpreter.Interpreter;

public class Program
{
    public static void ReadFile(string filename)
    {
        try
        {
            if (filename[^4..] != ".moa")
            {
                Console.WriteLine("This is not a moa file !");
                Environment.Exit(-1);
            }

            using var fstream = new FileStream(filename, FileMode.Open);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("moa file not found !");
            Environment.Exit(-1);
        }

        Console.WriteLine("Reading " + filename + "...");

        Run(File.ReadAllText(filename));
    }

    public static void Prompt()
    {
        for (; ; )
        {
            Console.Write("=> ");
            string? line = Console.ReadLine();

            if (line == null) break;

            Run(line);
        }
    }

    private static void Run(string source)
    {
        MoaScanner scanner = new(source);
        List<Moa.Scanner.Token> tokens = scanner.ScanTokens();
        MoaParser parser = new(tokens);

        List<Statement> lst_of_statements = parser.Parse();

        if (Error.Status) Environment.Exit(65);

        MoaInterpreter interpreter = new(lst_of_statements);

        if (Error.RuntimeStatus) Environment.Exit(70);
    }

    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: dotnet run [file]");
            Console.WriteLine("Usage: dotnet run");
        }
        else if (args.Length == 1)
        {
            ReadFile(args[0]);
        }
        else
        {
            Prompt();
        }
    }
}
