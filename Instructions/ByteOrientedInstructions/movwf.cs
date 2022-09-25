using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class movwf : ByteOrientedInstructions
    {
        public movwf()
        {
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _d = _instruction & _dBitmask;
        }

        public override void Execute()
        {
            _pic.WriteByte(_f,_pic.Wreg);
            _pic.IncProgCounter(false);
        }
    }
}
