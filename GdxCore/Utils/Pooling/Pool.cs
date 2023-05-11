using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Pooling;

namespace LibGDXSharp.Utils;

/// <summary>
/// A pool of objects that can be reused to avoid allocation.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Pool<T>
{
    // The maximum number of objects that will be pooled.
    public int Max { get; set; }

    // The highest number of free objects. Can be reset any time.
    public int Peak { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public Func< T > NewObject { get; init; } = null!;

    private readonly List< T? > _freeObjects;

    /// <summary>
    /// Creates a pool with an initial capacity of 16 and no maximum.
    /// </summary>
    public Pool() : this( 16 )
    {
    }

    /// <summary>
    /// Creates a new pool with a specified initial capacity,
    /// </summary>
    /// <param name="initialCapacity">
    /// The initial size of the array supporting the pool. No objects are created/pre-allocated.
    /// </param>
    /// <param name="max">The maximum number of free objects to store in this pool.</param>
    public Pool( int initialCapacity, int max = int.MaxValue )
    {
        _freeObjects = new List< T? >( initialCapacity );

        this.Max = max;
    }

    /// <summary>
    /// Returns an object from this pool.
    /// The object may be new (from <see cref="NewObject"/>) or reused
    /// (previously <see cref="Free(T)"/> freed).
    /// </summary>
    public virtual T Obtain()
    {
        if ( _freeObjects.Count == 0 ) return NewObject();

        T? item = _freeObjects[ ^1 ];

        _freeObjects[ ^1 ] = default;

        return item!;
    }

    /// <summary>
    /// Puts the specified object in the pool, making it eligible to be returned by
    /// <see cref="Obtain"/>. If the pool already contains <see cref="Max"/> free objects,
    /// the specified object is discarded using <see cref="Discard"/> and not added to the pool.
    /// The pool does not check if an object is already freed, so the same object must not be
    /// freed multiple times.
    /// </summary>
    /// <param name="obj">The object to add to the pool.</param>
    /// <exception cref="ArgumentException"></exception>
    public virtual void Free( T obj )
    {
        if ( obj == null ) throw new ArgumentException( "obj cannot be null." );

        if ( _freeObjects.Count < Max )
        {
            _freeObjects.Add( obj );

            Peak = Math.Max( Peak, _freeObjects.Count );

            Reset( obj );
        }
        else
        {
            Discard( obj );
        }
    }

    /// <summary>
    /// Adds the specified number of new free objects to the pool.
    /// Usually called early on as a pre-allocation mechanism but
    /// can be used at any time.
    /// </summary>
    /// <param name="size">The number of objects to be added.</param>
    public void Fill( int size )
    {
        for ( var i = 0; i < size; i++ )
        {
            if ( _freeObjects.Count < Max )
            {
                _freeObjects.Add( NewObject() );
            }
        }

        Peak = Math.Max( Peak, _freeObjects.Count );
    }

    /// <summary>
    /// Called when an object is freed to clear the state of the object for possible
    /// later reuse. The default implementation calls <see cref="IPoolable.Reset"/>
    /// if the object is Poolable.
    /// </summary>
    public void Reset( T obj )
    {
        if ( obj is IPoolable poolable )
        {
            poolable.Reset();
        }
    }

    /// <summary>
    /// Called when an object is discarded. This is the case when an object is
    /// freed, but the maximum capacity of the pool is reached, and when the
    /// pool is <see cref="Clear"/>ed.
    /// </summary>
    protected virtual void Discard( T? obj )
    {
    }

    /// <summary>
    /// Puts the specified objects in the pool. Null objects within the array
    /// are silently ignored. The pool does not check if an object is already
    /// freed, so the same object must not be freed multiple times.
    /// </summary>
    public virtual void FreeAll( List< T > objects )
    {
        if ( objects == null ) throw new ArgumentException( "objects cannot be null." );

        var max = this.Max;

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            if ( objects[ i ] == null ) continue;

            if ( this._freeObjects.Count < max )
            {
                this._freeObjects.Add( objects[ i ] );

                Reset( objects[ i ] );
            }
            else
            {
                Discard( objects[ i ] );
            }
        }

        Peak = Math.Max( Peak, this._freeObjects.Count );
    }

    /// <summary>
    /// Removes and discards all free objects from this pool.
    /// </summary>
    public void Clear()
    {
        for ( var i = 0; i < _freeObjects.Count; i++ )
        {
            T? obj = _freeObjects[ i ];
            _freeObjects.RemoveAt( i );
            Discard( obj );
        }
    }

    /// <summary>
    /// The number of objects available to be obtained.
    /// </summary>
    public int GetFree() => _freeObjects.Count;
}