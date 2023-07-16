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

using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A group that lays out its children top to bottom vertically, with optional wrapping. <seealso cref="getChildren()"/> can be sorted to
/// change the order of the actors (eg <seealso cref="Actor.setZIndex(int)"/>). This can be easier than using <seealso cref="Table"/> when actors need
/// to be inserted into or removed from the middle of the group. <seealso cref="invalidate()"/> must be called after changing the children
/// order.
/// <para>
/// The preferred width is the largest preferred width of any child. The preferred height is the sum of the children's preferred
/// heights plus spacing. The preferred size is slightly different when <seealso cref="wrap() wrap"/> is enabled. The min size is the
/// preferred size and the max size is 0.
/// </para>
/// <para>
/// Widgets are sized using their <seealso cref="Layout.getPrefWidth() preferred height"/>, so widgets which return 0 as their preferred
/// height will be given a height of 0.
/// @author Nathan Sweet 
/// </para>
/// </summary>
public class VerticalGroup : WidgetGroup
{
    private float          _prefWidth;
    private float          _prefHeight;
    private float          _lastPrefWidth;
    private bool           _sizeInvalid = true;
    private List< float >? _columnSizes; // column height, column width, ...
    private int            _align = Align.Top;
    private int            _columnAlign;
    private bool           _reverse;
    private bool           _round = true;
    private bool           _wrap;
    private bool           _expand;
    private float          _space;
    private float          _wrapSpace;
    private float          _fill;
    private float          _padTop;
    private float          _padLeft;
    private float          _padBottom;
    private float          _padRight;

    public VerticalGroup()
    {
        Touchable = Touchable.ChildrenOnly;
    }

    public new void Invalidate()
    {
        base.Invalidate();
        _sizeInvalid = true;
    }

    private void ComputeSize()
    {
        _sizeInvalid = false;

        SnapshotArray< Actor > children = Children;
        var                    n        = children.Size;

        _prefWidth = 0;

        if ( _wrap )
        {
            _prefHeight = 0;

            if ( _columnSizes == null )
            {
                _columnSizes = new List< float >();
            }
            else
            {
                _columnSizes.Clear();
            }

            List< float > columnSizes = this._columnSizes;

            var space     = this._space;
            var wrapSpace = this._wrapSpace;

            var pad         = _padTop + _padBottom;
            var groupHeight = Height - pad;

            float x           = 0;
            float y           = 0;
            float columnWidth = 0;
            int   i           = 0, incr = 1;

            if ( _reverse )
            {
                i    = n - 1;
                n    = -1;
                incr = -1;
            }

            for ( ; i != n; i += incr )
            {
                Actor child = children.Get( i );

                float width;
                float height;

                if ( child is ILayout layoutChild )
                {
                    width  = layoutChild.PrefWidth;
                    height = layoutChild.PrefHeight;

                    if ( height > groupHeight )
                    {
                        height = Math.Max( groupHeight, layoutChild.MinHeight );
                    }
                }
                else
                {
                    width  = child.Width;
                    height = child.Height;
                }

                var incrY = height + ( y > 0 ? space : 0 );

                if ( y + incrY > groupHeight && y > 0 )
                {
                    columnSizes.Add( y );
                    columnSizes.Add( columnWidth );

                    _prefHeight = Math.Max( _prefHeight, y + pad );

                    if ( x > 0 )
                    {
                        x += wrapSpace;
                    }

                    x           += columnWidth;
                    columnWidth =  0;
                    y           =  0;
                    incrY       =  height;
                }

                y           += incrY;
                columnWidth =  Math.Max( columnWidth, width );
            }

            columnSizes.Add( y );
            columnSizes.Add( columnWidth );

            _prefHeight = Math.Max( _prefHeight, y + pad );

            if ( x > 0 )
            {
                x += wrapSpace;
            }

            _prefWidth = Math.Max( _prefWidth, x + columnWidth );
        }
        else
        {
            _prefHeight = _padTop + _padBottom + _space * ( n - 1 );

            for ( var i = 0; i < n; i++ )
            {
                Actor child = children.Get( i );

                if ( child is ILayout layoutChild )
                {
                    _prefWidth  =  Math.Max( _prefWidth, layoutChild.PrefWidth );
                    _prefHeight += layoutChild.PrefHeight;
                }
                else
                {
                    _prefWidth  =  Math.Max( _prefWidth, child.Width );
                    _prefHeight += child.Height;
                }
            }
        }

        _prefWidth += _padLeft + _padRight;

        if ( _round )
        {
            _prefWidth  = ( long )Math.Round( _prefWidth, MidpointRounding.AwayFromZero );
            _prefHeight = ( long )Math.Round( _prefHeight, MidpointRounding.AwayFromZero );
        }
    }

    public new void Layout()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        if ( _wrap )
        {
            layoutWrapped();

            return;
        }

        var round       = this._round;
        var align       = this._align;
        var space       = this._space;
        var padLeft     = this._padLeft;
        var fill        = this._fill;
        var columnWidth = ( _expand ? Width : _prefWidth ) - padLeft - _padRight;
        var y           = _prefHeight - _padTop + space;

        if ( ( align & Align.top ) != 0 )
        {
            y += getHeight() - prefHeight;
        }
        else if ( ( align & Align.bottom ) == 0 ) // center
        {
            y += ( getHeight() - prefHeight ) / 2;
        }

        float startX;

        if ( ( align & Align.left ) != 0 )
        {
            startX = padLeft;
        }
        else if ( ( align & Align.right ) != 0 )
        {
            startX = getWidth() - padRight - columnWidth;
        }
        else
        {
            startX = padLeft + ( getWidth() - padLeft - padRight - columnWidth ) / 2;
        }

        align = columnAlign;

        SnapshotArray< Actor > children = getChildren();
        int                    i        = 0, n = children.size, incr = 1;

        if ( reverse )
        {
            i    = n - 1;
            n    = -1;
            incr = -1;
        }

        for ( var r = 0; i != n; i += incr )
        {
            Actor child = children.get( i );

            float  width, height;
            Layout layout = null;

            if ( child is Layout )
            {
                layout = ( Layout )child;
                width  = layout.getPrefWidth();
                height = layout.getPrefHeight();
            }
            else
            {
                width  = child.getWidth();
                height = child.getHeight();
            }

            if ( fill > 0 )
            {
                width = columnWidth * fill;
            }

            if ( layout != null )
            {
                width = Math.Max( width, layout.getMinWidth() );
                float maxWidth = layout.getMaxWidth();

                if ( maxWidth > 0 && width > maxWidth )
                {
                    width = maxWidth;
                }
            }

            var x = startX;

            if ( ( align & Align.right ) != 0 )
            {
                x += columnWidth - width;
            }
            else if ( ( align & Align.left ) == 0 ) // center
            {
                x += ( columnWidth - width ) / 2;
            }

            y -= height + space;

            if ( round )
            {
                child.setBounds
                    (
                    ( int )Math.Round( x, MidpointRounding.AwayFromZero ),
                    ( int )Math.Round( y, MidpointRounding.AwayFromZero ),
                    ( int )Math.Round( width, MidpointRounding.AwayFromZero ),
                    ( int )Math.Round( height, MidpointRounding.AwayFromZero )
                    );
            }
            else
            {
                child.setBounds( x, y, width, height );
            }

            if ( layout != null )
            {
                layout.validate();
            }
        }
    }

    private void layoutWrapped()
    {
        var prefWidth = getPrefWidth();

        if ( prefWidth != lastPrefWidth )
        {
            lastPrefWidth = prefWidth;
            invalidateHierarchy();
        }

        int     align     = this.align;
        boolean round     = this.round;
        float   space     = this.space, padLeft = this.padLeft, fill = this.fill, wrapSpace = this.wrapSpace;
        float   maxHeight = prefHeight - padTop - padBottom;
        float   columnX   = padLeft,                     groupHeight = getHeight();
        float   yStart    = prefHeight - padTop + space, y           = 0, columnWidth = 0;

        if ( ( align & Align.right ) != 0 )
            columnX += getWidth() - prefWidth;
        else if ( ( align & Align.left ) == 0 ) // center
            columnX += ( getWidth() - prefWidth ) / 2;

        if ( ( align & Align.top ) != 0 )
            yStart += groupHeight - prefHeight;
        else if ( ( align & Align.bottom ) == 0 ) // center
            yStart += ( groupHeight - prefHeight ) / 2;

        groupHeight -= padTop;
        align       =  columnAlign;

        FloatArray             columnSizes = this.columnSizes;
        SnapshotArray< Actor > children    = getChildren();
        int                    i           = 0, n = children.size, incr = 1;

        if ( reverse )
        {
            i    = n - 1;
            n    = -1;
            incr = -1;
        }

        for ( var r = 0; i != n; i += incr )
        {
            Actor child = children.get( i );

            float                 width, height;
            Layout                layout = null;
            if ( child instanceof Layout) {
                layout = ( Layout )child;
                width  = layout.getPrefWidth();
                height = layout.getPrefHeight();
                if ( height > groupHeight ) height = Math.max( groupHeight, layout.getMinHeight() );
            } else {
                width  = child.getWidth();
                height = child.getHeight();
            }

            if ( y - height - space < padBottom || r == 0 )
            {
                r = Math.min
                    ( r, columnSizes.size - 2 ); // In case an actor changed size without invalidating this layout.

                y = yStart;

                if ( ( align & Align.bottom ) != 0 )
                    y -= maxHeight - columnSizes.get( r );
                else if ( ( align & Align.top ) == 0 ) // center
                    y -= ( maxHeight - columnSizes.get( r ) ) / 2;

                if ( r > 0 )
                {
                    columnX += wrapSpace;
                    columnX += columnWidth;
                }

                columnWidth =  columnSizes.get( r + 1 );
                r           += 2;
            }

            if ( fill > 0 ) width = columnWidth * fill;

            if ( layout != null )
            {
                width = Math.max( width, layout.getMinWidth() );
                float maxWidth                                = layout.getMaxWidth();
                if ( maxWidth > 0 && width > maxWidth ) width = maxWidth;
            }

            var x = columnX;

            if ( ( align & Align.right ) != 0 )
                x += columnWidth - width;
            else if ( ( align & Align.left ) == 0 ) // center
                x += ( columnWidth - width ) / 2;

            y -= height + space;

            if ( round )
                child.setBounds( Math.round( x ), Math.round( y ), Math.round( width ), Math.round( height ) );
            else
                child.setBounds( x, y, width, height );

            if ( layout != null ) layout.validate();
        }
    }

    public float getPrefWidth()
    {
        if ( sizeInvalid ) computeSize();

        return prefWidth;
    }

    public float getPrefHeight()
    {
        if ( wrap ) return 0;
        if ( sizeInvalid ) computeSize();

        return prefHeight;
    }

    /** If true (the default), positions and sizes are rounded to integers. */
    public void setRound( boolean round )
    {
        this.round = round;
    }

    /** The children will be displayed last to first. */
    public VerticalGroup reverse()
    {
        this.reverse = true;

        return this;
    }

    /** If true, the children will be displayed last to first. */
    public VerticalGroup reverse( boolean reverse )
    {
        this.reverse = reverse;

        return this;
    }

    public boolean getReverse()
    {
        return reverse;
    }

    /** Sets the vertical space between children. */
    public VerticalGroup space( float space )
    {
        this.space = space;

        return this;
    }

    public float getSpace()
    {
        return space;
    }

    /** Sets the horizontal space between columns when wrap is enabled. */
    public VerticalGroup wrapSpace( float wrapSpace )
    {
        this.wrapSpace = wrapSpace;

        return this;
    }

    public float getWrapSpace()
    {
        return wrapSpace;
    }

    /** Sets the padTop, padLeft, padBottom, and padRight to the specified value. */
    public VerticalGroup pad( float pad )
    {
        padTop    = pad;
        padLeft   = pad;
        padBottom = pad;
        padRight  = pad;

        return this;
    }

    public VerticalGroup pad( float top, float left, float bottom, float right )
    {
        padTop    = top;
        padLeft   = left;
        padBottom = bottom;
        padRight  = right;

        return this;
    }

    public VerticalGroup padTop( float padTop )
    {
        this.padTop = padTop;

        return this;
    }

    public VerticalGroup padLeft( float padLeft )
    {
        this.padLeft = padLeft;

        return this;
    }

    public VerticalGroup padBottom( float padBottom )
    {
        this.padBottom = padBottom;

        return this;
    }

    public VerticalGroup padRight( float padRight )
    {
        this.padRight = padRight;

        return this;
    }

    public float getPadTop()
    {
        return padTop;
    }

    public float getPadLeft()
    {
        return padLeft;
    }

    public float getPadBottom()
    {
        return padBottom;
    }

    public float getPadRight()
    {
        return padRight;
    }

    /** Sets the alignment of all widgets within the vertical group. Set to {@link Align#center}, {@link Align#top},
	     * {@link Align#bottom}, {@link Align#left}, {@link Align#right}, or any combination of those. */
    public VerticalGroup align( int align )
    {
        this.align = align;

        return this;
    }

    /** Sets the alignment of all widgets within the vertical group to {@link Align#center}. This clears any other alignment. */
    public VerticalGroup center()
    {
        align = Align.center;

        return this;
    }

    /** Sets {@link Align#top} and clears {@link Align#bottom} for the alignment of all widgets within the vertical group. */
    public VerticalGroup top()
    {
        align |= Align.top;
        align &= ~Align.bottom;

        return this;
    }

    /** Adds {@link Align#left} and clears {@link Align#right} for the alignment of all widgets within the vertical group. */
    public VerticalGroup left()
    {
        align |= Align.left;
        align &= ~Align.right;

        return this;
    }

    /** Sets {@link Align#bottom} and clears {@link Align#top} for the alignment of all widgets within the vertical group. */
    public VerticalGroup bottom()
    {
        align |= Align.bottom;
        align &= ~Align.top;

        return this;
    }

    /** Adds {@link Align#right} and clears {@link Align#left} for the alignment of all widgets within the vertical group. */
    public VerticalGroup right()
    {
        align |= Align.right;
        align &= ~Align.left;

        return this;
    }

    public int getAlign()
    {
        return align;
    }

    public VerticalGroup fill()
    {
        fill = 1f;

        return this;
    }

    /** @param fill 0 will use preferred height. */
    public VerticalGroup fill( float fill )
    {
        this.fill = fill;

        return this;
    }

    public float getFill()
    {
        return fill;
    }

    public VerticalGroup expand()
    {
        expand = true;

        return this;
    }

    /** When true and wrap is false, the columns will take up the entire vertical group width. */
    public VerticalGroup expand( boolean expand )
    {
        this.expand = expand;

        return this;
    }

    public boolean getExpand()
    {
        return expand;
    }

    /** Sets fill to 1 and expand to true. */
    public VerticalGroup grow()
    {
        expand = true;
        fill   = 1;

        return this;
    }

    /** If false, the widgets are arranged in a single column and the preferred height is the widget heights plus spacing.
	     * <p>
	     * If true, the widgets will wrap using the height of the vertical group. The preferred height of the group will be 0 as it is
	     * expected that something external will set the height of the group. Widgets are sized to their preferred height unless it is
	     * larger than the group's height, in which case they are sized to the group's height but not less than their minimum height.
	     * Default is false.
	     * <p>
	     * When wrap is enabled, the group's preferred width depends on the height of the group. In some cases the parent of the group
	     * will need to layout twice: once to set the height of the group and a second time to adjust to the group's new preferred
	     * width. */
    public VerticalGroup wrap()
    {
        wrap = true;

        return this;
    }

    public VerticalGroup wrap( boolean wrap )
    {
        this.wrap = wrap;

        return this;
    }

    public boolean getWrap()
    {
        return wrap;
    }

    /** Sets the vertical alignment of each column of widgets when {@link #wrap() wrapping} is enabled and sets the horizontal
	     * alignment of widgets within each column. Set to {@link Align#center}, {@link Align#top}, {@link Align#bottom},
	     * {@link Align#left}, {@link Align#right}, or any combination of those. */
    public VerticalGroup columnAlign( int columnAlign )
    {
        this.columnAlign = columnAlign;

        return this;
    }

    /** Sets the alignment of widgets within each column to {@link Align#center}. This clears any other alignment. */
    public VerticalGroup columnCenter()
    {
        columnAlign = Align.center;

        return this;
    }

    /** Adds {@link Align#top} and clears {@link Align#bottom} for the alignment of each column of widgets when {@link #wrap()
	     * wrapping} is enabled. */
    public VerticalGroup columnTop()
    {
        columnAlign |= Align.top;
        columnAlign &= ~Align.bottom;

        return this;
    }

    /** Adds {@link Align#left} and clears {@link Align#right} for the alignment of widgets within each column. */
    public VerticalGroup columnLeft()
    {
        columnAlign |= Align.left;
        columnAlign &= ~Align.right;

        return this;
    }

    /** Adds {@link Align#bottom} and clears {@link Align#top} for the alignment of each column of widgets when {@link #wrap()
	     * wrapping} is enabled. */
    public VerticalGroup columnBottom()
    {
        columnAlign |= Align.bottom;
        columnAlign &= ~Align.top;

        return this;
    }

    /** Adds {@link Align#right} and clears {@link Align#left} for the alignment of widgets within each column. */
    public VerticalGroup columnRight()
    {
        columnAlign |= Align.right;
        columnAlign &= ~Align.left;

        return this;
    }

    protected void drawDebugBounds( ShapeRenderer shapes )
    {
        super.drawDebugBounds( shapes );

        if ( !getDebug() ) return;
        shapes.set( ShapeType.Line );
        if ( getStage() != null ) shapes.setColor( getStage().getDebugColor() );

        shapes.rect
            (
            getX() + padLeft, getY() + padBottom, getOriginX(), getOriginY(), getWidth() - padLeft - padRight,
            getHeight() - padBottom - padTop, getScaleX(), getScaleY(), getRotation()
            );
    }
}