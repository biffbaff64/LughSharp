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

using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils.Exceptions;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

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
    private bool         _programmaticChangeEvents = true;
    private ButtonStyle? _style;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a button without setting the style or size.
    /// At least a style must be set before using this button.
    /// </summary>
    public Button()
    {
        Initialise();
    }

    public Button( Skin skin ) : base( skin )
    {
        Initialise();

        ConstructorHelper( skin.Get< ButtonStyle >() );
    }

    public Button( Skin skin, string styleName )
        : base( skin )
    {
        Initialise();

        ConstructorHelper( skin.Get< ButtonStyle >( styleName ) );
    }

    public Button( Actor child, Skin skin, string styleName )
        : this( child, skin.Get< ButtonStyle >( styleName ) )
    {
        Skin = skin;
    }

    public Button( Actor child, ButtonStyle style )
    {
        Initialise();
        Add( child );

        ConstructorHelper( style );
    }

    public Button( ButtonStyle style )
    {
        Initialise();

        ConstructorHelper( style );
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

    /// <summary>
    /// Returns the button's style. Modifying the returned style may not have an
    /// effect until <see cref="Style"/> set() is called.
    /// </summary>
    public virtual ButtonStyle? Style
    {
        get => _style;

        set
        {
            _style = value ?? throw new ArgumentException( "style cannot be null." );

            SetBackground( GetBackgroundDrawable() );
        }
    }

    /// <summary>
    /// Somewhere to call virtual methods that used to be called
    /// from constructors.
    /// </summary>
    private void ConstructorHelper( ButtonStyle style )
    {
        Style = style;
        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    private void Initialise()
    {
        Touchable = Touchable.Enabled;

        ClickListener = new ButtonClickListener( this );

        AddListener( ClickListener! );
    }

    public void SetChecked( bool isChecked )
    {
        SetChecked( isChecked, _programmaticChangeEvents );
    }

    public void SetChecked( bool isChecked, bool fireEvent )
    {
        if ( IsChecked == isChecked )
        {
            return;
        }

        if ( ( ButtonGroup != null ) && !ButtonGroup.CanCheck( this, isChecked ) )
        {
            return;
        }

        IsChecked = isChecked;

        if ( fireEvent )
        {
            var changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

            if ( changeEvent is not null )
            {
                if ( Fire( changeEvent ) )
                {
                    IsChecked = !isChecked;
                }

                Pools< ChangeListener.ChangeEvent >.Free( changeEvent );
            }
        }
    }

    /// <summary>
    /// If false, <see cref="SetChecked(bool)"/> and <see cref="Toggle()"/> will not
    /// fire <see cref="ChangeListener.ChangeEvent()"/>.
    /// The event will only be fired when the user clicks the button
    /// </summary>
    public void SetProgrammaticChangeEvents( bool programmaticChangeEvents )
    {
        _programmaticChangeEvents = programmaticChangeEvents;
    }

    /// <summary>
    /// Returns appropriate background drawable from the style based on the current button state.
    /// </summary>
    public virtual IDrawable? GetBackgroundDrawable()
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

    /// <inheritdoc />
    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        SetBackground( GetBackgroundDrawable() );

        float offsetX;
        float offsetY;

        GdxRuntimeException.ThrowIfNull( Style );

        if ( IsPressed() && !IsDisabled )
        {
            offsetX = Style.PressedOffsetX;
            offsetY = Style.PressedOffsetY;
        }
        else if ( IsChecked && !IsDisabled )
        {
            offsetX = Style.CheckedOffsetX;
            offsetY = Style.CheckedOffsetY;
        }
        else
        {
            offsetX = Style.UnpressedOffsetX;
            offsetY = Style.UnpressedOffsetY;
        }

        var offset = ( offsetX != 0 ) || ( offsetY != 0 );

        if ( offset )
        {
            foreach ( var actor in Children )
            {
                actor.MoveBy( offsetX, offsetY );
            }
        }

        base.Draw( batch, parentAlpha );

        if ( offset )
        {
            for ( var i = 0; i < Children.Size; i++ )
            {
                Children.GetAt( i ).MoveBy( -offsetX, -offsetY );
            }
        }

        if ( Stage is { ActionsRequestRendering: true }
          && ( IsPressed() != ClickListener?.Pressed ) )
        {
            Gdx.Graphics.RequestRendering();
        }
    }

    // ========================================================================
    // ========================================================================

    #region properties

    public          void  Toggle()    => SetChecked( !IsChecked );
    public          bool  IsPressed() => ClickListener!.VisualPressed;
    public          bool  IsOver()    => ClickListener!.Over;
    public override float MinWidth    => PrefWidth;
    public override float MinHeight   => PrefHeight;

    public ButtonClickListener?   ClickListener { get; set; }
    public bool                   IsChecked     { get; private set; }
    public bool                   IsDisabled    { get; set; }
    public ButtonGroup< Button >? ButtonGroup   { get; set; }

    /// <inheritdoc />
    public override float PrefWidth
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

    /// <inheritdoc />
    public override float PrefHeight
    {
        get
        {
            var height = GetPrefHeight();

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

    #endregion properties

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class ButtonClickListener : ClickListener
    {
        private readonly Button _button;

        public ButtonClickListener( Button button )
        {
            _button = button;
        }

        public override void OnClicked( InputEvent inputEvent, float x, float y )
        {
            if ( _button.IsDisabled )
            {
                return;
            }

            _button.SetChecked( !_button.IsChecked, true );
        }
    }

    // ========================================================================
    // ========================================================================

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

        // ====================================================================

        public ButtonStyle()
        {
        }

        public ButtonStyle( IDrawable? up, IDrawable? down, IDrawable? ischecked )
        {
            Up      = up;
            Down    = down;
            Checked = ischecked;
        }

        public ButtonStyle( ButtonStyle style )
        {
            Up               = style.Up;
            Down             = style.Down;
            Over             = style.Over;
            Focused          = style.Focused;
            Disabled         = style.Disabled;
            Checked          = style.Checked;
            CheckedOver      = style.CheckedOver;
            CheckedDown      = style.CheckedDown;
            CheckedFocused   = style.CheckedFocused;
            PressedOffsetX   = style.PressedOffsetX;
            PressedOffsetY   = style.PressedOffsetY;
            UnpressedOffsetX = style.UnpressedOffsetX;
            UnpressedOffsetY = style.UnpressedOffsetY;
            CheckedOffsetX   = style.CheckedOffsetX;
            CheckedOffsetY   = style.CheckedOffsetY;
        }
    }
}
