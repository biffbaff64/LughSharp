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

using LibGDXSharp.Maths;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

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
/// <para>
/// <seealso cref="LibGDXSharp.Maths.MathUtils.LerpAngleDeg(float, float, float)"/>
/// </para>
/// </summary>
public class RotateToAction : TemporalAction
{
    public float Start                { get; set; }
    public float Rotation             { get; set; }
    public bool  UseShortestDirection { get; set; } = false;

    public RotateToAction()
    {
    }

    /** @param useShortestDirection Set to true to move directly to the closest angle */
    public RotateToAction( bool useShortestDirection )
    {
        this.UseShortestDirection = useShortestDirection;
    }

    protected new void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot Begin with null Target Actor!" );
        }

        Start = Target.Rotation;
    }

    protected override void Update( float percent )
    {
        if ( Target == null ) return;

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
            rotation = MathUtils.LerpAngleDeg( this.Start, this.Rotation, percent );
        }
        else
        {
            rotation = Start + ( ( Rotation - Start ) * percent );
        }

        Target.Rotation = rotation;
    }
}