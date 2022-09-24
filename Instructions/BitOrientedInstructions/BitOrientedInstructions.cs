using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal abstract class BitOrientedInstructions
    {
        protected int _f;
        protected int _b;
        protected int _instruction;
        protected int _fBitmask = 0b_00_0000_0111_1111;
        protected int _bBitmask = 0b_00_0011_1000_0000;
        protected Pic _uc;
        

        public abstract void Decode();

        public abstract void Execute();

    }
}
