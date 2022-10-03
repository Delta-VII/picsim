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
            "0000000", //NOP
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
            "00000001000011" //SLEEP
        };

        public Pic PicObject
        {
            get => pic;
            set => pic = value;
        }

        public void DecodeInstructions(int instructionsCode)
        {
            string inst = DecodeOpcode(instructionsCode);
            switch (inst)
            {
                case "000111":
                    program.Add(new addwf(instructionsCode, pic));
                    break;

                case "000101":
                    program.Add(new andwf(instructionsCode, pic));
                    break;

                case "0000011":
                    program.Add(new clrf(instructionsCode, pic));
                    break;

                case "0000010":
                    program.Add(new clrw(instructionsCode, pic));
                    break;

                case "001001":
                    program.Add(new comf(instructionsCode, pic));
                    break;

                case "000011":
                    program.Add(new decf(instructionsCode, pic));
                    break;

                case "001011":
                    program.Add(new decfsz(instructionsCode, pic));
                    break;

                case "001010":
                    program.Add(new incf(instructionsCode, pic));
                    break;

                case "001111":
                    program.Add(new incfsz(instructionsCode, pic));
                    break;

                case "000100":
                    program.Add(new iorwf(instructionsCode, pic));
                    break;

                case "0010000":
                    program.Add(new movf(instructionsCode, pic));
                    break;

                case "0000001":
                    program.Add(new movwf(instructionsCode, pic));
                    break;

                case "0000000":
                    program.Add(new nop(instructionsCode, pic));
                    break;

                case "0011010":
                    program.Add(new rlf(instructionsCode, pic));
                    break;

                case "0011000":
                    //program.Add(new rrf(instructionsCode, pic));
                    break;

                case "0000100":
                    program.Add(new subwf(instructionsCode, pic));
                    break;

                case "0011100":
                    program.Add(new swapf(instructionsCode, pic));
                    break;

                case "0001100":
                    program.Add(new xorwf(instructionsCode, pic));
                    break;

                case "0100":
                    program.Add(new bcf(instructionsCode, pic));
                    break;

                case "0101":
                    program.Add(new bsf(instructionsCode, pic));
                    break;

                case "0110":
                    program.Add(new btfsc(instructionsCode, pic));
                    break;

                case "0111":
                    program.Add(new btfss(instructionsCode, pic));
                    break;

                case "111110":
                    program.Add(new addlw(instructionsCode, pic));
                    break;

                case "111001":
                    program.Add(new andlw(instructionsCode, pic));
                    break;

                case "100000":
                    //program.Add(new call(instructionsCode, pic));
                    break;

                case "00000001100100":
                    //program.Add(new clrwdt(instructionsCode, pic));
                    break;

                case "101000":
                    program.Add(new Goto(instructionsCode, pic));
                    break;

                case "111000":
                    program.Add(new iorlw(instructionsCode, pic));
                    break;

                case "110000":
                    program.Add(new movlw(instructionsCode, pic));
                    break;

                case "00000000001001":
                    //program.Add(new retfie(instructionsCode, pic));
                    break;

                case "110100":
                    //program.Add(new retlw(instructionsCode, pic));
                    break;

                case "00000000001000":
                    //program.Add(new Return(instructionsCode, pic));
                    break;

                case "00000001000011":
                    //program.Add(new sleep(instructionsCode, pic));
                    break;

                case "111100":
                    program.Add(new sublw(instructionsCode, pic));
                    break;

                case "111010":
                    program.Add(new xorlw(instructionsCode, pic));
                    break;
                default:
                    break;
            }
        }

        private string DecodeOpcode(int inst)
        {
            var instString = Convert.ToString(inst, 2);

            var found = false;
            var result = "";


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
            RefreshRegisters();
            Debug.WriteLine(program[pic.ProgCntr]);
            program[pic.ProgCntr].Execute();
            pic.ProgCntr += 1;
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
            pic.RamBank0[0x03].Value = 0;
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
            pic.RamBank1[0x01].Value = 0;
            //PCL
            pic.RamBank1[0x02].Value = 0;
            //STATUS
            pic.RamBank1[0x03].Value = 0;
            //FSR
            pic.RamBank1[0x04].Value = 0;
            //TRISA
            pic.RamBank1[0x05].Value = 0;
            //TRISB
            pic.RamBank1[0x06].Value = 0;
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
            pic.RamBank1[0x02].Value = pic.RamBank0[0x02].Value;
            pic.RamBank1[0x03].Value = pic.RamBank0[0x03].Value;
            pic.RamBank1[0x04].Value = pic.RamBank0[0x04].Value;
            pic.RamBank1[0x0A].Value = pic.RamBank0[0x0A].Value;
            pic.RamBank1[0x0B].Value = pic.RamBank0[0x0B].Value;
        }

        public void ResetPic()
        {
        }
    }
}