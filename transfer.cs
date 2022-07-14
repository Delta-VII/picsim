using System.Collections;

namespace picsim;

public class Transfer
{
    public Transfer(byte[] ram, int programCounter, Stack stack, int stackpointer, string path, ushort ioToggle, ushort controlButtons, int[] breakpoints)
    {
        Ram = ram;
        ProgramCounter = programCounter;
        Stack = stack;
        Stackpointer = stackpointer;
        Path = path;
        IoToggle = ioToggle;
        ControlButtons = controlButtons;
        Breakpoints = breakpoints;
    }

    public byte[] Ram1
    {
        get => Ram;
        set => Ram = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int ProgramCounter1
    {
        get => ProgramCounter;
        set => ProgramCounter = value;
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

    //Sim to GUI
    private byte[] Ram;
    private int ProgramCounter;
    private Stack Stack;
    private int Stackpointer;
    //GUI to Sim
    private string Path;
    private ushort IoToggle;
    private ushort ControlButtons;
    private int[] Breakpoints;
}