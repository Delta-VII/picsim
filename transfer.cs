using System.Collections;

namespace Try;

public class TransferSimToGui
{
    public byte[] Ram1
    {
        get => _ram;
        set => _ram = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Laufzeit
    {
        get => _laufzeit;
        set => _laufzeit = value;
    }

    public Stack Stack1
    {
        get => _stack;
        set => _stack = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Stackpointer1
    {
        get => _stackpointer;
        set => _stackpointer = value;
    }

    public bool[] Sfr
    {
        get => _sFr;
        set => _sFr = value;
    }

    private byte[] _ram;
    private int _laufzeit;
    private Stack _stack;
    private int _stackpointer;
    private bool[] _sFr;
}

public class TransferGuiToSim
{
    public string Path1
    {
        get => _path;
        set => _path = value ?? throw new ArgumentNullException(nameof(value));
    }

    public bool[] IoToggleRa
    {
        get => _ioToggleRa;
        set => _ioToggleRa = value;
    }

    public bool[] IoToggleRb
    {
        get => _ioToggleRb;
        set => _ioToggleRb = value;
    }

    public ushort ControlButtons1
    {
        get => _controlButtons;
        set => _controlButtons = value;
    }

    public int[] Breakpoints1
    {
        get => _breakpoints;
        set => _breakpoints = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int RunMode1
    {
        get => _runMode;
        set => _runMode = value;
    }

    public bool Watchdog1
    {
        get => _watchdog;
        set => _watchdog = value;
    }

    
    private string _path;
    private bool[] _ioToggleRa;
    private bool[] _ioToggleRb;
    private ushort _controlButtons;
    private int[] _breakpoints;
    private int _runMode;
    private bool _watchdog;
}