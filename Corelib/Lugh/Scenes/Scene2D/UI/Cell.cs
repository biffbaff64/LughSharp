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

using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;
using Corelib.Lugh.Utils.Pooling;

namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// A Cell for use with <see cref="Table"/>s.
/// </summary>
[PublicAPI]
public class Cell : IResetable
{
    private const float ZEROF   = 0f;
    private const float ONEF    = 1f;
    private const int   ZEROI   = 0;
    private const int   ONEI    = 1;
    private const int   CENTERI = ONEI;
    private const int   TOPI    = Align.TOP;
    private const int   BOTTOMI = Align.BOTTOM;
    private const int   LEFTI   = Align.LEFT;
    private const int   RIGHTI  = Align.RIGHT;

    // ========================================================================

    public Value? MinWidth    { get; set; }
    public Value? MinHeight   { get; set; }
    public Value? PrefWidth   { get; set; }
    public Value? PrefHeight  { get; set; }
    public Value? MaxWidth    { get; set; }
    public Value? MaxHeight   { get; set; }
    public Value? SpaceTop    { get; set; }
    public Value? SpaceLeft   { get; set; }
    public Value? SpaceBottom { get; set; }
    public Value? SpaceRight  { get; set; }
    public Value? PadTop      { get; set; }
    public Value? PadLeft     { get; set; }
    public Value? PadBottom   { get; set; }
    public Value? PadRight    { get; set; }
    public int    Alignment   { get; set; } = Align.NONE;
    public int    ExpandX     { get; set; }
    public int    ExpandY     { get; set; }
    public int    Colspan     { get; set; }
    public bool   UniformX    { get; set; }
    public bool   UniformY    { get; set; }

    // ========================================================================

    public Actor? Actor       { get; set; }
    public float  ActorX      { get; set; }
    public float  ActorY      { get; set; }
    public float  ActorWidth  { get; set; }
    public float  ActorHeight { get; set; }
    public float  FillX       { get; set; }
    public float  FillY       { get; set; }

    // ========================================================================

    public Table? Table             { get; set; }
    public bool   EndRow            { get; set; }
    public int    Column            { get; set; }
    public int    Row               { get; set; }
    public int    CellAboveIndex    { get; set; }
    public float  ComputedPadTop    { get; set; }
    public float  ComputedPadLeft   { get; set; }
    public float  ComputedPadBottom { get; set; }
    public float  ComputedPadRight  { get; set; }

    // ========================================================================

    private Cell?   _defaults;
    private IFiles? _files;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Default constructor. Creates a new Cell with properties set to
    /// Cell defaults.
    /// </summary>
    public Cell()
    {
        CellAboveIndex = -1;

        var defaults = GetCellDefaults();

        if ( defaults != null )
        {
            Set( defaults );
        }
    }

    /// <summary>
    /// Reset state so the cell can be reused, setting all constraints to
    /// their <see cref="GetCellDefaults()"/> values.
    /// </summary>
    public void Reset()
    {
        Actor          = null;
        Table          = null!;
        EndRow         = false;
        CellAboveIndex = -1;

        Set( GetCellDefaults() );
    }

    /// <summary>
    /// Sets the actor in this cell and adds the actor to the cell's table.
    /// If null, removes any current actor.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetActor< T >( T? newActor ) where T : Actor
    {
        if ( Actor != newActor )
        {
            if ( Actor?.Parent == Table )
            {
                Actor?.Remove();
            }

            Actor = newActor;

            if ( Actor != null )
            {
                Table?.AddActor( Actor );
            }
        }

        return this;
    }

    /// <summary>
    /// Removes the current actor for the cell, if any.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell ClearActor()
    {
        SetActor< Actor >( null );

        return this;
    }

    /// <summary>
    /// Returns <b>true</b> if this Cells <see cref="Actor"/> is not null.
    /// </summary>
    public bool HasActor() => Actor != null;

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified value.
    /// </summary>
    /// <param name="size">The <see cref="Value"/> to use.</param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentNullException">If parameter <tt>size</tt> is null.</exception>
    public Cell Size( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        MinWidth   = size;
        MinHeight  = size;
        PrefWidth  = size;
        PrefHeight = size;
        MaxWidth   = size;
        MaxHeight  = size;

        return this;
    }

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified values.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Size( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        MinWidth   = width;
        MinHeight  = height;
        PrefWidth  = width;
        PrefHeight = height;
        MaxWidth   = width;
        MaxHeight  = height;

        return this;
    }

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Size( float size )
    {
        Size( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified values.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Size( float width, float height )
    {
        Size( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth, prefWidth, and maxWidth to the specified value.
    /// </summary>
    /// <param name="width"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell Width( Value width )
    {
        ArgumentNullException.ThrowIfNull( width );

        MinWidth  = width;
        PrefWidth = width;
        MaxWidth  = width;

        return this;
    }

    /// <summary>
    /// Sets the minWidth, prefWidth, and maxWidth to the specified value.
    /// </summary>
    /// <param name="width"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell Width( float width )
    {
        Width( Value.Fixed.ValueOf( width ) );

        return this;
    }

    /// <summary>
    /// Sets the minHeight, prefHeight, and maxHeight to the specified value.
    /// </summary>
    /// <param name="height"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell Height( Value height )
    {
        ArgumentNullException.ThrowIfNull( height );

        MinHeight  = height;
        PrefHeight = height;
        MaxHeight  = height;

        return this;
    }

    /// <summary>
    /// Sets the minHeight, prefHeight, and maxHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Height( float height )
    {
        Height( Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell MinSize( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        MinWidth  = size;
        MinHeight = size;

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell MinSize( float size )
    {
        MinSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified values.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell MinSize( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        MinWidth  = width;
        MinHeight = height;

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified values.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell MinSize( float width, float height )
    {
        MinSize( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinWidth"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minWidth">
    /// The new value for MinWidth, passed as a <see cref="Value"/>
    /// </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMinWidth( Value minWidth )
    {
        ArgumentNullException.ThrowIfNull( minWidth );

        MinWidth = minWidth;

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinHeight"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minHeight">
    /// The new value for MinHeight, passed as a <see cref="Value"/>
    /// </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMinHeight( Value minHeight )
    {
        ArgumentNullException.ThrowIfNull( minHeight );

        MinHeight = minHeight;

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinWidth"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minWidth">
    /// The new value for MinWidth, passed as a float.
    /// </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMinWidth( float minWidth )
    {
        MinWidth = Value.Fixed.ValueOf( minWidth );

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinHeight"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minHeight">
    /// The new value for MinHeight, passed as a float
    /// </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMinHeight( float minHeight )
    {
        MinHeight = Value.Fixed.ValueOf( minHeight );

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// and <see cref="PrefHeight"/> properties directly, as this Cell is returned
    /// for chaining purposes.
    /// </remarks>
    public Cell SetPrefSize( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        PrefWidth  = size;
        PrefHeight = size;

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// and <see cref="PrefHeight"/> properties directly, as this Cell is returned
    /// for chaining purposes.
    /// </remarks>
    public Cell SetPrefSize( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        PrefWidth  = width;
        PrefHeight = height;

        return this;
    }

    /// <summary>
    /// Sets the <see cref="PrefWidth"/> for this Cell.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// property directly, as this Cell is returned for chaining purposes.
    /// </remarks>
    public Cell SetPrefWidth( Value prefWidth )
    {
        ArgumentNullException.ThrowIfNull( prefWidth );

        PrefWidth = prefWidth;

        return this;
    }

    /// <summary>
    /// Sets the <see cref="PrefHeight"/> for this Cell.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefHeight"/>
    /// property directly, as this Cell is returned for chaining purposes.
    /// </remarks>
    public Cell SetPrefHeight( Value prefHeight )
    {
        ArgumentNullException.ThrowIfNull( prefHeight );

        PrefHeight = prefHeight;

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    /// <param name="width"> The new width. </param>
    /// <param name="height"> The new height. </param>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// and <see cref="PrefHeight"/> properties directly, as this Cell is returned
    /// for chaining purposes.
    /// </remarks>
    public Cell SetPrefSize( float width, float height )
    {
        SetPrefSize( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified values.
    /// </summary>
    /// <param name="size"> The new size, which applies to both width and height. </param>
    /// <returns> This Cell for chaining. </returns>
    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// and <see cref="PrefHeight"/> properties directly, as this Cell is returned
    /// for chaining purposes.
    /// </remarks>
    public Cell SetPrefSize( float size )
    {
        SetPrefSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefWidth"/>
    /// property directly, as this Cell is returned for chaining purposes.
    /// </remarks>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetPrefWidth( float prefWidth )
    {
        PrefWidth = Value.Fixed.ValueOf( prefWidth );

        return this;
    }

    /// <remarks>
    /// A method is required for this, instead of accessing the <see cref="PrefHeight"/>
    /// property directly, as this Cell is returned for chaining purposes.
    /// </remarks>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetPrefHeight( float prefHeight )
    {
        PrefHeight = Value.Fixed.ValueOf( prefHeight );

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified value.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="size"></param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell SetMaxSize( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        MaxWidth  = size;
        MaxHeight = size;

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified values.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell SetMaxSize( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        MaxWidth  = width;
        MaxHeight = height;

        return this;
    }

    /// <summary>
    /// If the maxWidth is 0, no maximum width is used.
    /// </summary>
    /// <param name="maxWidth"></param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell SetMaxWidth( Value maxWidth )
    {
        ArgumentNullException.ThrowIfNull( maxWidth );

        MaxWidth = maxWidth;

        return this;
    }

    /// <summary>
    /// If the maxHeight is 0, no maximum height is used.
    /// </summary>
    /// <param name="maxHeight"></param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell SetMaxHeight( Value maxHeight )
    {
        ArgumentNullException.ThrowIfNull( maxHeight );

        MaxHeight = maxHeight;

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified value.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="size"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMaxSize( float size )
    {
        SetMaxSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified values.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMaxSize( float width, float height )
    {
        SetMaxSize( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// If the maxWidth is 0, no maximum width is used.
    /// </summary>
    /// <param name="maxWidth"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMaxWidth( float maxWidth )
    {
        MaxWidth = Value.Fixed.ValueOf( maxWidth );

        return this;
    }

    /// <summary>
    /// If the maxHeight is 0, no maximum height is used.
    /// </summary>
    /// <param name="maxHeight"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetMaxHeight( float maxHeight )
    {
        MaxHeight = Value.Fixed.ValueOf( maxHeight );

        return this;
    }

    /// <summary>
    /// Sets the spaceTop, spaceLeft, spaceBottom, and spaceRight to the specified value.
    /// </summary>
    /// <param name="space"></param>
    /// <returns> This Cell for chaining. </returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell Space( Value space )
    {
        ArgumentNullException.ThrowIfNull( space );

        SpaceTop    = space;
        SpaceLeft   = space;
        SpaceBottom = space;
        SpaceRight  = space;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="top"></param>
    /// <param name="left"></param>
    /// <param name="bottom"></param>
    /// <param name="right"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell Space( Value top, Value left, Value bottom, Value right )
    {
        ArgumentNullException.ThrowIfNull( top );
        ArgumentNullException.ThrowIfNull( left );
        ArgumentNullException.ThrowIfNull( bottom );
        ArgumentNullException.ThrowIfNull( right );

        SpaceTop    = top;
        SpaceLeft   = left;
        SpaceBottom = bottom;
        SpaceRight  = right;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="spaceTop"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetSpaceTop( Value spaceTop )
    {
        ArgumentNullException.ThrowIfNull( spaceTop );

        SpaceTop = spaceTop;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <param name="spaceLeft"></param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetSpaceLeft( Value spaceLeft )
    {
        ArgumentNullException.ThrowIfNull( spaceLeft );

        SpaceLeft = spaceLeft;

        return this;
    }

    public Cell SetSpaceBottom( Value spaceBottom )
    {
        ArgumentNullException.ThrowIfNull( spaceBottom );

        SpaceBottom = spaceBottom;

        return this;
    }

    public Cell SetSpaceRight( Value spaceRight )
    {
        ArgumentNullException.ThrowIfNull( spaceRight );

        SpaceRight = spaceRight;

        return this;
    }

    public Cell Space( float space )
    {
        if ( space < 0 )
        {
            throw new ArgumentException( $"space cannot be < 0: {space}" );
        }

        Space( Value.Fixed.ValueOf( space ) );

        return this;
    }

    public Cell Space( float top, float left, float bottom, float right )
    {
        if ( top < 0 )
        {
            throw new ArgumentException( $"top cannot be < 0: {top}" );
        }

        if ( left < 0 )
        {
            throw new ArgumentException( $"left cannot be < 0: {left}" );
        }

        if ( bottom < 0 )
        {
            throw new ArgumentException( $"bottom cannot be < 0: {bottom}" );
        }

        if ( right < 0 )
        {
            throw new ArgumentException( $"right cannot be < 0: {right}" );
        }

        Space( Value.Fixed.ValueOf( top ),
               Value.Fixed.ValueOf( left ),
               Value.Fixed.ValueOf( bottom ),
               Value.Fixed.ValueOf( right ) );

        return this;
    }

    public Cell SetSpaceTop( float spaceTop )
    {
        if ( spaceTop < 0 )
        {
            throw new ArgumentException( $"spaceTop cannot be < 0: {spaceTop}" );
        }

        SpaceTop = Value.Fixed.ValueOf( spaceTop );

        return this;
    }

    public Cell SetSpaceLeft( float spaceLeft )
    {
        if ( spaceLeft < 0 )
        {
            throw new ArgumentException( $"spaceLeft cannot be < 0: {spaceLeft}" );
        }

        SpaceLeft = Value.Fixed.ValueOf( spaceLeft );

        return this;
    }

    public Cell SetSpaceBottom( float spaceBottom )
    {
        if ( spaceBottom < 0 )
        {
            throw new ArgumentException( $"spaceBottom cannot be < 0: {spaceBottom}" );
        }

        SpaceBottom = Value.Fixed.ValueOf( spaceBottom );

        return this;
    }

    public Cell SetSpaceRight( float spaceRight )
    {
        if ( spaceRight < 0 )
        {
            throw new ArgumentException( $"spaceRight cannot be < 0: {spaceRight}" );
        }

        SpaceRight = Value.Fixed.ValueOf( spaceRight );

        return this;
    }

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight to the specified value.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Pad( Value pad )
    {
        ArgumentNullException.ThrowIfNull( pad );

        PadTop    = pad;
        PadLeft   = pad;
        PadBottom = pad;
        PadRight  = pad;

        return this;
    }

    public Cell Pad( Value top, Value left, Value bottom, Value right )
    {
        ArgumentNullException.ThrowIfNull( top );
        ArgumentNullException.ThrowIfNull( left );
        ArgumentNullException.ThrowIfNull( bottom );
        ArgumentNullException.ThrowIfNull( right );

        PadTop    = top;
        PadLeft   = left;
        PadBottom = bottom;
        PadRight  = right;

        return this;
    }

    public Cell SetPadTop( Value padTop )
    {
        ArgumentNullException.ThrowIfNull( padTop );

        PadTop = padTop;

        return this;
    }

    public Cell SetPadBottom( Value padBottom )
    {
        ArgumentNullException.ThrowIfNull( padBottom );

        PadBottom = padBottom;

        return this;
    }

    public Cell SetPadRight( Value padRight )
    {
        ArgumentNullException.ThrowIfNull( padRight );

        PadRight = padRight;

        return this;
    }

    public Cell Pad( float pad )
    {
        Pad( Value.Fixed.ValueOf( pad ) );

        return this;
    }

    public Cell Pad( float top, float left, float bottom, float right )
    {
        Pad( Value.Fixed.ValueOf( top ),
             Value.Fixed.ValueOf( left ),
             Value.Fixed.ValueOf( bottom ),
             Value.Fixed.ValueOf( right ) );

        return this;
    }

    public Cell SetPadTop( float padTop )
    {
        PadTop = Value.Fixed.ValueOf( padTop );

        return this;
    }

    public Cell SetPadLeft( float padLeft )
    {
        PadLeft = Value.Fixed.ValueOf( padLeft );

        return this;
    }

    public Cell SetPadBottom( float padBottom )
    {
        PadBottom = Value.Fixed.ValueOf( padBottom );

        return this;
    }

    public Cell SetPadRight( float padRight )
    {
        PadRight = Value.Fixed.ValueOf( padRight );

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillX"/> and <see cref="FillY"/> to <see cref="ONEF"/>
    /// </summary>
    /// <returns> This Cell for chaining </returns>
    public Cell SetFill( float fx = ONEF, float fy = ONEF )
    {
        FillX = fx;
        FillY = fy;

        return this;
    }

    public Cell SetFillConditional( bool x, bool y )
    {
        FillX = x ? ONEF : ZEROF;
        FillY = y ? ONEF : ZEROF;

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillX"/> to <see cref="ONEF"/>.
    /// Leaves <see cref="FillY"/> unchanged.
    /// </summary>
    /// <returns> This Cell for chaining </returns>
    public Cell SetFillX( float fx = ONEF )
    {
        FillX = fx;

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillY"/> to <see cref="ONEF"/>.
    /// Leaves <see cref="FillX"/> unchanged.
    /// </summary>
    /// <returns> This Cell for chaining </returns>
    public Cell SetFillY( float fy = ONEF )
    {
        FillY = fy;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the actor within the cell. Set to <see cref="Align.CENTER"/>,
    /// <see cref="Align.TOP"/>, <see cref="Align.BOTTOM"/>, <see cref="Align.LEFT"/>,
    /// <see cref="Align.RIGHT"/>, or any combination of those.
    /// </summary>
    public Cell SetAlignment( int align )
    {
        Alignment = align;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the actor within the cell to <see cref="Align.CENTER"/>.
    /// This clears any other alignment.
    /// </summary>
    public Cell Center()
    {
        Alignment = CENTERI;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.TOP"/> and clears <see cref="Align.BOTTOM"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell Top()
    {
        if ( Alignment == Align.NONE )
        {
            Alignment = TOPI;
        }
        else
        {
            Alignment = ( Alignment | Align.TOP ) & ~Align.BOTTOM;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.LEFT"/> and clears <see cref="Align.RIGHT"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell Left()
    {
        if ( Alignment == Align.NONE )
        {
            Alignment = LEFTI;
        }
        else
        {
            Alignment = ( Alignment | Align.LEFT ) & ~Align.RIGHT;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.BOTTOM"/> and clears <see cref="Align.TOP"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell Bottom()
    {
        if ( Alignment == Align.NONE )
        {
            Alignment = BOTTOMI;
        }
        else
        {
            Alignment = ( Alignment | Align.BOTTOM ) & ~Align.TOP;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.RIGHT"/> and clears <see cref="Align.LEFT"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell Right()
    {
        if ( Alignment == Align.NONE )
        {
            Alignment = RIGHTI;
        }
        else
        {
            Alignment = ( Alignment | Align.RIGHT ) & ~Align.LEFT;
        }

        return this;
    }

    /// <summary>
    /// Grow this Cell in X and Y.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell Grow()
    {
        ExpandX = ONEI;
        ExpandY = ONEI;
        FillX   = ONEF;
        FillY   = ONEF;

        return this;
    }

    /// <summary>
    /// Grow this Cell in X.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell GrowX()
    {
        ExpandX = ONEI;
        FillX   = ONEF;

        return this;
    }

    /// <summary>
    /// Grow this Cell in Y.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell GrowY()
    {
        ExpandY = ONEI;
        FillY   = ONEF;

        return this;
    }

    /// <summary>
    /// Expand this Cell in X.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetExpandX()
    {
        ExpandX = ONEI;

        return this;
    }

    /// <summary>
    /// Expand this Cell in Y.
    /// </summary>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetExpandY()
    {
        ExpandY = ONEI;

        return this;
    }

    /// <summary>
    /// Expand this Cell in X and Y.
    /// </summary>
    /// <param name="x"> The value to expand in X. Default is <see cref="ONEI"/>. </param>
    /// <param name="y"> The value to expand in Y. Default is <see cref="ONEI"/>. </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell Expand( int x = ONEI, int y = ONEI )
    {
        ExpandX = x;
        ExpandY = y;

        return this;
    }

    /// <summary>
    /// Expand in X if X condition is <b>true</b>.
    /// Expand in Y if Y condition is <b>true</b>.
    /// </summary>
    /// <param name="expandX"> True to expand in X. </param>
    /// <param name="expandY"> True to expand in Y. </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell ExpandConditional( bool expandX, bool expandY )
    {
        ExpandX = expandX ? ONEI : ZEROI;
        ExpandY = expandY ? ONEI : ZEROI;

        return this;
    }

    /// <summary>
    /// Sets the number of columns this cell spans.
    /// </summary>
    /// <param name="colspan"> The number of columns to span. </param>
    /// <returns> This Cell for chaining. </returns>
    public Cell SetColspan( int colspan )
    {
        Colspan = colspan;

        return this;
    }

    public Cell SetUniform( bool ux = true, bool uy = true )
    {
        UniformX = ux;
        UniformY = uy;
        
        return this;
    }
    
    public Cell SetUniformX( bool ux = true )
    {
        UniformX = ux;

        return this;
    }
    
    public Cell SetUniformY( bool uy = true )
    {
        UniformY = uy;

        return this;
    }

    /// <summary>
    /// Sets the bounds for this Cells <see cref="Actor"/>.
    /// </summary>
    public void SetActorBounds( float x, float y, float width, float height )
    {
        ActorX      = x;
        ActorY      = y;
        ActorWidth  = width;
        ActorHeight = height;
    }

    // ========================================================================

    //@formatter:off
    public float GetMinWidth()    => MinWidth!.Get( Actor );
    public float GetMinHeight()   => MinHeight!.Get( Actor );
    public float GetPrefWidth()   => PrefWidth!.Get( Actor );
    public float GetPrefHeight()  => PrefHeight!.Get( Actor );
    public float GetMaxWidth()    => MaxWidth!.Get( Actor );
    public float GetMaxHeight()   => MaxHeight!.Get( Actor );
    public float GetSpaceTop()    => SpaceTop!.Get( Actor );
    public float GetSpaceLeft()   => SpaceLeft!.Get( Actor );
    public float GetSpaceBottom() => SpaceBottom!.Get( Actor );
    public float GetSpaceRight()  => SpaceRight!.Get( Actor );
    public float GetPadTop()      => PadTop!.Get( Actor );
    public float GetPadLeft()     => PadLeft!.Get( Actor );
    public float GetPadBottom()   => PadBottom!.Get( Actor );
    public float GetPadRight()    => PadRight!.Get( Actor );
    //@formatter:on

    // ========================================================================

    /// <summary>
    /// Gets the X-padding for this Cell, by adding together <see cref="PadLeft"/>
    /// and <see cref="PadRight"/>.
    /// </summary>
    /// <returns> The X-padding value. </returns>
    /// <exception cref="GdxRuntimeException">
    /// If either <see cref="PadLeft"/> or <see cref="PadRight"/> is null.
    /// </exception>
    public float GetPadX()
    {
        if ( ( PadLeft == null ) || ( PadRight == null ) )
        {
            throw new GdxRuntimeException( "PadLeft / PadRight cannot be null!" );
        }

        return PadLeft.Get( Actor ) + PadRight.Get( Actor );
    }

    /// <summary>
    /// Gets the Y-padding for this Cell, by adding together <see cref="PadTop"/>
    /// and <see cref="PadBottom"/>.
    /// </summary>
    /// <returns> The Y-padding value. </returns>
    /// <exception cref="GdxRuntimeException">
    /// If either <see cref="PadTop"/> or <see cref="PadBottom"/> is null.
    /// </exception>
    public float GetPadY()
    {
        if ( ( PadTop == null ) || ( PadBottom == null ) )
        {
            throw new GdxRuntimeException( "PadTop / PadBottom cannot be null!" );
        }

        return PadTop.Get( Actor ) + PadBottom.Get( Actor );
    }

    /// <summary>
    /// Sets all constraint fields to null. */
    /// </summary>
    public void Clear()
    {
        MinWidth    = null;
        MinHeight   = null;
        PrefWidth   = null;
        PrefHeight  = null;
        MaxWidth    = null;
        MaxHeight   = null;
        SpaceTop    = null;
        SpaceLeft   = null;
        SpaceBottom = null;
        SpaceRight  = null;
        PadTop      = null;
        PadLeft     = null;
        PadBottom   = null;
        PadRight    = null;
        FillX       = 0f;
        FillY       = 0f;
        Alignment   = Align.NONE;
        ExpandX     = default( int );
        ExpandY     = default( int );
        Colspan     = default( int );
        UniformX    = default( bool );
        UniformY    = default( bool );
    }

    /// <summary>
    /// Sets the properties of this Cell to the properties from the given
    /// Cell, if the other Cell is not null.
    /// </summary>
    /// <param name="cell"> The other Cell. </param>
    public void Set( Cell? cell )
    {
        if ( cell == null )
        {
            return;
        }

        MinWidth    = cell.MinWidth;
        MinHeight   = cell.MinHeight;
        PrefWidth   = cell.PrefWidth;
        PrefHeight  = cell.PrefHeight;
        MaxWidth    = cell.MaxWidth;
        MaxHeight   = cell.MaxHeight;
        SpaceTop    = cell.SpaceTop;
        SpaceLeft   = cell.SpaceLeft;
        SpaceBottom = cell.SpaceBottom;
        SpaceRight  = cell.SpaceRight;
        PadTop      = cell.PadTop;
        PadLeft     = cell.PadLeft;
        PadBottom   = cell.PadBottom;
        PadRight    = cell.PadRight;
        Alignment   = cell.Alignment;
        ExpandX     = cell.ExpandX;
        ExpandY     = cell.ExpandY;
        Colspan     = cell.Colspan;
        UniformX    = cell.UniformX;
        UniformY    = cell.UniformY;
        FillX       = cell.FillX;
        FillY       = cell.FillY;
    }

    /// <summary>
    /// Merge this Cell with the given Cell.
    /// </summary>
    /// <param name="cell"> The Cell to merge with. </param>
    public void Merge( Cell? cell )
    {
        if ( cell == null )
        {
            return;
        }

        MinWidth    = cell.MinWidth ?? MinWidth;
        MinHeight   = cell.MinHeight ?? MinHeight;
        PrefWidth   = cell.PrefWidth ?? PrefWidth;
        PrefHeight  = cell.PrefHeight ?? PrefHeight;
        MaxWidth    = cell.MaxWidth ?? MaxWidth;
        MaxHeight   = cell.MaxHeight ?? MaxHeight;
        SpaceTop    = cell.SpaceTop ?? SpaceTop;
        SpaceLeft   = cell.SpaceLeft ?? SpaceLeft;
        SpaceBottom = cell.SpaceBottom ?? SpaceBottom;
        SpaceRight  = cell.SpaceRight ?? SpaceRight;
        PadTop      = cell.PadTop ?? PadTop;
        PadLeft     = cell.PadLeft ?? PadLeft;
        PadBottom   = cell.PadBottom ?? PadBottom;
        PadRight    = cell.PadRight ?? PadRight;

        if ( cell.Alignment != default( int ) )
        {
            Alignment = cell.Alignment;
        }

        if ( cell.ExpandX != default( int ) )
        {
            ExpandX = cell.ExpandX;
        }

        if ( cell.ExpandY != default( int ) )
        {
            ExpandY = cell.ExpandY;
        }

        if ( cell.Colspan != default( int ) )
        {
            Colspan = cell.Colspan;
        }

        if ( cell.UniformX != default( bool ) )
        {
            UniformX = cell.UniformX;
        }

        if ( cell.UniformY != default( bool ) )
        {
            UniformY = cell.UniformY;
        }

        FillX = cell.FillX;
        FillY = cell.FillY;
    }

    /// <summary>
    /// Returns the defaults to use for all cells. This can be used to avoid
    /// needing to set the same defaults for every table (eg, for spacing).
    /// </summary>
    public Cell? GetCellDefaults()
    {
        if ( ( _files == null ) || ( _files != GdxApi.Files ) )
        {
            _files = GdxApi.Files;

            _defaults = new Cell
            {
                MinWidth    = Value.MinWidth,
                MinHeight   = Value.MinHeight,
                PrefWidth   = Value.PrefWidth,
                PrefHeight  = Value.PrefHeight,
                MaxWidth    = Value.MaxWidth,
                MaxHeight   = Value.MaxHeight,
                SpaceTop    = Value.Zero,
                SpaceLeft   = Value.Zero,
                SpaceBottom = Value.Zero,
                SpaceRight  = Value.Zero,
                PadTop      = Value.Zero,
                PadLeft     = Value.Zero,
                PadBottom   = Value.Zero,
                PadRight    = Value.Zero,
                FillX       = ZEROF,
                FillY       = ZEROF,
                Alignment   = CENTERI,
                ExpandX     = ZEROI,
                ExpandY     = ZEROI,
                Colspan     = ONEI,
                UniformX    = default( bool ),
                UniformY    = default( bool ),
            };
        }

        return _defaults;
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return Actor != null ? Actor.ToString() : base.ToString();
    }
}
