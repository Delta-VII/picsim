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
            this._uc = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _b = _instruction & _bBitmask;
        }

        public override void Execute()
        {
            Decode();
            if (_uc.ReadBit(_f, _b) == true)
            {
                _uc.IncProgCounter(false);
            } else if (_uc.ReadBit(_f, _b) == false)
            {
                _uc.IncProgCounter(true);
            }
            
        }
    }
}
