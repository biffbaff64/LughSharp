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

namespace LibGDXSharp.Maths;

/// <summary>
/// Takes a linear value in the range of 0-1 and outputs a (usually) non-linear, interpolated value.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class Interpolation
{
    // ------------------------------------------------------------------------

    // ------------------------------------------------------------------------

    public abstract float Apply( float a );

    public float Apply( float start, float end, float a )
    {
        return start + ( ( end - start ) * Apply( a ) );
    }
}

// ------------------------------------------------------------------------

//    public readonly Interpolation linear = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a;
//        }
//    };

// Aka "smoothstep".
//    public readonly Interpolation smooth = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a * a * ( 3 - 2 * a );
//        }
//    };

//    public readonly Interpolation smooth2 = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            a = a * a * ( 3 - 2 * a );

//            return a * a * ( 3 - 2 * a );
//        }
//    };

// By Ken Perlin.
//    public readonly Interpolation smoother = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return a * a * a * ( a * ( a * 6 - 15 ) + 10 );
//        }
//    };

//    public readonly Interpolation fade = smoother;

//    public readonly Interpolation pow2InInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            if ( a < MathUtils.FLOAT_ROUNDING_ERROR ) return 0;

//            return ( float )Math.sqrt( a );
//        }
//    };

//    public readonly Interpolation pow2OutInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            if ( a < MathUtils.FLOAT_ROUNDING_ERROR ) return 0;

//            return 1 - ( float )Math.sqrt( -( a - 1 ) );
//        }
//    };

//    public readonly Interpolation pow3InInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return ( float )Math.cbrt( a );
//        }
//    };

//    public readonly Interpolation pow3OutInverse = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - ( float )Math.cbrt( -( a - 1 ) );
//        }
//    };

//    public readonly Interpolation sine = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return ( 1 - MathUtils.cos( a * MathUtils.PI ) ) / 2;
//        }
//    };

//    public readonly Interpolation sineIn = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - MathUtils.cos( a * MathUtils.HALF_PI );
//        }
//    };

//    public readonly Interpolation sineOut = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return MathUtils.sin( a * MathUtils.HALF_PI );
//        }
//    };

//    public readonly Interpolation circle = new Interpolation()
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

//    public readonly Interpolation circleIn = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            return 1 - ( float )Math.sqrt( 1 - a * a );
//        }
//    };

//    public readonly Interpolation circleOut = new Interpolation()
//    {
//        public float apply( float a )
//        {
//            a--;

//            return ( float )Math.sqrt( 1 - a * a );
//        }
//    };

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------
//public readonly Pow pow2 = new( 2 );
//
//// Slow, then fast.
//public readonly PowIn pow2In   = new( 2 );
//public readonly PowIn slowFast = new( 2 );
//
//// Fast, then slow.
//public readonly PowOut     pow2Out    = new( 2 );
//public readonly PowOut     fastSlow   = new( 2 );
//public readonly Pow        pow3       = new( 3 );
//public readonly PowIn      pow3In     = new( 3 );
//public readonly PowOut     pow3Out    = new( 3 );
//public readonly Pow        pow4       = new( 4 );
//public readonly PowIn      pow4In     = new( 4 );
//public readonly PowOut     pow4Out    = new( 4 );
//public readonly Pow        pow5       = new( 5 );
//public readonly PowIn      pow5In     = new( 5 );
//public readonly PowOut     pow5Out    = new( 5 );
//public readonly Exp        exp10      = new( 2, 10 );
//public readonly ExpIn      exp10In    = new( 2, 10 );
//public readonly ExpOut     exp10Out   = new( 2, 10 );
//public readonly Exp        exp5       = new( 2, 5 );
//public readonly ExpIn      exp5In     = new( 2, 5 );
//public readonly ExpOut     exp5Out    = new( 2, 5 );
//public readonly Elastic    elastic    = new( 2, 10, 7, 1 );
//public readonly ElasticIn  elasticIn  = new( 2, 10, 6, 1 );
//public readonly ElasticOut elasticOut = new( 2, 10, 7, 1 );
//public readonly Swing      swing      = new( 1.5f );
//public readonly SwingIn    swingIn    = new( 2f );
//public readonly SwingOut   swingOut   = new( 2f );
//public readonly Bounce     bounce     = new( 4 );
//public readonly BounceIn   bounceIn   = new( 4 );
//public readonly BounceOut  bounceOut  = new( 4 );

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public class LinearImp : Interpolation
{
    public override float Apply( float a ) => a;
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public class PowImp : Interpolation
{
    protected readonly int power;

    public PowImp( int power )
    {
        this.power = power;
    }

    public override float Apply( float a )
    {
        if ( a <= 0.5f ) return ( float )Math.Pow( a * 2, power ) / 2;

        return ( ( float )Math.Pow( ( a - 1 ) * 2, power )
                 / ( ( power % 2 ) == 0 ? -2 : 2 ) )
               + 1;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class PowInImp : PowImp
{
    public PowInImp( int power ) : base( power )
    {
    }

    public new float Apply( float a )
    {
        return ( float )Math.Pow( a, power );
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class PowOutImp : PowImp
{
    public PowOutImp( int power ) : base( power )
    {
    }

    public new float Apply( float a )
    {
        return ( ( float )Math.Pow( a - 1, power ) * ( ( power % 2 ) == 0 ? -1 : 1 ) ) + 1;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public class ExpImp : Interpolation
{
    protected readonly float value;
    protected readonly float power;
    protected readonly float min;
    protected readonly float scale;

    public ExpImp( float value, float power )
    {
        this.value = value;
        this.power = power;

        min   = ( float )Math.Pow( value, -power );
        scale = 1 / ( 1 - min );
    }

    public override float Apply( float a )
    {
        if ( a <= 0.5f )
        {
            return ( ( ( float )Math.Pow( value, power * ( ( a * 2 ) - 1 ) ) - min ) * scale ) / 2;
        }

        return ( 2 - ( ( ( float )Math.Pow( value, -power * ( ( a * 2 ) - 1 ) ) - min ) * scale ) ) / 2;
    }
};

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class ExpInImp : ExpImp
{
    public ExpInImp( float value, float power )
        : base( value, power )
    {
    }

    public new float Apply( float a )
    {
        return ( ( float )Math.Pow( value, power * ( a - 1 ) ) - min ) * scale;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class ExpOutImp : ExpImp
{
    public ExpOutImp( float value, float power )
        : base( value, power )
    {
    }

    public new float Apply( float a )
    {
        return 1 - ( ( ( float )Math.Pow( value, -power * a ) - min ) * scale );
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public class ElasticImp : Interpolation
{
    protected readonly float value;
    protected readonly float power;
    protected readonly float scale;
    protected readonly float bounces;

    public ElasticImp( float value, float power, int bounces, float scale )
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

            return ( ( float )Math.Pow( value, power * ( a - 1 ) )
                     * MathUtils.Sin( a * bounces )
                     * scale )
                   / 2;
        }

        a =  1 - a;
        a *= 2;

        return 1
               - ( ( ( float )Math.Pow( value, power * ( a - 1 ) )
                     * MathUtils.Sin( ( a ) * bounces )
                     * scale )
                   / 2 );
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class ElasticInImp : ElasticImp
{
    public ElasticInImp( float value, float power, int bounces, float scale )
        : base( value, power, bounces, scale )
    {
    }

    public new float Apply( float a )
    {
        if ( a >= 0.99f ) return 1;

        return ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class ElasticOutImp : ElasticImp
{
    public ElasticOutImp( float value, float power, int bounces, float scale )
        : base( value, power, bounces, scale )
    {
    }

    public new float Apply( float a )
    {
        if ( a == 0 ) return 0;

        a = 1 - a;

        return ( 1 - ( ( float )Math.Pow( value, power * ( a - 1 ) ) * MathUtils.Sin( a * bounces ) * scale ) );
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class Bounce : BounceOut
{
    public Bounce( float[] widths, float[] heights )
        : base( widths, heights )
    {
    }

    public Bounce( int bounces ) : base( bounces )
    {
    }

    private float Out( float a )
    {
        var test = a + ( widths[ 0 ] / 2 );

        if ( test < widths[ 0 ] ) return ( test / ( widths[ 0 ] / 2 ) ) - 1;

        return base.Apply( a );
    }

    public new float Apply( float a )
    {
        if ( a <= 0.5f ) return ( 1 - Out( 1 - ( a * 2 ) ) ) / 2;

        return Out( ( ( a * 2 ) - 1 ) / 2 ) + 0.5f;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public class BounceOut : Interpolation
{
    protected readonly float[] widths;
    protected readonly float[] heights;

    public BounceOut( float[] widths, float[] heights )
    {
        if ( widths.Length != heights.Length )
        {
            throw new ArgumentException( "Must be the same number of widths and heights." );
        }

        this.widths  = widths;
        this.heights = heights;
    }

    public BounceOut( int bounces )
    {
        if ( ( bounces < 2 ) || ( bounces > 5 ) )
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
        if ( a is 1 ) return 1;

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

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class BounceIn : BounceOut
{
    public BounceIn( float[] widths, float[] heights )
        : base( widths, heights )
    {
    }

    public BounceIn( int bounces ) : base( bounces )
    {
    }

    public new float Apply( float a )
    {
        return 1 - base.Apply( 1 - a );
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class Swing : Interpolation
{
    private readonly float _scale;

    public Swing( float scale )
    {
        this._scale = scale * 2;
    }

    public override float Apply( float a )
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

public sealed class SwingOut : Interpolation
{
    private readonly float _scale;

    public SwingOut( float scale )
    {
        this._scale = scale;
    }

    public override float Apply( float a )
    {
        a--;

        return ( a * a * ( ( ( _scale + 1 ) * a ) + _scale ) ) + 1;
    }
}

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

public sealed class SwingIn : Interpolation
{
    private readonly float _scale;

    public SwingIn( float scale )
    {
        this._scale = scale;
    }

    public override float Apply( float a )
    {
        return a * a * ( ( ( _scale + 1 ) * a ) - _scale );
    }
}