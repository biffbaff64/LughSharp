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

using LibGDXSharp.Graphics.G2D;
using LibGDXSharp.Scenes.Scene2D.Utils;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A slider is a horizontal indicator that allows a user to set a value. The slider has
/// a range (min, max) and a stepping between each value the slider represents.
/// <para>
/// <see cref="LibGDXSharp.Scenes.Listeners.ChangeListener.ChangeEvent"/> is fired when the
/// slider knob is moved. Canceling the event will move the knob to where it was previously.
/// </para>
/// <para>
/// For a horizontal progress bar, its preferred height is determined by the larger of the
/// knob and background, and the preferred width is 140, a relatively arbitrary size. These
/// parameters are reversed for a vertical progress bar.
/// </para>
/// </summary>
public class Slider : ProgressBar
{
    private int          _button          = -1;
    private int          _draggingPointer = -1;
    private bool         _mouseOver;
    private Interpolator _visualInterpolationInverse = Interpolation.linear;
    private float[]      _snapValues;
    private float        threshold;

    public Slider( float min, float max, float stepSize, bool vertical, Skin skin )
        : this( min, max, stepSize, vertical, skin.Get< SliderStyle >( "default-" + ( vertical ? "vertical" : "horizontal" ) ) )
    {
    }

    public Slider( float min, float max, float stepSize, bool vertical, Skin skin, String styleName )
        : this( min, max, stepSize, vertical, skin.Get< SliderStyle >( styleName ) )
    {
    }

    /// Creates a new slider. If horizontal, its width is determined by the prefWidth
    /// parameter, its height is determined by the maximum of the height of either the
    /// slider <see cref="NinePatch"/> or slider handle <see cref="TextureRegion"/>.
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
//        addListener( new InputListener()
//        {
//            public bool touchDown (InputEvent ev, float x, float y, int pointer, int button)
//            {
//                if (disabled) return false;
//                if (Slider.this.button != -1 && Slider.this.button != button) return false;
//                if (draggingPointer != -1) return false;
//                draggingPointer = pointer;
//                calculatePositionAndValue(x, y);
//                return true;
//            }
//
//            public void touchUp( InputEvent ev, float x, float y, int pointer, int button )
//            {
//                if ( pointer != draggingPointer ) return;
//                draggingPointer = -1;
//
//                // The position is invalid when focus is cancelled
//                if (event.isTouchFocusCancel() || !calculatePositionAndValue( x, y )) {
//                    // Fire an event on touchUp even if the value didn't change, so listeners can see when a drag ends via isDragging.
//                    ChangeEvent changeEvent = Pools.obtain( ChangeEvent.class);
//                    fire( changeEvent );
//                    Pools.free( changeEvent );
//                }
//            }
//
//            public void touchDragged( InputEvent ev, float x, float y, int pointer )
//            {
//                calculatePositionAndValue( x, y );
//            }
//
//            public void enter( InputEvent ev, float x, float y, int pointer, Actor fromActor )
//            {
//                if ( pointer == -1 ) mouseOver = true;
//            }
//
//            public void exit( InputEvent ev, float x, float y, int pointer, Actor toActor )
//            {
//                if ( pointer == -1 ) mouseOver = false;
//            }
//        } );
    }

    /// Returns the slider's style. Modifying the returned style may not have an effect until
    /// <see cref="SetStyle(SliderStyle)"/> is called.
    public SliderStyle GetStyle()
    {
        return ( SliderStyle )base.Style;
    }

    public bool IsOver()
    {
        return _mouseOver;
    }

    protected IDrawable? getBackgroundDrawable()
    {
        SliderStyle style = ( SliderStyle )super.getStyle();

        if ( disabled && style.disabledBackground != null ) return style.disabledBackground;
        if ( IsDragging() && style.BackgroundDown != null ) return style.BackgroundDown;
        if ( _mouseOver && style.BackgroundOver != null ) return style.BackgroundOver;

        return style.background;
    }

    protected @Null Drawable getKnobDrawable()
    {
        SliderStyle style = ( SliderStyle )super.getStyle();

        if ( disabled && style.disabledKnob != null ) return style.disabledKnob;
        if ( IsDragging() && style.KnobDown != null ) return style.KnobDown;
        if ( _mouseOver && style.KnobOver != null ) return style.KnobOver;

        return style.knob;
    }

    protected Drawable getKnobBeforeDrawable()
    {
        SliderStyle style = ( SliderStyle )super.getStyle();

        if ( disabled && style.disabledKnobBefore != null ) return style.disabledKnobBefore;
        if ( IsDragging() && style.KnobBeforeDown != null ) return style.KnobBeforeDown;
        if ( _mouseOver && style.KnobBeforeOver != null ) return style.KnobBeforeOver;

        return style.knobBefore;
    }

    protected Drawable getKnobAfterDrawable()
    {
        SliderStyle style = ( SliderStyle )super.getStyle();

        if ( disabled && style.disabledKnobAfter != null ) return style.disabledKnobAfter;
        if ( IsDragging() && style.KnobAfterDown != null ) return style.KnobAfterDown;
        if ( _mouseOver && style.KnobAfterOver != null ) return style.KnobAfterOver;

        return style.knobAfter;
    }

    bool calculatePositionAndValue( float x, float y )
    {
        SliderStyle style = getStyle();
        Drawable    knob  = style.knob;
        Drawable    bg    = getBackgroundDrawable();

        float value;
        float oldPosition = position;

        float min = getMinValue();
        float max = getMaxValue();

        if ( vertical )
        {
            float height     = getHeight() - bg.getTopHeight() - bg.getBottomHeight();
            float knobHeight = knob == null ? 0 : knob.getMinHeight();
            position = y - bg.getBottomHeight() - knobHeight * 0.5f;
            value    = min + ( max - min ) * _visualInterpolationInverse.apply( position / ( height - knobHeight ) );
            position = Math.max( Math.min( 0, bg.getBottomHeight() ), position );
            position = Math.min( height - knobHeight, position );
        }
        else
        {
            float width     = getWidth() - bg.getLeftWidth() - bg.getRightWidth();
            float knobWidth = knob == null ? 0 : knob.getMinWidth();
            position = x - bg.getLeftWidth() - knobWidth * 0.5f;
            value    = min + ( max - min ) * _visualInterpolationInverse.apply( position / ( width - knobWidth ) );
            position = Math.max( Math.min( 0, bg.getLeftWidth() ), position );
            position = Math.min( width - knobWidth, position );
        }

        float oldValue                                                                                         = value;
        if ( !Gdx.input.isKeyPressed( Keys.SHIFT_LEFT ) && !Gdx.input.isKeyPressed( Keys.SHIFT_RIGHT ) ) value = snap( value );
        bool valueSet                                                                                          = setValue( value );
        if ( value == oldValue ) position                                                                      = oldPosition;

        return valueSet;
    }

    /** Returns a snapped value. */
    protected float snap( float value )
    {
        if ( _snapValues == null || _snapValues.length == 0 ) return value;
        float bestDiff = -1, bestValue = 0;

        for ( int i = 0; i < _snapValues.length; i++ )
        {
            float snapValue = _snapValues[ i ];
            float diff      = Math.abs( value - snapValue );

            if ( diff <= threshold )
            {
                if ( bestDiff == -1 || diff < bestDiff )
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
        this._snapValues = values;
        this.threshold   = threshld;
    }

    /** Returns true if the slider is being dragged. */
    public bool IsDragging()
    {
        return _draggingPointer != -1;
    }

    /** Sets the mouse button, which can trigger a change of the slider. Is -1, so every button, by default. */
    public void setButton( int button )
    {
        this._button = button;
    }

    /** Sets the inverse interpolation to use for display. This should perform the inverse of the
   * {@link #setVisualInterpolation(Interpolation) visual interpolation}. */
    public void setVisualInterpolationInverse( Interpolation interpolation )
    {
        this._visualInterpolationInverse = interpolation;
    }

    /** Sets the value using the specified visual percent.
  * @see #setVisualInterpolation(Interpolation) */
    public void setVisualPercent( float percent )
    {
        SetValue( min + ( max - min ) * _visualInterpolationInverse.apply( percent ) );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    public class SliderStyle : ProgressBarStyle
    {
        public IDrawable? BackgroundOver { get; set; }
        public IDrawable? BackgroundDown { get; set; }
        public IDrawable? KnobBeforeOver { get; set; }
        public IDrawable? KnobOver       { get; set; }
        public IDrawable? KnobAfterOver  { get; set; }
        public IDrawable? KnobBeforeDown { get; set; }
        public IDrawable? KnobDown       { get; set; }
        public IDrawable? KnobAfterDown  { get; set; }

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
    }
}
