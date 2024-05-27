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


namespace LughSharp.LibCore.Graphics;

/// <summary>
/// A color class, holding the r, g, b and alpha component as floats in
/// the range [0,1].
/// All methods perform clamping on the internal values after execution.
/// </summary>
[PublicAPI]
public sealed class Color
{
    public readonly static Color White      = new( 1, 1, 1, 1 );
    public readonly static Color LightGray  = new( 0xbfbfbfff );
    public readonly static Color Gray       = new( 0x7f7f7fff );
    public readonly static Color DarkGray   = new( 0x3f3f3fff );
    public readonly static Color Black      = new( 0, 0, 0, 1 );
    public readonly static Color Clear      = new( 0, 0, 0, 0 );
    public readonly static Color Blue       = new( 0, 0, 1, 1 );
    public readonly static Color Navy       = new( 0, 0, 0.5f, 1 );
    public readonly static Color Royal      = new( 0x4169e1ff );
    public readonly static Color Slate      = new( 0x708090ff );
    public readonly static Color Sky        = new( 0x87ceebff );
    public readonly static Color Cyan       = new( 0, 1, 1, 1 );
    public readonly static Color Teal       = new( 0, 0.5f, 0.5f, 1 );
    public readonly static Color Green      = new( 0x00ff00ff );
    public readonly static Color Chartreuse = new( 0x7fff00ff );
    public readonly static Color Lime       = new( 0x32cd32ff );
    public readonly static Color Forest     = new( 0x228b22ff );
    public readonly static Color Olive      = new( 0x6b8e23ff );
    public readonly static Color Yellow     = new( 0xffff00ff );
    public readonly static Color Gold       = new( 0xffd700ff );
    public readonly static Color Goldenrod  = new( 0xdaa520ff );
    public readonly static Color Orange     = new( 0xffa500ff );
    public readonly static Color Brown      = new( 0x8b4513ff );
    public readonly static Color Tan        = new( 0xd2b48cff );
    public readonly static Color Firebrick  = new( 0xb22222ff );
    public readonly static Color Red        = new( 0xff0000ff );
    public readonly static Color Scarlet    = new( 0xff341cff );
    public readonly static Color Coral      = new( 0xff7f50ff );
    public readonly static Color Salmon     = new( 0xfa8072ff );
    public readonly static Color Pink       = new( 0xff69b4ff );
    public readonly static Color Magenta    = new( 1, 0, 1, 1 );
    public readonly static Color Purple     = new( 0xa020f0ff );
    public readonly static Color Violet     = new( 0xee82eeff );
    public readonly static Color Maroon     = new( 0xb03060ff );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Convenience for frequently used <tt>White.ToFloatBits()</tt>
    /// </summary>
    public readonly static float WhiteFloatBits = White.ToFloatBits();

    /// <summary>
    /// Constructor, sets all the components to 0.
    /// </summary>
    public Color() : this( 0, 0, 0, 0 )
    {
    }

    /// <summary>
    /// Constructor, sets the Color components using the specified integer value in
    /// the format RGBA8888. This is inverse to the rgba8888(r, g, b, a) method.
    /// </summary>
    /// <param name="rgba8888"> An integer color value in RGBA8888 format. </param>
    public Color( int rgba8888 ) : this( ( uint ) rgba8888 )
    {
    }

    /// <summary>
    /// Constructor, sets the Color components using the specified integer value in
    /// the format RGBA8888. This is inverse to the rgba8888(r, g, b, a) method.
    /// </summary>
    /// <param name="rgba8888"> An uint color value in RGBA8888 format. </param>
    public Color( uint rgba8888 )
    {
        Set( rgba8888 );
    }

    /// <summary>
    /// Constructor, sets the components of the color.
    /// </summary>
    /// <param name="r"> The red component. </param>
    /// <param name="g"> The green component. </param>
    /// <param name="b"> The blue component. </param>
    /// <param name="a"> The alpha component. </param>
    public Color( float r, float g, float b, float a )
    {
        R = r;
        G = g;
        B = b;
        A = a;

        Clamp();
    }

    /// <summary>
    /// Constructs a new color using the components from the supplied color.
    /// </summary>
    public Color( Color color )
    {
        Set( color );
    }

    /// <summary>
    /// Sets this colors components using the components from the supplied colot.
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    public Color Set( Color color )
    {
        Set( color.R, color.G, color.B, color.A );

        return this;
    }

    /// <summary>
    /// Sets this colors components using the supplied r,g,b,a components.
    /// </summary>
    /// <returns></returns>
    public Color Set( float r, float g, float b, float a )
    {
        R = r;
        G = g;
        B = b;
        A = a;

        return this;
    }

    /// <summary>
    /// Sets this color's component values through an integer representation.
    /// </summary>
    /// <param name="rgba"></param>
    /// <returns>This color for chaining.</returns>
    public Color Set( uint rgba )
    {
        var color = this;

        RGBA8888ToColor( ref color, rgba );

        Set( color.R, color.G, color.B, color.A );

        return this;
    }

    /// <summary>
    /// Multiplies each of this colors components by the corresponding
    /// components in the supplied Color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns>This Color for chaining.</returns>
    public Color Mul( Color color )
    {
        R *= color.R;
        G *= color.G;
        B *= color.B;
        A *= color.A;

        return Clamp();
    }

    /// <summary>
    /// Multiplies the colour components by the supplied value.
    /// </summary>
    /// <returns>This Color for chaining.</returns>
    public Color Mul( float value )
    {
        R *= value;
        G *= value;
        B *= value;
        A *= value;

        return Clamp();
    }

    /// <summary>
    /// Multiplies each of this colors components by the corresponding
    /// supplied components.
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    /// <returns>This Color for chaining.</returns>
    public Color Mul( float r, float g, float b, float a )
    {
        R *= r;
        G *= g;
        B *= b;
        A *= a;

        return Clamp();
    }

    /// <summary>
    /// Adds the components from the supplied Color to the corresponding
    /// components of this color.
    /// </summary>
    /// <param name="color"> The Color to add. </param>
    /// <returns>This Color for chaining.</returns>
    public Color Add( Color color )
    {
        R += color.R;
        G += color.G;
        B += color.B;
        A += color.A;

        return Clamp();
    }

    /// <summary>
    /// Adds the supplied Color components to the corresponding
    /// components of this Color.
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    public Color Add( float r, float g, float b, float a )
    {
        R += r;
        G += g;
        B += b;
        A += a;

        return Clamp();
    }

    /// <summary>
    /// Subtracts the elements in the supplied Color from the equivalent
    /// elements in this Color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns>This Color for chaining.</returns>
    public Color Sub( Color color )
    {
        R -= color.R;
        G -= color.G;
        B -= color.B;
        A -= color.A;

        return Clamp();
    }

    /// <summary>
    /// Subtracts the supplied elements from the equivalent elements in this Color.
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    /// <returns>This Color for chaining.</returns>
    public Color Sub( float r, float g, float b, float a )
    {
        R -= r;
        G -= g;
        B -= b;
        A -= a;

        return Clamp();
    }

    /// <summary>
    /// Clamps this Colors RGBA components to a valid range [0 - 1]
    /// </summary>
    /// <returns>This Color for chaining.</returns>
    private Color Clamp()
    {
        R = Math.Clamp( R, 0, 1 );
        G = Math.Clamp( G, 0, 1 );
        B = Math.Clamp( B, 0, 1 );
        A = Math.Clamp( A, 0, 1 );

        return this;
    }

    /// <summary>
    /// Clamps the provided Colors RGBA components to a valid range [0 - 1]
    /// </summary>
    private Color Clamp( Color color )
    {
        color.R = Math.Clamp( color.R, 0, 1 );
        color.G = Math.Clamp( color.G, 0, 1 );
        color.B = Math.Clamp( color.B, 0, 1 );
        color.A = Math.Clamp( color.A, 0, 1 );

        return color;
    }

    /// <summary>
    /// Linearly interpolates between this color and the target color by
    /// 'interpolationCoefficient' which is in the range [0,1].
    /// The result is stored in this color.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="interpolationCoefficient"></param>
    /// <returns>This Color for chaining.</returns>
    public Color Lerp( Color target, float interpolationCoefficient )
    {
        ArgumentNullException.ThrowIfNull( target );

        if ( interpolationCoefficient is < 0.0f or > 1.0f )
        {
            throw new ArgumentOutOfRangeException( nameof( interpolationCoefficient ),
                                                   "Interpolation coefficient must be between 0f and 1f." );
        }

        R += interpolationCoefficient * ( target.R - R );
        G += interpolationCoefficient * ( target.G - G );
        B += interpolationCoefficient * ( target.B - B );
        A += interpolationCoefficient * ( target.A - A );

        return Clamp();
    }

    /// <summary>
    /// Linearly interpolates between this color and the target color by
    /// 'interpolationCoefficient' which is in the range [0,1].
    /// The result is stored in this color.
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    /// <param name="interpolationCoefficient"></param>
    /// <returns>This Color for chaining.</returns>
    public Color Lerp( float r, float g, float b, float a, float interpolationCoefficient )
    {
        if ( interpolationCoefficient is < 0.0f or > 1.0f )
        {
            throw new ArgumentOutOfRangeException( nameof( interpolationCoefficient ),
                                                   "Interpolation coefficient must be between 0f and 1f." );
        }

        R += interpolationCoefficient * ( r - R );
        G += interpolationCoefficient * ( g - G );
        B += interpolationCoefficient * ( b - B );
        A += interpolationCoefficient * ( a - A );

        return Clamp();
    }

    /// <summary>
    /// Multiplies the RGB values by the alpha.
    /// </summary>
    /// <returns>This color for chaining.</returns>
    public Color PremultiplyAlpha()
    {
        R *= A;
        G *= A;
        B *= A;

        return Clamp();
    }

    public static bool operator ==( Color? c1, object? c2 )
    {
        if ( c1 is null )
        {
            return c2 is null;
        }

        return c1.Equals( c2 );
    }

    public static bool operator !=( Color? c1, object? c2 )
    {
        return !( c1 == c2 );
    }

    /// <summary>
    /// Converts a 16-bit RGB565 integer value to a Color object.
    /// </summary>
    /// <param name="color">The Color object to assign the converted values to.</param>
    /// <param name="value">The 16-bit RGB565 integer value.</param>
    public static void RGB565ToColor( ref Color color, int value )
    {
        // Ensure the value is within the valid range for 16-bit RGB565
        if ( value is < 0 or > 0xFFFF )
        {
            throw new ArgumentOutOfRangeException( nameof( value ),
                                                   "Value must be a 16-bit integer." );
        }

        color.R = ( ( value & 0xF800 ) >> 11 ) / 31f;
        color.G = ( ( value & 0x07E0 ) >> 5 ) / 63f;
        color.B = ( value & 0x001F ) / 31f;
    }

    /// <summary>
    /// Converts a 16-bit RGBA4444 integer value to a Color object.
    /// </summary>
    /// <param name="color">The Color object to assign the converted values to.</param>
    /// <param name="value">The 16-bit RGBA4444 integer value.</param>
    public static void RGBA4444ToColor( ref Color color, int value )
    {
        // Ensure the value is within the valid range for 16-bit RGBA4444
        if ( value is < 0 or > 0xFFFF )
        {
            throw new ArgumentOutOfRangeException( nameof( value ),
                                                   "Value must be a 16-bit integer." );
        }

        color.R = ( ( value & 0xF000 ) >> 12 ) / 15f;
        color.G = ( ( value & 0x0F00 ) >> 8 ) / 15f;
        color.B = ( ( value & 0x00F0 ) >> 4 ) / 15f;
        color.A = ( value & 0x000F ) / 15f;
    }

    /// <summary>
    /// Converts a 32-bit RGBA8888 integer value to a Color object.
    /// </summary>
    /// <param name="color">The Color object to assign the converted values to.</param>
    /// <param name="value">The 32-bit RGBA8888 integer value.</param>
    public static void RGBA8888ToColor( ref Color color, int value )
    {
        ArgumentNullException.ThrowIfNull( color );

        if ( value is < 0 or > 0xFFFF )
        {
            throw new ArgumentOutOfRangeException( nameof( value ),
                                                   "Value must be a 16-bit integer." );
        }

        color.R = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.G = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.B = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.A = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// Converts a 32-bit ARGB8888 integer value to a Color object.
    /// </summary>
    /// <param name="color">The Color object to assign the converted values to.</param>
    /// <param name="value">The 32-bit ARGB8888 integer value.</param>
    public static void ARGB8888ToColor( ref Color color, int value )
    {
        color.A = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.R = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.G = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.B = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// Sets the Color components using the specified float value in the format ABGR8888.
    /// </summary>
    /// <param name="color">The Color object to assign the converted values to.</param>
    /// <param name="value">The float value representing the color in ABGR8888 format.</param>
    public static void ABGR8888ToColor( ref Color color, float value )
    {
        // Convert the float value to an integer representing the color
        var c = NumberUtils.FloatToIntColor( value );

        // Extract and assign color components using bitwise operations
        color.A = ( ( c & 0xff000000 ) >>> 24 ) / 255f;
        color.B = ( ( c & 0x00ff0000 ) >>> 16 ) / 255f;
        color.G = ( ( c & 0x0000ff00 ) >>> 8 ) / 255f;
        color.R = ( c & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// Sets the Color components using the specified integer value
    /// in the format RGBA8888. This is inverse to the
    /// RGBA8888(r, g, b, a) method.
    /// </summary>
    /// <param name="color">The Color to be modified.</param>
    /// <param name="value">An integer color value in RGBA8888 format.</param>
    private void RGBA8888ToColor( ref Color color, uint value )
    {
        color.R = ( ( value & 0xff000000 ) >> 24 ) / 255f;
        color.G = ( ( value & 0x00ff0000 ) >> 16 ) / 255f;
        color.B = ( ( value & 0x0000ff00 ) >> 8 ) / 255f;
        color.A = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public int RGBA8888ToInt( float r, float g, float b, float a )
    {
        return ( ( int ) ( r * 255 ) << 24 )
             | ( ( int ) ( g * 255 ) << 16 )
             | ( ( int ) ( b * 255 ) << 8 )
             | ( int ) ( a * 255 );
    }

    /// <summary>
    /// Sets the RGB Color components using the specified Hue-Saturation-Value.
    /// Note that HSV components are voluntary not clamped to preserve high range
    /// color and can range beyond typical values.
    /// </summary>
    /// <param name="h">The Hue in degree from 0 to 360</param>
    /// <param name="s">The Saturation from 0 to 1</param>
    /// <param name="v">The Value (brightness) from 0 to 1</param>
    /// <returns>The modified Color for chaining.</returns>
    public Color FromHsv( float h, float s, float v )
    {
        h %= 360; // Ensure hue is in the range [0, 360]
        if ( h < 0 ) h += 360;

        var i = ( int ) ( h / 60 ) % 6;
        var f = ( h / 60 ) - i;
        var p = v * ( 1 - s );
        var q = v * ( 1 - ( s * f ) );
        var t = v * ( 1 - ( s * ( 1 - f ) ) );

        switch ( i )
        {
            case 0:
                R = v;
                G = t;
                B = p;

                break;

            case 1:
                R = q;
                G = v;
                B = p;

                break;

            case 2:
                R = p;
                G = v;
                B = t;

                break;

            case 3:
                R = p;
                G = q;
                B = v;

                break;

            case 4:
                R = t;
                G = p;
                B = v;

                break;

            default:
                R = v;
                G = p;
                B = q;

                break;
        }

        return Clamp();
    }

    /// <summary>
    /// Sets RGB components using the specified Hue-Saturation-Value.
    /// This is a convenient method for fromHsv(float, float, float).
    /// This is the inverse of toHsv(float[]).
    /// </summary>
    /// <param name="hsv"></param>
    /// <returns></returns>
    public Color FromHsv( float[] hsv )
    {
        return FromHsv( hsv[ 0 ], hsv[ 1 ], hsv[ 2 ] );
    }

    /// <summary>
    /// Converts the RGB color values to HSV and stores the result in the provided array.
    /// </summary>
    /// <param name="hsv">An array of at least 3 elements where the HSV values will be stored.</param>
    /// <returns>The array with HSV values.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the provided array does not have at least 3 elements.
    /// </exception>
    public float[] ToHsv( float[] hsv )
    {
        if ( hsv.Length < 3 )
        {
            throw new ArgumentException( "The hsv array must have at least 3 elements.", nameof( hsv ) );
        }

        var max   = Math.Max( Math.Max( R, G ), B );
        var min   = Math.Min( Math.Min( R, G ), B );
        var range = max - min;

        // Hue calculation
        if ( Math.Abs( range ) < MathUtils.FLOAT_TOLERANCE )
        {
            hsv[ 0 ] = 0; // Undefined hue, achromatic case
        }
        else if ( Math.Abs( max - R ) < MathUtils.FLOAT_TOLERANCE )
        {
            hsv[ 0 ] = ( ( ( 60 * ( G - B ) ) / range ) + 360 ) % 360;
        }
        else if ( Math.Abs( max - G ) < MathUtils.FLOAT_TOLERANCE )
        {
            hsv[ 0 ] = ( ( ( 60 * ( B - R ) ) / range ) + 120 ) % 360;
        }
        else // max == B
        {
            hsv[ 0 ] = ( ( ( 60 * ( R - G ) ) / range ) + 240 ) % 360;
        }

        // Saturation calculation
        hsv[ 1 ] = max > 0 ? range / max : 0;

        // Value calculation
        hsv[ 2 ] = max;

        return hsv;
    }

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR and
    /// then converts it to a float. Alpha is compressed from 0-255 to use only even
    /// numbers between 0-254 to avoid using float bits in the NaN range
    /// (see NumberUtils.intToFloatColor(int)).
    /// Converting a color to a float and back can be lossy for alpha.
    /// </summary>
    /// <returns></returns>
    public float ToFloatBits()
    {
        var color = ( ( int ) ( 255 * A ) << 24 )
                  | ( ( int ) ( 255 * B ) << 16 )
                  | ( ( int ) ( 255 * G ) << 8 )
                  | ( int ) ( 255 * R );

        return NumberUtils.IntToFloatColor( color );
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static float ToFloatBits( int r, int g, int b, int a )
    {
        var color      = ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r;
        var floatColor = NumberUtils.IntToFloatColor( color );

        return floatColor;
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static float ToFloatBits( float r, float g, float b, float a )
    {
        var color = ( ( int ) ( 255 * a ) << 24 )
                  | ( ( int ) ( 255 * b ) << 16 )
                  | ( ( int ) ( 255 * g ) << 8 )
                  | ( int ) ( 255 * r );

        return NumberUtils.IntToFloatColor( color );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int ToIntBits()
    {
        return ( ( int ) ( 255 * A ) << 24 )
             | ( ( int ) ( 255 * B ) << 16 )
             | ( ( int ) ( 255 * G ) << 8 )
             | ( int ) ( 255 * R );
    }

    /// <summary>
    /// </summary>
    /// <param name="r"></param>
    /// <param name="g"></param>
    /// <param name="b"></param>
    /// <param name="a"></param>
    /// <returns></returns>
    public static int ToIntBits( int r, int g, int b, int a )
    {
        return ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r;
    }

    /// <summary>
    /// Returns a new color from a hex string with the format RRGGBBAA.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color ValueOf( string hex )
    {
        var color = new Color();

        return ValueOf( hex, ref color );
    }

    /// <summary>
    /// Parses a hex color string and assigns the color values to the provided Color object.
    /// </summary>
    /// <param name="hex">The hex color string, which can optionally start with '#'.</param>
    /// <param name="color">The Color object to assign the parsed values to.</param>
    /// <returns>The Color object with the parsed color values.</returns>
    /// <exception cref="ArgumentException">Thrown if the hex string is not valid.</exception>
    public static Color ValueOf( string hex, ref Color color )
    {
        if ( string.IsNullOrEmpty( hex ) )
        {
            throw new ArgumentException( "Hex string cannot be null or empty.", nameof( hex ) );
        }

        // Remove the leading '#' if present
        if ( hex[ 0 ] == '#' )
        {
            hex = hex.Substring( 1 );
        }

        if ( ( hex.Length != 6 ) && ( hex.Length != 8 ) )
        {
            throw new ArgumentException( "Hex string must be 6 or 8 characters long.", nameof( hex ) );
        }

        try
        {
            color.R = ParseHexComponent( hex.Substring( 0, 2 ) );
            color.G = ParseHexComponent( hex.Substring( 2, 2 ) );
            color.B = ParseHexComponent( hex.Substring( 4, 2 ) );
            color.A = hex.Length == 8 ? ParseHexComponent( hex.Substring( 6, 2 ) ) : 1f;
        }
        catch ( Exception ex ) when ( ex is FormatException or OverflowException )
        {
            throw new ArgumentException( "Hex string contains invalid characters or values.", nameof( hex ), ex );
        }

        return color;
    }

    /// <summary>
    /// Parses a hex component (2 characters) to a float value between 0 and 1.
    /// </summary>
    /// <param name="hexComponent">The hex component to parse.</param>
    /// <returns>The parsed float value.</returns>
    /// <exception cref="FormatException">Thrown if the hex component is not a valid hex number.</exception>
    /// <exception cref="OverflowException">Thrown if the hex component value is too large to fit in an Int32.</exception>
    private static float ParseHexComponent( string hexComponent )
    {
        return Convert.ToInt32( hexComponent, 16 ) / 255f;
    }

    public static int Alpha( float alpha )
    {
        return ( int ) ( alpha * 255.0f );
    }

    public static int LuminanceAlpha( float luminance, float alpha )
    {
        return ( ( int ) ( luminance * 255.0f ) << 8 ) | ( int ) ( alpha * 255 );
    }

    public static int RGB565( float r, float g, float b )
    {
        return ( ( int ) ( r * 31 ) << 11 ) | ( ( int ) ( g * 63 ) << 5 ) | ( int ) ( b * 31 );
    }

    public static int RGBA4444( float r, float g, float b, float a )
    {
        return ( ( int ) ( r * 15 ) << 12 )
             | ( ( int ) ( g * 15 ) << 8 )
             | ( ( int ) ( b * 15 ) << 4 )
             | ( int ) ( a * 15 );
    }

    public static int RGB888( float r, float g, float b )
    {
        return ( ( int ) ( r * 255 ) << 16 ) | ( ( int ) ( g * 255 ) << 8 ) | ( int ) ( b * 255 );
    }

    public static int RGBA8888( float r, float g, float b, float a )
    {
        return ( ( int ) ( r * 255 ) << 24 )
             | ( ( int ) ( g * 255 ) << 16 )
             | ( ( int ) ( b * 255 ) << 8 )
             | ( int ) ( a * 255 );
    }

    public static int ARGB8888( float a, float r, float g, float b )
    {
        return ( ( int ) ( a * 255 ) << 24 )
             | ( ( int ) ( r * 255 ) << 16 )
             | ( ( int ) ( g * 255 ) << 8 )
             | ( int ) ( b * 255 );
    }

    public static int RGB565( Color color )
    {
        ArgumentNullException.ThrowIfNull( color );

        return ( ( int ) ( color.R * 31 ) << 11 )
             | ( ( int ) ( color.G * 63 ) << 5 )
             | ( int ) ( color.B * 31 );
    }

    public static int RGBA4444( Color color )
    {
        ArgumentNullException.ThrowIfNull( color );

        return ( ( int ) ( color.R * 15 ) << 12 )
             | ( ( int ) ( color.G * 15 ) << 8 )
             | ( ( int ) ( color.B * 15 ) << 4 )
             | ( int ) ( color.A * 15 );
    }

    public static int RGB888( Color color )
    {
        ArgumentNullException.ThrowIfNull( color );

        return ( ( int ) ( color.R * 255 ) << 16 )
             | ( ( int ) ( color.G * 255 ) << 8 )
             | ( int ) ( color.B * 255 );
    }

    public static int RGBA8888( Color color )
    {
        ArgumentNullException.ThrowIfNull( color );

        return ( ( int ) ( color.R * 255 ) << 24 )
             | ( ( int ) ( color.G * 255 ) << 16 )
             | ( ( int ) ( color.B * 255 ) << 8 )
             | ( int ) ( color.A * 255 );
    }

    public static int ARGB8888( Color color )
    {
        ArgumentNullException.ThrowIfNull( color );

        return ( ( int ) ( color.A * 255 ) << 24 )
             | ( ( int ) ( color.R * 255 ) << 16 )
             | ( ( int ) ( color.G * 255 ) << 8 )
             | ( int ) ( color.B * 255 );
    }

    /// <summary>
    /// Creates a copy of this <see cref="Color"/> object.
    /// </summary>
    /// <returns>
    /// A new <see cref="Color"/> object that is a copy of the current instance.
    /// </returns>
    public Color Copy()
    {
        ArgumentNullException.ThrowIfNull( this );

        return new Color( this );
    }

    /// <inheritdoc/>
    public override bool Equals( object? obj )
    {
        if ( ( obj == null ) || ( GetType() != obj.GetType() ) )
        {
            return false;
        }

        if ( this == obj )
        {
            return true;
        }

        var color = ( Color ) obj;

        return ToIntBits() == color.ToIntBits();
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        var result = White != 0F ? NumberUtils.FloatToIntBits( White.ToFloatBits() ) : 0;

        result = ( 31 * result ) + ( LightGray != 0F ? NumberUtils.FloatToIntBits( LightGray.ToFloatBits() ) : 0 );
        result = ( 31 * result ) + ( Gray != 0F ? NumberUtils.FloatToIntBits( Gray.ToFloatBits() ) : 0 );
        result = ( 31 * result ) + ( DarkGray != 0F ? NumberUtils.FloatToIntBits( DarkGray.ToFloatBits() ) : 0 );

        return result;
    }

    #region Colour Components

    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }

    #endregion Colour Components
}
