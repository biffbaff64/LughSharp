// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils;

public class NumberUtils
{
    public static int FloatToIntBits( float value )
    {
        var result = FloatToRawIntBits( value );

        // Check for NaN based on values of bit fields, maximum
        // exponent and nonzero significand.
        if ( ( ( result & FloatConsts.Exp_Bit_Mask ) == FloatConsts.Exp_Bit_Mask )
             && ( ( result & FloatConsts.Signif_Bit_Mask ) != 0 ) )
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
}