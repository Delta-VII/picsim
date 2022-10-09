using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal abstract class ByteOrientedInstructions : Instruction
    {
        protected int _f;
        protected int _d;
        protected int _fBitmask = 0b_00_0000_0111_1111;
        protected int _dBitmask = 0b_00_0000_1000_0000;
    }
}
