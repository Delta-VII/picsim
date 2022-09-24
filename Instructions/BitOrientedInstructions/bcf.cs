using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal class bcf : BitOrientedInstructions
    {
        public bcf(int instruction, Pic uc)
        {
            _instruction = instruction;
            _uc = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = _instruction & _bBitmask;
        }

        public override void Execute()
        {
            Decode();
            this._uc.WriteBit(false, _b, _f);
            _uc.IncProgCounter(false);
        }

    }
}
