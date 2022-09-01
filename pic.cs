using System;
using System.Collections;

namespace Try
{

	public partial class Pic
	{
		
		//pic components
		public byte[] Ram = new byte[256];
        private ushort[] _programMemory = new ushort[1024];
		private byte _wreg;
		public int ProgrammLaufzeit;
		private int _programCounter;
		private ushort _instructionRegister;
		public Stack Stack = new Stack();
		public int Stackpointer;
		private byte[] _eeprom = new byte[64];
		private int _cycle;
		private int _cycle2;
		private int _quarz;
		private int _quarz2;
		private int _laufzeit;
		private int _laufzeit2;
		private byte _latcha;
		private byte _latchb;
		private double _osc;
		private int _triggerWatchdog;

		public event EventHandler<UpdateRegistersEventArgs> EvtUpdateRegisters;

		public void SetProgramMemory(string[] s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				ushort temp = ushort.Parse(s[i], System.Globalization.NumberStyles.HexNumber);
				_programMemory[i] = temp;
			}
		}
		
		public void Reset(){
			for(int i = 0; i<Ram.Length;i++){
				WriteRam(i,0);
			}
			WriteRam(0x3,24);
			WriteRam(0x83,24);
			_wreg=0;
			_programCounter = 0;
			_cycle = 500;
			_cycle2 = 500;
			_quarz = 0;
			_quarz2=0;
			Stackpointer= 0;
		}
		
		void PORTAchange(int valuePORTA){
      for(int i = 0; i<8;i++){
          int k = Convert.ToInt32(Math.Pow(2,i));
          if(((GetRam(Trisa) & k) == 0) & ((valuePORTA & k)== 0) ){
              WriteRam(Porta,Convert.ToByte(GetRam(Porta) & ((~k) & 0b_1111_1111)));
          }
          if(((GetRam(Trisa)&k)==0) & ((valuePORTA&k)==k) ){
              WriteRam(Porta,Convert.ToByte(GetRam(Porta) | k));
          }
          if(((GetRam(Trisa) & k) == k) & ((valuePORTA & k) == 0)){
              _latcha = Convert.ToByte(_latcha & ((~k) & 0b_1111_1111));
          }
          if(((GetRam(Trisa) & k) == k) & ((valuePORTA & k) == k)){
              _latcha = Convert.ToByte(_latcha | k);
          } 
      } 
		}

    void PORTBchange(int valuePORTB){
	    if (((((valuePORTB & 1) == 0)) && ((GetRam(Portb) & 1) == 1)) ||
	        ((((valuePORTB & 1) == 1)) && ((GetRam(Portb) & 1) == 0)))
	    {
		    Interrupt();
	    }
        for(int i = 0; i<8;i++){
          int k = Convert.ToInt32(Math.Pow(2,i));
          if(((GetRam(Trisb) & k) == 0) & ((valuePORTB & k) == 0)){
              WriteRam(Portb, Convert.ToByte(GetRam(Portb) & ((~k) & 0b_1111_1111)));
          }
          if(((GetRam(Trisb) & k) == 0) & ((valuePORTB & k) == k)){
              WriteRam(Portb, Convert.ToByte((GetRam(Portb) | k)));
          }
          if(((GetRam(Trisb)&k)==k)   & ((valuePORTB&k)==0) ){
              _latchb = Convert.ToByte(_latchb & ((~k) & 0b_1111_1111));
          }
          if(((GetRam(Trisb)&k)==k)   & ((valuePORTB&k)==k) ){
              _latchb = Convert.ToByte(_latchb | k);
          }
          
        }
    }

    void TRISAchange(){
        for(int i = 0; i<8;i++){
            int k = Convert.ToInt32(Math.Pow(2,i));
            if(((GetRam(Trisa) & k) == k)){

            }
            if(((GetRam(Trisa)&k)==0) && ((_latcha&k) ==0)){
                WriteRam(Porta, Convert.ToByte(GetRam(Porta) & ((~k) & 0b_1111_1111)));

            }
            if(((GetRam(Trisa)&k)==0) && ((_latcha&k) ==k)){
                 WriteRam(Porta, Convert.ToByte(GetRam(Porta) | k));
                 _latcha = Convert.ToByte(_latcha & ((~k)&0b11111111));
            }
        }
    }

    void TRISBchange(){
        for(int i = 0; i<8;i++){
            int k = Convert.ToInt32(Math.Pow(2,i));
            if (((GetRam(Trisb) & k) == k))
            {
	            
            }
            if(((GetRam(Trisb) & k)== 0) && ((_latchb&k) == 0)){
                WriteRam(Portb, Convert.ToByte(GetRam(Portb) & ((~k) & 0b_1111_1111)));

            }
            if(((GetRam(Trisb)&k)==0) && ((_latchb&k) ==k)){
                 WriteRam(Portb,Convert.ToByte(GetRam(Portb) | k));
                 _latchb = Convert.ToByte(_latchb & ((~k) & 0b_1111_1111));
            }
        }
    }
		
		private void Watchdogcycle(){
			
			_quarz++;
			_laufzeit = Convert.ToInt32((4/_osc)*_quarz);
			_laufzeit2 = Convert.ToInt32((4/_osc)*_quarz2);
			if (_triggerWatchdog == 1)
			{
				_quarz2++;
				if ((GetRam(Option) & 0b_0000_1000) == 8) {
					int prescaletmp = GetRam(Option) & 0b00000111;
					int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp)));
					if (_cycle2 == 500)
					{
						_cycle2 = prescale;
					}
					if(_laufzeit2>18000){_quarz2=0;_cycle2--;}

					if (_cycle2 == 0) {
						_cycle2 = prescale;
						if((GetRam(Status)&0b_0000_1000)==8){
							Reset();
						}
						else
						{
							WriteRam(Status,(Convert.ToByte(GetRam(Status)|0b_0000_1000)));
						}
						
					}


				}
				if ((GetRam(Option) & 0b_0000_1000) == 0) {
					if(_laufzeit2>18000)
					{
						_quarz2 =0;
						if ((GetRam(Status) & 0b00001000) == 8)
						{
							Reset();
						}
						else
						{
							WriteRam(Status,(Convert.ToByte(GetRam(Status) | 0b_0000_1000)));
						}
						
					}
				}
				
			}
		}

		private void SetTimerCycle() {
			_quarz++;
			if (_triggerWatchdog == 1){_quarz2++;};
			if ((GetRam(Option) & 0b_0010_0000) == 0)
			{
				if ((GetRam(Option) & 0b_0000_1000) == 0)
				{        
					int prescaletmp = GetRam(Option) & 0b_0000_0111;
					int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp+1)));
					if (_cycle == 500)
					{
						_cycle = prescale;
					}
					_cycle--;
					if (_cycle == 0) {
						_cycle = prescale;
						WriteRam(Tmr0,Convert.ToByte(GetRam(Tmr0) + 1));
					}
					if (GetRam(Tmr0) > 255) {
						WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) & 0b_1111_1111));
                        WriteRam(Intcon, Convert.ToByte(GetRam(Intcon) | 0b_0000_0100));
                        Interrupt();
					}

				}
				if ((GetRam(Option) & 0b00001000) == 8) {
					WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) + 1));
				}
                if (GetRam(Tmr0) > 255) {
					WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) & 0b11111111));
                    WriteRam(Intcon, Convert.ToByte(GetRam(Intcon) | 0b00000100));
                    Interrupt();
				}
			}
		}

    private void CountTimerIo() {
	    
        if ((GetRam(Option) & 0b_0010_0000) == 32) {
            bool t0CSstate = (GetRam(Option) & 0b_0001_0000) > 4;
            if ((GetRam(Option) & 0b_0000_1000) == 0) {
                bool ra4State = (GetRam(Porta) & 0b_0001_0000) > 4;
                if (ra4State && t0CSstate == false) {
                    _cycle++;
                }
                if (!ra4State && t0CSstate) {
                    _cycle++;
                }
                int prescaletmp = GetRam(Option) & 0b_0000_0111;
                int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp+1)));
                if (_cycle >= prescale) {
                    _cycle = 0;
                    WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) + 1));
                }
                if (GetRam(Tmr0) > 255) {
	                WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) & 0b_1111_1111));
	                WriteRam(Intcon, Convert.ToByte(GetRam(Intcon) | 0b_0000_0100));
                }
            }
            if ((GetRam(Option) & 0b_0000_1000) == 8) {
                bool ra4State = (GetRam(Porta) & 0b_0001_0000) > 4;
                if (!ra4State && !t0CSstate) {
	                WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) + 1));
                }
                if (!ra4State && t0CSstate) {
	                WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) + 1));
                }
                if (GetRam(Tmr0) > 255) {
	                WriteRam(Tmr0, Convert.ToByte(GetRam(Tmr0) & 0b_1111_1111));
	                WriteRam(Intcon, Convert.ToByte(GetRam(Intcon) | 0b_0000_0100));
                    Interrupt();
                }

            }
        }

    }
		private void WriteRam(int address, byte value)
		{
			if (address == Status)
			{
				Ram[address] = value;
				Ram[address + 128] = value;
			}
			else if (address == Intcon)
			{
				Ram[address] = value;
				Ram[address + 128] = value;
			}
			else if (address == Pcl)
			{
				Ram[address] = value;
				Ram[address + 128] = value;
			}
            
			else
			{
				if ((GetRam(Status) | Rp0) == 0)
				{
					if (address < 255)
					{
						Ram[address] = value;
					}
					else if (address > 255)
					{
						Ram[address - 128] = value;
					}

				}
				else if ((GetRam(Status) | Rp0) == 32)
				{
					if (address < 255)
					{
						Ram[address + 128] = value;
					}
					else if (address > 255)
					{
						Ram[address] = value;
					}
				}

			}if((address == 0x88) && ((GetRam(Status) & 0b_0010_0000)== 32)&&((GetRam(Eecon1) | 0b_0001_0000) == 2)){
				if ((GetRam(Eecon1) & 0b00000110) == 6)
				{
					int eeaddr = GetRam(Eeadr);
					int eedat = GetRam(Eedata);
					_eeprom[eeaddr]= Convert.ToByte(eedat);
					WriteRam(Eecon1, GetRam(Eecon1 | 0b_0001_0000));
				}
			}
			
		}

		private byte GetRam(ushort address)
		{
			return Ram[address];
		}

		private void CheckZ(byte value)
		
		{
			if (value == 0)
			{
				SetZ(true);
			}
			else {
				SetZ(false);
			}
		}

		private void CheckC(byte value)
		{
			if (value < 255)
			{
				SetC(false);
			}
			else if (value > 255)
			{
				SetC(true);
			}
		}

		private void CheckDc(byte value)
		{
			if (value > 15)
			{
				SetDc(true);
			}
			else if (value < 0)

			{
				SetDc(false);
			}
		}

		private void SetDc(bool value)
		{
			if (value == true)
			{
				WriteRam(Status, GetRam(Status | Dc));
			}
			else if (value == false)
			{
				WriteRam(Status, GetRam(Status & Dc));
			}
		}

		private void SetC(bool value)
		{
			if (value == true)
			{
				WriteRam(Status, GetRam(Status | C));
			}
			else if (value == false)
			{
				WriteRam(Status, GetRam(Status & C));
			}
		}

		private void SetZ(bool value)
		{
			if (value == true)
			{
				WriteRam(Status, GetRam(Status | Z));
			}
			else if (value == false)
			{
				WriteRam(Status, GetRam(Status & Z));
			}
		}

		private void SaveResult(bool dbit, byte result, ushort address)
		{
			if (dbit)
			{
				WriteRam(address, result);
			}
			else if (!dbit)

			{
				_wreg = result;
			}
		}

		private string DecodeOperation()
		{
			string[] byteWiseOpcodes = Opcodes.GetOpcodesByteOriented();
			string[] bitWiseOpcodes = Opcodes.GetOpcodesBitOriented();
			string[] opcodesLiteral = Opcodes.GetOpcodesLiteral();
			string[] opcodesControl = Opcodes.GetOpCodesControl();

			int prog = _instructionRegister;
			string temp = Convert.ToString(prog,2);

			foreach (string s in byteWiseOpcodes)
			{
				if (s == temp.Substring(0, 7))
				{
					return s;
				}
			}

			foreach (string s in bitWiseOpcodes)
			{
				if (s == temp.Substring(0, 4))
				{
					return s;
				}
			}

			foreach (string s in opcodesLiteral)
			{
				if (s == temp.Substring(0, 6))
				{
					return s;
				}
			}

			foreach (string s in opcodesControl)
			{
				if (s == temp)
				{
					return s;
				}
			}

			return "";
		}

		private void DecodeInstruction(string inst)
		{
			switch (inst)
			{
				//ADDWF
				case "000111":
					Addwf();
					break;
				//ANDWF
				case "000101":
					Andwf();
					break;
				//CLRF
				case "0000011":
					Clrf();
					break;
				//CLRW
				case "0000010":
					Clrw();
					break;
				//COMF
				case "0010010":
					Comf();
					break;
				//DECF
				case "0000110":
					Decf();
					break;
				//DECFSZ
				case "0010110":
					Decfsz();
					break;
				//INCF
				case "0010100":
					Incf();
					break;
				//INCFSZ
				case "0011110":
					Incfsz();
					break;
				//IORWF
				case "0001000":
					Iorwf();
					break;
				//MOVF
				case "0010000":
					Movf();
					break;
				//MOVWF
				case "0000001":
					Movwf();
					break;
				//NOP
				case "0000000":
					Nop();
					break;
				//RLF
				case "0011010":
					Rlf();
					break;
				//RRF
				case "0011000":
					Rrf();
					break;
				//SUBWF
				case "0000100":
					Subwf();
					break;
				//SWAPF
				case "0011100":
					Swapf();
					break;
				//XORWF
				case "0001100":
					Xorwf();
					break;
				//BCF
				case "0100":
					Bcf();
					break;
				//BSF
				case "0101":
					Bsf();
					break;
				//BTFSC
				case "0110":
					Btfsc();
					break;
				//BTFSS
				case "0111":
					Btfss();
					break;
				//ADDLW
				case "111110":
					Andlw();
					break;
				//ANDLW
				case "111001":
					Addlw();
					break;
				//CALL
				case "100000":
					Call();
					break;
				//CLRWDT
				case "00000001100100":
					Clrwdt();
					break;
				//GOTO
				case "101000":
					GOto();
					break;
				//IORLW
				case "111000":
					Iorwl();
					break;
				//MOVLW
				case "110000":
					Movlw();
					break;
				//RETFIE
				case "00000000001001":
					Retfie();
					break;
				//RETLW
				case "110100":
					Retlw();
					break;
				//RETURN
				case "00000000001000":
					REturn();
					break;
				//SLEEP
				case "00000001000011":
					Sleep();
					break;
				//SUBLW
				case "111100":
					Sublw();
					break;
				//XORLW
				case "111010":
					Xorlw();
					break;
			}
		}

		public async Task<int> Step()
		{
			Execute();
			_programCounter += 1;
			
            return 1;
		}

		private bool DecodeDBit()
		{
			ushort inst = _instructionRegister;
			if ((inst & 0b_00_0000_1000_0000) == 128)
			{
				return true;
			}
			else if ((inst & 0b_00_0000_1000_0000) == 0)
			{
				return false;
			}

			return false;
		}

		private byte DecodeFBits()
		{
			ushort inst = _instructionRegister;
			int result = inst & 0b_00_0000_0111_1111;
			return Convert.ToByte(result);
		}

		private byte DecodeBBits()
		{
			ushort inst = _instructionRegister;
			byte result = Convert.ToByte(inst & 0b_00_0001_1100_0000);
			return result;
		}

		private byte DecodeLiteralGeneral()
		{
			ushort inst = _instructionRegister;
			byte result = Convert.ToByte(inst & 0b_00_0000_1111_1111);
			return result;
		}

		private int DecodeLiteralJump()
		{
			ushort inst = _instructionRegister;
			int result = inst & 0b_00_0111_1111_1111;
			return result;
		}

		private void RefreshRegisters()
		{
			RefreshProgram();
			RefreshIndirectAddress();
			EvtUpdateRegisters?.Invoke(this, new UpdateRegistersEventArgs(14151801));
		}

		private void CheckInterrupts()
		{
			if ((GetRam(Intcon) & 0b_1000_0000) == 128)
			{
				if ((GetRam(Intcon & 0b_0010_0000) == 32) && (GetRam(Intcon) & 0b_0000_0100) == 4)
				{
					Interrupt();
				}
			}

			if ((GetRam(Intcon) & 0b_1000_0000) == 128)
			{
				if (((GetRam(Intcon) & 0b_0001_0000) == 16) & ((GetRam(Intcon) & 0b_0000_0010) == 2))
				{
					if ((((GetRam(Intcon) & 0b_0000_0001) == 0) & ((GetRam(Intcon) & 0b_0100_0000) == 64)) ||
					    (((GetRam(Intcon) & 0b_0000_0001) == 1) && ((GetRam(Intcon) & 0b_0100_0000) == 0)))
					{
						Interrupt();
					}
				}
			}

			if ((GetRam(Intcon) & 0b_1000_0000) == 128)
			{
				if (((GetRam(Intcon) & 0b_0000_1000) == 8) & ((GetRam(Intcon) & 0b_0000_0001) == 1))
				{
					Interrupt();
				}
			}
		}

		void Interrupt()
		{
			WriteRam(Intcon, Intcon & 0b_0111_1111);
			Stack.Push(_programCounter);
			Stackpointer++;

		}

		private void RefreshProgram()
		{/*
			byte pcl = Convert.ToByte(_programCounter | 0b_0000_0000_1111_1111);
			byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);

			WriteRam(Pcl, pcl);
			WriteRam(Pclath, pclath);

			_programCounter = pclath << 8 | pcl;
			*/
			
		}

		private void RefreshIndirectAddress()
		{
			byte pointer = GetRam(Fsr);
			WriteRam(Indaddr, GetRam(pointer));
		}

		private void Execute()
		{
			_instructionRegister = _programMemory[_programCounter];
			DecodeInstruction(DecodeOperation());
			_programCounter += 1;
			Console.WriteLine("W-Register: {0} | DC: {1} | C: {2} | Z: {3}", _wreg, Dc, C, Z);
		}
    }
} 
        
