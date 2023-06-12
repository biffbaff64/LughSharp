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

using System.Collections;

namespace LibGDXSharp.Utils.Collections;

public class PredicateIterator<T> : IEnumerator< T >
{
    public IEnumerator< T? > Enumerator { get; set; }
    public IPredicate< T >   Predicate  { get; set; }
    public bool              End        { get; set; } = false;
    public bool              Peeked     { get; set; } = false;
    public T?                NextItem   { get; set; } = default;

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public PredicateIterator( IEnumerable< T? > enumerable, IPredicate< T > predicate )
        : this( enumerable.GetEnumerator(), predicate )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="enumerator"></param>
    /// <param name="predicate"></param>
    public PredicateIterator( IEnumerator< T? > enumerator, IPredicate< T > predicate )
    {
        this.Enumerator = enumerator;
        this.Predicate  = predicate;
        End             = false;
        Peeked          = false;
        NextItem        = default;
    }

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public void Set( IEnumerable< T? > enumerable, IPredicate< T > predicate )
    {
        Set( enumerable.GetEnumerator(), predicate );
    }

    /// <summary>
    /// </summary>
    /// <param name="iterator"></param>
    /// <param name="predicate"></param>
    public void Set( IEnumerator< T? > iterator, IPredicate< T > predicate )
    {
        this.Enumerator = iterator;
        this.Predicate  = predicate;
        End             = false;
        Peeked          = false;
        NextItem        = default;
    }

    public bool HasNext()
    {
        if ( End ) return false;

        if ( NextItem != null ) return true;

        Peeked = true;

        while ( Enumerator.MoveNext() )
        {
            var n = Enumerator.Current;

            if ( Predicate.Evaluate( n ) )
            {
                NextItem = n;

                return true;
            }
        }

        End = true;

        return false;
    }

    public T? Next()
    {
        if ( ( NextItem == null ) && !HasNext() )
        {
            return default( T );
        }

        var result = NextItem;
        NextItem = default( T );
        Peeked   = false;

        return result;
    }

    public void Remove()
    {
        if ( Peeked )
        {
            throw new GdxRuntimeException( "Cannot remove between a call to hasNext() and next()." );
        }

        Enumerator.Dispose();
    }

    public bool MoveNext()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    object? IEnumerator.Current => Current;

    public T Current { get; }

    public void Dispose()
    {
        Remove();
    }
}