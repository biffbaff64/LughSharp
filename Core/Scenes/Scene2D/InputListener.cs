namespace LibGDXSharp.Scenes.Scene2D;

/// <summary>
/// EventListener for low-level input events. Unpacks <see cref="InputEvent"/>s
/// and calls the appropriate method. By default the methods here do nothing with
/// the event. Users are expected to override the methods they are interested in.
/// </summary>
public class InputListener : IEventListener
{
    private readonly static Vector2 tmpCoords = new Vector2();

    /// <summary>
    /// Try to handle the given event, if it is an <see cref="InputEvent"/>.
    /// <para>
    /// If the input event is of type <see cref="InputEvent.EventType.TouchDown"/>,
    /// <see cref="InputEvent.TouchFocus"/> is true and
    /// <see cref="TouchDown(InputEvent, float, float, int, int)"/> returns true
    /// ( indicating the event was handled ) then this listener is added to the
    /// stage's touch focus via:-
    /// <para>
    /// <tt>
    /// <see cref="Stage.AddTouchFocus(IEventListener, Actor, Actor, int, int)"/>
    /// </tt>
    /// </para>
    /// so it will receive all touch dragged events until the next touch up event. 
    /// </para>
    /// </summary>
    public virtual bool Handle( Event e )
    {
        if ( e is not InputEvent inputEvent )
        {
            return false;
        }

        switch ( inputEvent.Type )
        {
            case InputEvent.EventType.KeyDown:
                return KeyDown( inputEvent, inputEvent.KeyCode );

            case InputEvent.EventType.KeyUp:
                return KeyUp( inputEvent, inputEvent.KeyCode );

            case InputEvent.EventType.KeyTyped:
                return KeyTyped( inputEvent, inputEvent.Character );
            
            default:
                break;
        }

        inputEvent.ToCoordinates( inputEvent.ListenerActor, tmpCoords );

        switch ( inputEvent.Type )
        {
            case InputEvent.EventType.TouchDown:
                var handled = TouchDown( inputEvent, tmpCoords.X, tmpCoords.Y,
                                         inputEvent.Pointer, inputEvent.Button );

                if ( handled && inputEvent.TouchFocus )
                {
                    inputEvent.Stage?.AddTouchFocus( this, inputEvent.ListenerActor,
                                                    inputEvent.TargetActor, inputEvent.Pointer,
                                                    inputEvent.Button );
                }

                return handled;

            case InputEvent.EventType.TouchUp:
                TouchUp( inputEvent, tmpCoords.X, tmpCoords.Y,
                         inputEvent.Pointer, inputEvent.Button );

                return true;

            case InputEvent.EventType.TouchDragged:
                TouchDragged( inputEvent, tmpCoords.X, tmpCoords.Y, inputEvent.Pointer );

                return true;

            case InputEvent.EventType.MouseMoved:
                return MouseMoved( inputEvent, tmpCoords.X, tmpCoords.Y );

            case InputEvent.EventType.Scrolled:
                return Scrolled( inputEvent, tmpCoords.X, tmpCoords.Y,
                                 inputEvent.ScrollAmountX, inputEvent.ScrollAmountY );

            case InputEvent.EventType.Enter:
                Enter( inputEvent, tmpCoords.X, tmpCoords.Y, inputEvent.Pointer, inputEvent.RelatedActor );

                return false;

            case InputEvent.EventType.Exit:
                Exit( inputEvent, tmpCoords.X, tmpCoords.Y, inputEvent.Pointer, inputEvent.RelatedActor );

                return false;
            
            default:
                break;
        }

        return false;
    }

    /// <summary>
    /// Called when a mouse button or a finger touch goes down on the actor.
    /// If true is returned, this listener will have
    /// <see cref="Stage.AddTouchFocus(IEventListener, Actor, Actor, int, int)"/>,
    /// so it will receive all touchDragged and touchUp events, even those not
    /// over this actor, until touchUp is received. Also when true is returned,
    /// the event is handled by <see cref="Event.Handle()"/>.
    /// </summary>
    /// <see cref="InputEvent "/>
    protected bool TouchDown( InputEvent inputEvent, float x, float y, int pointer, int button )
    {
        return false;
    }

    /// <summary>
    /// Called when a mouse button or a finger touch goes up anywhere, but only
    /// if touchDown previously returned true for the mouse button or touch.
    /// The touchUp event is always handled by <see cref="Event.Handle()"/>.
    /// </summary>
    /// <see cref="InputEvent "/>
    protected void TouchUp( InputEvent inputEvent, float x, float y, int pointer, int button )
    {
    }

    /// <summary>
    /// Called when a mouse button or a finger touch is moved anywhere, but only
    /// if touchDown previously returned true for the mouse button or touch.
    /// The touchDragged event is always handled by <see cref="Event.Handle()"/>.
    /// </summary>
    /// <see cref="InputEvent "/>
    protected void TouchDragged( InputEvent inputEvent, float x, float y, int pointer )
    {
    }

    /// <summary>
    /// Called any time the mouse is moved when a button is not down. This event
    /// only occurs on the desktop. When true is returned, the event is handled
    /// by <see cref="Event.Handle()"/>.
    /// </summary>
    /// <see cref="InputEvent "/>
    protected bool MouseMoved( InputEvent inputEvent, float x, float y )
    {
        return false;
    }

    /// <summary>
    /// Called any time the mouse cursor or a finger touch is moved over an actor.
    /// On the desktop, this event occurs even when no mouse buttons are pressed
    /// (pointer will be -1).
    /// </summary>
    /// <param name="inputEvent"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pointer"></param>
    /// <param name="fromActor"> May be null. </param>
    /// <see cref="InputEvent "/>
    protected void Enter( InputEvent inputEvent, float x, float y, int pointer, Actor? fromActor )
    {
    }

    /// <summary>
    /// Called any time the mouse cursor or a finger touch is moved out of an actor.
    /// On the desktop, this event occurs even when no mouse buttons are pressed
    /// (pointer will be -1). </summary>
    /// <param name="inputEvent"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="pointer"></param>
    /// <param name="toActor"> May be null. </param>
    /// <see cref="InputEvent "/>
    protected void Exit( InputEvent inputEvent, float x, float y, int pointer, Actor? toActor )
    {
    }

    /// <summary>
    /// Called when the mouse wheel has been scrolled. When true is returned,
    /// the event is handled in <see cref="Event.Handle()"/>.
    /// </summary>
    protected bool Scrolled( InputEvent inputEvent, float x, float y, float amountX, float amountY )
    {
        return false;
    }

    /// <summary>
    /// Called when a key goes down. When true is returned, the event is
    /// handled by <see cref="Event.Handle()"/>.
    /// </summary>
    protected bool KeyDown( InputEvent inputEvent, int keycode )
    {
        return false;
    }

    /// <summary>
    /// Called when a key goes up. When true is returned, the event is
    /// handled by <see cref="Event.Handle()"/>.
    /// </summary>
    protected bool KeyUp( InputEvent inputEvent, int keycode )
    {
        return false;
    }

    /// <summary>
    /// Called when a key is typed. When true is returned, the event is
    /// handled by <see cref="Event.Handle()"/>.
    /// </summary>
    ///<param name="inputEvent"></param>
    /// <param name="character">
    /// May be 0 for key typed events that don't map to a character (ctrl, shift, etc).
    /// </param>
    protected bool KeyTyped( InputEvent inputEvent, char character )
    {
        return false;
    }
}
