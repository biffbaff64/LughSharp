// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

namespace Corelib.Lugh.Scenes.Scene2D.Listeners;

/// <summary>
/// Detects mouse or finger touch drags on an actor. A touch must go down over the actor
/// and a drag won't start until it is moved outside the <see cref="TapSquareSize"/> tap
/// square. Any touch (not just the first) will trigger this listener. While pressed, other
/// touch downs are ignored.
/// </summary>
[PublicAPI]
public class DragListener : InputListener
{
    /// <summary>
    /// Sets the button to listen for, all other buttons are ignored.
    /// </summary>
    public int Button { get; set; } = IInput.Buttons.LEFT;

    /// <summary>
    /// Returns true if a touch has been dragged outside the tap square.
    /// </summary>
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

    private float _dragLastX;
    private float _dragLastY;
    private int   _pressedPointer = -1;

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( ev == null )
        {
            return false;
        }

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

    /// <inheritdoc />
    public override void TouchDragged( InputEvent? ev, float x, float y, int pointer )
    {
        if ( ev == null )
        {
            return;
        }

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

    /// <inheritdoc />
    public override void TouchUp( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( ev == null )
        {
            return;
        }

        if ( pointer == _pressedPointer )
        {
            if ( IsDragging )
            {
                DragStop( ev, x, y, pointer );
            }

            Cancel();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pointer"></param>
    public virtual void DragStart( InputEvent ev, float x, float y, int pointer )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pointer"></param>
    public virtual void Drag( InputEvent ev, float x, float y, int pointer )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="ev"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pointer"></param>
    public virtual void DragStop( InputEvent ev, float x, float y, int pointer )
    {
    }

    /// <summary>
    /// If a drag is in progress, no further drag methods will be
    /// called until a new drag is started.
    /// </summary>
    public virtual void Cancel()
    {
        IsDragging      = false;
        _pressedPointer = -1;
    }

    /// <summary>
    /// The distance from drag start to the current drag position.
    /// </summary>
    public float GetDragDistance()
    {
        return Vector2.Len( DragX - DragStartX, DragY - DragStartY );
    }

    /// <summary>
    /// Returns the amount on the x axis that the touch has been
    /// dragged since the last drag event.
    /// </summary>
    public float GetDeltaX()
    {
        return DragX - _dragLastX;
    }

    /// <summary>
    /// Returns the amount on the y axis that the touch has been
    /// dragged since the last drag event.
    /// </summary>
    public float GetDeltaY()
    {
        return DragY - _dragLastY;
    }
}
