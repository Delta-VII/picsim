using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class rlf : ByteOrientedInstructions
    {
        public rlf(int instruction, Pic uc)
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
            var register = Convert.ToUInt32(_pic.GetByte(_f));
            var result = register << 1;
            if (_pic.GetCFlag())
            {
                result++;
            }
            //_pic.SetCFlag();
        }
    }
}