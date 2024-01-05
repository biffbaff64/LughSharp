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

namespace LibGDXSharp.Scenes.Listeners;

public class DragScrollListener : DragListener
{
    private readonly static Vector2             TmpCoords      = new();
    private readonly        Interpolation.ExpIn _interpolation = Interpolation.Exp5In;
    private                 float               _maxSpeed      = 75;
    private                 float               _minSpeed      = 15;
    private                 float               _padBottom;
    private                 float               _padTop;
    private                 long                _rampTime = 1750;

    private readonly ScrollPane _scroll;
    private          long       _startTime;
    private          float      _tickSecs = 0.05f;

    public DragScrollListener( ScrollPane scroll ) => _scroll = scroll;

    public void Setup( float minSpeedPixels, float maxSpeedPixels, float tickSecs, float rampSecs )
    {
        _minSpeed = minSpeedPixels;
        _maxSpeed = maxSpeedPixels;
        _tickSecs = tickSecs;
        _rampTime = ( long )( rampSecs * 1000 );
    }

    private float GetScrollPixels() => _interpolation.Apply( _minSpeed,
                                                             _maxSpeed,
                                                             Math.Min( 1, ( TimeUtils.Millis() - _startTime ) / ( float )_rampTime ) );

    public override void Drag( InputEvent ev, float x, float y, int pointer )
    {
        ev.ListenerActor?.LocalToActorCoordinates( _scroll, TmpCoords.Set( x, y ) );

        if ( IsAbove( TmpCoords.Y ) )
        {
            // TODO: Add code for moving the scrollbar UP
        }
        else if ( IsBelow( TmpCoords.Y ) )
        {
            // TODO: Add code for moving the scrollbar DOWN
        }
    }

    public override void DragStop( InputEvent ev, float x, float y, int pointer )
    {
    }

    protected bool IsAbove( float y ) => y >= ( _scroll.Height - _padTop );

    protected bool IsBelow( float y ) => y < _padBottom;

    protected void Scroll( float y ) => _scroll.SetScrollY( y );

    public void SetPadding( float padtop, float padbottom )
    {
        _padTop    = padtop;
        _padBottom = padbottom;
    }
}
