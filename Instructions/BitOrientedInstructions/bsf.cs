using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal class bsf : BitOrientedInstructions
    {
        public bsf(int instruction, Pic uc)
        {
            this._instruction = instruction;
            this._uc = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = _instruction & _bBitmask;
        }

        public override void Execute()
        {
            Decode();
            this._uc.WriteBit(true, _b, _f);
            _uc.IncProgCounter(false);
        }
    }
}
