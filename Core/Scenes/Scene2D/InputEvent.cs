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

using LibGDXSharp.Maths;

namespace LibGDXSharp.Scenes.Scene2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
[PublicAPI]
public class InputEvent : Event
{
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
    /// </summary>
    public new void Reset()
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

    /// <summary>
    /// Returns true if this event is a touchUp triggered by <see cref="Stage.CancelTouchFocus()"/>.
    /// </summary>
    public bool TouchFocusCancel => MathUtils.IsEqual( StageX, int.MinValue )
                                    || MathUtils.IsEqual( StageY, int.MinValue );

    /// <summary>
    /// If false, <see cref="Event.Handle()"/> will not add the listener
    /// to the stage's touch focus when a touch down event is handled.
    /// Default is true. 
    /// </summary>
    public bool TouchFocus { get; set; } = true;

    public override string? ToString()
    {
        return ( Type != null ) ? Type.ToString() : string.Empty;
    }

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
        /// The mouse pointer or an active touch have entered (i.e., <see cref="Actor.Hit(float, float, bool)"/>) an actor.
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
        KeyTyped
    }
}