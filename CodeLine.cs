using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim
{
    public class CodeLine
    {
        private string _code;
        private bool _breakPoint;
        private bool _containsCode;
        private int _lineNumber;
        private bool _wasActive;

        public bool WasActive
        {
            get => _wasActive;
            set => _wasActive = value;
        }

        public string Code
        {
            get => _code;
            set => _code = value;
        }

        public bool BreakPoint
        {
            get => _breakPoint;
            set => _breakPoint = value;
        }

        public bool ContainsCode
        {
            get => _containsCode;
            set => _containsCode = value;
        }

        public int LineNumber
        {
            get => _lineNumber;
            set => _lineNumber = value;
        }

        public CodeLine(string line, bool code, int lineNumber)
        {
            Code = line;
            ContainsCode = code;
            LineNumber = _lineNumber;
        }
    }
}
