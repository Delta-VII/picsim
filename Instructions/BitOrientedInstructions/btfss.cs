using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal class btfss : BitOrientedInstructions
    {
        public btfss(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = (_instruction & _bBitmask) >> 7;
        }

        public override void Execute()
        {
            Decode();
            var register = _pic.GetByte(_f);
            var mask = Convert.ToInt32(Math.Pow(2, _b));
            register &= mask;
            if (register != 0) {
                _pic.ProgCntr++;
                _pic.Timercycle();
            }
            _pic.Timercycle();
        }
    }
}