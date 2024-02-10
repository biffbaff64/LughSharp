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


namespace LibGDXSharp.LibCore.Maths;

[PublicAPI]
public interface IInterpolation
{
    Func< float, float > Interp { get; set; }

    float Apply( float x );

    float Apply( float start, float end, float a );
}

[PublicAPI]
public class Interpolator : IInterpolation
{
    public Func< float, float > Interp { get; set; } = null!;

    public virtual float Apply( float x ) => Interp( x );

    public virtual float Apply( float start, float end, float a ) => start + ( ( end - start ) * Apply( a ) );
}

[PublicAPI]
public class Interpolation
{
    public static Interpolator linear = new() { Interp = a => a };

    public static Interpolator smooth = new() { Interp = a => a * a * ( 3 - ( 2 * a ) ) };

    public static Interpolator smooth2 = new()
    {
        Interp = a =>
        {
            a = a * a * ( 3 - ( 2 * a ) );

            return a * a * ( 3 - ( 2 * a ) );
        }
    };

    public static Interpolator smoother = new()
    {
        Interp = a => a * a * a * ( ( a * ( ( a * 6 ) - 15 ) ) + 10 )
    };

    public static Interpolator fade = smoother;

    public static Interpolator pow2InInverse = new() { Interp = a => a };

    public static Interpolator pow2OutInverse = new()
    {
        Interp = a =>
        {
            if ( a < MathUtils.FLOAT_ROUNDING_ERROR )
            {
                return 0;
            }

            return 1 - ( float )Math.Sqrt( -( a - 1 ) );
        }
    };

    public static Interpolator pow3InInverse = new()
    {
        Interp = a => ( float )Math.Cbrt( a )
    };

    public static Interpolator pow3OutInverse = new()
    {
        Interp = a => 1 - ( float )Math.Cbrt( -( a - 1 ) )
    };

    public static Interpolator sine = new()
    {
        Interp = a => ( 1 - MathUtils.Cos( a * MathUtils.PI ) ) / 2
    };

    public static Interpolator sineIn = new()
    {
        Interp = a => 1 - MathUtils.Cos( a * ( MathUtils.PI / 2 ) )
    };

    public static Interpolator sineOut = new()
    {
        Interp = a => MathUtils.Sin( a * ( MathUtils.PI / 2 ) )
    };

    public static Interpolator circle = new()
    {
        Interp = a =>
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( 1 - ( float )Math.Sqrt( 1 - ( a * a ) ) ) / 2;
            }

            a--;
            a *= 2;

            return ( ( float )Math.Sqrt( 1 - ( a * a ) ) + 1 ) / 2;
        }
    };

    public static Interpolator circleIn = new()
    {
        Interp = a => 1 - ( float )Math.Sqrt( 1 - ( a * a ) )
    };

    public static Interpolator circleOut = new()
    {
        Interp = a =>
        {
            a--;

            return ( float )Math.Sqrt( 1 - ( a * a ) );
        }
    };

    public readonly static Pow            Pow2       = new( 2 );
    public readonly static PowIn          Pow2In     = new( 2 );
    public readonly static PowOut         Pow2Out    = new( 2 );
    public readonly static Pow            Pow3       = new( 3 );
    public readonly static PowIn          Pow3In     = new( 3 );
    public readonly static PowOut         Pow3Out    = new( 3 );
    public readonly static Pow            Pow4       = new( 4 );
    public readonly static PowIn          Pow4In     = new( 4 );
    public readonly static PowOut         Pow4Out    = new( 4 );
    public readonly static Pow            Pow5       = new( 5 );
    public readonly static PowIn          Pow5In     = new( 5 );
    public readonly static PowOut         Pow5Out    = new( 5 );
    public readonly static Exp            Exp10      = new( 2, 10 );
    public readonly static ExpIn          Exp10In    = new( 2, 10 );
    public readonly static ExpOut         Exp10Out   = new( 2, 10 );
    public readonly static Exp            Exp5       = new( 2, 5 );
    public readonly static ExpIn          Exp5In     = new( 2, 5 );
    public readonly static ExpOut         Exp5Out    = new( 2, 5 );
    public readonly static ElasticImpl    Elastic    = new( 2, 10, 7, 1 );
    public readonly static ElasticInImpl  ElasticIn  = new( 2, 10, 6, 1 );
    public readonly static ElasticOutImpl ElasticOut = new( 2, 10, 7, 1 );
    public readonly static SwingImpl      Swing      = new( 1.5f );
    public readonly static SwingInImpl    SwingIn    = new( 2f );
    public readonly static SwingOutImpl   SwingOut   = new( 2f );
    public readonly static BounceImpl     Bounce     = new( 4 );
    public readonly static BounceInImpl   BounceIn   = new( 4 );
    public readonly static BounceOutImpl  BounceOut  = new( 4 );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class Pow : Interpolator
    {
        protected readonly int power;

        public Pow( int p ) => power = p;

        public override float Apply( float start, float end, float a )
        {
            if ( a <= 0.5f )
            {
                return ( float )Math.Pow( a * 2, power ) / 2;
            }

            return ( ( float )Math.Pow( ( a - 1 ) * 2, power ) / ( ( power % 2 ) == 0 ? -2 : 2 ) ) + 1;
        }
    }

    [PublicAPI]
    public class PowIn : Pow
    {
        public PowIn( int power ) : base( power )
        {
        }

        public override float Apply( float a ) => ( float )Math.Pow( a, power );
    }

    [PublicAPI]
    public class PowOut : Pow
    {
        public PowOut( int power ) : base( power )
        {
        }

        public override float Apply( float a ) => ( ( float )Math.Pow( a - 1, power ) * ( ( power % 2 ) == 0 ? -1 : 1 ) ) + 1;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class Exp : Interpolator
    {
        protected readonly float min;
        protected readonly float power;
        protected readonly float scale;
        protected readonly float value;

        public Exp( float value, float power )
        {
            this.value = value;
            this.power = power;
            min        = ( float )Math.Pow( value, -power );
            scale      = 1 / ( 1 - min );
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                return ( ( ( float )Math.Pow( value, power * ( ( a * 2 ) - 1 ) ) - min ) * scale ) / 2;
            }

            return ( 2 - ( ( ( float )Math.Pow( value, -power * ( ( a * 2 ) - 1 ) ) - min ) * scale ) ) / 2;
        }
    }

    [PublicAPI]
    public class ExpIn : Exp
    {
        public ExpIn( float value, float power )
            : base( value, power )
        {
        }

        public new float Apply( float a ) => ( ( float )Math.Pow( value, power * ( a - 1 ) ) - min ) * scale;
    }

    [PublicAPI]
    public class ExpOut : Exp
    {
        public ExpOut( float value, float power )
            : base( value, power )
        {
        }

        public new float Apply( float a ) => 1 - ( ( ( float )Math.Pow( value, -power * a ) - min ) * scale );
    }

    [PublicAPI]
    public class ElasticImpl : Interpolator
    {
        protected readonly float bounces;
        protected readonly float power;
        protected readonly float scale;
        protected readonly float value;

        public ElasticImpl( float value, float power, int bounces, float scale )
        {
            this.value   = value;
            this.power   = power;
            this.scale   = scale;
            this.bounces = bounces * MathUtils.PI * ( ( bounces % 2 ) == 0 ? 1 : -1 );
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale ) / 2;
            }

            a =  1 - a;
            a *= 2;

            return 1 - ( ( ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale ) / 2 );
        }
    }

    [PublicAPI]
    public class ElasticInImpl : ElasticImpl
    {
        public ElasticInImpl( float value, float power, int bounces, float scale )
            : base( value, power, bounces, scale )
        {
        }

        public new float Apply( float a )
        {
            if ( a >= 0.99 )
            {
                return 1;
            }

            return ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale;
        }
    }

    [PublicAPI]
    public class ElasticOutImpl : ElasticImpl
    {
        public ElasticOutImpl( float value, float power, int bounces, float scale )
            : base( value, power, bounces, scale )
        {
        }

        public new float Apply( float a )
        {
            if ( a == 0 )
            {
                return 0;
            }

            a = 1 - a;

            return 1 - ( ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale );
        }
    }

    [PublicAPI]
    public class BounceImpl : BounceOutImpl
    {
        public BounceImpl( float[] widths, float[] heights )
            : base( widths, heights )
        {
        }

        public BounceImpl( int bounces )
            : base( bounces )
        {
        }

        private float Out( float a )
        {
            var test = a + ( widths[ 0 ] / 2 );

            if ( test < widths[ 0 ] )
            {
                return ( test / ( widths[ 0 ] / 2 ) ) - 1;
            }

            return base.Apply( a );
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                return ( 1 - Out( 1 - ( a * 2 ) ) ) / 2;
            }

            return ( Out( ( a * 2 ) - 1 ) / 2 ) + 0.5f;
        }
    }

    [PublicAPI]
    public class BounceOutImpl : Interpolator
    {
        protected readonly float[] heights;
        protected readonly float[] widths;

        public BounceOutImpl( float[] widths, float[] heights )
        {
            if ( widths.Length != heights.Length )
            {
                throw new ArgumentException( "Must be the same number of widths and heights." );
            }

            this.widths  = widths;
            this.heights = heights;
        }

        public BounceOutImpl( int bounces )
        {
            if ( bounces is < 2 or > 5 )
            {
                throw new ArgumentException( "bounces cannot be < 2 or > 5: " + bounces );
            }

            widths       = new float[ bounces ];
            heights      = new float[ bounces ];
            heights[ 0 ] = 1;

            switch ( bounces )
            {
                case 2:
                    widths[ 0 ]  = 0.6f;
                    widths[ 1 ]  = 0.4f;
                    heights[ 1 ] = 0.33f;

                    break;

                case 3:
                    widths[ 0 ]  = 0.4f;
                    widths[ 1 ]  = 0.4f;
                    widths[ 2 ]  = 0.2f;
                    heights[ 1 ] = 0.33f;
                    heights[ 2 ] = 0.1f;

                    break;

                case 4:
                    widths[ 0 ]  = 0.34f;
                    widths[ 1 ]  = 0.34f;
                    widths[ 2 ]  = 0.2f;
                    widths[ 3 ]  = 0.15f;
                    heights[ 1 ] = 0.26f;
                    heights[ 2 ] = 0.11f;
                    heights[ 3 ] = 0.03f;

                    break;

                case 5:
                    widths[ 0 ]  = 0.3f;
                    widths[ 1 ]  = 0.3f;
                    widths[ 2 ]  = 0.2f;
                    widths[ 3 ]  = 0.1f;
                    widths[ 4 ]  = 0.1f;
                    heights[ 1 ] = 0.45f;
                    heights[ 2 ] = 0.3f;
                    heights[ 3 ] = 0.15f;
                    heights[ 4 ] = 0.06f;

                    break;
            }

            widths[ 0 ] *= 2;
        }

        public override float Apply( float a )
        {
            if ( a is 1.0f )
            {
                return 1;
            }

            a += widths[ 0 ] / 2;

            float width = 0, height = 0;

            for ( int i = 0, n = widths.Length; i < n; i++ )
            {
                width = widths[ i ];

                if ( a <= width )
                {
                    height = heights[ i ];

                    break;
                }

                a -= width;
            }

            a /= width;

            var z = ( 4 / width ) * height * a;

            return 1 - ( ( z - ( z * a ) ) * width );
        }
    }

    [PublicAPI]
    public class BounceInImpl : BounceOutImpl
    {
        public BounceInImpl( float[] widths, float[] heights )
            : base( widths, heights )
        {
        }

        public BounceInImpl( int bounces )
            : base( bounces )
        {
        }

        public new float Apply( float a ) => 1 - base.Apply( 1 - a );
    }

    [PublicAPI]
    public class SwingImpl : Interpolator
    {
        protected readonly float scale;

        public SwingImpl( float scale ) => this.scale = scale * 2;

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( a * a * ( ( ( scale + 1 ) * a ) - scale ) ) / 2;
            }

            a--;
            a *= 2;

            return ( ( a * a * ( ( ( scale + 1 ) * a ) + scale ) ) / 2 ) + 1;
        }
    }

    [PublicAPI]
    public class SwingOutImpl : Interpolator
    {
        private readonly float _scale;

        public SwingOutImpl( float scale ) => _scale = scale;

        public override float Apply( float a )
        {
            a--;

            return ( a * a * ( ( ( _scale + 1 ) * a ) + _scale ) ) + 1;
        }
    }

    [PublicAPI]
    public class SwingInImpl : Interpolator
    {
        private readonly float _scale;

        public SwingInImpl( float scale ) => _scale = scale;

        public override float Apply( float a ) => a * a * ( ( ( _scale + 1 ) * a ) - _scale );
    }
}
