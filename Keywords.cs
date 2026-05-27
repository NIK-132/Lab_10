using System.Collections.Generic;

namespace Компилятор
{
    class Keywords
    {
        private static Dictionary<byte, Dictionary<string, byte>> _keywords;

        static Keywords()
        {
            _keywords = new Dictionary<byte, Dictionary<string, byte>>();

            var tmp = new Dictionary<string, byte>();
            tmp["do"] = LexicalAnalyzer.Dosy;
            tmp["if"] = LexicalAnalyzer.Ifsy;
            tmp["in"] = LexicalAnalyzer.Insy;
            tmp["of"] = LexicalAnalyzer.Ofsy;
            tmp["or"] = LexicalAnalyzer.Orsy;
            tmp["to"] = LexicalAnalyzer.Tosy;
            _keywords[2] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["end"] = LexicalAnalyzer.Endsy;
            tmp["var"] = LexicalAnalyzer.Varsy;
            tmp["div"] = LexicalAnalyzer.Divsy;
            tmp["and"] = LexicalAnalyzer.Andsy;
            tmp["not"] = LexicalAnalyzer.Notsy;
            tmp["for"] = LexicalAnalyzer.Forsy;
            tmp["mod"] = LexicalAnalyzer.Modsy;
            tmp["nil"] = LexicalAnalyzer.Nilsy;
            tmp["set"] = LexicalAnalyzer.Setsy;
            _keywords[3] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["then"] = LexicalAnalyzer.Thensy;
            tmp["else"] = LexicalAnalyzer.Elsesy;
            tmp["case"] = LexicalAnalyzer.Casesy;
            tmp["file"] = LexicalAnalyzer.Filesy;
            tmp["goto"] = LexicalAnalyzer.Gotosy;
            tmp["type"] = LexicalAnalyzer.Typesy;
            tmp["with"] = LexicalAnalyzer.Withsy;
            _keywords[4] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["begin"] = LexicalAnalyzer.Beginsy;
            tmp["while"] = LexicalAnalyzer.Whilesy;
            tmp["array"] = LexicalAnalyzer.Arraysy;
            tmp["const"] = LexicalAnalyzer.Constsy;
            tmp["label"] = LexicalAnalyzer.Labelsy;
            tmp["until"] = LexicalAnalyzer.Untilsy;
            _keywords[5] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["downto"] = LexicalAnalyzer.Downtosy;
            tmp["packed"] = LexicalAnalyzer.Packedsy;
            tmp["record"] = LexicalAnalyzer.Recordsy;
            tmp["repeat"] = LexicalAnalyzer.Repeatsy;
            _keywords[6] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["program"] = LexicalAnalyzer.Programsy;
            _keywords[7] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["function"] = LexicalAnalyzer.Functionsy;
            _keywords[8] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["procedure"] = LexicalAnalyzer.Procedurensy;
            _keywords[9] = tmp;
        }

        public static byte GetCode(string name)
        {
            if (name == null) return 0;
            byte len = (byte)name.Length;
            if (_keywords.TryGetValue(len, out var dict))
            {
                if (dict.TryGetValue(name, out byte code))
                {
                    return code;
                }
            }
            return 0;
        }
    }
}