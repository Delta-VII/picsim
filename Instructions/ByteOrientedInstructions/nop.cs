using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class nop : ByteOrientedInstructions
    {
        public nop(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
        }

        public override void Execute()
        {
            _pic.Timercycle();
        }
    }
}