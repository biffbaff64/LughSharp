// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using LibGDXSharp.Scenes.Listeners;

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// <summary>
/// A listener that shows a tooltip actor when the mouse is over another actor.
/// </summary>
public class Tooltip<T> : InputListener where T : Actor
{
    public Container< T >      Container   { get; set; }
    public TooltipManager< T > Manager     { get; set; }
    public Actor?              TargetActor { get; set; }
    public bool                Instant     { get; set; }
    public bool                Always      { get; set; }

    private readonly Vector2 _tmp = new();

    public Tooltip( T? contents ) : this( contents, new TooltipManager< T >() )
    {
    }

    public Tooltip( T? contents, TooltipManager< T > manager )
    {
        this.Manager             = manager;
        this.Container           = new TooltipContainer( this, contents );
        this.Container.Touchable = Touchable.Disabled;
    }

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

        this.TargetActor = actor;

        Container.Pack();

        var offsetX = Manager.OffsetX;
        var offsetY = Manager.OffsetY;
        var dist    = Manager.EdgeDistance;

        Vector2 point = actor.LocalToStageCoordinates( _tmp.Set( x + offsetX, y - offsetY - Container.Height ) );

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
        if ( ( pointer != -1 ) || ( Gdx.Input.IsTouched() ) )
        {
            return;
        }

        Actor? actor = ev?.ListenerActor;

        if ( ( fromActor != null ) && fromActor.IsDescendantOf( actor ) )
        {
            return;
        }

        SetContainerPosition( actor, x, y );

        this.Manager.Enter( this );
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
        this.Manager.Hide( this );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

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
