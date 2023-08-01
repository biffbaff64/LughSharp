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
using LibGDXSharp.Scenes.Listeners;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// Displays a dialog, which is a window with a title, a content table, and a
/// button table. Methods are provided to add a label to the content table and
/// buttons to the button table, but any widgets can be added. When a button
/// is clicked, <see cref="Result(object)"/> is called and the dialog is
/// removed from the stage.
/// </summary>
public class Dialog : Window
{
    public bool CancelHide { get; set; }

    private Skin?                        _skin;
    private Dictionary< Actor, object >? _values = new();
    private Actor?                       _previousKeyboardFocus;
    private Actor?                       _previousScrollFocus;
    private FocusListener?               _focusListener;

    public Dialog( string title, Skin skin )
        : base( title, skin.Get< Window.WindowStyle >() )
    {
        //TODO: Why is there base.Skin AND this._skin?
        base.Skin  = skin;
        this._skin = skin;

        Initialise();
    }

    public Dialog( string title, Skin skin, string windowStyleName )
        : base
            (
            title, ( Window.WindowStyle )skin.Get< Window.WindowStyle >( windowStyleName )
            ) //TODO: <= Don't like this, find a better way
    {
        //TODO: Why is there base.Skin AND this._skin?
        base.Skin  = skin;
        this._skin = skin;
        Initialise();
    }

    public Dialog( string title, Window.WindowStyle windowStyle )
        : base( title, windowStyle )
    {
        Initialise();
    }

    private void Initialise()
    {
        base.IsModal = true;

        Defaults().Space( 6 );

        ContentTable = new Table( _skin );
        ButtonTable  = new Table( _skin );

        Add( ContentTable ).Expand().SetFill();
        AddRow();
        Add( ButtonTable ).SetFillX();

        ContentTable.Defaults().Space( 6 );
        ButtonTable.Defaults().Space( 6 );

//        buttonTable.addListener( new ChangeListener()
//        {
//            public void changed (ChangeEvent event, Actor actor)
//            {
//                if (!values.containsKey(actor)) return;
//                while (actor.getParent() != buttonTable)
//                actor = actor.getParent();
//                result(values.get(actor));
//                if (!CancelHide) hide();
//                CancelHide = false;
//            }
//        });

//        focusListener = new FocusListener()
//        {
//            public void keyboardFocusChanged (FocusEvent event, Actor actor, bool focused)
//            {
//                if (!focused) focusChanged(event);
//            }

//            public void scrollFocusChanged( FocusListener.FocusEvent event, Actor actor, bool focused )
//            {
//                if ( !focused ) focusChanged( event );
//            }

//            private void focusChanged( FocusListener.FocusEvent event )
//            {
//                Stage stage = getStage();

//                if ( isModal
//                     && stage != null
//                     && stage.getRoot().getChildren().size > 0
//                     && stage.getRoot().getChildren().peek() == Dialog.this) {
        // Dialog is top most actor.
//                    Actor newFocusedActor =  event.getRelatedActor();

//                    if ( newFocusedActor != null
//                         && !newFocusedActor.isDescendantOf( Dialog.this )
//                         && !( newFocusedActor.equals
//                                   ( _previousKeyboardFocus )
//                               || newFocusedActor.equals( _previousScrollFocus ) ) )
//                        event.cancel();
//                }
//            }
//        };
    }

    protected new void SetStage( Stage? stage )
    {
//        if ( stage == null )
//        {
//            AddListener( _focusListener );
//        }
//        else
//        {
//            RemoveListener( _focusListener );
//        }

        base.SetStage( stage! );
    }

//    protected InputListener ignoreTouchDown = new InputListener()
//    {
//        public bool touchDown (InputEvent event, float x, float y, int pointer, int button)
//        {
//            event.cancel();
//            return false;
//        }
//    };

    public Table? ContentTable { get; private set; }

    public Table? ButtonTable { get; private set; }

    /// <summary>
    /// Adds a label to the content table. The dialog must have
    /// been constructed with a skin to use this method. 
    /// </summary>
    public Dialog Text( string? text )
    {
        if ( _skin == null )
            throw new IllegalStateException
                ( "This method may only be used if the dialog was constructed with a Skin." );

        return Text( text, _skin.Get< Label.LabelStyle >() );
    }

    /// <summary>
    /// Adds a label to the content table.
    /// </summary>
    public Dialog Text( string? text, Label.LabelStyle labelStyle )
    {
        return Text( new Label( text, labelStyle ) );
    }

    /// <summary>
    /// Adds the given Label to the content table
    /// </summary>
    public Dialog Text( Label label )
    {
        ContentTable?.Add( label );

        return this;
    }

    /// <summary>
    /// Adds a text button to the button table. Null will be passed to
    /// <see cref="Result(object)"/> if this button is clicked. The
    /// dialog must have been constructed with a skin to use this method.
    /// </summary>
    public Dialog Button( string text, object? obj = null )
    {
        if ( _skin == null )
            throw new IllegalStateException
                ( "This method may only be used if the dialog was constructed with a Skin." );

        return Button( text, obj, _skin.Get< TextButton.TextButtonStyle >() );
    }

    /// <summary>
    /// Adds a text button to the button table.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="obj">
    /// The object that will be passed to <see cref="Result(object)"/>
    /// if this button is clicked. May be null.
    /// </param>
    /// <param name="buttonStyle"></param>
    public Dialog Button( string text, object? obj, TextButton.TextButtonStyle buttonStyle )
    {
        return Button( new TextButton( text, buttonStyle ), obj );
    }

    /**
     * Adds the given button to the button table.
     * @param object The object that will be passed to {@link #result(Object)} if this button is clicked. May be null.
     */
    public Dialog Button( Button button, object? obj = null )
    {
        ButtonTable?.Add( button );

        SetObject( button, obj! );

        return this;
    }

    /**
     * {@link #pack() Packs} the dialog (but doesn't set the position), adds it to the stage, sets it as the keyboard and scroll
     * focus, clears any actions on the dialog, and adds the specified action to it. The previous keyboard and scroll focus are
     * remembered so they can be restored when the dialog is hidden.
     * @param action May be null.
     */
    public Dialog Show( Stage stage, Action? action )
    {
        ClearActions();
//        RemoveCaptureListener( ignoreTouchDown );
        _previousKeyboardFocus = null;

        Actor? actor = stage.KeyboardFocus;

        if ( ( actor != null ) && !actor.IsDescendantOf( this ) )
        {
            _previousKeyboardFocus = actor;
        }

        _previousScrollFocus = null;

        actor = stage.ScrollFocus;

        if ( ( actor != null ) && !actor.IsDescendantOf( this ) )
        {
            _previousScrollFocus = actor;
        }

        stage.AddActor( this );

        Pack();

        stage.CancelTouchFocus();
        stage.KeyboardFocus = this;
        stage.ScrollFocus   = this;

        if ( action != null ) AddAction( action );

        return this;
    }

    /**
     * Centers the dialog in the stage and calls {@link #show(Stage, Action)}
     * with a {@link Actions#fadeIn(float, Interpolation)} action.
     */
    public Dialog Show( Stage stage )
    {
//        Show( stage, sequence( Actions.alpha( 0 ), Actions.fadeIn( 0.4f, Interpolation.fade ) ) );

        SetPosition
            (
            (float)Math.Round( ( stage.StageWidth - Width ) / 2 ),
            (float)Math.Round( ( stage.StageHeight - Height ) / 2 )
            );

        return this;
    }

    /**
     * Removes the dialog from the stage, restoring the previous keyboard and
     * scroll focus, and adds the specified action to the dialog.
     * @param action
     * If null, the dialog is removed immediately. Otherwise, the dialog is removed when the action completes. The
     * dialog will not respond to touch down events during the action. */
    public void Hide( Action? action )
    {
        Stage? stage = base.Stage;

        if ( stage != null )
        {
//            RemoveListener( _focusListener );

            if ( _previousKeyboardFocus is { Stage: null } )
            {
                _previousKeyboardFocus = null;
            }

            Actor? actor = stage.KeyboardFocus;

            if ( ( actor == null ) || actor.IsDescendantOf( this ) )
            {
                stage.KeyboardFocus = _previousKeyboardFocus;
            }

            if ( _previousScrollFocus is { Stage: null } )
            {
                _previousScrollFocus = null;
            }

            actor = stage.ScrollFocus;

            if ( ( actor == null ) || actor.IsDescendantOf( this ) )
            {
                stage.ScrollFocus = _previousScrollFocus;
            }
        }

        if ( action != null )
        {
//            AddCaptureListener( ignoreTouchDown );
//            AddAction( sequence( action, Actions.RemoveListener( ignoreTouchDown, true ), Actions.RemoveActor() ) );
        }
        else
        {
            Remove();
        }
    }

    /// <summary>
    /// Hides the dialog. Called automatically when a button is clicked.
    /// The default implementation fades out the dialog over 400 milliseconds.
    /// </summary>
    public void Hide()
    {
//        Hide( FadeOut( 0.4f, Interpolation.Fade ) );
    }

    public void SetObject( Actor actor, object obj )
    {
        System.Diagnostics.Debug.Assert( _values != null, nameof( _values ) + " != null" );

        _values[ actor ] = obj;
    }

    /// <summary>
    /// If this key is pressed, <see cref="Result(object)"/> is
    /// called with the specified object.
    /// </summary>
    /// <seealso cref="IInput.Keys"/>
    public Dialog Key( int keycode, object obj )
    {
//        addListener( new InputListener()
//        {
//            public bool keyDown (InputEvent event, int keycode2)
//            {
//                if (keycode == keycode2)
//                {
        // Delay a frame to eat the keyTyped event.
//                    Gdx.app.postRunnable(new Runnable()
//                    {
//                        public void run ()
//                        {
//                            result(object);
//                            if (!CancelHide) hide();
//                            CancelHide = false;
//                        }
//                    });
//                }

//                return false;
//            }
//        });

        return this;
    }

    /// <summary>
    /// Called when a button is clicked. The dialog will be hidden after this
    /// method returns unless <see cref="Cancel()"/> is called.
    /// </summary>
    /// <param name="obj"> The object specified when the button was added. </param>
    protected void Result( object? obj )
    {
    }
}