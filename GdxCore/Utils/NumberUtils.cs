namespace LibGDXSharp.Utils
{
    public class NumberUtils
    {
        public static int FloatToIntBits( float value )
        {
            var result = FloatToRawIntBits( value );

            // Check for NaN based on values of bit fields, maximum
            // exponent and nonzero significand.
            if ( ( ( result & FloatConsts.Exp_Bit_Mask ) == FloatConsts.Exp_Bit_Mask )
                 && ( result & FloatConsts.Signif_Bit_Mask ) != 0 )
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
}
