using System;
using System.Collections;

public class pic
{
    //pic parts
    private byte[] _ram = new byte[256];
    private int RAMpointer = 0;
    private ushort[] _ProgramMemory = new ushort[1024];
    private byte wreg = 0;
    private int laufzeit = 0;
    private int programCounter = 0;
    private ushort InstructionRegister = 0;
    private Stack stack = new Stack();


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
        return this.programCounter;
    }

    public void SetProgramCounter(int i)
    {
        this.programCounter = i;
    }

    public void SetProgramMemory(string[] s)
    {
        for (int i = 0; i < s.Length; i++)
        {
            ushort temp = ushort.Parse(s[i]);
            _ProgramMemory[i] = temp;
        }
    }

    private void LoadInstructionRegister()
    {
        this.InstructionRegister = _ProgramMemory[programCounter];
    }

    private void decode()
    {

    }



}