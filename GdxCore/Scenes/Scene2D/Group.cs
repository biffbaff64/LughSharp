using System.Numerics;

using LibGDXSharp.G2D;
using LibGDXSharp.Maths;
using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Scenes.Scene2D;

/// <summary>
/// 2D scene graph node that may contain other actors.
/// <p>
/// Actors have a z-order equal to the order they were inserted into the group.
/// Actors inserted later will be drawn on top of actors added earlier.
/// Touch events that hit more than one actor are distributed to topmost actors
/// first.
/// </p>
/// </summary>
public class Group : Actor, ICullable
{
    public SnapshotArray< Actor > Children { get; set; } = new SnapshotArray< Actor >( true, 4 );

    private readonly Affine2         _worldTransform    = new Affine2();
    private readonly Matrix4x4       _computedTransform = new Matrix4x4();
    private readonly Matrix4x4       _oldTransform      = new Matrix4x4();
    private          bool            _transform         = true;
    private          RectangleShape? _cullingArea;

    /// <summary>
    /// </summary>
    /// <param name="delta"></param>
    public new void Act( float delta )
    {
        base.Act( delta );

        var actors = Children.Begin();

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            actors[ i ].Act( delta );
        }

        Children.End();
    }

    /// <summary>
    /// Draws the group and its children.
    /// <p>
    /// The default implementation calls <see cref="ApplyTransform(IBatch, Matrix4)"/> if needed,
    /// then <see cref="DrawChildren(IBatch, float)"/>, then <see cref="ResetTransform(IBatch)"/>
    /// if needed.
    /// </p>
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="parentAlpha"></param>
    public new void Draw( IBatch batch, float parentAlpha )
    {
        if ( _transform )
        {
            ApplyTransform( batch, ComputeTransform() );
        }

        DrawChildren( batch, parentAlpha );

        if ( _transform )
        {
            ResetTransform( batch );
        }
    }

    /// <summary>
    /// Draws all children.
    /// <p>
    /// <see cref="ApplyTransform(IBatch, Matrix4)"/> should be called before and
    /// <see cref="ResetTransform(IBatch)"/> after this method if <see cref="SetTransform(bool)"/>
    /// transform is true.
    /// </p>
    /// <p>
    /// If <see cref="SetTransform(bool)"/> transform is false these methods don't need
    /// to be called, children positions are temporarily offset by the group position when
    /// drawn. This method avoids drawing children completely outside the
    /// <see cref="SetCullingArea(RectangleShape)"/> culling area, if set.
    /// </p>
    /// </summary>
    protected void DrawChildren( IBatch batch, float parentAlpha )
    {
        parentAlpha *= this.Color.A;

        var children = this.Children;
        var actors   = children.Begin();

        if ( this._cullingArea != null )
        {
            // Draw children only if inside culling area.
            var cullLeft   = this._cullingArea.X;
            var cullRight  = cullLeft + this._cullingArea.Width;
            var cullBottom = this._cullingArea.Y;
            var cullTop    = cullBottom + this._cullingArea.Height;

            if ( _transform )
            {
                for ( int i = 0, n = children.Size; i < n; i++ )
                {
                    Actor child = actors[ i ];

                    if ( !child.IsVisible() )
                    {
                        continue;
                    }

                    var cx = child.X;
                    var cy = child.Y;

                    if ( cx <= cullRight
                         && cy <= cullTop
                         && cx + child.Width >= cullLeft
                         && cy + child.Height >= cullBottom )
                    {
                        child.Draw( batch, parentAlpha );
                    }
                }
            }
            else
            {
                // No transform for this group, offset each child.
                var offsetX = X;
                var offsetY = Y;

                X = 0;
                Y = 0;

                for ( int i = 0, n = children.Size; i < n; i++ )
                {
                    Actor child = actors[ i ];

                    if ( !child.IsVisible() )
                    {
                        continue;
                    }

                    float cx = child.X;
                    float cy = child.Y;

                    if ( cx <= cullRight
                         && cy <= cullTop
                         && cx + child.Width >= cullLeft
                         && cy + child.Height >= cullBottom )
                    {
                        child.X = cx + offsetX;
                        child.Y = cy + offsetY;
                        child.Draw( batch, parentAlpha );
                        child.X = cx;
                        child.Y = cy;
                    }
                }

                X = offsetX;
                Y = offsetY;
            }
        }
        else
        {
            // No culling, draw all children.
            if ( _transform )
            {
                for ( int i = 0, n = children.Size; i < n; i++ )
                {
                    Actor child = actors[ i ];

                    if ( !child.IsVisible() )
                    {
                        continue;
                    }

                    child.Draw( batch, parentAlpha );
                }
            }
            else
            {
                // No transform for this group, offset each child.
                var offsetX = X;
                var offsetY = Y;

                X = 0;
                Y = 0;

                for ( int i = 0, n = children.Size; i < n; i++ )
                {
                    Actor child = actors[ i ];

                    if ( !child.IsVisible() )
                    {
                        continue;
                    }

                    var cx = child.X;
                    var cy = child.Y;

                    child.X = cx + offsetX;
                    child.Y = cy + offsetY;
                    child.Draw( batch, parentAlpha );
                    child.X = cx;
                    child.Y = cy;
                }

                X = offsetX;
                Y = offsetY;
            }
        }

        children.End();
    }

    /// <summary>
    /// Draws this actor's debug lines if <see cref="DebugFlag"/> is true and,
    /// regardless of <see cref="DebugFlag"/>, calls
    /// <see cref="Actor.DrawDebug(ShapeRenderer)"/> on each child. 
    /// </summary>
    public new void DrawDebug( ShapeRenderer shapes )
    {
        DrawDebugBounds( shapes );

        if ( _transform )
        {
            ApplyTransform( shapes, ComputeTransform() );
        }

        DrawDebugChildren( shapes );

        if ( _transform )
        {
            ResetTransform( shapes );
        }
    }

    /// <summary>
    /// Draws all children. <seealso cref="ApplyTransform(IBatch, Matrix4X4)"/> should be
    /// called before and <seealso cref="resetTransform(IBatch)"/> after this method if
    /// <seealso cref="SetTransform(bool)"/> transform is true.
    /// <p>
    /// If <seealso cref="SetTransform"/> transform is false these methods don't
    /// need to be called, children positions are temporarily offset by the group position
    /// when drawn. This method avoids drawing children completely outside the
    /// <seealso cref="SetCullingArea(Rectangle)"/> culling area, if set. 
    /// </p>
    /// </summary>
    protected void DrawDebugChildren( ShapeRenderer shapes )
    {
        Actor[] actors = Children.Begin();

        // No culling, draw all children.
        if ( _transform )
        {
            for ( int i = 0, n = Children.Size; i < n; i++ )
            {
                Actor child = actors[ i ];

                if ( !child.IsVisible() )
                {
                    continue;
                }

                if ( !child.DebugFlag && !( child is Group ) )
                {
                    continue;
                }

                child.DrawDebug( shapes );
            }

            shapes.Flush();
        }
        else
        {
            // No transform for this group, offset each child.
            var offsetX = X;
            var offsetY = Y;

            X = 0;
            Y = 0;

            for ( int i = 0, n = Children.Size; i < n; i++ )
            {
                Actor child = actors[ i ];

                if ( !child.IsVisible() )
                {
                    continue;
                }

                if ( !child.DebugFlag && !( child is Group ) )
                {
                    continue;
                }

                var cx = child.X;
                var cy = child.Y;

                child.X = cx + offsetX;
                child.Y = cy + offsetY;
                child.DrawDebug( shapes );
                child.X = cx;
                child.Y = cy;
            }

            X = offsetX;
            Y = offsetY;
        }

        Children.End();
    }

    /// <summary>
    /// Returns the transform for this group's coordinate system. </summary>
    protected Matrix4x4 ComputeTransform()
    {
        _worldTransform.SetToTrnRotScl( X + OriginX, Y + OriginY, Rotation, scaleX, scaleY );

        if ( originX != 0 || originY != 0 )
        {
            worldTransform.translate( -originX, -originY );
        }

        // Find the first parent that transforms.
        Group parentGroup = parent;

        while ( parentGroup != null )
        {
            if ( parentGroup.transform )
            {
                break;
            }

            parentGroup = parentGroup.parent;
        }

        if ( parentGroup != null )
        {
            worldTransform.preMul( parentGroup.worldTransform );
        }

        _computedTransform.Set( worldTransform );

        return _computedTransform;
    }

    /// <summary>
    /// Set the batch's transformation matrix, often with the result of
    /// <seealso cref="ComputeTransform()"/>. Note this causes the batch to
    /// be flushed.
    /// <seealso cref="ResetTransform(IBatch)"/> will restore the transform
    /// to what it was before this call. 
    /// </summary>
    protected void ApplyTransform( IBatch batch, Matrix4x4 transform )
    {
        _oldTransform.Set( batch.GetTransformMatrix() );

        batch.SetTransformMatrix( transform );
    }

    /// <summary>
    /// Restores the batch transform to what it was before <seealso cref="applyTransform(Batch, Matrix4)"/>. Note this causes the batch to
    /// be flushed. 
    /// </summary>
    protected void ResetTransform( IBatch batch )
    {
        batch.SetTransformMatrix( _oldTransform );
    }

    /// <summary>
    /// Set the shape renderer transformation matrix, often with the result of
    /// <seealso cref="ComputeTransform()"/>.
    /// <p>
    /// Note this causes the shape renderer to be flushed.
    /// <seealso cref="ResetTransform(ShapeRenderer)"/> will restore the transform
    /// to what it was before this call. 
    /// </p>
    /// </summary>
    protected void ApplyTransform( ShapeRenderer shapes, Matrix4x4 transform )
    {
        _oldTransform.Set( shapes.GetTransformMatrix() );

        shapes.SetTransformMatrix( transform );
        shapes.Flush();
    }

    /// <summary>
    /// Restores the shape renderer transform to what it was before <seealso cref="applyTransform(Batch, Matrix4)"/>. Note this causes the
    /// shape renderer to be flushed. 
    /// </summary>
    protected void ResetTransform( ShapeRenderer shapes )
    {
        shapes.SetTransformMatrix( _oldTransform );
    }

    /** Children completely outside of this rectangle will not be drawn. This is only valid for use with unrotated and unscaled
	 * actors.
	 * @param cullingArea May be null. */
    public void SetCullingArea( @Null rectangle cullingArea )
    {
        this.cullingArea = cullingArea;
    }

    /** @return May be null.
	 * @see #setCullingArea(Rectangle) */
    public @Null Rectangle getCullingArea()
    {
        return cullingArea;
    }

    public @Null Actor hit( float x, float y, bool touchable )
    {
        if ( touchable && GetTouchable() == Touchable.disabled ) return null;
        if ( !IsVisible() ) return null;
        Vector2 point         = tmp;
        Actor[] childrenArray = children.items;

        for ( int i = children.size - 1; i >= 0; i-- )
        {
            Actor child = childrenArray[ i ];
            child.parentToLocalCoordinates( point.set( x, y ) );
            Actor hit = child.hit( point.x, point.y, touchable );

            if ( hit != null ) return hit;
        }

        return super.hit( x, y, touchable );
    }

    /** Called when actors are added to or removed from the group. */
    protected void ChildrenChanged()
    {
    }

    /** Adds an actor as a child of this group, removing it from its previous parent. If the actor is already a child of this
	 * group, no changes are made. */
    public void AddActor( Actor actor )
    {
        if ( actor.parent != null )
        {
            if ( actor.parent == this ) return;
            actor.parent.removeActor( actor, false );
        }

        children.add( actor );
        actor.setParent( this );
        actor.setStage( getStage() );
        ChildrenChanged();
    }

    /** Adds an actor as a child of this group at a specific index, removing it from its previous parent. If the actor is already a
	 * child of this group, no changes are made.
	 * @param index May be greater than the number of children. */
    public void AddActorAt( int index, Actor actor )
    {
        if ( actor.parent != null )
        {
            if ( actor.parent == this ) return;
            actor.parent.removeActor( actor, false );
        }

        if ( index >= children.size )
            children.add( actor );
        else
            children.insert( index, actor );

        actor.setParent( this );
        actor.setStage( getStage() );
        ChildrenChanged();
    }

    /** Adds an actor as a child of this group immediately before another child actor, removing it from its previous parent. If the
	 * actor is already a child of this group, no changes are made. */
    public void AddActorBefore( Actor actorBefore, Actor actor )
    {
        if ( actor.parent != null )
        {
            if ( actor.parent == this ) return;
            actor.parent.removeActor( actor, false );
        }

        int index = children.indexOf( actorBefore, true );
        children.insert( index, actor );
        actor.setParent( this );
        actor.setStage( getStage() );
        ChildrenChanged();
    }

    /** Adds an actor as a child of this group immediately after another child actor, removing it from its previous parent. If the
	 * actor is already a child of this group, no changes are made. If <code>actorAfter</code> is not in this group, the actor is
	 * added as the last child. */
    public void AddActorAfter( Actor actorAfter, Actor actor )
    {
        if ( actor.parent != null )
        {
            if ( actor.parent == this ) return;
            actor.parent.removeActor( actor, false );
        }

        int index = children.indexOf( actorAfter, true );

        if ( index == children.size || index == -1 )
            children.add( actor );
        else
            children.insert( index + 1, actor );

        actor.setParent( this );
        actor.setStage( getStage() );
        ChildrenChanged();
    }

    /** Removes an actor from this group and unfocuses it. Calls {@link #removeActor(Actor, bool)} with true. */
    public bool RemoveActor( Actor actor )
    {
        return RemoveActor( actor, true );
    }

    /** Removes an actor from this group. Calls {@link #removeActorAt(int, bool)} with the actor's child index. */
    public bool RemoveActor( Actor actor, bool unfocus )
    {
        int index = children.indexOf( actor, true );

        if ( index == -1 ) return false;
        RemoveActorAt( index, unfocus );

        return true;
    }

    /** Removes an actor from this group. If the actor will not be used again and has actions, they should be
	 * {@link Actor#clearActions() cleared} so the actions will be returned to their
	 * {@link Action#setPool(com.badlogic.gdx.utils.Pool) pool}, if any. This is not done automatically.
	 * @param unfocus If true, {@link Stage#unfocus(Actor)} is called.
	 * @return the actor removed from this group. */
    public Actor RemoveActorAt( int index, bool unfocus )
    {
        Actor actor = children.removeIndex( index );

        if ( unfocus )
        {
            Stage stage = getStage();
            if ( stage != null ) stage.unfocus( actor );
        }

        actor.setParent( null );
        actor.setStage( null );
        ChildrenChanged();

        return actor;
    }

    /** Removes all actors from this group. */
    public void ClearChildren()
    {
        Actor[] actors = children.begin();

        for ( int i = 0, n = children.size; i < n; i++ )
        {
            Actor child = actors[ i ];
            child.setStage( null );
            child.setParent( null );
        }

        children.end();
        children.clear();
        ChildrenChanged();
    }

    /** Removes all children, actions, and listeners from this group. */
    public void Clear()
    {
        super.clear();
        ClearChildren();
    }

    /** Returns the first actor found with the specified name. Note this recursively compares the name of every actor in the
	 * group. */
    public @Null< T Extends Actor> T FindActor( string name )
    {
        Array< Actor > children = this.children;

        for ( int i = 0, n = children.size; i < n; i++ )
            if ( name.equals( children.get( i ).getName() ) )
                return ( T )children.get( i );

        for ( int i = 0, n = children.size; i < n; i++ )
        {
            Actor                 child = children.get( i );
            if ( child instanceof group) {
                Actor actor = ( ( Group )child ).FindActor( name );

                if ( actor != null ) return ( T )actor;
            }
        }

        return null;
    }

    protected void SetStage( Stage stage )
    {
        super.setStage( stage );
        Actor[] childrenArray = children.items;

        for ( int i = 0, n = children.size; i < n; i++ )
            childrenArray[ i ].setStage( stage ); // StackOverflowError here means the group is its own ascendant.
    }

    /** Swaps two actors by index. Returns false if the swap did not occur because the indexes were out of bounds. */
    public bool SwapActor( int first, int second )
    {
        int maxIndex = children.size;

        if ( first < 0 || first >= maxIndex ) return false;
        if ( second < 0 || second >= maxIndex ) return false;
        children.swap( first, second );

        return true;
    }

    /** Swaps two actors. Returns false if the swap did not occur because the actors are not children of this group. */
    public bool SwapActor( Actor first, Actor second )
    {
        int firstIndex  = children.indexOf( first, true );
        int secondIndex = children.indexOf( second, true );

        if ( firstIndex == -1 || secondIndex == -1 ) return false;
        children.swap( firstIndex, secondIndex );

        return true;
    }

    /** Returns the child at the specified index. */
    public Actor GetChild( int index )
    {
        return children.get( index );
    }

    /** Returns an ordered list of child actors in this group. */
    public SnapshotArray< Actor > GetChildren()
    {
        return children;
    }

    public bool HasChildren()
    {
        return children.size > 0;
    }

    /** When true (the default), the Batch is transformed so children are drawn in their parent's coordinate system. This has a
	 * performance impact because {@link Batch#flush()} must be done before and after the transform. If the actors in a group are
	 * not rotated or scaled, then the transform for the group can be set to false. In this case, each child's position will be
	 * offset by the group's position for drawing, causing the children to appear in the correct location even though the Batch has
	 * not been transformed. */
    public void SetTransform( bool transform )
    {
        this.transform = transform;
    }

    public bool IsTransform()
    {
        return transform;
    }

    /** Converts coordinates for this group to those of a descendant actor. The descendant does not need to be a direct child.
	 * @throws IllegalArgumentException if the specified actor is not a descendant of this group. */
    public Vector2 LocalToDescendantCoordinates( Actor descendant, Vector2 localCoords )
    {
        Group parent = descendant.parent;

        if ( parent == null ) throw new IllegalArgumentException( "Child is not a descendant: " + descendant );
        // First convert to the actor's parent coordinates.
        if ( parent != this ) LocalToDescendantCoordinates( parent, localCoords );
        // Then from each parent down to the descendant.
        descendant.parentToLocalCoordinates( localCoords );

        return localCoords;
    }

    /** If true, {@link #drawDebug(ShapeRenderer)} will be called for this group and, optionally, all children recursively. */
    public void SetDebug( bool enabled, bool recursively )
    {
        SetDebug( enabled );

        if ( recursively )
        {
            foreach( Actor child in Children)
            {
                if ( child is Group group)
                {
                    group.SetDebug( enabled, recursively );
                }
                else
                {
                    child.SetDebug( enabled );
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Group DebugAll()
    {
        SetDebug( true, true );

        return this;
    }

    /// <summary>
    /// Returns a description of the actor hierarchy, recursively.
    /// </summary>
    public string Tostring()
    {
        var buffer = new StringBuilder( 128 );
            
        ToString( buffer, 1 );
            
        buffer.SetLength( buffer.Length() - 1 );

        return buffer.ToString();
    }

    /// <summary>
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="indent"></param>
    private void ToString( StringBuilder buffer, int indent )
    {
        buffer.Append( base.ToString() ?? string.Empty );
        buffer.Append( '\n' );

        var actors = Children.Begin();

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            for ( var ii = 0; ii < indent; ii++ )
            {
                buffer.Append( "|  " );
            }

            if ( actors[ i ] is Group group )
            {
                group.Tostring( buffer, indent + 1 );
            }
            else
            {
                buffer.Append( actors[ i ] );
                buffer.Append( '\n' );
            }
        }

        Children.End();
    }
}