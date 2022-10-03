using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class swapf : ByteOrientedInstructions
    {
        public swapf(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _d = _instruction & _dBitmask;
        }

        public override void Execute()
        {
            Decode();
            var register = _pic.GetByte(_f);
            var result = ((register & 0x0F) << 4 | (register & 0xF0) >> 4);
            _pic.WriteResult(_d, _f, result);
            _pic.IncRuntime(false);
        }
    }
}