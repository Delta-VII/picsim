using System;
using System.Collections.Generic;
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
        public List<object> program;
        private Pic pic;
        private System.Collections.Generic.Dictionary<int, string> opcodes = new Dictionary<int, string>()
        {
            {0b_00_0111_0000_0000, "ADDWF"},
            {0b_00_0101_0000_0000, "ANDWF"},
            {0b_00_0001_1000_0000, "CLRF"},
            {0b_00_0001_0000_0000, "CLRW"},
            {0b_00_1001_0000_0000, "COMF"},
            {0b_00_0011_0000_0000, "DECF"},
            {0b_00_1011_0000_0000, "DECFSZ"},
            {0b_00_1010_0000_0000, "INCF"},
            {0b_00_1111_0000_0000, "INCFSZ"},
            {0b_00_0100_0000_0000, "IORWF"},
            {0b_00_1000_0000_0000, "MOVF"},
            {0b_00_0000_1000_0000, "MOVWF"},
            {0b_00_0000_0000_0000, "NOP"},
            {0b_00_1101_0000_0000, "RLF"},
            {0b_00_1100_0000_0000, "RRF"},
            {0b_00_0010_0000_0000, "SUBWF"},
            {0b_00_1110_0000_0000, "SWAPF"},
            {0b_00_0110_0000_0000, "XORWF"},
            {0b_01_0000_0000_0000, "BCF"},
            {0b_01_0100_0000_0000, "BSF"},
            {0b_01_1000_0000_0000, "BTFSC"},
            {0b_01_1100_0000_0000, "BTFSS"},
            {0b_11_1110_0000_0000, "ADDLW"},
            {0b_11_1001_0000_0000, "ANDLW"},
            {0b_10_0000_0000_0000, "CALL"},
            {0b_00_0000_0110_0100, "CLRWDT"},
            {0b_10_1000_0000_0000, "GOTO"},
            {0b_11_1000_0000_0000, "IORLW"},
            {0b_11_0000_0000_0000, "MOVLW"},
            {0b_00_0000_0000_1001, "RETFIE"},
            {0b_11_0100_0000_0000, "RETLW"},
            {0b_00_0000_0000_1000, "RETURN"},
            {0b_00_0000_0100_0011, "SLEEP"},
            {0b_11_1100_0000_0000, "SUBLW"},
            {0b_11_1010_0000_0000, "XORLW"}
        };

        public void DecodeInstructions(List<int> instructionsCodes)
        {
            foreach (var item in instructionsCodes)
            {
                string inst = DecodeOpcode(item);
                switch (inst)
                {
                    case "ADDWF":
                        program.Add(new addwf(item, pic));
                        break;

                    case "ANDWF":
                        program.Add(new andwf(item, pic));
                        break;

                    case "CLRF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "CLRW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "DECF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "DECFSZ":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "INCF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "INCFSZ":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "IORWF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "MOVF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "MOVWF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "NOP":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "RLF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "RRF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "SUBWF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "SWAPF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "XORWF":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "BCF":
                        program.Add(new bcf(item, pic));
                        break;

                    case "BSF":
                        program.Add(new bsf(item, pic));
                        break;

                    case "BTFSC":
                        program.Add(new btfsc(item, pic));
                        break;

                    case "BTFSS":
                        program.Add(new btfss(item, pic));
                        break;

                    case "ADDLW":
                        program.Add(new addlw(item, pic));
                        break;

                    case "ANDLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "CALL":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "CLRWDT":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "GOTO":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "IORLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "MOVLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "RETFIE":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "RETLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "RETURN":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "SLEEP":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "SUBLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    case "XORLW":
                        program.Add(new Bcf(item, pic));
                        break;

                    default:
                        break;

                }
            }
        }

        private string DecodeOpcode(int inst)
        {
            string opcode = "";
            foreach (var item in opcodes)
            {
                int temp = item.Key;
                if ((temp & inst) == temp)
                {
                    opcode = item.Value;
                }
            }
            return opcode;
        }

    }
}