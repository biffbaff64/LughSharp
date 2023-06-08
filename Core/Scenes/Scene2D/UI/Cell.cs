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

    public Value  MinWidth    { get; set; }
    public Value  MinHeight   { get; set; }
    public Value  PrefWidth   { get; set; }
    public Value  PrefHeight  { get; set; }
    public Value  MaxWidth    { get; set; }
    public Value  MaxHeight   { get; set; }
    public Value  SpaceTop    { get; set; }
    public Value  SpaceLeft   { get; set; }
    public Value  SpaceBottom { get; set; }
    public Value  SpaceRight  { get; set; }
    public Value  PadTop      { get; set; }
    public Value  PadLeft     { get; set; }
    public Value  PadBottom   { get; set; }
    public Value  PadRight    { get; set; }
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
        
        Cell<T> defaults = GetCellDefaults();
        
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
    public Cell<T> ClearActor()
    {
        SetActor( null );
        
        return this;
    }

    public bool HasActor() => this.Actor != null;
    
    
    // -------------------- From IPoolable.cs --------------------

    /// <summary>
    /// Resets the object for reuse. Object references should
    /// be nulled and fields may be set to default values.
    /// </summary>
    public void Reset()
    {
    }
}