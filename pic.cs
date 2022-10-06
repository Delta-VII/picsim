﻿using System.Collections;
using System.ComponentModel;


namespace picsim
{
    internal partial class Pic
    {
        private List<ramCell> _ramBank0 = new List<ramCell>();
        private List<ramCell> _ramBank1 = new List<ramCell>();
        private BindingList<StackItem> _stackDgv = new BindingList<StackItem>();
        private int _wreg;

        public int Wreg
        {
            get => _wreg;
            set => _wreg = value;
        }

        public int ProgCntr
        {
            get => _programCounter;
            set => _programCounter = value;
        }

        public int Runtime
        {
            get => _runtime;
        }

        public BindingList<StackItem> StackDGV
        {
            get => _stackDgv;
            set => _stackDgv = value;
        }

        public List<ramCell> RamBank0
        {
            get => _ramBank0;
            set => _ramBank0 = value;
        }

        public List<ramCell> RamBank1
        {
            get => _ramBank1;
            set => _ramBank1 = value;
        }

        private int _programCounter;
        private int _runtime;
        private int _instructionRegister;
        private Stack<int> _stack = new Stack<int>();
        private int _stackpointer;
        private int[] _eeprom = new int[64];

        public void push(int address)
        {
            _stack.Push(address);
            _stackDgv.Insert(0,new StackItem());
            _stackDgv[0].Value = address;
        }

        public int pop()
        {
            var address = _stack.Pop();
            _stackDgv.RemoveAt(0);
            return address;
        }

        public void SetIRPFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << Irp);
            }
            else
            {
                _ramBank0[Status].Value |= 1 << Irp;
            }
        }

        public void SetToFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << To);
            }
            else if (value == true)
            {
                _ramBank0[Status].Value |= 1 << To;
            }
        }

        public void SetPdFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << Pd);
            }
            else if (value == true)
            {
                _ramBank0[Status].Value |= 1 << Pd;
            }
        }

        public void SetZFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << 2);
            }
            else if (value)
            {
                _ramBank0[Status].Value |= 1 << 2;
            }
        }

        public void SetDCFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << 1);
            }
            else if (value)
            {
                _ramBank0[Status].Value |= 1 << 1;
            }
        }

        public void SetCFlag(bool value)
        {
            if (value == false)
            {
                _ramBank0[Status].Value &= ~(1 << 0);
            }
            else if (value)
            {
                _ramBank0[Status].Value |= 1 << 0;
            }
        }

        public bool GetIrpFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[Irp];
        }

        public bool GetToFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[To];
        }

        public bool GetPdFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[Pd];
        }

        public bool GetZFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[Z];
        }

        public bool GetDCFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[Dc];
        }

        public bool GetCFlag()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[C];
        }

        public bool GetRp0()
        {
            BitArray b = new BitArray(new int[] { _ramBank0[Status].Value });
            return b[Rp0];
        }

        public void WriteBit(bool value, int bitNumber, int address)
        {
            if ((GetRp0() == false) && (address <= 128))
            {
                if (value == false)
                {
                    _ramBank0[Status].Value &= ~(1 << bitNumber);
                }
                else if (value == true)
                {
                    _ramBank0[Status].Value |= 1 << bitNumber;
                }
            }
            else if ((GetRp0() == true) && (address >= 128))
            {
                if (value == false)
                {
                    _ramBank1[Status].Value &= ~(1 << bitNumber);
                }
                else if (value == true)
                {
                    _ramBank1[Status].Value |= 1 << bitNumber;
                }
            }
        }

        public bool ReadBit(int address, int bit)
        {
            BitArray b = new BitArray(new int[] { _ramBank0[address].Value });
            return b[bit];
        }

        public void WriteByte(int address, int value)
        {
            if ((GetRp0() == false) && (address <= 128))
            {
                _ramBank0[address].Value = value;
            }
            else if ((GetRp0()) && (address >= 128))
            {
                _ramBank1[address].Value = value;
            }
        }

        public int GetByte(int address)
        {
            int returnValue = 0;
            if ((GetRp0() == false) && (address <= 128))
            {
                returnValue = _ramBank0[Status].Value;
            }
            else if ((GetRp0() == true) && (address >= 128))
            {
                returnValue = _ramBank0[Status].Value;
            }

            return returnValue;
        }

        public void IncRuntime(bool twoCycle)
        {
            if (twoCycle)
            {
                //_programCounter += 2;
            }
            else if (twoCycle == false)
            {
                //_programCounter++;
            }
        }

        public void WriteResult(int dbit, int fbit, int result)
        {
            if (dbit == 0)
            {
                Wreg = result;
            }
            else if (dbit != 0)
            {
                WriteByte(fbit, result);
            }
        }

        public void ZFlag(int result)
        {
            if (result == 0)
            {
                SetZFlag(true);
            }
            else
            {
                SetZFlag(false);
            }
        }

        public void DcFlag(int _k, int wreg)
        {
            var tempDC1 = _k & 0b_0000_1111;
            var tempDC2 = wreg & 0b_0000_1111;
            var tempDC3 = tempDC1 + tempDC2;
            
            if ((tempDC3 & 0b_0001_0000) == 16)
            {
                SetDCFlag(true);
            }
            else
            {
                SetDCFlag(false);
            }
        }

        public void CFlag(int result)
        {
            if (result > 255)
            {
                SetCFlag(true);
            }
            else
            {
                SetCFlag(false);
            }
        }
        
        public uint RotateLeft(uint value, int count)
        {
            return (value << count) | (value >> (32 - count));
        }

        public uint RotateRight(uint value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
        
    }
}