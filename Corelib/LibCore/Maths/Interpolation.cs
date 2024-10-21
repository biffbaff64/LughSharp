// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Maths;

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

    public virtual float Apply( float x )
    {
        return Interp( x );
    }

    public virtual float Apply( float start, float end, float a )
    {
        return start + ( ( end - start ) * Apply( a ) );
    }
}

[PublicAPI]
public class Interpolation
{
    public static Interpolator Linear = new() { Interp = a => a };

    public static Interpolator Smooth = new() { Interp = a => a * a * ( 3 - ( 2 * a ) ) };

    public static Interpolator Smooth2 = new()
    {
        Interp = a =>
        {
            a = a * a * ( 3 - ( 2 * a ) );

            return a * a * ( 3 - ( 2 * a ) );
        }
    };

    public static Interpolator Smoother = new()
    {
        Interp = a => a * a * a * ( ( a * ( ( a * 6 ) - 15 ) ) + 10 )
    };

    public static Interpolator Fade = Smoother;

    public static Interpolator Pow2InInverse = new() { Interp = a => a };

    public static Interpolator Pow2OutInverse = new()
    {
        Interp = a =>
        {
            if ( a < FloatConstants.FLOAT_TOLERANCE )
            {
                return 0;
            }

            return 1 - ( float ) Math.Sqrt( -( a - 1 ) );
        }
    };

    public static Interpolator Pow3InInverse = new()
    {
        Interp = a => ( float ) Math.Cbrt( a )
    };

    public static Interpolator Pow3OutInverse = new()
    {
        Interp = a => 1 - ( float ) Math.Cbrt( -( a - 1 ) )
    };

    public static Interpolator Sine = new()
    {
        Interp = a => ( 1 - MathUtils.Cos( a * MathUtils.PI ) ) / 2
    };

    public static Interpolator SineIn = new()
    {
        Interp = a => 1 - MathUtils.Cos( a * ( MathUtils.PI / 2 ) )
    };

    public static Interpolator SineOut = new()
    {
        Interp = a => MathUtils.Sin( a * ( MathUtils.PI / 2 ) )
    };

    public static Interpolator Circle = new()
    {
        Interp = a =>
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( 1 - ( float ) Math.Sqrt( 1 - ( a * a ) ) ) / 2;
            }

            a--;
            a *= 2;

            return ( ( float ) Math.Sqrt( 1 - ( a * a ) ) + 1 ) / 2;
        }
    };

    public static Interpolator CircleIn = new()
    {
        Interp = a => 1 - ( float ) Math.Sqrt( 1 - ( a * a ) )
    };

    public static Interpolator CircleOut = new()
    {
        Interp = a =>
        {
            a--;

            return ( float ) Math.Sqrt( 1 - ( a * a ) );
        }
    };

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public static readonly Pow            Pow2       = new( 2 );
    public static readonly PowIn          Pow2In     = new( 2 );
    public static readonly PowOut         Pow2Out    = new( 2 );
    public static readonly Pow            Pow3       = new( 3 );
    public static readonly PowIn          Pow3In     = new( 3 );
    public static readonly PowOut         Pow3Out    = new( 3 );
    public static readonly Pow            Pow4       = new( 4 );
    public static readonly PowIn          Pow4In     = new( 4 );
    public static readonly PowOut         Pow4Out    = new( 4 );
    public static readonly Pow            Pow5       = new( 5 );
    public static readonly PowIn          Pow5In     = new( 5 );
    public static readonly PowOut         Pow5Out    = new( 5 );
    public static readonly Exp            Exp10      = new( 2, 10 );
    public static readonly ExpIn          Exp10In    = new( 2, 10 );
    public static readonly ExpOut         Exp10Out   = new( 2, 10 );
    public static readonly Exp            Exp5       = new( 2, 5 );
    public static readonly ExpIn          Exp5In     = new( 2, 5 );
    public static readonly ExpOut         Exp5Out    = new( 2, 5 );
    public static readonly ElasticImpl    Elastic    = new( 2, 10, 7, 1 );
    public static readonly ElasticInImpl  ElasticIn  = new( 2, 10, 6, 1 );
    public static readonly ElasticOutImpl ElasticOut = new( 2, 10, 7, 1 );
    public static readonly SwingImpl      Swing      = new( 1.5f );
    public static readonly SwingInImpl    SwingIn    = new( 2f );
    public static readonly SwingOutImpl   SwingOut   = new( 2f );
    public static readonly BounceImpl     Bounce     = new( 4 );
    public static readonly BounceInImpl   BounceIn   = new( 4 );
    public static readonly BounceOutImpl  BounceOut  = new( 4 );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class Pow : Interpolator
    {
        protected readonly int Power;

        public Pow( int p )
        {
            Power = p;
        }

        public override float Apply( float start, float end, float a )
        {
            if ( a <= 0.5f )
            {
                return ( float ) Math.Pow( a * 2, Power ) / 2;
            }

            return ( ( float ) Math.Pow( ( a - 1 ) * 2, Power ) / ( ( Power % 2 ) == 0 ? -2 : 2 ) ) + 1;
        }
    }

    [PublicAPI]
    public class PowIn : Pow
    {
        public PowIn( int power ) : base( power )
        {
        }

        public override float Apply( float a )
        {
            return ( float ) Math.Pow( a, Power );
        }
    }

    [PublicAPI]
    public class PowOut : Pow
    {
        public PowOut( int power ) : base( power )
        {
        }

        public override float Apply( float a )
        {
            return ( ( float ) Math.Pow( a - 1, Power ) * ( ( Power % 2 ) == 0 ? -1 : 1 ) ) + 1;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class Exp : Interpolator
    {
        protected readonly float Min;
        protected readonly float Power;
        protected readonly float Scale;
        protected readonly float Value;

        public Exp( float value, float power )
        {
            Value = value;
            Power = power;
            Min   = ( float ) Math.Pow( value, -power );
            Scale = 1 / ( 1 - Min );
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                return ( ( ( float ) Math.Pow( Value, Power * ( ( a * 2 ) - 1 ) ) - Min ) * Scale ) / 2;
            }

            return ( 2 - ( ( ( float ) Math.Pow( Value, -Power * ( ( a * 2 ) - 1 ) ) - Min ) * Scale ) ) / 2;
        }
    }

    [PublicAPI]
    public class ExpIn : Exp
    {
        public ExpIn( float value, float power )
            : base( value, power )
        {
        }

        public override float Apply( float a )
        {
            return ( ( float ) Math.Pow( Value, Power * ( a - 1 ) ) - Min ) * Scale;
        }
    }

    [PublicAPI]
    public class ExpOut : Exp
    {
        public ExpOut( float value, float power )
            : base( value, power )
        {
        }

        public override float Apply( float a )
        {
            return 1 - ( ( ( float ) Math.Pow( Value, -Power * a ) - Min ) * Scale );
        }
    }

    [PublicAPI]
    public class ElasticImpl : Interpolator
    {
        protected readonly float Bounces;
        protected readonly float Power;
        protected readonly float Scale;
        protected readonly float Value;

        public ElasticImpl( float value, float power, int bounces, float scale )
        {
            Value   = value;
            Power   = power;
            Scale   = scale;
            Bounces = bounces * MathUtils.PI * ( ( bounces % 2 ) == 0 ? 1 : -1 );
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( ( float ) Math.Pow( Value, Power * ( a - 1 ) ) * MathUtils.Sin( a * Bounces ) * Scale ) / 2;
            }

            a =  1 - a;
            a *= 2;

            return 1 - ( ( ( float ) Math.Pow( Value, Power * ( a - 1 ) ) * MathUtils.Sin( a * Bounces ) * Scale ) / 2 );
        }
    }

    [PublicAPI]
    public class ElasticInImpl : ElasticImpl
    {
        public ElasticInImpl( float value, float power, int bounces, float scale )
            : base( value, power, bounces, scale )
        {
        }

        public override float Apply( float a )
        {
            if ( a >= 0.99 )
            {
                return 1;
            }

            return ( float ) Math.Pow( Value, Power * ( a - 1 ) ) * MathUtils.Sin( a * Bounces ) * Scale;
        }
    }

    [PublicAPI]
    public class ElasticOutImpl : ElasticImpl
    {
        public ElasticOutImpl( float value, float power, int bounces, float scale )
            : base( value, power, bounces, scale )
        {
        }

        public override float Apply( float a )
        {
            if ( a == 0 )
            {
                return 0;
            }

            a = 1 - a;

            return 1 - ( ( float ) Math.Pow( Value, Power * ( a - 1 ) ) * MathUtils.Sin( a * Bounces ) * Scale );
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
            var test = a + ( Widths[ 0 ] / 2 );

            if ( test < Widths[ 0 ] )
            {
                return ( test / ( Widths[ 0 ] / 2 ) ) - 1;
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
        protected readonly float[] Heights;
        protected readonly float[] Widths;

        public BounceOutImpl( float[] widths, float[] heights )
        {
            if ( widths.Length != heights.Length )
            {
                throw new ArgumentException( "Must be the same number of widths and heights." );
            }

            Widths  = widths;
            Heights = heights;
        }

        public BounceOutImpl( int bounces )
        {
            if ( bounces is < 2 or > 5 )
            {
                throw new ArgumentException( "bounces cannot be < 2 or > 5: " + bounces );
            }

            Widths       = new float[ bounces ];
            Heights      = new float[ bounces ];
            Heights[ 0 ] = 1;

            switch ( bounces )
            {
                case 2:
                    Widths[ 0 ]  = 0.6f;
                    Widths[ 1 ]  = 0.4f;
                    Heights[ 1 ] = 0.33f;

                    break;

                case 3:
                    Widths[ 0 ]  = 0.4f;
                    Widths[ 1 ]  = 0.4f;
                    Widths[ 2 ]  = 0.2f;
                    Heights[ 1 ] = 0.33f;
                    Heights[ 2 ] = 0.1f;

                    break;

                case 4:
                    Widths[ 0 ]  = 0.34f;
                    Widths[ 1 ]  = 0.34f;
                    Widths[ 2 ]  = 0.2f;
                    Widths[ 3 ]  = 0.15f;
                    Heights[ 1 ] = 0.26f;
                    Heights[ 2 ] = 0.11f;
                    Heights[ 3 ] = 0.03f;

                    break;

                case 5:
                    Widths[ 0 ]  = 0.3f;
                    Widths[ 1 ]  = 0.3f;
                    Widths[ 2 ]  = 0.2f;
                    Widths[ 3 ]  = 0.1f;
                    Widths[ 4 ]  = 0.1f;
                    Heights[ 1 ] = 0.45f;
                    Heights[ 2 ] = 0.3f;
                    Heights[ 3 ] = 0.15f;
                    Heights[ 4 ] = 0.06f;

                    break;
            }

            Widths[ 0 ] *= 2;
        }

        public override float Apply( float a )
        {
            if ( a is 1.0f )
            {
                return 1;
            }

            a += Widths[ 0 ] / 2;

            float width = 0, height = 0;

            for ( int i = 0, n = Widths.Length; i < n; i++ )
            {
                width = Widths[ i ];

                if ( a <= width )
                {
                    height = Heights[ i ];

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

        public override float Apply( float a )
        {
            return 1 - base.Apply( 1 - a );
        }
    }

    [PublicAPI]
    public class SwingImpl : Interpolator
    {
        protected readonly float Scale;

        public SwingImpl( float scale )
        {
            Scale = scale * 2;
        }

        public override float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( a * a * ( ( ( Scale + 1 ) * a ) - Scale ) ) / 2;
            }

            a--;
            a *= 2;

            return ( ( a * a * ( ( ( Scale + 1 ) * a ) + Scale ) ) / 2 ) + 1;
        }
    }

    [PublicAPI]
    public class SwingOutImpl : Interpolator
    {
        private readonly float _scale;

        public SwingOutImpl( float scale )
        {
            _scale = scale;
        }

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

        public SwingInImpl( float scale )
        {
            _scale = scale;
        }

        public override float Apply( float a )
        {
            return a * a * ( ( ( _scale + 1 ) * a ) - _scale );
        }
    }
}
