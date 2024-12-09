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


namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// The Equalizer class can be used to specify equalization settings for the
/// MPEG audio decoder. The equalizer consists of 32 band-pass filters. Each
/// band of the equalizer can take on a fractional value between -1.0 and +1.0.
/// At -1.0, the input signal is attenuated by 6dB, at +1.0 the signal is
/// amplified by 6dB.
/// </summary>
[PublicAPI]
public class Equalizer
{
    /// <summary>
    /// Equalizer setting to denote that a given band will not be
    /// present in the output signal.
    /// </summary>
    public const float BAND_NOT_PRESENT = float.NegativeInfinity;

    public static readonly Equalizer PassThruEq = new();

    private const int BANDS = 32;

    private readonly float[] _settings = new float[ BANDS ];

    // ========================================================================

    /// <summary>
    /// Creates a new Equalizer instance.
    /// </summary>
    public Equalizer()
    {
    }

    /// <summary>
    /// Creates a new Equalizer instance, using the supplied float[] array
    /// to initialise this instances equalizer bands.
    /// </summary>
    public Equalizer( float[] settings )
    {
        SetFromFloatArray = settings;
    }

    public Equalizer( EQFunction eq )
    {
        FromEQFunction = eq;
    }

    public float[] SetFromFloatArray
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
    /// Sets the bands of this equalizer to the value the bands of another
    /// equalizer. Bands that are not present in both equalizers are ignored.
    /// </summary>
    public virtual Equalizer SetFromEqualizer
    {
        set
        {
            if ( value != this )
            {
                SetFromFloatArray = value._settings;
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
    public virtual int CurrentBandCount => _settings.Length;

    /// <summary>
    /// Retrieves an array of floats whose values represent a scaling factor that can be
    /// applied to linear samples in each band to provide the equalization represented by
    /// this instance.
    /// </summary>
    /// <returns> an array of factors that can be applied to the subbands. </returns>
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
    /// Sets all bands to 0.0f
    /// </summary>
    public void Reset()
    {
        for ( var i = 0; i < BANDS; i++ )
        {
            _settings[ i ] = 0.0f;
        }
    }

    /// <summary>
    /// Sets the given band to the provided value.
    /// </summary>
    /// <param name="band"> The band. </param>
    /// <param name="neweq"> The new value. </param>
    /// <returns></returns>
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

    /// <summary>
    /// Returns the Equalizer band limit for the specified band.
    /// </summary>
    private static float Limit( float eq )
    {
        return eq switch
        {
            BAND_NOT_PRESENT => eq,
            > 1.0f           => 1.0f,
            < -1.0f          => -1.0f,
            var _            => eq,
        };
    }

    /// <summary>
    /// Converts an equalizer band setting to a sample factor. The factor is
    /// determined by the function f = 2^n where n is the equalizer band setting
    /// in the range [-1.0,1.0].
    /// </summary>
    public static float GetBandFactor( float eq )
    {
        if ( eq.Equals( BAND_NOT_PRESENT ) )
        {
            return 0.0f;
        }

        var f = ( float ) Math.Pow( 2.0, eq );

        return f;
    }

    // ========================================================================

    [PublicAPI]
    public abstract class EQFunction
    {
        /// <summary>
        /// Returns the setting of a band in the equalizer.
        /// </summary>
        /// <param name="band"> The index of the band to retrieve the setting for. </param>
        /// <returns> the setting of the specified band. This is a value between -1 and +1. </returns>
        public virtual float GetBand( int band )
        {
            return 0.0f;
        }
    }
}
