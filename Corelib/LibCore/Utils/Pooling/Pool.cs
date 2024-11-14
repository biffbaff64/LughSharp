// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Pooling;

/// <summary>
/// A pool of objects that can be reused to avoid allocation.
/// </summary>
[PublicAPI]
public class Pool< T > // : IPoolable< T >
{
    public delegate T? NewObjectHandler();

    public int Max  { get; }      // The maximum number of objects that will be pooled.
    public int Peak { get; set; } // The highest number of free objects. Can be reset any time.

    // ========================================================================

    private readonly List< T? > _freeObjects;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new pool with a specified initial capacity,
    /// </summary>
    /// <param name="initialCapacity">
    /// The initial size of the array supporting the pool. No objects are created/pre-allocated.
    /// Default value is 16.
    /// </param>
    /// <param name="max">
    /// The maximum number of free objects to store in this pool.
    /// Default is int.MaxValue, ie NO max.
    /// </param>
    public Pool( int initialCapacity = 16, int max = int.MaxValue )
    {
        _freeObjects = new List< T? >( initialCapacity );
        Max          = max;
    }
    
    public virtual NewObjectHandler? NewObject { get; set; } = null;

    /// <summary>
    /// Returns an object from this pool.
    /// The object may be new (from <see cref="NewObject"/>) or reused
    /// (previously <see cref="Free(T)"/> freed).
    /// </summary>
    public virtual T? Obtain()
    {
        if ( _freeObjects.Count == 0 )
        {
            GdxRuntimeException.ThrowIfNull( NewObject );

            return NewObject();
        }

        var item = _freeObjects[ ^1 ];

        _freeObjects[ ^1 ] = default( T? );

        return item;
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
                _freeObjects.Add( NewObject!() );
            }
        }

        Peak = Math.Max( Peak, _freeObjects.Count );
    }

    /// <summary>
    /// Called when an object is freed to clear the state of the object for possible
    /// later reuse. The default implementation calls <see cref="IResetable.Reset"/>
    /// if the object is Poolable.
    /// </summary>
    public virtual void Reset( T obj )
    {
        if ( obj is IResetable poolable )
        {
            poolable.Reset();
        }
    }

    /// <summary>
    /// Called when an object is discarded. This is the case when an object is
    /// freed, but the maximum capacity of the pool is reached, and when the
    /// pool is <see cref="Clear"/>ed.
    /// </summary>
    public virtual void Discard( T? obj )
    {
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
        ArgumentNullException.ThrowIfNull( obj );

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
    /// Puts the specified objects in the pool. Null objects within the array
    /// are silently ignored. The pool does not check if an object is already
    /// freed, so the same object must not be freed multiple times.
    /// </summary>
    public virtual void FreeAll( List< T > objects )
    {
        ArgumentNullException.ThrowIfNull( objects );

        for ( int i = 0, n = objects.Count; i < n; i++ )
        {
            if ( objects[ i ] == null )
            {
                continue;
            }

            if ( _freeObjects.Count < Max )
            {
                _freeObjects.Add( objects[ i ] );

                Reset( objects[ i ] );
            }
            else
            {
                Discard( objects[ i ] );
            }
        }

        Peak = Math.Max( Peak, _freeObjects.Count );
    }

    /// <summary>
    /// Removes and discards all free objects from this pool.
    /// </summary>
    public virtual void Clear()
    {
        for ( var i = 0; i < _freeObjects.Count; i++ )
        {
            var obj = _freeObjects[ i ];
            _freeObjects.RemoveAt( i );
            Discard( obj );
        }
    }

    /// <summary>
    /// The number of objects available to be obtained.
    /// </summary>
    public virtual int GetFree()
    {
        return _freeObjects.Count;
    }
}
