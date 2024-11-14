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

using Corelib.LibCore.Maths;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Scenes.Scene2D.Actions;

/// <summary>
/// Sets the actor's rotation from its current value to a specific value.
/// <para>
/// By default, the rotation will take you from the starting value to the specified
/// value via simple subtraction. For example, setting the start at 350 and the target
/// at 10 will result in 340 degrees of movement.
/// </para>
/// <para>
/// If the action is instead set to useShortestDirection instead, it will rotate
/// straight to the target angle, regardless of where the angle starts and stops.
/// For example, starting at 350 and rotating to 10 will cause 20 degrees of rotation.
/// </para>
/// </summary>
[PublicAPI]
public class RotateToAction : TemporalAction
{
    public float Start                { get; set; }
    public float Rotation             { get; set; }
    public bool  UseShortestDirection { get; set; } = false;

    // ========================================================================
    
    /// <summary>
    /// </summary>
    /// <param name="useShortestDirection">
    /// Set to true to move directly to the closest angle
    /// </param>
    public RotateToAction( bool useShortestDirection )
    {
        UseShortestDirection = useShortestDirection;
    }

    protected override void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot Begin with null Target Actor!" );
        }

        Start = Target.Rotation;
    }

    protected override void Update( float percent )
    {
        if ( Target == null )
        {
            return;
        }

        float rotation;

        if ( percent == 0 )
        {
            rotation = Start;
        }
        else if ( percent is 1.0f )
        {
            rotation = Rotation;
        }
        else if ( UseShortestDirection )
        {
            rotation = MathUtils.LerpAngleDeg( Start, Rotation, percent );
        }
        else
        {
            rotation = Start + ( ( Rotation - Start ) * percent );
        }

        Target.Rotation = rotation;
    }
}
