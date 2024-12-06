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


namespace Corelib.Lugh.Maths;

[PublicAPI]
public class FloatCounter
{
    public FloatCounter( int windowSize )
    {
        Mean = windowSize > 1 ? new WindowedMean( windowSize ) : null;

        Reset();
    }

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

    /// <summary>
    /// Add a value and update all fields.
    /// </summary>
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
            Value = Mean.Mean;
        }
        else
        {
            Value = Latest;
        }

        if ( ( Mean == null ) || Mean.HasEnoughData() )
        {
            if ( Value < Min )
            {
                Min = Value;
            }

            if ( Value > Max )
            {
                Max = Value;
            }
        }
    }

    /// <summary>
    /// Reset all values to their default value.
    /// </summary>
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
