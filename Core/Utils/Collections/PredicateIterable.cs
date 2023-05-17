using System.Collections;

namespace LibGDXSharp.Utils.Collections;

/// <summary>
/// </summary>
/// <typeparam name="T"></typeparam>
public class PredicateIterable<T> : IEnumerable< T >
{
    public IEnumerable< T >        Enumerable { get; set; }
    public IPredicate< T >         Predicate  { get; set; }
    public PredicateIterator< T >? Enumerator { get; set; } = null;

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public PredicateIterable( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        this.Enumerable = enumerable;
        this.Predicate  = predicate;
    }

    /// <summary>
    /// </summary>
    /// <param name="enumerable"></param>
    /// <param name="predicate"></param>
    public void Set( IEnumerable< T > enumerable, IPredicate< T > predicate )
    {
        this.Enumerable = enumerable;
        this.Predicate  = predicate;
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
}