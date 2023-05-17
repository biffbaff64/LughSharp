using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class DataUtils
{
    /// <summary>
    /// Returns the value obtained by reversing the order of the bytes in the
    /// two's complement representation of the specified <tt>short</tt> value.
    /// </summary>
    /// <param name="input">the value whose bytes are to be reversed</param>
    /// <returns>
    /// the value obtained by reversing (or, equivalently, swapping) the bytes
    /// in the specified short value.
    /// </returns>
    public static short ReverseBytes( short input )
    {
        return (short) (((input & 0xFF00) >> 8) | (input << 8));
    }
        
    public static char ReverseBytes( char input )
    {
        return (char) (((input & 0xFF00) >> 8) | (input << 8));
    }
        
    public static int ReverseBytes( int input )
    {
        return ((input >>> 24)           ) |
               ((input >>   8) &   0xFF00) |
               ((input <<   8) & 0xFF0000) |
               ((input << 24));
    }
        
    public static long ReverseBytes( long input )
    {
        input = ( (input & 0x00ff00ff00ff00ffL) << 8 ) | ( (input >>> 8) & 0x00ff00ff00ff00ffL );
            
        return (input << 48) | ((input & 0xffff0000L) << 16) |
               ((input >>> 16) & 0xffff0000L) | (input >>> 48);
    }
        
    /// <summary>
    /// Converts the argument to an <tt>int</tt> by an unsigned
    /// conversion.  In an unsigned conversion to an <tt>int</tt>, the
    /// high-order 16 bits of the <tt>int</tt> are zero and the
    /// low-order 16 bits are equal to the bits of the <tt>short</tt> argument.
    /// 
    /// Consequently, zero and positive <tt>short</tt> values are mapped
    /// to a numerically equal <tt>int</tt> value and negative <tt>
    /// short</tt> values are mapped to an <tt>int</tt> value equal to the
    /// input plus 2<sup>16</sup>.
    /// </summary>
    /// <param name="x"> the value to convert to an unsigned <tt>int</tt> </param>
    /// <returns> the argument converted to <tt>int</tt> by an unsigned
    ///         conversion
    /// </returns>
    public static int ToUnsignedInt(short x)
    {
        return ((int) x) & 0xffff;
    }

    /// <summary>
    /// Converts the argument to a <tt>long</tt> by an unsigned
    /// conversion.  In an unsigned conversion to a <tt>long</tt>, the
    /// high-order 48 bits of the <tt>long</tt> are zero and the
    /// low-order 16 bits are equal to the bits of the <tt>short</tt> argument.
    /// 
    /// Consequently, zero and positive <tt>short</tt> values are mapped
    /// to a numerically equal <tt>long</tt> value and negative <tt>
    /// short</tt> values are mapped to a <tt>long</tt> value equal to the
    /// input plus 2<sup>16</sup>.
    /// </summary>
    /// <param name="x"> the value to convert to an unsigned <tt>long</tt> </param>
    /// <returns> the argument converted to <tt>long</tt> by an unsigned
    ///         conversion
    /// </returns>
    public static long ToUnsignedLong(short x)
    {
        return ((long) x) & 0xffffL;
    }
}