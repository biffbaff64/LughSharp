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

using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Moves an actor from its current position to a specific position.
/// </summary>
public class MoveToAction : TemporalAction
{
    public float StartX    { get; set; }
    public float StartY    { get; set; }
    public float EndX      { get; set; }
    public float EndY      { get; set; }
    public int   Alignment { get; set; } = Align.BOTTOM_LEFT;

    protected override void Begin()
    {
        StartX = Target!.GetX( Alignment );
        StartY = Target.GetY( Alignment );
    }

    protected override void Update( float percent )
    {
        float x, y;

        switch ( percent )
        {
            case 0:
                x = StartX;
                y = StartY;

                break;

            case 1:
                x = EndX;
                y = EndY;

                break;

            default:
                x = StartX + ( ( EndX - StartX ) * percent );
                y = StartY + ( ( EndY - StartY ) * percent );

                break;
        }

        Target?.SetPosition( x, y, Alignment );
    }

    public override void Reset()
    {
        base.Reset();
        Alignment = Align.BOTTOM_LEFT;
    }

    public void SetStartPosition( float x, float y )
    {
        StartX = x;
        StartY = y;
    }

    public void SetPosition( float x, float y )
    {
        EndX = x;
        EndY = y;
    }

    public void SetPosition( float x, float y, int alignment )
    {
        EndX      = x;
        EndY      = y;
        Alignment = alignment;
    }
}
