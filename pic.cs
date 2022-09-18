using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim
{
    internal partial class Pic
    {
        private int[] _ramBank0 = new int[128];
        private int[] _ramBank1 = new int[128];
        private int[] _programMemory = new int[1024];
        private int _wreg;
        private int _programCounter;
        private int _instructionRegister;
        private Stack _stack = new Stack();
        private int _stackpointer;
        private int[] _eeprom = new int[64];

        public void SetIRPFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << Irp);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << Irp;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public void SetToFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << To);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << To;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public void SetPdFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << Pd);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << Pd;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public void SetZFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << Z);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << Z;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public void SetDCFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << Dc);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << Dc;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public void SetCFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status] &= ~(1 << C);
            }
            else if (value == true)
            {
                _ramBank0[Status] |= 1 << C;
            }

            _ramBank1[0x83] = _ramBank0[Status];
        }

        public bool GetIrpFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[Irp];
        }

        public bool GetToFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[To];
        }

        public bool GetPdFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[Pd];
        }

        public bool GetZFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[Z];
        }

        public bool GetDCFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[Dc];
        }

        public bool GetCFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[C];
        }

        public bool GetRp0()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status] });
            return b[Rp0];
        }

        public void WriteBit(bool value, int bitNumber, int address)
        {
            if ((GetRp0() == false) && (address <= 128))
            {
                if (value == false)
                {
                    _ramBank0[address] &= ~(1 << bitNumber);
                }
                else if (value == true)
                {
                    _ramBank0[address] |= 1 << bitNumber;
                }
            }
            else if ((GetRp0() == true) && (address >= 128))
            {
                if (value == false)
                {
                    _ramBank1[address] &= ~(1 << bitNumber);
                }
                else if (value == true)
                {
                    _ramBank1[address] |= 1 << bitNumber;
                }
            }
        }

        public bool ReadBit(int address, int bit)
        {
            BitArray b = new BitArray(new int[] { _ramBank0[address] });
            return b[bit];
        }

        public void WriteByte(int address, int value)
        {
            if ((GetRp0() == false) && (address <= 128))
            {
                _ramBank0[address] = value;
            }
            else if ((GetRp0() == true) && (address >= 128))
            {
                _ramBank1[address] = value;
            }
        }

        public int GetByte(int address)
        {
            int returnValue = 0;
            if ((GetRp0() == false) && (address <= 128))
            {
                returnValue = _ramBank0[address];
            }
            else if ((GetRp0() == true) && (address >= 128))
            {
                returnValue = _ramBank1[address];
            }
            return returnValue;
        }
    }
}
