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

using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Scenes.Scene2D.Listeners;
using Corelib.Lugh.Scenes.Scene2D.Utils;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A slider is a horizontal indicator that allows a user to set a value. The slider has
/// a range (min, max) and a stepping between each value the slider represents.
/// <para>
/// <see cref="ChangeListener.ChangeEvent"/> is fired when the
/// slider knob is moved. Canceling the event will move the knob to where it was previously.
/// </para>
/// <para>
/// For a horizontal progress bar, its preferred height is determined by the larger of the
/// knob and background, and the preferred width is 140, a relatively arbitrary size. These
/// parameters are reversed for a vertical progress bar.
/// </para>
/// </summary>
[PublicAPI]
public class Slider : ProgressBar
{
    private int      _draggingPointer = -1;
    private float[]? _snapValues;
    private float    _threshold;

    public Slider( float min, float max, float stepSize, bool vertical, Skin skin )
        : this( min, max, stepSize, vertical, skin.Get< SliderStyle >( "default-" + ( vertical ? "vertical" : "horizontal" ) ) )
    {
    }

    public Slider( float min, float max, float stepSize, bool vertical, Skin skin, string styleName )
        : this( min, max, stepSize, vertical, skin.Get< SliderStyle >( styleName ) )
    {
    }

    /// Creates a new slider. If horizontal, its width is determined by the prefWidth
    /// parameter, its height is determined by the maximum of the height of either the
    /// slider
    /// <see cref="NinePatch"/>
    /// or slider handle
    /// <see cref="TextureRegion"/>
    /// .
    /// The min and max values determine the range the values of this slider can take on,
    /// the stepSize parameter specifies the distance between individual values. E.g. min
    /// could be 4, max could be 10 and stepSize could be 0.2, giving you a total of 30
    /// values, 4.0 4.2, 4.4 and so on.
    /// <param name="min"> the minimum value </param>
    /// <param name="max"> the maximum value </param>
    /// <param name="stepSize"> the step size between values </param>
    /// <param name="vertical"></param>
    /// <param name="style"> the <see cref="SliderStyle"/> </param>
    public Slider( float min, float max, float stepSize, bool vertical, SliderStyle style )
        : base( min, max, stepSize, vertical, style )
    {
        AddListener( new SliderInputListener( this ) );
    }

    public bool MouseOver { get; set; }

    /// <summary>
    /// Sets the mouse button, which can trigger a change of the slider.
    /// Is set to -1, so every button, by default.
    /// </summary>
    public int MouseButton { get; set; } = -1;

    /// <summary>
    /// Sets the inverse interpolation to use for display. This should perform the
    /// inverse of setting <see cref="ProgressBar.VisualInterpolation"/>".
    /// </summary>
    public Interpolator VisualInterpolationInverse { get; set; } = Interpolation.Linear;

    public SliderStyle GetStyle()
    {
        return ( SliderStyle ) Style;
    }

    protected IDrawable? GetBackgroundDrawable()
    {
        var style = ( SliderStyle ) Style;

        if ( IsDisabled && ( style.DisabledBackground != null ) )
        {
            return style.DisabledBackground;
        }

        if ( IsDragging() && ( style.BackgroundDown != null ) )
        {
            return style.BackgroundDown;
        }

        if ( MouseOver && ( style.BackgroundOver != null ) )
        {
            return style.BackgroundOver;
        }

        return style.Background;
    }

    protected IDrawable? GetKnobDrawable()
    {
        var style = ( SliderStyle ) Style;

        if ( IsDisabled && ( style.DisabledKnob != null ) )
        {
            return style.DisabledKnob;
        }

        if ( IsDragging() && ( style.KnobDown != null ) )
        {
            return style.KnobDown;
        }

        if ( MouseOver && ( style.KnobOver != null ) )
        {
            return style.KnobOver;
        }

        return style.Knob;
    }

    protected IDrawable? GetKnobBeforeDrawable()
    {
        var style = ( SliderStyle ) Style;

        if ( IsDisabled && ( style.DisabledKnobBefore != null ) )
        {
            return style.DisabledKnobBefore;
        }

        if ( IsDragging() && ( style.KnobBeforeDown != null ) )
        {
            return style.KnobBeforeDown;
        }

        if ( MouseOver && ( style.KnobBeforeOver != null ) )
        {
            return style.KnobBeforeOver;
        }

        return style.KnobBefore;
    }

    protected IDrawable? GetKnobAfterDrawable()
    {
        var style = ( SliderStyle ) Style;

        if ( IsDisabled && ( style.DisabledKnobAfter != null ) )
        {
            return style.DisabledKnobAfter;
        }

        if ( IsDragging() && ( style.KnobAfterDown != null ) )
        {
            return style.KnobAfterDown;
        }

        if ( MouseOver && ( style.KnobAfterOver != null ) )
        {
            return style.KnobAfterOver;
        }

        return style.KnobAfter;
    }

    private bool CalculatePositionAndValue( float x, float y )
    {
        var style = GetStyle();
        var knob  = style.Knob;
        var bg    = GetBackgroundDrawable();

        float value;
        var   oldPosition = KnobPosition;

        var min = MinValue;
        var max = MaxValue;

        if ( IsVertical )
        {
            var height     = Height - bg!.TopHeight - bg.BottomHeight;
            var knobHeight = knob?.MinHeight ?? 0;

            KnobPosition = y - bg.BottomHeight - ( knobHeight * 0.5f );
            value        = min + ( ( max - min ) * VisualInterpolationInverse.Apply( KnobPosition / ( height - knobHeight ) ) );
            KnobPosition = Math.Max( Math.Min( 0, bg.BottomHeight ), KnobPosition );
            KnobPosition = Math.Min( height - knobHeight, KnobPosition );
        }
        else
        {
            var width     = Width - bg!.LeftWidth - bg.RightWidth;
            var knobWidth = knob?.MinWidth ?? 0;

            KnobPosition = x - bg.LeftWidth - ( knobWidth * 0.5f );
            value        = min + ( ( max - min ) * VisualInterpolationInverse.Apply( KnobPosition / ( width - knobWidth ) ) );
            KnobPosition = Math.Max( Math.Min( 0, bg.LeftWidth ), KnobPosition );
            KnobPosition = Math.Min( width - knobWidth, KnobPosition );
        }

        var oldValue = value;

        if ( !Gdx.Input.IsKeyPressed( IInput.Keys.SHIFT_LEFT )
          && !Gdx.Input.IsKeyPressed( IInput.Keys.SHIFT_RIGHT ) )
        {
            value = GetSnapped( value );
        }

        var valueSet = SetBarPosition( value );

        if ( value.Equals( oldValue ) )
        {
            KnobPosition = oldPosition;
        }

        return valueSet;
    }

    protected float GetSnapped( float value )
    {
        if ( _snapValues is { Length: 0 } )
        {
            return value;
        }

        float bestDiff = -1, bestValue = 0;

        foreach ( var snapValue in _snapValues! )
        {
            var diff = Math.Abs( value - snapValue );

            if ( diff <= _threshold )
            {
                if ( bestDiff.Equals( -1 ) || ( diff < bestDiff ) )
                {
                    bestDiff  = diff;
                    bestValue = snapValue;
                }
            }
        }

        return bestDiff.Equals( -1f ) ? value : bestValue;
    }

    /// <summary>
    /// Will make this progress bar snap to the specified values, if the
    /// knob is within the threshold.
    /// </summary>
    /// <param name="values"> May be null. </param>
    /// <param name="threshld"></param>
    public void SetSnapToValues( float[] values, float threshld )
    {
        _snapValues = values;
        _threshold  = threshld;
    }

    /// <summary>
    /// Returns true if the slider is being dragged.
    /// </summary>
    public bool IsDragging()
    {
        return _draggingPointer != -1;
    }

    /// <summary>
    /// Sets the value using the specified visual percent.
    /// </summary>
    public void SetVisualPercent( float percent )
    {
        Value = MinValue + ( ( MaxValue - MinValue ) * VisualInterpolationInverse.Apply( percent ) );
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SliderStyle : ProgressBarStyle
    {
        public SliderStyle()
        {
        }

        public SliderStyle( IDrawable background, IDrawable knob )
            : base( background, knob )
        {
        }

        public SliderStyle( SliderStyle style ) : base( style )
        {
            BackgroundOver = style.BackgroundOver;
            BackgroundDown = style.BackgroundDown;

            KnobOver = style.KnobOver;
            KnobDown = style.KnobDown;

            KnobBeforeOver = style.KnobBeforeOver;
            KnobBeforeDown = style.KnobBeforeDown;

            KnobAfterOver = style.KnobAfterOver;
            KnobAfterDown = style.KnobAfterDown;
        }

        public IDrawable? BackgroundOver { get; set; }
        public IDrawable? BackgroundDown { get; set; }
        public IDrawable? KnobBeforeOver { get; set; }
        public IDrawable? KnobOver       { get; set; }
        public IDrawable? KnobAfterOver  { get; set; }
        public IDrawable? KnobBeforeDown { get; set; }
        public IDrawable? KnobDown       { get; set; }
        public IDrawable? KnobAfterDown  { get; set; }
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class SliderInputListener : InputListener
    {
        private readonly Slider _parent;

        public SliderInputListener( Slider parent )
        {
            _parent = parent;
        }

        public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( _parent.IsDisabled )
            {
                return false;
            }

            if ( ( _parent.MouseButton != -1 ) && ( _parent.MouseButton != button ) )
            {
                return false;
            }

            if ( _parent._draggingPointer != -1 )
            {
                return false;
            }

            _parent._draggingPointer = pointer;
            _parent.CalculatePositionAndValue( x, y );

            return true;
        }

        public override void TouchUp( InputEvent? ev, float x, float y, int pointer, int button )
        {
            if ( pointer != _parent._draggingPointer )
            {
                return;
            }

            // The position is invalid when focus is cancelled
            if ( ev!.TouchFocusCancel || !_parent.CalculatePositionAndValue( x, y ) )
            {
                // Fire an event on touchUp even if the value didn't change, so
                // listeners can see when a drag ends via isDragging.
                var changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

                _parent.Fire( changeEvent );
                Pools< ChangeListener.ChangeEvent >.Free( changeEvent );
            }
        }

        public override void TouchDragged( InputEvent? ev, float x, float y, int pointer )
        {
            _parent.CalculatePositionAndValue( x, y );
        }

        public override void Enter( InputEvent? ev, float x, float y, int pointer, Actor? from )
        {
            if ( pointer == -1 )
            {
                _parent.MouseOver = true;
            }
        }

        public override void Exit( InputEvent? ev, float x, float y, int pointer, Actor? to )
        {
            if ( pointer == -1 )
            {
                _parent.MouseOver = false;
            }
        }
    }
}
