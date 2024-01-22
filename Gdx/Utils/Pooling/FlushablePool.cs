// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Pooling;

/// <summary>
///     A <see cref="Pool{T}" /> which keeps track of the obtained items
///     (see <see cref="Obtain()" />), which can be freed all at once using the
///     <see cref="Flush()" /> method.
/// </summary>
[PublicAPI]
public abstract class FlushablePool<T> : Pool< T >
{
    private readonly List< T > _obtained = [ ];

    /// <inheritdoc />
    protected FlushablePool()
    {
    }

    /// <inheritdoc />
    protected FlushablePool( int initialCapacity )
        : base( initialCapacity )
    {
    }

    /// <inheritdoc />
    protected FlushablePool( int initialCapacity, int max )
        : base( initialCapacity, max )
    {
    }

    /// <inheritdoc />
    public override T? Obtain()
    {
        T? result = base.Obtain();

        _obtained.Add( result! );

        return result;
    }

    /// <summary>
    ///     Frees all obtained instances.
    /// </summary>
    public virtual void Flush()
    {
        base.FreeAll( _obtained );
        _obtained.Clear();
    }

    /// <inheritdoc />
    public override void Free( T obj )
    {
        _obtained.Remove( obj );

        base.Free( obj );
    }

    /// <inheritdoc />
    public override void FreeAll( List< T > objects )
    {
        foreach ( T obj in objects )
        {
            _obtained.Remove( obj );
        }

        base.FreeAll( objects );
    }
}
