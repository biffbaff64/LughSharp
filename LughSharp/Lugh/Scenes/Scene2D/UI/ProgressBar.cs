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

using LughSharp.Lugh.Graphics.G2D;
using LughSharp.Lugh.Maths;
using LughSharp.Lugh.Scenes.Scene2D.Listeners;
using LughSharp.Lugh.Scenes.Scene2D.Utils;
using LughSharp.Lugh.Utils.Pooling;

namespace LughSharp.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A progress bar is a widget that visually displays the progress of some activity
/// or a value within given range. The progress bar has a range (min, max) and a
/// stepping between each value it represents. The percentage of completeness typically
/// starts out as an empty progress bar and gradually becomes filled in as the task
/// or variable value progresses.
/// <para>
/// A <see cref="ChangeListener.ChangeEvent"/> is fired when the progress bar knob is moved. Cancelling
/// the event will move the knob to where it was previously.
/// </para>
/// <para>
/// For a horizontal progress bar, its preferred height is determined by the larger of
/// the knob and background, and the preferred width is 140, a relatively arbitrary size.
/// These parameters are reversed for a vertical progress bar.
/// </para>
/// </summary>
[PublicAPI]
public class ProgressBar : Widget, IDisableable
{
    public float KnobPosition { get; set; }
    public float MinValue     { get; set; }
    public float MaxValue     { get; set; }
    public float Value        { get; set; }
    public bool  IsVertical   { get; set; }
    public bool  IsRound      { get; set; } = true;
    public bool  IsDisabled   { get; set; }

    public Interpolator AnimateInterpolation { get; set; } = Interpolation.Linear;
    public Interpolator VisualInterpolation  { get; set; } = Interpolation.Linear;

    // ========================================================================
    
    private const    float DEFAULT_PREF_WIDTH        = 140f;
    private const    float DEFAULT_PREF_HEIGHT       = 140f;
    private readonly bool  _programmaticChangeEvents = true;
    private readonly float _stepSize;

    private readonly ProgressBarStyle? _style;
    private          float             _animateDuration;

    private float _animateFromValue;
    private float _animateTime;

    // ========================================================================
    // ========================================================================
    
    public ProgressBar( float min, float max, float stepSize, bool vertical, Skin skin )
        : this( min,
                max,
                stepSize,
                vertical,
                skin.Get< ProgressBarStyle >( "default-" + ( vertical ? "vertical" : "horizontal" ) ) )
    {
    }

    public ProgressBar( float min, float max, float stepSize, bool vertical, Skin skin, string styleName )
        : this( min, max, stepSize, vertical, skin.Get< ProgressBarStyle >( styleName ) )
    {
    }

    /// <summary>
    /// Creates a new progress bar. If horizontal, its width is determined by the
    /// prefWidth parameter, and its height is determined by the maximum of the height
    /// of either the progress bar <see cref="NinePatch"/> or progress bar handle
    /// <see cref="TextureRegion"/>. The min and max values determine the range the
    /// values of this progress bar can take on, the stepSize parameter specifies the
    /// distance between individual values.
    /// <para>
    /// E.g. min could be 4, max could be 10 and stepSize could be 0.2, giving you a
    /// total of 30 values, 4.0 4.2, 4.4 and so on.
    /// </para>
    /// </summary>
    /// <param name="min"> the minimum value </param>
    /// <param name="max"> the maximum value </param>
    /// <param name="stepSize"> the step size between values </param>
    /// <param name="vertical"></param>
    /// <param name="style"> the <see cref="ProgressBarStyle"/>  </param>
    public ProgressBar( float min, float max, float stepSize, bool vertical, ProgressBarStyle style )
    {
        if ( min > max )
        {
            throw new ArgumentException( "max must be > min. min,max: " + min + ", " + max );
        }

        if ( stepSize <= 0 )
        {
            throw new ArgumentException( "stepSize must be > 0: " + stepSize );
        }

        Style = style;

        MinValue   = min;
        MaxValue   = max;
        StepSize   = stepSize;
        IsVertical = vertical;
        Value      = min;

        SetSize( GetPrefWidth(), GetPrefHeight() );
    }

    // ========================================================================

    public bool IsAnimating => _animateTime > 0;

    public float StepSize
    {
        get => _stepSize;
        init
        {
            if ( _stepSize <= 0 )
            {
                throw new ArgumentException( "steps must be > 0: " + _stepSize );
            }

            _stepSize = value;
        }
    }

    public ProgressBarStyle Style
    {
        get => _style!;
        init
        {
            ArgumentNullException.ThrowIfNull( value );

            _style = value;
            InvalidateHierarchy();
        }
    }

    public override void Act( float delta )
    {
        base.Act( delta );

        if ( _animateTime > 0 )
        {
            _animateTime -= delta;

            if ( Stage is { ActionsRequestRendering: true } )
            {
                GdxApi.Graphics.RequestRendering();
            }
        }
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        var knob        = Style.Knob;
        var currentKnob = GetKnobDrawable();
        var bg          = GetBackgroundDrawable();
        var knobBefore  = GetKnobBeforeDrawable();
        var knobAfter   = GetKnobAfterDrawable();

        var x          = X;
        var y          = Y;
        var width      = Width;
        var height     = Height;
        var knobHeight = knob?.MinHeight ?? 0;
        var knobWidth  = knob?.MinWidth ?? 0;
        var percent    = GetVisualPercent();

        batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

        if ( IsVertical )
        {
            var   positionHeight = height;
            float bgTopHeight    = 0;
            float bgBottomHeight = 0;

            if ( bg != null )
            {
                if ( IsRound )
                {
                    bg.Draw( batch,
                             ( float ) Math.Round( x + ( ( width - bg.MinWidth ) * 0.5f ) ),
                             y,
                             ( float ) Math.Round( bg.MinWidth ),
                             height );
                }
                else
                {
                    bg.Draw( batch, x + ( ( width - bg.MinWidth ) * 0.5f ), y, bg.MinWidth, height );
                }

                bgTopHeight    =  bg.TopHeight;
                bgBottomHeight =  bg.BottomHeight;
                positionHeight -= bgTopHeight + bgBottomHeight;
            }

            float knobHeightHalf;

            if ( knob == null )
            {
                knobHeightHalf = knobBefore == null ? 0 : knobBefore.MinHeight * 0.5f;
                KnobPosition   = ( positionHeight - knobHeightHalf ) * percent;
                KnobPosition   = Math.Min( positionHeight - knobHeightHalf, KnobPosition );
            }
            else
            {
                knobHeightHalf = knobHeight * 0.5f;
                KnobPosition   = ( positionHeight - knobHeight ) * percent;
                KnobPosition   = Math.Min( positionHeight - knobHeight, KnobPosition ) + bgBottomHeight;
            }

            KnobPosition = Math.Max( bgBottomHeight, KnobPosition );

            if ( knobBefore != null )
            {
                if ( IsRound )
                {
                    knobBefore.Draw( batch,
                                     ( float ) Math.Round( x + ( ( width - knobBefore.MinWidth ) * 0.5f ) ),
                                     ( float ) Math.Round( y + bgTopHeight ),
                                     ( float ) Math.Round( knobBefore.MinWidth ),
                                     ( float ) Math.Round( KnobPosition + knobHeightHalf ) );
                }
                else
                {
                    knobBefore.Draw( batch,
                                     x + ( ( width - knobBefore.MinWidth ) * 0.5f ),
                                     y + bgTopHeight,
                                     knobBefore.MinWidth,
                                     KnobPosition + knobHeightHalf );
                }
            }

            if ( knobAfter != null )
            {
                if ( IsRound )
                {
                    knobAfter.Draw( batch,
                                    ( float ) Math.Round( x + ( ( width - knobAfter.MinWidth ) * 0.5f ) ),
                                    ( float ) Math.Round( y + KnobPosition + knobHeightHalf ),
                                    ( float ) Math.Round( knobAfter.MinWidth ),
                                    ( float ) Math.Round( height - KnobPosition - knobHeightHalf - bgBottomHeight ) );
                }
                else
                {
                    knobAfter.Draw( batch,
                                    x + ( ( width - knobAfter.MinWidth ) * 0.5f ),
                                    y + KnobPosition + knobHeightHalf,
                                    knobAfter.MinWidth,
                                    height - KnobPosition - knobHeightHalf - bgBottomHeight );
                }
            }

            if ( currentKnob != null )
            {
                float w = currentKnob.MinWidth, h = currentKnob.MinHeight;
                x += ( width - w ) * 0.5f;
                y += ( ( knobHeight - h ) * 0.5f ) + KnobPosition;

                if ( IsRound )
                {
                    x = ( float ) Math.Round( x );
                    y = ( float ) Math.Round( y );
                    w = ( float ) Math.Round( w );
                    h = ( float ) Math.Round( h );
                }

                currentKnob.Draw( batch, x, y, w, h );
            }
        }
        else
        {
            var positionWidth = width;

            float bgLeftWidth = 0, bgRightWidth = 0;

            if ( bg != null )
            {
                if ( IsRound )
                {
                    bg.Draw( batch,
                             x,
                             ( float ) Math.Round( y + ( ( height - bg.MinHeight ) * 0.5f ) ),
                             width,
                             ( float ) Math.Round( bg.MinHeight ) );
                }
                else
                {
                    bg.Draw( batch, x, y + ( ( height - bg.MinHeight ) * 0.5f ), width, bg.MinHeight );
                }

                bgLeftWidth   =  bg.LeftWidth;
                bgRightWidth  =  bg.RightWidth;
                positionWidth -= bgLeftWidth + bgRightWidth;
            }

            float knobWidthHalf;

            if ( knob == null )
            {
                knobWidthHalf = knobBefore == null ? 0 : knobBefore.MinWidth * 0.5f;
                KnobPosition  = ( positionWidth - knobWidthHalf ) * percent;
                KnobPosition  = Math.Min( positionWidth - knobWidthHalf, KnobPosition );
            }
            else
            {
                knobWidthHalf = knobWidth * 0.5f;
                KnobPosition  = ( positionWidth - knobWidth ) * percent;
                KnobPosition  = Math.Min( positionWidth - knobWidth, KnobPosition ) + bgLeftWidth;
            }

            KnobPosition = Math.Max( bgLeftWidth, KnobPosition );

            if ( knobBefore != null )
            {
                if ( IsRound )
                {
                    knobBefore.Draw( batch,
                                     ( float ) Math.Round( x + bgLeftWidth ),
                                     ( float ) Math.Round( y + ( ( height - knobBefore.MinHeight ) * 0.5f ) ),
                                     ( float ) Math.Round( KnobPosition + knobWidthHalf ),
                                     ( float ) Math.Round( knobBefore.MinHeight ) );
                }
                else
                {
                    knobBefore.Draw( batch,
                                     x + bgLeftWidth,
                                     y + ( ( height - knobBefore.MinHeight ) * 0.5f ),
                                     KnobPosition + knobWidthHalf,
                                     knobBefore.MinHeight );
                }
            }

            if ( knobAfter != null )
            {
                if ( IsRound )
                {
                    knobAfter.Draw( batch,
                                    ( float ) Math.Round( x + KnobPosition + knobWidthHalf ),
                                    ( float ) Math.Round( y + ( ( height - knobAfter.MinHeight ) * 0.5f ) ),
                                    ( float ) Math.Round( width - KnobPosition - knobWidthHalf - bgRightWidth ),
                                    ( float ) Math.Round( knobAfter.MinHeight ) );
                }
                else
                {
                    knobAfter.Draw( batch,
                                    x + KnobPosition + knobWidthHalf,
                                    y + ( ( height - knobAfter.MinHeight ) * 0.5f ),
                                    width - KnobPosition - knobWidthHalf - bgRightWidth,
                                    knobAfter.MinHeight );
                }
            }

            if ( currentKnob != null )
            {
                var w = currentKnob.MinWidth;
                var h = currentKnob.MinHeight;

                x += ( ( knobWidth - w ) * 0.5f ) + KnobPosition;
                y += ( height - h ) * 0.5f;

                if ( IsRound )
                {
                    x = ( float ) Math.Round( x );
                    y = ( float ) Math.Round( y );
                    w = ( float ) Math.Round( w );
                    h = ( float ) Math.Round( h );
                }

                currentKnob.Draw( batch, x, y, w, h );
            }
        }
    }

    /// <summary>
    /// Sets the progress bar position, rounded to the nearest step size and
    /// clamped to the minimum and maximum values. <see cref="Clamp(float)"/>
    /// can be overridden to allow values outside of the progress bar's min/max
    /// range.
    /// </summary>
    /// <returns>
    /// <tt>false</tt> if the value was not changed because the progress bar
    /// already had the value or it was canceled by a listener.
    /// </returns>
    public bool SetBarPosition( float value )
    {
        value = Clamp( Round( value ) );

        var oldValue = Value;

        if ( value.Equals( oldValue ) )
        {
            return false;
        }

        var oldVisualValue = GetVisualValue();

        Value = value;

        if ( _programmaticChangeEvents )
        {
            var changeEvent = Pools< ChangeListener.ChangeEvent >.Obtain();

            var cancelled = Fire( changeEvent );

            Pools< ChangeListener.ChangeEvent >.Free( changeEvent );

            if ( cancelled )
            {
                Value = oldValue;

                return false;
            }
        }

        if ( _animateDuration > 0 )
        {
            _animateFromValue = oldVisualValue;
            _animateTime      = _animateDuration;
        }

        return true;
    }

    /// <summary>
    /// Rounds the value using the progress bar's step size.
    /// This can be overridden to customize or disable rounding.
    /// </summary>
    private float Round( float value )
    {
        return ( float ) ( Math.Round( value / StepSize ) * StepSize );
    }

    /// <summary>
    /// Clamps the value to the progress bar's min/max range. This can be overridden
    /// to allow a range different from the progress bar knob's range.
    /// </summary>
    private float Clamp( float value )
    {
        return MathUtils.Clamp( value, MinValue, MaxValue );
    }

    /// <summary>
    /// Sets the range of this progress bar. The progress bar's current value is clamped to the range.
    /// </summary>
    public void SetRange( float min, float max )
    {
        if ( min > max )
        {
            throw new ArgumentException( "min must be <= max: " + min + " <= " + max );
        }

        MinValue = min;
        MaxValue = max;

        if ( Value < min )
        {
            Value = min;
        }
        else if ( Value > max )
        {
            Value = max;
        }
    }

    public float GetPrefWidth()
    {
        if ( IsVertical )
        {
            var knob = Style.Knob;
            var bg   = GetBackgroundDrawable();

            return Math.Max( knob?.MinWidth ?? 0, bg?.MinWidth ?? 0 );
        }

        return DEFAULT_PREF_WIDTH;
    }

    public float GetPrefHeight()
    {
        if ( IsVertical )
        {
            return DEFAULT_PREF_HEIGHT;
        }

        var knob = Style.Knob;
        var bg   = GetBackgroundDrawable();

        return Math.Max( knob?.MinHeight ?? 0, bg?.MinHeight ?? 0 );
    }

    /// <summary>
    /// If > 0, changes to the progress bar value via <see cref="Value"/>
    /// will happen over this duration in seconds.
    /// </summary>
    /// <param name="duration"></param>
    public void SetAnimateDuration( float duration )
    {
        _animateDuration = duration;
    }

    /// <summary>
    /// If <see cref="SetAnimateDuration(float)"/> animating the progress bar value,
    /// this returns the value current displayed.
    /// </summary>
    public float GetVisualValue()
    {
        if ( _animateTime > 0 )
        {
            return AnimateInterpolation.Apply( _animateFromValue, Value, 1 - ( _animateTime / _animateDuration ) );
        }

        return Value;
    }

    /// <summary>
    /// Sets the visual value equal to the actual value. This can be used
    /// to set the value without animating.
    /// </summary>
    public void UpdateVisualValue()
    {
        _animateTime = 0;
    }

    public float GetPercent()
    {
        if ( MinValue.Equals( MaxValue ) )
        {
            return 0;
        }

        return ( Value - MinValue ) / ( MaxValue - MinValue );
    }

    public float GetVisualPercent()
    {
        if ( MinValue.Equals( MaxValue ) )
        {
            return 0;
        }

        return VisualInterpolation.Apply( ( GetVisualValue() - MinValue ) / ( MaxValue - MinValue ) );
    }

    private IDrawable? GetBackgroundDrawable()
    {
        if ( IsDisabled && ( Style.DisabledBackground != null ) )
        {
            return Style.DisabledBackground;
        }

        return Style.Background;
    }

    private IDrawable? GetKnobDrawable()
    {
        if ( IsDisabled && ( Style.DisabledKnob != null ) )
        {
            return Style.DisabledKnob;
        }

        return Style.Knob;
    }

    private IDrawable? GetKnobBeforeDrawable()
    {
        if ( IsDisabled && ( Style.DisabledKnobBefore != null ) )
        {
            return Style.DisabledKnobBefore;
        }

        return Style.KnobBefore;
    }

    private IDrawable? GetKnobAfterDrawable()
    {
        if ( IsDisabled && ( Style.DisabledKnobAfter != null ) )
        {
            return Style.DisabledKnobAfter;
        }

        return Style.KnobAfter;
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// The style for a progress bar, see <see cref="ProgressBar"/>.
    /// </summary>
    [PublicAPI]
    public class ProgressBarStyle
    {
        // The progress bar background, stretched only in one direction.
        public IDrawable? Background         { get; set; }
        public IDrawable? DisabledBackground { get; set; }
        public IDrawable? Knob               { get; set; }
        public IDrawable? DisabledKnob       { get; set; }
        public IDrawable? KnobBefore         { get; set; }
        public IDrawable? DisabledKnobBefore { get; set; }
        public IDrawable? KnobAfter          { get; set; }
        public IDrawable? DisabledKnobAfter  { get; set; }

        public ProgressBarStyle()
        {
        }

        public ProgressBarStyle( IDrawable background, IDrawable knob )
        {
            Background = background;
            Knob       = knob;
        }

        public ProgressBarStyle( ProgressBarStyle style )
        {
            Background         = style.Background;
            DisabledBackground = style.DisabledBackground;
            Knob               = style.Knob;
            DisabledKnob       = style.DisabledKnob;
            KnobBefore         = style.KnobBefore;
            DisabledKnobBefore = style.DisabledKnobBefore;
            KnobAfter          = style.KnobAfter;
            DisabledKnobAfter  = style.DisabledKnobAfter;
        }
    }
}
