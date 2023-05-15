namespace LibGDXSharp.Utils;

class Entry<TKe, TVe>
{
    public TKe? key;
    public TVe? value;

    public override string ToString()
    {
        return key + " = " + value;
    }
}