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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Sets the alpha for an actor's color (or a specified color), from the current alpha
/// to the new alpha. Note this action transitions from the alpha at the time the action
/// starts to the specified alpha.
/// </summary>
[PublicAPI]
public class AlphaAction : TemporalAction
{
    private float _start;
    public  float Alpha { get; set; }

    // ========================================================================
    
    /// <inheritdoc />
    protected override void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot begin with a null Target!" );
        }

        _start = Target.Color.A;
    }

    /// <inheritdoc />
    protected override void Update( float percent )
    {
        if ( Target == null )
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
