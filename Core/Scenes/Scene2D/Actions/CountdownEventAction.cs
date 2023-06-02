namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class CountdownEventAction<T> : EventAction<T> where T : Event
{
    private readonly int _count;
    private int _current;

    public CountdownEventAction ( T eventClass, int count)
        : base( eventClass )
    {
        this._count = count;
    }

    public bool Handle(T ev)
    {
        _current++;
        return _current >= _count;
    }
}