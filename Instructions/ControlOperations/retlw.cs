using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class retlw : ControlOperations
    {
        public retlw(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _k = _instruction & _kBitmask;
        }

        public override void Execute()
        {
            Decode();
            _pic.ProgCntr = _pic.pop();
            _pic.Wreg = _k;
            _pic.Timercycle();
            _pic.Timercycle();
        }
    }
}
