﻿using System;
using System.Collections;

namespace Try
{

	public partial class Pic
	{
		
		//pic components
		public byte[] _ram = new byte[256];
        private ushort[] _programMemory = new ushort[1024];
		private byte _wreg = 0;
		public int _ProgrammLaufzeit = 0;
		private int _programCounter = 0;
		private ushort _instructionRegister = 0;
		public Stack _stack = new Stack();
		public int _stackpointer;
		private byte[] _eeprom = new byte[64];
		private int Cycle;
		private int Cycle2;
		private int Quarz;
		private int Quarz2;
		private int laufzeit;
		private int laufzeit2;
		private double osc;
		private int TriggerWatchdog;

		public event EventHandler<UpdateRegistersEventArgs> EvtUpdateRegisters;

		public int GetProgramCounter()
		{
			return _programCounter;
		}
		
		public void SetProgramMemory(string[] s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				ushort temp = ushort.Parse(s[i], System.Globalization.NumberStyles.HexNumber);
				_programMemory[i] = temp;
			}
		}
		
		public void Reset(){
			for(int i = 0; i<_ram.Length;i++){
				WriteRAM(i,0);
			}
			WriteRAM(0x3,24);
			WriteRAM(0x83,24);
			_wreg=0;
			_programCounter = 0;
			Cycle = 500;
			Cycle2 = 500;
			Quarz = 0;
			Quarz2=0;
			_stackpointer= 0;
		}
		
		private void Watchdogcycle(){
			
			Quarz++;
			laufzeit = Convert.ToInt32((4/osc)*Quarz);
			laufzeit2 = Convert.ToInt32((4/osc)*Quarz2);
			if (TriggerWatchdog == 1)
			{
				Quarz2++;
				if ((GetRAM(Option) & 0b_0000_1000) == 8) {
					int prescaletmp = GetRAM(Option) & 0b00000111;
					int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp)));
					if (Cycle2 == 500)
					{
						Cycle2 = prescale;
					}
					if(laufzeit2>18000){Quarz2=0;Cycle2--;}

					if (Cycle2 == 0) {
						Cycle2 = prescale;
						if((GetRAM(Status)&0b_0000_1000)==8){
							Reset();
						}
						else
						{
							WriteRAM(Status,(Convert.ToByte(GetRAM(Status)|0b_0000_1000)));
						}
						
					}


				}
				if ((GetRAM(Option) & 0b_0000_1000) == 0) {
					if(laufzeit2>18000)
					{
						Quarz2 =0;
						if ((GetRAM(Status) & 0b00001000) == 8)
						{
							Reset();
						}
						else
						{
							WriteRAM(Status,(Convert.ToByte(GetRAM(Status) | 0b_0000_1000)));
						}
						
					}
				}
				
			}
		}

		void timersetcycle() {
        Quarz++;
        if (TriggerWatchdog == 1){Quarz2++;};
        if ((GetRAM(Option) & 0b00100000) == 0)
        {
            if ((GetRAM(Option) & 0b00001000) == 0)
            {        
                int prescaletmp = GetRAM(Option) & 0b00000111;
                int prescale = Convert.ToInt32(Math.Pow(2, (prescaletmp+1)));
                if (Cycle == 500)
                {
	                Cycle = prescale;
                }
                Cycle--;
                if (Cycle == 0) {
	                Cycle = prescale;
                    SRAM[TMR0]++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
                }
                if (SRAM[TMR0] > 255) {
                    SRAM[TMR0] = SRAM[TMR0] & 0b11111111; // TMR0 wird auf Überlauf kontrolliert
                    SRAM[INTCON] = SRAM[INTCON] | 0b00000100;
                    checkinterrupt();
                }

            }
            if ((SRAM[OPTION] & 0b00001000) == 8) {         //Kein Prescaler gesetzt
                    SRAM[TMR0]++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
            }
                if (SRAM[TMR0] > 255) {
                    SRAM[TMR0] = SRAM[TMR0] & 0b11111111; // TMR0 wird auf Überlauf kontrolliert
                    SRAM[INTCON] = SRAM[INTCON] | 0b00000100;
                    Interrupt();
                }

        }
    }


    void timersetIO() {                                           //Timerfunktion für das zählen der IO Flanken
        if ((SRAM[OPTION] & 0b00100000) == 32) {                  //Transition on RA4/T0CKI pin
            int T0CSstate = (SRAM[OPTION] & 0b00010000) > 4;
            if ((SRAM[OPTION] & 0b00001000) == 0) {              //Prescaler to TMR0
                int Ra4state = (SRAM[PORTA] & 0b00010000) > 4;   //if RA4 =0 : RA4state = 0 / if RA4 = 1 : Ra4State = 1
                if (Ra4state == 1 && T0CSstate == 0) {
                    cyclecount++;
                }
                if (Ra4state == 0 && T0CSstate == 1) {
                    cyclecount++;
                }
                int prescaletmp = SRAM[OPTION] & 0b00000111;
                int prescale = pow(2, (prescaletmp+1));
                if (cyclecount >= prescale) {
                    cyclecount = 0;
                    SRAM[TMR0]++;                           //SRAM wird erhöt wenn cyclecounts entsprechend dem Prescaler durchgelaufen sind
                }
                if (SRAM[TMR0] > 255) {
                    SRAM[TMR0] = SRAM[TMR0] & 0b11111111;// TMR0 wird auf Überlauf kontrolliert
                    SRAM[INTCON] = SRAM[INTCON] | 0b00000100;
                }
            }
            if ((SRAM[OPTION] & 0b00001000) == 8) {              //Kein Prescaler
                int Ra4state = (SRAM[PORTA] & 0b00010000) > 4;   //if RA4 =0 : RA4state = 0/ if RA4 = 1 : Ra4State = 1
                if (Ra4state == 1 && T0CSstate == 0) {
                    SRAM[TMR0]++;
                }
                if (Ra4state == 0 && T0CSstate == 1) {
                    SRAM[TMR0]++;
                }
                if (SRAM[TMR0] > 255) {
                    SRAM[TMR0] = SRAM[TMR0] & 0b11111111;       // TMR0 wird auf Überlauf kontrolliert unf fängt wieder bei 0 an
                    SRAM[INTCON] = SRAM[INTCON] | 0b00000100;   //Timerinterrupt flag be Überlauf setzen
                    checkinterrupt();
                }

            }
        }
    }
		
		private void WriteRAM(int address, byte value)
		{
			if (address == Status)
			{
				_ram[address] = value;
				_ram[address + 128] = value;
			}
			else if (address == Intcon)
			{
				_ram[address] = value;
				_ram[address + 128] = value;
			}
			else if (address == Pcl)
			{
				_ram[address] = value;
				_ram[address + 128] = value;
			}
            
			else
			{
				if ((GetRAM(Status) | Rp0) == 0)
				{
					if (address < 255)
					{
						_ram[address] = value;
					}
					else if (address > 255)
					{
						_ram[address - 128] = value;
					}

				}
				else if ((GetRAM(Status) | Rp0) == 32)
				{
					if (address < 255)
					{
						_ram[address + 128] = value;
					}
					else if (address > 255)
					{
						_ram[address] = value;
					}
				}

			}if((address == 0x88) && ((GetRAM(Status) & 0b_0010_0000)== 32)&&((GetRAM(Eecon1) | 0b_0001_0000) == 2)){
				if ((GetRAM(Eecon1) & 0b00000110) == 6)
				{
					int eeaddr = GetRAM(Eeadr);
					int eedat = GetRAM(Eedata);
					_eeprom[eeaddr]= Convert.ToByte(eedat);
					WriteRAM(Eecon1, GetRAM(Eecon1 | 0b_0001_0000));
				}
			}
			
		}

		private byte GetRAM(ushort address)
		{
			return _ram[address];
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

		private void CheckDC(byte value)
		{
			if (value > 15)
			{
				SetDC(true);
			}
			else if (value < 0)

			{
				SetDC(false);
			}
		}

		private void SetDC(bool value)
		{
			if (value == true)
			{
				WriteRAM(Status, GetRAM(Status | Dc));
			}
			else if (value == false)
			{
				WriteRAM(Status, GetRAM(Status & Dc));
			}
		}

		private void SetC(bool value)
		{
			if (value == true)
			{
				WriteRAM(Status, GetRAM(Status | C));
			}
			else if (value == false)
			{
				WriteRAM(Status, GetRAM(Status & C));
			}
		}

		private void SetZ(bool value)
		{
			if (value == true)
			{
				WriteRAM(Status, GetRAM(Status | Z));
			}
			else if (value == false)
			{
				WriteRAM(Status, GetRAM(Status & Z));
			}
		}

		private void SaveResult(bool dbit, byte result, ushort address)
		{
			if (dbit)
			{
				WriteRAM(address, result);
			}
			else if (!dbit)

			{
				_wreg = result;
			}
		}

		private string DecodeOperation()
		{
			string[] ByteWiseOpcodes = Opcodes.GetOpcodesByteOriented();
			string[] BitWiseOpcodes = Opcodes.GetOpcodesBitOriented();
			string[] OpcodesLiteral = Opcodes.GetOpcodesLiteral();
			string[] OpcodesControl = Opcodes.GetOpCodesControl();

			int prog = _instructionRegister;
			string temp = Convert.ToString(prog,2);

			foreach (string s in ByteWiseOpcodes)
			{
				if (s == temp.Substring(0, 7))
				{
					return s;
				}
			}

			foreach (string s in BitWiseOpcodes)
			{
				if (s == temp.Substring(0, 4))
				{
					return s;
				}
			}

			foreach (string s in OpcodesLiteral)
			{
				if (s == temp.Substring(0, 6))
				{
					return s;
				}
			}

			foreach (string s in OpcodesControl)
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
					addwf();
					break;
				//ANDWF
				case "000101":
					andwf();
					break;
				//CLRF
				case "0000011":
					clrf();
					break;
				//CLRW
				case "0000010":
					clrw();
					break;
				//COMF
				case "0010010":
					comf();
					break;
				//DECF
				case "0000110":
					decf();
					break;
				//DECFSZ
				case "0010110":
					decfsz();
					break;
				//INCF
				case "0010100":
					incf();
					break;
				//INCFSZ
				case "0011110":
					incfsz();
					break;
				//IORWF
				case "0001000":
					iorwf();
					break;
				//MOVF
				case "0010000":
					movf();
					break;
				//MOVWF
				case "0000001":
					movwf();
					break;
				//NOP
				case "0000000":
					nop();
					break;
				//RLF
				case "0011010":
					rlf();
					break;
				//RRF
				case "0011000":
					rrf();
					break;
				//SUBWF
				case "0000100":
					subwf();
					break;
				//SWAPF
				case "0011100":
					swapf();
					break;
				//XORWF
				case "0001100":
					xorwf();
					break;
				//BCF
				case "0100":
					bcf();
					break;
				//BSF
				case "0101":
					bsf();
					break;
				//BTFSC
				case "0110":
					btfsc();
					break;
				//BTFSS
				case "0111":
					btfss();
					break;
				//ADDLW
				case "111110":
					andlw();
					break;
				//ANDLW
				case "111001":
					addlw();
					break;
				//CALL
				case "100000":
					call();
					break;
				//CLRWDT
				case "00000001100100":
					clrwdt();
					break;
				//GOTO
				case "101000":
					gOTO();
					break;
				//IORLW
				case "111000":
					iorwl();
					break;
				//MOVLW
				case "110000":
					movlw();
					break;
				//RETFIE
				case "00000000001001":
					retfie();
					break;
				//RETLW
				case "110100":
					retlw();
					break;
				//RETURN
				case "00000000001000":
					rETURN();
					break;
				//SLEEP
				case "00000001000011":
					sleep();
					break;
				//SUBLW
				case "111100":
					sublw();
					break;
				//XORLW
				case "111010":
					xorlw();
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
			if ((GetRAM(Intcon) & 0b_1000_0000) == 128)
			{
				if ((GetRAM(Intcon & 0b_0010_0000) == 32) && (GetRAM(Intcon) & 0b_0000_0100) == 4)
				{
					Interrupt();
				}
			}

			if ((GetRAM(Intcon) & 0b_1000_0000) == 128)
			{
				if (((GetRAM(Intcon) & 0b_0001_0000) == 16) & ((GetRAM(Intcon) & 0b_0000_0010) == 2))
				{
					if ((((GetRAM(Intcon) & 0b_0000_0001) == 0) & ((GetRAM(Intcon) & 0b_0100_0000) == 64)) ||
					    (((GetRAM(Intcon) & 0b_0000_0001) == 1) && ((GetRAM(Intcon) & 0b_0100_0000) == 0)))
					{
						Interrupt();
					}
				}
			}

			if ((GetRAM(Intcon) & 0b_1000_0000) == 128)
			{
				if (((GetRAM(Intcon) & 0b_0000_1000) == 8) & ((GetRAM(Intcon) & 0b_0000_0001) == 1))
				{
					Interrupt();
				}
			}
		}

		void Interrupt()
		{
			WriteRAM(Intcon, Intcon & 0b_0111_1111);
			_stack.Push(_programCounter);
			_stackpointer++;

		}
		//TODO: FIXME
		private void RefreshProgram()
		{/*
			byte pcl = Convert.ToByte(_programCounter | 0b_0000_0000_1111_1111);
			byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);

			WriteRAM(Pcl, pcl);
			WriteRAM(Pclath, pclath);

			_programCounter = pclath | pcl;
			*/
		}

		private void RefreshIndirectAddress()
		{
			byte pointer = GetRAM(Fsr);
			WriteRAM(Indaddr, GetRAM(pointer));
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
        
