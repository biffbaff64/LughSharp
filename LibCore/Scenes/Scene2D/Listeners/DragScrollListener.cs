// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using LughSharp.LibCore.Scenes.Scene2D.UI;

namespace LughSharp.LibCore.Scenes.Scene2D.Listeners;

[PublicAPI]
public class DragScrollListener : DragListener
{
    private readonly static Vector2             _tmpCoords     = new();
    private readonly        Interpolation.ExpIn _interpolation = Interpolation.Exp5In;

    private readonly ScrollPane _scroll;
    private          float      _maxSpeed = 75;
    private          float      _minSpeed = 15;
    private          float      _padBottom;
    private          float      _padTop;
    private          long       _rampTime = 1750;
    private          long       _startTime;
    private          float      _tickSecs = 0.05f;

    public DragScrollListener( ScrollPane scroll )
    {
        _scroll = scroll;
    }

    public void Setup( float minSpeedPixels, float maxSpeedPixels, float tickSecs, float rampSecs )
    {
        _minSpeed = minSpeedPixels;
        _maxSpeed = maxSpeedPixels;
        _tickSecs = tickSecs;
        _rampTime = ( long ) ( rampSecs * 1000 );
    }

    private float GetScrollPixels()
    {
        return _interpolation.Apply( _minSpeed,
                                     _maxSpeed,
                                     Math.Min( 1, ( TimeUtils.Millis() - _startTime ) / ( float ) _rampTime ) );
    }

    public override void Drag( InputEvent ev, float x, float y, int pointer )
    {
        ev.ListenerActor?.LocalToActorCoordinates( _scroll, _tmpCoords.Set( x, y ) );

        if ( IsAbove( _tmpCoords.Y ) )
        {
            // TODO: Add code for moving the scrollbar UP
        }
        else if ( IsBelow( _tmpCoords.Y ) )
        {
            // TODO: Add code for moving the scrollbar DOWN
        }
    }

    public override void DragStop( InputEvent ev, float x, float y, int pointer )
    {
    }

    protected bool IsAbove( float y )
    {
        return y >= ( _scroll.Height - _padTop );
    }

    protected bool IsBelow( float y )
    {
        return y < _padBottom;
    }

    protected void Scroll( float y )
    {
        _scroll.SetScrollY( y );
    }

    public void SetPadding( float padtop, float padbottom )
    {
        _padTop    = padtop;
        _padBottom = padbottom;
    }
}