using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class Return : ControlOperations
    {
        public Return(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            
        }

        public override void Execute()
        {
            Decode();
            _pic.ProgCntr = _pic.pop();
            _pic.Timercycle();
            _pic.Timercycle();
        }
    }
}
