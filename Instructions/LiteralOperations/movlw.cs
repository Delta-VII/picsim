using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal class movlw : LiteralOperations
    {
        public movlw(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _k = _kBitmask & _instruction;
        }

        public override void Execute()
        {
            Decode();
            _pic.Wreg = _k;
            _pic.Timercycle();
        }
    }
}