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

using Corelib.Lugh.Graphics;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Sets the actor's color (or a specified color), from the current to the new
/// color. Note this action transitions from the color at the time the action
/// starts to the specified color.
/// </summary>
[PublicAPI]
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

    protected override void Begin()
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
