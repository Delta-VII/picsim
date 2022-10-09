using System.Collections;
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
        
        public int Quarzcycle1
        {
            get => _quarzcycle1;
            set => _quarzcycle1 = value;
        }

        public int Cyclecount1
        {
            get => _cyclecount1;
            set => _cyclecount1 = value;
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
        private int[] _eeprom = new int[64];
        private int _quarzcycle;
        private int _quarzcycle1;
        private bool _watchdogtrigger;
        private int _watchdogtimer;
        private int _laufzeit;
        private int _laufzeit1;
        private int _quarz = 1;
        private int _cyclecount = 500;
        private int _cyclecount1 = 500;

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

        public void WriteByte(int address, int value)
        {
            if (GetRp0() == false)
            {
                
                switch (address)
                {
                    case 0x00:
                        _ramBank0[_ramBank0[Fsr].Value].Value = value;
                        break;
                    default:
                        _ramBank0[address].Value = value;
                        break;
                }

                switch (address)
                {
                    case 0x03:
                        _ramBank1[0x03].Value = _ramBank0[0x03].Value;
                        break;
                    case 0x02:
                        _ramBank1[0x02].Value = _ramBank0[0x02].Value;
                        break;
                    case 0x0A:
                        _ramBank1[0x0A].Value = _ramBank0[0x0A].Value;
                        break;
                    case 0x0B:
                        _ramBank1[0x0B].Value = _ramBank0[0x0B].Value;
                        break;
                    default:
                        break;
                }
            }
                else if (GetRp0())
            {
                if (address == 0x00)
                {
                    _ramBank1[_ramBank0[Fsr].Value].Value = value;
                }
                else
                {
                    _ramBank1[address].Value = value;
                }
                
                switch (address)
                {
                    case 0x03:
                        _ramBank0[0x03].Value = _ramBank1[0x03].Value;
                        break;
                    case 0x02:
                        _ramBank0[0x02].Value = _ramBank1[0x02].Value;
                        break;
                    case 0x0A:
                        _ramBank0[0x0A].Value = _ramBank1[0x0A].Value;
                        break;
                    case 0x0B:
                        _ramBank0[0x0B].Value = _ramBank1[0x0B].Value;
                        break;
                    default:
                        break;
                        
                }
                
            }
        }

        public int GetByte(int address)
        {
            int returnValue = 0;
            if (GetRp0() == false)
            {
                returnValue = _ramBank0[address].Value;
                
                if (address == 0x00)
                {
                    returnValue = _ramBank0[_ramBank0[Fsr].Value].Value;
                }
                else
                {
                    returnValue = _ramBank0[address].Value;
                }
            }
            else if (GetRp0())
            {
                if (address == 0x00)
                {
                    returnValue = _ramBank1[_ramBank0[Fsr].Value].Value;
                }
                else
                {
                    returnValue = _ramBank1[address].Value;
                }
            }
            return returnValue;
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
        
        public void watchdogcycle(){
        //qInfo()<<"watchdogtimer"<<watchdogtimer<<"\n";
       // qInfo()<<"quarzcycle2"<<quarzcycle2<<"\n";
        _quarzcycle++;
        _laufzeit = (4/_quarz) * _quarzcycle;
        _laufzeit1 = (4/_quarz) * _quarzcycle1;
            if (_watchdogtrigger == true)
            {
                _quarzcycle1++;               //TMR0 On Clockcycles
            if ((_ramBank1[1].Value & 0b_0000_1000) == 8) {         //Prescaler to Watchdog

                int prescaletmp = _ramBank1[1].Value & 0b_0000_0111;
                int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp)));
                if (_cyclecount1 == 500)
                {
                    _cyclecount1 = prescale;
                }//um cyclecount zu initalisieren, cyclecoount wird nie wieder den wert 500 erreichen

                if (_laufzeit1 > 18000)
                {
                    _quarzcycle1 = 0;
                    _cyclecount1--;
                }

                if (_cyclecount1 == 0)
                {
                    _cyclecount1 = prescale;
                    if((RamBank0[3].Value & 0b_0000_1000)==8){
                        Reset();
                    }
                    else
                    {
                        RamBank0[3].Value |= 0b_0000_1000;
                    }
                                                                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
                }
                
            }
            if ((_ramBank1[1].Value & 0b_0000_1000) == 0) {                     //Kein Prescaler gesetzt
                   if (_laufzeit1>18000)
                   {
                       _quarzcycle1 = 0;
                       if ((RamBank0[3].Value & 0b_0000_1000) == 8)
                       {
                           Reset();
                       }
                       else
                       {
                           RamBank0[3].Value |= 0b_0000_1000;
                       }
                   }                          //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
            }
            
        }
    }

        public void Timercycle()
        {                             //Timerfunktion für das zählen der Clockcycles
        _quarzcycle++;
        if (_watchdogtrigger == true)
        {
            _quarzcycle1++;
        };
        if ((_ramBank1[1].Value & 0b_0010_0000) == 0) {               //TMR0 On Clockcycles
            if ((_ramBank1[1].Value & 0b_0000_1000) == 0) {         //Prescaler to TMR0

                var prescaletmp = _ramBank1[1].Value & 0b00000111;
                var prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp+1)));
                if (_cyclecount == 500)
                {
                    _cyclecount = prescale;
                }//um cyclecount zu initalisieren, cyclecoount wird nie wieder den wert 500 erreichen
                _cyclecount--;
                if (_cyclecount == 0)
                {
                    _cyclecount = prescale;
                    _ramBank0[1].Value++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
                }
                if (_ramBank0[1].Value > 255) {
                    _ramBank0[1].Value &= 0b_1111_1111; // TMR0 wird auf Überlauf kontrolliert
                    _ramBank0[0x0B].Value |= 0b_0000_0100;
                    checkinterrupt();
                }

            }
            if ((_ramBank1[1].Value & 0b_0000_1000) == 8)
            {         //Kein Prescaler gesetzt
                _ramBank0[1].Value++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
            }
                if (_ramBank0[1].Value > 255) {
                    _ramBank0[1].Value &= 0b_1111_1111; // TMR0 wird auf Überlauf kontrolliert
                    _ramBank0[0x0B].Value |= 0b_0000_0100;
                    checkinterrupt();
                }

        }
    }

        public void TimersetIO() {                                           //Timerfunktion für das zählen der IO Flanken
        if ((_ramBank1[1].Value & 0b_0010_0000) == 32)
        {                  //Transition on RA4/T0CKI pin
            var T0CSstate = (_ramBank1[1].Value & 0b_0001_0000) > 4;
            if ((_ramBank1[1].Value & 0b_0000_1000) == 0)
            {              //Prescaler to TMR0
                var Ra4state = (_ramBank0[5].Value & 0b_0001_0000) > 4;   //if RA4 =0 : RA4state = 0 / if RA4 = 1 : Ra4State = 1
                if (Ra4state && T0CSstate == false)
                {
                    _cyclecount++;
                }
                if (Ra4state == false && T0CSstate) {
                    _cyclecount++;
                }
                int prescaletmp = _ramBank1[1].Value & 0b_0000_0111;
                int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp+1)));
                if (_cyclecount >= prescale)
                {
                    _cyclecount = 0;
                    _ramBank0[1].Value++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
                }
                if (_ramBank0[1].Value > 255)
                {
                    _ramBank0[1].Value &= 0b_1111_1111;// TMR0 wird auf Überlauf kontrolliert
                    _ramBank0[0x0B].Value |= 0b_0000_0100;
                }
            }
            if ((_ramBank1[1].Value & 0b_0000_1000) == 8)
            {              //Kein Prescaler
                var Ra4state = (_ramBank0[5].Value & 0b_0001_0000) > 4;   //if RA4 =0 : RA4state = 0/ if RA4 = 1 : Ra4State = 1
                if (Ra4state && T0CSstate == false)
                {
                    _ramBank0[1].Value++;
                }
                if (Ra4state == false && T0CSstate)
                {
                    _ramBank0[1].Value++;
                }
                if (_ramBank0[1].Value > 255)
                {
                    _ramBank0[1].Value &= 0b_1111_1111;       // TMR0 wird auf Überlauf kontrolliert unf fängt wieder bei 0 an
                    _ramBank0[0x0B].Value |= 0b_0000_0100;   //Timerinterrupt flag be Überlauf setzen
                    checkinterrupt();
                }

            }
        }
    }

        public void checkinterrupt() {
        
        if ((_ramBank0[0x0B].Value & 0b_1000_0000) == 128)
        {
            if (((_ramBank0[0x0B].Value & 0b_0010_0000) == 32) && ((_ramBank0[0x0B].Value & 0b_0000_0100) == 4))
            {
                tmr0interrupt();
            }
        }
        if((_ramBank0[0x0B].Value&0b_1000_0000)==128)
        {
            if (((_ramBank0[0x0B].Value & 0b_0001_0000) == 16) && ((_ramBank0[0x0B].Value & 0b_0000_0010) == 2))
            {
                if ((((_ramBank0[0x0B].Value & 0b_0000_0001) == 0) && ((_ramBank0[0x0B].Value & 0b_0100_0000) == 64)) ||
                    (((_ramBank0[0x0B].Value & 0b_0000_0001) == 1) && ((_ramBank0[0x0B].Value & 0b_0100_0000) == 0)))
                {
                    tmr0interrupt();
                }
                 }
        }
        if((_ramBank0[0x0B].Value&0b_1000_0000) == 128)
        {
            if (((_ramBank0[0x0B].Value & 0b_0000_1000) == 8) && ((_ramBank0[0x0B].Value & 0b_0000_0001) == 1))
            {
                tmr0interrupt();
            }
        }
    }

        private void tmr0interrupt() { 
            _ramBank0[0x0B].Value &= 0b01111111;
            push(_programCounter);
            _programCounter = 4;
            _programCounter--;
        }

        public bool Watchdogtrigger
        {
            get => _watchdogtrigger;
            set => _watchdogtrigger = value;
        }

        public void Reset(){
            for (int i = 0; i < _ramBank0.Count; i++)
            {
                _ramBank0[i].Value = 0;
            }
            
            for (int i = 0; i < _ramBank1.Count; i++)
            {
                _ramBank1[i].Value = 0;
            }
            
            _ramBank0[3].Value =24;
            _ramBank1[3].Value = 24;
            _wreg=0;
            _programCounter = 0;
            _cyclecount = 500;
            _cyclecount1 = 500;
            _quarzcycle = 0;
            _quarzcycle1=0;
            StackDGV.Clear();
            _stack.Clear();
        }
        
    }
}