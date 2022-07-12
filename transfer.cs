using System.Collections;

namespace picsim;

public class Transfer
{
    public byte Ram1
    {
        get => Ram;
        set => Ram = value;
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
    private byte Ram;
    private int ProgramCounter;
    private Stack Stack;
    private int Stackpointer;
    //GUI to Sim
    private string Path;
    private ushort IoToggle;
    private ushort ControlButtons;
    private int[] Breakpoints;
}