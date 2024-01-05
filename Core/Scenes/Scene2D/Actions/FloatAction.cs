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

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
///     An action that has a float, whose value is transitioned over time.
/// </summary>
public class FloatAction : TemporalAction
{

    /// <summary>
    ///     Creates a FloatAction that transitions from 0 to 1.
    /// </summary>
    public FloatAction()
    {
        Start    = 0;
        EndValue = 1;
    }

    /// <summary>
    ///     Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end )
    {
        Start    = start;
        EndValue = end;
    }

    /// <summary>
    ///     Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration ) : base( duration )
    {
        Start    = start;
        EndValue = end;
    }

    /// <summary>
    ///     Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration, IInterpolation interpolation )
        : base( duration, interpolation )
    {
        Start    = start;
        EndValue = end;
    }

    public float Start    { get; set; }
    public float EndValue { get; set; }
    public float Value    { get; set; }

    protected override void Begin() => Value = Start;

    protected override void Update( float percent )
    {
        if ( percent == 0 )
        {
            Value = Start;
        }
        else if ( percent is 1.0f )
        {
            Value = EndValue;
        }
        else
        {
            Value = Start + ( ( EndValue - Start ) * percent );
        }
    }
}
