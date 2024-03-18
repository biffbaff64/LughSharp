// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////


namespace LughSharp.LibCore.Maths;

[PublicAPI]
public class NumberUtils
{
    public static int FloatToIntBits( float value )
    {
        var result = FloatToRawIntBits( value );

        // Check for NaN based on values of bit fields, maximum
        // exponent and nonzero significand.
        if ( ( ( result & FloatConsts.EXP_BIT_MASK ) == FloatConsts.EXP_BIT_MASK )
          && ( ( result & FloatConsts.SIGNIF_BIT_MASK ) != 0 ) )
        {
            result = 0x7fc00000;
        }

        return result;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int FloatToRawIntBits( float value )
    {
        return BitConverter.SingleToInt32Bits( value );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float IntBitsToFloat( int value )
    {
        return BitConverter.Int32BitsToSingle( value );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int FloatToIntColor( float value )
    {
        var intBits = FloatToRawIntBits( value );

        intBits |= ( int )( ( intBits >>> 24 ) * ( 255f / 254f ) ) << 24;

        return intBits;
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static float IntToFloatColor( int value )
    {
        return BitConverter.Int32BitsToSingle( value );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static long DoubleToLongBits( double value )
    {
        return BitConverter.DoubleToInt64Bits( value );
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static double LongBitsToDouble( long value )
    {
        return BitConverter.Int64BitsToDouble( value );
    }

    /// <summary>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int? ParseInt( string? str )
    {
        if ( int.TryParse( str, out var p ) )
        {
            return p;
        }

        return null;
    }

    /// <summary>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static float? ParseFloat( string? str )
    {
        if ( float.TryParse( str, out var p ) )
        {
            return p;
        }

        return null;
    }
}
