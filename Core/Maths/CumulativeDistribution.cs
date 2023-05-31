using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

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
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class CumulativeDistribution<T>
{
    public sealed class CumulativeValue
    {
        public readonly T value;

        public float frequency;
        public float interval;

        public CumulativeValue( T value, float frequency, float interval )
        {
            this.value     = value;
            this.frequency = frequency;
            this.interval  = interval;
        }
    }

    private readonly List< CumulativeValue > _values;

    public CumulativeDistribution()
    {
        _values = new List< CumulativeValue >( 10 );
    }

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

        foreach ( CumulativeValue cv in _values )
        {
            sum          += cv.interval;
            cv.frequency =  sum;
        }
    }

    /// <summary>
    /// Generate the cumulative distribution in [0,1] where each interval
    /// will get a frequency between [0,1]
    /// </summary>
    public virtual void GenerateNormalized()
    {
        float sum = 0;

        foreach ( CumulativeValue cv in _values )
        {
            sum += cv.interval;
        }

        float intervalSum = 0;

        foreach ( CumulativeValue cv in _values )
        {
            intervalSum  += cv.interval / sum;
            cv.frequency =  intervalSum;
        }
    }

    /// <summary>
    /// Generate the cumulative distribution in [0,1] where each value will have
    /// the same frequency and interval size
    /// </summary>
    public virtual void GenerateUniform()
    {
        float freq = 1f / _values.Count;

        for ( int i = 0; i < _values.Count; ++i )
        {
            // reset the interval to the normalized frequency
            _values[ i ].interval  = freq;
            _values[ i ].frequency = ( i + 1 ) * freq;
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
            var             imid  = imin + ( ( imax - imin ) / 2 );
            CumulativeValue value = _values[ imid ];

            if ( probability < value.frequency )
            {
                imax = imid - 1;
            }
            else if ( probability > value.frequency )
            {
                imin = imid + 1;
            }
            else
            {
                break;
            }
        }

        return _values[ imin ].value;
    }

    /// <returns> the value whose interval contains a random probability in [0,1] </returns>
    public virtual T Value() => Value( MathUtils.Random() );

    /// <returns> the amount of values </returns>
    public virtual int Size() => _values.Count;

    ///<returns> the interval size for the value at the given position </returns>
    public virtual float GetInterval( int index ) => _values[ index ].interval;

    ///<returns> the value at the given position </returns>
    public virtual T GetValue( int index ) => _values[ index ].value;

    /// <summary>
    /// Set the interval size on the passed in object.
    ///  The object must be present in the distribution. 
    /// </summary>
    public virtual void SetInterval( T obj, float intervalSize )
    {
        foreach ( CumulativeValue value in _values )
        {
            if ( Equals( value.value, obj ) )
            {
                value.interval = intervalSize;

                return;
            }
        }
    }

    /// <summary>
    /// Sets the interval size for the value at the given index
    /// </summary>
    public virtual void SetInterval( int index, float intervalSize )
    {
        _values[ index ].interval = intervalSize;
    }

    /// <summary>
    /// Removes all the values from the distribution
    /// </summary>
    public virtual void Clear()
    {
        _values.Clear();
    }
}