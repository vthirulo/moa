
using System;
using System.IO;

namespace Moa
{
    class Scanner
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
            } catch (FileNotFoundException) {
                Console.WriteLine("moa file not found !");
                Environment.Exit(-1);
            }

            Console.WriteLine("Reading " + filename + "...");

            var ienum = File.ReadLines(filename).GetEnumerator();
            while (ienum.MoveNext())
            {
                var line = ienum.Current;

                Run(line);
            }
            }

        public static void Prompt()
        {
            for (;;)
            {
                Console.Write("=> ");
                string? line = Console.ReadLine();

                if (line == null) break;

                Run(line);
            }
        }

        private static void Run(string words)
        {

        }
    }
}
