/*
namespace picsim;

internal partial class Pic
{
    
    private void Addwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(_wreg + temp) ;
        CheckZ(value);
        CheckDc(value);
        CheckC(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
    }
    
    private void Andwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(_wreg & temp) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Clrf()
    {
        CheckInterrupts();
        int address = DecodeFBits();
        WriteRam(Convert.ToByte(address), 0);
        SetZ(true);
        ProgrammLaufzeit += 1;
        RefreshRegisters();    
    }
    
    private void Clrw()
    {
        CheckInterrupts();
        _wreg = 0;
        SetZ(true);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Comf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(~temp);
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Decf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(temp - 1) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Decfsz()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(temp - 1) ;
        SaveResult(DecodeDBit(), value, address);
        if (value != 0)
        {
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            Nop();
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        RefreshRegisters();
        
    }
    
    private void Incf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(temp + 1) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
    }
    
    private void Incfsz()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(temp + 1) ;
        SaveResult(DecodeDBit(), value, address);
        if (value != 0)
        {
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            Nop();
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        RefreshRegisters();
    }
    
    private void Iorwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(_wreg | temp) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Movf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte value = GetRam(address);
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Movwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        WriteRam(address,_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();    
    }
    
    private void Nop()
    {
        CheckInterrupts();
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Rlf()
    {
        byte address = DecodeFBits();
        byte temp = GetRam(address);

        bool carry;

        if ((temp | 0b_1000_000) == 0)
        {
            SetC(false);
        }
        else
        {
            SetC(true);
        }
        byte value = Convert.ToByte(temp << 1);
        SaveResult(DecodeDBit(),value, address);
        CheckInterrupts();
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Rrf()
    {
        byte address = DecodeFBits();
        byte temp = GetRam(address);

        bool carry;

        if ((temp | 0b_0000_0001) == 0)
        {
            SetC(false);
        }
        else
        {
            SetC(true);
        }
        byte value = Convert.ToByte(temp >> 1);
        SaveResult(DecodeDBit(),value, address);
        CheckInterrupts();
        ProgrammLaufzeit += 1;
        RefreshRegisters();
    }
    
    private void Subwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRam(address);
        byte value = Convert.ToByte(temp + (~_wreg + 1)) ;
        CheckZ(value);
        CheckDc(value);
        CheckC(value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Swapf()
    {   byte address = DecodeFBits();
        byte value = GetRam(address);

        byte upperNibble = Convert.ToByte(value & 0b_1111_0000);
        byte lowerNibble = Convert.ToByte(value & 0b_0000_1111);

        byte result = Convert.ToByte(lowerNibble | upperNibble);
        
        WriteRam(address, value);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Xorwf()
    {
        byte address = DecodeFBits();
        byte value = GetRam(address);

        byte result = Convert.ToByte(_wreg ^ value);
        SaveResult(DecodeDBit(), value, address);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Bcf()
    {   
        byte address = DecodeFBits();
        byte value = GetRam(address);
        byte bit = DecodeBBits();
        byte bitin = Convert.ToByte(~bit);
        byte result = Convert.ToByte(bitin & value);
        WriteRam(address,result);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Bsf()
    {   
        byte address = DecodeFBits();
        byte value = GetRam(address);
        byte bit = DecodeBBits();
        byte bitin = Convert.ToByte(~bit);
        byte result = Convert.ToByte(bitin | value);
        WriteRam(address,result);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Btfsc()
    {
        byte address = DecodeFBits();
        byte value = GetRam(address);
        byte bit = DecodeBBits();
        byte bitin = Convert.ToByte(~bit);
        if ((bitin & value) == 0)
        {
            _programCounter =+ 2;
            Nop();
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
    }
    
    private void Btfss()
    {
        byte address = DecodeFBits();
        byte value = GetRam(address);
        byte bit = DecodeBBits();
        byte bitin = Convert.ToByte(~bit);
        if ((bitin & value) == 0)
        {
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            Nop();
            ProgrammLaufzeit += 1;
            RefreshRegisters();
        }
        
    }
    
    private void Addlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg & literal);
        CheckZ(_wreg);
        CheckC(_wreg);
        CheckDc(_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Andlw()
    {
        byte address = DecodeFBits();
        byte value = GetRam(address);
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg & literal);
        CheckZ(_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Call()
    {
        Stack.Push(_programCounter + 1);
        Stackpointer++;
        int literal = DecodeLiteralJump();
        byte pcl = Convert.ToByte(literal);
        byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);
        byte pclath43 = Convert.ToByte(pclath | 0b_1_1000);
        int value = pclath43 | pcl;
        _programCounter = value;
        ProgrammLaufzeit =+ 2;

    }
    
    private void Clrwdt()
    {
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void GOto()
    {
        int literal = DecodeLiteralJump();
        byte pcl = Convert.ToByte(literal);
        byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);
        byte pclath43 = Convert.ToByte(pclath | 0b_1_1000);
        int value = pclath43 | pcl;
        _programCounter = value;
        ProgrammLaufzeit =+ 2;

    }
    
    private void Iorwl()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg | literal);
        CheckZ(_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Movlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = literal;
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Retfie()
    {
        int value = (int) Stack.Pop();
        Stackpointer--;
        _programCounter = value;
        WriteRam(Intcon, Convert.ToByte(GetRam(Intcon) | 0b_0100_0000));
        ProgrammLaufzeit += 1;
        Nop();
    }
    
    private void Retlw()
    {
        int value = (int) Stack.Pop();
        Stackpointer--;
        _wreg = DecodeLiteralGeneral();
        _programCounter = value;
        ProgrammLaufzeit += 1;
        Nop();
    }
    
    private void REturn()
    {
        int value = (int) Stack.Pop();
        Stackpointer--;
        _programCounter = value;
        ProgrammLaufzeit += 1;
        Nop();
    }
    
    private void Sleep()
    {
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Sublw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(literal - (~_wreg + 1));
        CheckZ(_wreg);
        CheckC(_wreg);
        CheckDc(_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void Xorlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg ^ literal);
        CheckZ(_wreg);
        ProgrammLaufzeit += 1;
        RefreshRegisters();
    }
    
}
*/