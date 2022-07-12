using System;
using System.Collections;

namespace picsim
{

    public class Pic
    {
        //pic parts
        private byte[] _ram = new byte[256];
        private int _ramPointer = 0;
        private ushort[] _programMemory = new ushort[1024];
        private byte _wreg = 0;
        private int _laufzeit = 0;
        private int _programCounter = 0;
        private ushort _instructionRegister = 0;
        private Stack _stack = new Stack();

        //constants
        private const byte Indaddr = 0;
        private const byte Tmr0 = 0x01;
        private const byte Option = 0x81;
        private const byte Pcl = 0x02;
        private const byte Status = 0x03;
        private const byte Fsr = 0x04;
        private const byte Porta = 0x05;
        private const byte Trisa = 0x85;
        private const byte Portb = 0x06;
        private const byte Trisb = 0x86;
        private const byte Eedata = 0x08;
        private const byte Eecon1 = 0x88;
        private const byte Eeadr = 0x09;
        private const byte Eecon2 = 0x89;
        private const byte Pclath = 0x0A;
        private const byte Intcon = 0X0B;

        //constants statusreg
        private const byte Irp = 0b_1000_0000;
        private const byte Rp1 = 0b_0100_0000;
        private const byte Rp0 = 0b_0010_0000;
        private const byte To = 0b_0001_0000;
        private const byte Pd = 0b_0000_1000;
        private const byte Z = 0b_0000_0100;
        private const byte Dc = 0b_0000_0010;
        private const byte C = 0b_0000_0001;

        //constants portA
        private const byte Ra4 = 0b_0001_0000;
        private const byte Ra3 = 0b_0000_1000;
        private const byte Ra2 = 0b_0000_0100;
        private const byte Ra1 = 0b_0000_0010;
        private const byte Ra0 = 0b_0000_0001;

        //constants portB
        private const byte Rb7 = 0b_1000_0000;
        private const byte Rb6 = 0b_0100_0000;
        private const byte Rb5 = 0b_0010_0000;
        private const byte Rb4 = 0b_0001_0000;
        private const byte Rb3 = 0b_0000_1000;
        private const byte Rb2 = 0b_0000_0100;
        private const byte Rb1 = 0b_0000_0010;
        private const byte Rb0 = 0b_0000_0001;


        public int GetProgramCounter()
        {
            return _programCounter;
        }

        public void SetProgramCounter(int i)
        {
            _programCounter = i;
        }

        public void SetProgramMemory(string[] s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                ushort temp = ushort.Parse(s[i]);
                _programMemory[i] = temp;
            }
        }

        private void LoadInstructionRegister()
        {
            _instructionRegister = _programMemory[_programCounter];
        }

        private void DecodeOperation()
        {
            string[] ByteWiseOpcodes = Opcodes.GetOpcodesByteOriented();
            string[] BitWiseOpcodes = Opcodes.GetOpcodesBitOriented();
            string[] OpcodesLiteral = Opcodes.GetOpcodesLiteral();
            string[] OpcodesControl = Opcodes.GetOpCodesControl();
            
            int prog = _instructionRegister;
            string temp = prog.ToString();
            

        }
        
        private void Execute(string inst)
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
        }
        
    }