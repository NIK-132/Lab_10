using System.Collections.Generic;

namespace Компилятор
{
    class Keywords
    {
        private Dictionary<byte, Dictionary<string, byte>> _keywords;

        public Dictionary<byte, Dictionary<string, byte>> KeywordsTable
        {
            get 
            { 
                return _keywords; 
            }
        }

        public Keywords()
        {
            _keywords = new Dictionary<byte, Dictionary<string, byte>>();

            var tmp = new Dictionary<string, byte>();
            tmp["do"] = LexicalAnalyzer.dosy;
            tmp["if"] = LexicalAnalyzer.ifsy;
            tmp["in"] = LexicalAnalyzer.insy;
            tmp["of"] = LexicalAnalyzer.ofsy;
            tmp["or"] = LexicalAnalyzer.orsy;
            tmp["to"] = LexicalAnalyzer.tosy;
            _keywords[2] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["end"] = LexicalAnalyzer.endsy;
            tmp["var"] = LexicalAnalyzer.varsy;
            tmp["div"] = LexicalAnalyzer.divsy;
            tmp["and"] = LexicalAnalyzer.andsy;
            tmp["not"] = LexicalAnalyzer.notsy;
            tmp["for"] = LexicalAnalyzer.forsy;
            tmp["mod"] = LexicalAnalyzer.modsy;
            tmp["nil"] = LexicalAnalyzer.nilsy;
            tmp["set"] = LexicalAnalyzer.setsy;
            _keywords[3] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["then"] = LexicalAnalyzer.thensy;
            tmp["else"] = LexicalAnalyzer.elsesy;
            tmp["case"] = LexicalAnalyzer.casesy;
            tmp["file"] = LexicalAnalyzer.filesy;
            tmp["goto"] = LexicalAnalyzer.gotosy;
            tmp["type"] = LexicalAnalyzer.typesy;
            tmp["with"] = LexicalAnalyzer.withsy;
            _keywords[4] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["begin"] = LexicalAnalyzer.beginsy;
            tmp["while"] = LexicalAnalyzer.whilesy;
            tmp["array"] = LexicalAnalyzer.arraysy;
            tmp["const"] = LexicalAnalyzer.constsy;
            tmp["label"] = LexicalAnalyzer.labelsy;
            tmp["until"] = LexicalAnalyzer.untilsy;
            _keywords[5] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["downto"] = LexicalAnalyzer.downtosy;
            tmp["packed"] = LexicalAnalyzer.packedsy;
            tmp["record"] = LexicalAnalyzer.recordsy;
            tmp["repeat"] = LexicalAnalyzer.repeatsy;
            _keywords[6] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["program"] = LexicalAnalyzer.programsy;
            _keywords[7] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["function"] = LexicalAnalyzer.functionsy;
            _keywords[8] = tmp;

            tmp = new Dictionary<string, byte>();
            tmp["procedure"] = LexicalAnalyzer.procedurensy;
            _keywords[9] = tmp;
        }
    }
}