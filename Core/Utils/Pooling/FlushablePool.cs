namespace LibGDXSharp.Utils.Pooling;

/// <summary>
/// A <see cref="Pool{T}"/> which keeps track of the obtained items
/// (see <see cref="Obtain()"/>), which can be freed all at once using the
/// <see cref="Flush()"/> method.
/// </summary>
public abstract class FlushablePool<T> : Pool< T >
{
    private readonly List< T > _obtained = new();

    protected FlushablePool()
    {
    }

    protected FlushablePool( int initialCapacity )
        : base( initialCapacity )
    {
    }

    protected FlushablePool( int initialCapacity, int max )
        : base( initialCapacity, max )
    {
    }

    public override T Obtain()
    {
        T result = base.Obtain();
        _obtained.Add( result );

        return result;
    }

    /// <summary>
    /// Frees all obtained instances.
    /// </summary>
    public virtual void Flush()
    {
        base.FreeAll( _obtained );
        _obtained.Clear();
    }

    public override void Free( T obj )
    {
        _obtained.Remove( obj );

        base.Free( obj );
    }

    public override void FreeAll( List< T > objects )
    {
        foreach ( T obj in objects )
        {
            _obtained.Remove( obj );
        }
            
        base.FreeAll( objects );
    }
}