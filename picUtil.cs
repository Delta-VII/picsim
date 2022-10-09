using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using picsim.Instructions;
using picsim.Instructions.BitOrientedInstructions;
using picsim.Instructions.ByteOrientedInstructions;
using picsim.Instructions.ControlOperations;
using picsim.Instructions.LiteralOperations;

namespace picsim
{
    class PicUtil
    {
        public List<Instruction> program = new List<Instruction>();
        private Pic pic = new Pic();

        private string[] _opcodesByteOriented = new[]
        {
            "000111", //ADDWF
            "000101", //ANDWF
            "0000011", //CLRF
            "0000010", //CLRW
            "001001", //COMF
            "000011", //DECF
            "001011", //DECFSZ
            "001010", //INCF
            "001111", //INCFSZ
            "000100", //IORWF
            "0010000", //MOVF
            "0000001", //MOVWF
            "0011010", //RLF
            "0011000", //RRF
            "0000100", //SUBWF
            "0011100", //SWAPF
            "0001100" //XORWF
        };

        private string[] _opcodesBitOriented = new[]
        {
            "0100", //BCF
            "0101", //BSF
            "0110", //BTFSC
            "0111" //BTFSS
        };

        private string[] _opcodesLiteral = new[]
        {
            "111110", //ADDLW
            "111001", //ANDLW
            "100000", //CALL
            "101000", //GOTO
            "111000", //IORLW
            "110000", //MOVLW
            "110100", //RETLW
            "111100", //SUBLW
            "111010" //XORLW
        };

        private string[] _opCodesControl = new[]
        {
            "00000001100100", //CLRWDT
            "00000000001001", //RETFIE
            "00000000001000", //RETURN
            "00000001000011", //SLEEP
            "000000000000000" //NOP
        };

        public Pic PicObject
        {
            get => pic;
            set => pic = value;
        }

        public void DecodeInstructions(int instructionsCode)
        {
            var code = instructionsCode & 0b11000000000000;
            var ident = code >> 12;
            var code2 = instructionsCode & 0b00111100000000;
            var ident2 = code2 >> 8;
            var code3 = instructionsCode & 0b00000011110000;
            var ident3 = code3 >> 4;
            var ident4 = instructionsCode & 0b00000000001111;

            switch (ident)
            {
                case 0:
                {
                    if ((ident2 == 0 && ident3 == 0) && (ident4 == 8))
                    {
                        //RETURN
                        program.Add(new Return(instructionsCode, pic));
                    }

                    if ((ident2 == 0 && ident3 == 0) && (ident4 == 9))
                    {
                        //RETFIE
                        program.Add(new retfie(instructionsCode, pic));
                    }

                    if (ident2 == 0 && ident3 > 7)
                    {
                        //MOVWF
                        program.Add(new movwf(instructionsCode, pic));
                    }

                    if (ident2 == 7)
                    {
                        //ADDWF
                        program.Add(new addwf(instructionsCode, pic));
                    }

                    if (ident2 == 1 && ident3 > 7)
                    {
                        //CLRF
                        program.Add(new clrf(instructionsCode, pic));
                    }

                    if (ident2 == 1 && ident3 < 8)
                    {
                        //CLRW
                        program.Add(new clrw(instructionsCode, pic));
                    }

                    if (ident2 == 5)
                    {
                        //ANDWF
                        program.Add(new andwf(instructionsCode, pic));
                    }

                    if (ident2 == 9)
                    {
                        //COMPF
                        program.Add(new comf(instructionsCode, pic));
                    }

                    if (ident2 == 3)
                    {
                        //DECF
                        program.Add(new decf(instructionsCode, pic));
                    }

                    if (ident2 == 10)
                    {
                        //INCF
                        program.Add(new incf(instructionsCode, pic));
                    }

                    if (ident2 == 4)
                    {
                        //IORWF
                        program.Add(new iorwf(instructionsCode, pic));
                    }

                    if (ident2 == 8)
                    {
                        //MOVF
                        program.Add(new movf(instructionsCode, pic));
                    }

                    if (ident2 == 6)
                    {
                        //XORWF
                        program.Add(new xorwf(instructionsCode, pic));
                    }

                    if (ident2 == 14)
                    {
                        //SWAPF
                        program.Add(new swapf(instructionsCode, pic));
                    }

                    if (ident2 == 2)
                    {
                        //SUBWF
                        program.Add(new subwf(instructionsCode, pic));
                    }

                    if (ident2 == 11)
                    {
                        //DECFSZ
                        program.Add(new decfsz(instructionsCode, pic));
                    }

                    if (ident2 == 13)
                    {
                        //   qInfo() << "RLF" << "\n";                                             //RLF
                        program.Add(new rlf(instructionsCode, pic));
                    }

                    if (ident2 == 12)
                    {
                        //   qInfo() << "RRF" << "\n";                                                //RRF
                        program.Add(new rrf(instructionsCode, pic));
                    }

                    if (ident2 == 15)
                    {
                        //INCFSZ
                        program.Add(new incfsz(instructionsCode, pic));
                    }

                    if ((ident2 == 0) && (ident3 == 0) && (ident4 == 0))
                    {
                        //nop
                        program.Add(new nop(instructionsCode, pic));
                    }

                    if ((ident2 == 0) && (ident3 == 6) && (ident4 == 3))
                    {
                        //sleep
                        program.Add(new sleep(instructionsCode, pic));
                    }
                }
                    break;
                case 1:
                {

                    if ((ident2 & 0b1100) == 0)
                    {
                        //BCF
                        program.Add(new bcf(instructionsCode, pic));
                    }

                    if ((ident2 & 0b1100) == 4)
                    {
                        //BSF
                        program.Add(new bsf(instructionsCode, pic));
                    }

                    if ((ident2 & 0b1100) == 8)
                    {
                        //"BTFSC"
                        program.Add(new btfsc(instructionsCode, pic));
                    }

                    if ((ident2 & 0b1100) == 12)
                    {
                        // "BTFSS"
                        program.Add(new btfss(instructionsCode, pic));
                    }
                }
                    break;
                case 2:
                {
                    if (ident2 > 7)
                    {
                                       // goto
                        program.Add(new Goto(instructionsCode, pic));
                    }
                    if (ident2 < 8)
                    {
                        //call
                        program.Add(new call(instructionsCode, pic));
                    }

                }
                    break;
                case 3:
                {
                    if (ident2 < 4)
                    {
                        //MOVLW
                        program.Add(new movlw(instructionsCode, pic));
                    }

                    if ((ident2 == 4) || (ident2 == 5) || (ident2 == 6) || (ident2 == 7))
                    {
                        //RETLW
                        program.Add(new retlw(instructionsCode, pic));
                    }

                    if (ident2 == 8)
                    {
                        //IORLW
                        program.Add(new iorlw(instructionsCode, pic));
                    }

                    if (ident2 == 9)
                    {
                        //ANDLW
                        program.Add(new andlw(instructionsCode, pic));
                    }

                    if (ident2 == 10)
                    {
                        //XORLW
                        program.Add(new xorlw(instructionsCode, pic));
                    }

                    if ((ident2 == 12) || (ident2 == 13))
                    {
                        //SUBLW
                        program.Add(new sublw(instructionsCode, pic));
                    }

                    if ((ident2 == 14) || (ident2 == 15))
                    {
                        //ADDLW
                        program.Add(new addlw(instructionsCode, pic));
                    }
                }
                    break;
            }
        }

        private string DecodeOpcode(int inst)
        {
            var instString = Convert.ToString(inst, 2);

            var found = false;
            var result = "";

            switch (instString)
            {
                case "0":
                    result = "00000000000000";
                    found = true;
                    break;
                case "10000":
                    result = "0010000";
                    found = true;
                    break;
                case "1001":
                    result = "0010010";
                    found = true;
                    break;
                case "1000":
                    result = "00000000001000";
                    found = true;
                    break;
                default:
                    break;
            }
            
            //Byteoriented Instructions
            if (found == false)
            {
                foreach (var opcode in _opcodesByteOriented)
                {
                    instString.Substring(0, 7);
                    if (instString.StartsWith(opcode))
                    {
                        found = true;
                        result = opcode;
                        break;
                    }
                }
            }

            //Bitoriented Instructions
            if (found == false)
            {
                foreach (var opcode in _opcodesBitOriented)
                {
                    instString.Substring(0, 4);
                    if (instString.StartsWith(opcode))
                    {
                        found = true;
                        result = opcode;
                        break;
                    }
                }
            }

            //Literal Instructions
            if (found == false)
            {
                foreach (var opcode in _opcodesLiteral)
                {
                    instString.Substring(0, 6);
                    if (instString.StartsWith(opcode))
                    {
                        found = true;
                        result = opcode;
                        break;
                    }
                }
            }

            //Control Options
            if (found == false)
            {
                foreach (var opcode in _opCodesControl)
                {
                    if (instString.StartsWith(opcode))
                    {
                        found = true;
                        result = opcode;
                        break;
                    }
                }
            }

            return result;
        }

        public void Execute()
        {
            pic.checkinterrupt();
            RefreshRegisters();
            Debug.WriteLine(program[pic.ProgCntr]);
            program[pic.ProgCntr].Execute();
            pic.ProgCntr += 1;
            RefreshRegisters();
        }

        public void InitPic()
        {
            for (int i = 0; i <= 127; i++)
            {
                pic.RamBank0.Add(new ramCell());
                pic.RamBank1.Add(new ramCell());
            }

            for (int i = 0; i <= 127; i++)
            {
                pic.RamBank0[i].Address = i;
            }

            for (int i = 0; i <= 127; i++)
            {
                pic.RamBank1[i].Address = i + 128;
            }

            pic.Wreg = 0;
            //INDF
            pic.RamBank0[0x00].Value = 0;
            //TMR0
            pic.RamBank0[0x01].Value = 0;
            //PCL
            pic.RamBank0[0x02].Value = 0;
            //STATUS
            pic.RamBank0[0x03].Value = 0b_0001_1000;
            //FSR
            pic.RamBank0[0x04].Value = 0;
            //PORTA
            pic.RamBank0[0x05].Value = 0;
            //PORTB
            pic.RamBank0[0x06].Value = 0;
            //EEDATA
            pic.RamBank0[0x08].Value = 0;
            //EEADR
            pic.RamBank0[0x09].Value = 0;
            //PCLATH
            pic.RamBank0[0x0A].Value = 0;
            //INTCON
            pic.RamBank0[0x0B].Value = 0;
            //INDF
            pic.RamBank1[0x00].Value = 0;
            //OPTION_REG
            pic.RamBank1[0x01].Value = 0b_1111_1111;
            //PCL
            pic.RamBank1[0x02].Value = 0;
            //STATUS
            pic.RamBank1[0x03].Value = 0b_0001_1000;
            //FSR
            pic.RamBank1[0x04].Value = 0;
            //TRISA
            pic.RamBank1[0x05].Value = 0b_0001_1111;
            //TRISB
            pic.RamBank1[0x06].Value = 0b_1111_1111;
            //EECON1
            pic.RamBank1[0x08].Value = 0;
            //EECON2
            pic.RamBank1[0x09].Value = 0;
            //PCLATH
            pic.RamBank1[0x0A].Value = 0;
            //INTCON
            pic.RamBank1[0x0B].Value = 0;
        }

        private void RefreshRegisters()
        {
            SetPclath();
            pic.RamBank1[0x02].Value = pic.RamBank0[0x02].Value; //mirroring of PCL
            pic.RamBank1[0x03].Value = pic.RamBank0[0x03].Value; //mirroring of STATUS
            pic.RamBank1[0x04].Value = pic.RamBank0[0x04].Value; //mirroring of FSR
            pic.RamBank1[0x0A].Value = pic.RamBank0[0x0A].Value; //mirroring of PCLATH
            pic.RamBank1[0x0B].Value = pic.RamBank0[0x0B].Value; //mirroring of INTCON
            pic.RamBank1[0x00].Value = pic.RamBank0[0x00].Value; //mirroring of INDF
            pic.RamBank0[0x02].Value = pic.ProgCntr & 0b_1111_1111; //PCL
        }

        private void SetPclath()
        {
            var temp = pic.RamBank0[0x02].Value;
            var temp1 = temp & 0b_1_1111_0000_0000;
            var result = pic.RotateRight(Convert.ToUInt16(temp1), 17);
            pic.RamBank0[0x0A].Value = Convert.ToInt32(result);
        }
        
    }
}