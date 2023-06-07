using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class RepeatAction : DelegateAction
{
    public const int Forever = -1;

    public int  RepeatCount   { get; set; }
    public int  ExecutedCount { get; set; }
    public bool Finished      { get; set; }

    protected override bool Delegate( float delta )
    {
        if ( ExecutedCount == RepeatCount ) return true;

        if ( Action == null ) return false;
        
        if ( Action.Act( delta ) )
        {
            if ( Finished ) return true;
            if ( RepeatCount > 0 ) ExecutedCount++;
            if ( ExecutedCount == RepeatCount ) return true;

            Action?.Restart();
        }

        return false;
    }

    /// <summary>
    /// Causes the action to not repeat again.
    /// </summary>
    public void Finish()
    {
        Finished = true;
    }

    public new void Restart()
    {
        base.Restart();
        
        ExecutedCount = 0;
        Finished      = false;
    }
}