using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class subwf : ByteOrientedInstructions
    {
        public subwf(int instruction, Pic uc)
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
            var wreg = _pic.Wreg;
            var w_2comp = (~wreg + 1) & 0xff;
            var result = register + w_2comp;
            _pic.ZFlag(result);
            _pic.CFlag(result);
            _pic.DcFlag(register,w_2comp);
            result &= 0b_1111_1111;
            _pic.WriteResult(_d, _f, result);
            _pic.Timercycle();
            
        }
    }
}