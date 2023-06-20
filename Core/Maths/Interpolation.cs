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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

/// <summary>
/// Takes a linear value in the range of 0-1 and outputs a (usually) non-linear, interpolated value.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class Interpolation
{
    public abstract float Apply( float a );

    public float Apply( float start, float end, float a )
    {
        return start + ( ( end - start ) * Apply( a ) );
    }

//    public readonly static Interpolation linear = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a;
//        }
//    };

    //

    /// Aka "smoothstep".
//    public static readonly Interpolation smooth = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a * a * ( 3 - 2 * a );
//        }
//    };

//    public static readonly Interpolation smooth2 = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            a = a * a * ( 3 - 2 * a );

//            return a * a * ( 3 - 2 * a );
//        }
//    };

    /// By Ken Perlin.
//    public static readonly Interpolation smoother = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a * a * a * ( a * ( a * 6 - 15 ) + 10 );
//        }
//    };
    
    public readonly static Interpolation Fade = smoother;

    public readonly static Pow Pow2 = new Pow( 2 );

    /// Slow, then fast.
    public readonly static PowIn Pow2In = new PowIn( 2 );
    public readonly static PowIn SlowFast = Pow2In;

    /// Fast, then slow.
    public readonly static PowOut Pow2Out = new PowOut( 2 );
    public readonly static PowOut FastSlow = Pow2Out;

//    public readonly static Interpolation pow2InInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            if ( a < MathUtils.FLOAT_ROUNDING_ERROR ) return 0;

//            return ( float )Math.sqrt( a );
//        }
//    };

//    public static readonly Interpolation pow2OutInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            if ( a < MathUtils.FLOAT_ROUNDING_ERROR ) return 0;

//            return 1 - ( float )Math.sqrt( -( a - 1 ) );
//        }
//    };

    public static readonly Pow    Pow3    = new Pow( 3 );
    public static readonly PowIn  Pow3In  = new PowIn( 3 );
    public static readonly PowOut Pow3Out = new PowOut( 3 );

//    public static readonly Interpolation pow3InInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return ( float )Math.cbrt( a );
//        }
//    };

//    public static readonly Interpolation pow3OutInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - ( float )Math.cbrt( -( a - 1 ) );
//        }
//    };

    public static readonly Pow    Pow4    = new Pow( 4 );
    public static readonly PowIn  Pow4In  = new PowIn( 4 );
    public static readonly PowOut Pow4Out = new PowOut( 4 );

    public static readonly Pow    Pow5    = new Pow( 5 );
    public static readonly PowIn  Pow5In  = new PowIn( 5 );
    public static readonly PowOut Pow5Out = new PowOut( 5 );

//    public static readonly Interpolation sine = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return ( 1 - MathUtils.cos( a * MathUtils.PI ) ) / 2;
//        }
//    };

//    public static readonly Interpolation sineIn = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - MathUtils.cos( a * MathUtils.HALF_PI );
//        }
//    };

//    public static readonly Interpolation sineOut = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return MathUtils.sin( a * MathUtils.HALF_PI );
//        }
//    };

    public static readonly Exp    Exp10    = new Exp( 2, 10 );
    public static readonly ExpIn  Exp10In  = new ExpIn( 2, 10 );
    public static readonly ExpOut Exp10Out = new ExpOut( 2, 10 );

    public static readonly Exp    Exp5    = new Exp( 2, 5 );
    public static readonly ExpIn  Exp5In  = new ExpIn( 2, 5 );
    public static readonly ExpOut Exp5Out = new ExpOut( 2, 5 );

//    public static readonly Interpolation circle = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            if ( a <= 0.5f )
//            {
//                a *= 2;

//                return ( 1 - ( float )Math.sqrt( 1 - a * a ) ) / 2;
//            }

//            a--;
//            a *= 2;

//            return ( ( float )Math.sqrt( 1 - a * a ) + 1 ) / 2;
//        }
//    };

//    public static readonly Interpolation circleIn = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - ( float )Math.sqrt( 1 - a * a );
//        }
//    };

//    public static readonly Interpolation circleOut = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            a--;

//            return ( float )Math.sqrt( 1 - a * a );
//        }
//    };

    public static readonly Elastic    Elastic    = new Elastic( 2, 10, 7, 1 );
    public static readonly ElasticIn  ElasticIn  = new ElasticIn( 2, 10, 6, 1 );
    public static readonly ElasticOut ElasticOut = new ElasticOut( 2, 10, 7, 1 );

    public static readonly Swing    Swing    = new Swing( 1.5f );
    public static readonly SwingIn  SwingIn  = new SwingIn( 2f );
    public static readonly SwingOut SwingOut = new SwingOut( 2f );

    public static readonly Bounce    Bounce    = new Bounce( 4 );
    public static readonly BounceIn  BounceIn  = new BounceIn( 4 );
    public static readonly BounceOut BounceOut = new BounceOut( 4 );

    //

    public static class Pow : Interpolation
    {
        readonly int _power;

        public Pow( int power )
        {
            this._power = power;
        }

        public float Apply( float a )
        {
            if ( a <= 0.5f ) return ( float )Math.pow( a * 2, _power ) / 2;

            return ( ( float )Math.pow( ( a - 1 ) * 2, _power ) / ( ( _power % 2 ) == 0 ? -2 : 2 ) ) + 1;
        }
    }

    public static class PowIn : Pow
    {
        public PowIn( int power )
        {
            base( power );
        }

        public float Apply( float a )
        {
            return ( float )Math.pow( a, _power );
        }
    }

    public static class PowOut : Pow
    {
        public PowOut( int power )
        {
            base( power );
        }

        public float Apply( float a )
        {
            return ( ( float )Math.pow( a - 1, _power ) * ( ( _power % 2 ) == 0 ? -1 : 1 ) ) + 1;
        }
    }

//

    public static class Exp : Interpolation
    {
        readonly float _value, _power, _min, _scale;

        public Exp( float value, float power )
        {
            this._value = value;
            this._power = power;
            _min        = ( float )Math.pow( value, -power );
            _scale      = 1 / ( 1 - _min );
        }

        public float Apply( float a )
        {
            if ( a <= 0.5f ) return ( ( ( float )Math.pow( _value, _power * ( ( a * 2 ) - 1 ) ) - _min ) * _scale ) / 2;

            return ( 2 - ( ( ( float )Math.pow( _value, -_power * ( ( a * 2 ) - 1 ) ) - _min ) * _scale ) ) / 2;
        }
    };

    public static class ExpIn : Exp
    {
        public ExpIn( float value, float power )
        {
            base( value, power );
        }

        public float Apply( float a )
        {
            return ( ( float )Math.pow( _value, _power * ( a - 1 ) ) - _min ) * _scale;
        }
    }

    public static class ExpOut : Exp
    {
        public ExpOut( float value, float power )
        {
            base( value, power );
        }

        public float Apply( float a )
        {
            return 1 - ( ( ( float )Math.pow( _value, -_power * a ) - _min ) * _scale );
        }
    }

//

    public static class Elastic : Interpolation
    {
        readonly float _value, _power, _scale, _bounces;

        public Elastic( float value, float power, int bounces, float scale )
        {
            this._value   = value;
            this._power   = power;
            this._scale   = scale;
            this._bounces = bounces * MathUtils.PI * ( ( bounces % 2 ) == 0 ? 1 : -1 );
        }

        public float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( ( float )Math.pow( _value, _power * ( a - 1 ) ) * MathUtils.sin( a * _bounces ) * _scale ) / 2;
            }

            a =  1 - a;
            a *= 2;

            return 1 - ( ( ( float )Math.pow( _value, _power * ( a - 1 ) ) * MathUtils.sin( ( a ) * _bounces ) * _scale ) / 2 );
        }
    }

    public static class ElasticIn : Elastic
    {
        public ElasticIn( float value, float power, int bounces, float scale )
        {
            base( value, power, bounces, scale );
        }

        public float Apply( float a )
        {
            if ( a >= 0.99 ) return 1;

            return ( float )Math.pow( _value, _power * ( a - 1 ) ) * MathUtils.sin( a * _bounces ) * _scale;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public static class ElasticOut : Elastic
    {
        public ElasticOut( float value, float power, int bounces, float scale )
        {
            base( value, power, bounces, scale );
        }

        public float Apply( float a )
        {
            if ( a == 0 ) return 0;
            a = 1 - a;

            return ( 1 - ( ( float )Math.pow( _value, _power * ( a - 1 ) ) * MathUtils.sin( a * _bounces ) * _scale ) );
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public static class Bounce : BounceOut
    {
        public Bounce( float[] widths, float[] heights )
        {
            base( widths, heights );
        }

        public Bounce( int bounces )
        {
            base( bounces );
        }

        private float out (float a)
        {
            float test = a + ( _widths[ 0 ] / 2 );

            if ( test < _widths[ 0 ] ) return ( test / ( _widths[ 0 ] / 2 ) ) - 1;

            return base.Apply( a );
        }

        public float Apply( float a )
        {
            if ( a <= 0.5f ) return ( 1 - out
            ( 1 - ( a * 2 ) )) / 2;

            return out( ( ( a * 2 ) - 1 ) / 2 ) + 0.5f;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public static class BounceOut : Interpolation
    {
        readonly float[] _widths, _heights;

        public BounceOut( float[] widths, float[] heights )
        {
            if ( widths.length != heights.length )
            {
                throw new ArgumentException( "Must be the same number of widths and heights." );
            }

            this._widths  = widths;
            this._heights = heights;
        }

        public BounceOut( int bounces )
        {
            if ( ( bounces < 2 ) || ( bounces > 5 ) )
            {
                throw new ArgumentException( "bounces cannot be < 2 or > 5: " + bounces );
            }

            _widths       = new float[ bounces ];
            _heights      = new float[ bounces ];
            _heights[ 0 ] = 1;

            switch ( bounces )
            {
                case 2:
                    _widths[ 0 ]  = 0.6f;
                    _widths[ 1 ]  = 0.4f;
                    _heights[ 1 ] = 0.33f;

                    break;

                case 3:
                    _widths[ 0 ]  = 0.4f;
                    _widths[ 1 ]  = 0.4f;
                    _widths[ 2 ]  = 0.2f;
                    _heights[ 1 ] = 0.33f;
                    _heights[ 2 ] = 0.1f;

                    break;

                case 4:
                    _widths[ 0 ]  = 0.34f;
                    _widths[ 1 ]  = 0.34f;
                    _widths[ 2 ]  = 0.2f;
                    _widths[ 3 ]  = 0.15f;
                    _heights[ 1 ] = 0.26f;
                    _heights[ 2 ] = 0.11f;
                    _heights[ 3 ] = 0.03f;

                    break;

                case 5:
                    _widths[ 0 ]  = 0.3f;
                    _widths[ 1 ]  = 0.3f;
                    _widths[ 2 ]  = 0.2f;
                    _widths[ 3 ]  = 0.1f;
                    _widths[ 4 ]  = 0.1f;
                    _heights[ 1 ] = 0.45f;
                    _heights[ 2 ] = 0.3f;
                    _heights[ 3 ] = 0.15f;
                    _heights[ 4 ] = 0.06f;

                    break;
            }

            _widths[ 0 ] *= 2;
        }

        public float Apply( float a )
        {
            if ( a == 1 ) return 1;
            a += _widths[ 0 ] / 2;
            float width = 0, height = 0;

            for ( int i = 0, n = _widths.length; i < n; i++ )
            {
                width = _widths[ i ];

                if ( a <= width )
                {
                    height = _heights[ i ];

                    break;
                }

                a -= width;
            }

            a /= width;
            float z = ( 4 / width ) * height * a;

            return 1 - ( ( z - ( z * a ) ) * width );
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class BounceIn : BounceOut
    {
        public BounceIn( float[] widths, float[] heights )
        {
            base( widths, heights );
        }

        public BounceIn( int bounces )
        {
            base( bounces );
        }

        public float Apply( float a )
        {
            return 1 - base.Apply( 1 - a );
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class Swing : Interpolation
    {
        private readonly float _scale;

        public Swing( float scale )
        {
            this._scale = scale * 2;
        }

        public float Apply( float a )
        {
            if ( a <= 0.5f )
            {
                a *= 2;

                return ( a * a * ( ( ( _scale + 1 ) * a ) - _scale ) ) / 2;
            }

            a--;
            a *= 2;

            return ( ( a * a * ( ( ( _scale + 1 ) * a ) + _scale ) ) / 2 ) + 1;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class SwingOut : Interpolation
    {
        private readonly float _scale;

        public SwingOut( float scale )
        {
            this._scale = scale;
        }

        public float Apply( float a )
        {
            a--;

            return ( a * a * ( ( ( _scale + 1 ) * a ) + _scale ) ) + 1;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class SwingIn : Interpolation
    {
        private readonly float _scale;

        public SwingIn( float scale )
        {
            this._scale = scale;
        }

        public float Apply( float a )
        {
            return a * a * ( ( ( _scale + 1 ) * a ) - _scale );
        }
    }
}