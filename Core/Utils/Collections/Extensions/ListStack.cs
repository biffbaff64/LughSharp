namespace LibGDXSharp.Utils.Collections.Extensions;

/// <summary>
/// List-based implementation of a Stack.
/// </summary>
public class ListStack<T> : List< T >
{
    public T Peek()
    {
        return this[ Count - 1 ];
    }

    public void Push( T element )
    {
        Add( element );
    }

    public T Pop()
    {
        T result = Peek();
            
        Remove( result );

        return result;
    }
}