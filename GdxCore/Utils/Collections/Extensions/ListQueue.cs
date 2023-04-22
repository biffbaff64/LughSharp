namespace LibGDXSharp.Utils.Collections.Extensions
{
    /// <summary>
    /// List-based implementation of a Queue.
    /// </summary>
    public class ListQueue<T> : List<T>
    {
        public T Peek()
        {
            return this[0];
        }

        public void Enqueue( T element )
        {
            Add( element );
        }

        public T Dequeue()
        {
            T result = Peek();
            
            Remove( result );
            
            return result;
        }
    }
}

