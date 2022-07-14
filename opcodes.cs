using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace picsim
{
    public class Opcodes
    {
        private static string[] _opcodesByteOriented = { "000111", "000101", "0000011", "0000010", "001001", "000011", "001011", "001010", "001111", "000100", "0010000", "0000001", "0000000", "0011010", "0011000", "0000100", "0011100", "0001100" };
        private static string[] _opcodesBitOriented = { "0100", "0101", "0110", "0111" };
        private static string[] _opcodesLiteral = { "111110", "111001", "100000", "101000", "111000", "110000", "110100", "111100", "111010" };
        private static string[] _opCodesControl = { "00000001100100", "00000000001001", "00000000001000", "00000001000011" };

        public static string[] GetOpcodesByteOriented()
        {
            return _opcodesByteOriented;
        }

        public static string[] GetOpcodesBitOriented()
        {
            return _opcodesBitOriented;
        }

        public static string[] GetOpcodesLiteral()
        {
            return _opcodesLiteral;
        }

        public static string[] GetOpCodesControl()
        {
            return _opCodesControl;
        }
    }

}