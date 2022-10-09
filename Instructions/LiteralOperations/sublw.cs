using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal class sublw : LiteralOperations
    {
        public sublw(int instruction, Pic uc)
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
            var wreg = _pic.Wreg;
            var w_2comp = (~wreg & 0b_1111_1111) + 1;
            var result = _k + w_2comp;
            _pic.Wreg = result;
            _pic.Timercycle();
            _pic.ZFlag(result);
            _pic.CFlag(result);
            _pic.DcFlag(_k,w_2comp);
        }
    }
}