using System.Collections.Generic;

namespace Компилятор
{
    static class ErrorTable
    {
        static readonly Dictionary<byte, string> 
            errors = new Dictionary<byte, string>
        {
            { 1, "недопустимый символ" },
            { 100, "использование имени не соответствует описанию" },
            { 102, "пустая символьная константа" },
            { 103, "отсутствует закрывающая кавычка в символьной константе" },
            { 104, "некорректная символьная константа" },
            { 105, "незакрытая круглая скобка" },
            { 106, "лишняя закрывающая круглая скобка" },
            { 147, "тип метки не совпадает с типом выбирающего выражения" },
            { 203, "целая константа превышает допустимый диапазон" },
        };

        public static string GetDescription(byte code)
        {
            return errors.TryGetValue(code, out string desc) ? desc : null;
        }
    }
}