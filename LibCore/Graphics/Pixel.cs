// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.LibCore.Graphics;

[PublicAPI]
public struct Pixel
{
    public float R { get; set; } // Red
    public float G { get; set; } // Green
    public float B { get; set; } // Blue
    public float A { get; set; } // Alpha

    /// <summary>
    /// Creates a pixel from a <see cref="Color"/>.
    /// </summary>
    public Pixel( Color c )
    {
        this.A = c.A;
        this.R = c.R;
        this.G = c.G;
        this.B = c.B;
    }

    /// <summary>
    /// Creates a new pixel from integer RGB values.
    /// </summary>
    public Pixel( int r, int g, int b )
        : this( ( float ) r, ( float ) g, ( float ) b )
    {
    }

    /// <summary>
    /// Creates a new pixel from RGBA values.
    /// </summary>
    public Pixel( int r, int g, int b, int a )
        : this( ( float ) r, ( float ) g, ( float ) b, ( float ) a )
    {
    }

    /// <summary>
    /// Creates a pixel from RGB values.
    /// </summary>
    public Pixel( float r, float g, float b )
    {
        this.A = byte.MaxValue;
        this.R = r;
        this.G = g;
        this.B = b;
    }

    /// <summary>
    /// Creates a pixel from RGBA values.
    /// </summary>
    public Pixel( float r, float g, float b, float a )
    {
        this.A = a;
        this.R = r;
        this.G = g;
        this.B = b;
    }

    /// <summary>
    /// Creates a pixel from HSL-A values.
    /// </summary>
    public Pixel( double hue, double saturation, double lightness, float alpha = float.MaxValue )
    {
        lightness  %= 1.0;
        saturation %= 1.0;
        
        var q = lightness < 0.5 ? lightness * ( 1.0 + saturation ) : lightness + saturation - lightness * saturation;
        var p = 2.0 * lightness - q;
        
        hue %= 360.0;
        hue /= 360.0;
        
        var rgb1 = Pixel.HueToRGB( p, q, hue + 1.0 / 3.0 );
        var rgb2 = Pixel.HueToRGB( p, q, hue );
        var rgb3 = Pixel.HueToRGB( p, q, hue - 1.0 / 3.0 );
        
        this.A = alpha;
        this.R = Pixel.Window( rgb1 * ( double ) byte.MaxValue );
        this.G = Pixel.Window( rgb2 * ( double ) byte.MaxValue );
        this.B = Pixel.Window( rgb3 * ( double ) byte.MaxValue );
    }

    /// <summary>
    /// </summary>
    /// <param name="p"></param>
    /// <param name="q"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    private static double HueToRGB( double p, double q, double t )
    {
        if ( t < 0.0 )
        {
            ++t;
        }

        if ( t > 1.0 )
        {
            --t;
        }

        if ( t < 1.0 / 6.0 )
        {
            return p + ( q - p ) * 6.0 * t;
        }

        if ( t < 0.5 )
        {
            return q;
        }

        return t < 2.0 / 3.0 ? p + ( q - p ) * ( 2.0 / 3.0 - t ) * 6.0 : p;
    }

    /// <summary>
    /// </summary>
    /// <param name="c"></param>
    /// <returns></returns>
    private static byte Window( double c )
    {
        return ( byte ) Math.Min( Math.Max( 0.0, c ), ( double ) byte.MaxValue );
    }

    /// <summary>
    /// The alpha value of the Pixel from 0 to 1.
    /// </summary>
    public double AlphaValue
    {
        get => ( double ) this.A / ( double ) byte.MaxValue;
        set => this.A = ( byte ) ( value * ( double ) byte.MaxValue );
    }

    /// <summary>
    /// The red value of the Pixel from 0 to 1.
    /// </summary>
    public double RedValue
    {
        get => ( double ) this.R / ( double ) byte.MaxValue;
        set => this.R = ( byte ) ( value * ( double ) byte.MaxValue );
    }

    /// <summary>
    /// The green value of the Pixel from 0 to 1.
    /// </summary>
    public double GreenValue
    {
        get => ( double ) this.G / ( double ) byte.MaxValue;
        set => this.G = ( byte ) ( value * ( double ) byte.MaxValue );
    }

    /// <summary>
    /// The blue value of the Pixel from 0 to 1.
    /// </summary>
    public double BlueValue
    {
        get => ( double ) this.B / ( double ) byte.MaxValue;
        set => this.B = ( byte ) ( value * ( double ) byte.MaxValue );
    }

    /// <summary>
    /// The Hue value of the Pixel, radially spanning from 0 to 360 degrees.
    /// </summary>
    public double Hue
    {
        get
        {
            var num1 = ( double ) this.R / ( double ) byte.MaxValue;
            var num2 = ( double ) this.G / ( double ) byte.MaxValue;
            var num3 = ( double ) this.B / ( double ) byte.MaxValue;
            var num4 = num1;
            var num5 = num1;
            
            if ( num2 > num4 )
            {
                num4 = num2;
            }

            if ( num3 > num4 )
            {
                num4 = num3;
            }

            if ( num2 < num5 )
            {
                num5 = num2;
            }

            if ( num3 < num5 )
            {
                num5 = num3;
            }

            var num6 = num4 - num5;
            var num7 = 0.0;
            
            if ( Math.Abs( num1 - num4 ) < FloatConstants.FLOAT_TOLERANCE )
            {
                num7 = ( num2 - num3 ) / num6;
            }
            else if ( Math.Abs( num2 - num4 ) < FloatConstants.FLOAT_TOLERANCE )
            {
                num7 = 2.0 + ( num3 - num1 ) / num6;
            }
            else if ( Math.Abs( num3 - num4 ) < FloatConstants.FLOAT_TOLERANCE )
            {
                num7 = 4.0 + ( num1 - num2 ) / num6;
            }

            var d = num7 * 60.0;
            
            if ( d < 0.0 )
            {
                d += 360.0;
            }

            return double.IsNaN( d ) ? 0.0 : d;
        }
        set => this = new Pixel( value, this.Saturation, this.Lightness, this.A );
    }

    /// <summary>
    /// The saturation value of the Pixel.
    /// </summary>
    public double Saturation
    {
        get
        {
            var num1       = ( double ) this.R / ( double ) byte.MaxValue;
            var num2       = ( double ) this.G / ( double ) byte.MaxValue;
            var num3       = ( double ) this.B / ( double ) byte.MaxValue;
            var saturation = 0.0;
            var num4       = num1;
            var num5       = num1;
            
            if ( num2 > num4 )
            {
                num4 = num2;
            }

            if ( num3 > num4 )
            {
                num4 = num3;
            }

            if ( num2 < num5 )
            {
                num5 = num2;
            }

            if ( num3 < num5 )
            {
                num5 = num3;
            }

            if ( Math.Abs( num4 - num5 ) > FloatConstants.FLOAT_TOLERANCE )
            {
                saturation = ( num4 + num5 ) / 2.0 > 0.5 ? ( num4 - num5 ) / ( 2.0 - num4 - num5 ) : ( num4 - num5 ) / ( num4 + num5 );
            }

            return saturation;
        }
        set => this = new Pixel( this.Hue, value, this.Lightness, this.A );
    }

    /// <summary>
    /// The lightness value of the Pixel.
    /// </summary>
    public double Lightness
    {
        get
        {
            var num1 = ( double ) this.R / ( double ) byte.MaxValue;
            var num2 = ( double ) this.G / ( double ) byte.MaxValue;
            var num3 = ( double ) this.B / ( double ) byte.MaxValue;
            var num4 = num1;
            var num5 = num1;
            
            if ( num2 > num4 )
            {
                num4 = num2;
            }

            if ( num3 > num4 )
            {
                num4 = num3;
            }

            if ( num2 < num5 )
            {
                num5 = num2;
            }

            if ( num3 < num5 )
            {
                num5 = num3;
            }

            return ( num4 + num5 ) / 2.0;
        }
        set => this = new Pixel( this.Hue, this.Saturation, value, this.A );
    }

    /// <summary>
    /// The GDI color equivalent.
    /// </summary>
    public Color Color
    {
        get => Graphics.Color.FromFloatBits( this.A, this.R, this.G, this.B );
        set => this = new Pixel( value.A, value.R, value.G, value.B );
    }

    /// <summary>
    /// Additive mix of two pixels.
    /// </summary>
    public static Pixel operator +( Pixel a, Pixel b )
    {
        return new Pixel( ( byte ) ( ( a.AlphaValue / 2.0 + b.AlphaValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( ( a.RedValue / 2.0 + b.RedValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( ( a.GreenValue / 2.0 + b.GreenValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( ( a.BlueValue / 2.0 + b.BlueValue / 2.0 ) * ( double ) byte.MaxValue ) );
    }

    /// <summary>
    /// Multiplicative mix of two pixels.
    /// </summary>
    public static Pixel operator *( Pixel a, Pixel b )
    {
        return new Pixel( ( byte ) ( a.AlphaValue * b.AlphaValue * ( double ) byte.MaxValue ),
                          ( byte ) ( a.RedValue * b.RedValue * ( double ) byte.MaxValue ),
                          ( byte ) ( a.GreenValue * b.GreenValue * ( double ) byte.MaxValue ),
                          ( byte ) ( a.BlueValue * b.BlueValue * ( double ) byte.MaxValue ) );
    }

    /// <summary>
    /// Difference mix of two pixels.
    /// </summary>
    public static Pixel operator -( Pixel a, Pixel b )
    {
        return new Pixel( ( byte ) ( Math.Abs( a.AlphaValue / 2.0 - b.AlphaValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( Math.Abs( a.RedValue / 2.0 - b.RedValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( Math.Abs( a.GreenValue / 2.0 - b.GreenValue / 2.0 ) * ( double ) byte.MaxValue ),
                          ( byte ) ( Math.Abs( a.BlueValue / 2.0 - b.BlueValue / 2.0 ) * ( double ) byte.MaxValue ) );
    }
}