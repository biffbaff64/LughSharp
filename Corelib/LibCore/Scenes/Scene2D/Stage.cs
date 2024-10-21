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

using System.Reflection.Metadata;
using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.Cameras;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Graphics.Viewport;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.UI;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using Corelib.LibCore.Utils.Pooling;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;
using Platform = Corelib.LibCore.Core.Platform;

namespace Corelib.LibCore.Scenes.Scene2D;

/// <summary>
/// A 2D scene graph containing hierarchies of actors. Stage handles the viewport and
/// distributes input events. setViewport(Viewport) controls the coordinates used within
/// the stage and sets up the camera used to convert between stage coordinates and screen
/// coordinates.
/// A stage must receive input events so it can distribute them to actors. This is
/// typically done by passing the stage to Gdx.Input.SetInputProcessor.
/// An InputMultiplexer may be used to handle input events before or after the stage does.
/// If an actor handles an event by returning true from the input method, then the stage's
/// input method will also return true, causing subsequent InputProcessors to not receive
/// the event.
/// The Stage and its constituents (like Actors and Listeners) are not thread-safe and
/// should only be updated and queried from a single thread (presumably the main render
/// thread). Methods should be reentrant, so you can update Actors and Stages from within
/// callbacks and handlers.
/// </summary>
[PublicAPI]
public class Stage : InputAdapter, IDisposable
{
    public SnapshotArray< TouchFocus > TouchFocuses { get; }      = new( true, 4 );
    public Camera?                     Camera       { get; set; } = null!;
    public Viewport                    Viewport     { get; }
    public IBatch                      Batch        { get; }
    public bool                        Debug        { get; set; } // True if any actor has ever had debug enabled.

    private readonly bool     _ownsBatch;
    private readonly Actor?[] _pointerOverActors = new Actor?[ 20 ];
    private readonly int[]    _pointerScreenX    = new int[ 20 ];
    private readonly int[]    _pointerScreenY    = new int[ 20 ];
    private readonly bool[]   _pointerTouched    = new bool[ 20 ];
    private readonly Group    _root              = null!;
    private readonly Vector2  _tempCoords        = new();

    private bool            _debugAll;
    private bool            _debugParentUnderMouse;
    private ShapeRenderer?  _debugShapes;
    private Table.DebugType _debugTableUnderMouse = Table.DebugType.None;
    private bool            _debugUnderMouse;
    private Actor?          _keyboardFocus;
    private Actor?          _mouseOverActor;
    private int             _mouseScreenX;
    private int             _mouseScreenY;
    private Actor?          _scrollFocus;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a stage with a <see cref="ScalingViewport"/> set to
    /// <see cref="Scaling.Stretch"/>. The stage will use its own <see cref="IBatch"/>
    /// which will be disposed when the stage is disposed.
    /// </summary>
    public Stage()
        : this( new ScalingViewport( Scaling.Stretch,
                                     Gdx.Graphics.Width,
                                     Gdx.Graphics.Height,
                                     new OrthographicCamera() ), new SpriteBatch() )
    {
        _ownsBatch = true;
    }

    /// <summary>
    /// Creates a stage with the specified viewport. The stage will use its own
    /// <see cref="IBatch"/> which will be disposed when the stage is disposed.
    /// </summary>
    public Stage( Viewport viewport ) : this( viewport, new SpriteBatch() )
    {
        _ownsBatch = true;
    }

    /// <summary>
    /// Creates a stage with the specified <see cref="Viewport"/> and <see cref="IBatch"/>.
    /// This can be used to specify an existing batch or to customize which batch implementation
    /// is used.
    /// </summary>
    /// <param name="viewport"> The viewport for this stage. </param>
    /// <param name="batch">
    /// Will not be disposed if <see cref="Dispose()"/> is called,
    /// handle disposal yourself.
    /// </param>
    public Stage( Viewport? viewport, IBatch? batch )
    {
        Viewport = viewport ?? throw new ArgumentException( "viewport cannot be null." );
        Batch    = batch ?? throw new ArgumentException( "batch cannot be null." );

        Root = new Group();
        Root.SetStage( this );

        viewport.Update( Gdx.Graphics.Width, Gdx.Graphics.Height, true );
    }

    public float Width  => Viewport.WorldWidth;
    public float Height => Viewport.WorldHeight;

    /// <summary>
    /// Sets the actor that will receive key events.
    /// </summary>
    /// <param name="value"> May be null. </param>
    /// <returns>
    /// true if the unfocus and focus events were not cancelled by a <see cref="FocusListener"/>.
    /// </returns>
    public Actor? KeyboardFocus
    {
        get => _keyboardFocus;
        set
        {
            if ( _keyboardFocus == value )
            {
                return;
            }

            var focusEvent = Pools< FocusListener.FocusEvent >.Obtain();

            if ( focusEvent == null )
            {
                return;
            }

            focusEvent.Stage = this;
            focusEvent.Type  = FocusListener.FocusEvent.FeType.Keyboard;

            var oldKeyboardFocus = _keyboardFocus;

            if ( oldKeyboardFocus != null )
            {
                focusEvent.Focused      = false;
                focusEvent.RelatedActor = value;

                oldKeyboardFocus.Fire( focusEvent );
            }

            var success = !focusEvent.IsCancelled;

            if ( success )
            {
                _keyboardFocus = value;

                if ( value != null )
                {
                    focusEvent.Focused      = true;
                    focusEvent.RelatedActor = oldKeyboardFocus;

                    value.Fire( focusEvent );
                    success = !focusEvent.IsCancelled;

                    if ( !success )
                    {
                        _keyboardFocus = oldKeyboardFocus;
                    }
                }
            }

            Pools< FocusListener.FocusEvent >.Free( focusEvent );
        }
    }

    /// <summary>
    /// Sets the actor that will receive scroll events.
    /// </summary>
    /// <param name="value"> May be null. </param>
    /// <returns>
    /// true if the unfocus and focus events were not cancelled
    /// by a <see cref="FocusListener"/>.
    /// </returns>
    public Actor? ScrollFocus
    {
        get => _scrollFocus;
        set
        {
            if ( _scrollFocus == value )
            {
                return;
            }

            var focusEvent = Pools< FocusListener.FocusEvent >.Obtain();

            if ( focusEvent == null )
            {
                return;
            }

            focusEvent.Stage = this;
            focusEvent.Type  = FocusListener.FocusEvent.FeType.Scroll;

            var oldScrollFocus = ScrollFocus;

            if ( oldScrollFocus != null )
            {
                focusEvent.Focused      = false;
                focusEvent.RelatedActor = value;
                oldScrollFocus.Fire( focusEvent );
            }

            var success = !focusEvent.IsCancelled;

            if ( success )
            {
                _scrollFocus = value;

                if ( value != null )
                {
                    focusEvent.Focused      = true;
                    focusEvent.RelatedActor = oldScrollFocus;
                    value.Fire( focusEvent );

                    success = !focusEvent.IsCancelled;

                    if ( !success )
                    {
                        _scrollFocus = oldScrollFocus;
                    }
                }
            }

            Pools< FocusListener.FocusEvent >.Free( focusEvent );
        }
    }

    /// <summary>
    /// Returns the root group which holds all actors in the stage.
    /// </summary>
    public Group Root
    {
        get => _root;
        private init
        {
            value.Parent?.RemoveActor( value, false );

            _root = value;

            value.Parent = null;
            value.Stage  = this;
        }
    }

    /// <summary>
    /// Returns the root's child actors.
    /// </summary>
    /// <see cref="Group.Children "/>
    public SnapshotArray< Actor > Actors => Root.Children;

    /// <summary>
    /// If true, any actions executed during a call to <see cref="Act()"/>)
    /// will result in a call to <see cref="IGraphics.RequestRendering()"/>.
    /// Widgets that animate or otherwise require additional rendering may check
    /// this setting before calling <see cref="IGraphics.RequestRendering()"/>.
    /// Default is true.
    /// </summary>
    public bool ActionsRequestRendering { get; set; } = true;

    /// <summary>
    /// The default color that can be used by actors to draw debug lines.
    /// </summary>
    public Color DebugColor { get; } = new( 0, 1, 0, 0.85f );

    /// <summary>
    /// If true, debug lines are shown for actors even when
    /// <see cref="Actor.IsVisible"/> is false.
    /// </summary>
    public bool DebugInvisibleActors { get; set; }

    /// <summary>
    /// If true, debug lines are shown for all actors.
    /// </summary>
    public bool DebugAll
    {
        get => _debugAll;
        set
        {
            if ( _debugAll == value )
            {
                return;
            }

            _debugAll = value;

            if ( value )
            {
                Debug = true;
            }
            else
            {
                Root.SetDebug( false, true );
            }
        }
    }

    /// <summary>
    /// If true, debug is enabled only for the actor under the mouse.
    /// Can be combined with <see cref="DebugAll"/>.
    /// </summary>
    public bool DebugUnderMouse
    {
        set
        {
            if ( _debugUnderMouse == value )
            {
                return;
            }

            _debugUnderMouse = value;

            if ( value )
            {
                Debug = true;
            }
            else
            {
                Root.SetDebug( false, true );
            }
        }
    }

    /// <summary>
    /// If true, debug is enabled only for the parent of the actor under
    /// the mouse. Can be combined with <see cref="DebugAll"/>.
    /// </summary>
    public bool DebugParentUnderMouse
    {
        set
        {
            if ( _debugParentUnderMouse == value )
            {
                return;
            }

            _debugParentUnderMouse = value;

            if ( value )
            {
                Debug = true;
            }
            else
            {
                Root.SetDebug( false, true );
            }
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Clear();

        if ( _ownsBatch )
        {
            Batch.Dispose();
        }

        _debugShapes?.Dispose();
    }

    /// <summary>
    /// Draw the stage. If the Viewport camera has not been set, or the
    /// root <see cref="Group"/> is invisible, the stage will not draw.
    /// </summary>
    public virtual void Draw()
    {
        Camera = Viewport.Camera;

        if ( ( Camera == null ) || !Root.IsVisible )
        {
            return;
        }

        Camera.Update();

        Batch.SetProjectionMatrix( Camera.Combined );
        Batch.Begin();
        Root.Draw( Batch, 1 );
        Batch.End();

        if ( Debug )
        {
            DrawDebug();
        }
    }

    /// <summary>
    /// Calls <see cref="Act(float)"/> with Gdx.Graphics.GetDeltaTime(),
    /// limited to a minimum of 30fps.
    /// </summary>
    public virtual void Act()
    {
        Act( Math.Min( Gdx.Graphics.DeltaTime, 1 / 30f ) );
    }

    /// <summary>
    /// Calls the <see cref="Actor.Act(float)"/> method on each actor in the
    /// stage. Typically called each frame. This method also fires enter and exit
    /// events.
    /// </summary>
    /// <param name="delta"> Time in seconds since the last frame.</param>
    public virtual void Act( float delta )
    {
        // Update over actors. Done in act() because actors may change position,
        // which can fire enter/exit without an input event.
        for ( int pointer = 0, n = _pointerOverActors.Length; pointer < n; pointer++ )
        {
            var overLast = _pointerOverActors[ pointer ];

            // Check if pointer is gone.
            if ( !_pointerTouched[ pointer ] )
            {
                if ( overLast != null )
                {
                    _pointerOverActors[ pointer ] = null;

                    ScreenToStageCoordinates( _tempCoords.Set( _pointerScreenX[ pointer ],
                                                               _pointerScreenY[ pointer ] ) );

                    // Exit over last.
                    var inputEvent = Pools< InputEvent >.Obtain();

                    if ( inputEvent == null )
                    {
                        continue;
                    }

                    inputEvent.Type         = InputEvent.EventType.Exit;
                    inputEvent.Stage        = this;
                    inputEvent.StageX       = _tempCoords.X;
                    inputEvent.StageY       = _tempCoords.Y;
                    inputEvent.RelatedActor = overLast;
                    inputEvent.Pointer      = pointer;

                    overLast.Fire( inputEvent );

                    Pools< InputEvent >.Free( inputEvent );
                }

                continue;
            }

            // Update over actor for the pointer.
            _pointerOverActors[ pointer ] = FireEnterAndExit( overLast,
                                                              _pointerScreenX[ pointer ],
                                                              _pointerScreenY[ pointer ],
                                                              pointer );
        }

        // Update over actor for the mouse on the desktop.
        var type = Gdx.App.AppType;

        if ( type is Platform.ApplicationType.WindowsGL or Platform.ApplicationType.WebGL )
        {
            _mouseOverActor = FireEnterAndExit( _mouseOverActor, _mouseScreenX, _mouseScreenY, -1 );
        }

        Root.Act( delta );
    }

    /// <summary>
    /// </summary>
    /// <param name="overLast"></param>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="pointer"></param>
    /// <returns></returns>
    private Actor? FireEnterAndExit( Actor? overLast, int screenX, int screenY, int pointer )
    {
        // Find the actor under the point.
        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        var over = Hit( _tempCoords.X, _tempCoords.Y, true );

        if ( over == overLast )
        {
            return overLast;
        }

        // Exit overLast.
        if ( overLast != null )
        {
            var inputEvent = Pools< InputEvent >.Obtain();

            if ( inputEvent != null )
            {
                inputEvent.Stage        = this;
                inputEvent.StageX       = _tempCoords.Y;
                inputEvent.StageY       = _tempCoords.Y;
                inputEvent.Pointer      = pointer;
                inputEvent.Type         = InputEvent.EventType.Exit;
                inputEvent.RelatedActor = over;

                overLast.Fire( inputEvent );
                Pools< InputEvent >.Free( inputEvent );
            }
        }

        // Enter over.
        if ( over != null )
        {
            var inputEvent = Pools< InputEvent >.Obtain();

            if ( inputEvent != null )
            {
                inputEvent.Stage        = this;
                inputEvent.StageX       = _tempCoords.X;
                inputEvent.StageY       = _tempCoords.Y;
                inputEvent.Pointer      = pointer;
                inputEvent.Type         = InputEvent.EventType.Enter;
                inputEvent.RelatedActor = overLast;

                over.Fire( inputEvent );
                Pools< InputEvent >.Free( inputEvent );
            }
        }

        return over;
    }

    /// <summary>
    /// Applies a touch down event to the stage and returns true if an actor in
    /// the scene <see cref="Handle"/> the event.
    /// </summary>
    public override bool TouchDown( int screenX, int screenY, int pointer, int button )
    {
        if ( !IsInsideViewport( screenX, screenY ) )
        {
            return false;
        }

        _pointerTouched[ pointer ] = true;
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            throw new GdxRuntimeException( "Null InputEvent for TouchDown!" );
        }

        inputEvent.Type    = InputEvent.EventType.TouchDown;
        inputEvent.Stage   = this;
        inputEvent.StageX  = _tempCoords.X;
        inputEvent.StageY  = _tempCoords.Y;
        inputEvent.Pointer = pointer;
        inputEvent.Button  = button;

        var target = Hit( _tempCoords.X, _tempCoords.Y, true );

        if ( target == null )
        {
            if ( Root.Touchable == Touchable.Enabled )
            {
                Root.Fire( inputEvent );
            }
        }
        else
        {
            target.Fire( inputEvent );
        }

        var handled = inputEvent.IsHandled;

        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a touch moved event to the stage and returns true if an actor in the scene
    /// <see cref="Event.SetHandled"/> handled the event. Only <see cref="InputListener"/>
    /// listeners that returned true for touchDown will receive this event.
    /// </summary>
    public override bool TouchDragged( int screenX, int screenY, int pointer )
    {
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;
        _mouseScreenX              = screenX;
        _mouseScreenY              = screenY;

        if ( TouchFocuses.Size == 0 )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            throw new GdxRuntimeException( "Null InputEvent for TouchDragged!" );
        }

        inputEvent.Type    = InputEvent.EventType.TouchDragged;
        inputEvent.Stage   = this;
        inputEvent.StageX  = _tempCoords.X;
        inputEvent.StageY  = _tempCoords.X;
        inputEvent.Pointer = pointer;

        TouchFocus?[] focuses = TouchFocuses.Begin();

        for ( int i = 0, n = TouchFocuses.Size; i < n; i++ )
        {
            var focus = focuses[ i ];

            if ( focus?.Pointer != pointer )
            {
                continue;
            }

            if ( !TouchFocuses.Contains( focus ) )
            {
                // Touch focus already gone.
                continue;
            }

            inputEvent.TargetActor   = focus.Target;
            inputEvent.ListenerActor = focus.ListenerActor;

            if ( ( focus.Listener != null ) && focus.Listener.Handle( inputEvent ) )
            {
                inputEvent.SetHandled();
            }
        }

        TouchFocuses.End();

        var handled = inputEvent.IsHandled;

        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a touch up event to the stage and returns true if an actor in the
    /// scene <see cref="Event.SetHandled"/> handled the event.
    /// Only <see cref="InputListener"/> listeners that returned true for
    /// touchDown will receive this event.
    /// </summary>
    public override bool TouchUp( int screenX, int screenY, int pointer, int button )
    {
        _pointerTouched[ pointer ] = false;
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;

        if ( TouchFocuses.Size == 0 )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Type    = InputEvent.EventType.TouchUp;
        inputEvent.Stage   = this;
        inputEvent.StageX  = _tempCoords.X;
        inputEvent.StageY  = _tempCoords.Y;
        inputEvent.Pointer = pointer;
        inputEvent.Button  = button;

        TouchFocus?[] focuses = TouchFocuses.Begin();

        for ( int i = 0, n = TouchFocuses.Size; i < n; i++ )
        {
            var focus = focuses[ i ];

            if ( ( focus?.Pointer != pointer ) || ( focus.Button != button ) )
            {
                continue;
            }

            if ( !TouchFocuses.Remove( focus ) )
            {
                // Touch focus already gone.
                continue;
            }

            inputEvent.TargetActor   = focus.Target;
            inputEvent.ListenerActor = focus.ListenerActor;

            if ( ( focus.Listener != null ) && focus.Listener.Handle( inputEvent ) )
            {
                inputEvent.SetHandled();
            }

            Pools< TouchFocus >.Free( focus );
        }

        TouchFocuses.End();

        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a mouse moved event to the stage and returns true if an actor
    /// in the scene <see cref="Event.SetHandled"/> the event. This event only
    /// occurs on the desktop.
    /// </summary>
    public override bool MouseMoved( int screenX, int screenY )
    {
        _mouseScreenX = screenX;
        _mouseScreenY = screenY;

        if ( !IsInsideViewport( screenX, screenY ) )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Stage  = this;
        inputEvent.Type   = InputEvent.EventType.MouseMoved;
        inputEvent.StageX = _tempCoords.X;
        inputEvent.StageY = _tempCoords.Y;

        Actor? target;

        if ( ( target = Hit( _tempCoords.X, _tempCoords.Y, true ) ) == null )
        {
            target = Root;
        }

        target.Fire( inputEvent );
        var handled = inputEvent.IsHandled;

        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a mouse scroll event to the stage and returns true if an actor
    /// in the scene <see cref="Event.SetHandled"/> the event. This event only
    /// occurs on the desktop.
    /// </summary>
    public override bool Scrolled( float amountX, float amountY )
    {
        var target = ScrollFocus ?? Root;

        ScreenToStageCoordinates( _tempCoords.Set( _mouseScreenX, _mouseScreenY ) );

        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Stage         = this;
        inputEvent.Type          = InputEvent.EventType.Scrolled;
        inputEvent.ScrollAmountX = amountX;
        inputEvent.ScrollAmountY = amountY;
        inputEvent.StageX        = _tempCoords.X;
        inputEvent.StageY        = _tempCoords.Y;

        target.Fire( inputEvent );
        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a key down event to the actor that has
    /// <see cref="Stage.KeyboardFocus"/>, if any, and returns
    /// true if the event was handled in <see cref="Event.SetHandled"/>.
    /// </summary>
    public override bool KeyDown( int keyCode )
    {
        var target     = _keyboardFocus ?? Root;
        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Stage   = this;
        inputEvent.Type    = InputEvent.EventType.KeyDown;
        inputEvent.KeyCode = keyCode;

        target.Fire( inputEvent );
        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a key up event to the actor that has <see cref="Stage.KeyboardFocus"/>,
    /// if any, and returns true if the event was <see cref="Event.SetHandled"/>.
    /// </summary>
    public override bool KeyUp( int keyCode )
    {
        var target     = _keyboardFocus ?? Root;
        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Stage   = this;
        inputEvent.Type    = InputEvent.EventType.KeyUp;
        inputEvent.KeyCode = keyCode;

        target.Fire( inputEvent );
        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a key typed event to the actor that has <see cref="Stage.KeyboardFocus"/>,
    /// if any, and returns true if the event was <see cref="Event.SetHandled"/>.
    /// </summary>
    public override bool KeyTyped( char character )
    {
        var target     = _keyboardFocus ?? Root;
        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return false;
        }

        inputEvent.Stage     = this;
        inputEvent.Type      = InputEvent.EventType.KeyTyped;
        inputEvent.Character = character;

        target.Fire( inputEvent );
        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Adds the listener to be notified for all touchDragged and touchUp events for
    /// the specified pointer and button. Touch focus is added automatically when true
    /// is returned from <see cref="InputListener.TouchDown(InputEvent, float, float, int, int)"/>
    /// The specified actors will be used as the <see cref="Event.ListenerActor"/> and
    /// <see cref="Event.TargetActor"/> for the touchDragged and touchUp events.
    /// </summary>
    public void AddTouchFocus( IEventListener listener,
                               Actor? listenerActor,
                               Actor? target,
                               int pointer,
                               int button )
    {
        var focus = Pools< TouchFocus >.Obtain();

        if ( focus == null )
        {
            return;
        }

        focus.ListenerActor = listenerActor;
        focus.Target        = target;
        focus.Listener      = listener;
        focus.Pointer       = pointer;
        focus.Button        = button;

        TouchFocuses.Add( focus );
    }

    /// <summary>
    /// Removes touch focus for the specified listener, pointer, and button.
    /// Note the listener will not receive a touchUp event when this method
    /// is used.
    /// </summary>
    public void RemoveTouchFocus( IEventListener listener,
                                  Actor listenerActor,
                                  Actor target,
                                  int pointer,
                                  int button )
    {
        for ( var i = TouchFocuses.Size - 1; i >= 0; i-- )
        {
            var focus = TouchFocuses.GetAt( i );

            if ( ( focus.Listener == listener )
              && ( focus.ListenerActor == listenerActor )
              && ( focus.Target == target )
              && ( focus.Pointer == pointer )
              && ( focus.Button == button ) )
            {
                TouchFocuses.RemoveAt( i );
                Pools< TouchFocus >.Free( focus );
            }
        }
    }

    /// <summary>
    /// Cancels touch focus for all listeners with the specified listener actor.
    /// </summary>
    /// <see cref="CancelTouchFocus() "/>
    public void CancelTouchFocus( Actor listenerActor )
    {
        // Cancel all current touch focuses for the specified listener, allowing
        // for concurrent modification, and never cancel the same focus twice.
        InputEvent?   inputEvent = null;
        TouchFocus?[] items      = TouchFocuses.Begin();

        for ( int i = 0, n = TouchFocuses.Size; i < n; i++ )
        {
            var focus = items[ i ];

            if ( focus?.ListenerActor != listenerActor )
            {
                continue;
            }

            if ( !TouchFocuses.Remove( focus ) )
            {
                continue; // Touch focus already gone.
            }

            if ( inputEvent == null )
            {
                inputEvent = Pools< InputEvent >.Obtain();

                if ( inputEvent == null )
                {
                    continue;
                }

                inputEvent.Stage  = this;
                inputEvent.Type   = InputEvent.EventType.TouchUp;
                inputEvent.StageX = int.MinValue;
                inputEvent.StageY = int.MinValue;
            }

            inputEvent.TargetActor   = focus.Target;
            inputEvent.ListenerActor = focus.ListenerActor;
            inputEvent.Pointer       = focus.Pointer;
            inputEvent.Button        = focus.Button;

            focus.Listener?.Handle( inputEvent );

            // Cannot return TouchFocus to pool, as it may still be in use
            // (eg if cancelTouchFocus is called from touchDragged).
        }

        TouchFocuses.End();

        if ( inputEvent != null )
        {
            Pools< InputEvent >.Free( inputEvent );
        }
    }

    /// <summary>
    /// Removes all touch focus listeners, sending a touchUp event to each listener.
    /// Listeners typically expect to receive a touchUp event when they have touch
    /// focus. The location of the touchUp is <see cref="int.MinValue"/>. Listeners can use
    /// <see cref="InputEvent.TouchFocusCancel()"/> to ignore this event if needed.
    /// </summary>
    public void CancelTouchFocus()
    {
        CancelTouchFocusExcept( null, null );
    }

    /// <summary>
    /// Cancels touch focus for all listeners except the specified listener.
    /// </summary>
    /// <see cref="CancelTouchFocus() "/>
    public void CancelTouchFocusExcept( IEventListener? exceptListener, Actor? exceptActor )
    {
        var inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            return;
        }

        inputEvent.Stage  = this;
        inputEvent.Type   = InputEvent.EventType.TouchUp;
        inputEvent.StageX = int.MinValue;
        inputEvent.StageY = int.MinValue;

        // Cancel all current touch focuses except for the specified listener,
        // allowing for concurrent modification, and never cancel the same focus twice.
        TouchFocus?[] items = TouchFocuses.Begin();

        for ( int i = 0, n = TouchFocuses.Size; i < n; i++ )
        {
            var focus = items[ i ];

            if ( ( focus?.Listener == exceptListener )
              && ( focus?.ListenerActor == exceptActor ) )
            {
                continue;
            }

            if ( focus != null )
            {
                if ( !TouchFocuses.Remove( focus ) )
                {
                    continue; // Touch focus already gone.
                }
            }

            inputEvent.TargetActor   = focus?.Target;
            inputEvent.ListenerActor = focus?.ListenerActor;
            inputEvent.Pointer       = focus!.Pointer;
            inputEvent.Button        = focus.Button;

            focus.Listener?.Handle( inputEvent );

            // Cannot return TouchFocus to pool, as it may still be in use
            // (eg if cancelTouchFocus is called from touchDragged).
        }

        TouchFocuses.End();

        Pools< InputEvent >.Free( inputEvent );
    }

    /// <summary>
    /// Adds an actor to the root of the stage.
    /// </summary>
    /// <see cref="Group.AddActor "/>
    public virtual void AddActor( Actor actor )
    {
        Root.AddActor( actor );
    }

    /// <summary>
    /// Adds an action to the root of the stage.
    /// </summary>
    /// <see cref="Group.AddAction(Action) "/>
    public virtual void AddAction( Action action )
    {
        Root.AddAction( action );
    }

    /// <summary>
    /// Adds a listener to the root.
    /// </summary>
    /// <see cref="Actor.AddListener(IEventListener) "/>
    public bool AddListener( IEventListener listener )
    {
        return Root.AddListener( listener );
    }

    /// <summary>
    /// Removes a listener from the root.
    /// </summary>
    /// <see cref="Actor.RemoveListener(IEventListener) "/>
    public bool RemoveListener( IEventListener listener )
    {
        return Root.RemoveListener( listener );
    }

    /// <summary>
    /// Adds a capture listener to the root.
    /// </summary>
    /// <see cref="Actor.AddCaptureListener(IEventListener) "/>
    public bool AddCaptureListener( IEventListener listener )
    {
        return Root.AddCaptureListener( listener );
    }

    /// <summary>
    /// Removes a listener from the root.
    /// </summary>
    /// <see cref="Actor.RemoveCaptureListener(IEventListener) "/>
    public bool RemoveCaptureListener( IEventListener listener )
    {
        return Root.RemoveCaptureListener( listener );
    }

    /// <summary>
    /// Removes the root's children, actions, and listeners.
    /// </summary>
    public virtual void Clear()
    {
        UnfocusAll();
        Root.Clear();
    }

    /// <summary>
    /// Removes the touch, keyboard, and scroll focused actors.
    /// </summary>
    public virtual void UnfocusAll()
    {
        ScrollFocus   = null;
        KeyboardFocus = null;
        CancelTouchFocus();
    }

    /// <summary>
    /// Removes the touch, keyboard, and scroll focus for the specified
    /// actor and any descendants.
    /// </summary>
    public virtual void Unfocus( Actor actor )
    {
        CancelTouchFocus( actor );

        if ( ( ScrollFocus != null ) && ScrollFocus.IsDescendantOf( actor ) )
        {
            ScrollFocus = null;
        }

        if ( ( _keyboardFocus != null ) && _keyboardFocus.IsDescendantOf( actor ) )
        {
            KeyboardFocus = null;
        }
    }

    /// <summary>
    /// Returns the <see cref="Actor"/> at the specified location in stage coordinates.
    /// Hit testing is performed in the order the actors were inserted into the stage, last
    /// inserted actors being tested first. To get stage coordinates from screen coordinates,
    /// use <see cref="ScreenToStageCoordinates(Vector2)"/>.
    /// </summary>
    /// <param name="stageX"> X Coordinate of hit. </param>
    /// <param name="stageY"> Y Coordinate of hit. </param>
    /// <param name="touchable">
    /// If true, the hit detection will respect the <see cref="Actor.Touchable"/>.
    /// </param>
    /// <returns> May be null if no actor was hit.  </returns>
    public virtual Actor? Hit( float stageX, float stageY, bool touchable )
    {
        Root.ParentToLocalCoordinates( _tempCoords.Set( stageX, stageY ) );

        return Root.Hit( _tempCoords.X, _tempCoords.Y, touchable );
    }

    /// <summary>
    /// Transforms the screen coordinates to stage coordinates.
    /// </summary>
    /// <param name="screenCoords">
    /// Input screen coordinates and output for resulting stage coordinates.
    /// </param>
    public virtual Vector2 ScreenToStageCoordinates( Vector2 screenCoords )
    {
        Viewport.Unproject( screenCoords );

        return screenCoords;
    }

    /// <summary>
    /// Transforms the stage coordinates to screen coordinates.
    /// </summary>
    /// <param name="stageCoords">
    /// Input stage coordinates and output for resulting screen coordinates.
    /// </param>
    public virtual Vector2 StageToScreenCoordinates( Vector2 stageCoords )
    {
        Viewport.Project( stageCoords );
        stageCoords.Y = Gdx.Graphics.Height - stageCoords.Y;

        return stageCoords;
    }

    /// <summary>
    /// Transforms the coordinates to screen coordinates. The coordinates can be
    /// anywhere in the stage since the transform matrix describes how to convert
    /// them.
    /// The transform matrix is typically obtained from <see cref="IBatch.TransformMatrix"/>
    /// during <see cref="Actor.Draw(IBatch, float)"/>.
    /// </summary>
    /// <see cref="Actor.LocalToStageCoordinates(Vector2)"/>
    public virtual Vector2 ToScreenCoordinates( Vector2 coords, Matrix4 transformMatrix )
    {
        return Viewport.ToScreenCoordinates( coords, transformMatrix );
    }

    /// <summary>
    /// Calculates window scissor coordinates from local coordinates using the
    /// batch's current transformation matrix.
    /// </summary>
    public virtual void CalculateScissors( RectangleShape localRect, RectangleShape scissorRect )
    {
        Matrix4 transformMatrix;

        if ( ( _debugShapes != null ) && _debugShapes.IsDrawing() )
        {
            transformMatrix = _debugShapes.TransformMatrix;
        }
        else
        {
            transformMatrix = Batch.TransformMatrix;
        }

        Viewport.CalculateScissors( transformMatrix, localRect, scissorRect );
    }

    /// <summary>
    /// If not <see cref="Table.DebugType.None"/>, debug is enabled only for the first
    /// ascendant of the actor under the mouse that is a table.
    /// Can be combined with <see cref="DebugAll"/>.
    /// </summary>
    /// <param name="debugTableUnderMouse">May be null for <see cref="Table.DebugType.None"/>.</param>
    public virtual void SetDebugTableUnderMouse( Table.DebugType debugTableUnderMouse )
    {
        if ( Enum.IsDefined( typeof( Table.DebugType ), debugTableUnderMouse ) )
        {
            _debugTableUnderMouse = Table.DebugType.None;
        }

        if ( _debugTableUnderMouse == debugTableUnderMouse )
        {
            return;
        }

        _debugTableUnderMouse = debugTableUnderMouse;

        if ( debugTableUnderMouse != Table.DebugType.None )
        {
            Debug = true;
        }
        else
        {
            Root.SetDebug( false, true );
        }
    }

    /// <summary>
    /// If true, debug is enabled only for the first ascendant of the actor
    /// under the mouse that is a table.
    /// Can be combined with <see cref="DebugAll"/>.
    /// </summary>
    public virtual void SetDebugTableUnderMouse( bool debugTableUnderMouse )
    {
        SetDebugTableUnderMouse( debugTableUnderMouse ? Table.DebugType.All : Table.DebugType.None );
    }

    /// <summary>
    /// Draws the debug shapes for all actors on this stage.
    /// </summary>
    private void DrawDebug()
    {
        if ( _debugShapes == null )
        {
            _debugShapes = new ShapeRenderer();
            _debugShapes.SetAutoShapeType( true );
        }

        if ( _debugUnderMouse || _debugParentUnderMouse || ( _debugTableUnderMouse != Table.DebugType.None ) )
        {
            ScreenToStageCoordinates( _tempCoords.Set( Gdx.Input.GetX(), Gdx.Input.GetY() ) );

            var actor = Hit( _tempCoords.X, _tempCoords.Y, true );

            if ( actor == null )
            {
                return;
            }

            if ( _debugParentUnderMouse && ( actor.Parent != null ) )
            {
                actor = actor.Parent;
            }

            if ( _debugTableUnderMouse == Table.DebugType.None )
            {
                actor.DebugActive = true;
            }
            else
            {
                // Move up the 'family tree' to find which actor is the Table.
                while ( actor != null )
                {
                    if ( actor is Table )
                    {
                        break;
                    }

                    actor = actor.Parent;
                }

                if ( actor == null )
                {
                    return;
                }

                ( ( Table ) actor ).DebugLines( _debugTableUnderMouse );
            }

            if ( _debugAll && actor is Group group )
            {
                group.DebugAll();
            }

            DisableDebug( Root, actor );
        }
        else
        {
            if ( _debugAll )
            {
                Root.DebugAll();
            }
        }

        Gdx.GL.glEnable( IGL.GL_BLEND );

        _debugShapes.ProjectionMatrix = Camera!.Combined;
        _debugShapes.Begin();

        Root.DrawDebug( _debugShapes );

        _debugShapes.End();

        Gdx.GL.glDisable( IGL.GL_BLEND );
    }

    /// <summary>
    /// Disables debug on all actors recursively except the specified
    /// actor and any children.
    /// </summary>
    private void DisableDebug( Actor actor, Actor except )
    {
        ArgumentNullException.ThrowIfNull( actor );
        ArgumentNullException.ThrowIfNull( except );

        if ( actor == except )
        {
            return;
        }

        actor.DebugActive = false;

        if ( actor is Group group )
        {
            for ( int i = 0, n = group.Children.Size; i < n; i++ )
            {
                if ( group.Children.GetAt( i ) != except )
                {
                    group.Children.GetAt( i ).DebugActive = false;
                }
            }
        }
    }

    /// <summary>
    /// Check if screen coordinates are inside the viewport's screen area.
    /// </summary>
    public virtual bool IsInsideViewport( int screenX, int screenY )
    {
        var x0 = Viewport.ScreenX;
        var x1 = x0 + Viewport.ScreenWidth;
        var y0 = Viewport.ScreenY;
        var y1 = y0 + Viewport.ScreenHeight;

        screenY = Gdx.Graphics.Height - 1 - screenY;

        return ( screenX >= x0 ) && ( screenX < x1 )
                                 && ( screenY >= y0 ) && ( screenY < y1 );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    //TODO: NEEDS Documentation!!

    [PublicAPI]
    public class TouchFocus //: IComparable< TouchFocus >
    {
        public IEventListener? Listener      { get; set; } = null;
        public Actor?          ListenerActor { get; set; } = null;
        public Actor?          Target        { get; set; } = null;
        public int             Button        { get; set; } = 0;
        public int             Pointer       { get; set; } = 0;

        public void Reset()
        {
            ListenerActor = null;
            Listener      = null;
            Target        = null;
            Pointer       = 0;
            Button        = 0;
        }
    }
}