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

using LughSharp.Lugh.Maths;

namespace LughSharp.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// An action that has a float, whose value is transitioned over time.
/// </summary>
[PublicAPI]
public class FloatAction : TemporalAction
{
    public float Start    { get; }
    public float EndValue { get; }
    public float Value    { get; set; }

    // ========================================================================
    
    /// <summary>
    /// Creates a FloatAction that transitions from 0.0f to 1.0f.
    /// </summary>
    public FloatAction()
    {
        Start    = 0.0f;
        EndValue = 1.0f;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end )
    {
        Start    = start;
        EndValue = end;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration ) : base( duration )
    {
        Start    = start;
        EndValue = end;
    }

    /// <summary>
    /// Creates a FloatAction that transitions from start to end.
    /// </summary>
    public FloatAction( float start, float end, float duration, IInterpolation interpolation )
        : base( duration, interpolation )
    {
        Start    = start;
        EndValue = end;
    }

    /// <inheritdoc />
    protected override void Begin()
    {
        Value = Start;
    }

    /// <inheritdoc />
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
