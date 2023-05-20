using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Scenes.Scene2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class TouchFocus
{
    public IEventListener? listener;
    public Actor?          listenerActor;
    public Actor?          target;
    public int             pointer;
    public int             button;

    public void Reset()
    {
        listenerActor = null;
        listener      = null;
        target        = null;
        pointer       = 0;
        button        = 0;
    }
}