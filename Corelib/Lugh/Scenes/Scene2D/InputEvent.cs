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

using Corelib.Lugh.Maths;

namespace Corelib.Lugh.Scenes.Scene2D;

[PublicAPI]
public class InputEvent : Event
{
    /// <summary>
    /// Types of low-level input events supported by scene2d.
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// A new touch for a pointer on the stage was detected
        /// </summary>
        TouchDown,

        /// <summary>
        /// A pointer has stopped touching the stage.
        /// </summary>
        TouchUp,

        /// <summary>
        /// A pointer that is touching the stage has moved.
        /// </summary>
        TouchDragged,

        /// <summary>
        /// The mouse pointer has moved (without a mouse button being active).
        /// </summary>
        MouseMoved,

        /// <summary>
        /// The mouse pointer or an active touch have entered
        /// (i.e., <see cref="Actor.Hit(float, float, bool)"/>) an actor.
        /// </summary>
        Enter,

        /// <summary>
        /// The mouse pointer or an active touch have exited an actor.
        /// </summary>
        Exit,

        /// <summary>
        /// The mouse scroll wheel has changed.
        /// </summary>
        Scrolled,

        /// <summary>
        /// A keyboard key has been pressed.
        /// </summary>
        KeyDown,

        /// <summary>
        /// A keyboard key has been released.
        /// </summary>
        KeyUp,

        /// <summary>
        /// A keyboard key has been pressed and released.
        /// </summary>
        KeyTyped,
    }

    /// <summary>
    /// The stage x coordinate where the event occurred. Valid for: TouchDown,
    /// TouchDragged, TouchUp, MouseMoved, Enter, and Exit.
    /// </summary>
    public float StageX { get; set; }

    /// <summary>
    /// The stage x coordinate where the event occurred. Valid for: TouchDown,
    /// TouchDragged, TouchUp, MouseMoved, Enter, and Exit.
    /// </summary>
    public float StageY { get; set; }

    /// <summary>
    /// The type of input event.
    /// </summary>
    public EventType? Type { get; set; }

    /// <summary>
    /// The pointer index for the event. The first touch is index 0, second touch is index 1, etc.
    /// Always -1 on desktop.
    /// Valid for: TouchDown, TouchDragged, TouchUp, Enter, and Exit.
    /// </summary>
    public int Pointer { get; set; }

    /// <summary>
    /// The index for the mouse button pressed. Always 0 on Android.
    /// Valid for: TouchDown and TouchUp.
    /// </summary>
    public int Button { get; set; }

    /// <summary>
    /// The key code of the key that was pressed. Valid for: keyDown and keyUp.
    /// </summary>
    public int KeyCode { get; set; }

    /// <summary>
    /// The character for the key that was type. Valid for: keyTyped.
    /// </summary>
    public char Character { get; set; }

    /// <summary>
    /// The amount the mouse was scrolled horizontally. Valid for: Scrolled.
    /// </summary>
    public float ScrollAmountX { get; set; }

    /// <summary>
    /// The amount the mouse was scrolled vertically. Valid for: scrolled.
    /// </summary>
    public float ScrollAmountY { get; set; }

    /// <summary>
    /// The actor related to the event. Valid for: Enter and Exit.
    /// For enter, this is the actor being exited, or null.
    /// For exit, this is the actor being entered, or null.
    /// </summary>
    public Actor? RelatedActor { get; set; }

    /// <summary>
    /// Returns true if this event is a touchUp triggered by <see cref="Stage.CancelTouchFocus()"/>.
    /// </summary>
    public bool TouchFocusCancel
        => MathUtils.IsEqual( StageX, int.MinValue )
        || MathUtils.IsEqual( StageY, int.MinValue );

    /// <summary>
    /// If false, <see cref="Event.SetHandled"/> will not add the listener
    /// to the stage's touch focus when a touch down event is handled.
    /// Default is true.
    /// </summary>
    public bool TouchFocus { get; set; } = true;

    /// <summary>
    /// Resets this event.
    /// </summary>
    public override void Reset()
    {
        base.Reset();

        RelatedActor = null;
        Button       = -1;
    }

    /// <summary>
    /// Sets actorCoords to this event's coordinates relative to the specified actor.
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="actorCoords"> Output for resulting coordinates.</param>
    public Vector2 ToCoordinates( Actor? actor, Vector2 actorCoords )
    {
        actorCoords.Set( StageX, StageY );
        actor?.StageToLocalCoordinates( actorCoords );

        return actorCoords;
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Type != null ? Type.ToString() : string.Empty;
    }
}
