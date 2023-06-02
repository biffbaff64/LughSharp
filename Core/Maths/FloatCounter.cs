using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Maths;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class FloatCounter
{
    /// <summary>
    /// The amount of values added
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// The sum of all values
    /// </summary>
    public float Total { get; set; }

    /// <summary>
    /// The smallest value
    /// </summary>
    public float Min { get; set; }

    /// <summary>
    /// The largest value
    /// </summary>
    public float Max { get; set; }

    /// <summary>
    /// The average value (total / count)
    /// </summary>
    public float Average { get; set; }

    /// <summary>
    /// The latest raw value
    /// </summary>
    public float Latest { get; set; }

    /// <summary>
    /// The current windowed mean value
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Provides access to the WindowedMean if any (can be null)
    /// </summary>
    public WindowedMean? Mean { get; set; }

    public FloatCounter( int windowSize )
    {
        Mean = ( windowSize > 1 ) ? new WindowedMean( windowSize ) : null;

        Reset();
    }

    /// <summary>
    /// Add a value and update all fields. </summary>
    /// <param name="value"> The value to add  </param>
    public void Put( float value )
    {
        Latest =  value;
        Total  += value;
        Count++;
        Average = Total / Count;

        if ( Mean != null )
        {
            Mean.AddValue( value );
            this.Value = Mean.Mean;
        }
        else
        {
            this.Value = Latest;
        }

        if ( ( Mean == null ) || Mean.HasEnoughData() )
        {
            if ( this.Value < Min )
            {
                Min = this.Value;
            }

            if ( this.Value > Max )
            {
                Max = this.Value;
            }
        }
    }

    /// <summary>
    /// Reset all values to their default value. </summary>
    public void Reset()
    {
        Count   = 0;
        Total   = 0f;
        Min     = float.MaxValue;
        Max     = -float.MaxValue;
        Average = 0f;
        Latest  = 0f;
        Value   = 0f;

        Mean?.Clear();
    }

    public override string ToString()
    {
        return $"FloatCounter: count={Count}, total={Total}, min={Min},"
               + $"max={Max}, average={Average}, latest={Latest}, value={Value}";
    }
}