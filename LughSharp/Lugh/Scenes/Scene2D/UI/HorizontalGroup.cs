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

using LughSharp.Lugh.Graphics.GLUtils;
using LughSharp.Lugh.Scenes.Scene2D.Utils;
using LughSharp.Lugh.Utils;
using LughSharp.Lugh.Utils.Collections;
using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A group that lays out its children side by side horizontally, with optional
/// wrapping. This can be easier than using <see cref="Table"/> when actors need
/// to be inserted into or removed from the middle of the group.
/// <para>
/// <see cref="Group.Children"/> can be sorted to change the order of the actors
/// <see cref="Actor.SetZIndex(int)"/>. <see cref="Invalidate()"/> must be called
/// after changing the children order.
/// </para>
/// <para>
/// The preferred width is the sum of the children's preferred widths plus spacing.
/// The preferred height is the largest preferred height of any child. The preferred
/// size is slightly different when <see cref="Wrap"/>" is enabled. The min size
/// is the preferred size and the max size is 0.
/// </para>
/// <para>
/// Widgets are sized using their <see cref="ILayout.PrefWidth"/>", so widgets
/// which return 0 as their preferred width will be given a width of 0 (eg, a label
/// with {@link Label#setWrap(bool) word wrap} enabled).
/// </para>
/// </summary>
public class HorizontalGroup : WidgetGroup
{
    private float _lastPrefHeight;
    private float _prefHeight;

    private float          _prefWidth;
    private int            _rowAlign;
    private List< float >? _rowSizes; // row width, row height, ...
    private bool           _sizeInvalid = true;

    public HorizontalGroup()
    {
        Touchable = Touchable.ChildrenOnly;
    }

    public bool  Wrap      { get; set; }
    public float Fill      { get; set; }
    public bool  Expand    { get; set; }
    public int   Alignment { get; set; } = Align.LEFT;
    public float PadTop    { get; set; }
    public float PadLeft   { get; set; }
    public float PadBottom { get; set; }
    public float PadRight  { get; set; }

    /// <summary>
    /// If true, the children will be displayed last to first.
    /// </summary>
    public bool Reverse { get; set; }

    /// <summary>
    /// If true, rows will wrap above the previous rows.
    /// </summary>
    public bool WrapReverse { get; set; }

    /// <summary>
    /// The horizontal space between children.
    /// </summary>
    public float Space { get; set; }

    /// <summary>
    /// The vertical space between rows when wrap is enabled.
    /// </summary>
    public float WrapSpace { get; set; }

    /// <summary>
    /// If true (the default), positions and sizes are rounded to integers.
    /// </summary>
    public bool Round { get; set; } = true;

    public override float PrefWidth
    {
        get
        {
            if ( Wrap )
            {
                return 0;
            }

            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _prefWidth;
        }
        set => _prefWidth = value;
    }

    public override float PrefHeight
    {
        get
        {
            if ( _sizeInvalid )
            {
                ComputeSize();
            }

            return _prefHeight;
        }
        set => _prefHeight = value;
    }

    /// <inheritdoc />
    public override void Invalidate()
    {
        base.Invalidate();
        _sizeInvalid = true;
    }

    private void ComputeSize()
    {
        _sizeInvalid = false;
        SnapshotArray< Actor > children = Children;

        var n = children.Size;

        _prefHeight = 0;

        if ( Wrap )
        {
            _prefWidth = 0;

            if ( _rowSizes == null )
            {
                _rowSizes = new List< float >();
            }
            else
            {
                _rowSizes.Clear();
            }

            List< float > rowSizes = _rowSizes;

            var space      = Space;
            var wrapSpace  = WrapSpace;
            var pad        = PadLeft + PadRight;
            var groupWidth = Width - pad;
            var x          = 0f;
            var y          = 0f;
            var rowHeight  = 0f;
            var i          = 0;
            var incr       = 1;

            if ( Reverse )
            {
                i    = n - 1;
                n    = -1;
                incr = -1;
            }

            for ( ; i != n; i += incr )
            {
                var child = children.GetAt( i );

                float width;
                float height;

                if ( child is ILayout layout )
                {
                    width = layout.PrefWidth;

                    if ( width > groupWidth )
                    {
                        width = Math.Max( groupWidth, layout.MinWidth );
                    }

                    height = layout.PrefHeight;
                }
                else
                {
                    width  = child.Width;
                    height = child.Height;
                }

                var incrX = width + ( x > 0 ? space : 0 );

                if ( ( ( x + incrX ) > groupWidth ) && ( x > 0 ) )
                {
                    rowSizes.Add( x );
                    rowSizes.Add( rowHeight );

                    _prefWidth = Math.Max( _prefWidth, x + pad );

                    if ( y > 0 )
                    {
                        y += wrapSpace;
                    }

                    y         += rowHeight;
                    rowHeight =  0;
                    x         =  0;
                    incrX     =  width;
                }

                x         += incrX;
                rowHeight =  Math.Max( rowHeight, height );
            }

            rowSizes.Add( x );
            rowSizes.Add( rowHeight );

            _prefWidth = Math.Max( _prefWidth, x + pad );

            if ( y > 0 )
            {
                y += wrapSpace;
            }

            _prefHeight = Math.Max( _prefHeight, y + rowHeight );
        }
        else
        {
            _prefWidth = PadLeft + PadRight + ( Space * ( n - 1 ) );

            for ( var i = 0; i < n; i++ )
            {
                var child = children.GetAt( i );

                // ReSharper disable once MergeCastWithTypeCheck
                if ( child is ILayout )
                {
                    var layout = ( ILayout ) child;

                    _prefWidth  += layout.PrefWidth;
                    _prefHeight =  Math.Max( _prefHeight, layout.PrefHeight );
                }
                else
                {
                    _prefWidth  += child.Width;
                    _prefHeight =  Math.Max( _prefHeight, child.Height );
                }
            }
        }

        _prefHeight += PadTop + PadBottom;

        if ( Round )
        {
            _prefWidth  = MathF.Round( _prefWidth );
            _prefHeight = MathF.Round( _prefHeight );
        }
    }

    /// <inheritdoc />
    public override void SetLayout()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        if ( Wrap )
        {
            LayoutWrapped();

            return;
        }

        var round     = Round;
        var align     = Alignment;
        var space     = Space;
        var padBottom = PadBottom;
        var fill      = Fill;
        var rowHeight = ( Expand ? Height : _prefHeight ) - PadTop - padBottom;
        var x         = PadLeft;

        if ( ( align & Align.RIGHT ) != 0 )
        {
            x += Width - _prefWidth;
        }
        else if ( ( align & Align.LEFT ) == 0 ) // center
        {
            x += ( Width - _prefWidth ) / 2;
        }

        float startY;

        if ( ( align & Align.BOTTOM ) != 0 )
        {
            startY = padBottom;
        }
        else if ( ( align & Align.TOP ) != 0 )
        {
            startY = Height - PadTop - rowHeight;
        }
        else
        {
            startY = padBottom + ( ( Height - padBottom - PadTop - rowHeight ) / 2 );
        }

        align = _rowAlign;

        SnapshotArray< Actor > children = Children;

        var i    = 0;
        var n    = children.Size;
        var incr = 1;

        if ( Reverse )
        {
            i    = n - 1;
            n    = -1;
            incr = -1;
        }

        for ( ; i != n; i += incr )
        {
            var child = children.GetAt( i );

            float    width;
            float    height;
            ILayout? layout = null;

            // ReSharper disable once MergeCastWithTypeCheck
            if ( child is ILayout )
            {
                layout = ( ILayout ) child;
                width  = layout.PrefWidth;
                height = layout.PrefHeight;
            }
            else
            {
                width  = child.Width;
                height = child.Height;
            }

            if ( fill > 0 )
            {
                height = rowHeight * fill;
            }

            if ( layout != null )
            {
                height = Math.Max( height, layout.MinHeight );

                var maxHeight = layout.MaxHeight;

                if ( ( maxHeight > 0 ) && ( height > maxHeight ) )
                {
                    height = maxHeight;
                }
            }

            var y = startY;

            if ( ( align & Align.TOP ) != 0 )
            {
                y += rowHeight - height;
            }
            else if ( ( align & Align.BOTTOM ) == 0 ) // center
            {
                y += ( rowHeight - height ) / 2;
            }

            if ( round )
            {
                child.SetBounds(
                                MathF.Round( x ),
                                MathF.Round( y ),
                                MathF.Round( width ),
                                MathF.Round( height )
                               );
            }
            else
            {
                child.SetBounds( x, y, width, height );
            }

            x += width + space;

            if ( layout != null )
            {
                layout.Validate();
            }
        }
    }

    private void LayoutWrapped()
    {
        var prefHeight = PrefHeight;

        if ( !prefHeight.Equals( _lastPrefHeight ) )
        {
            _lastPrefHeight = prefHeight;
            InvalidateHierarchy();
        }

        var   align      = Alignment;
        var   round      = Round;
        var   space      = Space;
        var   fill       = Fill;
        var   wrapSpace  = WrapSpace;
        var   maxWidth   = _prefWidth - PadLeft - PadRight;
        var   rowY       = prefHeight - PadTop;
        var   groupWidth = Width;
        var   xStart     = PadLeft;
        float x          = 0;
        float rowHeight  = 0;
        float rowDir     = -1;

        if ( _rowSizes is null )
        {
            throw new GdxRuntimeException( "_rowSizes cannot be null!" );
        }

        if ( ( align & Align.TOP ) != 0 )
        {
            rowY += Height - prefHeight;
        }
        else if ( ( align & Align.BOTTOM ) == 0 ) // center
        {
            rowY += ( Height - prefHeight ) / 2;
        }

        if ( WrapReverse )
        {
            rowY   -= prefHeight + _rowSizes[ 1 ];
            rowDir =  1;
        }

        if ( ( align & Align.RIGHT ) != 0 )
        {
            xStart += groupWidth - _prefWidth;
        }
        else if ( ( align & Align.LEFT ) == 0 ) // center
        {
            xStart += ( groupWidth - _prefWidth ) / 2;
        }

        groupWidth -= PadRight;
        align      =  _rowAlign;

        List< float >?         rowSizes = _rowSizes;
        SnapshotArray< Actor > children = Children;

        var i    = 0;
        var n    = children.Size;
        var incr = 1;

        if ( Reverse )
        {
            i    = n - 1;
            n    = -1;
            incr = -1;
        }

        for ( var r = 0; i != n; i += incr )
        {
            var      child  = children.GetAt( i );
            ILayout? layout = null;

            float width;
            float height;

            // ReSharper disable once MergeCastWithTypeCheck
            if ( child is ILayout )
            {
                layout = ( ILayout ) child;
                width  = layout.PrefWidth;

                if ( width > groupWidth )
                {
                    width = Math.Max( groupWidth, layout.MinWidth );
                }

                height = layout.PrefHeight;
            }
            else
            {
                width  = child.Width;
                height = child.Height;
            }

            if ( ( ( x + width ) > groupWidth ) || ( r == 0 ) )
            {
                r = Math.Min( r, rowSizes.Count - 2 ); // In case an actor changed size without invalidating this layout.
                x = xStart;

                if ( ( align & Align.RIGHT ) != 0 )
                {
                    x += maxWidth - rowSizes[ r ];
                }
                else if ( ( align & Align.LEFT ) == 0 ) // center
                {
                    x += ( maxWidth - rowSizes[ r ] ) / 2;
                }

                rowHeight = rowSizes[ r + 1 ];

                if ( r > 0 )
                {
                    rowY += wrapSpace * rowDir;
                }

                rowY += rowHeight * rowDir;
                r    += 2;
            }

            if ( fill > 0 )
            {
                height = rowHeight * fill;
            }

            if ( layout != null )
            {
                height = Math.Max( height, layout.MinHeight );

                var maxHeight = layout.MaxHeight;

                if ( ( maxHeight > 0 ) && ( height > maxHeight ) )
                {
                    height = maxHeight;
                }
            }

            var y = rowY;

            if ( ( align & Align.TOP ) != 0 )
            {
                y += rowHeight - height;
            }
            else if ( ( align & Align.BOTTOM ) == 0 ) // center
            {
                y += ( rowHeight - height ) / 2;
            }

            if ( round )
            {
                child.SetBounds(
                                MathF.Round( x ),
                                MathF.Round( y ),
                                MathF.Round( width ),
                                MathF.Round( height )
                               );
            }
            else
            {
                child.SetBounds( x, y, width, height );
            }

            x += width + space;

            if ( layout != null )
            {
                layout.Validate();
            }
        }
    }

    /// <summary>
    /// If false, the widgets are arranged in a single row and the preferred
    /// width is the widget widths plus spacing.
    /// <para>
    /// If true, the widgets will wrap using the width of the horizontal group.
    /// The preferred width of the group will be 0 as it is expected that something
    /// external will set the width of the group. Widgets are sized to their
    /// preferred width unless it is larger than the group's width, in which case
    /// they are sized to the group's width but not less than their minimum width.
    /// </para>
    /// <para>
    /// Default is false.
    /// </para>
    /// <para>
    /// When wrap is enabled, the group's preferred height depends on the width of
    /// the group. In some cases the parent of the group will need to layout twice:
    /// once to set the width of the group and a second time to adjust to the group's
    /// new preferred height.
    /// </para>
    /// </summary>
    public HorizontalGroup SetWrap( bool wrap = true )
    {
        Wrap = wrap;

        return this;
    }

    // ========================================================================

    /// <summary>
    /// </summary>
    /// <param name="fill"> 0f will use preferred width </param>
    /// .
    public HorizontalGroup SetFill( float fill = 1f )
    {
        Fill = fill;

        return this;
    }

    /// <summary>
    /// When true and wrap is false, the rows will take up the
    /// entire horizontal group height.
    /// </summary>
    public HorizontalGroup SetExpand( bool expand = true )
    {
        Expand = expand;

        return this;
    }

    public HorizontalGroup Grow()
    {
        Expand = true;
        Fill   = 1;

        return this;
    }

    protected override void DrawDebugBounds( ShapeRenderer shapes )
    {
        base.DrawDebugBounds( shapes );

        if ( !DebugActive )
        {
            return;
        }

        shapes.Set( ShapeRenderer.ShapeTypes.Lines );

        if ( Stage != null )
        {
            shapes.Color = Stage.DebugColor;
        }

        shapes.Rect( X + PadLeft,
                     Y + PadBottom,
                     OriginX,
                     OriginY,
                     Width - PadLeft - PadRight,
                     Height - PadBottom - PadTop,
                     ScaleX,
                     ScaleY,
                     Rotation );
    }

    // ========================================================================

    #region padding

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight to the specified value.
    /// </summary>
    public HorizontalGroup SetPadding( float pad )
    {
        PadTop    = pad;
        PadLeft   = pad;
        PadBottom = pad;
        PadRight  = pad;

        return this;
    }

    public HorizontalGroup SetPadding( float top, float left, float bottom, float right )
    {
        PadTop    = top;
        PadLeft   = left;
        PadBottom = bottom;
        PadRight  = right;

        return this;
    }

    public HorizontalGroup SetPadTop( float padTop )
    {
        PadTop = padTop;

        return this;
    }

    public HorizontalGroup SetPadLeft( float padLeft )
    {
        PadLeft = padLeft;

        return this;
    }

    public HorizontalGroup SetPadBottom( float padBottom )
    {
        PadBottom = padBottom;

        return this;
    }

    public HorizontalGroup SetPadRight( float padRight )
    {
        PadRight = padRight;

        return this;
    }

    #endregion padding

    // ========================================================================

    #region alignment

    /// <summary>
    /// Sets the alignment of all widgets within the horizontal group.
    /// <para>
    /// Set to <see cref="Align.CENTER"/>, <see cref="Align.LEFT"/>,
    /// <see cref="Align.RIGHT"/>, <see cref="Align.TOP"/>,
    /// <see cref="Align.BOTTOM"/>, or any combination of those.
    /// </para>
    /// </summary>
    public HorizontalGroup SetAlign( int align )
    {
        Alignment = align;

        return this;
    }

    /// <summary>
    /// Sets the alignment of all widgets within the horizontal group to
    /// <see cref="Align.CENTER"/>. This clears any other alignment.
    /// </summary>
    public HorizontalGroup AlignCenter()
    {
        Alignment = Align.CENTER;

        return this;
    }

    /// <summary>
    /// Sets <see cref="Align.TOP"/> and clears <see cref="Align.BOTTOM"/> for
    /// the alignment of all widgets within the horizontal group.
    /// </summary>
    public HorizontalGroup AlignTop()
    {
        Alignment |= Align.TOP;
        Alignment &= ~Align.BOTTOM;

        return this;
    }

    /// <summary>
    /// Sets <see cref="Align.BOTTOM"/> and clears <see cref="Align.TOP"/> for
    /// the alignment of all widgets within the horizontal group.
    /// </summary>
    public HorizontalGroup AlignBottom()
    {
        Alignment |= Align.BOTTOM;
        Alignment &= ~Align.TOP;

        return this;
    }

    /// <summary>
    /// Sets <see cref="Align.LEFT"/> and clears <see cref="Align.RIGHT"/> for
    /// the alignment of all widgets within the horizontal group.
    /// </summary>
    public HorizontalGroup AlignLeft()
    {
        Alignment |= Align.LEFT;
        Alignment &= ~Align.RIGHT;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.RIGHT"/> and clears <see cref="Align.LEFT"/> for
    /// the alignment of all widgets within the horizontal group.
    /// </summary>
    public HorizontalGroup AlignRight()
    {
        Alignment |= Align.RIGHT;
        Alignment &= ~Align.LEFT;

        return this;
    }

    /// <summary>
    /// Sets the horizontal alignment of each row of widgets when <see cref="Wrap"/>
    /// is enabled and sets the vertical alignment of widgets within each row.
    /// </summary>
    /// <param name="rowAlign">
    /// Set to <see cref="Align.CENTER"/>, <see cref="Align.LEFT"/>, <see cref="Align.RIGHT"/>,
    /// <see cref="Align.TOP"/>, <see cref="Align.BOTTOM"/> or any combination of those.
    /// </param>
    /// <returns></returns>
    public HorizontalGroup RowAlign( int rowAlign )
    {
        _rowAlign = rowAlign;

        return this;
    }

    /// <summary>
    /// Sets the alignment of widgets within each row to <see cref="Align.CENTER"/>.
    /// This clears any other alignment.
    /// </summary>
    public HorizontalGroup RowCenter()
    {
        _rowAlign = Align.CENTER;

        return this;
    }

    /// <summary>
    /// Sets <see cref="Align.TOP"/> and clears <see cref="Align.BOTTOM"/> for
    /// the alignment of widgets within each row.
    /// </summary>
    public HorizontalGroup RowTop()
    {
        _rowAlign |= Align.TOP;
        _rowAlign &= ~Align.BOTTOM;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.LEFT"/> and clears <see cref="Align.RIGHT"/> for
    /// the alignment of each row of widgets when <see cref="Wrap"/> is enabled.
    /// </summary>
    public HorizontalGroup RowLeft()
    {
        _rowAlign |= Align.LEFT;
        _rowAlign &= ~Align.RIGHT;

        return this;
    }

    /// <summary>
    /// Sets <see cref="Align.BOTTOM"/> and clears <see cref="Align.TOP"/> for
    /// the alignment of widgets within each row.
    /// </summary>
    public HorizontalGroup RowBottom()
    {
        _rowAlign |= Align.BOTTOM;
        _rowAlign &= ~Align.TOP;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.RIGHT"/> and clears <see cref="Align.LEFT"/> for
    /// the alignment of each row of widgets when <see cref="Wrap"/> is enabled.
    /// </summary>
    public HorizontalGroup RowRight()
    {
        _rowAlign |= Align.RIGHT;
        _rowAlign &= ~Align.LEFT;

        return this;
    }

    #endregion alignment
}
