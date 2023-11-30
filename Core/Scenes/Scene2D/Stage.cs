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

using System.Reflection.Metadata;

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Pooling;
using LibGDXSharp.Utils.Viewport;

using Matrix4 = LibGDXSharp.Maths.Matrix4;

namespace LibGDXSharp.Scenes.Scene2D;

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
public class Stage : InputAdapter
{
    // True if any actor has ever had debug enabled.
    public bool debug;

    private readonly Vector2  _tempCoords        = new();
    private readonly Actor?[] _pointerOverActors = new Actor?[ 20 ];
    private readonly bool[]   _pointerTouched    = new bool[ 20 ];
    private readonly int[]    _pointerScreenX    = new int[ 20 ];
    private readonly int[]    _pointerScreenY    = new int[ 20 ];

    public readonly SnapshotArray< TouchFocus > touchFocuses = new( true, 4 );

    private readonly bool  _ownsBatch;
    private readonly Group _root = null!;

    private int _mouseScreenX;
    private int _mouseScreenY;

    private Actor? _mouseOverActor;
    private Actor? _keyboardFocus;
    private Actor? _scrollFocus;

    private ShapeRenderer?  _debugShapes;
    private bool            _debugAll;
    private bool            _debugUnderMouse;
    private bool            _debugParentUnderMouse;
    private Table.DebugType _debugTableUnderMouse = Table.DebugType.None;

    /// <summary>
    /// Creates a stage with a <see cref="ScalingViewport"/> set to
    /// <see cref="Scaling.Stretch"/>. The stage will use its own <see cref="IBatch"/>
    /// which will be disposed when the stage is disposed. 
    /// </summary>
    public Stage() : this
        (
         new ScalingViewport
             (
              Scaling.Stretch,
              Gdx.Graphics.Width,
              Gdx.Graphics.Height,
              new OrthographicCamera()
             ),
         new SpriteBatch()
        )
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
    /// Creates a stage with the specified viewport and batch. This can be used
    /// to specify an existing batch or to customize which batch implementation is used.
    /// </summary>
    /// <param name="viewport"></param>
    /// <param name="batch">
    /// Will not be disposed if <see cref="Dispose()"/> is called,
    /// handle disposal yourself.
    /// </param>
    public Stage( Viewport? viewport, IBatch? batch )
    {
        this.Viewport = viewport ?? throw new ArgumentException( "viewport cannot be null." );
        this.Batch    = batch ?? throw new ArgumentException( "batch cannot be null." );

        Root = new Group();
        Root.SetStage( this );

        viewport.Update( Gdx.Graphics.Width, Gdx.Graphics.Height, true );
    }

    /// <summary>
    /// </summary>
    public void Draw()
    {
        Camera = Viewport.Camera;
        Camera.Update();

        if ( !Root.IsVisible )
        {
            return;
        }

        Batch.SetProjectionMatrix( Camera.Combined );

        Batch.Begin();
        Root.Draw( Batch, 1 );
        Batch.End();

        if ( debug )
        {
            DrawDebug();
        }
    }

    private void DrawDebug()
    {
        if ( _debugShapes == null )
        {
            _debugShapes = new ShapeRenderer();
            _debugShapes.SetAutoShapeType( true );
        }

        if ( _debugUnderMouse
          || _debugParentUnderMouse
          || ( _debugTableUnderMouse != Table.DebugType.None ) )
        {
            ScreenToStageCoordinates( _tempCoords.Set( Gdx.Input.GetX(), Gdx.Input.GetY() ) );

            Actor? actor = Hit( _tempCoords.X, _tempCoords.Y, true );

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

                ( ( Table )actor ).DebugLines( _debugTableUnderMouse );
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

        Gdx.GL.GLEnable( IGL20.GL_BLEND );

        _debugShapes.ProjectionMatrix = Camera.Combined;
        _debugShapes.Begin();

        Root.DrawDebug( _debugShapes );

        _debugShapes.End();

        Gdx.GL.GLDisable( IGL20.GL_BLEND );
    }

    /// <summary>
    /// Disables debug on all actors recursively except the specified
    /// actor and any children.
    /// </summary>

    // TODO: Refactor this to remove the recursiveness
    private void DisableDebug( Actor actor, Actor except )
    {
        if ( actor == except )
        {
            return;
        }

        actor.DebugActive = false;

        if ( actor is Group group )
        {
            for ( int i = 0, n = group.Children.Size; i < n; i++ )
            {
                DisableDebug( group.Children.Get( i ), except );
            }
        }
    }

    /// <summary>
    /// Calls <see cref="Act(float)"/> with Gdx.Graphics.GetDeltaTime(),
    /// limited to a minimum of 30fps.
    /// </summary>
    public void Act()
    {
        Act( Math.Min( Gdx.Graphics.DeltaTime, 1 / 30f ) );
    }

    /// <summary>
    /// Calls the <see cref="Actor.Act(float)"/> method on each actor in the
    /// stage. Typically called each frame. This method also fires enter and exit
    /// events.
    /// </summary>
    /// <param name="delta"> Time in seconds since the last frame.</param>
    public void Act( float delta )
    {
        // Update over actors. Done in act() because actors may change position,
        // which can fire enter/exit without an input event.
        for ( int pointer = 0, n = _pointerOverActors.Length; pointer < n; pointer++ )
        {
            Actor? overLast = _pointerOverActors[ pointer ];

            // Check if pointer is gone.
            if ( !_pointerTouched[ pointer ] )
            {
                if ( overLast != null )
                {
                    _pointerOverActors[ pointer ] = null;

                    ScreenToStageCoordinates
                        (
                         _tempCoords.Set
                             (
                              _pointerScreenX[ pointer ],
                              _pointerScreenY[ pointer ]
                             )
                        );

                    // Exit over last.
                    InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
            _pointerOverActors[ pointer ] = FireEnterAndExit
                (
                 overLast,
                 _pointerScreenX[ pointer ],
                 _pointerScreenY[ pointer ],
                 pointer
                );
        }

        // Update over actor for the mouse on the desktop.
        IApplication.ApplicationType type = Gdx.App.AppType;

        if ( type is IApplication.ApplicationType.Desktop
                     or IApplication.ApplicationType.WebGL )
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

        Actor? over = Hit( _tempCoords.X, _tempCoords.Y, true );

        if ( over == overLast )
        {
            return overLast;
        }

        // Exit overLast.
        if ( overLast != null )
        {
            InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
            InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
    public new bool TouchDown( int screenX, int screenY, int pointer, int button )
    {
        if ( !IsInsideViewport( screenX, screenY ) )
        {
            return false;
        }

        _pointerTouched[ pointer ] = true;
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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

        Actor? target = Hit( _tempCoords.X, _tempCoords.Y, true );

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
    /// Applies a touch moved event to the stage and returns true if an actor in
    /// the scene <see cref="Event.Handle()"/> handled the event.
    /// Only <see cref="InputListener"/> listeners that returned true for
    /// touchDown will receive this event. 
    /// </summary>
    public new bool TouchDragged( int screenX, int screenY, int pointer )
    {
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;
        _mouseScreenX              = screenX;
        _mouseScreenY              = screenY;

        if ( this.touchFocuses.Size == 0 )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

        if ( inputEvent == null )
        {
            throw new GdxRuntimeException( "Null InputEvent for TouchDragged!" );
        }

        inputEvent.Type    = InputEvent.EventType.TouchDragged;
        inputEvent.Stage   = this;
        inputEvent.StageX  = _tempCoords.X;
        inputEvent.StageY  = _tempCoords.X;
        inputEvent.Pointer = pointer;

        TouchFocus?[] focuses = this.touchFocuses.Begin();

        for ( int i = 0, n = this.touchFocuses.Size; i < n; i++ )
        {
            TouchFocus? focus = focuses[ i ];

            if ( focus?.pointer != pointer )
            {
                continue;
            }

            if ( !touchFocuses.Contains( focus ) )
            {
                // Touch focus already gone.
                continue;
            }

            inputEvent.TargetActor   = focus.target;
            inputEvent.ListenerActor = focus.listenerActor;

            if ( ( focus.listener != null ) && focus.listener.Handle( inputEvent ) )
            {
                inputEvent.Handle();
            }
        }

        touchFocuses.End();

        var handled = inputEvent.IsHandled;

        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a touch up event to the stage and returns true if an actor in the
    /// scene <see cref="Event.Handle()"/> handled the event.
    /// Only <see cref="InputListener"/> listeners that returned true for
    /// touchDown will receive this event. 
    /// </summary>
    public new bool TouchUp( int screenX, int screenY, int pointer, int button )
    {
        _pointerTouched[ pointer ] = false;
        _pointerScreenX[ pointer ] = screenX;
        _pointerScreenY[ pointer ] = screenY;

        if ( this.touchFocuses.Size == 0 )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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

        TouchFocus?[] focuses = touchFocuses.Begin();

        for ( int i = 0, n = this.touchFocuses.Size; i < n; i++ )
        {
            TouchFocus? focus = focuses[ i ];

            if ( ( focus?.pointer != pointer ) || ( focus.button != button ) )
            {
                continue;
            }

            if ( !this.touchFocuses.Remove( focus ) )
            {
                // Touch focus already gone.
                continue;
            }

            inputEvent.TargetActor   = focus.target;
            inputEvent.ListenerActor = focus.listenerActor;

            if ( ( focus.listener != null ) && focus.listener.Handle( inputEvent ) )
            {
                inputEvent.Handle();
            }

            Pools< TouchFocus >.Free( focus );
        }

        touchFocuses.End();

        var handled = inputEvent.IsHandled;
        Pools< InputEvent >.Free( inputEvent );

        return handled;
    }

    /// <summary>
    /// Applies a mouse moved event to the stage and returns true if an actor
    /// in the scene <see cref="Event.Handle()"/> the event. This event only
    /// occurs on the desktop. 
    /// </summary>
    public new bool MouseMoved( int screenX, int screenY )
    {
        _mouseScreenX = screenX;
        _mouseScreenY = screenY;

        if ( !IsInsideViewport( screenX, screenY ) )
        {
            return false;
        }

        ScreenToStageCoordinates( _tempCoords.Set( screenX, screenY ) );

        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
    /// in the scene <see cref="Event.Handle()"/> the event. This event only
    /// occurs on the desktop. 
    /// </summary>
    public new bool Scrolled( float amountX, float amountY )
    {
        Actor target = ScrollFocus ?? Root;

        ScreenToStageCoordinates( _tempCoords.Set( _mouseScreenX, _mouseScreenY ) );

        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
    /// true if the event was handled in <see cref="Event.Handle()"/>. 
    /// </summary>
    public new bool KeyDown( int keyCode )
    {
        Actor       target     = _keyboardFocus ?? Root;
        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
    /// if any, and returns true if the event was <see cref="Event.Handle()"/>. 
    /// </summary>
    public new bool KeyUp( int keyCode )
    {
        Actor       target     = _keyboardFocus ?? Root;
        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
    /// if any, and returns true if the event was <see cref="Event.Handle()"/>. 
    /// </summary>
    public new bool KeyTyped( char character )
    {
        Actor       target     = _keyboardFocus ?? Root;
        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
        TouchFocus? focus = Pools< TouchFocus >.Obtain();

        if ( focus == null )
        {
            return;
        }

        focus.listenerActor = listenerActor;
        focus.target        = target;
        focus.listener      = listener;
        focus.pointer       = pointer;
        focus.button        = button;

        touchFocuses.Add( focus );
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
        for ( var i = this.touchFocuses.Size - 1; i >= 0; i-- )
        {
            TouchFocus focus = this.touchFocuses.Get( i );

            if ( ( focus.listener == listener )
              && ( focus.listenerActor == listenerActor )
              && ( focus.target == target )
              && ( focus.pointer == pointer )
              && ( focus.button == button ) )
            {
                this.touchFocuses.RemoveAt( i );
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
        TouchFocus?[] items      = this.touchFocuses.Begin();

        for ( int i = 0, n = this.touchFocuses.Size; i < n; i++ )
        {
            TouchFocus? focus = items[ i ];

            if ( focus?.listenerActor != listenerActor )
            {
                continue;
            }

            if ( !this.touchFocuses.Remove( focus ) )
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

            inputEvent.TargetActor   = focus.target;
            inputEvent.ListenerActor = focus.listenerActor;
            inputEvent.Pointer       = focus.pointer;
            inputEvent.Button        = focus.button;

            focus.listener?.Handle( inputEvent );

            // Cannot return TouchFocus to pool, as it may still be in use
            // (eg if cancelTouchFocus is called from touchDragged).
        }

        this.touchFocuses.End();

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
        InputEvent? inputEvent = Pools< InputEvent >.Obtain();

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
        TouchFocus?[] items = this.touchFocuses.Begin();

        for ( int i = 0, n = touchFocuses.Size; i < n; i++ )
        {
            TouchFocus? focus = items[ i ];

            if ( ( focus?.listener == exceptListener )
              && ( focus?.listenerActor == exceptActor ) )
            {
                continue;
            }

            if ( focus != null )
            {
                if ( !touchFocuses.Remove( focus ) )
                {
                    continue; // Touch focus already gone.
                }
            }

            inputEvent.TargetActor   = focus?.target;
            inputEvent.ListenerActor = focus?.listenerActor;
            inputEvent.Pointer       = focus!.pointer;
            inputEvent.Button        = focus.button;

            focus.listener?.Handle( inputEvent );

            // Cannot return TouchFocus to pool, as it may still be in use
            // (eg if cancelTouchFocus is called from touchDragged).
        }

        touchFocuses.End();

        Pools< InputEvent >.Free( inputEvent );
    }

    /// <summary>
    /// Adds an actor to the root of the stage.
    /// </summary>
    /// <see cref="Group.AddActor "/>
    public void AddActor( Actor actor )
    {
        Root.AddActor( actor );
    }

    /// <summary>
    /// Adds an action to the root of the stage.
    /// </summary>
    /// <see cref="Group.AddAction(Action) "/>
    public void AddAction( Action action )
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
    public void Clear()
    {
        UnfocusAll();
        Root.Clear();
    }

    /// <summary>
    /// Removes the touch, keyboard, and scroll focused actors.
    /// </summary>
    public void UnfocusAll()
    {
        ScrollFocus   = null;
        KeyboardFocus = null;
        CancelTouchFocus();
    }

    /// <summary>
    /// Removes the touch, keyboard, and scroll focus for the specified
    /// actor and any descendants.
    /// </summary>
    public void Unfocus( Actor actor )
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
    /// <param name="stageX"></param>
    /// <param name="stageY"></param>
    /// <param name="touchable">
    /// If true, the hit detection will respect the <see cref="Actor.Touchable"/>.
    /// </param>
    /// <returns> May be null if no actor was hit.  </returns>
    public Actor? Hit( float stageX, float stageY, bool touchable )
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
    public Vector2 ScreenToStageCoordinates( Vector2 screenCoords )
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
    public Vector2 StageToScreenCoordinates( Vector2 stageCoords )
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
    public Vector2 ToScreenCoordinates( Vector2 coords, Matrix4 transformMatrix )
    {
        return Viewport.ToScreenCoordinates( coords, transformMatrix );
    }

    /// <summary>
    /// Calculates window scissor coordinates from local coordinates using the
    /// batch's current transformation matrix.
    /// </summary>
    public void CalculateScissors( RectangleShape localRect, RectangleShape scissorRect )
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

            FocusListener.FocusEvent? focusEvent = Pools< FocusListener.FocusEvent >.Obtain();

            if ( focusEvent == null )
            {
                return;
            }

            focusEvent.Stage = this;
            focusEvent.Type  = FocusListener.FocusEvent.FeType.Keyboard;

            Actor? oldKeyboardFocus = _keyboardFocus;

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

            FocusListener.FocusEvent? focusEvent = Pools< FocusListener.FocusEvent >.Obtain();

            if ( focusEvent == null )
            {
                return;
            }
            
            focusEvent.Stage = this;
            focusEvent.Type  = FocusListener.FocusEvent.FeType.Scroll;

            Actor? oldScrollFocus = ScrollFocus;

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

    public Viewport Viewport { get; set; }
    public IBatch   Batch    { get; }

    /// <summary>
    /// The viewport's world width.
    /// </summary>
    public float Width => Viewport.WorldWidth;

    /// <summary>
    /// The viewport's world height.
    /// </summary>
    public float Height => Viewport.WorldHeight;

    /// <summary>
    /// The viewport's camera.
    /// </summary>
    public Camera Camera { get; set; } = null!;

    /// <summary>
    /// Returns the root group which holds all actors in the stage.
    /// </summary>
    public Group Root
    {
        get => _root;
        private init
        {
            value.Parent?.RemoveActor( value, false );

            this._root = value;

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
    public bool ActionsRequestRendering { set; get; } = true;

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
            if ( this._debugAll == value )
            {
                return;
            }

            this._debugAll = value;

            if ( value )
            {
                debug = true;
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
            if ( this._debugUnderMouse == value )
            {
                return;
            }

            this._debugUnderMouse = value;

            if ( value )
            {
                debug = true;
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
            if ( this._debugParentUnderMouse == value )
            {
                return;
            }

            this._debugParentUnderMouse = value;

            if ( value )
            {
                debug = true;
            }
            else
            {
                Root.SetDebug( false, true );
            }
        }
    }

    /// <summary>
    /// If not <see cref="Table.DebugType.None"/>, debug is enabled only for the first
    /// ascendant of the actor under the mouse that is a table.
    /// Can be combined with <see cref="DebugAll"/>.
    /// </summary>
    /// <param name="debugTableUnderMouse">May be null for <see cref="Table.DebugType.None"/>.</param>
    public void SetDebugTableUnderMouse( Table.DebugType debugTableUnderMouse )
    {
        if ( Enum.IsDefined( typeof( Table.DebugType ), debugTableUnderMouse ) )
        {
            _debugTableUnderMouse = Table.DebugType.None;
        }

        if ( this._debugTableUnderMouse == debugTableUnderMouse )
        {
            return;
        }

        this._debugTableUnderMouse = debugTableUnderMouse;

        if ( debugTableUnderMouse != Table.DebugType.None )
        {
            debug = true;
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
    public void SetDebugTableUnderMouse( bool debugTableUnderMouse )
    {
        SetDebugTableUnderMouse( debugTableUnderMouse ? Table.DebugType.All : Table.DebugType.None );
    }

    /// <summary>
    /// </summary>
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
    /// Check if screen coordinates are inside the viewport's screen area.
    /// </summary>
    public bool IsInsideViewport( int screenX, int screenY )
    {
        var x0 = Viewport.ScreenX;
        var x1 = x0 + Viewport.ScreenWidth;
        var y0 = Viewport.ScreenY;
        var y1 = y0 + Viewport.ScreenHeight;

        screenY = Gdx.Graphics.Height - 1 - screenY;

        return ( screenX >= x0 ) && ( screenX < x1 ) && ( screenY >= y0 ) && ( screenY < y1 );
    }
}
