using System;
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

            try
            {
                InputOutput.LoadFile(inputFile);

                LexicalAnalyzer lex = new LexicalAnalyzer();
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    while (!InputOutput.EndOfFile)
                    {
                        byte sym = lex.NextSym();
                        if (sym == 0) break;

                        writer.Write(sym + " ");
                    }
                }

                Console.WriteLine("Лексический анализ завершён. " +
                    "Результат в файле " + outputFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
        }

        private static void CreateTestFile(string path)
        {
            string[] lines = {
                "program example;",
                "var a, b: integer;",
                "ch, s: char;",
                "const c = 10;",
                "ch := 'ab';",
                "s := 'a;",
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