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

using Corelib.LibCore.Graphics.G2D;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Maths;
using Corelib.LibCore.Scenes.Scene2D.Utils;
using Corelib.LibCore.Utils.Collections;
using Matrix4 = Corelib.LibCore.Maths.Matrix4;

namespace Corelib.LibCore.Scenes.Scene2D;

/// <summary>
/// 2D scene graph node that may contain other actors.
/// <para>
/// Actors have a z-order equal to the order they were inserted into the group.
/// Actors inserted later will be drawn on top of actors added earlier. Touch
/// events that hit more than one actor are distributed to topmost actors first.
/// </para>
/// </summary>
[PublicAPI]
public class Group : Actor, ICullable
{
    public SnapshotArray< Actor > Children    { get; set; } = new( 4 );
    public RectangleShape?        CullingArea { get; set; }

    /// <summary>
    /// When true (the default), the Batch is transformed so children are drawn
    /// in their parent's coordinate system. This has a performance impact because
    /// <see cref="IBatch.Flush()"/> must be done before and after the transform.
    /// If the actors in a group are not rotated or scaled, then the transform for
    /// the group can be set to false. In this case, each child's position will be
    /// offset by the group's position for drawing, causing the children to appear
    /// in the correct location even though the Batch has not been transformed.
    /// </summary>
    public bool Transform { get; set; } = true;
    
    // ------------------------------------------------------------------------

    private readonly Matrix4 _computedTransform = new();
    private readonly Matrix4 _oldTransform      = new();
    private readonly Vector2 _tmp               = new();
    private readonly Affine2 _worldTransform    = new();

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override void Act( float delta )
    {
        base.Act( delta );

        Actor?[] actors = Children.Begin();

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            actors[ i ]?.Act( delta );
        }

        Children.End();
    }

    /// <summary>
    /// Draws the group and its children.
    /// <para>
    /// The default implementation calls <see cref="ApplyTransform(IBatch, Matrix4)"/> if needed,
    /// then <see cref="DrawChildren(IBatch, float)"/>, then <see cref="ResetTransform(IBatch)"/>
    /// if needed.
    /// </para>
    /// </summary>
    /// <param name="batch"></param>
    /// <param name="parentAlpha"></param>
    public override void Draw( IBatch batch, float parentAlpha )
    {
        if ( Transform )
        {
            ApplyTransform( batch, ComputeTransform() );
        }

        DrawChildren( batch, parentAlpha );

        if ( Transform )
        {
            ResetTransform( batch );
        }
    }

    /// <summary>
    /// Draws all children.
    /// <para>
    /// <see cref="ApplyTransform(IBatch, Matrix4)"/> should be called before and
    /// <see cref="ResetTransform(IBatch)"/> after this method if <see cref="Transform"/>
    /// is true.
    /// </para>
    /// <para>
    /// If <see cref="Transform"/> is false these methods don't need to be called,
    /// children positions are temporarily offset by the group position when drawn.
    /// This method avoids drawing children completely outside the
    /// <see cref="CullingArea"/> culling area, if set.
    /// </para>
    /// </summary>
    protected void DrawChildren( IBatch batch, float parentAlpha )
    {
        parentAlpha *= Color.A;

        Actor?[] actors = Children.Begin();

        if ( CullingArea != null )
        {
            // Draw children only if inside culling area.
            var cullLeft   = CullingArea.X;
            var cullRight  = cullLeft + CullingArea.Width;
            var cullBottom = CullingArea.Y;
            var cullTop    = cullBottom + CullingArea.Height;

            if ( Transform )
            {
                for ( int i = 0, n = Children.Size; i < n; i++ )
                {
                    var child = actors[ i ];

                    if ( child is not { IsVisible: true } )
                    {
                        continue;
                    }

                    var cx = child.X;
                    var cy = child.Y;

                    if ( ( cx <= cullRight )
                      && ( cy <= cullTop )
                      && ( ( cx + child.Width ) >= cullLeft )
                      && ( ( cy + child.Height ) >= cullBottom ) )
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

                for ( int i = 0, n = Children.Size; i < n; i++ )
                {
                    var child = actors[ i ];

                    if ( child is not { IsVisible: true } )
                    {
                        continue;
                    }

                    var cx = child.X;
                    var cy = child.Y;

                    if ( ( cx <= cullRight )
                      && ( cy <= cullTop )
                      && ( ( cx + child.Width ) >= cullLeft )
                      && ( ( cy + child.Height ) >= cullBottom ) )
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
            if ( Transform )
            {
                for ( int i = 0, n = Children.Size; i < n; i++ )
                {
                    var child = actors[ i ];

                    if ( child is not { IsVisible: true } )
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

                for ( int i = 0, n = Children.Size; i < n; i++ )
                {
                    var child = actors[ i ];

                    if ( child is not { IsVisible: true } )
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

        Children.End();
    }

    /// <summary>
    /// Draws this actor's debug lines if <see cref="Actor.DebugActive"/> is
    /// true and, regardless of <see cref="Actor.DebugActive"/>, calls
    /// <see cref="Actor.DrawDebug(ShapeRenderer)"/> on each child.
    /// </summary>
    public override void DrawDebug( ShapeRenderer shapes )
    {
        DrawDebugBounds( shapes );

        if ( Transform )
        {
            ApplyTransform( shapes, ComputeTransform() );
        }

        DrawDebugChildren( shapes );

        if ( Transform )
        {
            ResetTransform( shapes );
        }
    }

    /// <summary>
    /// Draws all children. <see cref="ApplyTransform(IBatch, Matrix4)"/> should be
    /// called before and <see cref="ResetTransform(IBatch)"/> after this method if
    /// <see cref="Transform"/> is true.
    /// <para>
    /// If <see cref="Transform"/> is false these methods don't need to be called,
    /// children positions are temporarily offset by the group position when drawn.
    /// This method avoids drawing children completely outside the
    /// <see cref="CullingArea"/> culling area, if set.
    /// </para>
    /// </summary>
    protected void DrawDebugChildren( ShapeRenderer shapes )
    {
        Actor?[] actors = Children.Begin();

        // No culling, draw all children.
        if ( Transform )
        {
            for ( int i = 0, n = Children.Size; i < n; i++ )
            {
                var child = actors[ i ];

                if ( child == null )
                {
                    continue;
                }

                if ( !child.IsVisible )
                {
                    continue;
                }

                if ( !child.DebugActive && child is not Group )
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
                var child = actors[ i ];

                if ( child == null )
                {
                    continue;
                }

                if ( !child.IsVisible )
                {
                    continue;
                }

                if ( !child.DebugActive && child is not Group )
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
    /// Returns the transform for this group's coordinate system.
    /// </summary>
    protected Matrix4 ComputeTransform()
    {
        _worldTransform.SetToTrnRotScl( X + OriginX, Y + OriginY, Rotation, ScaleX, ScaleY );

        if ( ( OriginX != 0 ) || ( OriginY != 0 ) )
        {
            _worldTransform.Translate( -OriginX, -OriginY );
        }

        // Find the first parent that transforms.
        var parentGroup = Parent;

        while ( parentGroup != null )
        {
            if ( parentGroup.Transform )
            {
                break;
            }

            parentGroup = parentGroup.Parent;
        }

        if ( parentGroup != null )
        {
            _worldTransform.PreMul( parentGroup._worldTransform );
        }

        _computedTransform.Set( _worldTransform );

        return _computedTransform;
    }

    /// <summary>
    /// Set the batch's transformation matrix, often with the result of
    /// <see cref="ComputeTransform()"/>. Note this causes the batch to
    /// be flushed.
    /// <see cref="ResetTransform(IBatch)"/> will restore the transform
    /// to what it was before this call.
    /// </summary>
    protected void ApplyTransform( IBatch batch, Matrix4 transform )
    {
        _oldTransform.Set( batch.TransformMatrix );

        batch.SetTransformMatrix( transform );
    }

    /// <summary>
    /// Restores the batch transform to what it was before
    /// <see cref="ApplyTransform(IBatch, Matrix4)"/>.
    /// Note this causes the batch to be flushed.
    /// </summary>
    protected void ResetTransform( IBatch batch )
    {
        batch.SetTransformMatrix( _oldTransform );
    }

    /// <summary>
    /// Set the shape renderer transformation matrix, often with the result of
    /// <see cref="ComputeTransform()"/>.
    /// <para>
    /// Note this causes the shape renderer to be flushed.
    /// <see cref="ResetTransform(ShapeRenderer)"/> will restore the transform
    /// to what it was before this call.
    /// </para>
    /// </summary>
    protected void ApplyTransform( ShapeRenderer shapes, Matrix4 transform )
    {
        _oldTransform.Set( shapes.TransformMatrix );

        shapes.TransformMatrix = transform;
        shapes.Flush();
    }

    /// <summary>
    /// Restores the shape renderer transform to what it was before
    /// <see cref="ApplyTransform(IBatch, Matrix4)"/>.
    /// Note this causes the shape renderer to be flushed.
    /// </summary>
    protected void ResetTransform( ShapeRenderer shapes )
    {
        shapes.TransformMatrix = _oldTransform;
    }

    /// <summary>
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="touchable"></param>
    /// <returns></returns>
    public override Actor? Hit( float x, float y, bool touchable )
    {
        if ( touchable && ( Touchable == Touchable.Disabled ) )
        {
            return null;
        }

        if ( !IsVisible )
        {
            return null;
        }

        for ( var i = Children.Size - 1; i >= 0; i-- )
        {
            var child = Children.GetAt( i );

            child.ParentToLocalCoordinates( _tmp.Set( x, y ) );

            var hit = child.Hit( _tmp.X, _tmp.Y, touchable );

            if ( hit != null )
            {
                return hit;
            }
        }

        return base.Hit( x, y, touchable );
    }

    /// <summary>
    /// Called when actors are added to or removed from the group.
    /// </summary>
    protected virtual void ChildrenChanged()
    {
    }

    /// <summary>
    /// Adds an actor as a child of this group, removing it from its previous
    /// parent. If the actor is already a child of this group, no changes are made.
    /// </summary>
    public virtual void AddActor( Actor actor )
    {
        if ( actor.Parent != null )
        {
            if ( actor.Parent == this )
            {
                return;
            }

            actor.Parent.RemoveActor( actor, false );
        }

        Children.Add( actor );

        actor.Parent = this;
        actor.Stage  = Stage;

        ChildrenChanged();
    }

    /// <summary>
    /// Adds an actor as a child of this group at a specific index, removing
    /// it from its previous parent. If the actor is already a child of this
    /// group, no changes are made.
    /// </summary>
    /// <param name="index"> May be greater than the number of children. </param>
    /// <param name="actor"></param>
    public virtual void AddActorAt( int index, Actor actor )
    {
        if ( actor.Parent != null )
        {
            if ( actor.Parent == this )
            {
                return;
            }

            actor.Parent.RemoveActor( actor, false );
        }

        if ( index >= Children.Size )
        {
            Children.Add( actor );
        }
        else
        {
            Children.Insert( index, actor );
        }

        actor.Parent = this;
        actor.Stage  = Stage;

        ChildrenChanged();
    }

    /// <summary>
    /// Adds an actor as a child of this group immediately before another child
    /// actor, removing it from its previous parent. If the actor is already a
    /// child of this group, no changes are made.
    /// </summary>
    public virtual void AddActorBefore( Actor actorBefore, Actor actor )
    {
        if ( actor.Parent != null )
        {
            if ( actor.Parent == this )
            {
                return;
            }

            actor.Parent.RemoveActor( actor, false );
        }

        var index = Children.IndexOf( actorBefore );

        Children.Insert( index, actor );

        actor.Parent = this;
        actor.Stage  = Stage;

        ChildrenChanged();
    }

    /// <summary>
    /// Adds an actor as a child of this group immediately after another child actor,
    /// removing it from its previous parent. If the actor is already a child of this
    /// group, no changes are made. If <tt>actorAfter</tt> is not in this group, the
    /// actor is added as the last child.
    /// </summary>
    public virtual void AddActorAfter( Actor actorAfter, Actor actor )
    {
        if ( actor.Parent != null )
        {
            if ( actor.Parent == this )
            {
                return;
            }

            actor.Parent.RemoveActor( actor, false );
        }

        var index = Children.IndexOf( actorAfter );

        if ( ( index == Children.Size ) || ( index == -1 ) )
        {
            Children.Add( actor );
        }
        else
        {
            Children.Insert( index + 1, actor );
        }

        actor.Parent = this;
        actor.Stage  = Stage;

        ChildrenChanged();
    }

    /// <summary>
    /// Removes an actor from this group.
    /// <param name="actor"> The actor to remove. </param>
    /// <param name="unfocus"> Unfocuses the actor if true. </param>
    /// </summary>
    public virtual bool RemoveActor( Actor actor, bool unfocus )
    {
        var index = Children.IndexOf( actor );

        if ( index == -1 )
        {
            return false;
        }

        RemoveActorAt( index, unfocus );

        return true;
    }

    /// <summary>
    /// Removes an actor from this group. If the actor will not be used again and
    /// has actions, they should be <see cref="Actor.ClearActions()"/>  cleared so
    /// the actions will be returned to their <see cref="Action.Pool"/>, if
    /// any. This is not done automatically.
    /// </summary>
    /// <param name="index"> The group index of the actor to remove. </param>
    /// <param name="unfocus"> Unfocuses the actor if true. </param>
    /// <returns> The actor removed from this group. </returns>
    public virtual Actor RemoveActorAt( int index, bool unfocus )
    {
        var actor = Children.RemoveAt( index );

        if ( unfocus )
        {
            Stage?.Unfocus( actor );
        }

        actor.Parent = null;
        actor.Stage  = null;

        ChildrenChanged();

        return actor;
    }

    /// <summary>
    /// Removes all actors from this group.
    /// </summary>
    public virtual void ClearChildren()
    {
        Actor?[] actors = Children.Begin();

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            actors[ i ]!.Stage  = null;
            actors[ i ]!.Parent = null;
        }

        Children.End();
        Children.Clear();

        ChildrenChanged();
    }

    /// <summary>
    /// Removes all children, actions, and listeners from this group.
    /// </summary>
    public override void Clear()
    {
        base.Clear();
        ClearChildren();
    }

    /// <summary>
    /// Returns the first actor found with the specified name. Note this recursively
    /// compares the name of every actor in the group.
    /// </summary>
    public T? FindActor< T >( string name ) where T : Actor
    {
        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            if ( name.Equals( Children.GetAt( i ).Name ) )
            {
                return ( T ) Children.GetAt( i );
            }
        }

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            var child = Children.GetAt( i );

            if ( child is Group group )
            {
                Actor? actor = group.FindActor< T >( name );

                if ( actor != null )
                {
                    return ( T ) actor;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Sets the stage for this groups actors to act upon.
    /// </summary>
    public virtual void SetStage( Stage? stage )
    {
        Stage = stage;

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            Children.GetAt( i ).Stage = stage; // StackOverflowError here means the group is its own ascendant.
        }
    }

    /// <summary>
    /// Swaps two actors by index. Returns false if the swap did not
    /// occur because the indexes were out of bounds.
    /// </summary>
    public bool SwapActor( int first, int second )
    {
        var maxIndex = Children.Size;

        if ( ( first < 0 ) || ( first >= maxIndex ) )
        {
            return false;
        }

        if ( ( second < 0 ) || ( second >= maxIndex ) )
        {
            return false;
        }

        Children.Swap( first, second );

        return true;
    }

    /// <summary>
    /// Swaps two actors. Returns false if the swap did not occur
    /// because the actors are not children of this group.
    /// </summary>
    public bool SwapActor( Actor first, Actor second )
    {
        var firstIndex  = Children.IndexOf( first );
        var secondIndex = Children.IndexOf( second );

        if ( ( firstIndex == -1 ) || ( secondIndex == -1 ) )
        {
            return false;
        }

        Children.Swap( firstIndex, secondIndex );

        return true;
    }

    /// <summary>
    /// Returns the child at the specified index.
    /// </summary>
    public Actor GetChild( int index )
    {
        return Children.GetAt( index );
    }

    /// <summary>
    /// Returns true if this group has children.
    /// </summary>
    public bool HasChildren()
    {
        return Children.Size > 0;
    }

    /// <summary>
    /// Converts coordinates for this group to those of a descendant actor.
    /// The descendant does not need to be a direct child.
    /// </summary>
    /// <exception cref="ArgumentException">
    /// if the specified actor is not a descendant of this group.
    /// </exception>
    public Vector2 LocalToDescendantCoordinates( Actor descendant, Vector2 localCoords )
    {
        var parent = descendant.Parent;

        if ( parent == null )
        {
            throw new ArgumentException( $"Child is not a descendant: {descendant}" );
        }

        // First convert to the actor's parent coordinates.
        if ( parent != this )
        {
            LocalToDescendantCoordinates( parent, localCoords );
        }

        // Then from each parent down to the descendant.
        descendant.ParentToLocalCoordinates( localCoords );

        return localCoords;
    }

    /// <summary>
    /// If true, <see cref="DrawDebug(ShapeRenderer)"/> will be called for this
    /// group and, optionally, all children recursively.
    /// </summary>
    public void SetDebug( bool enabled, bool recursively )
    {
        DebugActive = enabled;

        if ( recursively )
        {
            foreach ( var child in Children )
            {
                if ( child is Group group )
                {
                    group.SetDebug( enabled, recursively );
                }
                else
                {
                    child.DebugActive = enabled;
                }
            }
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual Group DebugAll()
    {
        SetDebug( true, true );

        return this;
    }

    /// <summary>
    /// Returns a description of the actor hierarchy, recursively.
    /// </summary>
    public override string ToString()
    {
        var buffer = new StringBuilder( 128 );

        ToString( buffer, 1 );

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

        Actor?[] actors = Children.Begin();

        for ( int i = 0, n = Children.Size; i < n; i++ )
        {
            for ( var ii = 0; ii < indent; ii++ )
            {
                buffer.Append( "|  " );
            }

            if ( actors[ i ] is Group group )
            {
                group.ToString( buffer, indent + 1 );
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
