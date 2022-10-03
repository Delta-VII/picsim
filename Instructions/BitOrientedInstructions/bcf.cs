using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal class bcf : BitOrientedInstructions
    {
        public bcf(int instruction, Pic pic)
        {
            _instruction = instruction;
            _pic = pic;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = _instruction & _bBitmask;
        }

        public override void Execute()
        {
            Decode();
            _pic.WriteBit(false, _b, _f);
            _pic.IncRuntime(false);
        }
    }
}