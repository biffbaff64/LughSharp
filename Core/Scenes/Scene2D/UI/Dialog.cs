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

using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.Actions;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// Displays a dialog, which is a window with a title, a content table, and a button table.
/// Methods are provided to add a label to the content table and buttons to the button table,
/// but any widgets can be added. When a button is clicked, <see cref="Result(object)"/> is
/// called and the dialog is removed from the stage.
/// </summary>
[PublicAPI]
public class Dialog : Window
{
    public Actor? PreviousKeyboardFocus { get; set; }
    public Actor? PreviousScrollFocus   { get; set; }
    public Table? ContentTable          { get; private set; }
    public Table? ButtonTable           { get; private set; }
    public bool   CancelHide            { get; set; }

    private ChangeListener  _dialogChangeListener = null!;
    private FocusListener   _dialogFocusListener  = null!;
    private InputListener   _dialogInputListener  = null!;
    private IgnoreTouchDown _ignoreTouchDown      = null!;

    private Skin? _skin;

    private Dictionary< Actor, object >? _values = new();

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new Dialog, using the supplied title and <see cref="Skin"/>
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="skin"></param>
    public Dialog( string title, Skin skin )
        : base( title, skin.Get< Window.WindowStyle >() )
    {
        base.Skin  = skin;
        this._skin = skin;

        Initialise();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="skin"></param>
    /// <param name="windowStyle"> The <see cref="Window.WindowStyle"/> to use. </param>
    public Dialog( string title, Skin skin, string windowStyle )
        : base( title, skin.Get< Window.WindowStyle >( windowStyle ) )
    {
        base.Skin  = skin;
        this._skin = skin;

        Initialise();
    }

    /// <summary>
    /// Creates a new Dialog window, using the supplied name and <see cref="Window.WindowStyle"/>.
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="windowStyle"> The <see cref="Window.WindowStyle"/> to use. </param>
    public Dialog( string title, Window.WindowStyle windowStyle )
        : base( title, windowStyle )
    {
        Initialise();
    }

    /// <summary>
    /// Initialises the basic elements of this dialog, including the
    /// necessary listeners.
    /// </summary>
    private void Initialise()
    {
        base.IsModal = true;

        CellDefaults.Space( 6 );

        ContentTable = new Table( _skin );
        ButtonTable  = new Table( _skin );

        Add( ContentTable ).Expand().SetFill();
        AddRow();
        Add( ButtonTable ).SetFillX();

        ContentTable.CellDefaults.Space( 6 );
        ButtonTable.CellDefaults.Space( 6 );

        this._dialogFocusListener  = new DialogFocusObserver( this );
        this._dialogChangeListener = new DialogChangeObserver( this );
        this._dialogInputListener  = new DialogInputObserver( this );

        ButtonTable.AddListener( new ButtonTableChangeListener( this ) );

        AddCaptureListener( _dialogChangeListener );
        AddCaptureListener( _dialogFocusListener );
        AddCaptureListener( _dialogInputListener );
    }

    /// <summary>
    /// Sets the <see cref="Stage"/> which this Dialog will act on.
    /// </summary>
    protected new void SetStage( Stage? stage )
    {
        if ( stage == null )
        {
            AddListener( _dialogFocusListener );
        }
        else
        {
            RemoveListener( _dialogFocusListener );
        }

        base.SetStage( stage );
    }

    /// <summary>
    /// Adds a label to the content table. The dialog needs to have been constructed
    /// with a <see cref="Skin"/> to use this method. If it hasn't, a default Skin
    /// will be created which may need further adjustments.
    /// </summary>
    public Dialog Text( string? text )
    {
        if ( _skin == null )
        {
            this._skin = new Skin();

            DefaultSkinProvided();
        }

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
    /// Adds a text button to the button table. Null will be passed to <see cref="Result(object)"/>
    /// if this button is clicked. The dialog must have been constructed with a skin to use this
    /// method. If it hasn't, a default Skin will be created which may need further adjustments.
    /// </summary>
    public Dialog Button( string text, object? obj = null )
    {
        if ( _skin == null )
        {
            this._skin = new Skin();

            DefaultSkinProvided();
        }

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

    /// <summary>
    /// Adds the given button to the button table.
    /// </summary>
    /// <param name="button"></param>
    /// <param name="obj">
    /// The object that will be passed to <see cref="Result(object)"/> if this
    /// button is clicked. May be null.
    /// </param>
    public Dialog Button( Button button, object? obj = null )
    {
        ButtonTable?.Add( button );

        SetObject( button, obj! );

        return this;
    }

    /// <summary>
    /// <see cref="WidgetGroup.Pack()"/> the dialog (but doesn't set the position),
    /// adds it to the stage, sets it as the keyboard and scroll focus, clears any
    /// actions on the dialog, and adds the specified action to it. The previous
    /// keyboard and scroll focus are remembered so they can be restored when the
    /// dialog is hidden.
    /// </summary>
    /// <param name="stage"></param>
    /// <param name="action"> May be null. </param>
    public Dialog Show( Stage stage, Action? action )
    {
        ClearActions();

        RemoveCaptureListener( _ignoreTouchDown );

        PreviousKeyboardFocus = null;

        if ( ( stage.KeyboardFocus != null ) && !stage.KeyboardFocus.IsDescendantOf( this ) )
        {
            PreviousKeyboardFocus = stage.KeyboardFocus;
        }

        PreviousScrollFocus = null;

        if ( ( stage.ScrollFocus != null ) && !stage.ScrollFocus.IsDescendantOf( this ) )
        {
            PreviousScrollFocus = stage.ScrollFocus;
        }

        stage.AddActor( this );

        Pack();

        stage.CancelTouchFocus();
        stage.KeyboardFocus = this;
        stage.ScrollFocus   = this;

        if ( action != null )
        {
            AddAction( action );
        }

        return this;
    }

    /// <summary>
    /// Centers the dialog in the stage and calls <see cref="Show(Stage, Action)"/>
    /// with a <see cref="SceneActions.FadeIn(float, IInterpolation)"/> action.
    /// </summary>
    public Dialog Show( Stage stage )
    {
        Show( stage,
              SceneActions.Sequence( SceneActions.Alpha( 0 ),
                                     SceneActions.FadeIn( 0.4f, Interpolation.fade ) ) );

        SetPosition( ( float )Math.Round( ( stage.Width - Width ) / 2 ),
                     ( float )Math.Round( ( stage.Height - Height ) / 2 ) );

        return this;
    }

    /// <summary>
    /// Removes the dialog from the stage, restoring the previous keyboard and scroll focus,
    /// and adds the specified action to the dialog.
    /// </summary>
    /// <param name="action">
    /// If null, the dialog is removed immediately. Otherwise, the dialog is removed when the
    /// action completes. The dialog will not respond to touch down events during the action.
    /// </param>
    public void Hide( Action? action )
    {
        Stage? stage = base.Stage;

        if ( stage != null )
        {
            RemoveListener( _dialogFocusListener );

            if ( PreviousKeyboardFocus is { Stage: null } )
            {
                PreviousKeyboardFocus = null;
            }

            if ( ( stage.KeyboardFocus == null ) || stage.KeyboardFocus.IsDescendantOf( this ) )
            {
                stage.KeyboardFocus = PreviousKeyboardFocus;
            }

            if ( PreviousScrollFocus is { Stage: null } )
            {
                PreviousScrollFocus = null;
            }

            if ( ( stage.ScrollFocus == null ) || stage.ScrollFocus.IsDescendantOf( this ) )
            {
                stage.ScrollFocus = PreviousScrollFocus;
            }
        }

        if ( action != null )
        {
            AddCaptureListener( _ignoreTouchDown );

            AddAction( SceneActions.Sequence( action,
                                              SceneActions.RemoveListener( _ignoreTouchDown, true ),
                                              SceneActions.RemoveActor() ) );
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
        Hide( SceneActions.FadeOut( 0.4f, Interpolation.fade ) );
    }

    public void SetObject( Actor actor, object obj )
    {
        Debug.Assert( _values != null, nameof( _values ) + " != null" );

        _values[ actor ] = obj;
    }

    /// <summary>
    /// If this key is pressed, <see cref="Result(object)"/> is
    /// called with the specified object.
    /// </summary>
    /// <seealso cref="IInput.Keys"/>
    public Dialog Key( int keycode, object obj )
    {
//        AddListener
//            ( new InputListener
//                  (
//                  _ =>
//                  {
//                      public bool keyDown( InputEvent ev, int keycode2 )
//                      {
//                          if ( keycode == keycode2 )
//                          {
        // Delay a frame to eat the keyTyped event.
//                              Gdx.App.PostRunnable
//                                  ( new IRunnable
//                                        (
//                                        _ =>
//                                        {
//                                            public void Run()
//                                            {
//                                                result( object );

//                                                if ( !CancelHide )
//                                                {
//                                                    hide();
//                                                }

//                                                CancelHide = false;
//                                            }
//                                        }
//                                        );
//                          }

//                          return false;
//                      }
//                  }
//                  );

        return this;
    }

    /// <summary>
    /// Called when a button is clicked. The dialog will be hidden after this
    /// method returns unless <see cref="CancelHide"/> is set.
    /// </summary>
    /// <param name="obj"> The object specified when the button was added. </param>
    protected void Result( object? obj )
    {
    }

    private void DefaultSkinProvided()
    {
        Gdx.App.Debug( "Dialog",
                       "This method may only be used if the dialog was constructed "
                     + "with a Skin, a default Skin has been provided." );
    }
    
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    internal class ButtonTableChangeListener : ChangeListener
    {
        private readonly Dialog _dialog;

        public ButtonTableChangeListener( Dialog dialog )
        {
            this._dialog = dialog;
        }

        public override void Changed( ChangeEvent ev, Actor? actor )
        {
            if ( ( _dialog._values == null ) || ( actor == null ) )
            {
                return;
            }

            if ( !_dialog._values.ContainsKey( actor ) )
            {
                return;
            }

            while ( actor?.Parent != _dialog.ButtonTable )
            {
                actor = actor?.Parent;
            }

            _dialog.Result( _dialog._values[ actor! ] );

            if ( !_dialog.CancelHide )
            {
                _dialog.Hide();
            }

            _dialog.CancelHide = false;
        }
    }

    public class IgnoreTouchDown : InputListener
    {
        public override bool TouchDown( InputEvent ev, float x, float y, int pointer, int button )
        {
            ev.Cancel();

            return false;
        }
    }
}
