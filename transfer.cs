using System.Collections;

namespace picsim;

public class TransferSimToGUI
{
    public TransferSimToGUI(byte[] ram, int laufzeit, Stack stack, int stackpointer)
    {
        Ram = ram;
        _laufzeit = laufzeit;
        Stack = stack;
        Stackpointer = stackpointer;
    }

    public byte[] Ram1
    {
        get => Ram;
        set => Ram = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Laufzeit
    {
        get => _laufzeit;
        set => _laufzeit = value;
    }

    public Stack Stack1
    {
        get => Stack;
        set => Stack = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Stackpointer1
    {
        get => Stackpointer;
        set => Stackpointer = value;
    }

    private byte[] Ram;
    private int _laufzeit;
    private Stack Stack;
    private int Stackpointer;
    
}

public class TransferGuiToSim
{
    public string Path1
    {
        get => Path;
        set => Path = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ushort IoToggle1
    {
        get => IoToggle;
        set => IoToggle = value;
    }

    public ushort ControlButtons1
    {
        get => ControlButtons;
        set => ControlButtons = value;
    }

    public int[] Breakpoints1
    {
        get => Breakpoints;
        set => Breakpoints = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int RunMode1
    {
        get => RunMode;
        set => RunMode = value;
    }

    public bool Watchdog1
    {
        get => Watchdog;
        set => Watchdog = value;
    }

    public TransferGuiToSim(string path, ushort ioToggle, ushort controlButtons, int[] breakpoints, int runMode, bool watchdog)
    {
        Path = path;
        IoToggle = ioToggle;
        ControlButtons = controlButtons;
        Breakpoints = breakpoints;
        RunMode = runMode;
        Watchdog = watchdog;
    }

    private string Path;
    private ushort IoToggle;
    private ushort ControlButtons;
    private int[] Breakpoints;
    private int RunMode;
    private bool Watchdog;
}