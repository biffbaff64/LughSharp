// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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
using Corelib.Lugh.Scenes.Scene2D.Actions;
using Corelib.Lugh.Scenes.Scene2D.Listeners;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// Displays a dialog, which is a window with a title, a content table, and a button table.
/// Methods are provided to add a label to the content table and buttons to the button table,
/// but any widgets can be added. When a button is clicked, <see cref="Result(object)"/> is
/// called and the dialog is removed from the stage.
/// </summary>
[PublicAPI]
public class Dialog : Window
{
    private readonly IgnoreTouchDown _ignoreTouchDown = null!;

    private ChangeListener _dialogChangeListener = null!;
    private FocusListener  _dialogFocusListener  = null!;
    private InputListener  _dialogInputListener  = null!;

    private Skin? _skin;

    // ========================================================================

    /// <summary>
    /// Creates a new Dialog, using the supplied title and <see cref="Skin"/>
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="skin"></param>
    public Dialog( string title, Skin skin )
        : base( title, skin.Get< WindowStyle >() )
    {
        Skin  = skin;
        _skin = skin;

        Initialise();
    }

    /// <summary>
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="skin"></param>
    /// <param name="windowStyle"> The <see cref="Window.WindowStyle"/> to use. </param>
    public Dialog( string title, Skin skin, string windowStyle )
        : base( title, skin.Get< WindowStyle >( windowStyle ) )
    {
        Skin  = skin;
        _skin = skin;

        Initialise();
    }

    /// <summary>
    /// Creates a new Dialog window, using the supplied name and <see cref="Window.WindowStyle"/>.
    /// </summary>
    /// <param name="title"> A string holding the dialog name to display. </param>
    /// <param name="windowStyle"> The <see cref="Window.WindowStyle"/> to use. </param>
    public Dialog( string title, WindowStyle windowStyle )
        : base( title, windowStyle )
    {
        Initialise();
    }

    public Actor? PreviousKeyboardFocus { get; set; }
    public Actor? PreviousScrollFocus   { get; set; }
    public Table? ContentTable          { get; private set; }
    public Table? ButtonTable           { get; private set; }
    public bool   CancelHide            { get; set; }

    public Dictionary< Actor, object >? Values { get; set; } = new();

    /// <summary>
    /// Initialises the basic elements of this dialog, including the
    /// necessary listeners.
    /// </summary>
    private void Initialise()
    {
        IsModal = true;

        CellDefaults.Space( 6 );

        ContentTable = new Table( _skin );
        ButtonTable  = new Table( _skin );

        Add( ContentTable ).Expand().SetFill();
        AddRow();
        Add( ButtonTable ).SetFillX();

        ContentTable.CellDefaults.Space( 6 );
        ButtonTable.CellDefaults.Space( 6 );

        _dialogFocusListener  = new DialogFocusListener( this );
        _dialogChangeListener = new DialogChangeListener( this );
        _dialogInputListener  = new DialogInputListener( this, 0, null ); //TODO: Correct param values needed

        ButtonTable.AddListener( new ButtonTableChangeListener( this ) );

        AddCaptureListener( _dialogChangeListener );
        AddCaptureListener( _dialogFocusListener );
        AddCaptureListener( _dialogInputListener );
    }

    /// <summary>
    /// Sets the <see cref="Stage"/> which this Dialog will act on.
    /// </summary>
    public override void SetStage( Stage? stage )
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
            _skin = new Skin();

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
            _skin = new Skin();

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
    /// with a <see cref="Actions.FadeIn(float,Corelib.Lugh.Maths.IInterpolation)"/> action.
    /// </summary>
    public Dialog Show( Stage stage )
    {
        Show( stage,
              Scene2D.Actions.Actions.Sequence( Scene2D.Actions.Actions.Alpha( 0 ),
                                                Scene2D.Actions.Actions.FadeIn( 0.4f, Interpolation.Fade ) ) );

        SetPosition( ( float ) Math.Round( ( stage.Width - Width ) / 2 ),
                     ( float ) Math.Round( ( stage.Height - Height ) / 2 ) );

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
        var stage = Stage;

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

            AddAction( Scene2D.Actions.Actions.Sequence( action,
                                                         Scene2D.Actions.Actions.RemoveListener( _ignoreTouchDown, true ),
                                                         Scene2D.Actions.Actions.RemoveActor() ) );
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
        Hide( Scene2D.Actions.Actions.FadeOut( 0.4f, Interpolation.Fade ) );
    }

    public void SetObject( Actor actor, object obj )
    {
        Debug.Assert( Values != null, nameof( Values ) + " != null" );

        Values[ actor ] = obj;
    }

    /// <summary>
    /// If this key is pressed, <see cref="Result(object)"/> is
    /// called with the specified object.
    /// </summary>
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
        //                              GdxApi.App.PostRunnable
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
    public virtual void Result( object? obj )
    {
    }

    private void DefaultSkinProvided()
    {
        Logger.Debug( "This method may only be used if the dialog was constructed "
                    + "with a Skin, a default Skin has been provided." );
    }

    // ========================================================================
    // ========================================================================

    internal class ButtonTableChangeListener : ChangeListener
    {
        private readonly Dialog _dialog;

        public ButtonTableChangeListener( Dialog dialog )
        {
            _dialog = dialog;
        }

        public override void Changed( ChangeEvent ev, Actor? actor )
        {
            if ( ( _dialog.Values == null ) || ( actor == null ) )
            {
                return;
            }

            if ( !_dialog.Values.ContainsKey( actor ) )
            {
                return;
            }

            while ( actor?.Parent != _dialog.ButtonTable )
            {
                actor = actor?.Parent;
            }

            _dialog.Result( _dialog.Values[ actor! ] );

            if ( !_dialog.CancelHide )
            {
                _dialog.Hide();
            }

            _dialog.CancelHide = false;
        }
    }

    public class IgnoreTouchDown : InputListener
    {
        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            ev?.Cancel();

            return false;
        }
    }
}
