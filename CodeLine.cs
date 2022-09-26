using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim
{
    public class CodeLine
    {
        public int LineNumber { get; set; }
        public string Code { get; set; }
        public bool IsBreakPoint { get; set; }
        public bool IsActiveLine { get; set; }

        public CodeLine(string line)
        {
            Code = line;
            LineNumber = 1;
            IsActiveLine = false;
            IsBreakPoint = false;
        }
    }
}
