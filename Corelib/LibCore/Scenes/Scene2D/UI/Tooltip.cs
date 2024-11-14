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

using Corelib.LibCore.Scenes.Scene2D.Listeners;

namespace Corelib.LibCore.Scenes.Scene2D.UI;

/// <summary>
/// A listener that shows a tooltip actor when the mouse is over another actor.
/// </summary>
[PublicAPI]
public class Tooltip< T > : InputListener where T : Actor
{
    private readonly Vector2 _tmp = new();

    // ========================================================================

    public Tooltip( T? contents ) : this( contents, new TooltipManager< T >() )
    {
    }

    public Tooltip( T? contents, TooltipManager< T > manager )
    {
        Manager             = manager;
        Container           = new TooltipContainer( this, contents );
        Container.Touchable = Touchable.Disabled;
    }

    public Container< T >      Container   { get; set; }
    public TooltipManager< T > Manager     { get; set; }
    public Actor?              TargetActor { get; set; }
    public bool                Instant     { get; set; }
    public bool                Always      { get; set; }

    public Actor? Actor
    {
        get => Container.GetActor();
        set => Container.SetActor( value as T );
    }

    public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( Instant )
        {
            Container.ToFront();

            return false;
        }

        Manager.TouchDown( this );

        return false;
    }

    public override bool MouseMoved( InputEvent? ev, float x, float y )
    {
        if ( Container.HasParent() )
        {
            return false;
        }

        SetContainerPosition( ev?.ListenerActor, x, y );

        return true;
    }

    private void SetContainerPosition( Actor? actor, float x, float y )
    {
        if ( actor?.Stage == null )
        {
            return;
        }

        TargetActor = actor;

        Container.Pack();

        var offsetX = Manager.OffsetX;
        var offsetY = Manager.OffsetY;
        var dist    = Manager.EdgeDistance;

        var point = actor.LocalToStageCoordinates( _tmp.Set( x + offsetX, y - offsetY - Container.Height ) );

        if ( point.Y < dist )
        {
            point = actor.LocalToStageCoordinates( _tmp.Set( x + offsetX, y + offsetY ) );
        }

        if ( point.X < dist )
        {
            point.X = dist;
        }

        if ( ( point.X + Container.Width ) > ( actor.Stage.Width - dist ) )
        {
            point.X = actor.Stage.Width - dist - Container.Width;
        }

        if ( ( point.Y + Container.Height ) > ( actor.Stage.Height - dist ) )
        {
            point.Y = actor.Stage.Height - dist - Container.Height;
        }

        Container.SetPosition( point.X, point.Y );

        point = actor.LocalToStageCoordinates( _tmp.Set( actor.Width / 2, actor.Height / 2 ) );
        point.Sub( Container.X, Container.Y );

        Container.SetOrigin( point.X, point.Y );
    }

    public override void Enter( InputEvent? ev, float x, float y, int pointer, Actor? fromActor )
    {
        if ( ( pointer != -1 ) || Gdx.Input.IsTouched() )
        {
            return;
        }

        var actor = ev?.ListenerActor;

        if ( ( fromActor != null ) && fromActor.IsDescendantOf( actor ) )
        {
            return;
        }

        SetContainerPosition( actor, x, y );

        Manager.Enter( this );
    }

    public override void Exit( InputEvent? ev, float x, float y, int pointer, Actor? toActor )
    {
        if ( ( toActor != null ) && toActor.IsDescendantOf( ev?.ListenerActor ) )
        {
            return;
        }

        Hide();
    }

    public void Hide()
    {
        Manager.Hide( this );
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class TooltipContainer : Container< T >
    {
        private readonly Tooltip< T > _parent;

        public TooltipContainer( Tooltip< T > parent, T? contents )
            : base( contents )
        {
            _parent = parent;
        }

        public override void Act( float delta )
        {
            base.Act( delta );

            if ( _parent.TargetActor is { Stage: null } )
            {
                Remove();
            }
        }
    }
}
