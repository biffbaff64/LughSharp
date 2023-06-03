namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
/// An EventAction that is complete once it receives X number of events.
/// </summary>
public class CountdownEventAction<T> : EventAction<T> where T : Event
{
    private readonly int _count;
    private int _current;

    public CountdownEventAction ( T eventClass, int count)
        : base( eventClass )
    {
        this._count = count;
    }

    public override bool HandleDelegate( Event ev )
    {
        _current++;
        
        return _current >= _count;
    }
}