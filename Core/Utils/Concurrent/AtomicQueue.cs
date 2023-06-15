// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Concurrent;

/// <summary>
/// A queue that allows one thread to call <see cref="Put(T)"/> and another thread to call
/// <see cref="Poll()"/>. Multiple threads must not call these methods.
/// </summary>
/// <typeparam name="T"></typeparam>
public class AtomicQueue<T>
{
/*
    private readonly AtomicInteger             _writeIndex = new();
    private readonly AtomicInteger             _readIndex  = new();
    private readonly AtomicReferenceArray< T > _queue;

    public AtomicQueue( int capacity )
    {
        _queue = new AtomicReferenceArray<T>( capacity );
    }

    private int Next( int idx )
    {
        return ( idx + 1 ) % _queue.Length;
    }

    public bool Put( T? value )
    {
        int write = _writeIndex.Get();
        int read  = _readIndex.Get();
        int next  = Next( write );

        if ( next == read ) return false;

        _queue.set( write, value );
        _writeIndex.set( next );

        return true;
    }

    public T? Poll()
    {
        int read  = _readIndex.get();
        int write = _writeIndex.get();

        if ( read == write ) return default;

        T value = _queue.get( read );
        _readIndex.set( Next( read ) );

        return value;
    }
*/
}
