﻿using System;
using System.Collections;

namespace picsim
{

    public partial class Pic
    {
        //pic parts
        private byte[] _ram = new byte[256];
        private int _ramPointer = 0;
        private ushort[] _programMemory = new ushort[1024];
        private byte _wreg = 0;
        private int _laufzeit = 0;
        private ushort _programCounter = 0;
        private ushort _instructionRegister = 0;
        private Stack _stack = new Stack();
        private int _stackpointer;

        public int GetProgramCounter()
        {
            return _programCounter;
        }
		/*
        public void SetProgramCounter(int i)
        {
            _programCounter = i;
        }
		*/
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
	        } else if (address == Intcon)
	        {
		        _ram[address] = value;
		        _ram[address + 128] = value;
	        } else if (address == Pcl)
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

		        } else if ((GetRAM(Status) | Rp0) == 1)
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
	        } else if (value != 0)

	        {
		        SetZ(false);
	        }
        }
        
        private void CheckC(byte value)
        {
	        if (value < 255)
	        {
		        SetC(false);
	        } else if (value > 255)

	        {
		        SetC(true);
	        }
        }
        
        private void CheckDC(byte value)
        {
	        if (value > 15)
	        {
		        SetDC(true);
	        } else if (value < 0)

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
	        } else if (!dbit)

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
	            if (s == temp.Substring(0,7))
	            {
		            return s;
	            }
            }
            foreach (string s in BitWiseOpcodes)
            {
	            if (s == temp.Substring(0,4))
	            {
		            return s;
	            }
            }
            foreach (string s in OpcodesLiteral)
            {
	            if (s == temp.Substring(0,6))
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

					break;
				//INCF
				case "0010100":
					incf();
				break;
				//INCFSZ
				case "0011110":

				break;
				//IORWF
				case "0001000":
					iorwf();
				break;
				//MOVF
				case "0010000":

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
			
				break;
				//RRF
				case "0011000":
					
				break;
				//SUBWF
				case "0000100":
				
				break;
				//SWAPF
				case "0011100":
				
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

				break;
				//BTFSS
				case "0111":
				
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

				break;
				//CLRWDT
				case "00000001100100":

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

				break;
				//RETLW
				case "110100":

				break;
				//RETURN
				case "00000000001000":

				break;
				//SLEEP
				case "00000001000011":

				break;
				//SUBLW
				case "111100":

				break;
				//XORLW
				case "111010":
					xorlw();
				break;
            }
            }

        public async Task<int> Step(int mode)
        {
	        switch (mode)
	        {	//Run Mode
		        case 0:
			        for (int i = 0; i < 1024; i++)
			        {
				        _programCounter =+ 1;
			        }
			        break;
		        //Step Mode
		        case 1:
			        _programCounter = +1;
			        break;
	        }
	        return 1;
        }

        private bool DecodeDBit()
        {
	        ushort inst = _instructionRegister;
	        if ((inst & 0b_00_0000_1000_0000) == 128)
	        {
		        return true;
	        } else if ((inst & 0b_00_0000_1000_0000) == 0)
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
        }

        private void CheckInterrupts()
        {
	        if ((GetRAM(Intcon) & 0b_1000_0000) == 128) {
		        if ((GetRAM(Intcon & 0b_0010_0000) == 32) && (GetRAM(Intcon) & 0b_0000_0100) == 4) {
			        Tmr0_int();
		        }
	        }
	        if((GetRAM(Intcon) & 0b_1000_0000)==128){
		        if (((GetRAM(Intcon) & 0b_0001_0000) == 16) & ((GetRAM(Intcon) & 0b_0000_0010) == 2)) {
			        if ((((GetRAM(Intcon) & 0b_0000_0001) == 0) & ((GetRAM(Intcon) & 0b_0100_0000) == 64)) ||
			            (((GetRAM(Intcon) & 0b_0000_0001) == 1) && ((GetRAM(Intcon) & 0b_0100_0000) == 0)))
			        {
				        Tmr0_int();
			        }
		        }
	        }
	        if((GetRAM(Intcon)&0b_1000_0000)==128){
		        if (((GetRAM(Intcon) & 0b_0000_1000) == 8) & ((GetRAM(Intcon) & 0b_0000_0001) == 1)){
			        Tmr0_int();
		        }
	        }
        }
        
        void Tmr0_int() {
	        WriteRAM(Intcon, Intcon & 0b_0111_1111);
	        _stack.Push(_programCounter);
	        _stackpointer++;
	        //Progcount = 4;//TODO:Hier mal Heiko fragen
	        //Progcount--;//TODO: Siehe Zeile darüber

        }

        private void RefreshProgram()
        {
	        byte pcl = Convert.ToByte(_programCounter | 0b_0000_0000_1111_1111);
	        byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);
	        
	        WriteRAM(Pcl,pcl);
	        WriteRAM(Pclath,pclath);
        }
    }
        
}