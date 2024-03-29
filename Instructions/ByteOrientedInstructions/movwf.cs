﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ByteOrientedInstructions
{
    internal class movwf : ByteOrientedInstructions
    {
        public movwf(int instruction, Pic uc)
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
            _pic.Wreg &= 0b_1111_1111;
            _pic.WriteByte(_f, _pic.Wreg);
            _pic.Timercycle();
        }
    }
}