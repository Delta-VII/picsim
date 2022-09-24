using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.LiteralOperations
{
    internal abstract class LiteralOperations
    {
        
        protected int _instruction;
        protected int _kBitmask = 0b_00_0000_1111_1111;
        protected int _k;
        protected Pic _uc;
        
        public abstract void Decode();

        public abstract void Execute();
    }
}
