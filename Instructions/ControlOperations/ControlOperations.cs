using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal abstract class ControlOperations : Instruction
    {
        protected int _kBitmask = 0b_00_0000_1111_1111;
        protected int _kBitmaskGotoCall = 0b_00_0011_1111_1111;
        protected int _k;
    }
}