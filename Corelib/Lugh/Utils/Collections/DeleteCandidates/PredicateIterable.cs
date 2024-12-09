// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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


namespace Corelib.Lugh.Utils.Collections.DeleteCandidates;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
[PublicAPI]
public class PredicateIterable< T > : IEnumerable< T >
{
    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public PredicateIterable( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        Enumerable = enumerable;
        Predicate  = predicate;
    }

    public IEnumerable< T >        Enumerable { get; set; }
    public IPredicate< T >         Predicate  { get; set; }
    public PredicateIterator< T >? Enumerator { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< T > GetEnumerator()
    {
        return Enumerable.GetEnumerator();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public void Set( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        Enumerable = enumerable;
        Predicate  = predicate;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public IEnumerator< T > Iterator()
    {
        if ( Enumerator == null )
        {
            Enumerator = new PredicateIterator< T >( Enumerable.GetEnumerator(), Predicate );
        }
        else
        {
            Enumerator.Set( Enumerable.GetEnumerator(), Predicate );
        }

        return Enumerator;
    }
}
