using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal class addlw : LiteralOperations
    {
        
        public addlw(int instruction, Pic uc)
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
            var result = _pic.Wreg + _k;
            if (result == 0)
            {
                
            }
        }
        
    }
}
