using System.Diagnostics;

using LibGDXSharp.G2D;
using LibGDXSharp.Maths;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;
using LibGDXSharp.Utils.Collections.Extensions;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class Table : WidgetGroup
{
    public enum DebugType
    {
        None,
        All,
        Table,
        Cell,
        Actor
    }

    // ------------------------------------------------------------------------

    internal class DebugRect : RectangleShape
    {
        internal static Pool< DebugRect > Pool  { get; set; } = Pools< DebugRect >.Get();
        internal        Color             Color { get; set; } = null!;
    }

    // ------------------------------------------------------------------------

    public readonly static Color DebugTableColor = new(0, 0, 1, 1);
    public readonly static Color DebugCellColor  = new(1, 0, 0, 1);
    public readonly static Color DebugActorColor = new(0, 1, 0, 1);

    // ------------------------------------------------------------------------

    public Pool< Cell > CellPool { get; } = new()
    {
        NewObject = NewCellObject
    };

    private static Cell NewCellObject() => new();

    // ------------------------------------------------------------------------

    private static float[]? _columnWeightedWidth;
    private static float[]? _rowWeightedHeight;

    private readonly List< Cell > _cells = new(4);
    private readonly Cell         _cellDefaults;
    private readonly List< Cell > _columnDefaults = new(2);

    private Cell    _rowDefaults;
    private bool    _implicitEndRow;
    private bool    _sizeInvalid     = true;
    private float[] _columnMinWidth  = null!;
    private float[] _rowMinHeight    = null!;
    private float[] _columnPrefWidth = null!;
    private float[] _rowPrefHeight   = null!;
    private float[] _columnWidth     = null!;
    private float[] _rowHeight       = null!;
    private float[] _expandWidth     = null!;
    private float[] _expandHeight    = null!;
    private float   _tableMinWidth;
    private float   _tableMinHeight;
    private float   _tablePrefWidth;
    private float   _tablePrefHeight;

    private Value _padTop    = backgroundTop;
    private Value _padLeft   = backgroundLeft;
    private Value _padBottom = backgroundBottom;
    private Value _padRight  = backgroundRight;

    private int                _alignment = LibGDXSharp.Utils.Align.Center;
    private List< DebugRect >? _debugRects;

    private IDrawable? _background;
    private bool       _clip;
    private bool       _round = true;

    protected Table() : this( null )
    {
    }

    /// <summary>
    /// Creates a table with a skin, which is required to use <see cref="AssetDescriptor"/>
    /// </summary>
    public Table( Skin? skin )
    {
        this.Skin = skin;

        _cellDefaults = ObtainCell();

        Transform = false;
        Touchable = Touchable.ChildrenOnly;
    }

    private Cell ObtainCell()
    {
        Cell cell = CellPool.Obtain();
        cell.Table = this;

        return cell;
    }

    protected new void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        if ( Transform )
        {
            ApplyTransform( batch, ComputeTransform() );
            DrawBackground( batch, parentAlpha, 0, 0 );

            if ( _clip )
            {
                batch.Flush();

                var padLeft   = this._padLeft.Get( this );
                var padBottom = this._padBottom.Get( this );

                if ( ClipBegin
                        (
                         padLeft,
                         padBottom,
                         Width - padLeft - _padRight.Get( this ),
                         Height - padBottom - _padTop.Get( this )
                        )
                   )
                {
                    DrawChildren( batch, parentAlpha );
                    batch.Flush();
                    ClipEnd();
                }
            }
            else
            {
                DrawChildren( batch, parentAlpha );
            }

            ResetTransform( batch );
        }
        else
        {
            DrawBackground( batch, parentAlpha, GetX(), GetY() );
            base.Draw( batch, parentAlpha );
        }
    }

    /// <summary>
    /// Called to draw the background, before clipping is applied (if enabled).
    /// Default implementation draws the background drawable.
    /// </summary>
    protected void DrawBackground( IBatch batch, float parentAlpha, float x, float y )
    {
        if ( _background == null ) return;

        System.Diagnostics.Debug.Assert
            ( ( ( Actor )this ).Color != null, nameof( this.Color ) + " != null" );

        batch.SetColor
            (
             ( ( Actor )this ).Color.R,
             ( ( Actor )this ).Color.G,
             ( ( Actor )this ).Color.B,
             ( ( Actor )this ).Color.A * parentAlpha
            );

        _background.Draw( batch, x, y, Width, Height );
    }

    /// <summary>
    /// Sets the background drawable from the skin and adjusts the table's padding
    /// to match the background. This may only be called if a skin has been set
    /// with <see cref="Table(Skin)"/> or <see cref="Skin"/>.
    /// </summary>
    protected void SetBackground( string drawableName )
    {
        if ( Skin == null )
        {
            throw new IllegalStateException( "Table must have a skin set to use this method." );
        }

        SetBackground( Skin.GetDrawable( drawableName ) );
    }

    /// <summary>
    /// <param name="background"> May be null to clear the background. </param>
    /// </summary>
    protected void SetBackground( IDrawable? background )
    {
        if ( this._background == background ) return;

        float padTopOld    = GetPadTop(),
              padLeftOld   = GetPadLeft(),
              padBottomOld = GetPadBottom(),
              padRightOld  = GetPadRight();

        this._background = background; // The default pad values use the background's padding.

        float padTopNew    = GetPadTop(),
              padLeftNew   = GetPadLeft(),
              padBottomNew = GetPadBottom(),
              padRightNew  = GetPadRight();

        if ( ( !( padTopOld + padBottomOld ).Equals( padTopNew + padBottomNew ) )
             || ( !( padLeftOld + padRightOld ).Equals( padLeftNew + padRightNew ) ) )
        {
            InvalidateHierarchy();
        }
        else if ( !padTopOld.Equals( padTopNew )
                  || !padLeftOld.Equals( padLeftNew )
                  || !padBottomOld.Equals( padBottomNew )
                  || !padRightOld.Equals( padRightNew ) )
        {
            Invalidate();
        }
    }

    public Table Background( IDrawable? background )
    {
        SetBackground( background );

        return this;
    }

    public Table Background( string drawableName )
    {
        SetBackground( drawableName );

        return this;
    }

    public IDrawable? GetBackground()
    {
        return _background;
    }

    public new Actor? Hit( float x, float y, bool touchable )
    {
        if ( _clip )
        {
            if ( touchable && ( Touchable == Touchable.Disabled ) ) return null;
            if ( ( x < 0 ) || ( x >= Width ) || ( y < 0 ) || ( y >= Height ) ) return null;
        }

        return base.Hit( x, y, touchable );
    }

    public Table SetClip( bool enabled = true )
    {
        Clip = enabled;

        return this;
    }

    public new void Invalidate()
    {
        _sizeInvalid = true;
        base.Invalidate();
    }

    /// <summary>
    /// Adds a new cell to the table with the specified actor.
    /// </summary>
    public Cell Add<T>( T? actor ) where T : Actor
    {
        Cell cell = ObtainCell();
        cell.Actor = actor;

        // The row was ended for layout, not by the user, so revert it.
        if ( _implicitEndRow )
        {
            _implicitEndRow = false;
            Rows--;
            _cells.Peek().EndRow = false;
        }

        var cellCount = _cells.Count;

        if ( cellCount > 0 )
        {
            // Set cell column and row.
            Cell lastCell = _cells.Peek();

            if ( !lastCell.EndRow )
            {
                cell.Column = lastCell.Column + lastCell.Colspan;
                cell.Row    = lastCell.Row;
            }
            else
            {
                cell.Column = 0;
                cell.Row    = lastCell.Row + 1;
            }

            // Set the index of the cell above.
            if ( cell.Row > 0 )
            {
                outer:

                for ( var i = cellCount - 1; i >= 0; i-- )
                {
                    Cell other = _cells[ i ];

                    for ( int column = other.Column, nn = column + other.Colspan; column < nn; column++ )
                    {
                        if ( column == cell.Column )
                        {
                            cell.CellAboveIndex = i;

                            goto outer;
                        }
                    }
                }
            }
        }
        else
        {
            cell.Column = 0;
            cell.Row    = 0;
        }

        _cells.Add( cell );

        cell.Set( _cellDefaults );

        if ( cell.Column < _columnDefaults.Count ) cell.Merge( _columnDefaults[ cell.Column ] );

        cell.Merge( _rowDefaults );

        if ( actor != null ) AddActor( actor );

        return cell;
    }

    public Table Add( params Actor[] actors )
    {
        for ( int i = 0, n = actors.Length; i < n; i++ )
        {
            Add( actors[ i ] );
        }

        return this;
    }

    /** Adds a new cell with a label. This may only be called if a skin has been set with {@link Table#Table(Skin)} or
	 * {@link #setSkin(Skin)}. */
    public Cell< Label > Add( @Null charSequence text )
    {
        if ( skin == null ) throw new IllegalStateException( "Table must have a skin set to use this method." );

        return add( new Label( text, skin ) );
    }

    /** Adds a new cell with a label. This may only be called if a skin has been set with {@link Table#Table(Skin)} or
	 * {@link #setSkin(Skin)}. */
    public Cell< Label > Add( @Null charSequence text, String LabelStyleName) {
        if ( skin == null ) throw new IllegalStateException( "Table must have a skin set to use this method." );

        return add( new Label( text, skin.get( labelStyleName, LabelStyle.class)));
    }

    /** Adds a new cell with a label. This may only be called if a skin has been set with {@link Table#Table(Skin)} or
	 * {@link #setSkin(Skin)}. */
    public Cell< Label > Add( @Null charSequence text, String _fontName, _null Color Color) {
        if ( skin == null ) throw new IllegalStateException( "Table must have a skin set to use this method." );

        return add( new Label( text, new LabelStyle( skin.getFont( fontName ), color ) ) );
    }

    /** Adds a new cell with a label. This may only be called if a skin has been set with {@link Table#Table(Skin)} or
	 * {@link #setSkin(Skin)}. */
    public Cell< Label > Add( @Null charSequence text, String _fontName, _string colorName) {
        if ( skin == null ) throw new IllegalStateException( "Table must have a skin set to use this method." );

        return add( new Label( text, new LabelStyle( skin.getFont( fontName ), skin.getColor( colorName ) ) ) );
    }

    /** Adds a cell without an actor. */
    public Cell Add()
    {
        return add( ( Actor )null );
    }

    /** Adds a new cell to the table with the specified actors in a {@link Stack}.
	 * @param actors May be null or empty to add a stack without any actors. */
    public Cell< Stack > Stack( @Null actor...actors )
    {
        var stack = new Stack();

        if ( actors != null )
        {
            for ( int i = 0, n = actors.length; i < n; i++ )
                stack.addActor( actors[ i ] );
        }

        return add( stack );
    }

    public bool RemoveActor( Actor actor )
    {
        return RemoveActor( actor, true );
    }

    public bool RemoveActor( Actor actor, bool unfocus )
    {
        if ( !super.removeActor( actor, unfocus ) ) return false;
        Cell cell                      = GetCell( actor );
        if ( cell != null ) cell.actor = null;

        return true;
    }

    public Actor RemoveActorAt( int index, bool unfocus )
    {
        Actor actor                    = super.removeActorAt( index, unfocus );
        Cell  cell                     = GetCell( actor );
        if ( cell != null ) cell.actor = null;

        return actor;
    }

    /** Removes all actors and cells from the table. */
    public void ClearChildren()
    {
        Object[] cells = this.cells.items;

        for ( var i = this.cells.size - 1; i >= 0; i-- )
        {
            var   cell  = ( Cell )cells[ i ];
            Actor actor = cell.actor;
            if ( actor != null ) actor.remove();
        }

        cellPool.freeAll( this.cells );
        this.cells.clear();
        rows    = 0;
        columns = 0;
        if ( rowDefaults != null ) cellPool.free( rowDefaults );
        rowDefaults    = null;
        implicitEndRow = false;

        super.clearChildren();
    }

    /** Removes all actors and cells from the table (same as {@link #clearChildren()}) and additionally resets all table properties
	 * and cell, column, and row defaults. */
    public void Reset()
    {
        ClearChildren();
        padTop    = backgroundTop;
        padLeft   = backgroundLeft;
        padBottom = backgroundBottom;
        padRight  = backgroundRight;
        Align     = LibGDXSharp.Utils.Align.center;
        Debug( DebugType.none );
        cellDefaults.reset();

        for ( int i = 0, n = ColumnDefaults.size; i < n; i++ )
        {
            Cell columnCell = ColumnDefaults.get( i );
            if ( columnCell != null ) cellPool.free( columnCell );
        }

        ColumnDefaults.clear();
    }

    /** Indicates that subsequent cells should be added to a new row and returns the cell values that will be used as the defaults
	 * for all cells in the new row. */
    public Cell AddRow()
    {
        if ( cells.size > 0 )
        {
            if ( !implicitEndRow )
            {
                if ( cells.peek().endRow ) return rowDefaults; // Row was already ended.
                EndRow();
            }

            invalidate();
        }

        implicitEndRow = false;
        if ( rowDefaults != null ) cellPool.free( rowDefaults );
        rowDefaults = obtainCell();
        rowDefaults.clear();

        return rowDefaults;
    }

    private void EndRow()
    {
        object[] cells      = this.cells.items;
        var      rowColumns = 0;

        for ( var i = this.cells.size - 1; i >= 0; i-- )
        {
            var cell = ( Cell )cells[ i ];

            if ( cell.endRow ) break;
            rowColumns += cell.colspan;
        }

        Columns = Math.Max( Columns, rowColumns );
        rows++;
        this.cells.peek().endRow = true;
    }

    /** Gets the cell values that will be used as the defaults for all cells in the specified column. Columns are indexed starting
	 * at 0. */
    public Cell ColumnDefaults( int column )
    {
        Cell cell = ColumnDefaults.size > column ? ColumnDefaults.get( column ) : null;

        if ( cell == null )
        {
            cell = obtainCell();
            cell.clear();

            if ( column >= ColumnDefaults.size )
            {
                for ( int i = ColumnDefaults.size; i < column; i++ )
                {
                    ColumnDefaults.add( null );
                }

                ColumnDefaults.add( cell );
            }
            else
            {
                ColumnDefaults.set( column, cell );
            }
        }

        return cell;
    }

    /** Returns the cell for the specified actor in this table, or null. */
    public @Null< T Extends Actor> Cell< T > GetCell( T actor )
    {
        if ( actor == null ) throw new IllegalArgumentException( "actor cannot be null." );
        object[] cells = this.cells.items;

        for ( int i = 0, n = this.cells.size; i < n; i++ )
        {
            var c = ( Cell )cells[ i ];

            if ( c.Actor == actor ) return c;
        }

        return null;
    }

    /** Returns the cells for this table. */
    public List< Cell > GetCells()
    {
        return cells;
    }

    public float GetPrefWidth()
    {
        if ( sizeInvalid ) ComputeSize();
        float width = tablePrefWidth;

        if ( background != null ) return Math.Max( width, background.getMinWidth() );

        return width;
    }

    public float GetPrefHeight()
    {
        if ( sizeInvalid ) ComputeSize();
        float height = tablePrefHeight;

        if ( background != null ) return Math.Max( height, background.getMinHeight() );

        return height;
    }

    public float GetMinWidth()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _tableMinWidth;
    }

    public float GetMinHeight()
    {
        if ( _sizeInvalid ) ComputeSize();

        return _tableMinHeight;
    }

    /** The cell values that will be used as the defaults for all cells. */
    public Cell Defaults()
    {
        return _cellDefaults;
    }

    /** Sets the padTop, padLeft, padBottom, and padRight around the table to the specified value. */
    public Table Pad( Value pad )
    {
        ArgumentNullException.ThrowIfNull( pad );

        _padTop      = pad;
        _padLeft     = pad;
        _padBottom   = pad;
        _padRight    = pad;
        _sizeInvalid = true;

        return this;
    }

    public Table Pad( Value top, Value left, Value bottom, Value right )
    {
        if ( top == null ) throw new IllegalArgumentException( "top cannot be null." );
        if ( left == null ) throw new IllegalArgumentException( "left cannot be null." );
        if ( bottom == null ) throw new IllegalArgumentException( "bottom cannot be null." );
        if ( right == null ) throw new IllegalArgumentException( "right cannot be null." );
        padTop      = top;
        padLeft     = left;
        padBottom   = bottom;
        padRight    = right;
        sizeInvalid = true;

        return this;
    }

    /** Padding at the top edge of the table. */
    public Table PadTop( Value padTop )
    {
        if ( padTop == null ) throw new IllegalArgumentException( "padTop cannot be null." );
        this.padTop = padTop;
        sizeInvalid = true;

        return this;
    }

    /** Padding at the left edge of the table. */
    public Table PadLeft( Value padLeft )
    {
        if ( padLeft == null ) throw new IllegalArgumentException( "padLeft cannot be null." );
        this.padLeft = padLeft;
        sizeInvalid  = true;

        return this;
    }

    /** Padding at the bottom edge of the table. */
    public Table PadBottom( Value padBottom )
    {
        if ( padBottom == null ) throw new IllegalArgumentException( "padBottom cannot be null." );
        this.padBottom = padBottom;
        sizeInvalid    = true;

        return this;
    }

    /** Padding at the right edge of the table. */
    public Table PadRight( Value padRight )
    {
        if ( padRight == null ) throw new IllegalArgumentException( "padRight cannot be null." );
        this.padRight = padRight;
        sizeInvalid   = true;

        return this;
    }

    /** Sets the padTop, padLeft, padBottom, and padRight around the table to the specified value. */
    public Table Pad( float pad )
    {
        pad( Value.Fixed.valueOf( pad ) );

        return this;
    }

    public Table Pad( float top, float left, float bottom, float right )
    {
        padTop      = Value.Fixed.valueOf( top );
        padLeft     = Value.Fixed.valueOf( left );
        padBottom   = Value.Fixed.valueOf( bottom );
        padRight    = Value.Fixed.valueOf( right );
        sizeInvalid = true;

        return this;
    }

    /** Padding at the top edge of the table. */
    public Table PadTop( float padTop )
    {
        this.padTop = Value.Fixed.valueOf( padTop );
        sizeInvalid = true;

        return this;
    }

    /** Padding at the left edge of the table. */
    public Table PadLeft( float padLeft )
    {
        this.padLeft = Value.Fixed.valueOf( padLeft );
        sizeInvalid  = true;

        return this;
    }

    /** Padding at the bottom edge of the table. */
    public Table PadBottom( float padBottom )
    {
        this.padBottom = Value.Fixed.valueOf( padBottom );
        sizeInvalid    = true;

        return this;
    }

    /** Padding at the right edge of the table. */
    public Table PadRight( float padRight )
    {
        this.padRight = Value.Fixed.valueOf( padRight );
        sizeInvalid   = true;

        return this;
    }

    /** Alignment of the logical table within the table actor. Set to {@link Align#center}, {@link Align#top}, {@link Align#bottom}
	 * , {@link Align#left}, {@link Align#right}, or any combination of those. */
    public Table Align( int align )
    {
        this._alignment = align;

        return this;
    }

    /** Sets the alignment of the logical table within the table actor to {@link Align#center}. This clears any other alignment. */
    public Table Center()
    {
        _alignment = LibGDXSharp.Utils.Align.Center;

        return this;
    }

    /** Adds {@link Align#top} and clears {@link Align#bottom} for the alignment of the logical table within the table actor. */
    public Table Top()
    {
        _alignment |= LibGDXSharp.Utils.Align.Top;
        _alignment &= ~LibGDXSharp.Utils.Align.Bottom;

        return this;
    }

    /** Adds {@link Align#left} and clears {@link Align#right} for the alignment of the logical table within the table actor. */
    public Table Left()
    {
        _alignment |= LibGDXSharp.Utils.Align.Left;
        _alignment &= ~LibGDXSharp.Utils.Align.Right;

        return this;
    }

    /** Adds {@link Align#bottom} and clears {@link Align#top} for the alignment of the logical table within the table actor. */
    public Table Bottom()
    {
        _alignment |= LibGDXSharp.Utils.Align.Bottom;
        _alignment &= ~LibGDXSharp.Utils.Align.Top;

        return this;
    }

    /** Adds {@link Align#right} and clears {@link Align#left} for the alignment of the logical table within the table actor. */
    public Table Right()
    {
        _alignment |= LibGDXSharp.Utils.Align.Right;
        _alignment &= ~LibGDXSharp.Utils.Align.Left;

        return this;
    }

    public void SetDebug( bool enabled )
    {
        Debug( enabled ? DebugType.All : DebugType.None );
    }

    public Table Debug()
    {
        super.debug();

        return this;
    }

    public new Table DebugAll()
    {
        base.DebugAll();

        return this;
    }

    /** Turns on table debug lines. */
    public Table DebugTable()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Table )
        {
            this.TableDebug = DebugType.Table;
            Invalidate();
        }

        return this;
    }

    /** Turns on cell debug lines. */
    public Table DebugCell()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Cell )
        {
            this.TableDebug = DebugType.Cell;
            Invalidate();
        }

        return this;
    }

    /** Turns on actor debug lines. */
    public Table DebugActor()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Actor )
        {
            this.TableDebug = DebugType.Actor;
            Invalidate();
        }

        return this;
    }

    /** Turns debug lines on or off. */
    public Table Debug( Debug debug )
    {
        super.setDebug( debug != DebugType.none );

        if ( this.debug != debug )
        {
            this.debug = debug;

            if ( debug == DebugType.none )
            {
                clearDebugRects();
            }
            else
            {
                invalidate();
            }
        }

        return this;
    }

    public Value GetPadTopValue()
    {
        return _padTop;
    }

    public float GetPadTop()
    {
        return _padTop.Get( this );
    }

    public Value GetPadLeftValue()
    {
        return _padLeft;
    }

    public float GetPadLeft()
    {
        return _padLeft.Get( this );
    }

    public Value GetPadBottomValue()
    {
        return _padBottom;
    }

    public float GetPadBottom()
    {
        return _padBottom.Get( this );
    }

    public Value GetPadRightValue()
    {
        return _padRight;
    }

    public float GetPadRight()
    {
        return _padRight.Get( this );
    }

    /** Returns {@link #getPadLeft()} plus {@link #getPadRight()}. */
    public float GetPadX()
    {
        return _padLeft.Get( this ) + _padRight.Get( this );
    }

    /** Returns {@link #getPadTop()} plus {@link #getPadBottom()}. */
    public float GetPadY()
    {
        return _padTop.Get( this ) + _padBottom.Get( this );
    }

    public int GetAlign() => _alignment;

    /** Returns the row index for the y coordinate, or -1 if not over a row.
	 * @param y The y coordinate, where 0 is the top of the table. */
    public int GetRow( float y )
    {
        var n = this._cells.Count;

        if ( n == 0 ) return -1;

        y += GetPadTop();

        object[] cells = this._cells.ToArray();

        for ( int i = 0, row = 0; i < n; )
        {
            var c = ( Cell )cells[ i++ ];

            if ( ( c.ActorY + c.ComputedPadTop ) < y ) return row;
            if ( c.EndRow ) row++;
        }

        return -1;
    }

    /** Returns the height of the specified row, or 0 if the table layout has not been validated. */
    public float GetRowHeight( int rowIndex )
    {
        if ( _rowHeight == null ) return 0;

        return _rowHeight[ rowIndex ];
    }

    /** Returns the min height of the specified row. */
    public float GetRowMinHeight( int rowIndex )
    {
        if ( _sizeInvalid ) ComputeSize();

        return _rowMinHeight[ rowIndex ];
    }

    /** Returns the pref height of the specified row. */
    public float GetRowPrefHeight( int rowIndex )
    {
        if ( _sizeInvalid ) ComputeSize();

        return _rowPrefHeight[ rowIndex ];
    }

    /// <summary>
    /// Returns the width of the specified column, or 0 if the
    /// table layout has not been validated.
    /// </summary>
    public float GetColumnWidth( int columnIndex )
    {
        if ( _columnWidth == null ) return 0;

        return _columnWidth[ columnIndex ];
    }

    /// <summary>
    /// Returns the min height of the specified column.
    /// </summary>
    public float GetColumnMinWidth( int columnIndex )
    {
        if ( _sizeInvalid ) ComputeSize();

        return _columnMinWidth[ columnIndex ];
    }

    /// <summary>
    /// Returns the pref height of the specified column.
    /// </summary>
    public float GetColumnPrefWidth( int columnIndex )
    {
        if ( _sizeInvalid ) ComputeSize();

        return _columnPrefWidth[ columnIndex ];
    }

    private float[] EnsureSize( float[]? array, int size )
    {
        if ( ( array == null ) || ( array.Length < size ) )
        {
            return new float[ size ];
        }

        Array.Fill( array, 0, size, 0 );

        return array;
    }

    private void ComputeSize()
    {
        _sizeInvalid = false;

        object[] cells     = this._cells.ToArray();
        var      cellCount = this._cells.Count;

        // Implicitly end the row for layout purposes.
        if ( ( cellCount > 0 ) && !( ( Cell )cells[ cellCount - 1 ] ).EndRow )
        {
            EndRow();

            _implicitEndRow = true;
        }

        var columns         = this.Columns;
        var rows            = this.Rows;
        var columnMinWidth  = this._columnMinWidth = EnsureSize( this._columnMinWidth, columns );
        var rowMinHeight    = this._rowMinHeight = EnsureSize( this._rowMinHeight, rows );
        var columnPrefWidth = this._columnPrefWidth = EnsureSize( this._columnPrefWidth, columns );
        var rowPrefHeight   = this._rowPrefHeight = EnsureSize( this._rowPrefHeight, rows );
        var columnWidth     = this._columnWidth = EnsureSize( this._columnWidth, columns );
        var rowHeight       = this._rowHeight = EnsureSize( this._rowHeight, rows );
        var expandWidth     = this._expandWidth = EnsureSize( this._expandWidth, columns );
        var expandHeight    = this._expandHeight = EnsureSize( this._expandHeight, rows );

        float spaceRightLast = 0;

        for ( var i = 0; i < cellCount; i++ )
        {
            var   c      = ( Cell )cells[ i ];
            int   column = c.column, row = c.row, colspan = c.colspan;
            Actor a      = c.Actor;

            // Collect rows that expand and colspan=1 columns that expand.
            if ( ( c.expandY != 0 ) && ( expandHeight[ row ] == 0 ) ) expandHeight[ row ] = c.expandY;

            if ( ( colspan == 1 ) && ( c.expandX != 0 ) && ( expandWidth[ column ] == 0 ) )
                expandWidth[ column ] = c.expandX;

            // Compute combined padding/spacing for cells.
            // Spacing between actors isn't additive, the larger is used. Also, no spacing around edges.
            c.computedPadLeft = c.padLeft.get
                                    ( a )
                                + ( column == 0 ? 0 : Math.Max( 0, c.spaceLeft.get( a ) - spaceRightLast ) );

            c.computedPadTop = c.padTop.get( a );

            if ( c.cellAboveIndex != -1 )
            {
                var above = ( Cell )cells[ c.cellAboveIndex ];
                c.computedPadTop += Math.Max( 0, c.spaceTop.get( a ) - above.spaceBottom.get( a ) );
            }

            float spaceRight = c.spaceRight.get( a );
            c.computedPadRight  = c.padRight.get( a ) + ( ( column + colspan ) == columns ? 0 : spaceRight );
            c.computedPadBottom = c.padBottom.get( a ) + ( row == ( rows - 1 ) ? 0 : c.spaceBottom.get( a ) );
            spaceRightLast      = spaceRight;

            // Determine minimum and preferred cell sizes.
            float prefWidth = c.prefWidth.get( a ), prefHeight = c.prefHeight.get( a );
            float minWidth = c.minWidth.get( a ), minHeight = c.minHeight.get( a );
            float maxWidth = c.maxWidth.get( a ), maxHeight = c.maxHeight.get( a );
            if ( prefWidth < minWidth ) prefWidth = minWidth;
            if ( prefHeight < minHeight ) prefHeight = minHeight;
            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) ) prefWidth = maxWidth;
            if ( ( maxHeight > 0 ) && ( prefHeight > maxHeight ) ) prefHeight = maxHeight;

            if ( round )
            {
                minWidth   = ( float )Math.ceil( minWidth );
                minHeight  = ( float )Math.ceil( minHeight );
                prefWidth  = ( float )Math.ceil( prefWidth );
                prefHeight = ( float )Math.ceil( prefHeight );
            }

            if ( colspan == 1 )
            {
                // Spanned column min and pref width is added later.
                float hpadding = c.computedPadLeft + c.computedPadRight;
                columnPrefWidth[ column ] = Math.Max( columnPrefWidth[ column ], prefWidth + hpadding );
                columnMinWidth[ column ]  = Math.Max( columnMinWidth[ column ], minWidth + hpadding );
            }

            float vpadding = c.computedPadTop + c.computedPadBottom;
            rowPrefHeight[ row ] = Math.Max( rowPrefHeight[ row ], prefHeight + vpadding );
            rowMinHeight[ row ]  = Math.Max( rowMinHeight[ row ], minHeight + vpadding );
        }

        float uniformMinWidth  = 0, uniformMinHeight  = 0;
        float uniformPrefWidth = 0, uniformPrefHeight = 0;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c      = ( Cell )cells[ i ];
            int column = c.column;

            // Colspan with expand will expand all spanned columns if none of the spanned columns have expand.
            int expandX = c.expandX;
            outer:

            if ( expandX != 0 )
            {
                var nn = column + c.colspan;

                for ( var ii = column; ii < nn; ii++ )
                    if ( expandWidth[ ii ] != 0 )
                        break

                outer;

                for ( var ii = column; ii < nn; ii++ )
                    expandWidth[ ii ] = expandX;
            }

            // Collect uniform sizes.
            if ( ( c.uniformX == Boolean.TRUE ) && ( c.colspan == 1 ) )
            {
                float hpadding = c.computedPadLeft + c.computedPadRight;
                uniformMinWidth  = Math.Max( uniformMinWidth, columnMinWidth[ column ] - hpadding );
                uniformPrefWidth = Math.Max( uniformPrefWidth, columnPrefWidth[ column ] - hpadding );
            }

            if ( c.uniformY == Boolean.TRUE )
            {
                float vpadding = c.computedPadTop + c.computedPadBottom;
                uniformMinHeight  = Math.Max( uniformMinHeight, rowMinHeight[ c.row ] - vpadding );
                uniformPrefHeight = Math.Max( uniformPrefHeight, rowPrefHeight[ c.row ] - vpadding );
            }
        }

        // Size uniform cells to the same width/height.
        if ( ( uniformPrefWidth > 0 ) || ( uniformPrefHeight > 0 ) )
        {
            for ( var i = 0; i < cellCount; i++ )
            {
                var c = ( Cell )cells[ i ];

                if ( ( uniformPrefWidth > 0 ) && ( c.uniformX == Boolean.TRUE ) && ( c.colspan == 1 ) )
                {
                    float hpadding = c.computedPadLeft + c.computedPadRight;
                    columnMinWidth[ c.column ]  = uniformMinWidth + hpadding;
                    columnPrefWidth[ c.column ] = uniformPrefWidth + hpadding;
                }

                if ( ( uniformPrefHeight > 0 ) && ( c.uniformY == Boolean.TRUE ) )
                {
                    float vpadding = c.computedPadTop + c.computedPadBottom;
                    rowMinHeight[ c.row ]  = uniformMinHeight + vpadding;
                    rowPrefHeight[ c.row ] = uniformPrefHeight + vpadding;
                }
            }
        }

        // Distribute any additional min and pref width added by colspanned cells to the columns spanned.
        for ( var i = 0; i < cellCount; i++ )
        {
            var c       = ( Cell )cells[ i ];
            int colspan = c.colspan;

            if ( colspan == 1 ) continue;
            int column = c.column;

            Actor a = c.Actor;
            float minWidth = c.minWidth.get( a ), prefWidth = c.prefWidth.get( a ), maxWidth = c.maxWidth.get( a );
            if ( prefWidth < minWidth ) prefWidth = minWidth;
            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) ) prefWidth = maxWidth;

            if ( round )
            {
                minWidth  = ( float )Math.ceil( minWidth );
                prefWidth = ( float )Math.ceil( prefWidth );
            }

            float spannedMinWidth  = -( c.computedPadLeft + c.computedPadRight ), spannedPrefWidth = spannedMinWidth;
            float totalExpandWidth = 0;

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
            {
                spannedMinWidth  += columnMinWidth[ ii ];
                spannedPrefWidth += columnPrefWidth[ ii ];

                totalExpandWidth +=
                    expandWidth[ ii ]; // Distribute extra space using expand, if any columns have expand.
            }

            float extraMinWidth  = Math.Max( 0, minWidth - spannedMinWidth );
            float extraPrefWidth = Math.Max( 0, prefWidth - spannedPrefWidth );

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
            {
                var ratio = totalExpandWidth == 0 ? 1f / colspan : expandWidth[ ii ] / totalExpandWidth;
                columnMinWidth[ ii ]  += extraMinWidth * ratio;
                columnPrefWidth[ ii ] += extraPrefWidth * ratio;
            }
        }

        // Determine table min and pref size.
        float hpadding = padLeft.get( this ) + padRight.get( this );
        float vpadding = padTop.get( this ) + padBottom.get( this );
        tableMinWidth  = hpadding;
        tablePrefWidth = hpadding;

        for ( var i = 0; i < columns; i++ )
        {
            tableMinWidth  += columnMinWidth[ i ];
            tablePrefWidth += columnPrefWidth[ i ];
        }

        tableMinHeight  = vpadding;
        tablePrefHeight = vpadding;

        for ( var i = 0; i < rows; i++ )
        {
            tableMinHeight  += rowMinHeight[ i ];
            tablePrefHeight += Math.Max( rowMinHeight[ i ], rowPrefHeight[ i ] );
        }

        tablePrefWidth  = Math.Max( tableMinWidth, tablePrefWidth );
        tablePrefHeight = Math.Max( tableMinHeight, tablePrefHeight );
    }

    /// <summary>
    /// Positions and sizes children of the table using the cell associated
    /// with each child. The values given are the position within the parent
    /// and size of the table.
    /// </summary>
    public new void Layout()
    {
        if ( _sizeInvalid ) ComputeSize();

        var layoutWidth  = Width;
        var layoutHeight = Height;
        var columns      = this.Columns;
        var rows         = this.Rows;
        var columnWidth  = this._columnWidth;
        var rowHeight    = this._rowHeight;
        var padLeft      = this._padLeft.Get( this );
        var hpadding     = padLeft + _padRight.Get( this );
        var padTop       = this._padTop.Get( this );
        var vpadding     = padTop + _padBottom.Get( this );

        // Size columns and rows between min and pref size using (preferred - min)
        // size to weight distribution of extra space.
        float[] columnWeightedWidth;
        var     totalGrowWidth = _tablePrefWidth - _tableMinWidth;

        if ( totalGrowWidth == 0 )
        {
            columnWeightedWidth = _columnMinWidth;
        }
        else
        {
            var extraWidth = Math.Min( totalGrowWidth, Math.Max( 0, layoutWidth - _tableMinWidth ) );

            columnWeightedWidth = Table._columnWeightedWidth = EnsureSize( Table._columnWeightedWidth, columns );

            float[] columnMinWidth = this.columnMinWidth, columnPrefWidth = this.columnPrefWidth;

            for ( var i = 0; i < columns; i++ )
            {
                var growWidth = columnPrefWidth[ i ] - columnMinWidth[ i ];
                var growRatio = growWidth / totalGrowWidth;
                columnWeightedWidth[ i ] = columnMinWidth[ i ] + ( extraWidth * growRatio );
            }
        }

        float[] rowWeightedHeight;
        float   totalGrowHeight = tablePrefHeight - tableMinHeight;

        if ( totalGrowHeight == 0 )
            rowWeightedHeight = rowMinHeight;
        else
        {
            rowWeightedHeight = Table.rowWeightedHeight = EnsureSize( Table.rowWeightedHeight, rows );
            float   extraHeight = Math.Min( totalGrowHeight, Math.Max( 0, layoutHeight - tableMinHeight ) );
            float[] rowMinHeight = this.rowMinHeight, rowPrefHeight = this.rowPrefHeight;

            for ( var i = 0; i < rows; i++ )
            {
                var growHeight = rowPrefHeight[ i ] - rowMinHeight[ i ];
                var growRatio  = growHeight / totalGrowHeight;
                rowWeightedHeight[ i ] = rowMinHeight[ i ] + ( extraHeight * growRatio );
            }
        }

        // Determine actor and cell sizes (before expand or fill).
        object[] cells     = this.cells.items;
        int      cellCount = this.cells.size;

        for ( var i = 0; i < cellCount; i++ )
        {
            var   c      = ( Cell )cells[ i ];
            int   column = c.column, row = c.row;
            Actor a      = c.Actor;

            float spannedWeightedWidth = 0;
            int   colspan              = c.colspan;

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
                spannedWeightedWidth += columnWeightedWidth[ ii ];

            var weightedHeight = rowWeightedHeight[ row ];

            var prefWidth  = c.PrefWidth?.Get( a );
            var prefHeight = c.PrefHeight?.Get( a );
            var minWidth   = c.MinWidth?.Get( a );
            var minHeight  = c.MinHeight?.Get( a );
            var maxWidth   = c.MaxWidth?.Get( a );
            var maxHeight  = c.MaxHeight?.Get( a );

            if ( prefWidth < minWidth ) prefWidth                             = minWidth;
            if ( prefHeight < minHeight ) prefHeight                          = minHeight;
            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) ) prefWidth     = maxWidth;
            if ( ( maxHeight > 0 ) && ( prefHeight > maxHeight ) ) prefHeight = maxHeight;

            c.ActorWidth  = Math.Min( spannedWeightedWidth - c.ComputedPadLeft - c.ComputedPadRight, prefWidth );
            c.ActorHeight = Math.Min( weightedHeight - c.ComputedPadTop - c.ComputedPadBottom, prefHeight );

            if ( colspan == 1 )
            {
                _columnWidth[ column ] = Math.Max( _columnWidth[ column ], spannedWeightedWidth );
            }

            rowHeight[ row ] = Math.Max( rowHeight[ row ], weightedHeight );
        }

        // Distribute remaining space to any expanding columns/rows.
        var   expandWidth  = this._expandWidth;
        var   expandHeight = this._expandHeight;
        float totalExpand  = 0;

        for ( var i = 0; i < columns; i++ )
        {
            totalExpand += expandWidth[ i ];
        }

        if ( totalExpand > 0 )
        {
            var extra = layoutWidth - hpadding;

            for ( var i = 0; i < columns; i++ )
            {
                extra -= _columnWidth[ i ];
            }

            if ( extra > 0 )
            {
                // layoutWidth < tableMinWidth.
                var used      = 0.0f;
                var lastIndex = 0;

                for ( var i = 0; i < columns; i++ )
                {
                    if ( expandWidth[ i ] == 0 ) continue;

                    var amount = ( extra * expandWidth[ i ] ) / totalExpand;

                    _columnWidth[ i ] += amount;

                    used      += amount;
                    lastIndex =  i;
                }

                _columnWidth[ lastIndex ] += extra - used;
            }
        }

        totalExpand = 0;

        for ( var i = 0; i < rows; i++ )
            totalExpand += expandHeight[ i ];

        if ( totalExpand > 0 )
        {
            var extra = layoutHeight - vpadding;

            for ( var i = 0; i < rows; i++ )
                extra -= rowHeight[ i ];

            if ( extra > 0 )
            {
                // layoutHeight < tableMinHeight.
                float used      = 0;
                var   lastIndex = 0;

                for ( var i = 0; i < rows; i++ )
                {
                    if ( expandHeight[ i ] == 0 ) continue;
                    var amount = ( extra * expandHeight[ i ] ) / totalExpand;
                    rowHeight[ i ] += amount;
                    used           += amount;
                    lastIndex      =  i;
                }

                rowHeight[ lastIndex ] += extra - used;
            }
        }

        // Distribute any additional width added by colspanned cells to the columns spanned.
        for ( var i = 0; i < cellCount; i++ )
        {
            var c       = ( Cell )cells[ i ];
            int colspan = c.colspan;

            if ( colspan == 1 ) continue;

            float extraWidth = 0;

            for ( int column = c.column, nn = column + colspan; column < nn; column++ )
            {
                extraWidth += columnWeightedWidth[ column ] - columnWidth[ column ];
            }

            extraWidth -= Math.Max( 0, c.computedPadLeft + c.computedPadRight );

            extraWidth /= colspan;

            if ( extraWidth > 0 )
            {
                for ( int column = c.column, nn = column + colspan; column < nn; column++ )
                {
                    columnWidth[ column ] += extraWidth;
                }
            }
        }

        // Determine table size.
        float tableWidth = hpadding, tableHeight = vpadding;

        for ( var i = 0; i < columns; i++ )
        {
            tableWidth += columnWidth[ i ];
        }

        for ( var i = 0; i < rows; i++ )
        {
            tableHeight += rowHeight[ i ];
        }

        // Position table within the container.
        int align = this._alignment;
        var x     = padLeft;

        if ( ( align & LibGDXSharp.Utils.Align.Right ) != 0 )
            x += layoutWidth - tableWidth;
        else if ( ( align & LibGDXSharp.Utils.Align.Left ) == 0 ) // Center
            x += ( layoutWidth - tableWidth ) / 2;

        var y = padTop;

        if ( ( align & LibGDXSharp.Utils.Align.Bottom ) != 0 )
            y += layoutHeight - tableHeight;
        else if ( ( align & LibGDXSharp.Utils.Align.Top ) == 0 ) // Center
            y += ( layoutHeight - tableHeight ) / 2;

        // Size and position actors within cells.
        float currentX = x, currentY = y;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c = ( Cell )cells[ i ];

            float spannedCellWidth = 0;

            for ( int column = c.column, nn = column + c.colspan; column < nn; column++ )
                spannedCellWidth += columnWidth[ column ];

            spannedCellWidth -= c.computedPadLeft + c.computedPadRight;

            currentX += c.computedPadLeft;

            float fillX = c.fillX, fillY = c.fillY;

            if ( fillX > 0 )
            {
                c.ActorWidth = Math.Max( spannedCellWidth * fillX, c.minWidth.get( c.Actor ) );
                float maxWidth                   = c.maxWidth.get( c.Actor );
                if ( maxWidth > 0 ) c.ActorWidth = Math.Min( c.ActorWidth, maxWidth );
            }

            if ( fillY > 0 )
            {
                c.ActorHeight = Math.Max
                    (
                     ( rowHeight[ c.row ] * fillY ) - c.computedPadTop - c.computedPadBottom,
                     c.minHeight.get( c.Actor )
                    );

                float maxHeight                    = c.maxHeight.get( c.Actor );
                if ( maxHeight > 0 ) c.ActorHeight = Math.Min( c.ActorHeight, maxHeight );
            }

            align = c.align;

            if ( ( align & LibGDXSharp.Utils.Align.Left ) != 0 )
                c.ActorX = currentX;
            else if ( ( align & LibGDXSharp.Utils.Align.Right ) != 0 )
                c.ActorX = ( currentX + spannedCellWidth ) - c.ActorWidth;
            else
                c.ActorX = currentX + ( ( spannedCellWidth - c.ActorWidth ) / 2 );

            if ( ( align & LibGDXSharp.Utils.Align.Top ) != 0 )
            {
                c.ActorY = c.computedPadTop;
            }
            else if ( ( align & LibGDXSharp.Utils.Align.Bottom ) != 0 )
            {
                c.ActorY = rowHeight[ c.row ] - c.ActorHeight - c.computedPadBottom;
            }
            else
            {
                c.ActorY = ( ( ( rowHeight[ c.row ] - c.ActorHeight ) + c.computedPadTop ) - c.computedPadBottom ) / 2;
            }

            c.ActorY = layoutHeight - currentY - c.ActorY - c.ActorHeight;

            if ( _round )
            {
                c.ActorWidth  = ( float )Math.Ceiling( c.ActorWidth );
                c.ActorHeight = ( float )Math.Ceiling( c.ActorHeight );
                c.ActorX      = ( float )Math.Floor( c.ActorX );
                c.ActorY      = ( float )Math.Floor( c.ActorY );
            }

            c.Actor?.SetBounds( c.ActorX, c.ActorY, c.ActorWidth, c.ActorHeight );

            if ( c.EndRow )
            {
                currentX =  x;
                currentY += rowHeight[ c.Row ];
            }
            else
            {
                currentX += spannedCellWidth + c.ComputedPadRight;
            }
        }

        // Validate all children (some may not be in cells).
        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            if ( Children.Get( i ) is ILayout )
            {
                ( ( ILayout )Children.Get( i ) ).Validate();
            }
        }

        // Store debug rectangles.
        if ( TableDebug != DebugType.None )
        {
            AddDebugRects( x, y, tableWidth - hpadding, tableHeight - vpadding );
        }
    }

    // ------------------------------------------------------------------------

    #region Debugging

    private void AddDebugRects( float currentX, float currentY, float width, float height )
    {
        ClearDebugRects();

        if ( TableDebug is DebugType.Table or DebugType.All )
        {
            AddDebugRect( 0, 0, Width, Height, DebugTableColor );
            AddDebugRect( currentX, currentY, width, height, DebugTableColor );
        }

        var x = currentX;

        for ( int i = 0, n = _cells.Count; i < n; i++ )
        {
            Cell c = _cells[ i ];

            // Actor bounds.
            if ( ( TableDebug == DebugType.Actor ) || ( TableDebug == DebugType.All ) )
            {
                AddDebugRect( c.ActorX, c.ActorY, c.ActorWidth, c.ActorHeight, DebugActorColor );
            }

            // Cell bounds.
            float spannedCellWidth = 0;

            for ( int column = c.Column, nn = column + ( int )c.Colspan!; column < nn; column++ )
            {
                spannedCellWidth += _columnWidth![ column ];
            }

            spannedCellWidth -= c.ComputedPadLeft + c.ComputedPadRight;
            currentX         += c.ComputedPadLeft;

            if ( TableDebug is DebugType.Cell or DebugType.All )
            {
                AddDebugRect
                    (
                     currentX,
                     currentY + c.ComputedPadTop,
                     spannedCellWidth,
                     _rowHeight![ c.Row ] - c.ComputedPadTop - c.ComputedPadBottom,
                     DebugCellColor
                    );
            }

            if ( c.EndRow )
            {
                currentX =  x;
                currentY += _rowHeight![ c.Row ];
            }
            else
            {
                currentX += ( spannedCellWidth + c.ComputedPadRight );
            }
        }
    }

    private void ClearDebugRects()
    {
        _debugRects ??= new List< DebugRect >();

        DebugRect.Pool.FreeAll( _debugRects );

        _debugRects.Clear();
    }

    private void AddDebugRect( float x, float y, float w, float h, Color color )
    {
        DebugRect rect = DebugRect.Pool.Obtain();

        rect.Color = color;
        rect.Set( x, y, w, h );

        _debugRects?.Add( rect );
    }

    public new void DrawDebug( ShapeRenderer shapes )
    {
        if ( Transform )
        {
            ApplyTransform( shapes, ComputeTransform() );
            DrawDebugRects( shapes );

            if ( _clip )
            {
                shapes.Flush();

                var x      = 0.0f;
                var y      = 0.0f;
                var width  = Width;
                var height = Height;

                if ( _background != null )
                {
                    x      =  _padLeft.Get( this );
                    y      =  _padBottom.Get( this );
                    width  -= x + _padRight.Get( this );
                    height -= y + _padTop.Get( this );
                }

                if ( ClipBegin( x, y, width, height ) )
                {
                    DrawDebugChildren( shapes );
                    ClipEnd();
                }
            }
            else
            {
                DrawDebugChildren( shapes );
            }

            ResetTransform( shapes );
        }
        else
        {
            DrawDebugRects( shapes );
            base.DrawDebug( shapes );
        }
    }

    protected new void DrawDebugBounds( ShapeRenderer shapes )
    {
    }

    private void DrawDebugRects( ShapeRenderer shapes )
    {
        if ( ( _debugRects == null ) || !DebugFlag ) return;

        shapes.Set( ShapeRenderer.ShapeTypes.Line );

        if ( Stage != null ) shapes.Color = Stage.DebugColor;

        float x = 0;
        float y = 0;

        if ( !Transform )
        {
            x = X;
            y = Y;
        }

        for ( int i = 0, n = _debugRects.Count; i < n; i++ )
        {
            DebugRect debugRect = _debugRects[ i ];

            shapes.Color = debugRect.Color;
            shapes.Rect( x + debugRect.X, y + debugRect.Y, debugRect.Width, debugRect.Height );
        }
    }

    #endregion Debugging

    // ------------------------------------------------------------------------

    #region Properties

    /// <value>
    /// The skin that was passed to this table in its constructor, or null if none was given.
    /// </value>
    public Skin? Skin { get; set; }

    /// <summary>
    /// Causes the contents to be clipped if they exceed the table's bounds.
    /// Enabling clipping sets <see cref="Group.Transform"/> to true.
    /// </summary>
    public bool Clip
    {
        get => _clip;
        set
        {
            _clip     = value;
            Transform = value;
            Invalidate();
        }
    }

    public DebugType TableDebug { get; set; } = DebugType.None;

    /// <summary>
    /// If true (the default), positions and sizes of child actors are
    /// rounded and ceiled to the nearest integer value.
    /// </summary>
    public bool Round { get; set; }

    public int Rows { get; private set; }

    public int Columns { get; set; }

    #endregion Properties
    
}