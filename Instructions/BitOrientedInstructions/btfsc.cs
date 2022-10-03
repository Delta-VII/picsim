using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.BitOrientedInstructions
{
    internal class btfsc : BitOrientedInstructions
    {
        public btfsc(int instruction, Pic uc)
        {
            this._instruction = instruction;
            this._pic = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = _instruction & _bBitmask;
        }

        public override void Execute()
        {
            Decode();
            if (_pic.ReadBit(_f, _b) == true)
            {
                _pic.IncRuntime(false);
            }
            else if (_pic.ReadBit(_f, _b) == false)
            {
                _pic.IncRuntime(true);
            }
        }
    }
}