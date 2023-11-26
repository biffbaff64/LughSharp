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

using System.Runtime.InteropServices.JavaScript;

using LibGDXSharp.Scenes.Scene2D;
using LibGDXSharp.Scenes.Scene2D.UI;

using Timer = System.Timers.Timer;

namespace LibGDXSharp.Scenes.Listeners;

[PublicAPI]
public class DragScrollListener : DragListener
{
    private readonly static Vector2 TmpCoords = new Vector2();

    private ScrollPane          _scroll;
    private Task                _scrollUp;
    private Task                _scrollDown;
    private Interpolation.ExpIn _interpolation = Interpolation.Exp5In;
    private float               _minSpeed      = 15;
    private float               _maxSpeed      = 75;
    private float               _tickSecs      = 0.05f;
    private long                _startTime;
    private long                _rampTime = 1750;
    private float               _padTop;
    private float               _padBottom;

    public DragScrollListener( ScrollPane scroll )
    {
        this._scroll = scroll;

        _scrollUp = Task.Run( () =>
        {
            Scroll( _scroll.getScrollY() - getScrollPixels());
        } );

        _scrollDown = Task.Run( () =>
        {
            Scroll( _scroll.GetScrollY() + GetScrollPixels() );
        } );
    }

    public void Setup( float minSpeedPixels, float maxSpeedPixels, float tickSecs, float rampSecs )
    {
        this._minSpeed = minSpeedPixels;
        this._maxSpeed = maxSpeedPixels;
        this._tickSecs = tickSecs;
        this._rampTime = ( long )( rampSecs * 1000 );
    }

    private float GetScrollPixels()
    {
        return _interpolation.Apply( _minSpeed,
                                     _maxSpeed,
                                     Math.Min( 1, ( TimeUtils.Millis() - _startTime ) / ( float )_rampTime ) );
    }

    public void Drag( InputEvent ev, float x, float y, int pointer )
    {
        ev.ListenerActor?.LocalToActorCoordinates( _scroll, TmpCoords.Set( x, y ) );

        if ( IsAbove( TmpCoords.Y ) )
        {
            _scrollDown.Cancel();

            if ( !_scrollUp.IsScheduled() )
            {
                _startTime = TimeUtils.Millis();
                Timer.Schedule( _scrollUp, _tickSecs, _tickSecs );
            }

            return;
        }
        else if ( IsBelow( TmpCoords.y ) )
        {
            _scrollUp.Cancel();

            if ( !_scrollDown.IsScheduled() )
            {
                _startTime = System.currentTimeMillis();
                Timer.schedule( scrollDown, tickSecs, tickSecs );
            }

            return;
        }

        _scrollUp.cancel();
        _scrollDown.cancel();
    }

    public void DragStop( InputEvent ev, float x, float y, int pointer )
    {
        _scrollUp.Cancel();
        _scrollDown.Cancel();
    }

    protected bool IsAbove( float y )
    {
        return y >= _scroll.GetHeight() - _padTop;
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
        this._padTop    = padtop;
        this._padBottom = padbottom;
    }
}
