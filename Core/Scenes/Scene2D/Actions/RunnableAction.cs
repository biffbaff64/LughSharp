using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

using System.Threading;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class RunnableAction : Action
{
    private bool _ran;

    public override bool Act( float delta )
    {
        if ( !_ran )
        {
            _ran = true;
            Run();
        }

        return true;
    }

    /// <summary>
    /// Called to run the runnable.
    /// </summary>
    public void Run()
    {
        if ( Runnable == null ) throw new GdxRuntimeException( "Runnable is not initialised!" );
        
        Pool< object >? pool = base.Pool;

        // Ensure this action can't be returned to the pool inside the runnable.
        base.Pool = null;

        try
        {
            var thread = new Thread( Runnable! );
            thread.Start();
        }
        finally
        {
            base.Pool = pool;
        }
    }

    public new void Restart()
    {
        _ran = false;
    }

    public new void Reset()
    {
        base.Reset();
        Runnable = null;
    }

    public ThreadStart? Runnable { get; set; }
}