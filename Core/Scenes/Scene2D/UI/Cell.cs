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

using LibGDXSharp.Utils;
using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Cell : IPoolable
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

    private IFiles? _files;
    private Cell?   _defaults;

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

    public int  Alignment { get; set; } = Align.NONE;
    public int  ExpandX   { get; set; }
    public int  ExpandY   { get; set; }
    public int  Colspan   { get; set; }
    public bool UniformX  { get; set; }
    public bool UniformY  { get; set; }

    public Actor? Actor       { get; set; }
    public float  ActorX      { get; set; }
    public float  ActorY      { get; set; }
    public float  ActorWidth  { get; set; }
    public float  ActorHeight { get; set; }
    public float  FillX       { get; set; }
    public float  FillY       { get; set; }

    public Table? Table             { get; set; }
    public bool   EndRow            { get; set; }
    public int    Column            { get; set; }
    public int    Row               { get; set; }
    public int    CellAboveIndex    { get; set; }
    public float  ComputedPadTop    { get; set; }
    public float  ComputedPadLeft   { get; set; }
    public float  ComputedPadBottom { get; set; }
    public float  ComputedPadRight  { get; set; }

    /// <summary>
    /// Default constructor. 
    /// </summary>
    public Cell()
    {
        CellAboveIndex = -1;

        Cell? defaults = GetCellDefaults();

        if ( defaults != null ) Set( defaults );
    }

    /// <summary>
    /// Sets the actor in this cell and adds the actor to the cell's table.
    /// If null, removes any current actor.
    /// </summary>
    /// <returns>This Cell for chaining.</returns>
    public Cell SetActor<T>( T? newActor ) where T : Actor
    {
        if ( this.Actor != newActor )
        {
            if ( Actor?.Parent == this.Table )
            {
                Actor?.Remove();
            }

            Actor = newActor;

            if ( Actor != null ) this.Table?.AddActor( Actor );
        }

        return this;
    }

    /// <summary>
    /// Removes the current actor for the cell, if any.
    /// </summary>
    /// <returns>This Cell for chaining.</returns>
    public Cell ClearActor()
    {
        SetActor< Actor >( null );

        return this;
    }

    public bool HasActor() => this.Actor != null;

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified value.
    /// </summary>
    /// <param name="size">The <see cref="Value"/> to use.</param>
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
    public Cell Size( float size )
    {
        Size( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified values.
    /// </summary>
    public Cell Size( float width, float height )
    {
        Size( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth, prefWidth, and maxWidth to the specified value.
    /// </summary>
    /// <param name="width"></param>
    /// <returns></returns>
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
    /// <returns></returns>
    public Cell Width( float width )
    {
        Width( Value.Fixed.ValueOf( width ) );

        return this;
    }

    /// <summary>
    /// Sets the minHeight, prefHeight, and maxHeight to the specified value.
    /// </summary>
    /// <param name="height"></param>
    /// <returns></returns>
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
    public Cell Height( float height )
    {
        Height( Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified value.
    /// </summary>
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
    public Cell MinSize( float size )
    {
        MinSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified values.
    /// </summary>
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
    /// <returns>This Cell for chaining.</returns>
    public Cell SetMinWidth( Value minWidth )
    {
        ArgumentNullException.ThrowIfNull( minWidth );

        this.MinWidth = minWidth;

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinHeight"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minHeight">
    /// The new value for MinHeight, passed as a <see cref="Value"/>
    /// </param>
    /// <returns>This Cell for chaining.</returns>
    public Cell SetMinHeight( Value minHeight )
    {
        ArgumentNullException.ThrowIfNull( minHeight );

        this.MinHeight = minHeight;

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinWidth"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minWidth">
    /// The new value for MinWidth, passed as a float.
    /// </param>
    /// <returns>This Cell for chaining.</returns>
    public Cell SetMinWidth( float minWidth )
    {
        this.MinWidth = Value.Fixed.ValueOf( minWidth );

        return this;
    }

    /// <summary>
    /// Convenience method which sets the <see cref="MinHeight"/> property
    /// and then returns this Cell for chaining.
    /// </summary>
    /// <param name="minHeight">
    /// The new value for MinHeight, passed as a float
    /// </param>
    /// <returns>This Cell for chaining.</returns>
    public Cell SetMinHeight( float minHeight )
    {
        this.MinHeight = Value.Fixed.ValueOf( minHeight );

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
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
    public Cell SetPrefSize( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        PrefWidth  = width;
        PrefHeight = height;

        return this;
    }

    public Cell SetPrefWidth( Value prefWidth )
    {
        ArgumentNullException.ThrowIfNull( prefWidth );

        this.PrefWidth = prefWidth;

        return this;
    }

    public Cell SetPrefHeight( Value prefHeight )
    {
        ArgumentNullException.ThrowIfNull( prefHeight );

        this.PrefHeight = prefHeight;

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public Cell SetPrefSize( float width, float height )
    {
        SetPrefSize( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified values. 
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    public Cell SetPrefSize( float size )
    {
        SetPrefSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    public Cell SetPrefWidth( float prefWidth )
    {
        this.PrefWidth = Value.Fixed.ValueOf( prefWidth );

        return this;
    }

    public Cell SetPrefHeight( float prefHeight )
    {
        this.PrefHeight = Value.Fixed.ValueOf( prefHeight );

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified value.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
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
    /// <returns></returns>
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
    /// If the maxWidth is 0, no maximum width is used. */
    /// </summary>
    /// <param name="maxWidth"></param>
    /// <returns></returns>
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
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell SetMaxHeight( Value maxHeight )
    {
        ArgumentNullException.ThrowIfNull( maxHeight );

        this.MaxHeight = maxHeight;

        return this;
    }

    /// <summary>
    /// Sets the maxWidth and maxHeight to the specified value.
    /// If the max size is 0, no maximum size is used.
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
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
    /// <returns></returns>
    public Cell SetMaxSize( float width, float height )
    {
        SetMaxSize( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// If the maxWidth is 0, no maximum width is used.
    /// </summary>
    /// <param name="maxWidth"></param>
    /// <returns></returns>
    public Cell SetMaxWidth( float maxWidth )
    {
        this.MaxWidth = Value.Fixed.ValueOf( maxWidth );

        return this;
    }

    /// <summary>
    /// If the maxHeight is 0, no maximum height is used.
    /// </summary>
    /// <param name="maxHeight"></param>
    /// <returns></returns>
    public Cell SetMaxHeight( float maxHeight )
    {
        this.MaxHeight = Value.Fixed.ValueOf( maxHeight );

        return this;
    }

    /// <summary>
    /// Sets the spaceTop, spaceLeft, spaceBottom, and spaceRight to the specified value.
    /// </summary>
    /// <param name="space"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public Cell Space( Value space )
    {
        ArgumentNullException.ThrowIfNull( space );

        this.SpaceTop    = space;
        this.SpaceLeft   = space;
        this.SpaceBottom = space;
        this.SpaceRight  = space;

        return this;
    }

    public Cell Space( Value top, Value left, Value bottom, Value right )
    {
        ArgumentNullException.ThrowIfNull( top );
        ArgumentNullException.ThrowIfNull( left );
        ArgumentNullException.ThrowIfNull( bottom );
        ArgumentNullException.ThrowIfNull( right );

        this.SpaceTop    = top;
        this.SpaceLeft   = left;
        this.SpaceBottom = bottom;
        this.SpaceRight  = right;

        return this;
    }

    public Cell SetSpaceTop( Value spaceTop )
    {
        ArgumentNullException.ThrowIfNull( spaceTop );

        this.SpaceTop = spaceTop;

        return this;
    }

    public Cell SetSpaceLeft( Value spaceLeft )
    {
        ArgumentNullException.ThrowIfNull( spaceLeft );

        this.SpaceLeft = spaceLeft;

        return this;
    }

    public Cell SetSpaceBottom( Value spaceBottom )
    {
        ArgumentNullException.ThrowIfNull( spaceBottom );

        this.SpaceBottom = spaceBottom;

        return this;
    }

    public Cell SetSpaceRight( Value spaceRight )
    {
        ArgumentNullException.ThrowIfNull( spaceRight );

        this.SpaceRight = spaceRight;

        return this;
    }

    public Cell Space( float space )
    {
        if ( space < 0 ) throw new ArgumentException( "space cannot be < 0: " + space );

        Space( Value.Fixed.ValueOf( space ) );

        return this;
    }

    public Cell Space( float top, float left, float bottom, float right )
    {
        if ( top < 0 ) throw new ArgumentException( "top cannot be < 0: " + top );
        if ( left < 0 ) throw new ArgumentException( "left cannot be < 0: " + left );
        if ( bottom < 0 ) throw new ArgumentException( "bottom cannot be < 0: " + bottom );
        if ( right < 0 ) throw new ArgumentException( "right cannot be < 0: " + right );

        Space
            (
             Value.Fixed.ValueOf( top ),
             Value.Fixed.ValueOf( left ),
             Value.Fixed.ValueOf( bottom ),
             Value.Fixed.ValueOf( right )
            );

        return this;
    }

    public Cell SetSpaceTop( float spaceTop )
    {
        if ( spaceTop < 0 ) throw new ArgumentException( "spaceTop cannot be < 0: " + spaceTop );

        this.SpaceTop = Value.Fixed.ValueOf( spaceTop );

        return this;
    }

    public Cell SetSpaceLeft( float spaceLeft )
    {
        if ( spaceLeft < 0 ) throw new ArgumentException( "spaceLeft cannot be < 0: " + spaceLeft );

        this.SpaceLeft = Value.Fixed.ValueOf( spaceLeft );

        return this;
    }

    public Cell SetSpaceBottom( float spaceBottom )
    {
        if ( spaceBottom < 0 ) throw new ArgumentException( "spaceBottom cannot be < 0: " + spaceBottom );

        this.SpaceBottom = Value.Fixed.ValueOf( spaceBottom );

        return this;
    }

    public Cell SetSpaceRight( float spaceRight )
    {
        if ( spaceRight < 0 ) throw new ArgumentException( "spaceRight cannot be < 0: " + spaceRight );

        this.SpaceRight = Value.Fixed.ValueOf( spaceRight );

        return this;
    }

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight to the specified value.
    /// </summary>
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

        this.PadTop = padTop;

        return this;
    }

    public Cell SetPadBottom( Value padBottom )
    {
        ArgumentNullException.ThrowIfNull( padBottom );

        this.PadBottom = padBottom;

        return this;
    }

    public Cell SetPadRight( Value padRight )
    {
        ArgumentNullException.ThrowIfNull( padRight );

        this.PadRight = padRight;

        return this;
    }

    public Cell Pad( float pad )
    {
        Pad( Value.Fixed.ValueOf( pad ) );

        return this;
    }

    public Cell Pad( float top, float left, float bottom, float right )
    {
        Pad
            (
             Value.Fixed.ValueOf( top ),
             Value.Fixed.ValueOf( left ),
             Value.Fixed.ValueOf( bottom ),
             Value.Fixed.ValueOf( right )
            );

        return this;
    }

    public Cell SetPadTop( float padTop )
    {
        this.PadTop = Value.Fixed.ValueOf( padTop );

        return this;
    }

    public Cell SetPadLeft( float padLeft )
    {
        this.PadLeft = Value.Fixed.ValueOf( padLeft );

        return this;
    }

    public Cell SetPadBottom( float padBottom )
    {
        this.PadBottom = Value.Fixed.ValueOf( padBottom );

        return this;
    }

    public Cell SetPadRight( float padRight )
    {
        this.PadRight = Value.Fixed.ValueOf( padRight );

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillX"/> and <see cref="FillY"/> to <see cref="ONEF"/>
    /// </summary>
    /// <returns>This Cell for chaining</returns>
    public Cell SetFill()
    {
        FillX = ONEF;
        FillY = ONEF;

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillX"/> to <see cref="ONEF"/>.
    /// Leaves <see cref="FillY"/> unchanged.
    /// </summary>
    /// <returns>This Cell for chaining</returns>
    public Cell SetFillX()
    {
        FillX = ONEF;

        return this;
    }

    /// <summary>
    /// Sets <see cref="FillY"/> to <see cref="ONEF"/>.
    /// Leaves <see cref="FillX"/> unchanged.
    /// </summary>
    /// <returns>This Cell for chaining</returns>
    public Cell SetFillY()
    {
        FillY = ONEF;

        return this;
    }

    public Cell SetFill( float x, float y )
    {
        FillX = x;
        FillY = y;

        return this;
    }

    public Cell SetFill( bool x, bool y )
    {
        FillX = x ? ONEF : ZEROF;
        FillY = y ? ONEF : ZEROF;

        return this;
    }

    public Cell SetFill( bool fill )
    {
        FillX = fill ? ONEF : ZEROF;
        FillY = fill ? ONEF : ZEROF;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the actor within the cell. Set to <see cref="Align.CENTER"/>,
    /// <see cref="Align.TOP"/>, <see cref="Align.BOTTOM"/>, <see cref="Align.LEFT"/>,
    /// <see cref="Align.RIGHT"/>, or any combination of those. 
    /// </summary>
    public Cell SetAlignment( int align )
    {
        this.Alignment = align;

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

    public Cell Grow()
    {
        ExpandX = ONEI;
        ExpandY = ONEI;
        FillX   = ONEF;
        FillY   = ONEF;

        return this;
    }

    public Cell GrowX()
    {
        ExpandX = ONEI;
        FillX   = ONEF;

        return this;
    }

    public Cell GrowY()
    {
        ExpandY = ONEI;
        FillY   = ONEF;

        return this;
    }

    public Cell Expand()
    {
        ExpandX = ONEI;
        ExpandY = ONEI;

        return this;
    }

    public Cell SetExpandX()
    {
        ExpandX = ONEI;

        return this;
    }

    public Cell SetExpandY()
    {
        ExpandY = ONEI;

        return this;
    }

    public Cell Expand( int x, int y )
    {
        ExpandX = x;
        ExpandY = y;

        return this;
    }

    public Cell Expand( bool x, bool y )
    {
        ExpandX = x ? ONEI : ZEROI;
        ExpandY = y ? ONEI : ZEROI;

        return this;
    }

    public Cell SetColspan( int colspan )
    {
        this.Colspan = colspan;

        return this;
    }

    public Cell UniformToTrue()
    {
        UniformX = true;
        UniformY = true;

        return this;
    }

    public Cell UniformXToTrue()
    {
        UniformX = true;

        return this;
    }

    public Cell UniformYToTrue()
    {
        UniformY = true;

        return this;
    }

    public Cell Uniform( bool uniform )
    {
        UniformX = uniform;
        UniformY = uniform;

        return this;
    }

    public Cell Uniform( bool x, bool y )
    {
        UniformX = x;
        UniformY = y;

        return this;
    }

    public void SetActorBounds( float x, float y, float width, float height )
    {
        ActorX      = x;
        ActorY      = y;
        ActorWidth  = width;
        ActorHeight = height;
    }

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
    public float GetPadX()        => PadLeft!.Get( Actor ) + PadRight!.Get( Actor );
    public float GetPadY()        => PadTop!.Get( Actor ) + PadBottom!.Get( Actor );
    public void  AddRow()         => Table!.AddRow();

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
        ExpandX     = default;
        ExpandY     = default;
        Colspan     = default;
        UniformX    = default;
        UniformY    = default;
    }

    public void Set( Cell? cell )
    {
        if ( cell == null ) return;

        this.MinWidth    = cell.MinWidth;
        this.MinHeight   = cell.MinHeight;
        this.PrefWidth   = cell.PrefWidth;
        this.PrefHeight  = cell.PrefHeight;
        this.MaxWidth    = cell.MaxWidth;
        this.MaxHeight   = cell.MaxHeight;
        this.SpaceTop    = cell.SpaceTop;
        this.SpaceLeft   = cell.SpaceLeft;
        this.SpaceBottom = cell.SpaceBottom;
        this.SpaceRight  = cell.SpaceRight;
        this.PadTop      = cell.PadTop;
        this.PadLeft     = cell.PadLeft;
        this.PadBottom   = cell.PadBottom;
        this.PadRight    = cell.PadRight;
        this.FillX       = cell.FillX;
        this.FillY       = cell.FillY;
        this.Alignment   = cell.Alignment;
        this.ExpandX     = cell.ExpandX;
        this.ExpandY     = cell.ExpandY;
        this.Colspan     = cell.Colspan;
        this.UniformX    = cell.UniformX;
        this.UniformY    = cell.UniformY;
    }

    public void Merge( Cell? cell )
    {
        if ( cell == null ) return;

        //@formatter:off
        if ( cell.MinWidth != null )        MinWidth    = cell.MinWidth;
        if ( cell.MinHeight != null )       MinHeight   = cell.MinHeight;
        if ( cell.PrefWidth != null )       PrefWidth   = cell.PrefWidth;
        if ( cell.PrefHeight != null )      PrefHeight  = cell.PrefHeight;
        if ( cell.MaxWidth != null )        MaxWidth    = cell.MaxWidth;
        if ( cell.MaxHeight != null )       MaxHeight   = cell.MaxHeight;
        if ( cell.SpaceTop != null )        SpaceTop    = cell.SpaceTop;
        if ( cell.SpaceLeft != null )       SpaceLeft   = cell.SpaceLeft;
        if ( cell.SpaceBottom != null )     SpaceBottom = cell.SpaceBottom;
        if ( cell.SpaceRight != null )      SpaceRight  = cell.SpaceRight;
        if ( cell.PadTop != null )          PadTop      = cell.PadTop;
        if ( cell.PadLeft != null )         PadLeft     = cell.PadLeft;
        if ( cell.PadBottom != null )       PadBottom   = cell.PadBottom;
        if ( cell.PadRight != null )        PadRight    = cell.PadRight;
        if ( cell.Alignment != default )    Alignment   = cell.Alignment;
        if ( cell.ExpandX != default )      ExpandX     = cell.ExpandX;
        if ( cell.ExpandY != default )      ExpandY     = cell.ExpandY;
        if ( cell.Colspan != default )      Colspan     = cell.Colspan;
        if ( cell.UniformX != default )     UniformX    = cell.UniformX;
        if ( cell.UniformY != default )     UniformY    = cell.UniformY;

        FillX = cell.FillX;
        FillY = cell.FillY;
        //@formatter:on
    }

    public new string? ToString()
    {
        return Actor != null ? Actor.ToString() : base.ToString();
    }

    /// <summary>
    /// Returns the defaults to use for all cells. This can be used to avoid
    /// needing to set the same defaults for every table (eg, for spacing).
    /// </summary>
    public Cell? GetCellDefaults()
    {
        if ( ( _files == null ) || ( _files != Gdx.Files ) )
        {
            _files = Gdx.Files;

            _defaults = new Cell
            {
                MinWidth    = UI.Value.MinWidth,
                MinHeight   = UI.Value.MinHeight,
                PrefWidth   = UI.Value.PrefWidth,
                PrefHeight  = UI.Value.PrefHeight,
                MaxWidth    = UI.Value.MaxWidth,
                MaxHeight   = UI.Value.MaxHeight,
                SpaceTop    = UI.Value.ZERO,
                SpaceLeft   = UI.Value.ZERO,
                SpaceBottom = UI.Value.ZERO,
                SpaceRight  = UI.Value.ZERO,
                PadTop      = UI.Value.ZERO,
                PadLeft     = UI.Value.ZERO,
                PadBottom   = UI.Value.ZERO,
                PadRight    = UI.Value.ZERO,
                FillX       = ZEROF,
                FillY       = ZEROF,
                Alignment   = CENTERI,
                ExpandX     = ZEROI,
                ExpandY     = ZEROI,
                Colspan     = ONEI,
                UniformX    = default( bool ),
                UniformY    = default( bool )
            };

        }

        return _defaults;
    }

    // -------------------- From IPoolable.cs --------------------

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
}