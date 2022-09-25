using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal abstract class LiteralOperations : Instruction
    {
        protected int _kBitmask = 0b_00_0000_1111_1111;
        protected int _k;
       
    }
}
