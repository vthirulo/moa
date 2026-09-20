
namespace Moa;

public class Interpreter
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
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        Parser parser = new Parser(tokens);

        Expression parsed_expr = parser.Parse();

        if (Error.Status)
        {
            Console.WriteLine("asd");
            return;
        }

        Console.WriteLine(new AstPrinter().print(parsed_expr));
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
