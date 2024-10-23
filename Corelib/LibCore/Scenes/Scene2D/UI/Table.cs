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

using Corelib.LibCore.Assets;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;
using Corelib.LibCore.Utils.Pooling;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

[PublicAPI]
public class Table : WidgetGroup
{
    public enum DebugType
    {
        None,
        All,
        Table,
        Cell,
        Actor,
    }

    // ------------------------------------------------------------------------

    public static readonly Color DebugTableColor = new( 0, 0, 1, 1 );
    public static readonly Color DebugCellColor  = new( 1, 0, 0, 1 );
    public static readonly Color DebugActorColor = new( 0, 1, 0, 1 );

    // ------------------------------------------------------------------------

    private static float[]? _columnWeightedWidth;
    private static float[]? _rowWeightedHeight;

    private readonly List< Cell > _cells          = new( 4 );
    private readonly List< Cell > _columnDefaults = new( 2 );
    private readonly float[]      _columnWidth;
    private readonly float[]      _rowHeight;

    private int _alignment = Align.CENTER;

    private IDrawable?         _background;
    private bool               _clip;
    private float[]            _columnMinWidth;
    private float[]            _columnPrefWidth;
    private List< DebugRect >? _debugRects;
    private float[]            _expandHeight;
    private float[]            _expandWidth;
    private bool               _implicitEndRow = false;

    private Value _padBottom = BackgroundBottom;
    private Value _padLeft   = BackgroundLeft;
    private Value _padRight  = BackgroundRight;
    private Value _padTop    = BackgroundTop;

    private Cell?   _rowDefaults;
    private float[] _rowMinHeight;
    private float[] _rowPrefHeight;
    private bool    _sizeInvalid = true;
    private float   _tableMinHeight;
    private float   _tableMinWidth;
    private float   _tablePrefHeight;
    private float   _tablePrefWidth;

    // ------------------------------------------------------------------------

    public Table() : this( null )
    {
    }

    /// <summary>
    /// Creates a table with a skin, which is required to use <see cref="AssetDescriptor"/>
    /// </summary>
    public Table( Skin? skin )
    {
        _columnWidth     = default( float[]? )!;
        _columnMinWidth  = default( float[]? )!;
        _columnPrefWidth = default( float[]? )!;
        _rowHeight       = default( float[]? )!;
        _rowMinHeight    = default( float[]? )!;
        _rowPrefHeight   = default( float[]? )!;
        _expandWidth     = default( float[]? )!;
        _expandHeight    = default( float[]? )!;

        Skin               = skin;
        CellDefaults       = ObtainCell();
        Transform          = false;
        Touchable          = Touchable.ChildrenOnly;
        CellPool.NewObject = GetNewObject;
    }

    public Pool< Cell > CellPool     { get; } = new();
    public Cell         CellDefaults { get; set; }

    // ------------------------------------------------------------------------

    private Cell ObtainCell()
    {
        var cell = CellPool.Obtain();
        cell!.Table = this;

        return cell;
    }

    public override void Draw( IBatch batch, float parentAlpha )
    {
        Validate();

        if ( Transform )
        {
            ApplyTransform( batch, ComputeTransform() );
            DrawBackground( batch, parentAlpha, 0, 0 );

            if ( _clip )
            {
                batch.Flush();

                var padLeft   = _padLeft.Get( this );
                var padBottom = _padBottom.Get( this );

                if ( ClipBegin( padLeft,
                                padBottom,
                                Width - padLeft - _padRight.Get( this ),
                                Height - padBottom - _padTop.Get( this ) ) )
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
            DrawBackground( batch, parentAlpha, GetX( _alignment ), GetY( _alignment ) );
            base.Draw( batch, parentAlpha );
        }
    }

    /// <summary>
    /// Called to draw the background, before clipping is applied (if enabled).
    /// Default implementation draws the background drawable.
    /// </summary>
    protected virtual void DrawBackground( IBatch batch, float parentAlpha, float x, float y )
    {
        if ( _background == null )
        {
            return;
        }

        batch.SetColor( Color.R, Color.G, Color.B, Color.A * parentAlpha );

        _background.Draw( batch, x, y, Width, Height );
    }

    /// <summary>
    /// Sets the background drawable from the skin and adjusts the table's padding
    /// to match the background. This may only be called if a skin has been set
    /// with <see cref="Table"/> or <see cref="Skin"/>.
    /// </summary>
    protected void SetBackground( string drawableName )
    {
        if ( Skin == null )
        {
            throw new GdxRuntimeException( "Table must have a skin set to use this method." );
        }

        SetBackground( Skin.GetDrawable( drawableName ) );
    }

    /// <summary>
    ///     <param name="background"> May be null to clear the background. </param>
    /// </summary>
    protected void SetBackground( IDrawable? background )
    {
        if ( _background == background )
        {
            return;
        }

        var padTopOld    = GetPadTop();
        var padLeftOld   = GetPadLeft();
        var padBottomOld = GetPadBottom();
        var padRightOld  = GetPadRight();

        _background = background; // The default pad values use the background's padding.

        var padTopNew    = GetPadTop();
        var padLeftNew   = GetPadLeft();
        var padBottomNew = GetPadBottom();
        var padRightNew  = GetPadRight();

        if ( !( padTopOld + padBottomOld ).Equals( padTopNew + padBottomNew )
          || !( padLeftOld + padRightOld ).Equals( padLeftNew + padRightNew ) )
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

    /// <summary>
    /// Sets the table background to the supplied IDrawable.
    /// </summary>
    /// <param name="background"></param>
    /// <returns></returns>
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

    public override Actor? Hit( float x, float y, bool touchable )
    {
        if ( _clip )
        {
            if ( touchable && ( Touchable == Touchable.Disabled ) )
            {
                return null;
            }

            if ( ( x < 0 ) || ( x >= Width ) || ( y < 0 ) || ( y >= Height ) )
            {
                return null;
            }
        }

        return base.Hit( x, y, touchable );
    }

    public Table SetClip( bool enabled = true )
    {
        Clip = enabled;

        return this;
    }

    public override void Invalidate()
    {
        _sizeInvalid = true;
        base.Invalidate();
    }

    /// <summary>
    /// Adds a cell without an actor.
    /// </summary>
    public Cell AddWithoutActor()
    {
        return Add< Actor >( null! );
    }

    /// <summary>
    /// Adds a new cell to the table with the specified actor.
    /// </summary>
    public Cell Add< T >( T? actor ) where T : Actor
    {
        var cell = ObtainCell();
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
            var lastCell = _cells.Peek();

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
                // TODO: This label is direct from the Java libgdx.
                //       I don't like using them so this will be refactored at some point.
                //       I'm not even sure it's in the right place...should it be AFTER the loop?
            outer:

                for ( var i = cellCount - 1; i >= 0; i-- )
                {
                    var other = _cells[ i ];

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

        cell.Set( CellDefaults );

        if ( cell.Column < _columnDefaults.Count )
        {
            cell.Merge( _columnDefaults[ cell.Column ] );
        }

        cell.Merge( _rowDefaults );

        if ( actor != null )
        {
            AddActor( actor );
        }

        return cell;
    }

    public Table Add( params Actor[] actors )
    {
        for ( int i = 0, n = actors.Length; i < n; i++ )
        {
            Add< Actor >( actors[ i ] );
        }

        return this;
    }

    /// <summary>
    /// Adds a new cell with a label. This may only be called if a skin
    /// has been set with <see cref="Table"/> or <see cref="Skin"/>
    /// </summary>
    public Cell Add( string? text )
    {
        ArgumentNullException.ThrowIfNull( text );

        if ( Skin == null )
        {
            throw new GdxRuntimeException( "Table must have a skin set to use this method." );
        }

        return Add( new Label( text, Skin ) );
    }

    /// <summary>
    /// Adds a new cell with a label. This may only be called if a skin
    /// has been set with <see cref="Table"/> or <see cref="Skin"/>
    /// </summary>
    public Cell Add( string text, string labelStyleName )
    {
        if ( Skin == null )
        {
            throw new GdxRuntimeException( "Table must have a skin set to use this method." );
        }

        return Add( new Label( text, Skin.Get< Label.LabelStyle >( labelStyleName ) ) );
    }

    /// <summary>
    /// Adds a new cell with a label. This may only be called if a skin
    /// has been set with <see cref="Table"/> or <see cref="Skin"/>.
    /// </summary>
    public Cell Add( string? text, string fontName, Color color )
    {
        if ( Skin == null )
        {
            throw new GdxRuntimeException( "Table must have a skin set to use this method." );
        }

        return Add( new Label( text, new Label.LabelStyle( Skin.GetFont( fontName ), color ) ) );
    }

    /// <summary>
    /// Adds a new cell with a label. This may only be called if a skin
    /// has been set with <see cref="Table"/> or <see cref="Skin"/>.
    /// </summary>
    public Cell Add( string? text, string fontName, string colorName )
    {
        if ( Skin == null )
        {
            throw new GdxRuntimeException( "Table must have a skin set to use this method." );
        }

        return Add( new Label( text, new Label.LabelStyle( Skin.GetFont( fontName ), Skin.GetColor( colorName ) ) ) );
    }

    /// <summary>
    /// Adds a new cell to the table with the specified actors in a <see cref="Stack"/>.
    /// </summary>
    /// <param name="actors"> May be null or empty to add a stack without any actors. </param>
    public Cell Stack( params Actor[]? actors )
    {
        var stack = new Stack();

        if ( actors != null )
        {
            for ( int i = 0, n = actors.Length; i < n; i++ )
            {
                stack.AddActor( actors[ i ] );
            }
        }

        return Add( stack );
    }

    public override bool RemoveActor( Actor actor, bool unfocus )
    {
        if ( !base.RemoveActor( actor, unfocus ) )
        {
            return false;
        }

        if ( GetCell( actor ) != null )
        {
            GetCell( actor )!.Actor = null;
        }

        return true;
    }

    public override Actor RemoveActorAt( int index, bool unfocus )
    {
        var actor = base.RemoveActorAt( index, unfocus );

        GetCell( actor )!.Actor = null;

        return actor;
    }

    /// <summary>
    /// Removes all actors and cells from the table.
    /// </summary>
    public override void ClearChildren()
    {
        Cell[] cells = _cells.ToArray();

        for ( var i = _cells.Count - 1; i >= 0; i-- )
        {
            cells[ i ].Actor?.Remove();
        }

        CellPool.FreeAll( _cells );

        _cells.Clear();

        Rows    = 0;
        Columns = 0;

        if ( _rowDefaults != null )
        {
            CellPool.Free( _rowDefaults );
        }

        _rowDefaults    = null;
        _implicitEndRow = false;

        base.ClearChildren();
    }

    /// <summary>
    /// Removes all actors and cells from the table (same as <see cref="ClearChildren"/>)
    /// and additionally resets all table properties and cell, column, and row defaults.
    /// </summary>
    public void Reset()
    {
        ClearChildren();

        _padTop    = BackgroundTop;
        _padLeft   = BackgroundLeft;
        _padBottom = BackgroundBottom;
        _padRight  = BackgroundRight;

        _alignment = Align.CENTER;

        DebugLines( DebugType.None );

        CellDefaults.Reset();

        for ( int i = 0, n = _columnDefaults.Count; i < n; i++ )
        {
            if ( _columnDefaults?[ i ] != null )
            {
                CellPool.Free( _columnDefaults[ i ] );
            }
        }

        _columnDefaults?.Clear();
    }

    /// <summary>
    /// Indicates that subsequent cells should be added to a new row and returns the
    /// cell values that will be used as the defaults for all cells in the new row.
    /// </summary>
    public Cell? AddRow()
    {
        if ( _cells.Count > 0 )
        {
            if ( !_implicitEndRow )
            {
                if ( _cells.Peek().EndRow )
                {
                    // Row was already ended.
                    return _rowDefaults;
                }

                EndRow();
            }

            Invalidate();
        }

        _implicitEndRow = false;

        if ( _rowDefaults != null )
        {
            CellPool.Free( _rowDefaults );
        }

        _rowDefaults = ObtainCell();
        _rowDefaults.Clear();

        return _rowDefaults;
    }

    private void EndRow()
    {
        Cell[] cells      = _cells.ToArray();
        var    rowColumns = 0;

        for ( var i = _cells.Count - 1; i >= 0; i-- )
        {
            var cell = cells[ i ];

            if ( cell.EndRow )
            {
                break;
            }

            rowColumns += cell.Colspan;
        }

        Columns = Math.Max( Columns, rowColumns );

        Rows++;

        _cells.Peek().EndRow = true;
    }

    /// <summary>
    /// Gets the cell values that will be used as the defaults for all cells in the
    /// specified column. Columns are indexed starting at 0.
    /// </summary>
    /// <param name="column"></param>
    public Cell ColumnDefaults( int column )
    {
        var cell = _columnDefaults.Count > column ? _columnDefaults[ column ] : null;

        if ( cell == null )
        {
            cell = ObtainCell();
            cell.Clear();

            if ( column >= _columnDefaults.Count )
            {
                for ( var i = _columnDefaults.Count; i < column; i++ )
                {
                    _columnDefaults.Add( null! );
                }

                _columnDefaults.Add( cell );
            }
            else
            {
                _columnDefaults[ column ] = cell;
            }
        }

        return cell;
    }

    /// <summary>
    /// Returns the cell for the specified actor in this table, or null.
    /// </summary>
    public Cell? GetCell< T >( T actor ) where T : Actor
    {
        if ( actor == null )
        {
            throw new ArgumentException( "actor cannot be null." );
        }

        Cell[] cells = _cells.ToArray();

        for ( int i = 0, n = _cells.Count; i < n; i++ )
        {
            var c = cells[ i ];

            if ( c.Actor == actor )
            {
                return c;
            }
        }

        return null;
    }

    /// <summary>
    /// Returns the cells for this table.
    /// </summary>
    public List< Cell > GetCells()
    {
        return _cells;
    }

    protected virtual float GetPrefWidth()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        var width = _tablePrefWidth;

        return _background != null ? Math.Max( width, _background.MinWidth ) : width;
    }

    protected virtual float GetPrefHeight()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        var height = _tablePrefHeight;

        return _background != null ? Math.Max( height, _background.MinHeight ) : height;
    }

    public float GetMinWidth()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _tableMinWidth;
    }

    public float GetMinHeight()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _tableMinHeight;
    }

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight around the
    /// table to the specified value.
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
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
        _padTop      = top ?? throw new ArgumentException( "top cannot be null." );
        _padLeft     = left ?? throw new ArgumentException( "left cannot be null." );
        _padBottom   = bottom ?? throw new ArgumentException( "bottom cannot be null." );
        _padRight    = right ?? throw new ArgumentException( "right cannot be null." );
        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the top edge of the table. */
    /// </summary>
    /// <param name="padTop"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Table PadTop( Value padTop )
    {
        _padTop = padTop ?? throw new ArgumentException( "padTop cannot be null." );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the left edge of the table. */
    /// </summary>
    /// <param name="padLeft"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Table PadLeft( Value padLeft )
    {
        _padLeft = padLeft ?? throw new ArgumentException( "padLeft cannot be null." );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the bottom edge of the table. */
    /// </summary>
    /// <param name="padBottom"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Table PadBottom( Value padBottom )
    {
        _padBottom = padBottom ?? throw new ArgumentException( "padBottom cannot be null." );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the right edge of the table. */
    /// </summary>
    /// <param name="padRight"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Table PadRight( Value padRight )
    {
        _padRight = padRight ?? throw new ArgumentException( "padRight cannot be null." );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight around the table to the specified value.
    /// </summary>
    /// <param name="pad"></param>
    /// <returns></returns>
    public Table Pad( float pad )
    {
        Pad( Value.Fixed.ValueOf( pad ) );

        return this;
    }

    public Table Pad( float top, float left, float bottom, float right )
    {
        _padTop      = Value.Fixed.ValueOf( top );
        _padLeft     = Value.Fixed.ValueOf( left );
        _padBottom   = Value.Fixed.ValueOf( bottom );
        _padRight    = Value.Fixed.ValueOf( right );
        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the top edge of the table. */
    /// </summary>
    /// <param name="padTop"></param>
    /// <returns></returns>
    public Table PadTop( float padTop )
    {
        _padTop = Value.Fixed.ValueOf( padTop );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the left edge of the table. */
    /// </summary>
    /// <param name="padLeft"></param>
    /// <returns></returns>
    public Table PadLeft( float padLeft )
    {
        _padLeft = Value.Fixed.ValueOf( padLeft );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the bottom edge of the table. */
    /// </summary>
    /// <param name="padBottom"></param>
    /// <returns></returns>
    public Table PadBottom( float padBottom )
    {
        _padBottom = Value.Fixed.ValueOf( padBottom );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Padding at the right edge of the table. */
    /// </summary>
    /// <param name="padRight"></param>
    /// <returns></returns>
    public Table PadRight( float padRight )
    {
        _padRight = Value.Fixed.ValueOf( padRight );

        _sizeInvalid = true;

        return this;
    }

    /// <summary>
    /// Alignment of the logical table within the table actor.
    /// Set to <see cref="Align.CENTER"/>, <see cref="Align.TOP"/>, <see cref="Align.BOTTOM"/>,
    /// <see cref="Align.LEFT"/>, <see cref="Align.RIGHT"/>, or any combination of those.
    /// </summary>
    /// <param name="align"></param>
    /// <returns></returns>
    public Table SetAlignment( int align )
    {
        _alignment = align;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the logical table within the table actor to
    /// <see cref="Align.CENTER"/>. This clears any other alignment.
    /// </summary>
    /// <returns></returns>
    public Table Center()
    {
        _alignment = Align.CENTER;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.TOP"/> and clears <see cref="Align.BOTTOM"/> for
    /// the alignment of the logical table within the table actor.
    /// </summary>
    /// <returns></returns>
    public Table AddTopAlignment()
    {
        _alignment |= Align.TOP;
        _alignment &= ~Align.BOTTOM;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.LEFT"/> and clears <see cref="Align.RIGHT"/> for
    /// the alignment of the logical table within the table actor.
    /// </summary>
    /// <returns></returns>
    public Table AddLeftAlignment()
    {
        _alignment |= Align.LEFT;
        _alignment &= ~Align.RIGHT;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.BOTTOM"/> and clears <see cref="Align.TOP"/> for the
    /// alignment of the logical table within the table actor.
    /// </summary>
    public Table AddBottomAlignment()
    {
        _alignment |= Align.BOTTOM;
        _alignment &= ~Align.TOP;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.RIGHT"/> and clears <see cref="Align.LEFT"/> for
    /// the alignment of the logical table within the table actor.
    /// </summary>
    public Table AddRightAlignment()
    {
        _alignment |= Align.RIGHT;
        _alignment &= ~Align.LEFT;

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

    /// <summary>
    /// Returns <see cref="_padLeft"/> plus <see cref="_padRight"/>.
    /// </summary>
    /// <returns></returns>
    public float GetPadX()
    {
        return _padLeft.Get( this ) + _padRight.Get( this );
    }

    /// <summary>
    /// Returns <see cref="_padTop"/> plus <see cref="_padBottom"/>.
    /// </summary>
    /// <returns></returns>
    public float GetPadY()
    {
        return _padTop.Get( this ) + _padBottom.Get( this );
    }

    public int GetAlign()
    {
        return _alignment;
    }

    /// <summary>
    /// Returns the row index for the y coordinate, or -1 if not over a row.
    /// </summary>
    /// <param name="y"> The y coordinate, where 0 is the top of the table. </param>
    public int GetRow( float y )
    {
        var n = _cells.Count;

        if ( n == 0 )
        {
            return -1;
        }

        y += GetPadTop();

        Cell[] cells = _cells.ToArray();

        for ( int i = 0, row = 0; i < n; )
        {
            var c = cells[ i++ ];

            if ( ( c.ActorY + c.ComputedPadTop ) < y )
            {
                return row;
            }

            if ( c.EndRow )
            {
                row++;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the height of the specified row, or 0 if the table layout has not been validated.
    /// </summary>
    public float GetRowHeight( int rowIndex )
    {
        return _rowHeight[ rowIndex ];
    }

    /// <summary>
    /// Returns the min height of the specified row.
    /// </summary>
    /// <param name="rowIndex">The row number</param>
    public float GetRowMinHeight( int rowIndex )
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _rowMinHeight[ rowIndex ];
    }

    /// <summary>
    /// Returns the pref height of the specified row.
    /// </summary>
    /// <param name="rowIndex">The row number</param>
    public float GetRowPrefHeight( int rowIndex )
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _rowPrefHeight[ rowIndex ];
    }

    /// <summary>
    /// Returns the width of the specified column, or 0 if the
    /// table layout has not been validated.
    /// </summary>
    /// <param name="columnIndex">The column number</param>
    public float GetColumnWidth( int columnIndex )
    {
        return _columnWidth[ columnIndex ];
    }

    /// <summary>
    /// Returns the min height of the specified column.
    /// </summary>
    /// <param name="columnIndex">The column number</param>
    public float GetColumnMinWidth( int columnIndex )
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        return _columnMinWidth[ columnIndex ];
    }

    /// <summary>
    /// Returns the pref height of the specified column.
    /// </summary>
    /// <param name="columnIndex">The column number</param>
    public float GetColumnPrefWidth( int columnIndex )
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

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

        Cell[] cells     = _cells.ToArray();
        var    cellCount = _cells.Count;

        // Implicitly end the row for layout purposes.
        if ( ( cellCount > 0 ) && !cells[ cellCount - 1 ].EndRow )
        {
            EndRow();

            _implicitEndRow = true;
        }

        var columns         = Columns;
        var rows            = Rows;
        var columnMinWidth  = _columnMinWidth = EnsureSize( _columnMinWidth, columns );
        var rowMinHeight    = _rowMinHeight = EnsureSize( _rowMinHeight, rows );
        var columnPrefWidth = _columnPrefWidth = EnsureSize( _columnPrefWidth, columns );
        var rowPrefHeight   = _rowPrefHeight = EnsureSize( _rowPrefHeight, rows );
        var expandWidth     = _expandWidth = EnsureSize( _expandWidth, columns );
        var expandHeight    = _expandHeight = EnsureSize( _expandHeight, rows );

        float? spaceRightLast = 0;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c = cells[ i ];

            // Collect rows that expand and colspan=1 columns that expand.
            if ( ( c.ExpandY != 0 ) && ( expandHeight[ c.Row ] == 0 ) )
            {
                expandHeight[ c.Row ] = c.ExpandY;
            }

            if ( ( c.Colspan == 1 ) && ( c.ExpandX != 0 ) && ( expandWidth[ c.Column ] == 0 ) )
            {
                expandWidth[ c.Column ] = c.ExpandX;
            }

            // Compute combined padding/spacing for cells.
            // Spacing between actors isn't additive, the larger is used. Also, no spacing around edges.
            c.ComputedPadLeft = ( float ) ( c.PadLeft?.Get( c.Actor )
                                          + ( c.Column == 0 ? 0 : Math.Max( 0f, ( float ) ( c.SpaceLeft?.Get( c.Actor ) - spaceRightLast )! ) ) )!;

            c.ComputedPadTop = c.PadTop!.Get( c.Actor );

            if ( c.CellAboveIndex != -1 )
            {
                var above = cells[ c.CellAboveIndex ];

                c.ComputedPadTop += Math.Max( 0, ( float ) ( c.SpaceTop?.Get( c.Actor ) - above.SpaceBottom?.Get( c.Actor ) )! );
            }

            var spaceRight = c.SpaceRight?.Get( c.Actor );

            c.ComputedPadRight = ( float ) ( c.PadRight?.Get( c.Actor )
                                           + ( ( c.Column + c.Colspan ) == columns ? 0f : spaceRight ) )!;

            c.ComputedPadBottom = ( float ) ( c.PadBottom?.Get( c.Actor )
                                            + ( c.Row == ( rows - 1 ) ? 0 : c.SpaceBottom?.Get( c.Actor ) ) )!;

            spaceRightLast = spaceRight ?? 0f;

            // Determine minimum and preferred cell sizes.
            var prefWidth  = c.PrefWidth?.Get( c.Actor );
            var prefHeight = c.PrefHeight?.Get( c.Actor );
            var minWidth   = c.MinWidth?.Get( c.Actor );
            var minHeight  = c.MinHeight?.Get( c.Actor );
            var maxWidth   = c.MaxWidth?.Get( c.Actor );
            var maxHeight  = c.MaxHeight?.Get( c.Actor );

            if ( prefWidth < minWidth )
            {
                prefWidth = minWidth;
            }

            if ( prefHeight < minHeight )
            {
                prefHeight = minHeight;
            }

            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) )
            {
                prefWidth = maxWidth;
            }

            if ( ( maxHeight > 0 ) && ( prefHeight > maxHeight ) )
            {
                prefHeight = maxHeight;
            }

            if ( Round )
            {
                minWidth   = ( float ) Math.Ceiling( ( float ) minWidth! );
                minHeight  = ( float ) Math.Ceiling( ( float ) minHeight! );
                prefWidth  = ( float ) Math.Ceiling( ( float ) prefWidth! );
                prefHeight = ( float ) Math.Ceiling( ( float ) prefHeight! );
            }

            if ( c.Colspan == 1 )
            {
                // Spanned column min and pref width is added later.
                var hpadding = c.ComputedPadLeft + c.ComputedPadRight;

                columnPrefWidth[ c.Column ] = Math.Max(
                                                       columnPrefWidth[ c.Column ],
                                                       ( float ) ( prefWidth + hpadding )!
                                                      );

                columnMinWidth[ c.Column ] = Math.Max(
                                                      columnMinWidth[ c.Column ],
                                                      ( float ) ( minWidth + hpadding )!
                                                     );
            }

            var vpadding = c.ComputedPadTop + c.ComputedPadBottom;

            rowPrefHeight[ c.Row ] = Math.Max( rowPrefHeight[ c.Row ], ( float ) ( prefHeight + vpadding )! );
            rowMinHeight[ c.Row ]  = Math.Max( rowMinHeight[ c.Row ], ( float ) ( minHeight + vpadding )! );
        }

        float uniformMinWidth  = 0, uniformMinHeight  = 0;
        float uniformPrefWidth = 0, uniformPrefHeight = 0;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c      = cells[ i ];
            var column = c.Column;

            // Colspan with expand will expand all spanned columns
            // if none of the spanned columns have expand.
            var expandX = c.ExpandX;

            if ( expandX != 0 )
            {
                var nn = column + c.Colspan;

                for ( var ii = column; ii < nn; ii++ )
                {
                    if ( expandWidth[ ii ] != 0 )
                    {
                        break;
                    }
                }

                for ( var ii = column; ii < nn; ii++ )
                {
                    expandWidth[ ii ] = expandX;
                }
            }

            // Collect uniform sizes.
            if ( c is { UniformX: true, Colspan: 1 } )
            {
                var hpadding = c.ComputedPadLeft + c.ComputedPadRight;

                uniformMinWidth  = Math.Max( uniformMinWidth, columnMinWidth[ column ] - hpadding );
                uniformPrefWidth = Math.Max( uniformPrefWidth, columnPrefWidth[ column ] - hpadding );
            }

            if ( c.UniformY )
            {
                var vpadding = c.ComputedPadTop + c.ComputedPadBottom;

                uniformMinHeight  = Math.Max( uniformMinHeight, rowMinHeight[ c.Row ] - vpadding );
                uniformPrefHeight = Math.Max( uniformPrefHeight, rowPrefHeight[ c.Row ] - vpadding );
            }
        }

        // Size uniform cells to the same width/height.
        if ( ( uniformPrefWidth > 0 ) || ( uniformPrefHeight > 0 ) )
        {
            for ( var i = 0; i < cellCount; i++ )
            {
                var c = cells[ i ];

                if ( ( uniformPrefWidth > 0 ) && c is { UniformX: true, Colspan: 1 } )
                {
                    var computedPadding = c.ComputedPadLeft + c.ComputedPadRight;

                    columnMinWidth[ c.Column ]  = uniformMinWidth + computedPadding;
                    columnPrefWidth[ c.Column ] = uniformPrefWidth + computedPadding;
                }

                if ( ( uniformPrefHeight > 0 ) && c.UniformY )
                {
                    var computedPadding = c.ComputedPadTop + c.ComputedPadBottom;

                    rowMinHeight[ c.Row ]  = uniformMinHeight + computedPadding;
                    rowPrefHeight[ c.Row ] = uniformPrefHeight + computedPadding;
                }
            }
        }

        // Distribute any additional min and pref width added by colspanned cells to the columns spanned.
        for ( var i = 0; i < cellCount; i++ )
        {
            var c       = cells[ i ];
            var colspan = c.Colspan;

            if ( colspan == 1 )
            {
                continue;
            }

            var column    = c.Column;
            var minWidth  = c.MinWidth?.Get( c.Actor );
            var prefWidth = c.PrefWidth?.Get( c.Actor );
            var maxWidth  = c.MaxWidth?.Get( c.Actor );

            if ( prefWidth < minWidth )
            {
                prefWidth = minWidth;
            }

            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) )
            {
                prefWidth = maxWidth;
            }

            if ( Round )
            {
                minWidth  = ( float ) Math.Ceiling( ( float ) minWidth! );
                prefWidth = ( float ) Math.Ceiling( ( float ) prefWidth! );
            }

            float spannedMinWidth  = -( c.ComputedPadLeft + c.ComputedPadRight ), spannedPrefWidth = spannedMinWidth;
            float totalExpandWidth = 0;

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
            {
                spannedMinWidth  += columnMinWidth[ ii ];
                spannedPrefWidth += columnPrefWidth[ ii ];

                // Distribute extra space using expand, if any columns have expand.
                totalExpandWidth += expandWidth[ ii ];
            }

            var extraMinWidth  = Math.Max( 0, ( float ) ( minWidth - spannedMinWidth )! );
            var extraPrefWidth = Math.Max( 0, ( float ) ( prefWidth - spannedPrefWidth )! );

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
            {
                var ratio = totalExpandWidth == 0 ? 1f / colspan : expandWidth[ ii ] / totalExpandWidth;

                columnMinWidth[ ii ]  += extraMinWidth * ratio;
                columnPrefWidth[ ii ] += extraPrefWidth * ratio;
            }
        }

        // Determine table min and pref size.
        var hpad = _padLeft.Get( this ) + _padRight.Get( this );
        var vpad = _padTop.Get( this ) + _padBottom.Get( this );

        _tableMinWidth  = hpad;
        _tablePrefWidth = hpad;

        for ( var i = 0; i < columns; i++ )
        {
            _tableMinWidth  += columnMinWidth[ i ];
            _tablePrefWidth += columnPrefWidth[ i ];
        }

        _tableMinHeight  = vpad;
        _tablePrefHeight = vpad;

        for ( var i = 0; i < rows; i++ )
        {
            _tableMinHeight  += rowMinHeight[ i ];
            _tablePrefHeight += Math.Max( rowMinHeight[ i ], rowPrefHeight[ i ] );
        }

        _tablePrefWidth  = Math.Max( _tableMinWidth, _tablePrefWidth );
        _tablePrefHeight = Math.Max( _tableMinHeight, _tablePrefHeight );
    }

    /// <summary>
    /// Positions and sizes children of the table using the cell associated
    /// with each child. The values given are the position within the parent
    /// and size of the table.
    /// </summary>
    public void Layout()
    {
        if ( _sizeInvalid )
        {
            ComputeSize();
        }

        var layoutWidth  = Width;
        var layoutHeight = Height;
        var columns      = Columns;
        var rows         = Rows;
        var columnWidth  = _columnWidth;
        var rowHeight    = _rowHeight;
        var padLeft      = _padLeft.Get( this );
        var hpadding     = padLeft + _padRight.Get( this );
        var padTop       = _padTop.Get( this );
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

            columnWeightedWidth = _columnWeightedWidth = EnsureSize( _columnWeightedWidth, columns );

            var columnMinWidth  = _columnMinWidth;
            var columnPrefWidth = _columnPrefWidth;

            for ( var i = 0; i < columns; i++ )
            {
                var growWidth = columnPrefWidth[ i ] - columnMinWidth[ i ];
                var growRatio = growWidth / totalGrowWidth;
                columnWeightedWidth[ i ] = columnMinWidth[ i ] + ( extraWidth * growRatio );
            }
        }

        float[] rowWeightedHeight;
        var     totalGrowHeight = _tablePrefHeight - _tableMinHeight;

        if ( totalGrowHeight == 0 )
        {
            rowWeightedHeight = _rowMinHeight;
        }
        else
        {
            rowWeightedHeight = _rowWeightedHeight = EnsureSize( _rowWeightedHeight, rows );

            var extraHeight   = Math.Min( totalGrowHeight, Math.Max( 0, layoutHeight - _tableMinHeight ) );
            var rowMinHeight  = _rowMinHeight;
            var rowPrefHeight = _rowPrefHeight;

            for ( var i = 0; i < rows; i++ )
            {
                var growHeight = rowPrefHeight[ i ] - rowMinHeight[ i ];
                var growRatio  = growHeight / totalGrowHeight;
                rowWeightedHeight[ i ] = rowMinHeight[ i ] + ( extraHeight * growRatio );
            }
        }

        // Determine actor and cell sizes (before expand or fill).
        Cell[] cells     = _cells.ToArray();
        var    cellCount = _cells.Count;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c      = cells[ i ];
            var column = c.Column;
            var row    = c.Row;
            var a      = c.Actor!;

            float spannedWeightedWidth = 0;
            var   colspan              = c.Colspan;

            for ( int ii = column, nn = ii + colspan; ii < nn; ii++ )
            {
                spannedWeightedWidth += columnWeightedWidth[ ii ];
            }

            var weightedHeight = rowWeightedHeight[ row ];

            var prefWidth  = c.PrefWidth?.Get( a );
            var prefHeight = c.PrefHeight?.Get( a );
            var minWidth   = c.MinWidth?.Get( a );
            var minHeight  = c.MinHeight?.Get( a );
            var maxWidth   = c.MaxWidth?.Get( a );
            var maxHeight  = c.MaxHeight?.Get( a );

            if ( prefWidth < minWidth )
            {
                prefWidth = minWidth;
            }

            if ( prefHeight < minHeight )
            {
                prefHeight = minHeight;
            }

            if ( ( maxWidth > 0 ) && ( prefWidth > maxWidth ) )
            {
                prefWidth = maxWidth;
            }

            if ( ( maxHeight > 0 ) && ( prefHeight > maxHeight ) )
            {
                prefHeight = maxHeight;
            }

            c.ActorWidth = Math.Min( spannedWeightedWidth - c.ComputedPadLeft - c.ComputedPadRight, ( float ) prefWidth! );

            c.ActorHeight = Math.Min( weightedHeight - c.ComputedPadTop - c.ComputedPadBottom, ( float ) prefHeight! );

            if ( colspan == 1 )
            {
                _columnWidth[ column ] = Math.Max( _columnWidth[ column ], spannedWeightedWidth );
            }

            rowHeight[ row ] = Math.Max( rowHeight[ row ], weightedHeight );
        }

        // Distribute remaining space to any expanding columns/rows.
        var   expandWidth  = _expandWidth;
        var   expandHeight = _expandHeight;
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
                    if ( expandWidth[ i ] == 0 )
                    {
                        continue;
                    }

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
        {
            totalExpand += expandHeight[ i ];
        }

        if ( totalExpand > 0 )
        {
            var extra = layoutHeight - vpadding;

            for ( var i = 0; i < rows; i++ )
            {
                extra -= rowHeight[ i ];
            }

            if ( extra > 0 )
            {
                // layoutHeight < tableMinHeight.
                float used      = 0;
                var   lastIndex = 0;

                for ( var i = 0; i < rows; i++ )
                {
                    if ( expandHeight[ i ] == 0 )
                    {
                        continue;
                    }

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
            var c       = cells[ i ];
            var colspan = c.Colspan;

            if ( colspan == 1 )
            {
                continue;
            }

            float extraWidth = 0;

            for ( int column = c.Column, nn = column + colspan; column < nn; column++ )
            {
                extraWidth += columnWeightedWidth[ column ] - columnWidth[ column ];
            }

            extraWidth -= Math.Max( 0, c.ComputedPadLeft + c.ComputedPadRight );

            extraWidth /= colspan;

            if ( extraWidth > 0 )
            {
                for ( int column = c.Column, nn = column + colspan; column < nn; column++ )
                {
                    columnWidth[ column ] += extraWidth;
                }
            }
        }

        // Determine table size.
        var tableWidth  = hpadding;
        var tableHeight = vpadding;

        for ( var i = 0; i < columns; i++ )
        {
            tableWidth += columnWidth[ i ];
        }

        for ( var i = 0; i < rows; i++ )
        {
            tableHeight += rowHeight[ i ];
        }

        // Position table within the container.
        var x = padLeft;

        if ( ( _alignment & Align.RIGHT ) != 0 )
        {
            x += layoutWidth - tableWidth;
        }
        else if ( ( _alignment & Align.LEFT ) == 0 ) // Center
        {
            x += ( layoutWidth - tableWidth ) / 2;
        }

        var y = padTop;

        if ( ( _alignment & Align.BOTTOM ) != 0 )
        {
            y += layoutHeight - tableHeight;
        }
        else if ( ( _alignment & Align.TOP ) == 0 ) // Center
        {
            y += ( layoutHeight - tableHeight ) / 2;
        }

        // Size and position actors within cells.
        float currentX = x, currentY = y;

        for ( var i = 0; i < cellCount; i++ )
        {
            var c = cells[ i ];

            float spannedCellWidth = 0;

            for ( int column = c.Column, nn = ( column + c.Colspan )!; column < nn; column++ )
            {
                spannedCellWidth += columnWidth[ column ];
            }

            spannedCellWidth -= c.ComputedPadLeft + c.ComputedPadRight;

            currentX += c.ComputedPadLeft;

            var fillX = c.FillX;
            var fillY = c.FillY;

            if ( fillX > 0 )
            {
                c.ActorWidth = Math.Max( spannedCellWidth * fillX, c.MinWidth!.Get( c.Actor ) );

                var maxWidth = c.MaxWidth?.Get( c.Actor );

                if ( maxWidth > 0 )
                {
                    c.ActorWidth = Math.Min( c.ActorWidth, ( float ) maxWidth );
                }
            }

            if ( fillY > 0 )
            {
                c.ActorHeight = Math.Max(
                                         ( rowHeight[ c.Row ] * fillY ) - c.ComputedPadTop - c.ComputedPadBottom,
                                         c.MinHeight!.Get( c.Actor )
                                        );

                var maxHeight = c.MaxHeight?.Get( c.Actor );

                if ( maxHeight > 0 )
                {
                    c.ActorHeight = Math.Min( c.ActorHeight, ( float ) maxHeight );
                }
            }

            _alignment = c.Alignment;

            if ( ( _alignment & Align.LEFT ) != 0 )
            {
                c.ActorX = currentX;
            }
            else if ( ( _alignment & Align.RIGHT ) != 0 )
            {
                c.ActorX = ( currentX + spannedCellWidth ) - c.ActorWidth;
            }
            else
            {
                c.ActorX = currentX + ( ( spannedCellWidth - c.ActorWidth ) / 2 );
            }

            if ( ( _alignment & Align.TOP ) != 0 )
            {
                c.ActorY = c.ComputedPadTop;
            }
            else if ( ( _alignment & Align.BOTTOM ) != 0 )
            {
                c.ActorY = rowHeight[ c.Row ] - c.ActorHeight - c.ComputedPadBottom;
            }
            else
            {
                c.ActorY = ( ( ( rowHeight[ c.Row ] - c.ActorHeight ) + c.ComputedPadTop ) - c.ComputedPadBottom ) / 2;
            }

            c.ActorY = layoutHeight - currentY - c.ActorY - c.ActorHeight;

            if ( Round )
            {
                c.ActorWidth  = ( float ) Math.Ceiling( c.ActorWidth );
                c.ActorHeight = ( float ) Math.Ceiling( c.ActorHeight );
                c.ActorX      = ( float ) Math.Floor( c.ActorX );
                c.ActorY      = ( float ) Math.Floor( c.ActorY );
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
            if ( Children.GetAt( i ) is ILayout )
            {
                ( ( ILayout ) Children.GetAt( i ) ).Validate();
            }
        }

        // Store debug rectangles.
        if ( TableDebug != DebugType.None )
        {
            AddDebugRects( x, y, tableWidth - hpadding, tableHeight - vpadding );
        }
    }

    public Cell GetNewObject()
    {
        return new Cell();
    }

    // ------------------------------------------------------------------------

    internal class DebugRect : RectangleShape
    {
        internal static Pool< DebugRect > Pool  { get; }      = Pools< DebugRect >.Get();
        internal        Color             Color { get; set; } = null!;
    }

    // ------------------------------------------------------------------------

    #region Debugging

    public void SetDebug( bool enabled )
    {
        DebugLines( enabled ? DebugType.All : DebugType.None );
    }

    public override Table EnableDebug()
    {
        base.EnableDebug();

        return this;
    }

    public override Table DebugAll()
    {
        base.DebugAll();

        return this;
    }

    /// <summary>
    /// Turns on table debug lines.
    /// </summary>
    public Table DebugTable()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Table )
        {
            TableDebug = DebugType.Table;
            Invalidate();
        }

        return this;
    }

    /// <summary>
    /// Turns on cell debug lines.
    /// </summary>
    public Table DebugCell()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Cell )
        {
            TableDebug = DebugType.Cell;
            Invalidate();
        }

        return this;
    }

    /// <summary>
    /// Turns on actor debug lines.
    /// </summary>
    public Table DebugActor()
    {
        base.SetDebug( true, false );

        if ( TableDebug != DebugType.Actor )
        {
            TableDebug = DebugType.Actor;
            Invalidate();
        }

        return this;
    }

    /// <summary>
    /// Turns debug lines on or off.
    /// </summary>
    public Table DebugLines( DebugType debug )
    {
        base.SetDebug( debug != DebugType.None, false );

        if ( TableDebug != debug )
        {
            TableDebug = debug;

            if ( debug == DebugType.None )
            {
                ClearDebugRects();
            }
            else
            {
                Invalidate();
            }
        }

        return this;
    }

    private void ClearDebugRects()
    {
        _debugRects ??= [ ];

        DebugRect.Pool.FreeAll( _debugRects );

        _debugRects.Clear();
    }

    private void AddDebugRect( float x, float y, float w, float h, Color color )
    {
        var rect = DebugRect.Pool.Obtain();

        rect!.Color = color;
        rect.Set( x, y, w, h );

        _debugRects?.Add( rect );
    }

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
            var c = _cells[ i ];

            // Actor bounds.
            if ( TableDebug is DebugType.Actor or DebugType.All )
            {
                AddDebugRect( c.ActorX, c.ActorY, c.ActorWidth, c.ActorHeight, DebugActorColor );
            }

            // Cell bounds.
            float spannedCellWidth = 0;

            for ( int column = c.Column, nn = column + c.Colspan; column < nn; column++ )
            {
                spannedCellWidth += _columnWidth[ column ];
            }

            spannedCellWidth -= c.ComputedPadLeft + c.ComputedPadRight;
            currentX         += c.ComputedPadLeft;

            if ( TableDebug is DebugType.Cell or DebugType.All )
            {
                AddDebugRect(
                             currentX,
                             currentY + c.ComputedPadTop,
                             spannedCellWidth,
                             _rowHeight[ c.Row ] - c.ComputedPadTop - c.ComputedPadBottom,
                             DebugCellColor
                            );
            }

            if ( c.EndRow )
            {
                currentX =  x;
                currentY += _rowHeight[ c.Row ];
            }
            else
            {
                currentX += spannedCellWidth + c.ComputedPadRight;
            }
        }
    }

    public override void DrawDebug( ShapeRenderer shapes )
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

    private void DrawDebugRects( ShapeRenderer shapes )
    {
        if ( ( _debugRects == null ) || !DebugActive )
        {
            return;
        }

        shapes.Set( ShapeRenderer.ShapeTypes.Lines );

        if ( Stage != null )
        {
            shapes.Color = Stage.DebugColor;
        }

        float x = 0;
        float y = 0;

        if ( !Transform )
        {
            x = X;
            y = Y;
        }

        for ( int i = 0, n = _debugRects.Count; i < n; i++ )
        {
            var debugRect = _debugRects[ i ];

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
    public bool Round { get; set; } = true;

    public int Rows { get; private set; }

    public int Columns { get; set; }

    #endregion Properties

    #region backgrounds

    /// <summary>
    /// Value that is the top padding of the table's background.
    /// </summary>
    public static readonly Value BackgroundTop = new BackgroundTopDelegate();

    /// <summary>
    /// Value that is the bottom padding of the table's background.
    /// </summary>
    public static readonly Value BackgroundBottom = new BackgroundBottomDelegate();

    /// <summary>
    /// Value that is the left padding of the table's background.
    /// </summary>
    public static readonly Value BackgroundLeft = new BackgroundLeftDelegate();

    /// <summary>
    /// Value that is the right padding of the table's background.
    /// </summary>
    public static readonly Value BackgroundRight = new BackgroundRightDelegate();

    private sealed class BackgroundTopDelegate : Value
    {
        public override float Get( Actor? context = null )
        {
            return ( ( Table? ) context )?.GetBackground()?.TopHeight ?? 0;
        }
    }

    private sealed class BackgroundBottomDelegate : Value
    {
        public override float Get( Actor? context = null )
        {
            return ( ( Table? ) context )?.GetBackground()?.BottomHeight ?? 0;
        }
    }

    private sealed class BackgroundLeftDelegate : Value
    {
        public override float Get( Actor? context = null )
        {
            return ( ( Table? ) context )?.GetBackground()?.LeftWidth ?? 0;
        }
    }

    private sealed class BackgroundRightDelegate : Value
    {
        public override float Get( Actor? context = null )
        {
            return ( ( Table? ) context )?.GetBackground()?.RightWidth ?? 0;
        }
    }

    #endregion
}
