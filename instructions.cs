namespace Try;

public partial class Pic
{
    
    private void addwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(_wreg + temp) ;
        CheckZ(value);
        CheckDC(value);
        CheckC(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
    }
    
    private void andwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(_wreg & temp) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void clrf()
    {
        CheckInterrupts();
        int address = DecodeFBits();
        WriteRAM(Convert.ToByte(address), 0);
        SetZ(true);
        _laufzeit += 1;
        RefreshRegisters();    
    }
    
    private void clrw()
    {
        CheckInterrupts();
        _wreg = 0;
        SetZ(true);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void comf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(~temp);
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void decf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(temp - 1) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void decfsz()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(temp - 1) ;
        SaveResult(DecodeDBit(), value, address);
        if (value != 0)
        {
            _laufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            nop();
            _laufzeit += 1;
            RefreshRegisters();
        }
        RefreshRegisters();
        
    }
    
    private void incf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(temp + 1) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
    }
    
    private void incfsz()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(temp + 1) ;
        SaveResult(DecodeDBit(), value, address);
        if (value != 0)
        {
            _laufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            nop();
            _laufzeit += 1;
            RefreshRegisters();
        }
        RefreshRegisters();
    }
    
    private void iorwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(_wreg | temp) ;
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void movf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        CheckZ(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void movwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        WriteRAM(address,_wreg);
        _laufzeit += 1;
        RefreshRegisters();    
    }
    
    private void nop()
    {
        CheckInterrupts();
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void rlf()
    {
        byte address = DecodeFBits();
        byte temp = GetRAM(address);

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
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void rrf()
    {
        byte address = DecodeFBits();
        byte temp = GetRAM(address);

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
        _laufzeit += 1;
        RefreshRegisters();
    }
    
    private void subwf()
    {
        CheckInterrupts();
        byte address = DecodeFBits();
        byte temp = GetRAM(address);
        byte value = Convert.ToByte(temp + (~_wreg + 1)) ;
        CheckZ(value);
        CheckDC(value);
        CheckC(value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void swapf()
    {   byte address = DecodeFBits();
        byte value = GetRAM(address);

        byte upperNibble = Convert.ToByte(value & 0b_1111_0000);
        byte lowerNibble = Convert.ToByte(value & 0b_0000_1111);

        byte result = Convert.ToByte(lowerNibble | upperNibble);
        
        WriteRAM(address, value);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void xorwf()
    {
        byte address = DecodeFBits();
        byte value = GetRAM(address);

        byte result = Convert.ToByte(_wreg ^ value);
        SaveResult(DecodeDBit(), value, address);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void bcf()
    {   
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        byte bit = DecodeBBits();
        byte Bitin = Convert.ToByte(~bit);
        byte result = Convert.ToByte(Bitin & value);
        WriteRAM(address,result);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void bsf()
    {   
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        byte bit = DecodeBBits();
        byte Bitin = Convert.ToByte(~bit);
        byte result = Convert.ToByte(Bitin | value);
        WriteRAM(address,result);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void btfsc()
    {
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        byte bit = DecodeBBits();
        byte Bitin = Convert.ToByte(~bit);
        if ((Bitin & value) == 0)
        {
            _programCounter =+ 2;
            nop();
            _laufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _laufzeit += 1;
            RefreshRegisters();
        }
    }
    
    private void btfss()
    {
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        byte bit = DecodeBBits();
        byte Bitin = Convert.ToByte(~bit);
        if ((Bitin & value) == 0)
        {
            _laufzeit += 1;
            RefreshRegisters();
        }
        else
        {
            _programCounter =+ 2;
            nop();
            _laufzeit += 1;
            RefreshRegisters();
        }
        
    }
    
    private void addlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg & literal);
        CheckZ(_wreg);
        CheckC(_wreg);
        CheckDC(_wreg);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void andlw()
    {
        byte address = DecodeFBits();
        byte value = GetRAM(address);
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg & literal);
        CheckZ(_wreg);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void call()
    {
        _stack.Push(_programCounter + 1);
        _stackpointer++;
        int literal = DecodeLiteralJump();
        byte pcl = Convert.ToByte(literal);
        byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);
        byte pclath43 = Convert.ToByte(pclath | 0b_1_1000);
        int value = pclath43 | pcl;
        _programCounter = value;
        _laufzeit =+ 2;

    }
    
    private void clrwdt()
    {
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void gOTO()
    {
        int literal = DecodeLiteralJump();
        byte pcl = Convert.ToByte(literal);
        byte pclath = Convert.ToByte(_programCounter | 0b_0001_1111_0000_0000);
        byte pclath43 = Convert.ToByte(pclath | 0b_1_1000);
        int value = pclath43 | pcl;
        _programCounter = value;
        _laufzeit =+ 2;

    }
    
    private void iorwl()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg | literal);
        CheckZ(_wreg);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void movlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = literal;
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void retfie()
    {
        int value = (int) _stack.Pop();
        _stackpointer--;
        _programCounter = value;
        WriteRAM(Intcon, Convert.ToByte(GetRAM(Intcon) | 0b_0100_0000));
        _laufzeit += 1;
        nop();
    }
    
    private void retlw()
    {
        int value = (int) _stack.Pop();
        _stackpointer--;
        _wreg = DecodeLiteralGeneral();
        _programCounter = value;
        _laufzeit += 1;
        nop();
    }
    
    private void rETURN()
    {
        int value = (int) _stack.Pop();
        _stackpointer--;
        _programCounter = value;
        _laufzeit += 1;
        nop();
    }
    
    private void sleep()
    {
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void sublw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(literal - (~_wreg + 1));
        CheckZ(_wreg);
        CheckC(_wreg);
        CheckDC(_wreg);
        _laufzeit += 1;
        RefreshRegisters();
        
    }
    
    private void xorlw()
    {
        byte literal = DecodeLiteralGeneral();
        _wreg = Convert.ToByte(_wreg ^ literal);
        CheckZ(_wreg);
        _laufzeit += 1;
        RefreshRegisters();
    }
    
}
