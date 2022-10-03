namespace picsim;

public class ramCell
{
    private int _address;
    private int _value;

    public int Address
    {
        get => _address;
        set => _address = value;
    }

    public int Value
    {
        get => _value;
        set => _value = value;
    }
}