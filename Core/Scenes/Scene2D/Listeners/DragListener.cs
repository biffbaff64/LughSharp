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

namespace LibGDXSharp.Scenes.Listeners;

/**
 * Detects mouse or finger touch drags on an actor. A touch must go down over the actor and a drag won't start until it is moved
 * outside the {@link #setTapSquareSize(float) tap square}. Any touch (not just the first) will trigger this listener. While
 * pressed, other touch downs are ignored.
 */
public class DragListener : InputListener
{

    private float _dragLastX;
    private float _dragLastY;
    private int   _pressedPointer = -1;

    /// Sets the button to listen for, all other buttons are ignored.
    public int Button { get; set; } = IInput.Buttons.LEFT;

    /// Returns true if a touch has been dragged outside the tap square.
    public bool IsDragging { get; private set; }

    public float DragStartX      { get; set; }
    public float DragStartY      { get; set; }
    public float TapSquareSize   { get; set; }         = 14;
    public float TouchDownX      { get; private set; } = -1;
    public float TouchDownY      { get; private set; } = -1;
    public float StageTouchDownX { get; private set; } = -1;
    public float StageTouchDownY { get; private set; } = -1;
    public float DragX           { get; private set; }
    public float DragY           { get; private set; }

    public override bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
    {
        if ( _pressedPointer != -1 )
        {
            return false;
        }

        if ( ( pointer == 0 ) && ( Button != -1 ) && ( button != Button ) )
        {
            return false;
        }

        _pressedPointer = pointer;
        TouchDownX      = x;
        TouchDownY      = y;
        StageTouchDownX = ev.StageX;
        StageTouchDownY = ev.StageY;

        return true;
    }

    public override void TouchDragged( InputEvent ev, float x, float y, int pointer )
    {
        if ( pointer != _pressedPointer )
        {
            return;
        }

        if ( !IsDragging
          && ( ( Math.Abs( TouchDownX - x ) > TapSquareSize )
            || ( Math.Abs( TouchDownY - y ) > TapSquareSize ) ) )
        {
            IsDragging = true;
            DragStartX = x;
            DragStartY = y;

            DragStart( ev, x, y, pointer );

            DragX = x;
            DragY = y;
        }

        if ( IsDragging )
        {
            _dragLastX = DragX;
            _dragLastY = DragY;
            DragX      = x;
            DragY      = y;

            Drag( ev, x, y, pointer );
        }
    }

    public override void TouchUp( InputEvent ev, float x, float y, int pointer, int button )
    {
        if ( pointer == _pressedPointer )
        {
            if ( IsDragging )
            {
                DragStop( ev, x, y, pointer );
            }

            Cancel();
        }
    }

    public virtual void DragStart( InputEvent ev, float x, float y, int pointer )
    {
    }

    public virtual void Drag( InputEvent ev, float x, float y, int pointer )
    {
    }

    public virtual void DragStop( InputEvent ev, float x, float y, int pointer )
    {
    }

    /// <summary>
    ///     If a drag is in progress, no further drag methods will be
    ///     called until a new drag is started.
    /// </summary>
    public virtual void Cancel()
    {
        IsDragging      = false;
        _pressedPointer = -1;
    }

    /// <summary>
    ///     The distance from drag start to the current drag position.
    /// </summary>
    public float GetDragDistance() => Vector2.Len( DragX - DragStartX, DragY - DragStartY );

    /// <summary>
    ///     Returns the amount on the x axis that the touch has been
    ///     dragged since the last drag event.
    /// </summary>
    public float GetDeltaX() => DragX - _dragLastX;

    /// <summary>
    ///     Returns the amount on the y axis that the touch has been
    ///     dragged since the last drag event.
    /// </summary>
    public float GetDeltaY() => DragY - _dragLastY;
}
