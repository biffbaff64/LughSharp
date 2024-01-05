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
///     Sets the alpha for an actor's color (or a specified color), from the
///     current alpha to the new alpha. Note this action transitions from the
///     alpha at the time the action starts to the specified alpha.
/// </summary>
public class AlphaAction : TemporalAction
{

    private float _start;
    public  float Alpha { get; set; }

    protected override void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot begin with a null Target!" );
        }

        _start = Target.Color!.A;
    }

    protected override void Update( float percent )
    {
        if ( Target!.Color == null )
        {
            return;
        }

        if ( percent == 0 )
        {
            Target.Color.A = _start;
        }
        else if ( percent is 1.0f )
        {
            Target.Color.A = Alpha;
        }
        else
        {
            Target.Color.A = _start + ( ( Alpha - _start ) * percent );
        }
    }
}
