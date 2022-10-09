using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim.Instructions.ControlOperations
{
    internal class sleep: ControlOperations
    {
        public sleep(int instruction, Pic uc)
        {
            _instruction = instruction;
            _pic = uc;
        }

        public override void Decode()
        {
        }

        public override void Execute()
        {
            if((_pic.RamBank1[3].Value & 0b00001000)==8)
            {
                _pic.Cyclecount1 = 500;
                _pic.Quarzcycle1 = 0;}
                _pic.RamBank0[3].Value &= 0b11110111;
            if((_pic.RamBank1[3].Value & 0b00001000) == 0)
            {
                _pic.watchdogcycle();
                if ((_pic.RamBank1[3].Value & 0b00001000) == 0)
                {
                    _pic.ProgCntr--;
                }
            }
        }
    }
}
