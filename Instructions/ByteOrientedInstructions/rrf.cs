using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class rrf : ByteOrientedInstructions
    {
        public rrf(int instruction, Pic uc)
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
            if ((_pic.RamBank0[3].Value & 0b_0000_0001) == 1)
            {
                register |= 0b_1_0000_0000;
            }

            if ((register & 0b_0000_0001) == 1)
            {
                _pic.SetCFlag(true);
            }
            
            if ((register & 0b00000001) == 0)
            {
                _pic.SetCFlag(false);
            }
            register = register >> 1;
            register = register & 0b_1111_1111;
            _pic.WriteResult(_d,_f,register);
            _pic.Timercycle();
        }
    }
}
