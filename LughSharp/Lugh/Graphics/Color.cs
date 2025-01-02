// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Utils;

namespace LughSharp.Lugh.Graphics;

/// <summary>
/// A color class, holding the r, g, b and alpha component as floats in the range [0,1].
/// All methods perform clamping on the internal values after execution.
/// </summary>
[PublicAPI, DebuggerDisplay( "RGBADebugString" )]
public sealed class Color : ICloneable, IEquatable< Color >
{
    #region colour values

    public static readonly Color Red        = new( 0xff0000ff );
    public static readonly Color Green      = new( 0x00ff00ff );
    public static readonly Color Blue       = new( 0x0000ff00 );
    public static readonly Color Clear      = new( 0x00000000 );
    public static readonly Color White      = new( 0xffffffff );
    public static readonly Color Black      = new( 0x000000ff );
    public static readonly Color Gray       = new( 0x7f7f7fff );
    public static readonly Color LightGray  = new( 0xbfbfbfff );
    public static readonly Color DarkGray   = new( 0x3f3f3fff );
    public static readonly Color Slate      = new( 0x708090ff );
    public static readonly Color Navy       = new( 0x000080ff );
    public static readonly Color Royal      = new( 0x4169e1ff );
    public static readonly Color Sky        = new( 0x87ceebff );
    public static readonly Color Cyan       = new( 0x00ffffff );
    public static readonly Color Teal       = new( 0x007f7fff );
    public static readonly Color Chartreuse = new( 0x7fff00ff );
    public static readonly Color Lime       = new( 0x32cd32ff );
    public static readonly Color Forest     = new( 0x228b22ff );
    public static readonly Color Olive      = new( 0x6b8e23ff );
    public static readonly Color Yellow     = new( 0xffff00ff );
    public static readonly Color Gold       = new( 0xffd700ff );
    public static readonly Color Goldenrod  = new( 0xdaa520ff );
    public static readonly Color Orange     = new( 0xffa500ff );
    public static readonly Color Brown      = new( 0x8b4513ff );
    public static readonly Color Tan        = new( 0xd2b48cff );
    public static readonly Color Firebrick  = new( 0xb22222ff );
    public static readonly Color Scarlet    = new( 0xff341cff );
    public static readonly Color Coral      = new( 0xff7f50ff );
    public static readonly Color Salmon     = new( 0xfa8072ff );
    public static readonly Color Pink       = new( 0xff69b4ff );
    public static readonly Color Magenta    = new( 0xff00ffff );
    public static readonly Color Purple     = new( 0xa020f0ff );
    public static readonly Color Violet     = new( 0xee82eeff );
    public static readonly Color Maroon     = new( 0xb03060ff );

    /// <summary>
    /// Convenience for frequently used <tt>White.ToFloatBits()</tt>
    /// </summary>
    public static float WhiteFloatBits => White.ToFloatBitsABGR();

    #endregion colour values

    // ========================================================================
    // ========================================================================

    #region Colour Components

    /// <summary>
    /// Red Color Component
    /// </summary>
    public float R { get; set; }

    /// <summary>
    /// Green Color Component
    /// </summary>
    public float G { get; set; }

    /// <summary>
    /// Blue Color Component
    /// </summary>
    public float B { get; set; }

    /// <summary>
    /// Alpha Color Component
    /// </summary>
    public float A { get; set; }

    /// <summary>
    /// Color Components packed into a <b>uint</b>, stored in RGBA format.
    /// </summary>
    public uint RGBAPackedColor { get; private set; }

    /// <summary>
    /// Color Components packed into a <b>uint</b>, stored in ABGR format.
    /// </summary>
    public uint ABGRPackedColor { get; private set; }

    /// <summary>
    /// Color Components packed into a <b>float</b>, stored in RGBA format.
    /// </summary>
    public double RGBAFloatPack { get; private set; }

    /// <summary>
    /// Color Components packed into a <b>float</b>, stored in ABGR format.
    /// </summary>
    public double ABGRFloatPack { get; private set; }

    #endregion Colour Components

    // ========================================================================
    // ========================================================================

    private static Color _color = new();

    // ========================================================================
    // ========================================================================

    #region constructors

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
    /// <param name="rgba8888"> An uint color value in RGBA8888 format. </param>
    public Color( uint rgba8888 )
    {
        this.R = ( float )( ( rgba8888 & 0xff000000 ) >> 24 ) / 255.0f;
        this.G = ( float )( ( rgba8888 & 0x00ff0000 ) >> 16 ) / 255.0f;
        this.B = ( float )( ( rgba8888 & 0x0000ff00 ) >> 8 ) / 255.0f;
        this.A = ( float )( rgba8888 & 0x000000ff ) / 255.0f;

        Clamp();
    }

    /// <summary>
    /// Constructor, sets the components of the color.
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    public Color( float r, float g, float b, float a )
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;

        Clamp();
    }

    /// <summary>
    /// Constructs a new color using the components from the supplied color.
    /// </summary>
    public Color( Color color )
        : this( color.R, color.G, color.B, color.A )
    {
    }

    #endregion constructors

    // ========================================================================
    // ========================================================================

    #region general methods

    /// <summary>
    /// Sets this colors components using the components from the supplied color.
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    public Color Set( Color color )
    {
        this.R = color.R;
        this.G = color.G;
        this.B = color.B;
        this.A = color.A;

        return Clamp();
    }

    /// <summary>
    /// Sets this color's component values through an integer representation.
    /// </summary>
    /// <param name="rgba"> The integer representation. </param>
    /// <returns> This color for chaining. </returns>
    public Color Set( uint rgba )
    {
        var color = this;

        RGBA8888ToColor( ref color, rgba );

        this.R = color.R;
        this.G = color.G;
        this.B = color.B;
        this.A = color.A;

        return Clamp();
    }

    /// <summary>
    /// Sets this colors components using the supplied r,g,b,a components.
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns> This Color for chaining. </returns>
    public Color Set( float r, float g, float b, float a )
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.A = a;

        return Clamp();
    }

    /// <summary>
    /// Multiplies each of this color objects components by the corresponding components
    /// in the supplied Color.
    /// </summary>
    /// <param name="color"> The supplied color. </param>
    /// <returns> This Color for chaining. </returns>
    public Color Mul( Color color )
    {
        this.R *= color.R;
        this.G *= color.G;
        this.B *= color.B;
        this.A *= color.A;

        return Clamp();
    }

    /// <summary>
    /// Multiplies the colour components by the supplied value.
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    public Color Mul( float value )
    {
        this.R *= value;
        this.G *= value;
        this.B *= value;
        this.A *= value;

        return Clamp();
    }

    /// <summary>
    /// Multiplies each of this color objects components by the corresponding supplied components.
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns>This Color for chaining.</returns>
    public Color Mul( float r, float g, float b, float a )
    {
        this.R *= r;
        this.G *= g;
        this.B *= b;
        this.A *= a;

        return Clamp();
    }

    /// <summary>
    /// Adds the components from the supplied Color to the corresponding
    /// components of this color.
    /// </summary>
    /// <param name="color"> The Color to add. </param>
    /// <returns> This Color for chaining. </returns>
    public Color Add( Color color )
    {
        this.R += color.R;
        this.G += color.G;
        this.B += color.B;
        this.A += color.A;

        return Clamp();
    }

    /// <summary>
    /// Adds the supplied Color components to the corresponding components of this Color.
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns> This Color for chaining. </returns>
    public Color Add( float r, float g, float b, float a )
    {
        this.R += r;
        this.G += g;
        this.B += b;
        this.A += a;

        return Clamp();
    }

    /// <summary>
    /// Subtracts the elements in the supplied Color from the equivalent
    /// elements in this Color.
    /// </summary>
    /// <param name="color"> The color to subtract. </param>
    /// <returns> This Color for chaining. </returns>
    public Color Sub( Color color )
    {
        this.R -= color.R;
        this.G -= color.G;
        this.B -= color.B;
        this.A -= color.A;

        return Clamp();
    }

    /// <summary>
    /// Subtracts the supplied elements from the equivalent elements in this Color.
    /// </summary>
    /// <param name="r"> Red component. </param>
    /// <param name="g"> Green component. </param>
    /// <param name="b"> Blue component. </param>
    /// <param name="a"> Alpha component. </param>
    /// <returns> This Color for chaining. </returns>
    public Color Sub( float r, float g, float b, float a )
    {
        this.R -= r;
        this.G -= g;
        this.B -= b;
        this.A -= a;

        return Clamp();
    }

    /// <summary>
    /// Multiplies the RGB values by the alpha.
    /// </summary>
    /// <returns>This color for chaining.</returns>
    public Color PremultiplyAlpha()
    {
        this.R *= A;
        this.G *= A;
        this.B *= A;

        return Clamp();
    }

    /// <summary>
    /// Adds the components of the supplied Color object to the components
    /// of this Color object and returns the result as a new Color object.
    /// </summary>
    public Color AddNew( Color color )
    {
        return new Color( this.R + color.R,
                          this.G + color.G,
                          this.B + color.B,
                          this.A + color.A ).Clamp();
    }

    /// <summary>
    /// Subtracts the components of the supplied Color object from the
    /// components of this Color object and returns the result as a new
    /// Color object.
    /// </summary>
    public Color SubNew( Color color )
    {
        return new Color( this.R - color.R,
                          this.G - color.G,
                          this.B - color.B,
                          this.A - color.A ).Clamp();
    }

    /// <summary>
    /// Multiplies the components of this Color object by the components
    /// of the supplied Color object and returns the result as a new Color
    /// object.
    /// </summary>
    public Color MulNew( Color color )
    {
        return new Color( this.R * color.R,
                          this.G * color.G,
                          this.B * color.B,
                          this.A * color.A ).Clamp();
    }

    /// <summary>
    /// Clamps this Colors RGBA components to a valid range [0 - 1]
    /// </summary>
    /// <returns> This Color for chaining. </returns>
    private Color Clamp()
    {
        R = R < 0f ? 0f : ( R > 1f ? 1f : R );
        G = G < 0f ? 0f : ( G > 1f ? 1f : G );
        B = B < 0f ? 0f : ( B > 1f ? 1f : B );
        A = A < 0f ? 0f : ( A > 1f ? 1f : A );

        RGBAPackedColor = RGBA8888( R, G, B, A );
        ABGRPackedColor = ABGR8888( A, B, G, R );

        RGBAFloatPack = ToFloatBitsABGR( R, G, B, A );
        ABGRFloatPack = ToFloatBitsABGR( A, B, G, R );

        Logger.Debug( $"R: {R}, G: {G}, B: {B}, A: {A}" );
        Logger.Debug( $"RGBAPackedColor: {RGBAPackedColor}, {RGBAPackedColor:X8}" );
        Logger.Debug( $"ABGRPackedColor: {ABGRPackedColor}, {ABGRPackedColor:X8}" );
        Logger.Debug( $"RGBAFloatPack  : {RGBAFloatPack}" );
        Logger.Debug( $"ABGRFloatPack  : {ABGRFloatPack}" );

        return this;
    }

    /// <summary>
    /// Linearly interpolates between this color and the target color by
    /// 'interpolationCoefficient' which is in the range [0,1].
    /// The result is stored in this color.
    /// </summary>
    /// <param name="target"> The target color. </param>
    /// <param name="interpolationCoefficient"> This value must be in the range [0, 1] </param>
    /// <returns>This Color for chaining.</returns>
    public Color Lerp( Color target, float interpolationCoefficient )
    {
        ArgumentNullException.ThrowIfNull( target );

        if ( interpolationCoefficient is < 0.0f or > 1.0f )
        {
            throw new ArgumentOutOfRangeException
                ( nameof( interpolationCoefficient ), "Interpolation coefficient must be between 0f and 1f." );
        }

        this.R += interpolationCoefficient * ( target.R - this.R );
        this.G += interpolationCoefficient * ( target.G - this.G );
        this.B += interpolationCoefficient * ( target.B - this.B );
        this.A += interpolationCoefficient * ( target.A - this.A );

        return Clamp();
    }

    /// <summary>
    /// Linearly interpolates between this color and the target color by
    /// 'interpolationCoefficient' which is in the range [0,1].
    /// The result is stored in this color.
    /// </summary>
    /// <param name="r"> Red component. </param>
    /// <param name="g"> Green component. </param>
    /// <param name="b"> Blue component. </param>
    /// <param name="a"> Alpha component. </param>
    /// <param name="interpolationCoefficient"> This value must be in the range [0, 1] </param>
    /// <returns> This Color for chaining. </returns>
    public Color Lerp( float r, float g, float b, float a, float interpolationCoefficient )
    {
        if ( interpolationCoefficient is < 0.0f or > 1.0f )
        {
            throw new ArgumentOutOfRangeException( nameof( interpolationCoefficient ),
                                                   "Interpolation coefficient must be between 0f and 1f." );
        }

        this.R += interpolationCoefficient * ( r - this.R );
        this.G += interpolationCoefficient * ( g - this.G );
        this.B += interpolationCoefficient * ( b - this.B );
        this.A += interpolationCoefficient * ( a - this.A );

        return Clamp();
    }

    // ========================================================================
    // ========================================================================
    // ========================================================================
    // ========================================================================

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
            hex = hex[ 1.. ];
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
    /// Returns a new color from a hex string with the format <b>RRGGBBAA</b>.
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Color ValueOf( string hex )
    {
        var color = new Color();

        return ValueOf( hex, ref color );
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

    /// <summary>
    /// Returns the given Alpha value as a 32-bit uint.
    /// </summary>
    /// <param name="alpha"> The Alpha value. </param>
    /// <returns> The uint result. </returns>
    public static uint AlphaToInt( float alpha )
    {
        return ( uint )( alpha * 255.0f );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="luminance"></param>
    /// <param name="alpha"></param>
    /// <returns></returns>
    public static uint LuminanceAlpha( float luminance, float alpha )
    {
        return ( ( uint )( luminance * 255.0f ) << 8 ) | ( uint )( alpha * 255 );
    }

    #endregion general methods

    // ========================================================================
    // ========================================================================

    // ========================================================================
    // ========================================================================
    // RGBA and RGB ( Methods only )
    // ========================================================================
    // ========================================================================

    #region RGBA Methods

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR and then
    /// converts it to a float. Alpha is compressed from 0-255 to use only even numbers
    /// between 0-254 to avoid using float bits in the NaN range.
    /// <para>
    /// Note: Converting a color to a float and back can be lossy for alpha.
    /// </para>
    /// </summary>
    /// <returns> The resulting float. </returns>
    /// <seealso cref="NumberUtils.UIntToFloatColor"/>
    public float ToFloatBitsRGBA()
    {
        var r = ( ( uint )( 255f * R ) << 24 );
        var g = ( ( uint )( 255f * G ) << 16 );
        var b = ( ( uint )( 255f * B ) << 8 );
        var a = ( uint )( 255f * A );

        var intBits = ( uint )( r | g | b | a );

        return NumberUtils.UIntToFloatColor( intBits );
    }

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR and then
    /// converts it to a float. Alpha is compressed from 0-255 to use only even numbers
    /// between 0-254 to avoid using float bits in the NaN range.
    /// <para>
    /// Note: Converting a color to a float and back can be lossy for alpha.
    /// </para>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns></returns>
    public static float ToFloatBitsRGBA( float r, float g, float b, float a )
    {
        var rf = ( ( uint )( 255f * r ) << 24 );
        var gf = ( ( uint )( 255f * g ) << 16 );
        var bf = ( ( uint )( 255f * b ) << 8 );
        var af = ( uint )( 255f * a );

        var intBits = ( uint )( rf | gf | bf | af );

        return ( float )intBits;
    }

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format RGBA.
    /// </summary>
    /// <returns> the packed color as a 32-bit int. </returns>
    public uint PackedColorRGBA()
    {
        return ( ( ( uint )( 255f * R ) ) << 24 )
               | ( ( ( uint )( 255f * G ) ) << 16 )
               | ( ( ( uint )( 255f * B ) ) << 8 )
               | ( ( uint )( 255f * A ) );
    }

    /// <summary>
    /// Converts the supplied color components to an <b>uint</b>.
    /// </summary>
    /// <param name="r"> Red component. </param>
    /// <param name="g"> Green component. </param>
    /// <param name="b"> Blue component. </param>
    /// <param name="a"> Alpha component. </param>
    /// <returns></returns>
    public uint RGBA8888ToInt( float r, float g, float b, float a )
    {
        return ( ( ( uint )( 255f * r ) ) << 24 )
               | ( ( ( uint )( 255f * g ) ) << 16 )
               | ( ( ( uint )( 255f * b ) ) << 8 )
               | ( ( uint )( 255f * a ) );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 4  : Blue component</li>
    /// <li>Bits  5 - 10 : Green component</li>
    /// <li>Bits 11 - 15 : Red component</li>
    /// <li>Bits 16 - 31 : Undefined</li>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    public static uint RGB565( float r, float g, float b )
    {
        return ( ( uint )( r * 31 ) << 11 ) | ( ( uint )( g * 63 ) << 5 ) | ( uint )( b * 31 );
    }

    /// <summary>
    /// Returns the given R.G.B colour components as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 3  : Alpha component</li>
    /// <li>Bits  4 - 7  : Blue component</li>
    /// <li>Bits  8 - 11 : Green component</li>
    /// <li>Bits 12 - 15 : Red component</li>
    /// <li>Bits 16 - 31 : Undefined</li>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    public static uint RGBA4444( float r, float g, float b, float a )
    {
        return ( ( uint )( r * 15 ) << 12 )
               | ( ( uint )( g * 15 ) << 8 )
               | ( ( uint )( b * 15 ) << 4 )
               | ( uint )( a * 15 );
    }

    /// <summary>
    /// Returns the given R.G.B colour components as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Blue component</li>
    /// <li>Bits  8 - 15 : Green component</li>
    /// <li>Bits 16 - 23 : Red component</li>
    /// <li>Bits 24 - 31 : Undefined</li>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    public static uint RGB888( float r, float g, float b )
    {
        return ( ( uint )( r * 255 ) << 16 ) | ( ( uint )( g * 255 ) << 8 ) | ( uint )( b * 255 );
    }

    /// <summary>
    /// Returns the given seperate colour components as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Alpha component</li>
    /// <li>Bits  8 - 15 : Blue component</li>
    /// <li>Bits 16 - 23 : Green component</li>
    /// <li>Bits 24 - 31 : Red component</li>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    public static uint RGBA8888( float r, float g, float b, float a )
    {
        return ( ( uint )( r * 255 ) << 24 )
               | ( ( uint )( g * 255 ) << 16 )
               | ( ( uint )( b * 255 ) << 8 )
               | ( uint )( a * 255 );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 4  : Blue component</li>
    /// <li>Bits  5 - 10 : Green component</li>
    /// <li>Bits 11 - 15 : Red component</li>
    /// <li>Bits 16 - 31 : Undefined</li>
    /// </summary>
    /// <param name="color"> The colour. </param>
    public static uint RGB565( Color color )
    {
        return ( ( uint )( color.R * 31 ) << 11 )
               | ( ( uint )( color.G * 63 ) << 5 )
               | ( uint )( color.B * 31 );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 16-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 3  : Alpha component</li>
    /// <li>Bits  4 - 7  : Blue component</li>
    /// <li>Bits  8 - 11 : Green component</li>
    /// <li>Bits 12 - 15 : Red component</li>
    /// <li>Bits 16 - 31 : Undefined</li>
    /// </summary>
    /// <param name="color"> The colour. </param>
    public static uint RGBA4444( Color color )
    {
        return ( ( uint )( color.R * 15 ) << 12 )
               | ( ( uint )( color.G * 15 ) << 8 )
               | ( ( uint )( color.B * 15 ) << 4 )
               | ( uint )( color.A * 15 );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Blue component</li>
    /// <li>Bits  8 - 15 : Green component</li>
    /// <li>Bits 16 - 23 : Red component</li>
    /// <li>Bits 24 - 31 : Undefined</li>
    /// </summary>
    /// <param name="color"> The colour. </param>
    public static uint RGB888( Color color )
    {
        return ( ( uint )( color.R * 255 ) << 16 )
               | ( ( uint )( color.G * 255 ) << 8 )
               | ( uint )( color.B * 255 );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Alpha component</li>
    /// <li>Bits  8 - 15 : Blue component</li>
    /// <li>Bits 16 - 23 : Green component</li>
    /// <li>Bits 24 - 31 : Red component</li>
    /// </summary>
    /// <param name="color"> The colour. </param>
    public static uint RGBA8888( Color color )
    {
        return ( ( uint )( color.R * 255 ) << 24 )
               | ( ( uint )( color.G * 255 ) << 16 )
               | ( ( uint )( color.B * 255 ) << 8 )
               | ( uint )( color.A * 255 );
    }

    /// <summary>
    /// Converts a 32-bit RGBA8888 integer value to a Color object.
    /// </summary>
    /// <param name="color"> The Color object to assign the converted values to. </param>
    /// <param name="value"> The 32-bit RGBA8888 integer value. </param>
    public static void RGBA8888ToColor( ref Color color, uint value )
    {
        color.R = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.G = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.B = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.A = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// Converts a 16-bit RGB565 integer value to a Color object.
    /// </summary>
    /// <param name="color"> The Color object to assign the converted values to. </param>
    /// <param name="value"> The 16-bit RGB565 integer value. </param>
    public static void RGB565ToColor( ref Color color, uint value )
    {
        // Ensure the value is within the valid range for 16-bit RGB565
        if ( value > 0xFFFF )
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
    /// <param name="color"> The Color object to assign the converted values to. </param>
    /// <param name="value"> The 16-bit RGBA4444 integer value. </param>
    public static void RGBA4444ToColor( ref Color color, uint value )
    {
        color.R = ( ( value & 0xF000 ) >> 12 ) / 15f;
        color.G = ( ( value & 0x0F00 ) >> 8 ) / 15f;
        color.B = ( ( value & 0x00F0 ) >> 4 ) / 15f;
        color.A = ( value & 0x000F ) / 15f;
    }

    /// <summary>
    /// Returns a string representation of the Color components RGBA.
    /// </summary>
    public string RGBAToString()
    {
        return $"R:{R},G:{G},B:{B},A:{A}";
    }

    #endregion RGBA Methods

    // ========================================================================
    // ========================================================================
    // ABGR ( Methods only )
    // ========================================================================
    // ========================================================================

    #region ABGR Methods

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR and then
    /// converts it to a float. Alpha is compressed from 0-255 to use only even numbers
    /// between 0-254 to avoid using float bits in the NaN range.
    /// <para>
    /// Note: Converting a color to a float and back can be lossy for alpha.
    /// </para>
    /// </summary>
    /// <returns> The resulting float. </returns>
    /// <seealso cref="NumberUtils.UIntToFloatColor(uint)"/>
    public float ToFloatBitsABGR()
    {
        var a = ( ( uint )( 255f * A ) << 24 );
        var b = ( ( uint )( 255f * B ) << 16 );
        var g = ( ( uint )( 255f * G ) << 8 );
        var r = ( uint )( 255f * R );

        var intBits = ( uint )( a | b | g | r );

        return NumberUtils.UIntToFloatColor( intBits );
    }

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR and then
    /// converts it to a float. Alpha is compressed from 0-255 to use only even numbers
    /// between 0-254 to avoid using float bits in the NaN range.
    /// <para>
    /// Note: Converting a color to a float and back can be lossy for alpha.
    /// </para>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns></returns>
    public static float ToFloatBitsABGR( float a, float b, float g, float r )
    {
        var af = ( ( uint )( a * 255f ) ) << 24;
        var bf = ( ( uint )( b * 255f ) ) << 16;
        var gf = ( ( uint )( g * 255f ) ) << 8;
        var rf = ( uint )( r * 255f );

        var intBits = ( uint )( af | bf | gf | rf );

        return ( float )intBits;
    }

    /// <summary>
    /// Packs the color components into a 32-bit integer with the format ABGR.
    /// </summary>
    /// <returns> the packed color as a 32-bit int. </returns>
    public uint PackedColorABGR()
    {
        return ( ( uint )( 255f * A ) << 24 )
               | ( ( uint )( 255f * B ) << 16 )
               | ( ( uint )( 255f * G ) << 8 )
               | ( uint )( 255f * R );
    }

    /// <summary>
    /// Returns the given seperate colour components as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Alpha component</li>
    /// <li>Bits  8 - 15 : Blue component</li>
    /// <li>Bits 16 - 23 : Green component</li>
    /// <li>Bits 24 - 31 : Red component</li>
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    public static uint ABGR8888( float a, float b, float g, float r )
    {
        return ( ( uint )( a * 255 ) << 24 )
               | ( ( uint )( b * 255 ) << 16 )
               | ( ( uint )( g * 255 ) << 8 )
               | ( uint )( r * 255 );
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
    /// Packs the color components into a 32-bit integer with the format ABGR.
    /// Note that no range checking is performed for higher performance.
    /// </summary>
    /// <param name="r"> Red component </param>
    /// <param name="g"> Green component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="a"> Alpha component </param>
    /// <returns> the packed color as a 32-bit int. </returns>
    public static uint PackedColorABGR( uint a, uint b, uint g, uint r )
    {
        return ( a << 24 ) | ( b << 16 ) | ( g << 8 ) | r;
    }

    /// <summary>
    /// Returns a string representation of the Color components ABGR.
    /// </summary>
    public string ABGRToString()
    {
        return $"A:{A},B:{B},G:{G},R:{R}";
    }

    #endregion ABGR Methods

    // ========================================================================
    // ========================================================================
    // ARGB ( Methods only )
    // ========================================================================
    // ========================================================================

    #region ARGB Methods

    /// <summary>
    /// Converts a 32-bit ARGB8888 integer value to a Color object.
    /// </summary>
    /// <param name="color"> The Color object to assign the converted values to. </param>
    /// <param name="value"> The 32-bit ARGB8888 integer value. </param>
    public static void ARGB8888ToColor( ref Color color, uint value )
    {
        color.A = ( ( value & 0xff000000 ) >>> 24 ) / 255f;
        color.R = ( ( value & 0x00ff0000 ) >>> 16 ) / 255f;
        color.G = ( ( value & 0x0000ff00 ) >>> 8 ) / 255f;
        color.B = ( value & 0x000000ff ) / 255f;
    }

    /// <summary>
    /// Returns the given seperate colour components as a 32-bit uint in the
    /// following format:-
    /// <li> Bits  0 - 7  : Blue component </li>
    /// <li> Bits  8 - 15 : Green component </li>
    /// <li> Bits 16 - 23 : Red component </li>
    /// <li> Bits 24 - 31 : Alpha component </li>
    /// </summary>
    /// <param name="a"> Alpha component </param>
    /// <param name="b"> Blue component </param>
    /// <param name="g"> Green component </param>
    /// <param name="r"> Red component </param>
    public static uint ARGB8888( float a, float r, float g, float b )
    {
        return ( ( uint )( a * 255 ) << 24 )
               | ( ( uint )( r * 255 ) << 16 )
               | ( ( uint )( g * 255 ) << 8 )
               | ( uint )( b * 255 );
    }

    /// <summary>
    /// Returns the given <see cref="Color"/> as a 32-bit uint in the
    /// following format:-
    /// <li>Bits  0 - 7  : Blue component</li>
    /// <li>Bits  8 - 15 : Green component</li>
    /// <li>Bits 16 - 23 : Red component</li>
    /// <li>Bits 24 - 31 : Alpha component</li>
    /// </summary>
    /// <param name="color"> The colour. </param>
    public static uint ARGB8888( Color color )
    {
        return ( ( uint )( color.A * 255 ) << 24 )
               | ( ( uint )( color.R * 255 ) << 16 )
               | ( ( uint )( color.G * 255 ) << 8 )
               | ( uint )( color.B * 255 );
    }

    #endregion ARGB Methods

    // ========================================================================
    // ========================================================================
    // HSV ( Methods only )
    // ========================================================================
    // ========================================================================

    #region HSV Methods

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

        if ( h < 0 )
        {
            h += 360;
        }

        var i = ( uint )( h / 60 ) % 6;
        var f = ( h / 60 ) - i;
        var p = v * ( 1 - s );
        var q = v * ( 1 - ( s * f ) );
        var t = v * ( 1 - ( s * ( 1 - f ) ) );

        ( this.R, this.G, this.B ) = i switch
        {
            0     => ( v, t, p ),
            1     => ( q, v, p ),
            2     => ( p, v, t ),
            3     => ( p, q, v ),
            4     => ( t, p, v ),
            var _ => ( v, p, q ),
        };

        return Clamp();
    }

    /// <summary>
    /// Sets RGB components using the specified Hue-Saturation-Value. This is a
    /// convenient method for fromHsv(float, float, float). This is the inverse
    /// of toHsv(float[]).
    /// </summary>
    /// <param name="hsv"> The Hue-Saturation-Value. </param>
    /// <returns> The modified color for chaining. </returns>
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
        if ( Math.Abs( range ) < FloatConstants.FLOAT_TOLERANCE )
        {
            hsv[ 0 ] = 0; // Undefined hue, achromatic case
        }
        else if ( Math.Abs( max - R ) < FloatConstants.FLOAT_TOLERANCE )
        {
            hsv[ 0 ] = ( ( ( 60 * ( G - B ) ) / range ) + 360 ) % 360;
        }
        else if ( Math.Abs( max - G ) < FloatConstants.FLOAT_TOLERANCE )
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

    #endregion HSV Methods

    // ========================================================================
    // ========================================================================

    #region From IEquatable Interface

    /// <inheritdoc/>
    public bool Equals( Color? other )
    {
        if ( other is null )
        {
            return false;
        }

        return PackedColorABGR() == other.PackedColorABGR();
    }

    /// <inheritdoc />
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

        var color = ( Color )obj;

        return this.PackedColorABGR() == color.PackedColorABGR();
    }

    /// Not from IEquatable, but connected to it because of Equals()
    /// <inheritdoc />
    public override int GetHashCode()
    {
        return PackedColorABGR().GetHashCode();
    }

    #endregion From IEquatable Interface

    // ========================================================================
    // ========================================================================

    #region operators

    /// <summary>
    /// Determines whether two <see cref="Color"/> objects are equal.
    /// </summary>
    /// <param name="c1"> The first <see cref="Color"/> object to compare, or <b>null</b>. </param>
    /// <param name="c2"> The second object to compare, or <b>null</b>. </param>
    /// <returns> <b>true</b> if the two objects are equal; otherwise, <b>false</b>. </returns>
    public static bool operator ==( Color? c1, object? c2 )
    {
        if ( c1 is null )
        {
            return c2 is null;
        }

        return c1.Equals( c2 );
    }

    /// <summary>
    /// Determines whether two <see cref="Color"/> objects are not equal.
    /// </summary>
    /// <param name="c1"> The first <see cref="Color"/> object to compare, or null. </param>
    /// <param name="c2"> The second object to compare, or null. </param>
    /// <returns><b>true</b> if the two objects are not equal; otherwise, <b>false</b>.</returns>
    public static bool operator !=( Color? c1, object? c2 )
    {
        return !( c1 == c2 );
    }

    /// <summary>
    /// Multiplies the Color Components of Color 1 by the Color Components of
    /// Color 2, ending with returning Color 1.
    /// </summary>
    /// <param name="c1"> Color 1, which will be returned. </param>
    /// <param name="c2"> Color 2. </param>
    public static Color operator *( Color? c1, Color? c2 )
    {
        ArgumentNullException.ThrowIfNull( c1 );
        ArgumentNullException.ThrowIfNull( c2 );

        c1.R *= c2.R;
        c1.G *= c2.G;
        c1.B *= c2.B;
        c1.A *= c2.A;

        return c1;
    }

    #endregion operators

    // ========================================================================
    // ========================================================================

    #region From ICloneable Interface

    /// <inheritdoc/>
    public object Clone()
    {
        return new Color( this );
    }

    #endregion From ICloneable Interface

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Debug string for DebuggerDisplay attribute.
    /// </summary>
    internal string RGBADebugString => $"R: {R}. G: {G}. B: {B}. A: {A}";

    /// <summary>
    /// Debugs the various methods and properties for this class.
    /// </summary>
    public void Debug()
    {
        Logger.Debug( RGBADebugString );
        Logger.Debug( $"PackedColorABGR: {ABGRPackedColor} :: {ABGRPackedColor:X}" );
        Logger.Debug( $"PackedColorRGBA: {RGBAPackedColor} :: {RGBAPackedColor:X}" );
        Logger.Debug( $"ABGRFloatPack: {ABGRFloatPack}" );
        Logger.Debug( $"RGBAFloatPack: {RGBAFloatPack}" );
        Logger.Debug( $"RGBA8888ToInt(R, G, B, A): {RGBA8888ToInt( R, G, B, A ):X}" );
        Logger.Debug( $"ToFloatBitsABGR(): {ToFloatBitsABGR()}" );
        Logger.Debug( $"ToFloatBitsRGBA(): {ToFloatBitsRGBA()}" );
        Logger.Debug( $"ToFloatBitsABGR(F,F,F,F): {ToFloatBitsABGR( A, B, G, R )}" );
        Logger.Debug( $"ToFloatBitsRGBA(F,F,F,F): {ToFloatBitsRGBA( R, G, B, A )}" );
        Logger.Divider();
    }

    // ========================================================================
    // ========================================================================
}