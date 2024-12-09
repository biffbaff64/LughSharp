// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace Corelib.Lugh.Utils.Pooling;

[PublicAPI]
public interface IPoolable< T >
{
    Pool< T >.NewObjectHandler? NewObject { get; set; }

    /// <summary>
    /// Returns an object from this pool.
    /// The object may be new (from <see cref="Pool{T}.NewObject"/>) or reused
    /// (previously <see cref="Pool{T}.Free"/> freed).
    /// </summary>
    T? Obtain();

    /// <summary>
    /// Adds the specified number of new free objects to the pool.
    /// Usually called early on as a pre-allocation mechanism but
    /// can be used at any time.
    /// </summary>
    /// <param name="size">The number of objects to be added.</param>
    void Fill( int size );

    /// <summary>
    /// Called when an object is discarded. This is the case when an object is
    /// freed, but the maximum capacity of the pool is reached, and when the
    /// pool is <see cref="Pool{T}.Clear"/>ed.
    /// </summary>
    void Discard( T? obj );

    /// <summary>
    /// Puts the specified object in the pool, making it eligible to be returned by
    /// <see cref="Pool{T}.Obtain"/>. If the pool already contains <see cref="Pool{T}.Max"/> free objects,
    /// the specified object is discarded using <see cref="Pool{T}.Discard"/> and not added to the pool.
    /// The pool does not check if an object is already freed, so the same object must not be
    /// freed multiple times.
    /// </summary>
    /// <param name="obj">The object to add to the pool.</param>
    /// <exception cref="ArgumentException"></exception>
    void Free( T obj );

    /// <summary>
    /// Puts the specified objects in the pool. Null objects within the array
    /// are silently ignored. The pool does not check if an object is already
    /// freed, so the same object must not be freed multiple times.
    /// </summary>
    void FreeAll( List< T > objects );

    /// <summary>
    /// Removes and discards all free objects from this pool.
    /// </summary>
    void Clear();

    /// <summary>
    /// The number of objects available to be obtained.
    /// </summary>
    int GetFree();
}