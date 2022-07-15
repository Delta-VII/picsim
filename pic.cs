using System;
using System.Collections;

namespace Try
{

	public partial class Pic
	{
		
		//pic components
		public byte[] _ram = new byte[256];
        private ushort[] _programMemory = new ushort[1024];
		private byte _wreg = 0;
		public int _laufzeit = 0;
		private int _programCounter = 0;
		private ushort _instructionRegister = 0;
		public Stack _stack = new Stack();
		public int _stackpointer;
		private byte[] _eeprom = new byte[64];

		public event EventHandler<UpdateRegistersEventArgs> EvtUpdateRegisters;

		public int GetProgramCounter()
		{
			return _programCounter;
		}
		
		public void SetProgramMemory(string[] s)
		{
			for (int i = 0; i < s.Length; i++)
			{
				ushort temp = ushort.Parse(s[i]);
				_programMemory[i] = temp;
			}
		}

		private void WriteRAM(ushort address, byte value)
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
			string temp = prog.ToString();

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

		public async Task<int> Step(int mode)
		{/*
			switch (mode)
			{
				//Run Mode
				case 0:
					for (int i = 0; i < 1024; i++)
					{
						_programCounter = +1;
					}

					break;
				//Step Mode
				case 1:
					_programCounter = +1;
					break;
			}
			*/
			WriteRAM(0x30, Convert.ToByte(GetRAM(0xB0) + 1));
			RefreshRegisters();

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
    }
} 
        
