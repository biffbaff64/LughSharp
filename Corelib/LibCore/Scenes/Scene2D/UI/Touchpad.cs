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
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Listeners;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// An on-screen joystick. The movement area of the joystick is circular, centered on the
/// touchpad, and its size determined by the smaller touchpad dimension.
/// <para>
/// The preferred size of the touchpad is determined by the background.
/// </para>
/// <para>
/// A <see cref="ChangeListener.ChangeEvent"/> is fired when the touchpad knob is moved.
/// Cancelling the event will move the knob to where it was previously.
/// </para>
/// </summary>
[PublicAPI]
public class Touchpad : Widget
{
    private readonly Circle  _deadzoneBounds = new( 0, 0, 0 );
    private readonly Circle  _knobBounds     = new( 0, 0, 0 );
    private readonly Vector2 _knobPercent    = new();
    private readonly Vector2 _knobPosition   = new();
    private readonly Circle  _touchBounds    = new( 0, 0, 0 );
    private          float   _deadzoneRadius;

    private TouchpadStyle _style = null!;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="deadzoneRadius">
    /// The distance in pixels from the center of the touchpad required for the knob to be moved.
    /// </param>
    /// <param name="skin"></param>
    public Touchpad( float deadzoneRadius, Skin skin )
        : this( deadzoneRadius, skin.Get< TouchpadStyle >() )
    {
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="deadzoneRadius">
    /// The distance in pixels from the center of the touchpad required for the knob to be moved.
    /// </param>
    /// <param name="skin"></param>
    /// <param name="styleName"></param>
    public Touchpad( float deadzoneRadius, Skin skin, string styleName )
        : this( deadzoneRadius, skin.Get< TouchpadStyle >( styleName ) )
    {
    }

    /// <summary>
    /// Constructs a new Touchpad using the supplied <see cref="TouchpadStyle"/>
    /// and deadzone radius.
    /// </summary>
    /// <param name="deadzoneRadius">
    /// The distance in pixels from the center of the touchpad required for the knob to be moved.
    /// </param>
    /// <param name="style"></param>
    public Touchpad( float deadzoneRadius, TouchpadStyle style )
    {
        ConstructorHelper( deadzoneRadius, style );
    }

    // ------------------------------------------------------------------------

    private void ConstructorHelper( float deadzoneRadius, TouchpadStyle style )
    {
        if ( deadzoneRadius <= 0 )
        {
            throw new ArgumentException( "deadzoneRadius must be > 0" );
        }

        _deadzoneRadius = deadzoneRadius;

        _knobPosition.Set( Width / 2f, Height / 2f );

        Style = style;

        SetSize( Style.Background?.MinWidth ?? 0, Style.Background?.MinHeight ?? 0 );

        AddListener( new TouchpadInputListener( this ) );
    }
    
    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns the x-position of the knob relative to the center of the widget.
    /// The positive direction is right.
    /// </summary>
    public float KnobX => _knobPosition.X;

    /// <summary>
    /// Returns the y-position of the knob relative to the center of the widget.
    /// The positive direction is up.
    /// </summary>
    public float KnobY => _knobPosition.Y;

    /// <summary>
    /// Returns the x-position of the knob as a percentage from the center of the touchpad
    /// to the edge of the circular movement area. The positive direction is right.
    /// </summary>
    public float KnobPercentX => _knobPercent.X;

    /// <summary>
    /// Returns the y-position of the knob as a percentage from the center of the touchpad
    /// to the edge of the circular movement area. The positive direction is up.
    /// </summary>
    public float KnobPercentY => _knobPercent.Y;

    // ------------------------------------------------------------------------

    private void CalculatePositionAndValue( float x, float y, bool isTouchUp )
    {
        var oldPositionX = _knobPosition.X;
        var oldPositionY = _knobPosition.Y;
        var oldPercentX  = _knobPercent.X;
        var oldPercentY  = _knobPercent.Y;
        var centerX      = _knobBounds.X;
        var centerY      = _knobBounds.Y;

        _knobPosition.Set( centerX, centerY );
        _knobPercent.Set( 0f, 0f );

        if ( !isTouchUp )
        {
            if ( !_deadzoneBounds.Contains( x, y ) )
            {
                _knobPercent.Set( ( x - centerX ) / _knobBounds.Radius, ( y - centerY ) / _knobBounds.Radius );

                var length = _knobPercent.Len();

                if ( length > 1 )
                {
                    _knobPercent.Scale( 1 / length );
                }

                if ( _knobBounds.Contains( x, y ) )
                {
                    _knobPosition.Set( x, y );
                }
                else
                {
                    _knobPosition.Set( _knobPercent ).Nor().Scale( _knobBounds.Radius ).Add( _knobBounds.X, _knobBounds.Y );
                }
            }
        }

        if ( !oldPercentX.Equals( _knobPercent.X ) || !oldPercentY.Equals( _knobPercent.Y ) )
        {
            var changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

            if ( Fire( changeEvent ) )
            {
                _knobPercent.Set( oldPercentX, oldPercentY );
                _knobPosition.Set( oldPositionX, oldPositionY );
            }

            Pools< ChangeListener.ChangeEvent >.Free( changeEvent );
        }
    }

    public override Actor? Hit( float x, float y, bool touchable )
    {
        if ( touchable && ( Touchable != Touchable.Enabled ) )
        {
            return null;
        }

        if ( !IsVisible )
        {
            return null;
        }

        return _touchBounds.Contains( x, y ) ? this : null;
    }

    public void Layout()
    {
        // Recalc pad and deadzone bounds
        var halfWidth  = Width / 2;
        var halfHeight = Height / 2;
        var radius     = Math.Min( halfWidth, halfHeight );

        _touchBounds.Set( halfWidth, halfHeight, radius );

        if ( _style.Knob != null )
        {
            radius -= Math.Max( _style.Knob.MinWidth, _style.Knob.MinHeight ) / 2;
        }

        _knobBounds.Set( halfWidth, halfHeight, radius );
        _deadzoneBounds.Set( halfWidth, halfHeight, _deadzoneRadius );

        // Recalc pad values and knob position
        _knobPosition.Set( halfWidth, halfHeight );
        _knobPercent.Set( 0, 0 );
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        var c = Color;
        batch.SetColor( c.R, c.G, c.B, c.A * parentAlpha );

        var x = X;
        var y = Y;

        var bg = _style.Background;

        bg?.Draw( batch, x, y, Width, Height );

        var knob = _style.Knob;

        if ( knob != null )
        {
            x += _knobPosition.X - ( knob.MinWidth / 2f );
            y += _knobPosition.Y - ( knob.MinHeight / 2f );

            knob.Draw( batch, x, y, knob.MinWidth, knob.MinHeight );
        }
    }

    /// <summary>
    /// The distance in pixels from the center of the touchpad required for the knob to be moved.
    /// </summary>
    public void SetDeadzone( float deadzoneRadius )
    {
        if ( deadzoneRadius < 0 )
        {
            throw new ArgumentException( "deadzoneRadius must be > 0" );
        }

        _deadzoneRadius = deadzoneRadius;

        Invalidate();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    internal class TouchpadInputListener : InputListener
    {
        private readonly Touchpad _pad;

        public TouchpadInputListener( Touchpad pad )
        {
            _pad = pad;
        }

        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( _pad.IsTouched )
            {
                return false;
            }

            _pad.IsTouched = true;
            _pad.CalculatePositionAndValue( x, y, false );

            return true;
        }

        public override void TouchDragged( InputEvent? ev, float x, float y, int pointer )
        {
            _pad.CalculatePositionAndValue( x, y, false );
        }

        public override void TouchUp( InputEvent? ev, float x, float y, int pointer, int button )
        {
            _pad.IsTouched = false;
            _pad.CalculatePositionAndValue( x, y, _pad.ResetOnTouchUp );
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class TouchpadStyle
    {
        public TouchpadStyle()
        {
        }

        public TouchpadStyle( IDrawable? background, IDrawable? knob )
        {
            Background = background;
            Knob       = knob;
        }

        public TouchpadStyle( TouchpadStyle style )
        {
            Background = style.Background;
            Knob       = style.Knob;
        }

        // Stretched in both directions.
        public IDrawable? Background { get; set; }

        public IDrawable? Knob { get; set; }
    }

    // ------------------------------------------------------------------------

    #region properties

    public TouchpadStyle Style
    {
        get => _style;
        set
        {
            ArgumentNullException.ThrowIfNull( value );

            _style = value;

            InvalidateHierarchy();
        }
    }

    public bool IsTouched { get; set; }

    public override float PrefWidth => _style.Background?.MinWidth ?? 0;

    public override float PrefHeight => _style.Background?.MinHeight ?? 0;

    /// <summary>
    /// Whether to reset the knob to the center on touch up.
    /// </summary>
    public bool ResetOnTouchUp { get; set; } = true;

    #endregion properties
}
