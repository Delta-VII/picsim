namespace picsim;

internal partial class Pic
{
    //constants
    public const int Indaddr = 0;
    public const int Tmr0 = 0x01;
    public const int Option = 0x81;
    public const int Pcl = 0x02;
    public const int Status = 0x03;
    public const int Fsr = 0x04;
    public const int Porta = 0x05;
    public const int Trisa = 0x85;
    public const int Portb = 0x06;
    public const int Trisb = 0x86;
    public const int Eedata = 0x08;
    public const int Eecon1 = 0x88;
    public const int Eeadr = 0x09;
    public const int Eecon2 = 0x89;
    public const int Pclath = 0x0A;
    public const int Intcon = 0X0B;

    //constants statusreg
    public const int Irp = 7;
    public const int Rp1 = 6;
    public const int Rp0 = 5;
    public const int To = 4;
    public const int Pd = 3;
    public const int Z = 2;
    public const int Dc = 1;
    public const int C = 0;

    //constants portA
    public const int Ra4 = 0b_0001_0000;
    public const int Ra3 = 0b_0000_1000;
    public const int Ra2 = 0b_0000_0100;
    public const int Ra1 = 0b_0000_0010;
    public const int Ra0 = 0b_0000_0001;

    //constants portB
    public const int Rb7 = 0b_1000_0000;
    public const int Rb6 = 0b_0100_0000;
    public const int Rb5 = 0b_0010_0000;
    public const int Rb4 = 0b_0001_0000;
    public const int Rb3 = 0b_0000_1000;
    public const int Rb2 = 0b_0000_0100;
    public const int Rb1 = 0b_0000_0010;
    public const int Rb0 = 0b_0000_0001;
}