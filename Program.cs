using System;
using System.Collections.Generic;
using System.IO;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {
            string inputFile = "example.pas";
            string outputFile = "tokens.txt";

            CreateTestFile(inputFile);

            InputOutput.Initialize(inputFile);
            if (InputOutput.EndOfFile)
            {
                Console.WriteLine("Файл не найден или пуст.");
                return;
            }

            LexicalAnalyzer lex = new LexicalAnalyzer();
            List<byte> tokens = new List<byte>();

            while (!InputOutput.EndOfFile)
            {
                byte code = lex.NextSym();
                if (code != 0)
                {
                    tokens.Add(code);
                }
            }

            LexicalAnalyzer.CheckParenBalance();
            InputOutput.FlushErrors();
            InputOutput.Finish();

            File.WriteAllText(outputFile, string.Join(" ", tokens));
            Console.WriteLine("Лексический анализ завершён. " +
                "Результат в файле " + outputFile);
            Console.ReadKey();
        }

        private static void CreateTestFile(string path)
        {
            string[] lines = {
                "program example;",
                "var a, b: integer;",
                "ch, s: char;",
                "const c = 10;",
                "ch := '';",
                "s := 'a;",
                "// 40000",
                "   )",
                "(",
                "begin",
                "    a := 123.;",
                "    b := a * 2 + 40000;",
                "    write(a, b) #",
                "end."
            };
            File.WriteAllLines(path, lines);
        }
    }
}