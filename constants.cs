namespace picsim;

public partial class Pic
{
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
}