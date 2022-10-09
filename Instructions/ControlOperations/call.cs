using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class call : ControlOperations
    {
        public call(int instruction, Pic uc)
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
            _pic.push(_pic.ProgCntr + 1);
            var pclath = _pic.RamBank0[0x0A].Value << 8;
            var result = pclath + _k;
            _pic.ProgCntr = result - 1;
            _pic.Timercycle();
            _pic.Timercycle();
        }
    }
}
