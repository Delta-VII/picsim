using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions
{
    internal abstract class Instruction
    {
        protected int _instruction;
        protected Pic _pic;
        public abstract void Decode();

        public abstract void Execute();
    }
}
