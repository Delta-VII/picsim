using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class retfie : ControlOperations
    {
        public retfie(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
        }

        public override void Execute()
        {
            _pic.ProgCntr = _pic.pop();
            _pic.RamBank0[0x0B].Value |= 0b_1000_0000;
            _pic.Timercycle();
            _pic.Timercycle();
        }
    }
}
