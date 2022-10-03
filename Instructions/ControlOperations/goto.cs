using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class Goto : ControlOperations
    {
        public Goto(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _k = _instruction & _kBitmaskGotoCall;
        }

        public override void Execute()
        {
            Decode();
            _pic.ProgCntr = (_pic.ProgCntr & 0b00_0111_1111_1111) - 1;
        }
    }
}