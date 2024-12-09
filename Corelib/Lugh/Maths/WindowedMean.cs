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


namespace Corelib.Lugh.Maths;

/// <summary>
/// A simple class keeping track of the mean of a stream of values within
/// a certain window. the WindowedMean will only return a value in case
/// enough data has been sampled. After enough data has been sampled the
/// oldest sample will be replaced by the newest in case a new sample is
/// added.
/// </summary>
[PublicAPI]
public class WindowedMean
{
    private readonly float[] _values;

    private int   _addedValues = 0;
    private bool  _dirty       = true;
    private int   _lastValue;
    private float _mean = 0;

    /// <summary>
    /// constructor, window_size specifies the number of samples we will
    /// continuously get the mean and variance from. the class will only
    /// return meaning full values if at least window_size values have
    /// been added.
    /// </summary>
    /// <param name="windowSize">size of the sample window</param>
    public WindowedMean( int windowSize )
    {
        _values = new float[ windowSize ];
    }

    /// <summary>
    /// returns the mean of the samples added to this instance.
    /// Only returns meaningful results when at least window_size samples
    /// as specified in the constructor have been added.
    /// </summary>
    /// <returns> the mean </returns>
    public virtual float Mean
    {
        get
        {
            if ( HasEnoughData() )
            {
                if ( _dirty )
                {
                    float mean = 0;

                    foreach ( var t in _values )
                    {
                        mean += t;
                    }

                    _mean  = mean / _values.Length;
                    _dirty = false;
                }

                return _mean;
            }

            return 0;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns> the oldest value in the window </returns>
    public virtual float Oldest => _addedValues < _values.Length ? _values[ 0 ] : _values[ _lastValue ];

    /// <summary>
    /// </summary>
    /// <returns> the value last added </returns>
    public virtual float Latest => _values[ ( _lastValue - 1 ) == -1 ? _values.Length - 1 : _lastValue - 1 ];

    /// <summary>
    /// </summary>
    public virtual float Lowest
    {
        get
        {
            var lowest = float.MaxValue;

            foreach ( var t in _values )
            {
                lowest = Math.Min( lowest, t );
            }

            return lowest;
        }
    }

    /// <summary>
    /// </summary>
    public virtual float Highest
    {
        get
        {
            var lowest = float.MinValue;

            foreach ( var t in _values )
            {
                lowest = Math.Max( lowest, t );
            }

            return lowest;
        }
    }

    /// <summary>
    /// </summary>
    public virtual int ValueCount => _addedValues;

    /// <summary>
    /// </summary>
    public virtual int WindowSize => _values.Length;

    /// <summary>
    /// </summary>
    /// <returns>
    /// A new <code>float[]</code> containing all values currently in the window
    /// of the stream, in order from oldest to latest. The length of the array is
    /// smaller than the window size if not enough data has been added.
    /// </returns>
    public virtual float[] WindowValues
    {
        get
        {
            var windowValues = new float[ _addedValues ];

            if ( HasEnoughData() )
            {
                for ( var i = 0; i < windowValues.Length; i++ )
                {
                    windowValues[ i ] = _values[ ( i + _lastValue ) % _values.Length ];
                }
            }
            else
            {
                Array.Copy( _values, 0, windowValues, 0, _addedValues );
            }

            return windowValues;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns> whether the value returned will be meaningful</returns>
    public virtual bool HasEnoughData()
    {
        return _addedValues >= _values.Length;
    }

    /// <summary>
    /// clears this WindowedMean. The class will only return meaningful values
    /// after enough data has been added again.
    /// </summary>
    public virtual void Clear()
    {
        _addedValues = 0;
        _lastValue   = 0;

        for ( var i = 0; i < _values.Length; i++ )
        {
            _values[ i ] = 0;
        }

        _dirty = true;
    }

    /// <summary>
    /// adds a new sample to this mean. In case the window is full the oldest
    /// value will be replaced by this new value.
    /// </summary>
    /// <param name="value"> The value to add  </param>
    public virtual void AddValue( float value )
    {
        if ( _addedValues < _values.Length )
        {
            _addedValues++;
        }

        _values[ _lastValue++ ] = value;

        if ( _lastValue > ( _values.Length - 1 ) )
        {
            _lastValue = 0;
        }

        _dirty = true;
    }

    /// <summary>
    /// </summary>
    /// <returns> The standard deviation </returns>
    public virtual float StandardDeviation()
    {
        if ( !HasEnoughData() )
        {
            return 0;
        }

        var   mean = Mean;
        float sum  = 0;

        foreach ( var t in _values )
        {
            sum += ( t - mean ) * ( t - mean );
        }

        return ( float ) Math.Sqrt( sum / _values.Length );
    }
}
