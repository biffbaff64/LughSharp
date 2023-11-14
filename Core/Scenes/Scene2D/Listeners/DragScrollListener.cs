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

using LibGDXSharp.Scenes.Scene2D;
using LibGDXSharp.Scenes.Scene2D.UI;

using Timer = System.Timers.Timer;

namespace LibGDXSharp.Scenes.Listeners;

[PublicAPI]
public class DragScrollListener : DragListener
{
    static final Vector2 tmpCoords = new Vector2();

    private ScrollPane    _scroll;
    private Task          _scrollUp;
    private Task          _scrollDown;
    private Interpolation _interpolation = Interpolation.Exp5In;
    private float         _minSpeed      = 15;
    private float         _maxSpeed      = 75;
    private float         _tickSecs      = 0.05f;
    private long          _startTime;
    private long          _rampTime = 1750;
    private float         _padTop;
    private float         _padBottom;

    public DragScrollListener( ScrollPane scroll )
    {
        this.scroll = scroll;

        scrollUp = new Task()
        {
 


            public void run () {
            scroll(scroll.getScrollY() - getScrollPixels());
        }

        };

        scrollDown = new Task()
        {
 


            public void run () {
            scroll(scroll.getScrollY() + getScrollPixels());
        }

        };
    }

    public void setup( float minSpeedPixels, float maxSpeedPixels, float tickSecs, float rampSecs )
    {
        this.minSpeed = minSpeedPixels;
        this.maxSpeed = maxSpeedPixels;
        this.tickSecs = tickSecs;
        rampTime      = ( long )( rampSecs * 1000 );
    }

    private float GetScrollPixels()
    {
        return interpolation.apply( minSpeed, maxSpeed, Math.min( 1, ( System.currentTimeMillis() - startTime ) / ( float )rampTime ) );
    }

    public void drag( InputEvent event, float x, float y, int pointer) {
        event.getListenerActor().localToActorCoordinates( scroll, tmpCoords.set( x, y ) );

        if ( isAbove( tmpCoords.y ) )
        {
            scrollDown.cancel();

            if ( !scrollUp.isScheduled() )
            {
                startTime = System.currentTimeMillis();
                Timer.schedule( scrollUp, tickSecs, tickSecs );
            }

            return;
        }
        else if ( isBelow( tmpCoords.y ) )
        {
            scrollUp.cancel();

            if ( !scrollDown.isScheduled() )
            {
                startTime = System.currentTimeMillis();
                Timer.schedule( scrollDown, tickSecs, tickSecs );
            }

            return;
        }

        scrollUp.cancel();
        scrollDown.cancel();
    }

    public void dragStop( InputEvent event, float x, float y, int pointer) {
        scrollUp.cancel();
        scrollDown.cancel();
    }

    protected boolean isAbove( float y )
    {
        return y >= scroll.getHeight() - padTop;
    }

    protected bool IsBelow( float y )
    {
        return y < padBottom;
    }

    protected void Scroll( float y )
    {
        scroll.setScrollY( y );
    }

    public void SetPadding( float padtop, float padbottom )
    {
        this.padTop    = padtop;
        this.padBottom = padbottom;
    }
}
