namespace LibGDXSharp.Utils
{
    public class VStack<T> : Vector< T >
    {
        public VStack()
        {
        }

        /// <summary>
        /// Pushes an item onto the top of this stack. This has exactly
        /// the same effect as: <see cref="AddElement(item)"/>
        /// </summary>
        /// <param name="item">the item to be pushed onto this stack.</param>
        public T Push( T item )
        {
            AddElement( item );

            return item;
        }

        /// <summary>
        /// Removes the object at the top of this stack and returns that
        /// object as the value of this function.
        /// </summary>
        /// <returns>
        /// The object at the top of this stack (the last item of the <tt>Vector</tt> object).</returns>
        /// <exception cref="EmptyStackException">if this stack is empty.</exception>
        public T Pop()
        {
            lock ( this )
            {
                int len = Size();

                T obj = Peek();
                
                RemoveElementAt( len - 1 );

                return obj;
            }
        }

        /// <summary>
        /// Looks at the object at the top of this stack without removing it
        /// from the stack.
        /// </summary>
        /// <returns>
        /// The object at the top of this stack (the last item of the <tt>Vector</tt> object).
        /// </returns>
        /// <exception cref="EmptyStackException"> if this stack is empty.</exception>
        public T Peek()
        {
            lock ( this )
            {
                int len = Size();

                if ( len == 0 )
                {
                    throw new EmptyStackException();
                }

                return ElementAt( len - 1 );
            }
        }

        /// <summary>
        /// Tests if this stack is empty.
        /// </summary>
        /// <returns>
        /// <code>true</code> if and only if this stack contains no items;
        /// <code>false</code> otherwise.
        /// </returns>
        public bool IsEmpty()
        {
            return Size() == 0;
        }

        /// <summary>
        /// Returns the 1-based position where an object is on this stack.
        /// If the object <tt>o</tt> occurs as an item in this stack, this
        /// method returns the distance from the top of the stack of the
        /// occurrence nearest the top of the stack; the topmost item on the
        /// stack is considered to be at distance <tt>1</tt>. The <tt>equals</tt>
        /// method is used to compare <tt>o</tt> to the items in this stack.
        /// </summary>
        /// <param name="o">the desired object.</param>
        /// <returns>
        /// The 1-based position from the top of the stack where the object is
        /// located; the return value <code>-1</code> indicates that the object
        /// is not on the stack.
        /// </returns>
        public int Search( object o )
        {
            lock ( this )
            {
                int i = LastIndexOf( o );

                if ( i >= 0 )
                {
                    return Size() - i;
                }

                return -1;
            }
        }
    }
}
