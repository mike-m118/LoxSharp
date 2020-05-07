using System;
using System.Collections.Generic;
using System.IO;

namespace loxsharp
{
    class Program
    {
        static bool hasError = false;
        static int Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }
            switch (args.Length)
            {
                case 0:
                    return RunPrompt();
                    
                case 1:
                    return RunScript(file: args[0]);
                    
                default:
                    Console.WriteLine("Usage: loxsharp [scriptFile]");
                    return 64;
            }
        
    }

        private static int RunScript(string file)
        {
            var code = File.ReadAllText(path: file);
            Run(source: code);
            return hasError ? 65 : 0;
        }

        private static int RunPrompt()
        {
            for (;;)
            {
                Console.Write("lox >");
                Run(source: Console.ReadLine());
                hasError = false;
            }
        }
        private static void Run(string source)
        {
            var scanner = new Scanner(with: source);
            List<Token> tokens = scanner.AllTokens();
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }
        }
        public static void Error(int on, string withMessage)
        {
            Report(that: withMessage, line: on, where: "");
        }

        private static void Report(string that, int line, string where)
        {
            Console.WriteLine($"[line {line}] Error{where}: {that}");
            hasError = true;
        }
    }
}
