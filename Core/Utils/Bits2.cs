namespace LibGDXSharp.Utils;

public partial class Bits
{
    private static ByteOrder? _byteOrder;

    public static ByteOrder ByteOrder
    {
        get
        {
            if ( _byteOrder == null )
            {
                throw new GdxRuntimeException( "Unknown Byte Order!" );
            }

            return _byteOrder;
        }
        set => _byteOrder = value;
    }
}
