using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class AfterAction : DelegateAction
{
    private readonly List< Action > _waitForActions = new(4);

    public void SetTarget( Actor? target )
    {
        if ( target != null )
        {
            _waitForActions.AddAll( target.Actions );
        }

        base.Target = target;
    }

    public new void Restart()
    {
        base.Restart();

        _waitForActions.Clear();
    }

    protected override bool Delegate( float delta )
    {
        List< Action >? currentActions = Target?.Actions;

        if ( currentActions?.Count == 1 )
        {
            _waitForActions.Clear();
        }

        for ( var i = _waitForActions.Count - 1; i >= 0; i-- )
        {
            Action action = _waitForActions[ i ];

            if ( currentActions?.IndexOf( action ) == -1 )
            {
                _waitForActions.RemoveAt( i );
            }
        }

        return ( _waitForActions.Count <= 0 ) && Action!.Act( delta );
    }
}
