﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal class xorlw : LiteralOperations
    {
        public xorlw(int instruction, Pic uc)
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
            _pic.Wreg ^= _k;
            _pic.Wreg &= 0b_0000_0000;
            _pic.IncRuntime(false);
            _pic.ZFlag(_pic.Wreg);
        }
    }
}