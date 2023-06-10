using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class Cell<T> : IPoolable where T : Actor
{
    private const float Zerof   = 0f;
    private const float Onef    = 1f;
    private const int   Zeroi   = 0;
    private const int   Onei    = 1;
    private const int   Centeri = Onei;
    private const int   Topi    = LibGDXSharp.Utils.Align.Top;
    private const int   Bottomi = LibGDXSharp.Utils.Align.Bottom;
    private const int   Lefti   = LibGDXSharp.Utils.Align.Left;
    private const int   Righti  = LibGDXSharp.Utils.Align.Right;

    private IFiles?    _files;
    private Cell< T >? _defaults;

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
    public float? FillX       { get; set; }
    public float? FillY       { get; set; }
    public int?   Alignment   { get; set; }
    public int?   ExpandX     { get; set; }
    public int?   ExpandY     { get; set; }
    public int?   Colspan     { get; set; }
    public bool?  UniformX    { get; set; }
    public bool?  UniformY    { get; set; }

    public Actor? Actor       { get; set; }
    public float  ActorX      { get; set; }
    public float  ActorY      { get; set; }
    public float  ActorWidth  { get; set; }
    public float  ActorHeight { get; set; }

    public Table Table             { get; set; }
    public bool  EndRow            { get; set; }
    public int   Column            { get; set; }
    public int   Row               { get; set; }
    public int   CellAboveIndex    { get; set; }
    public float ComputedPadTop    { get; set; }
    public float ComputedPadLeft   { get; set; }
    public float ComputedPadBottom { get; set; }
    public float ComputedPadRight  { get; set; }

    public Cell()
    {
        CellAboveIndex = -1;

        Cell< T >? defaults = GetCellDefaults();

        if ( defaults != null ) Set( defaults );
    }

    /// <summary>
    /// Sets the actor in this cell and adds the actor to the cell's table.
    /// If null, removes any current actor.
    /// </summary>
    /// <returns>This Cell for chaining.</returns>
    public Cell< T > SetActor( T? newActor )
    {
        if ( this.Actor != newActor )
        {
            if ( Actor?.Parent == this.Table )
            {
                Actor.Remove();
            }

            Actor = newActor;

            if ( Actor != null ) this.Table.AddActor( Actor );
        }

        return this;
    }

    /// <summary>
    /// Removes the current actor for the cell, if any.
    /// </summary>
    /// <returns>This Cell for chaining.</returns>
    public Cell< T > ClearActor()
    {
        SetActor( null );

        return this;
    }

    public bool HasActor() => this.Actor != null;

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified value.
    /// </summary>
    /// <param name="size">The <see cref="Value"/> to use.</param>
    /// <exception cref="ArgumentNullException">If parameter <tt>size</tt> is null.</exception>
    public Cell< T > Size( Value size )
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
    public Cell< T > Size( Value width, Value height )
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
    public Cell< T > Size( float size )
    {
        Size( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the MinWidth, PrefWidth, MaxWidth, MinHeight, PrefHeight,
    /// and MaxHeight to the specified values.
    /// </summary>
    public Cell< T > Size( float width, float height )
    {
        Size( Value.Fixed.ValueOf( width ), Value.Fixed.ValueOf( height ) );

        return this;
    }

    /** Sets the minWidth, prefWidth, and maxWidth to the specified value. */
    public Cell< T > Width( Value width )
    {
        ArgumentNullException.ThrowIfNull( width );

        MinWidth  = width;
        PrefWidth = width;
        MaxWidth  = width;

        return this;
    }

    /** Sets the minWidth, prefWidth, and maxWidth to the specified value. */
    public Cell< T > Width( float width )
    {
        Width( Value.Fixed.ValueOf( width ) );

        return this;
    }

    /** Sets the minHeight, prefHeight, and maxHeight to the specified value. */
    public Cell< T > Height( Value height )
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
    public Cell< T > Height( float height )
    {
        Height( Value.Fixed.ValueOf( height ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified value.
    /// </summary>
    public Cell< T > MinSize( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        MinWidth  = size;
        MinHeight = size;

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified value.
    /// </summary>
    public Cell< T > MinSize( float size )
    {
        MinSize( Value.Fixed.ValueOf( size ) );

        return this;
    }

    /// <summary>
    /// Sets the minWidth and minHeight to the specified values.
    /// </summary>
    public Cell< T > MinSize( Value width, Value height )
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
    public Cell< T > MinSize( float width, float height )
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
    public Cell< T > SetMinWidth( Value minWidth )
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
    public Cell< T > SetMinHeight( Value minHeight )
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
    public Cell< T > SetMinWidth( float minWidth )
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
    public Cell< T > SetMinHeight( float minHeight )
    {
        this.MinHeight = Value.Fixed.ValueOf( minHeight );

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    public Cell< T > SetPrefSize( Value size )
    {
        ArgumentNullException.ThrowIfNull( size );

        PrefWidth  = size;
        PrefHeight = size;

        return this;
    }

    /// <summary>
    /// Sets the prefWidth and prefHeight to the specified value.
    /// </summary>
    public Cell< T > SetPrefSize( Value width, Value height )
    {
        ArgumentNullException.ThrowIfNull( width );
        ArgumentNullException.ThrowIfNull( height );

        PrefWidth  = width;
        PrefHeight = height;

        return this;
    }

    public Cell< T > SetPrefWidth( Value prefWidth )
    {
        ArgumentNullException.ThrowIfNull( prefWidth );
        
        this.PrefWidth = prefWidth;

        return this;
    }

    public Cell< T > prefHeight( Value prefHeight )
    {
        if ( prefHeight == null ) throw new ArgumentException( "prefHeight cannot be null." );
        this.prefHeight = prefHeight;

        return this;
    }

    /** Sets the prefWidth and prefHeight to the specified value. */
    public Cell< T > prefSize( float width, float height )
    {
        prefSize( Value.Fixed.valueOf( width ), Value.Fixed.valueOf( height ) );

        return this;
    }

    /** Sets the prefWidth and prefHeight to the specified values. */
    public Cell< T > prefSize( float size )
    {
        prefSize( Value.Fixed.valueOf( size ) );

        return this;
    }

    public Cell< T > prefWidth( float prefWidth )
    {
        this.prefWidth = Value.Fixed.valueOf( prefWidth );

        return this;
    }

    public Cell< T > prefHeight( float prefHeight )
    {
        this.prefHeight = Value.Fixed.valueOf( prefHeight );

        return this;
    }

    /** Sets the maxWidth and maxHeight to the specified value. If the max size is 0, no maximum size is used. */
    public Cell< T > maxSize( Value size )
    {
        if ( size == null ) throw new ArgumentException( "size cannot be null." );
        maxWidth  = size;
        maxHeight = size;

        return this;
    }

    /** Sets the maxWidth and maxHeight to the specified values. If the max size is 0, no maximum size is used. */
    public Cell< T > maxSize( Value width, Value height )
    {
        if ( width == null ) throw new ArgumentException( "width cannot be null." );
        if ( height == null ) throw new ArgumentException( "height cannot be null." );
        maxWidth  = width;
        maxHeight = height;

        return this;
    }

    /** If the maxWidth is 0, no maximum width is used. */
    public Cell< T > maxWidth( Value maxWidth )
    {
        if ( maxWidth == null ) throw new ArgumentException( "maxWidth cannot be null." );
        this.maxWidth = maxWidth;

        return this;
    }

    /** If the maxHeight is 0, no maximum height is used. */
    public Cell< T > maxHeight( Value maxHeight )
    {
        if ( maxHeight == null ) throw new ArgumentException( "maxHeight cannot be null." );
        this.maxHeight = maxHeight;

        return this;
    }

    /** Sets the maxWidth and maxHeight to the specified value. If the max size is 0, no maximum size is used. */
    public Cell< T > maxSize( float size )
    {
        maxSize( Value.Fixed.valueOf( size ) );

        return this;
    }

    /** Sets the maxWidth and maxHeight to the specified values. If the max size is 0, no maximum size is used. */
    public Cell< T > maxSize( float width, float height )
    {
        maxSize( Value.Fixed.valueOf( width ), Value.Fixed.valueOf( height ) );

        return this;
    }

    /** If the maxWidth is 0, no maximum width is used. */
    public Cell< T > maxWidth( float maxWidth )
    {
        this.maxWidth = Value.Fixed.valueOf( maxWidth );

        return this;
    }

    /** If the maxHeight is 0, no maximum height is used. */
    public Cell< T > maxHeight( float maxHeight )
    {
        this.maxHeight = Value.Fixed.valueOf( maxHeight );

        return this;
    }

    /** Sets the spaceTop, spaceLeft, spaceBottom, and spaceRight to the specified value. */
    public Cell< T > space( Value space )
    {
        if ( space == null ) throw new ArgumentException( "space cannot be null." );
        spaceTop    = space;
        spaceLeft   = space;
        spaceBottom = space;
        spaceRight  = space;

        return this;
    }

    public Cell< T > space( Value top, Value left, Value bottom, Value right )
    {
        if ( top == null ) throw new ArgumentException( "top cannot be null." );
        if ( left == null ) throw new ArgumentException( "left cannot be null." );
        if ( bottom == null ) throw new ArgumentException( "bottom cannot be null." );
        if ( right == null ) throw new ArgumentException( "right cannot be null." );
        spaceTop    = top;
        spaceLeft   = left;
        spaceBottom = bottom;
        spaceRight  = right;

        return this;
    }

    public Cell< T > spaceTop( Value spaceTop )
    {
        if ( spaceTop == null ) throw new ArgumentException( "spaceTop cannot be null." );
        this.spaceTop = spaceTop;

        return this;
    }

    public Cell< T > spaceLeft( Value spaceLeft )
    {
        if ( spaceLeft == null ) throw new ArgumentException( "spaceLeft cannot be null." );
        this.spaceLeft = spaceLeft;

        return this;
    }

    public Cell< T > spaceBottom( Value spaceBottom )
    {
        if ( spaceBottom == null ) throw new ArgumentException( "spaceBottom cannot be null." );
        this.spaceBottom = spaceBottom;

        return this;
    }

    public Cell< T > spaceRight( Value spaceRight )
    {
        if ( spaceRight == null ) throw new ArgumentException( "spaceRight cannot be null." );
        this.spaceRight = spaceRight;

        return this;
    }

    /** Sets the spaceTop, spaceLeft, spaceBottom, and spaceRight to the specified value. The space cannot be < 0. */
    public Cell< T > Space( float space )
    {
        if ( space < 0 ) throw new ArgumentException( "space cannot be < 0: " + space );
        Space( Value.Fixed.ValueOf( space ) );

        return this;
    }

    public Cell< T > Space( float top, float left, float bottom, float right )
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

    public Cell< T > SetSpaceTop( float spaceTop )
    {
        if ( spaceTop < 0 ) throw new ArgumentException( "spaceTop cannot be < 0: " + spaceTop );

        this.SpaceTop = Value.Fixed.ValueOf( spaceTop );

        return this;
    }

    public Cell< T > SetSpaceLeft( float spaceLeft )
    {
        if ( spaceLeft < 0 ) throw new ArgumentException( "spaceLeft cannot be < 0: " + spaceLeft );

        this.SpaceLeft = Value.Fixed.ValueOf( spaceLeft );

        return this;
    }

    public Cell< T > SetSpaceBottom( float spaceBottom )
    {
        if ( spaceBottom < 0 ) throw new ArgumentException( "spaceBottom cannot be < 0: " + spaceBottom );

        this.SpaceBottom = Value.Fixed.ValueOf( spaceBottom );

        return this;
    }

    public Cell< T > SetSpaceRight( float spaceRight )
    {
        if ( spaceRight < 0 ) throw new ArgumentException( "spaceRight cannot be < 0: " + spaceRight );

        this.SpaceRight = Value.Fixed.ValueOf( spaceRight );

        return this;
    }

    /// <summary>
    /// Sets the padTop, padLeft, padBottom, and padRight to the specified value.
    /// </summary>
    public Cell< T > Pad( Value pad )
    {
        ArgumentNullException.ThrowIfNull( pad );

        PadTop    = pad;
        PadLeft   = pad;
        PadBottom = pad;
        PadRight  = pad;

        return this;
    }

    public Cell< T > Pad( Value top, Value left, Value bottom, Value right )
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

    public Cell< T > SetPadTop( Value padTop )
    {
        ArgumentNullException.ThrowIfNull( padTop );

        this.PadTop = padTop;

        return this;
    }

    public Cell< T > SetPadBottom( Value padBottom )
    {
        ArgumentNullException.ThrowIfNull( padBottom );
        
        this.PadBottom = padBottom;

        return this;
    }

    public Cell< T > SetPadRight( Value padRight )
    {
        ArgumentNullException.ThrowIfNull( padRight );
        
        this.PadRight = padRight;

        return this;
    }

    public Cell< T > Pad( float pad )
    {
        Pad( Value.Fixed.ValueOf( pad ) );

        return this;
    }

    public Cell< T > Pad( float top, float left, float bottom, float right )
    {
        Pad( Value.Fixed.ValueOf( top ),
             Value.Fixed.ValueOf( left ),
             Value.Fixed.ValueOf( bottom ),
             Value.Fixed.ValueOf( right ) );

        return this;
    }

    public Cell< T > SetPadTop( float padTop )
    {
        this.PadTop = Value.Fixed.ValueOf( padTop );

        return this;
    }

    public Cell< T > SetPadLeft( float padLeft )
    {
        this.PadLeft = Value.Fixed.ValueOf( padLeft );

        return this;
    }

    public Cell< T > SetPadBottom( float padBottom )
    {
        this.PadBottom = Value.Fixed.ValueOf( padBottom );

        return this;
    }

    public Cell< T > SetPadRight( float padRight )
    {
        this.PadRight = Value.Fixed.ValueOf( padRight );

        return this;
    }

    public Cell< T > FillOneF()
    {
        FillX = Onef;
        FillY = Onef;

        return this;
    }

    public Cell< T > FillXOneF()
    {
        FillX = Onef;

        return this;
    }

    public Cell< T > FillYOneF()
    {
        FillY = Onef;

        return this;
    }

    public Cell< T > Fill( float x, float y )
    {
        FillX = x;
        FillY = y;

        return this;
    }

    public Cell< T > Fill( bool x, bool y )
    {
        FillX = x ? Onef : Zerof;
        FillY = y ? Onef : Zerof;

        return this;
    }

    public Cell< T > Fill( bool fill )
    {
        FillX = fill ? Onef : Zerof;
        FillY = fill ? Onef : Zerof;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the actor within the cell. Set to <see cref="Align.Center"/>,
    /// <see cref="Align.Top"/>, <see cref="Align.Bottom"/>, <see cref="Align.Left"/>,
    /// <see cref="Align.Right"/>, or any combination of those. 
    /// </summary>
    public Cell< T > SetAlignment( int align )
    {
        this.Alignment = align;

        return this;
    }

    /// <summary>
    /// Sets the alignment of the actor within the cell to <see cref="Align.Center"/>.
    /// This clears any other alignment.
    /// </summary>
    public Cell< T > Center()
    {
        Alignment = Centeri;

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.Top"/> and clears <see cref="Align.Bottom"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell< T > Top()
    {
        if ( Alignment == null )
        {
            Alignment = Topi;
        }
        else
        {
            Alignment = ( Alignment | Align.Top ) & ~Align.Bottom;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.Left"/> and clears <see cref="Align.Right"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell< T > Left()
    {
        if ( Alignment == null )
        {
            Alignment = Lefti;
        }
        else
        {
            Alignment = ( Alignment | Align.Left ) & ~Align.Right;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.Bottom"/> and clears <see cref="Align.Top"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell< T > Bottom()
    {
        if ( Alignment == null )
        {
            Alignment = Bottomi;
        }
        else
        {
            Alignment = ( Alignment | Align.Bottom ) & ~Align.Top;
        }

        return this;
    }

    /// <summary>
    /// Adds <see cref="Align.Right"/> and clears <see cref="Align.Left"/> for
    /// the alignment of the actor within the cell.
    /// </summary>
    public Cell< T > Right()
    {
        if ( Alignment == null )
        {
            Alignment = Righti;
        }
        else
        {
            Alignment = ( Alignment | Align.Right ) & ~Align.Left;
        }

        return this;
    }

    public Cell< T > Grow()
    {
        ExpandX = Onei;
        ExpandY = Onei;
        FillX   = Onef;
        FillY   = Onef;

        return this;
    }

    public Cell< T > GrowX()
    {
        ExpandX = Onei;
        FillX   = Onef;

        return this;
    }

    public Cell< T > GrowY()
    {
        ExpandY = Onei;
        FillY   = Onef;

        return this;
    }

    public Cell< T > Expand()
    {
        ExpandX = Onei;
        ExpandY = Onei;

        return this;
    }

    public Cell< T > SetExpandX()
    {
        ExpandX = Onei;

        return this;
    }

    public Cell< T > SetExpandY()
    {
        ExpandY = Onei;

        return this;
    }

    public Cell< T > Expand( int x, int y )
    {
        ExpandX = x;
        ExpandY = y;

        return this;
    }

    public Cell< T > Expand( bool x, bool y )
    {
        ExpandX = x ? Onei : Zeroi;
        ExpandY = y ? Onei : Zeroi;

        return this;
    }

    public Cell< T > SetColspan( int colspan )
    {
        this.Colspan = colspan;

        return this;
    }

    public Cell< T > UniformToTrue()
    {
        UniformX = true;
        UniformY = true;

        return this;
    }

    public Cell< T > UniformXToTrue()
    {
        UniformX = true;

        return this;
    }

    public Cell< T > UniformYToTrue()
    {
        UniformY = true;

        return this;
    }

    public Cell< T > Uniform( bool uniform )
    {
        UniformX = uniform;
        UniformY = uniform;

        return this;
    }

    public Cell< T > Uniform( bool x, bool y )
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
    public void  AddRow()         => Table.AddRow();

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
        FillX       = null;
        FillY       = null;
        Alignment   = null;
        ExpandX     = null;
        ExpandY     = null;
        Colspan     = null;
        UniformX    = null;
        UniformY    = null;
    }

    private void Set( Cell< T >? cell )
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

    private void Merge( Cell< T >? cell )
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
        if ( cell.FillX != null )           FillX       = cell.FillX;
        if ( cell.FillY != null )           FillY       = cell.FillY;
        if ( cell.Alignment != null )       Alignment   = cell.Alignment;
        if ( cell.ExpandX != null )         ExpandX     = cell.ExpandX;
        if ( cell.ExpandY != null )         ExpandY     = cell.ExpandY;
        if ( cell.Colspan != null )         Colspan     = cell.Colspan;
        if ( cell.UniformX != null )        UniformX    = cell.UniformX;
        if ( cell.UniformY != null )        UniformY    = cell.UniformY;
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
    public Cell< T >? GetCellDefaults()
    {
        if ( ( _files == null ) || ( _files != Gdx.Files ) )
        {
            _files = Gdx.Files;

            _defaults = new Cell< T >
            {
                MinWidth    = UI.Value.MinWidth,
                MinHeight   = UI.Value.MinHeight,
                PrefWidth   = UI.Value.PrefWidth,
                PrefHeight  = UI.Value.PrefHeight,
                MaxWidth    = UI.Value.MaxWidth,
                MaxHeight   = UI.Value.MaxHeight,
                SpaceTop    = UI.Value.Zero,
                SpaceLeft   = UI.Value.Zero,
                SpaceBottom = UI.Value.Zero,
                SpaceRight  = UI.Value.Zero,
                PadTop      = UI.Value.Zero,
                PadLeft     = UI.Value.Zero,
                PadBottom   = UI.Value.Zero,
                PadRight    = UI.Value.Zero,
                FillX       = Zerof,
                FillY       = Zerof,
                Alignment   = Centeri,
                ExpandX     = Zeroi,
                ExpandY     = Zeroi,
                Colspan     = Onei,
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