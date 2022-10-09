﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class decfsz : ByteOrientedInstructions
    {
        public decfsz(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
            _f = _instruction & _fBitmask;
            _d = _instruction & _dBitmask;
        }

        public override void Execute()
        {
            
            Decode();
            var register = _pic.GetByte(_f);
            register += 0b_1111_1111;
            register &= 0b_1111_1111;
            if (register == 0) {
                _pic.ProgCntr++;
                _pic.Timercycle();
            }
            _pic.WriteResult(_d,_f,register);
            _pic.Timercycle();
            
        }
    }
}