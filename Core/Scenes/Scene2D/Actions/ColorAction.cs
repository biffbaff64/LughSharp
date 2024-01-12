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
///     Sets the actor's color (or a specified color), from the current to the new
///     color. Note this action transitions from the color at the time the action
///     starts to the specified color.
/// </summary>
public class ColorAction : TemporalAction
{
    private readonly Color _endColor = new();
    private          float _startA;
    private          float _startB;
    private          float _startG;
    private          float _startR;

    public virtual Color EndColor
    {
        get => _endColor;
        set => _endColor.Set( value );
    }

    protected new void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot begin with a null Target!" );
        }

        _startR = Target.Color.R;
        _startG = Target.Color.G;
        _startB = Target.Color.B;
        _startA = Target.Color.A;
    }

    protected override void Update( float percent )
    {
        if ( percent == 0 )
        {
            Target?.Color.Set( _startR, _startG, _startB, _startA );
        }
        else if ( percent is 1.0f )
        {
            Target?.Color.Set( _endColor );
        }
        else
        {
            var r = _startR + ( ( _endColor.R - _startR ) * percent );
            var g = _startG + ( ( _endColor.G - _startG ) * percent );
            var b = _startB + ( ( _endColor.B - _startB ) * percent );
            var a = _startA + ( ( _endColor.A - _startA ) * percent );

            Target?.Color.Set( r, g, b, a );
        }
    }
}
