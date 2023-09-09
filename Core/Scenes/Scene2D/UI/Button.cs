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

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Listeners;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A button is a <see cref="Table"/> with a checked state and additional
/// <see cref="ButtonStyle"/> style fields for pressed, unpressed, and
/// checked. Each time a button is clicked, the checked state is toggled.
/// Being a table, a button can contain any other actors.
/// <para>
/// The button's padding is set to the background drawable's padding when
/// the background changes, overwriting any padding set manually. Padding
/// can still be set on the button's table cells.
/// </para>
/// <para>
/// A <see cref="ChangeListener.ChangeEvent"/> is fired when the button is
/// clicked. Cancelling the event will restore the checked button state to
/// what it was previously.
/// </para>
/// <para>
/// The preferred size of the button is determined by the background and the
/// button contents.
/// </para>
/// </summary>
[PublicAPI]
public class Button : Table, IDisableable
{

    #region private_data

    private ButtonStyle? _style;
    private bool         _programmaticChangeEvents = true;

    #endregion private_data

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region constructors

    public Button( Skin skin ) : base( skin )
    {
        Initialize();
        Style = skin.Get< ButtonStyle >();
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public Button( Skin skin, string styleName )
        : base( skin )
    {
        Initialize();
        Style = ( ButtonStyle )skin.Get< ButtonStyle >( styleName );
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public Button( Actor child, Skin skin, string styleName )
        : this( child, ( Skin )skin.Get< ButtonStyle >( styleName ) )
    {
        base.Skin = skin;
    }

    public Button( Actor child, ButtonStyle style )
    {
        Initialize();
        Add( child );
        Style = style;
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    public Button( ButtonStyle style )
    {
        Initialize();
        Style = style;
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    /// <summary>
    /// Creates a button without setting the style or size.
    /// At least a style must be set before using this button.
    /// </summary>
    public Button()
    {
        Initialize();
    }

    public Button( IDrawable? up )
        : this( new ButtonStyle( up, null, null ) )
    {
    }

    public Button( IDrawable? up, IDrawable? down )
        : this( new ButtonStyle( up, down, null ) )
    {
    }

    public Button( IDrawable? upImage, IDrawable? downImage, IDrawable? checkedImage )
        : this( new ButtonStyle( upImage, downImage, checkedImage ) )
    {
    }

    public Button( Actor child, Skin skin )
        : this( child, skin.Get< ButtonStyle >() )
    {
    }

    #endregion constructors

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private void Initialize()
    {
        Touchable = Touchable.Enabled;

        this.ClickListener = new ButtonClickListener( this );

        AddListener( this.ClickListener! );
    }

    public void SetChecked( bool isChecked )
    {
        SetChecked( isChecked, _programmaticChangeEvents );
    }

    public void SetChecked( bool isChecked, bool fireEvent )
    {
        if ( this.IsChecked == isChecked )
        {
            return;
        }

        if ( ( ButtonGroup != null ) && !ButtonGroup.CanCheck( this, isChecked ) )
        {
            return;
        }

        this.IsChecked = isChecked;

        if ( fireEvent )
        {
            ChangeListener.ChangeEvent changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

            if ( Fire( changeEvent ) )
            {
                this.IsChecked = !isChecked;
            }

            Pools< ChangeListener.ChangeEvent >.Free( changeEvent );
        }
    }

    /// <summary>
    /// If false, <see cref="SetChecked(bool)"/> and <see cref="Toggle()"/> will not
    /// fire <see cref="ChangeListener.ChangeEvent()"/>.
    /// The event will only be fired when the user clicks the button
    /// </summary>
    public void SetProgrammaticChangeEvents( bool programmaticChangeEvents )
    {
        this._programmaticChangeEvents = programmaticChangeEvents;
    }

    /// <summary>
    /// Returns the button's style. Modifying the returned style may not have an
    /// effect until <see cref="Style"/> set() is called.
    /// </summary>
    public ButtonStyle? Style
    {
        get => _style;
        // ReSharper disable once PropertyCanBeMadeInitOnly.Global
        set
        {
            this._style = value ?? throw new ArgumentException( "style cannot be null." );

            SetBackground( GetBackgroundDrawable() );
        }
    }

    /// <summary>
    /// Returns appropriate background drawable from the style based on the current button state.
    /// </summary>
    public IDrawable? GetBackgroundDrawable()
    {
        if ( IsDisabled && ( Style?.Disabled != null ) )
        {
            return Style.Disabled;
        }

        if ( IsPressed() )
        {
            if ( IsChecked && ( Style?.CheckedDown != null ) )
            {
                return Style.CheckedDown;
            }

            if ( Style?.Down != null )
            {
                return Style.Down;
            }
        }

        if ( IsOver() )
        {
            if ( IsChecked )
            {
                if ( Style?.CheckedOver != null )
                {
                    return Style.CheckedOver;
                }
            }
            else
            {
                if ( Style?.Over != null )
                {
                    return Style.Over;
                }
            }
        }

        var focused = HasKeyboardFocus();

        if ( IsChecked )
        {
            if ( focused && ( Style?.CheckedFocused != null ) )
            {
                return Style.CheckedFocused;
            }

            if ( Style?.Checked != null )
            {
                return Style.Checked;
            }

            if ( IsOver() && ( Style?.Over != null ) )
            {
                return Style.Over;
            }
        }

        if ( focused && ( Style?.Focused != null ) )
        {
            return Style.Focused;
        }

        return Style?.Up;
    }

    public new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        SetBackground( GetBackgroundDrawable() );

        float? offsetX;
        float? offsetY;

        if ( IsPressed() && !IsDisabled )
        {
            offsetX = Style?.PressedOffsetX;
            offsetY = Style?.PressedOffsetY;
        }
        else if ( IsChecked && !IsDisabled )
        {
            offsetX = Style?.CheckedOffsetX;
            offsetY = Style?.CheckedOffsetY;
        }
        else
        {
            offsetX = Style?.UnpressedOffsetX;
            offsetY = Style?.UnpressedOffsetY;
        }

        var offset = ( offsetX != 0 ) || ( offsetY != 0 );

        if ( offset )
        {
            foreach ( Actor actor in Children )
            {
                actor.MoveBy( ( float )offsetX!, ( float )offsetY! );
            }
        }

        base.Draw( batch, parentAlpha );

        if ( offset )
        {
            for ( int i = 0; i < Children.Size; i++ )
            {
                Children.Get( i ).MoveBy( ( float )-offsetX!, ( float )-offsetY! );
            }
        }

        if ( Stage is { ActionsRequestRendering: true }
             && ( IsPressed() != ClickListener?.Pressed ) )
        {
            Gdx.Graphics.RequestRendering();
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    #region properties

    public new float PrefWidth
    {
        get
        {
            var width = GetPrefWidth();

            if ( Style?.Up != null )
            {
                width = Math.Max( width, Style.Up.MinWidth );
            }

            if ( Style?.Down != null )
            {
                width = Math.Max( width, Style.Down.MinWidth );
            }

            if ( Style?.Checked != null )
            {
                width = Math.Max( width, Style.Checked.MinWidth );
            }

            return width;
        }
    }

    public new float PrefHeight
    {
        get
        {
            var height = base.GetPrefHeight();

            if ( Style?.Up != null )
            {
                height = Math.Max( height, Style.Up.MinHeight );
            }

            if ( Style?.Down != null )
            {
                height = Math.Max( height, Style.Down.MinHeight );
            }

            if ( Style?.Checked != null )
            {
                height = Math.Max( height, Style.Checked!.MinHeight );
            }

            return height;
        }
    }

    public ButtonClickListener?  ClickListener { get; set; }
    public bool                  IsChecked     { get; private set; }
    public bool                  IsDisabled    { get; set; }
    public ButtonGroup< Button > ButtonGroup   { get; set; }

    #endregion properties

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public     void  Toggle()    => SetChecked( !IsChecked );
    public     bool  IsPressed() => ClickListener!.VisualPressed;
    public     bool  IsOver()    => ClickListener!.Over;
    public new float MinWidth    => PrefWidth;
    public new float MinHeight   => PrefHeight;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class ButtonClickListener : ClickListener
    {
        private readonly Button _button;

        public ButtonClickListener( Button button )
        {
            this._button = button;
        }

        public new void Clicked( InputEvent inputEvent, float x, float y )
        {
            if ( _button.IsDisabled )
            {
                return;
            }

            _button.SetChecked( !_button.IsChecked, true );
        }
    }

    /// <summary>
    /// The style for a button, see <see cref="Button"/>.
    /// </summary>
    [PublicAPI]
    public class ButtonStyle
    {
        public IDrawable? Up               { get; set; }
        public IDrawable? Down             { get; set; }
        public IDrawable? Over             { get; set; }
        public IDrawable? Focused          { get; set; }
        public IDrawable? Disabled         { get; set; }
        public IDrawable? Checked          { get; set; }
        public IDrawable? CheckedOver      { get; set; }
        public IDrawable? CheckedDown      { get; set; }
        public IDrawable? CheckedFocused   { get; set; }
        public float      PressedOffsetX   { get; set; }
        public float      PressedOffsetY   { get; set; }
        public float      UnpressedOffsetX { get; set; }
        public float      UnpressedOffsetY { get; set; }
        public float      CheckedOffsetX   { get; set; }
        public float      CheckedOffsetY   { get; set; }

        public ButtonStyle()
        {
        }

        public ButtonStyle( IDrawable? up, IDrawable? down, IDrawable? ischecked )
        {
            this.Up   = up;
            this.Down = down;

            this.Checked = ischecked;
        }

        public ButtonStyle( ButtonStyle style )
        {
            Up       = style.Up;
            Down     = style.Down;
            Over     = style.Over;
            Focused  = style.Focused;
            Disabled = style.Disabled;

            Checked = style.Checked;

            CheckedOver    = style.CheckedOver;
            CheckedDown    = style.CheckedDown;
            CheckedFocused = style.CheckedFocused;

            PressedOffsetX   = style.PressedOffsetX;
            PressedOffsetY   = style.PressedOffsetY;
            UnpressedOffsetX = style.UnpressedOffsetX;
            UnpressedOffsetY = style.UnpressedOffsetY;
            CheckedOffsetX   = style.CheckedOffsetX;
            CheckedOffsetY   = style.CheckedOffsetY;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

//    ( clickListener = new ClickListener()
//            {


//                public void clicked( InputEvent event, float x, float y )
//                {
//                if ( isDisabled() ) return;
//                setChecked( !isChecked, true );
//            }

//        });
//    }
}