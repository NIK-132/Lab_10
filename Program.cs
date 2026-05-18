using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Компилятор
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("=== Тестирование модуля ввода-вывода ===\n");

            string[] sourceLines = {
                "program example ( input, output );",
                "const c = 3;",
                "b = 56;",
                "var a : 'a' ... 'c';",
                "k, i : integer;",
                "begin",
                "read( k, i );",
                "for a := 'a' to 'c' do",
                "case a of",
                "    k: i := i * k;",
                "    'b': i := i + 1;",
                "    i : k := k + 2;",
                "    b: i := i - k;",
                "    c: i := ( i + k ) * 2",
                "end;",
                "writeln( i, k )",
                "end."
            };
            File.WriteAllLines("example.pas", sourceLines);

            var errors = new List<(int line, int col, byte code)>
            {
                (10, 5, 100),
                (12, 5, 100),
                (13, 5, 147),
                (14, 5, 147)
            };

            const int LINE_WIDTH = 4;
            int lineNum = 1;
            int errCount = 0;

            for (int i = 0; i < sourceLines.Length; i++)
            {
                Console.WriteLine($"{lineNum,LINE_WIDTH} {sourceLines[i]}");
                lineNum++;

                var lineErrors = errors.Where(e => e.line == i + 1).ToList();
                if (lineErrors.Count == 0) continue;

                foreach (var e in lineErrors)
                {
                    errCount++;
                    Console.WriteLine($"**{errCount:D2}**");
                    int arrowOffset = LINE_WIDTH + 1 + (e.col - 1);
                    Console.WriteLine
                        ($"{new string(' ', arrowOffset)}^ ошибка код {e.code}");
                    string msg = e.code == 100
                        ? "использование имени не соответствует описанию"
                        : "тип метки не совпадает с типом выбирающего выражения";
                    Console.WriteLine($"{new string(' ', LINE_WIDTH + 1)}{msg}");
                }
            }

            Console.WriteLine($"\nКомпиляция окончена: ошибок — {errCount} !");
            Console.ReadKey();
        }
    }
}