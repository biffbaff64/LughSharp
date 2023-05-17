namespace LibGDXSharp.Utils.Collections;

public class CollectionsData
{
    /// <summary>
    /// When true, <see cref="Iterable.iterator()"/> for <see cref="Array"/>,
    /// <see cref="ObjectMap"/>, and other collections will allocate a new
    /// iterator for each invocation.
    /// <p>
    /// When false, the iterator is reused and nested use will throw an exception.
    /// Default is false.
    /// </p> 
    /// </summary>
    public static bool AllocateIterators { get; set; }
}