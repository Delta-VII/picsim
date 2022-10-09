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
            var result = register | mask;
            _pic.WriteResult(1,_f,result);
            _pic.Timercycle();
        }
    }
}