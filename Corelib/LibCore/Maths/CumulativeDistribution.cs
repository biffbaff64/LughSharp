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


namespace Corelib.LibCore.Maths;

/// <summary>
/// This class represents a cumulative distribution.
/// <para>
/// It can be used in scenarios where there are values with different probabilities
/// and it's required to pick one of those respecting the probability.
/// </para>
/// <para>
/// For example one could represent the frequency of the alphabet letters using a
/// cumulative distribution and use it to randomly pick a letter respecting their
/// probabilities (useful when generating random words).
/// </para>
/// <para>
/// Another example could be point generation on a mesh surface: one could generate a
/// cumulative distribution using triangles areas as interval size, in this way triangles
/// with a large area will be picked more often than triangles with a smaller one.
/// See <a href="http://en.wikipedia.org/wiki/Cumulative_distribution_function">Wikipedia</a>
/// for a detailed explanation.
/// </para>
/// </summary>
[PublicAPI]
public class CumulativeDistribution< T >
{
    private readonly List< CumulativeValue > _values = new( 10 );

    /// <summary>
    /// Adds a value with a given interval size to the distribution
    /// </summary>
    public virtual void Add( T value, float intervalSize )
    {
        _values.Add( new CumulativeValue( value, 0, intervalSize ) );
    }

    /// <summary>
    /// Adds a value with interval size equal to zero to the distribution
    /// </summary>
    public virtual void Add( T value )
    {
        _values.Add( new CumulativeValue( value, 0, 0 ) );
    }

    /// <summary>
    /// Generate the cumulative distribution
    /// </summary>
    public virtual void Generate()
    {
        float sum = 0;

        foreach ( var cv in _values )
        {
            sum          += cv.Interval;
            cv.Frequency =  sum;
        }
    }

    /// <summary>
    /// Generate the cumulative distribution in [0,1] where each interval
    /// will get a frequency between [0,1]
    /// </summary>
    public virtual void GenerateNormalized()
    {
        float sum = 0;

        foreach ( var cv in _values )
        {
            sum += cv.Interval;
        }

        float intervalSum = 0;

        foreach ( var cv in _values )
        {
            intervalSum  += cv.Interval / sum;
            cv.Frequency =  intervalSum;
        }
    }

    /// <summary>
    /// Generate the cumulative distribution in [0,1] where each value will have
    /// the same frequency and interval size
    /// </summary>
    public virtual void GenerateUniform()
    {
        var freq = 1f / _values.Count;

        for ( var i = 0; i < _values.Count; ++i )
        {
            // reset the interval to the normalized frequency
            _values[ i ].Interval  = freq;
            _values[ i ].Frequency = ( i + 1 ) * freq;
        }
    }


    /// <summary>
    /// Finds the value whose interval contains the given probability
    /// Binary search algorithm is used to find the value.
    /// </summary>
    /// <param name="probability"> </param>
    /// <returns> the value whose interval contains the probability  </returns>
    protected virtual T Value( float probability )
    {
        var imax = _values.Count - 1;
        var imin = 0;

        while ( imin <= imax )
        {
            var imid  = imin + ( ( imax - imin ) / 2 );
            var value = _values[ imid ];

            if ( probability < value.Frequency )
            {
                imax = imid - 1;
            }
            else if ( probability > value.Frequency )
            {
                imin = imid + 1;
            }
            else
            {
                break;
            }
        }

        return _values[ imin ].Value;
    }

    // ------------------------------------------------------------------------

    /// <returns> the value whose interval contains a random probability in [0,1] </returns>
    public virtual T Value()
    {
        return Value( MathUtils.Random() );
    }

    /// <returns> the amount of values </returns>
    public virtual int Size()
    {
        return _values.Count;
    }

    ///<returns> the interval size for the value at the given position </returns>
    public virtual float GetInterval( int index )
    {
        return _values[ index ].Interval;
    }

    ///<returns> the value at the given position </returns>
    public virtual T GetValue( int index )
    {
        return _values[ index ].Value;
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Set the interval size on the passed in object.
    /// The object must be present in the distribution.
    /// </summary>
    public virtual void SetInterval( T obj, float intervalSize )
    {
        foreach ( var value in _values )
        {
            if ( Equals( value.Value, obj ) )
            {
                value.Interval = intervalSize;

                return;
            }
        }
    }

    /// <summary>
    /// Sets the interval size for the value at the given index
    /// </summary>
    public virtual void SetInterval( int index, float intervalSize )
    {
        _values[ index ].Interval = intervalSize;
    }

    /// <summary>
    /// Removes all the values from the distribution
    /// </summary>
    public virtual void Clear()
    {
        _values.Clear();
    }

    [PublicAPI]
    public class CumulativeValue
    {
        public readonly T Value;

        public CumulativeValue( T value, float frequency, float interval )
        {
            Value     = value;
            Frequency = frequency;
            Interval  = interval;
        }

        public float Frequency { get; set; }
        public float Interval  { get; set; }
    }
}
