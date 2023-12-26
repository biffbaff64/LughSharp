// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
/// The Equalizer class can be used to specify
/// equalization settings for the MPEG audio decoder.
/// The equalizer consists of 32 band-pass filters.
/// Each band of the equalizer can take on a fractional value between
/// -1.0 and +1.0.
/// At -1.0, the input signal is attenuated by 6dB, at +1.0 the signal is
/// amplified by 6dB.
/// </summary>
[PublicAPI]
public class Equalizer
{
    private const int BANDS = 32;

    /// <summary>
    /// Equalizer setting to denote that a given band will not be
    /// present in the output signal.
    /// </summary>
    public const float BAND_NOT_PRESENT = float.NegativeInfinity;

    public readonly static Equalizer PassThruEq = new();

    private float[] _settings = new float[ BANDS ];

    /// <summary>
    /// Creates a new Equalizer instance.
    /// </summary>
    public Equalizer()
    {
    }

    //    private Equalizer(float b1, float b2, float b3, float b4, float b5,
    //                     float b6, float b7, float b8, float b9, float b10, float b11,
    //                     float b12, float b13, float b14, float b15, float b16,
    //                     float b17, float b18, float b19, float b20);

    public Equalizer( float[] settings )
    {
        FromFloatArray = settings;
    }

    public Equalizer( EQFunction eq )
    {
        FromEQFunction = eq;
    }

    public float[] FromFloatArray
    {
        set
        {
            Reset();

            var max = value.Length > BANDS ? BANDS : value.Length;

            for ( var i = 0; i < max; i++ )
            {
                _settings[ i ] = Limit( value[ i ] );
            }
        }
    }

    /// <summary>
    /// Sets the bands of this equalizer to the value the bands of
    /// another equalizer. Bands that are not present in both equalizers are ignored.
    /// </summary>
    public virtual Equalizer FromEqualizer
    {
        set
        {
            if ( value != this )
            {
                FromFloatArray = value._settings;
            }
        }
    }

    public EQFunction FromEQFunction
    {
        set
        {
            Reset();

            for ( var i = 0; i < BANDS; i++ )
            {
                _settings[ i ] = Limit( value.GetBand( i ) );
            }
        }
    }

    /// <summary>
    /// Retrieves the number of bands present in this equalizer.
    /// </summary>
    public virtual int BandCount => _settings.Length;

    /// <summary>
    /// Retrieves an array of floats whose values represent a
    /// scaling factor that can be applied to linear samples
    /// in each band to provide the equalization represented by
    /// this instance.
    /// </summary>
    /// <returns>
    /// an array of factors that can be applied to the
    /// subbands.
    /// </returns>
    public virtual float[] BandFactors
    {
        get
        {
            var factors = new float[ BANDS ];

            for ( var i = 0; i < BANDS; i++ )
            {
                factors[ i ] = GetBandFactor( _settings[ i ] );
            }

            return factors;
        }
    }

    /// <summary>
    /// Sets all bands to 0.0
    /// </summary>
    public void Reset()
    {
        for ( var i = 0; i < BANDS; i++ )
        {
            _settings[ i ] = 0.0f;
        }
    }

    public float SetBand( int band, float neweq )
    {
        var eq = 0.0f;

        if ( band is >= 0 and < BANDS )
        {
            eq                = _settings[ band ];
            _settings[ band ] = Limit( neweq );
        }

        return eq;
    }

    /// <summary>
    /// Retrieves the eq setting for a given band.
    /// </summary>
    public float GetBand( int band )
    {
        var eq = 0.0f;

        if ( band is >= 0 and < BANDS )
        {
            eq = _settings[ band ];
        }

        return eq;
    }

    private static float Limit( float eq )
    {
        return eq switch
               {
                   BAND_NOT_PRESENT => eq,
                   > 1.0f           => 1.0f,
                   < -1.0f          => -1.0f,
                   _                => eq
               };
    }

    /// <summary>
    /// Converts an equalizer band setting to a sample factor.
    /// The factor is determined by the function f = 2^n where
    /// n is the equalizer band setting in the range [-1.0,1.0].
    /// </summary>
    public static float GetBandFactor( float eq )
    {
        if ( eq.Equals( BAND_NOT_PRESENT ) )
        {
            return 0.0f;
        }

        var f = ( float )Math.Pow( 2.0, eq );

        return f;
    }

    [PublicAPI]
    public abstract class EQFunction
    {
        /// <summary>
        /// Returns the setting of a band in the equalizer.
        /// </summary>
        /// <param name="band">
        /// The index of the band to retrieve the setting for.
        /// </param>
        /// <returns>
        /// the setting of the specified band. This is a value between
        /// -1 and +1.
        /// </returns>
        public virtual float GetBand( int band ) => 0.0f;
    }
}
