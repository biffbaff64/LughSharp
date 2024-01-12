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
    public Container< T >? Container   { get; set; }
    public TooltipManager  Manager     { get; set; }
    public bool            Instant     { get; set; }
    public bool            Always      { get; set; }
    public Actor           TargetActor { get; set; }

    private Vector2 _tmp = new();
    
    public Tooltip( T? contents ) : this( contents, TooltipManager.Instance )
    {
    }

    public Tooltip( T? contents, TooltipManager manager )
    {
        this.Container           = new TooltipContainer( this );
        this.Container.Touchable = Touchable.Disabled;
    }

    public Actor? Actor
    {
        get => Container!.GetActor();
        set => Container!.SetActor( value as T );
    }

    public override bool TouchDown( InputEvent? ev, float x, float y, int pointer, int button )
    {
        if ( Instant )
        {
            Container?.ToFront();

            return false;
        }

        Manager.TouchDown( this );

        return false;
    }

    public override bool MouseMoved( InputEvent? ev, float x, float y )
    {
        if ( Container!.HasParent() )
        {
            return false;
        }
        
        SetContainerPosition( ev.ListenerActor, x, y);

        return true;
    }

    private void SetContainerPosition( Actor actor, float x, float y )
    {
        this.TargetActor = actor;

        Stage? stage = actor.Stage;

        if ( stage == null )
        {
            return;
        }

        Container?.Pack();
        
        float offsetX = Manager.OffsetX;
        float offsetY = Manager.OffsetY;
        float dist = Manager.EdgeDistance;

        Vector2 point = actor.LocalToStageCoordinates( _tmp.Set( x + offsetX, y - offsetY - Container.Height ) );
        
        if ( point.Y < dist ) point = actor.LocalToStageCoordinates( _tmp.Set( x + offsetX, y + offsetY ) );
        if ( point.X < dist ) point.X = dist;
        if ( point.X + Container.Width > stage.Width - dist ) point.X = stage.Width - dist - Container.Width;
        if ( point.Y + Container.Height > stage.Height - dist ) point.Y = stage.Height - dist - Container.Height;
        
        Container.SetPosition( point.X, point.Y );

        point = actor.LocalToStageCoordinates( _tmp.Set( actor.Width / 2, actor.Height / 2 ) );
        point.Sub( Container.X, Container.Y );
        Container.SetOrigin( point.X, point.Y );
    }

    public void enter( InputEvent event, float x, float y, int pointer, @Null Actor fromActor) {
        if ( pointer != -1 ) return;
        if ( Gdx.input.isTouched() ) return;
        Actor actor =  event.getListenerActor();

        if ( fromActor != null && fromActor.isDescendantOf( actor ) ) return;
        setContainerPosition( actor, x, y );
        manager.enter( this );
    }

    public void exit( InputEvent event, float x, float y, int pointer, @Null Actor toActor) {
        if ( toActor != null && toActor.isDescendantOf( event.getListenerActor())) return;
        hide();
    }

    public void hide()
    {
        manager.hide( this );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class TooltipContainer : Container< T >
    {
        private readonly Tooltip< T > _parent;

        public TooltipContainer( Tooltip< T > parent )
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
