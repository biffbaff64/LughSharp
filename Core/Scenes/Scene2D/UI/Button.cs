using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;
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
public class Button : Table, IDisableable
{
    private ButtonStyle? _style;
    private bool isChecked;
    private bool isDisabled;
    private ButtonGroup buttonGroup;
    private ClickListener clickListener;
    private bool programmaticChangeEvents = true;

    public Button( Skin skin ) : base( skin )
    {
        initialize();
        setStyle( skin.get( ButtonStyle.class));
        setSize( GetPrefWidth(), GetPrefHeight() );
}

public Button( Skin skin, String styleName )
    : base( skin )
{
    initialize();
    setStyle( skin.get( styleName, ButtonStyle.class));
        setSize( GetPrefWidth(), GetPrefHeight() );
    }

    public Button( Actor child, Skin skin, String styleName )
{
    this( child, skin.get( styleName, ButtonStyle.class));
SetSkin( skin );
    }

    public Button( Actor child, ButtonStyle style )
{
    initialize();
    add( child );
    setStyle( style );
    setSize( GetPrefWidth(), GetPrefHeight() );
}

public Button( ButtonStyle style )
{
    initialize();
    setStyle( style );
    setSize( GetPrefWidth(), GetPrefHeight() );
}

/** Creates a button without setting the style or size. At least a style must be set before using this button. */
public Button()
{
    initialize();
}

private void initialize()
{
    setTouchable( Touchable.enabled );

    addListener
        ( clickListener = new ClickListener()
        {




                public void clicked( InputEvent event, float x, float y )
    {
        if ( isDisabled() ) return;
        setChecked( !isChecked, true );
    }

});
    }

    public Button( @Null Drawable up )
{
    this( new ButtonStyle( up, null, null ) );
}

public Button( @Null Drawable up, @Null Drawable down)
{
    this( new ButtonStyle( up, down, null ) );
}

public Button( @Null Drawable up, @Null Drawable down, @Null Drawable checked)
{
    this( new ButtonStyle( up, down, checked ) );
}

public Button( Actor child, Skin skin )
        : this( child, skin.Get( ButtonStyle.class))
    {
}

public void setChecked( bool isChecked )
{
    setChecked( isChecked, programmaticChangeEvents );
}

void setChecked( bool isChecked, bool fireEvent )
{
    if ( this.isChecked == isChecked ) return;
    if ( buttonGroup != null && !buttonGroup.canCheck( this, isChecked ) ) return;
    this.isChecked = isChecked;

    if ( fireEvent )
    {
        ChangeListener.ChangeEvent changeEvent = Pools<>.obtain( ChangeListener.ChangeEvent.class);
if ( fire( changeEvent ) ) this.isChecked = !isChecked;
Pools.free( changeEvent );
        }
    }

    /** Toggles the checked state. This method changes the checked state, which fires a {@link ChangeEvent} (if programmatic change
	 * events are enabled), so can be used to simulate a button click. */
    public void toggle()
{
    setChecked( !isChecked );
}

public bool isChecked()
{
    return isChecked;
}

public bool isPressed()
{
    return clickListener.isVisualPressed();
}

public bool isOver()
{
    return clickListener.isOver();
}

public ClickListener getClickListener()
{
    return clickListener;
}

public bool isDisabled()
{
    return isDisabled;
}

/** When true, the button will not toggle {@link #isChecked()} when clicked and will not fire a {@link ChangeEvent}. */
public void setDisabled( bool isDisabled )
{
    this.isDisabled = isDisabled;
}

/** If false, {@link #setChecked(bool)} and {@link #toggle()} will not fire {@link ChangeEvent}. The event will only be
 * fired only when the user clicks the button */
public void setProgrammaticChangeEvents( bool programmaticChangeEvents )
{
    this.programmaticChangeEvents = programmaticChangeEvents;
}

public void setStyle( ButtonStyle style )
{
    if ( style == null ) throw new IllegalArgumentException( "style cannot be null." );
    this._style = style;

    setBackground( getBackgroundDrawable() );
}

/** Returns the button's style. Modifying the returned style may not have an effect until {@link #setStyle(ButtonStyle)} is
 * called. */
public ButtonStyle getStyle()
{
    return _style;
}

/** @return May be null. */
public @Null ButtonGroup getButtonGroup()
    {
    return buttonGroup;
}

/** Returns appropriate background drawable from the style based on the current button state. */
protected IDrawable? GetBackgroundDrawable()
{
    if ( isDisabled() && ( _style.Disabled != null ) ) return _style.Disabled;

    if ( IsPressed() )
    {
        if ( isChecked() && _style.CheckedDown != null ) return _style.CheckedDown;
        if ( _style.Down != null ) return _style.Down;
    }

    if ( isOver() )
    {
        if ( isChecked() )
        {
            if ( _style.CheckedOver != null ) return _style.CheckedOver;
        }
        else
        {
            if ( _style.Over != null ) return _style.Over;
        }
    }

    bool focused = hasKeyboardFocus();

    if ( isChecked() )
    {
        if ( focused && _style.CheckedFocused != null ) return _style.CheckedFocused;
        if ( _style.checked != null) return _style.checked

            ;

if ( isOver() && _style.Over != null ) return _style.Over;
}

if ( focused && _style.Focused != null ) return _style.Focused;

return _style.Up;
    }

    public new void Draw( IBatch batch, float parentAlpha )
{
    Validate();

    SetBackground( getBackgroundDrawable() );

    float offsetX = 0, offsetY = 0;

    if ( isPressed() && !isDisabled() )
    {
        offsetX = _style.PressedOffsetX;
        offsetY = _style.PressedOffsetY;
    }
    else if ( isChecked() && !isDisabled() )
    {
        offsetX = _style.CheckedOffsetX;
        offsetY = _style.CheckedOffsetY;
    }
    else
    {
        offsetX = _style.UnpressedOffsetX;
        offsetY = _style.UnpressedOffsetY;
    }

    var offset = ( offsetX != 0 ) || ( offsetY != 0 );

    SnapshotArray<Actor> children = Children;

    if ( offset )
    {
        foreach ( Actor actor in children )
        {
            actor.MoveBy( offsetX, offsetY );
        }
    }

    base.Draw( batch, parentAlpha );

    if ( offset )
    {
        for ( int i = 0; i < children.Size; i++ )
        {
            children.Get( i ).MoveBy( -offsetX, -offsetY );
        }
    }

    if ( ( Stage != null )
         && Stage.ActionsRequestRendering
         && ( isPressed() != clickListener.Pressed ) )
    {
        Gdx.Graphics.RequestRendering();
    }
}

public float PrefWidth
{
    get
    {
        float width = GetPrefWidth();

        if ( _style.Up != null ) width = Math.Max( width, _style.Up.MinWidth );
        if ( _style.Down != null ) width = Math.Max( width, _style.Down.MinWidth );
        if ( _style.Checked != null ) width = Math.Max( width, _style.Checked.MinWidth );

        return width;
    }
}

public float PrefHeight
{
    get
    {
        float height = base.GetPrefHeight();

        if ( _style.Up != null ) height = Math.Max( height, _style.Up.MinHeight );
        if ( _style.Down != null ) height = Math.Max( height, _style.Down.MinHeight );
        if ( _style.Checked != null ) height = Math.Max( height, _style.Checked!.MinHeight );

        return height;
    }
}

public float MinWidth => PrefWidth;
public float MinHeight => PrefHeight;

/// <summary>
/// The style for a button, see <see cref="Button"/>.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class ButtonStyle
{
    public IDrawable? Up { get; set; }
    public IDrawable? Down { get; set; }
    public IDrawable? Over { get; set; }
    public IDrawable? Focused { get; set; }
    public IDrawable? Disabled { get; set; }
    public IDrawable? Checked { get; set; }
    public IDrawable? CheckedOver { get; set; }
    public IDrawable? CheckedDown { get; set; }
    public IDrawable? CheckedFocused { get; set; }
    public float PressedOffsetX { get; set; }
    public float PressedOffsetY { get; set; }
    public float UnpressedOffsetX { get; set; }
    public float UnpressedOffsetY { get; set; }
    public float CheckedOffsetX { get; set; }
    public float CheckedOffsetY { get; set; }

    public ButtonStyle()
    {
    }

    public ButtonStyle( IDrawable? up, IDrawable? down, IDrawable? ischecked )
    {
        this.Up = up;
        this.Down = down;

        this.Checked = ischecked;
    }

    public ButtonStyle( ButtonStyle style )
    {
        Up = style.Up;
        Down = style.Down;
        Over = style.Over;
        Focused = style.Focused;
        Disabled = style.Disabled;

        Checked = style.Checked;

        CheckedOver = style.CheckedOver;
        CheckedDown = style.CheckedDown;
        CheckedFocused = style.CheckedFocused;

        PressedOffsetX = style.PressedOffsetX;
        PressedOffsetY = style.PressedOffsetY;
        UnpressedOffsetX = style.UnpressedOffsetX;
        UnpressedOffsetY = style.UnpressedOffsetY;
        CheckedOffsetX = style.CheckedOffsetX;
        CheckedOffsetY = style.CheckedOffsetY;
    }
}
}
