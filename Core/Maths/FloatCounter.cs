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

public class FloatCounter
{

    public FloatCounter( int windowSize )
    {
        Mean = windowSize > 1 ? new WindowedMean( windowSize ) : null;

        Reset();
    }

    /// <summary>
    ///     The amount of values added
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    ///     The sum of all values
    /// </summary>
    public float Total { get; set; }

    /// <summary>
    ///     The smallest value
    /// </summary>
    public float Min { get; set; }

    /// <summary>
    ///     The largest value
    /// </summary>
    public float Max { get; set; }

    /// <summary>
    ///     The average value (total / count)
    /// </summary>
    public float Average { get; set; }

    /// <summary>
    ///     The latest raw value
    /// </summary>
    public float Latest { get; set; }

    /// <summary>
    ///     The current windowed mean value
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    ///     Provides access to the WindowedMean if any (can be null)
    /// </summary>
    public WindowedMean? Mean { get; set; }

    /// <summary>
    ///     Add a value and update all fields.
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
    ///     Reset all values to their default value.
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

    public override string ToString() => $"FloatCounter: count={Count}, total={Total}, min={Min},"
                                       + $"max={Max}, average={Average}, latest={Latest}, value={Value}";
}
