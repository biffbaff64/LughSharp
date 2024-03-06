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


using LibGDXSharp.LibCore.Maths;

namespace LibGDXSharp.LibCore.Graphics;

/// <summary>
///     A color class, holding the r, g, b and alpha component as floats in
///     the range [0,1].
///     All methods perform clamping on the internal values after execution.
/// </summary>
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

    /// <summary>
    ///     Convenience for frequently used <tt>White.ToFloatBits()</tt>
    /// </summary>
    public readonly static float WhiteFloatBits = White.ToFloatBits();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Constructor, sets all the components to 0.
    /// </summary>
    public Color() : this( 0, 0, 0, 0 )
    {
    }

    /// <summary>
    ///     Constructor, sets the Color components using the specified integer value in
    ///     the format RGBA8888. This is inverse to the rgba8888(r, g, b, a) method.
    /// </summary>
    /// <param name="rgba8888"> An integer color value in RGBA8888 format. </param>
    public Color( int rgba8888 ) : this( ( uint )rgba8888 )
    {
    }

    /// <summary>
    ///     Constructor, sets the Color components using the specified integer value in
    ///     the format RGBA8888. This is inverse to the rgba8888(r, g, b, a) method.
    /// </summary>
    /// <param name="rgba8888"> An uint color value in RGBA8888 format. </param>
    public Color( uint rgba8888 )
    {
        Set( rgba8888 );
    }

    /// <summary>
    ///     Constructor, sets the components of the color.
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
    ///     Constructs a new color using the components from the supplied color.
    /// </summary>
    public Color( Color color )
    {
        Set( color );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public float R { get; set; }
    public float G { get; set; }
    public float B { get; set; }
    public float A { get; set; }

    /// <summary>
    ///     Sets this colors components using the components from the supplied colot.
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    public Color Set( Color color )
    {
        Set( color.R, color.G, color.B, color.A );

        return this;
    }

    /// <summary>
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
    ///     Sets this color's component values through an integer representation.
    /// </summary>
    /// <param name="rgba"></param>
    /// <returns>This color for chaining.</returns>
    public Color Set( uint rgba )
    {
        Color color = this;

        Rgba8888ToColor( ref color, rgba );

        Set( color.R, color.G, color.B, color.A );

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public Color Mul( Color color )
    {
        R *= color.R;
        G *= color.G;
        B *= color.B;
        A *= color.A;

        return Clamp();
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public Color Mul( float value )
    {
        R *= value;
        G *= value;
        B *= value;
        A *= value;

        return Clamp();
    }

    /// <summary>
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
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public Color Add( Color color )
    {
        R += color.R;
        G += color.G;
        B += color.B;
        A += color.A;

        return Clamp();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Color Add( float r, float g, float b, float a )
    {
        R += r;
        G += g;
        B += b;
        A += a;

        return Clamp();
    }

    /// <summary>
    ///     Subtracts the elements in the supplied Color from the equivalent
    ///     elements in this Color.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public Color Sub( Color color )
    {
        R -= color.R;
        G -= color.G;
        B -= color.B;
        A -= color.A;

        return Clamp();
    }

    /// <summary>
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
    ///     Clamps this Color's components to a valid range [0 - 1]
    /// </summary>
    /// <returns>This Color for chaining.</returns>
    private Color Clamp()
    {
        R = R switch
            {
                < 0 => 0, // If R < 0 R = 0
                > 1 => 1, // If R > 1 R = 1
                _   => R  // ( set R )
            };

        G = G switch
            {
                < 0 => 0,
                > 1 => 1,
                _   => G
            };

        B = B switch
            {
                < 0 => 0,
                > 1 => 1,
                _   => B
            };

        A = A switch
            {
                < 0 => 0,
                > 1 => 1,
                _   => A
            };

        return this;
    }

    /// <summary>
    ///     Linearly interpolates between this color and the target color by
    ///     'interpolationCoefficient' which is in the range [0,1].
    ///     The result is stored in this color.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="interpolationCoefficient"></param>
    /// <returns></returns>
    public Color Lerp( Color target, float interpolationCoefficient )
    {
        R += interpolationCoefficient * ( target.R - R );
        G += interpolationCoefficient * ( target.G - G );
        B += interpolationCoefficient * ( target.B - B );
        A += interpolationCoefficient * ( target.A - A );

        return Clamp();
    }

    /// <summary>
    ///     Linearly interpolates between this color and the target color by
    ///     'interpolationCoefficient' which is in the range [0,1].
    ///     The result is stored in this color.
    /// </summary>
    /// <param name="r">Red component</param>
    /// <param name="g">Green component</param>
    /// <param name="b">Blue component</param>
    /// <param name="a">Alpha component</param>
    /// <param name="interpolationCoefficient"></param>
    /// <returns></returns>
    public Color Lerp( float r, float g, float b, float a, float interpolationCoefficient )
    {
        R += interpolationCoefficient * ( r - R );
        G += interpolationCoefficient * ( g - G );
        B += interpolationCoefficient * ( b - B );
        A += interpolationCoefficient * ( a - A );

        return Clamp();
    }

    /// <summary>
    ///     Multiplies the RGB values by the alpha.
    /// </summary>
    /// <returns>This color for chaining.</returns>
    public Color PremultiplyAlpha()
    {
        R *= A;
        G *= A;
        B *= A;

        return this;
    }

    public static bool operator ==( Color? c1, object? c2 )
    {
        if ( c1 is null )
        {
            return false;
        }

        return c1.Equals( c2 );
    }

    public static bool operator !=( Color? c1, object? c2 )
    {
        if ( c1 is null )
        {
            return true;
        }

        return !c1.Equals( c2 );
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="value"></param>
    public static void Rgb565ToColor( ref Color color, int value )
    {
        color.R = ( ( value & 0x0000F800 ) >>> 11 ) / 31f;
        color.G = ( ( value & 0x000007E0 ) >>> 5 ) / 63f;
        color.B = ( ( value & 0x0000001F ) >>> 0 ) / 31f;
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="value"></param>
    public static void Rgba4444ToColor( ref Color color, int value )
    {
        color.R = ( ( value & 0x0000f000 ) >>> 12 ) / 15f;
        color.G = ( ( value & 0x00000f00 ) >>> 8 ) / 15f;
        color.B = ( ( value & 0x000000f0 ) >>> 4 ) / 15f;
        color.A = ( value & 0x0000000f ) / 15f;
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="value"></param>
    public static void Rgba8888ToColor( ref Color color, int value )
    {
        color.R = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.G = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.B = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.A = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="value"></param>
    public static void Argb8888ToColor( ref Color color, int value )
    {
        color.A = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.R = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.G = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.B = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// </summary>
    /// <param name="color"></param>
    /// <param name="value"></param>
    public static void Abgr8888ToColor( ref Color color, float value )
    {
        var c = NumberUtils.FloatToIntColor( value );

        color.A = ( ( c & 0xff000000 ) >>> 24 ) / 255f;
        color.B = ( ( c & 0x00ff0000 ) >>> 16 ) / 255f;
        color.G = ( ( c & 0x0000ff00 ) >>> 8 ) / 255f;
        color.R = ( c & 0x000000ff ) / 255f;
    }

    /// <summary>
    ///     Sets the Color components using the specified integer value
    ///     in the format RGBA8888. This is inverse to the
    ///     RGBA8888(r, g, b, a) method.
    /// </summary>
    /// <param name="color">The Color to be modified.</param>
    /// <param name="value">An integer color value in RGBA8888 format.</param>
    private void Rgba8888ToColor( ref Color color, uint value )
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
    public int Rgba8888( float r, float g, float b, float a )
    {
        return ( ( int )( r * 255 ) << 24 ) | ( ( int )( g * 255 ) << 16 ) | ( ( int )( b * 255 ) << 8 ) | ( int )( a * 255 );
    }

    /// <summary>
    ///     Sets the RGB Color components using the specified Hue-Saturation-Value.
    ///     Note that HSV components are voluntary not clamped to preserve high range
    ///     color and can range beyond typical values.
    /// </summary>
    /// <param name="h">The Hue in degree from 0 to 360</param>
    /// <param name="s">The Saturation from 0 to 1</param>
    /// <param name="v">The Value (brightness) from 0 to 1</param>
    /// <returns>The modified Color for chaining.</returns>
    public Color FromHsv( float h, float s, float v )
    {
        var x = ( ( h / 60f ) + 6 ) % 6;
        var i = ( int )x;
        var f = x - i;
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
    ///     Sets RGB components using the specified Hue-Saturation-Value.
    ///     This is a convenient method for fromHsv(float, float, float).
    ///     This is the inverse of toHsv(float[]).
    /// </summary>
    /// <param name="hsv"></param>
    /// <returns></returns>
    public Color FromHsv( float[] hsv )
    {
        return FromHsv( hsv[ 0 ], hsv[ 1 ], hsv[ 2 ] );
    }

    public float[] ToHsv( float[] hsv )
    {
        var max   = Math.Max( Math.Max( R, G ), B );
        var min   = Math.Min( Math.Min( R, G ), B );
        var range = max - min;

        if ( range == 0 )
        {
            hsv[ 0 ] = 0;
        }
        else if ( Math.Abs( max - R ) < 0.00001f )
        {
            hsv[ 0 ] = ( ( ( 60 * ( G - B ) ) / range ) + 360 ) % 360;
        }
        else if ( Math.Abs( max - G ) < 0.00001f )
        {
            hsv[ 0 ] = ( ( 60 * ( B - R ) ) / range ) + 120;
        }
        else
        {
            hsv[ 0 ] = ( ( 60 * ( R - G ) ) / range ) + 240;
        }

        if ( max > 0 )
        {
            hsv[ 1 ] = 1 - ( min / max );
        }
        else
        {
            hsv[ 1 ] = 0;
        }

        hsv[ 2 ] = max;

        return hsv;
    }

    /// <summary>
    ///     Packs the color components into a 32-bit integer with the format ABGR and
    ///     then converts it to a float. Alpha is compressed from 0-255 to use only even
    ///     numbers between 0-254 to avoid using float bits in the NaN range
    ///     (see NumberUtils.intToFloatColor(int)).
    ///     Converting a color to a float and back can be lossy for alpha.
    /// </summary>
    /// <returns></returns>
    public float ToFloatBits()
    {
        var color = ( ( int )( 255 * A ) << 24 )
                  | ( ( int )( 255 * B ) << 16 )
                  | ( ( int )( 255 * G ) << 8 )
                  | ( int )( 255 * R );

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
        var color = ( ( int )( 255 * a ) << 24 )
                  | ( ( int )( 255 * b ) << 16 )
                  | ( ( int )( 255 * g ) << 8 )
                  | ( int )( 255 * r );

        return NumberUtils.IntToFloatColor( color );
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int ToIntBits()
    {
        return ( ( int )( 255 * A ) << 24 )
             | ( ( int )( 255 * B ) << 16 )
             | ( ( int )( 255 * G ) << 8 )
             | ( int )( 255 * R );
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
    ///     Returns a new color from a hex string with the format RRGGBBAA.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color ValueOf( string hex )
    {
        return ValueOf( hex, new Color() );
    }

    /// <summary>
    /// </summary>
    /// <param name="hex"></param>
    /// <param name="color"></param>
    /// <returns></returns>
    public static Color ValueOf( string hex, Color color )
    {
        hex = hex[ 0 ] == '#' ? hex.Substring( 1 ) : hex;

        color.R = int.Parse( hex.Substring( 0, 2 ) ) / 255f;
        color.G = int.Parse( hex.Substring( 2, 4 ) ) / 255f;
        color.B = int.Parse( hex.Substring( 4, 6 ) ) / 255f;
        color.A = hex.Length != 8 ? 1 : int.Parse( hex.Substring( 6, 8 ) ) / 255f;

        return color;
    }

    public static int Alpha( float alpha )
    {
        return ( int )( alpha * 255.0f );
    }

    public static int LuminanceAlpha( float luminance, float alpha )
    {
        return ( ( int )( luminance * 255.0f ) << 8 ) | ( int )( alpha * 255 );
    }

    public static int RGB565( float r, float g, float b )
    {
        return ( ( int )( r * 31 ) << 11 ) | ( ( int )( g * 63 ) << 5 ) | ( int )( b * 31 );
    }

    public static int RGBA4444( float r, float g, float b, float a )
    {
        return ( ( int )( r * 15 ) << 12 )
             | ( ( int )( g * 15 ) << 8 )
             | ( ( int )( b * 15 ) << 4 )
             | ( int )( a * 15 );
    }

    public static int RGB888( float r, float g, float b )
    {
        return ( ( int )( r * 255 ) << 16 ) | ( ( int )( g * 255 ) << 8 ) | ( int )( b * 255 );
    }

    public static int RGBA8888( float r, float g, float b, float a )
    {
        return ( ( int )( r * 255 ) << 24 )
             | ( ( int )( g * 255 ) << 16 )
             | ( ( int )( b * 255 ) << 8 )
             | ( int )( a * 255 );
    }

    public static int ARGB8888( float a, float r, float g, float b )
    {
        return ( ( int )( a * 255 ) << 24 )
             | ( ( int )( r * 255 ) << 16 )
             | ( ( int )( g * 255 ) << 8 )
             | ( int )( b * 255 );
    }

    public static int RGB565( Color color )
    {
        return ( ( int )( color.R * 31 ) << 11 )
             | ( ( int )( color.G * 63 ) << 5 )
             | ( int )( color.B * 31 );
    }

    public static int RGBA4444( Color color )
    {
        return ( ( int )( color.R * 15 ) << 12 )
             | ( ( int )( color.G * 15 ) << 8 )
             | ( ( int )( color.B * 15 ) << 4 )
             | ( int )( color.A * 15 );
    }

    public static int RGB888( Color color )
    {
        return ( ( int )( color.R * 255 ) << 16 )
             | ( ( int )( color.G * 255 ) << 8 )
             | ( int )( color.B * 255 );
    }

    public static int RGBA8888( Color color )
    {
        return ( ( int )( color.R * 255 ) << 24 )
             | ( ( int )( color.G * 255 ) << 16 )
             | ( ( int )( color.B * 255 ) << 8 )
             | ( int )( color.A * 255 );
    }

    public static int ARGB8888( Color color )
    {
        return ( ( int )( color.A * 255 ) << 24 )
             | ( ( int )( color.R * 255 ) << 16 )
             | ( ( int )( color.G * 255 ) << 8 )
             | ( int )( color.B * 255 );
    }

    /// <summary>
    ///     Creates a copy of this Color object.
    /// </summary>
    /// <returns></returns>
    public Color Copy()
    {
        return new Color( this );
    }

    /// <inheritdoc />
    public override bool Equals( object? o )
    {
        if ( ( o == null ) || ( GetType() != o.GetType() ) )
        {
            return false;
        }

        if ( this == o )
        {
            return true;
        }

        var color = ( Color )o;

        return ToIntBits() == color.ToIntBits();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var result = White != 0F ? NumberUtils.FloatToIntBits( White.ToFloatBits() ) : 0;

        result = ( 31 * result ) + ( LightGray != 0F ? NumberUtils.FloatToIntBits( LightGray.ToFloatBits() ) : 0 );
        result = ( 31 * result ) + ( Gray != 0F ? NumberUtils.FloatToIntBits( Gray.ToFloatBits() ) : 0 );
        result = ( 31 * result ) + ( DarkGray != 0F ? NumberUtils.FloatToIntBits( DarkGray.ToFloatBits() ) : 0 );

        return result;
    }
}
