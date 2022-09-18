using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal abstract class BitOrientedInstructions
    {
        private int _f;
        private int _b;
        private bool _isTwoCycleOperation;
        private int _Instructionbitmask;
        private int _instruction;
        private int _fBitmask;
        private int _bBitmask;
        private Pic uc;
        

        public abstract void Decode();

        public abstract void Execute();

    }
}
