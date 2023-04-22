using System.Collections;

using LibGDXSharp.Gdx.Utils.Collections;

namespace LibGDXSharp.Utils
{
    /// <summary>
    /// The <tt>Vector</tt> class implements a growable array of objects. Like an
    /// array, it contains components that can be accessed using an integer index.
    /// However, the size of a <tt>Vector</tt> can grow or shrink as needed to
    /// accommodate adding and removing items after the <tt>Vector</tt> has been
    /// created.
    /// 
    /// <para>
    /// Each vector tries to optimize storage management by maintaining a<tt>capacity</tt>
    /// and a <tt>capacityIncrement</tt>. The <tt>capacity</tt> is always at least as
    /// large as the vector size; it is usually larger because as components are
    /// added to the vector, the vector's storage increases in chunks the size of
    /// <tt>capacityIncrement</tt>. An application can increase the capacity of a vector
    /// before inserting a large number of components; this reduces the amount of
    /// incremental reallocation.
    /// </para>
    /// <para><a name="fail-fast">
    /// The iterators returned by this class's <see cref="Iterator()"/> and
    /// <see cref="ListIterator(int)"/> methods are <em>fail-fast</em></a>:
    /// if the vector is structurally modified at any time after the iterator is created,
    /// in any way except through the iterator's own
    /// <seealso cref="ListIterator.Remove()"/> or
    /// <seealso cref="ListIterator.Add(Object)"/> methods, the iterator will throw a
    /// <seealso cref="ConcurrentModificationException"/>.  Thus, in the face of
    /// concurrent modification, the iterator fails quickly and cleanly, rather
    /// than risking arbitrary, non-deterministic behavior at an undetermined
    /// time in the future.  The <seealso cref="System.Collections.IEnumerator Enumerations"/> returned by
    /// the <seealso cref="elements() elements"/> method are <em>not</em> fail-fast.
    /// 
    /// </para>
    /// <para>Note that the fail-fast behavior of an iterator cannot be guaranteed
    /// as it is, generally speaking, impossible to make any hard guarantees in the
    /// presence of unsynchronized concurrent modification.  Fail-fast iterators
    /// throw {@code ConcurrentModificationException} on a best-effort basis.
    /// Therefore, it would be wrong to write a program that depended on this
    /// exception for its correctness:  <i>the fail-fast behavior of iterators
    /// should be used only to detect bugs.</i>
    /// 
    /// </para>
    /// <para>As of the Java 2 platform v1.2, this class was retrofitted to
    /// implement the <seealso cref="System.Collections.IList"/> interface, making it a member of the
    /// <a href="{@docRoot}/../technotes/guides/collections/index.html">
    /// Java Collections Framework</a>.  Unlike the new collection
    /// implementations, {@code Vector} is synchronized.  If a thread-safe
    /// implementation is not needed, it is recommended to use {@link
    /// ArrayList} in place of {@code Vector}.
    /// </para>
    /// </summary>
    public class Vector<T>
    {
        /// <summary>
        /// The array buffer into which the components of the vector are
        /// stored. The capacity of the vector is the length of this array buffer,
        /// and is at least large enough to contain all the vector's elements.
        /// <para>
        /// Any array elements following the last element in the Vector are null.
        /// </para>
        /// </summary>
        protected internal T[]? ElementData { get; set; }

        /// <summary>
        /// The number of valid components in this <tt>Vector</tt> object.
        /// Components <tt>elementData[0]</tt> through <tt>elementData[elementCount-1]</tt>
        /// are the actual items.
        /// </summary>
        protected internal int ElementCount { get; set; }

        /// <summary>
        /// The amount by which the capacity of the vector is automatically
        /// incremented when its size becomes greater than its capacity.  If
        /// the capacity increment is less than or equal to zero, the capacity
        /// of the vector is doubled each time it needs to grow.
        /// </summary>
        protected internal int CapacityIncrement { get; set; }

        /// <summary>
        /// Constructs an empty vector with the specified initial capacity and
        /// capacity increment.
        /// </summary>
        /// <param name="initialCapacity">the initial capacity of the vector</param>
        /// <param name="capacityIncrement">
        /// the amount by which the capacity is increased when the vector overflows</param>
        /// <exception cref="ArgumentException">
        /// if the specified initial capacity is negative
        /// </exception>
        public Vector( int initialCapacity = 10, int capacityIncrement = 0 )
        {
            if ( initialCapacity < 0 )
            {
                throw new System.ArgumentException( "Illegal Capacity: " + initialCapacity );
            }

            this.GetElementData    = new T[ initialCapacity ];
            this.CapacityIncrement = capacityIncrement;
        }

        /// <summary>
        /// Constructs a vector containing the elements of the specified collection,
        /// in the order they are returned by the collection's iterator.
        /// </summary>
        /// <param name="collection">
        /// the collection whose elements are to be placed into this vector
        /// </param>
        /// <exception cref="NullReferenceException">
        /// if the specified collection is null
        /// </exception>
        public Vector( IEnumerable< T > collection )
        {
            GetElementData = collection.ToArray();
            ElementCount   = GetElementData.Length;

            if ( GetElementData.GetType() != typeof(object[]) )
            {
                // TODO:
//                ElementData = Array.CopyOf( ElementData, ElementCount, typeof(object[]) );
            }
        }

        /// <summary>
        /// Copies the components of this vector into the specified array.
        /// The item at index <tt>k</tt> in this vector is copied into
        /// component <tt>k</tt> of <tt>anArray</tt>.
        /// </summary>
        /// <param name="anArray"> the array into which the components get copied </param>
        /// <exception cref="NullReferenceException"> if the given array is null </exception>
        /// <exception cref="IndexOutOfRangeException">
        /// if the specified array is not large enough to hold all the components of this vector
        /// </exception>
        /// <exception cref="FormatException">
        /// if a component of this vector is not of a runtime type that can be stored in the
        /// specified array
        /// </exception>
        public void CopyInto( object[] anArray )
        {
            lock ( this )
            {
                if ( GetElementData != null )
                {
                    Array.Copy
                        (
                         GetElementData,
                         0,
                         anArray,
                         0,
                         ElementCount
                        );
                }
            }
        }

        /// <summary>
        /// Trims the capacity of this vector to be the vector's current
        /// size. If the capacity of this vector is larger than its current
        /// size, then the capacity is changed to equal the size by replacing
        /// its internal data array, kept in the field <tt>elementData</tt>,
        /// with a smaller one. An application can use this operation to
        /// minimize the storage of a vector.
        /// </summary>
        public void TrimToSize()
        {
            lock ( this )
            {
                modCount++;

                var oldCapacity = GetElementData.Length;

                if ( ElementCount < oldCapacity )
                {
                    GetElementData = ArrayUtils.CopyOf( GetElementData, ElementCount );
                }
            }
        }

        /// <summary>
        /// Increases the capacity of this vector, if necessary, to ensure
        /// that it can hold at least the number of components specified by
        /// the minimum capacity argument.
        /// <para>
        /// If the current capacity of this vector is less than <tt>minCapacity</tt>,
        /// then its capacity is increased by replacing its internal data array, kept
        /// in the field <tt>ElementData</tt>, with a larger one. The size of the new
        /// data array will be the old size plus <tt>CapacityIncrement</tt>, unless
        /// the value of <tt>CapacityIncrement</tt> is less than or equal to zero,
        /// in which case the new capacity will be twice the old capacity; but if this
        /// new size is still smaller than <tt>minCapacity</tt>, then the new capacity will
        /// be <tt>minCapacity</tt>.
        /// </para>
        /// </summary>
        /// <param name="minCapacity"> the desired minimum capacity </param>
        public void EnsureCapacity( int minCapacity )
        {
            lock ( this )
            {
                if ( minCapacity > 0 )
                {
                    modCount++;

                    EnsureCapacityHelper( minCapacity );
                }
            }
        }

        /// <summary>
        /// This implements the unsynchronized semantics of ensureCapacity.
        /// Synchronized methods in this class can internally call this
        /// method for ensuring capacity without incurring the cost of an
        /// extra synchronization.
        /// </summary>
        private void EnsureCapacityHelper( int minCapacity )
        {
            // overflow-conscious code
            if ( minCapacity - GetElementData?.Length > 0 )
            {
                Grow( minCapacity );
            }
        }

        /// <summary>
        /// The maximum size of array to allocate.
        /// </summary>
        private const int MaxArraySize = int.MaxValue - 8;

        private void Grow( int minCapacity )
        {
            // overflow-conscious code
            int oldCapacity = GetElementData.Length;
            int newCapacity = oldCapacity + ( ( CapacityIncrement > 0 ) ? CapacityIncrement : oldCapacity );

            if ( newCapacity - minCapacity < 0 )
            {
                newCapacity = minCapacity;
            }

            if ( newCapacity - MaxArraySize > 0 )
            {
                newCapacity = HugeCapacity( minCapacity );
            }

            GetElementData = ArrayUtils.CopyOf( GetElementData, newCapacity );
        }

        private static int HugeCapacity( int minCapacity )
        {
            if ( minCapacity < 0 ) // overflow
            {
                throw new System.OutOfMemoryException();
            }

            return ( minCapacity > MaxArraySize ) ? int.MaxValue : MaxArraySize;
        }

        /// <summary>
        /// Sets the size of this vector. If the new size is greater than the
        /// current size, new {@code null} items are added to the end of
        /// the vector. If the new size is less than the current size, all
        /// components at index {@code newSize} and greater are discarded.
        /// </summary>
        /// <param name="newSize">   the new size of this vector </param>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the new size is negative </exception>
        public virtual int Size
        {
            set
            {
                lock ( this )
                {
                    modCount++;

                    if ( value > elementCount )
                    {
                        ensureCapacityHelper( value );
                    }
                    else
                    {
                        for ( int i = value; i < elementCount; i++ )
                        {
                            elementData[ i ] = null;
                        }
                    }

                    elementCount = value;
                }
            }
        }

        /// <summary>
        /// Returns the current capacity of this vector.
        /// </summary>
        /// <returns>  the current capacity (the length of its internal
        ///          data array, kept in the field {@code elementData}
        ///          of this vector) </returns>
        public virtual int Capacity()
        {
            lock ( this )
            {
                return elementData.length;
            }
        }

        /// <summary>
        /// Returns the number of components in this vector.
        /// </summary>
        /// <returns>  the number of components in this vector </returns>
        public virtual int Size()
        {
            lock ( this )
            {
                return elementCount;
            }
        }

        /// <summary>
        /// Tests if this vector has no components.
        /// </summary>
        /// <returns>  {@code true} if and only if this vector has
        ///          no components, that is, its size is zero;
        ///          {@code false} otherwise. </returns>
        public virtual bool Empty
        {
            get
            {
                lock ( this )
                {
                    return elementCount == 0;
                }
            }
        }

        /// <summary>
        /// Returns an enumeration of the components of this vector. The
        /// returned {@code Enumeration} object will generate all items in
        /// this vector. The first item generated is the item at index {@code 0},
        /// then the item at index {@code 1}, and so on.
        /// </summary>
        /// <returns>  an enumeration of the components of this vector </returns>
        /// <seealso cref="Iterator"/>
        public virtual IEnumerator< E > Elements()
        {
            return new IteratorAnonymousInnerClass( this );
        }

        private class IteratorAnonymousInnerClass : IEnumerator< E >
        {
            private readonly MissingClass _outerInstance;

            public IteratorAnonymousInnerClass( MissingClass outerInstance )
            {
                this._outerInstance = outerInstance;
                Count               = 0;
            }

            internal int Count;

            public bool hasMoreElements()
            {
                return Count < elementCount;
            }

            public T NextElement()
            {
                lock ( this )
                {
                    if ( Count < ElementCount )
                    {
                        return GetElementData( Count++ );
                    }
                }

                throw new NoSuchElementException( "Vector Enumeration" );
            }
        }

        /// <summary>
        /// Returns {@code true} if this vector contains the specified element.
        /// More formally, returns {@code true} if and only if this vector
        /// contains at least one element {@code e} such that
        /// <tt>(o==null&nbsp;?&nbsp;e==null&nbsp;:&nbsp;o.equals(e))</tt>.
        /// </summary>
        /// <param name="o"> element whose presence in this vector is to be tested </param>
        /// <returns> {@code true} if this vector contains the specified element </returns>
        public virtual bool Contains( object o )
        {
            return IndexOf( o, 0 ) >= 0;
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified element
        /// in this vector, or -1 if this vector does not contain the element.
        /// More formally, returns the lowest index {@code i} such that
        /// <tt>(o==null&nbsp;?&nbsp;get(i)==null&nbsp;:&nbsp;o.equals(get(i)))</tt>,
        /// or -1 if there is no such index.
        /// </summary>
        /// <param name="o"> element to search for </param>
        /// <returns> the index of the first occurrence of the specified element in
        ///         this vector, or -1 if this vector does not contain the element </returns>
        public virtual int IndexOf( object o )
        {
            return IndexOf( o, 0 );
        }

        /// <summary>
        /// Returns the index of the first occurrence of the specified element in
        /// this vector, searching forwards from {@code index}, or returns -1 if
        /// the element is not found.
        /// More formally, returns the lowest index {@code i} such that
        /// <tt>(i&nbsp;&gt;=&nbsp;index&nbsp;&amp;&amp;&nbsp;(o==null&nbsp;?&nbsp;get(i)==null&nbsp;:&nbsp;o.equals(get(i))))</tt>,
        /// or -1 if there is no such index.
        /// </summary>
        /// <param name="o"> element to search for </param>
        /// <param name="index"> index to start searching from </param>
        /// <returns> the index of the first occurrence of the element in
        ///         this vector at position {@code index} or later in the vector;
        ///         {@code -1} if the element is not found. </returns>
        /// <exception cref="IndexOutOfBoundsException"> if the specified index is negative </exception>
        /// <seealso cref="Object.equals(Object)"/>
        public virtual int IndexOf( object o, int index )
        {
            lock ( this )
            {
                if ( o == null )
                {
                    for ( int i = index; i < elementCount; i++ )
                    {
                        if ( elementData[ i ] == null )
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    for ( int i = index; i < elementCount; i++ )
                    {
                        if ( o.Equals( elementData[ i ] ) )
                        {
                            return i;
                        }
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Returns the index of the last occurrence of the specified element
        /// in this vector, or -1 if this vector does not contain the element.
        /// More formally, returns the highest index {@code i} such that
        /// <tt>(o==null&nbsp;?&nbsp;get(i)==null&nbsp;:&nbsp;o.equals(get(i)))</tt>,
        /// or -1 if there is no such index.
        /// </summary>
        /// <param name="o"> element to search for </param>
        /// <returns> the index of the last occurrence of the specified element in
        ///         this vector, or -1 if this vector does not contain the element </returns>
        public virtual int LastIndexOf( object o )
        {
            lock ( this )
            {
                return lastIndexOf( o, elementCount - 1 );
            }
        }

        /// <summary>
        /// Returns the index of the last occurrence of the specified element in
        /// this vector, searching backwards from {@code index}, or returns -1 if
        /// the element is not found.
        /// More formally, returns the highest index {@code i} such that
        /// <tt>(i&nbsp;&lt;=&nbsp;index&nbsp;&amp;&amp;&nbsp;(o==null&nbsp;?&nbsp;get(i)==null&nbsp;:&nbsp;o.equals(get(i))))</tt>,
        /// or -1 if there is no such index.
        /// </summary>
        /// <param name="o"> element to search for </param>
        /// <param name="index"> index to start searching backwards from </param>
        /// <returns> the index of the last occurrence of the element at position
        ///         less than or equal to {@code index} in this vector;
        ///         -1 if the element is not found. </returns>
        /// <exception cref="IndexOutOfBoundsException"> if the specified index is greater
        ///         than or equal to the current size of this vector </exception>
        public virtual int LastIndexOf( object o, int index )
        {
            lock ( this )
            {
                if ( index >= elementCount )
                {
                    throw new System.IndexOutOfRangeException( index + " >= " + elementCount );
                }

                if ( o == null )
                {
                    for ( int i = index; i >= 0; i-- )
                    {
                        if ( elementData[ i ] == null )
                        {
                            return i;
                        }
                    }
                }
                else
                {
                    for ( int i = index; i >= 0; i-- )
                    {
                        if ( o.Equals( elementData[ i ] ) )
                        {
                            return i;
                        }
                    }
                }

                return -1;
            }
        }

        /// <summary>
        /// Returns the component at the specified index.
        /// 
        /// <para>This method is identical in functionality to the <seealso cref="get(int)"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface).
        /// 
        /// </para>
        /// </summary>
        /// <param name="index">   an index into this vector </param>
        /// <returns>     the component at the specified index </returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index >= size()}) </exception>
        public virtual E ElementAt( int index )
        {
            lock ( this )
            {
                if ( index >= elementCount )
                {
                    throw new System.IndexOutOfRangeException( index + " >= " + elementCount );
                }

                return elementData( index );
            }
        }

        /// <summary>
        /// Returns the first component (the item at index {@code 0}) of
        /// this vector.
        /// </summary>
        /// <returns>     the first component of this vector </returns>
        /// <exception cref="NoSuchElementException"> if this vector has no components </exception>
        public virtual E FirstElement()
        {
            lock ( this )
            {
                if ( elementCount == 0 )
                {
                    throw new NoSuchElementException();
                }

                return elementData( 0 );
            }
        }

        /// <summary>
        /// Returns the last component of the vector.
        /// </summary>
        /// <returns>  the last component of the vector, i.e., the component at index
        ///          <code>size()&nbsp;-&nbsp;1</code>. </returns>
        /// <exception cref="NoSuchElementException"> if this vector is empty </exception>
        public virtual E LastElement()
        {
            lock ( this )
            {
                if ( elementCount == 0 )
                {
                    throw new NoSuchElementException();
                }

                return elementData( elementCount - 1 );
            }
        }

        /// <summary>
        /// Sets the component at the specified {@code index} of this
        /// vector to be the specified object. The previous component at that
        /// position is discarded.
        /// 
        /// <para>The index must be a value greater than or equal to {@code 0}
        /// and less than the current size of the vector.
        /// 
        /// </para>
        /// <para>This method is identical in functionality to the
        /// <seealso cref="set(int, Object) set(int, E)"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface). Note that the
        /// {@code set} method reverses the order of the parameters, to more closely
        /// match array usage.  Note also that the {@code set} method returns the
        /// old value that was stored at the specified position.
        /// 
        /// </para>
        /// </summary>
        /// <param name="obj">     what the component is to be set to </param>
        /// <param name="index">   the specified index </param>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index >= size()}) </exception>
        public virtual void SetElementAt( E obj, int index )
        {
            lock ( this )
            {
                if ( index >= elementCount )
                {
                    throw new System.IndexOutOfRangeException( index + " >= " + elementCount );
                }

                elementData[ index ] = obj;
            }
        }

        /// <summary>
        /// Deletes the component at the specified index. Each component in
        /// this vector with an index greater or equal to the specified
        /// {@code index} is shifted downward to have an index one
        /// smaller than the value it had previously. The size of this vector
        /// is decreased by {@code 1}.
        /// 
        /// <para>The index must be a value greater than or equal to {@code 0}
        /// and less than the current size of the vector.
        /// 
        /// </para>
        /// <para>This method is identical in functionality to the <seealso cref="remove(int)"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface).  Note that the
        /// {@code remove} method returns the old value that was stored at the
        /// specified position.
        /// 
        /// </para>
        /// </summary>
        /// <param name="index">   the index of the object to remove </param>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index >= size()}) </exception>
        public virtual void RemoveElementAt( int index )
        {
            lock ( this )
            {
                modCount++;

                if ( index >= elementCount )
                {
                    throw new System.IndexOutOfRangeException( index + " >= " + elementCount );
                }
                else if ( index < 0 )
                {
                    throw new System.IndexOutOfRangeException( index );
                }

                int j = elementCount - index - 1;

                if ( j > 0 )
                {
                    Array.Copy( elementData, index + 1, elementData, index, j );
                }

                elementCount--;
                elementData[ elementCount ] = null; // to let gc do its work
            }
        }

        /// <summary>
        /// Inserts the specified object as a component in this vector at the
        /// specified {@code index}. Each component in this vector with
        /// an index greater or equal to the specified {@code index} is
        /// shifted upward to have an index one greater than the value it had
        /// previously.
        /// 
        /// <para>The index must be a value greater than or equal to {@code 0}
        /// and less than or equal to the current size of the vector. (If the
        /// index is equal to the current size of the vector, the new element
        /// is appended to the Vector.)
        /// 
        /// </para>
        /// <para>This method is identical in functionality to the
        /// <seealso cref="add(int, Object) add(int, E)"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface).  Note that the
        /// {@code add} method reverses the order of the parameters, to more closely
        /// match array usage.
        /// 
        /// </para>
        /// </summary>
        /// <param name="obj">     the component to insert </param>
        /// <param name="index">   where to insert the new component </param>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index > size()}) </exception>
        public virtual void InsertElementAt( E obj, int index )
        {
            lock ( this )
            {
                modCount++;

                if ( index > elementCount )
                {
                    throw new System.IndexOutOfRangeException( index + " > " + elementCount );
                }

                ensureCapacityHelper( elementCount + 1 );
                Array.Copy( elementData, index, elementData, index + 1, elementCount - index );
                elementData[ index ] = obj;
                elementCount++;
            }
        }

        /// <summary>
        /// Adds the specified component to the end of this vector,
        /// increasing its size by one. The capacity of this vector is
        /// increased if its size becomes greater than its capacity.
        /// 
        /// <para>This method is identical in functionality to the
        /// <seealso cref="add(Object) add(E)"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface).
        /// 
        /// </para>
        /// </summary>
        /// <param name="obj">   the component to be added </param>
        public virtual void AddElement( E obj )
        {
            lock ( this )
            {
                modCount++;
                ensureCapacityHelper( elementCount + 1 );
                elementData[ elementCount++ ] = obj;
            }
        }

        /// <summary>
        /// Removes the first (lowest-indexed) occurrence of the argument
        /// from this vector. If the object is found in this vector, each
        /// component in the vector with an index greater or equal to the
        /// object's index is shifted downward to have an index one smaller
        /// than the value it had previously.
        /// 
        /// <para>This method is identical in functionality to the
        /// <seealso cref="remove(Object)"/> method (which is part of the
        /// <seealso cref="System.Collections.IList"/> interface).
        /// 
        /// </para>
        /// </summary>
        /// <param name="obj">   the component to be removed </param>
        /// <returns>  {@code true} if the argument was a component of this
        ///          vector; {@code false} otherwise. </returns>
        public virtual bool RemoveElement( object obj )
        {
            lock ( this )
            {
                modCount++;
                int i = indexOf( obj );

                if ( i >= 0 )
                {
                    removeElementAt( i );

                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Removes all components from this vector and sets its size to zero.
        /// 
        /// <para>This method is identical in functionality to the <seealso cref="clear"/>
        /// method (which is part of the <seealso cref="System.Collections.IList"/> interface).
        /// </para>
        /// </summary>
        public virtual void RemoveAllElements()
        {
            lock ( this )
            {
                modCount++;

                // Let gc do its work
                for ( int i = 0; i < elementCount; i++ )
                {
                    elementData[ i ] = null;
                }

                elementCount = 0;
            }
        }

        /// <summary>
        /// Returns a clone of this vector. The copy will contain a
        /// reference to a clone of the internal data array, not a reference
        /// to the original internal data array of this {@code Vector} object.
        /// </summary>
        /// <returns>  a clone of this vector </returns>
        public virtual object Clone()
        {
            lock ( this )
            {
                try
                {
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") Vector<E> v = (Vector<E>) super.clone();
                    List< E > v = ( List< E > )base.Clone();
                    v.elementData = Arrays.CopyOf( elementData, elementCount );
                    v.modCount    = 0;

                    return v;
                }
                catch ( CloneNotSupportedException e )
                {
                    // this shouldn't happen, since we are Cloneable
                    throw new InternalError( e );
                }
            }
        }

        /// <summary>
        /// Returns an array containing all of the elements in this Vector
        /// in the correct order.
        /// 
        /// @since 1.2
        /// </summary>
        public virtual object[] ToArray()
        {
            lock ( this )
            {
                return Arrays.CopyOf( elementData, elementCount );
            }
        }

        /// <summary>
        /// Returns an array containing all of the elements in this Vector in the
        /// correct order; the runtime type of the returned array is that of the
        /// specified array.  If the Vector fits in the specified array, it is
        /// returned therein.  Otherwise, a new array is allocated with the runtime
        /// type of the specified array and the size of this Vector.
        /// 
        /// <para>If the Vector fits in the specified array with room to spare
        /// (i.e., the array has more elements than the Vector),
        /// the element in the array immediately following the end of the
        /// Vector is set to null.  (This is useful in determining the length
        /// of the Vector <em>only</em> if the caller knows that the Vector
        /// does not contain any null elements.)
        /// 
        /// </para>
        /// </summary>
        /// <param name="a"> the array into which the elements of the Vector are to
        ///          be stored, if it is big enough; otherwise, a new array of the
        ///          same runtime type is allocated for this purpose. </param>
        /// <returns> an array containing the elements of the Vector </returns>
        /// <exception cref="ArrayStoreException"> if the runtime type of a is not a supertype
        /// of the runtime type of every element in this Vector </exception>
        /// <exception cref="NullPointerException"> if the given array is null
        /// @since 1.2 </exception>
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public synchronized <T> T[] toArray(T[] a)
        public virtual T[] ToArray<T>( T[] a )
        {
            lock ( this )
            {
                if ( a.Length < elementCount )
                {
                    return ( T[] )Arrays.CopyOf( elementData, elementCount, a.GetType() );
                }

                Array.Copy( elementData, 0, a, 0, elementCount );

                if ( a.Length > elementCount )
                {
                    a[ elementCount ] = null;
                }

                return a;
            }
        }

        // Positional Access Operations

        protected virtual T GetElementData( int index )
        {
            return ElementData[ index ];
        }

        /// <summary>
        /// Returns the element at the specified position in this Vector.
        /// </summary>
        /// <param name="index"> index of the element to return </param>
        /// <returns> object at the specified index </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// if the index is out of range (<code>index &lt; 0 || index >= size()</code>)
        /// </exception>
        public T Get( int index )
        {
            lock ( this )
            {
                if ( index >= ElementCount )
                {
                    throw new System.IndexOutOfRangeException( index.ToString() );
                }

                return GetElementData( index );
            }
        }

        /// <summary>
        /// Replaces the element at the specified position in this Vector with the
        /// specified element.
        /// </summary>
        /// <param name="index"> index of the element to replace </param>
        /// <param name="element"> element to be stored at the specified position </param>
        /// <returns> the element previously at the specified position </returns>
        /// <exception cref="IndexOutOfRangeException">
        /// if the index is out of range (<code>index &lt; 0 || index >= size()</code>)
        /// </exception>
        public T Set( int index, T element )
        {
            lock ( this )
            {
                if ( index >= ElementCount )
                {
                    throw new System.IndexOutOfRangeException( index );
                }

                E oldValue = GetElementData( index );
                elementData[ index ] = element;

                return oldValue;
            }
        }

        /// <summary>
        /// Appends the specified element to the end of this Vector.
        /// </summary>
        /// <param name="e"> element to be appended to this Vector </param>
        /// <returns> {@code true} (as specified by <seealso cref="Collection.add"/>)
        /// @since 1.2 </returns>
        public virtual bool Add( E e )
        {
            lock ( this )
            {
                modCount++;
                ensureCapacityHelper( elementCount + 1 );
                elementData[ elementCount++ ] = e;

                return true;
            }
        }

        /// <summary>
        /// Removes the first occurrence of the specified element in this Vector
        /// If the Vector does not contain the element, it is unchanged.  More
        /// formally, removes the element with the lowest index i such that
        /// {@code (o==null ? get(i)==null : o.equals(get(i)))} (if such
        /// an element exists).
        /// </summary>
        /// <param name="o"> element to be removed from this Vector, if present </param>
        /// <returns> true if the Vector contained the specified element
        /// @since 1.2 </returns>
        public virtual bool Remove( object o )
        {
            return removeElement( o );
        }

        /// <summary>
        /// Inserts the specified element at the specified position in this Vector.
        /// Shifts the element currently at that position (if any) and any
        /// subsequent elements to the right (adds one to their indices).
        /// </summary>
        /// <param name="index"> index at which the specified element is to be inserted </param>
        /// <param name="element"> element to be inserted </param>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index > size()})
        /// @since 1.2 </exception>
        public virtual void Add( int index, E element )
        {
            insertElementAt( element, index );
        }

        /// <summary>
        /// Removes the element at the specified position in this Vector.
        /// Shifts any subsequent elements to the left (subtracts one from their
        /// indices).  Returns the element that was removed from the Vector.
        /// </summary>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index >= size()}) </exception>
        /// <param name="index"> the index of the element to be removed </param>
        /// <returns> element that was removed
        /// @since 1.2 </returns>
        public virtual E Remove( int index )
        {
            lock ( this )
            {
                modCount++;

                if ( index >= elementCount )
                {
                    throw new System.IndexOutOfRangeException( index );
                }

                E oldValue = elementData( index );

                int numMoved = elementCount - index - 1;

                if ( numMoved > 0 )
                {
                    Array.Copy( elementData, index + 1, elementData, index, numMoved );
                }

                elementData[ --elementCount ] = null; // Let gc do its work

                return oldValue;
            }
        }

        /// <summary>
        /// Removes all of the elements from this Vector.  The Vector will
        /// be empty after this call returns (unless it throws an exception).
        /// 
        /// @since 1.2
        /// </summary>
        public virtual void Clear()
        {
            removeAllElements();
        }

        // Bulk Operations

        /// <summary>
        /// Returns true if this Vector contains all of the elements in the
        /// specified Collection.
        /// </summary>
        /// <param name="c"> a collection whose elements will be tested for containment
        ///          in this Vector </param>
        /// <returns> true if this Vector contains all of the elements in the
        ///         specified collection </returns>
        /// <exception cref="NullPointerException"> if the specified collection is null </exception>
        public virtual bool ContainsAll<T1>( ICollection< T1 > c )
        {
            lock ( this )
            {
                return base.ContainsAll( c );
            }
        }

        /// <summary>
        /// Appends all of the elements in the specified Collection to the end of
        /// this Vector, in the order that they are returned by the specified
        /// Collection's Iterator.  The behavior of this operation is undefined if
        /// the specified Collection is modified while the operation is in progress.
        /// (This implies that the behavior of this call is undefined if the
        /// specified Collection is this Vector, and this Vector is nonempty.)
        /// </summary>
        /// <param name="c"> elements to be inserted into this Vector </param>
        /// <returns> {@code true} if this Vector changed as a result of the call </returns>
        /// <exception cref="NullPointerException"> if the specified collection is null
        /// @since 1.2 </exception>
        public virtual bool AddAll<T1>( ICollection< T1 > c ) where T1 : E
        {
            lock ( this )
            {
                modCount++;
                object[] a      = c.ToArray();
                int      numNew = a.Length;
                ensureCapacityHelper( elementCount + numNew );
                Array.Copy( a, 0, elementData, elementCount, numNew );
                elementCount += numNew;

                return numNew != 0;
            }
        }

        /// <summary>
        /// Removes from this Vector all of its elements that are contained in the
        /// specified Collection.
        /// </summary>
        /// <param name="c"> a collection of elements to be removed from the Vector </param>
        /// <returns> true if this Vector changed as a result of the call </returns>
        /// <exception cref="ClassCastException"> if the types of one or more elements
        ///         in this vector are incompatible with the specified
        ///         collection
        /// (<a href="Collection.html#optional-restrictions">optional</a>) </exception>
        /// <exception cref="NullPointerException"> if this vector contains one or more null
        ///         elements and the specified collection does not support null
        ///         elements
        /// (<a href="Collection.html#optional-restrictions">optional</a>),
        ///         or if the specified collection is null
        /// @since 1.2 </exception>
        public virtual bool RemoveAll<T1>( ICollection< T1 > c )
        {
            lock ( this )
            {
                return base.RemoveAll( c );
            }
        }

        /// <summary>
        /// Retains only the elements in this Vector that are contained in the
        /// specified Collection.  In other words, removes from this Vector all
        /// of its elements that are not contained in the specified Collection.
        /// </summary>
        /// <param name="c"> a collection of elements to be retained in this Vector
        ///          (all other elements are removed) </param>
        /// <returns> true if this Vector changed as a result of the call </returns>
        /// <exception cref="ClassCastException"> if the types of one or more elements
        ///         in this vector are incompatible with the specified
        ///         collection
        /// (<a href="Collection.html#optional-restrictions">optional</a>) </exception>
        /// <exception cref="NullPointerException"> if this vector contains one or more null
        ///         elements and the specified collection does not support null
        ///         elements
        ///         (<a href="Collection.html#optional-restrictions">optional</a>),
        ///         or if the specified collection is null
        /// @since 1.2 </exception>
        public virtual bool RetainAll<T1>( ICollection< T1 > c )
        {
            lock ( this )
            {
                return base.RetainAll( c );
            }
        }

        /// <summary>
        /// Inserts all of the elements in the specified Collection into this
        /// Vector at the specified position.  Shifts the element currently at
        /// that position (if any) and any subsequent elements to the right
        /// (increases their indices).  The new elements will appear in the Vector
        /// in the order that they are returned by the specified Collection's
        /// iterator.
        /// </summary>
        /// <param name="index"> index at which to insert the first element from the
        ///              specified collection </param>
        /// <param name="c"> elements to be inserted into this Vector </param>
        /// <returns> {@code true} if this Vector changed as a result of the call </returns>
        /// <exception cref="ArrayIndexOutOfBoundsException"> if the index is out of range
        ///         ({@code index < 0 || index > size()}) </exception>
        /// <exception cref="NullPointerException"> if the specified collection is null
        /// @since 1.2 </exception>
        public virtual bool AddAll<T1>( int index, ICollection< T1 > c ) where T1 : E
        {
            lock ( this )
            {
                modCount++;

                if ( index < 0 || index > elementCount )
                {
                    throw new System.IndexOutOfRangeException( index );
                }

                object[] a      = c.ToArray();
                int      numNew = a.Length;
                ensureCapacityHelper( elementCount + numNew );

                int numMoved = elementCount - index;

                if ( numMoved > 0 )
                {
                    Array.Copy( elementData, index, elementData, index + numNew, numMoved );
                }

                Array.Copy( a, 0, elementData, index, numNew );
                elementCount += numNew;

                return numNew != 0;
            }
        }

        /// <summary>
        /// Compares the specified Object with this Vector for equality.  Returns
        /// true if and only if the specified Object is also a List, both Lists
        /// have the same size, and all corresponding pairs of elements in the two
        /// Lists are <em>equal</em>.  (Two elements {@code e1} and
        /// {@code e2} are <em>equal</em> if {@code (e1==null ? e2==null :
        /// e1.equals(e2))}.)  In other words, two Lists are defined to be
        /// equal if they contain the same elements in the same order.
        /// </summary>
        /// <param name="o"> the Object to be compared for equality with this Vector </param>
        /// <returns> true if the specified Object is equal to this Vector </returns>
        public override bool Equals( object o )
        {
            lock ( this )
            {
                return base.Equals( o );
            }
        }

        /// <summary>
        /// Returns the hash code value for this Vector.
        /// </summary>
        public override int GetHashCode()
        {
            lock ( this )
            {
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// Returns a string representation of this Vector, containing
        /// the String representation of each element.
        /// </summary>
        public override string ToString()
        {
            lock ( this )
            {
                return base.ToString();
            }
        }

        /// <summary>
        /// Returns a view of the portion of this List between fromIndex,
        /// inclusive, and toIndex, exclusive.  (If fromIndex and toIndex are
        /// equal, the returned List is empty.)  The returned List is backed by this
        /// List, so changes in the returned List are reflected in this List, and
        /// vice-versa.  The returned List supports all of the optional List
        /// operations supported by this List.
        /// 
        /// <para>This method eliminates the need for explicit range operations (of
        /// the sort that commonly exist for arrays).  Any operation that expects
        /// a List can be used as a range operation by operating on a subList view
        /// instead of a whole List.  For example, the following idiom
        /// removes a range of elements from a List:
        /// <pre>
        ///      list.subList(from, to).clear();
        /// </pre>
        /// Similar idioms may be constructed for indexOf and lastIndexOf,
        /// and all of the algorithms in the Collections class can be applied to
        /// a subList.
        /// 
        /// </para>
        /// <para>The semantics of the List returned by this method become undefined if
        /// the backing list (i.e., this List) is <i>structurally modified</i> in
        /// any way other than via the returned List.  (Structural modifications are
        /// those that change the size of the List, or otherwise perturb it in such
        /// a fashion that iterations in progress may yield incorrect results.)
        /// 
        /// </para>
        /// </summary>
        /// <param name="fromIndex"> low endpoint (inclusive) of the subList </param>
        /// <param name="toIndex"> high endpoint (exclusive) of the subList </param>
        /// <returns> a view of the specified range within this List </returns>
        /// <exception cref="IndexOutOfBoundsException"> if an endpoint index value is out of range
        ///         {@code (fromIndex < 0 || toIndex > size)} </exception>
        /// <exception cref="IllegalArgumentException"> if the endpoint indices are out of order
        ///         {@code (fromIndex > toIndex)} </exception>
        public virtual IList< E > SubList( int fromIndex, int toIndex )
        {
            lock ( this )
            {
                return Collections.synchronizedList( base.SubList( fromIndex, toIndex ), this );
            }
        }

        /// <summary>
        /// Removes from this list all of the elements whose index is between
        /// {@code fromIndex}, inclusive, and {@code toIndex}, exclusive.
        /// Shifts any succeeding elements to the left (reduces their index).
        /// This call shortens the list by {@code (toIndex - fromIndex)} elements.
        /// (If {@code toIndex==fromIndex}, this operation has no effect.)
        /// </summary>
        protected internal virtual void RemoveRange( int fromIndex, int toIndex )
        {
            lock ( this )
            {
                modCount++;
                int numMoved = elementCount - toIndex;
                Array.Copy( elementData, toIndex, elementData, fromIndex, numMoved );

                // Let gc do its work
                int newElementCount = elementCount - ( toIndex - fromIndex );

                while ( elementCount != newElementCount )
                {
                    elementData[ --elementCount ] = null;
                }
            }
        }

        /// <summary>
        /// Loads a {@code Vector} instance from a stream
        /// (that is, deserializes it).
        /// This method performs checks to ensure the consistency
        /// of the fields.
        /// </summary>
        /// <param name="in"> the stream </param>
        /// <exception cref="java.io.IOException"> if an I/O error occurs </exception>
        /// <exception cref="ClassNotFoundException"> if the stream contains data
        ///         of a non-existing class </exception>
        private void ReadObject( ObjectInputStream @in )
        {
            ObjectInputStream.GetField gfields = @in.readFields();
            int                        count   = gfields.get( "elementCount", 0 );
            object[]                   data    = ( object[] )gfields.get( "elementData", null );

            if ( count < 0 || data == null || count > data.Length )
            {
                throw new StreamCorruptedException( "Inconsistent vector internals" );
            }

            elementCount = count;
            elementData  = ( object[] )data.Clone();
        }

        /// <summary>
        /// Save the state of the {@code Vector} instance to a stream (that
        /// is, serialize it).
        /// This method performs synchronization to ensure the consistency
        /// of the serialized data.
        /// </summary>
//JAVA TO C# CONVERTER WARNING: Method 'throws' clauses are not available in C#:
//ORIGINAL LINE: private void writeObject(java.io.ObjectOutputStream s) throws java.io.IOException
        private void WriteObject( java.io.ObjectOutputStream s )
        {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final java.io.ObjectOutputStream.PutField fields = s.putFields();
            java.io.ObjectOutputStream.PutField fields = s.putFields();
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final Object[] data;
            object[] data;

            lock ( this )
            {
                fields.put( "capacityIncrement", capacityIncrement );
                fields.put( "elementCount", elementCount );
                data = elementData.clone();
            }

            fields.put( "elementData", data );
            s.writeFields();
        }

        /// <summary>
        /// Returns a list iterator over the elements in this list (in proper
        /// sequence), starting at the specified position in the list.
        /// The specified index indicates the first element that would be
        /// returned by an initial call to <seealso cref="ListIterator.next next"/>.
        /// An initial call to <seealso cref="ListIterator.previous previous"/> would
        /// return the element with the specified index minus one.
        /// 
        /// <para>The returned list iterator is <a href="#fail-fast"><i>fail-fast</i></a>.
        /// 
        /// </para>
        /// </summary>
        /// <exception cref="IndexOutOfBoundsException"> {@inheritDoc} </exception>
        public virtual IEnumerator< E > ListIterator( int index )
        {
            lock ( this )
            {
                if ( index < 0 || index > elementCount )
                {
                    throw new System.IndexOutOfRangeException( "Index: " + index );
                }

                return new ListItr( index );
            }
        }

        /// <summary>
        /// Returns a list iterator over the elements in this list (in proper
        /// sequence).
        /// 
        /// <para>The returned list iterator is <a href="#fail-fast"><i>fail-fast</i></a>.
        /// 
        /// </para>
        /// </summary>
        /// <seealso cref=".listIterator(int)"/>
        public virtual IEnumerator< E > ListIterator()
        {
            lock ( this )
            {
                return new ListItr( 0 );
            }
        }

        /// <summary>
        /// Returns an iterator over the elements in this list in proper sequence.
        /// 
        /// <para>The returned iterator is <a href="#fail-fast"><i>fail-fast</i></a>.
        /// 
        /// </para>
        /// </summary>
        /// <returns> an iterator over the elements in this list in proper sequence </returns>
        public virtual IEnumerator< E > Iterator()
        {
            lock ( this )
            {
                return new Itr();
            }
        }

        /// <summary>
        /// An optimized version of AbstractList.Itr
        /// </summary>
        internal class Itr : IEnumerator< E >
        {
            internal int Cursor;                // index of next element to return
            internal int LastRet          = -1; // index of last element returned; -1 if no such
            internal int ExpectedModCount = modCount;

            public bool HasNext()
            {
                // Racy but within spec, since modifications are checked
                // within or after synchronization in next/previous
                return Cursor != ElementCount;
            }

            public E Next()
            {
                lock ( Vector.this)

                {
                    CheckForComodification();
                    int i = Cursor;

                    if ( i >= ElementCount )
                    {
                        throw new NoSuchElementException();
                    }

                    Cursor = i + 1;

                    return GetElementData( LastRet = i );
                }
            }

            public virtual void Remove()
            {
                if ( LastRet == -1 )
                {
                    throw new System.InvalidOperationException();
                }

                lock ( Vector.this)

                {
                    CheckForComodification();
                    Vector.this.remove( LastRet );
                    ExpectedModCount = modCount;
                }

                Cursor  = LastRet;
                LastRet = -1;
            }

//JAVA TO C# CONVERTER TASK: There is no C# equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public void forEachRemaining(Consumer<? super E> action)
            public override void ForEachRemaining<T1>( System.Action< T1 > action )
            {
                Objects.requireNonNull( action );
                lock ( Vector.this)

                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int size = elementCount;
                    int size = elementCount;
                    int i    = Cursor;

                    if ( i >= size )
                    {
                        return;
                    }

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") final E[] elementData = (E[]) Vector.this.elementData;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    E[] elementData = ( E[] )Vector.this.elementData;

                    if ( i >= elementData.Length )
                    {
                        throw new ConcurrentModificationException();
                    }

                    while ( i != size && modCount == ExpectedModCount )
                    {
                        action( elementData[ i++ ] );
                    }

                    // update once at end of iteration to reduce heap write traffic
                    Cursor  = i;
                    LastRet = i - 1;
                    CheckForComodification();
                }
            }

            internal void CheckForComodification()
            {
                if ( modCount != ExpectedModCount )
                {
                    throw new ConcurrentModificationException();
                }
            }
        }

        /// <summary>
        /// An optimized version of AbstractList.ListItr
        /// </summary>
        internal sealed class ListItr : Itr, IEnumerator< E >
        {
            internal ListItr( int index ) : base()
            {
                Cursor = index;
            }

            public bool HasPrevious()
            {
                return Cursor != 0;
            }

            public int NextIndex()
            {
                return Cursor;
            }

            public int PreviousIndex()
            {
                return Cursor - 1;
            }

            public E Previous()
            {
                lock ( Vector.this)

                {
                    CheckForComodification();
                    int i = Cursor - 1;

                    if ( i < 0 )
                    {
                        throw new NoSuchElementException();
                    }

                    Cursor = i;

                    return elementData( LastRet = i );
                }
            }

            public void Set( E e )
            {
                if ( lastRet == -1 )
                {
                    throw new System.InvalidOperationException();
                }

                lock ( Vector< E >.this )

                {
                    CheckForComodification();
                    Vector< E >.this.set( lastRet, e );
                }
            }

            public void Add( E e )
            {
                int i = cursor;
                lock ( Vector.this)

                {
                    CheckForComodification();

                    Vector.this.add( i, e );

                    ExpectedModCount = modCount;
                }

                cursor  = i + 1;
                lastRet = -1;
            }
        }

//JAVA TO C# CONVERTER TASK: There is no C# equivalent to the Java 'super' constraint:
//ORIGINAL LINE: @Override public synchronized void forEach(Consumer<? super E> action)
        public override void ForEach<T1>( System.Action< T1 > action )
        {
            lock ( this )
            {
                Objects.requireNonNull( action );
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int expectedModCount = modCount;
                int expectedModCount = modCount;
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") final E[] elementData = (E[]) this.elementData;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                E[] elementData = ( E[] )this.elementData;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int elementCount = this.elementCount;
                int elementCount = this.elementCount;

                for ( int i = 0; modCount == expectedModCount && i < elementCount; i++ )
                {
                    action( elementData[ i ] );
                }

                if ( modCount != expectedModCount )
                {
                    throw new ConcurrentModificationException();
                }
            }
        }

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Override @SuppressWarnings("unchecked") public synchronized boolean removeIf(Predicate<? super E> filter)
//JAVA TO C# CONVERTER TASK: There is no C# equivalent to the Java 'super' constraint:
        public override bool RemoveIf<T1>( System.Predicate< T1 > filter )
        {
            lock ( this )
            {
                Objects.requireNonNull( filter );
                // figure out which elements are to be removed
                // any exception thrown from the filter predicate at this stage
                // will leave the collection unmodified
                int removeCount = 0;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int size = elementCount;
                int size = elementCount;
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final BitSet removeSet = new BitSet(size);
                BitArray removeSet = new BitArray( size );
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int expectedModCount = modCount;
                int expectedModCount = modCount;

                for ( int i = 0; modCount == expectedModCount && i < size; i++ )
                {
//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") final E element = (E) elementData[i];
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
                    E element = ( E )elementData[ i ];

                    if ( filter( element ) )
                    {
                        removeSet.Set( i, true );
                        removeCount++;
                    }
                }

                if ( modCount != expectedModCount )
                {
                    throw new ConcurrentModificationException();
                }

                // shift surviving elements left over the spaces left by removed elements
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final boolean anyToRemove = removeCount > 0;
                bool anyToRemove = removeCount > 0;

                if ( anyToRemove )
                {
//JAVA TO C# CONVERTER WARNING: The original Java variable was marked 'final':
//ORIGINAL LINE: final int newSize = size - removeCount;
                    int newSize = size - removeCount;

                    for ( int i = 0, j = 0; ( i < size ) && ( j < newSize ); i++, j++ )
                    {
                        i                = removeSet.nextClearBit( i );
                        elementData[ j ] = elementData[ i ];
                    }

                    for ( int k = newSize; k < size; k++ )
                    {
                        elementData[ k ] = null; // Let gc do its work
                    }

                    elementCount = newSize;

                    if ( modCount != expectedModCount )
                    {
                        throw new ConcurrentModificationException();
                    }

                    modCount++;
                }

                return anyToRemove;
            }
        }

        public override void ReplaceAll( System.Func< E, E > @operator )
        {
            lock ( this )
            {
                Objects.requireNonNull( @operator );
                int expectedModCount = modCount;
                int size             = elementCount;

                for ( int i = 0; modCount == expectedModCount && i < size; i++ )
                {
                    elementData[ i ] = @operator( ( E )elementData[ i ] );
                }

                if ( modCount != expectedModCount )
                {
                    throw new ConcurrentModificationException();
                }

                modCount++;
            }
        }

        public override void Sort<T1>( IComparer< T1 > c )
        {
            lock ( this )
            {
                int expectedModCount = modCount;
                Array.Sort( ( E[] )elementData, 0, elementCount, c );

                if ( modCount != expectedModCount )
                {
                    throw new ConcurrentModificationException();
                }

                modCount++;
            }
        }

        /// <summary>
        /// Creates a <em><a href="Spliterator.html#binding">late-binding</a></em>
        /// and <em>fail-fast</em> <seealso cref="Spliterator"/> over the elements in this
        /// list.
        /// 
        /// <para>The {@code Spliterator} reports <seealso cref="Spliterator.SIZED"/>,
        /// <seealso cref="Spliterator.SUBSIZED"/>, and <seealso cref="Spliterator.ORDERED"/>.
        /// Overriding implementations should document the reporting of additional
        /// characteristic values.
        /// 
        /// </para>
        /// </summary>
        /// <returns> a {@code Spliterator} over the elements in this list
        /// @since 1.8 </returns>
        public override Spliterator< E > Spliterator()
        {
            return new VectorSpliterator< E >( this, null, 0, -1, 0 );
        }

        /// <summary>
        /// Similar to ArrayList Spliterator </summary>
        internal sealed class VectorSpliterator<E> : Spliterator< E >
        {
            private readonly List< E > _list;
            private          object[]  _array;
            private          int       _index;            // current index, modified on advance/split
            private          int       _fence;            // -1 until used; then one past last index
            private          int       _expectedModCount; // initialized when fence set

            /// <summary>
            /// Create new spliterator covering the given  range </summary>
            internal VectorSpliterator( List< E > list, object[] array, int origin, int fence, int expectedModCount )
            {
                this._list             = list;
                this._array            = array;
                this._index            = origin;
                this._fence            = fence;
                this._expectedModCount = expectedModCount;
            }

            private int Fence
            {
                get
                {
                    // initialize on first use
                    int hi;

                    if ( ( hi = _fence ) < 0 )
                    {
                        lock ( _list )
                        {
                            _array            = _list.elementData;
                            _expectedModCount = _list.modCount;
                            hi                = _fence = _list.elementCount;
                        }
                    }

                    return hi;
                }
            }

            public Spliterator< E > TrySplit()
            {
                int hi = Fence, lo = _index, mid = ( int )( ( uint )( lo + hi ) >> 1 );

                return ( lo >= mid ) ? null : new VectorSpliterator< E >( _list, _array, lo, _index = mid, _expectedModCount );
            }

//JAVA TO C# CONVERTER TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public boolean tryAdvance(Consumer<? super E> action)
//JAVA TO C# CONVERTER TASK: There is no C# equivalent to the Java 'super' constraint:
            public bool TryAdvance<T1>( System.Action< T1 > action )
            {
                int i;

                if ( action == null )
                {
                    throw new System.NullReferenceException();
                }

                if ( Fence > ( i = _index ) )
                {
                    _index = i + 1;
                    action( ( E )_array[ i ] );

                    if ( _list.modCount != _expectedModCount )
                    {
                        throw new ConcurrentModificationException();
                    }

                    return true;
                }

                return false;
            }

            public void ForEachRemaining<T1>( System.Action< T1 > action )
            {
                int       i;  // hoist accesses and checks from loop
                int       hi; // hoist accesses and checks from loop
                List< E > lst;
                object[]  a;

                if ( action == null )
                {
                    throw new System.NullReferenceException();
                }

                if ( ( lst = list ) != null )
                {
                    if ( ( hi = fence ) < 0 )
                    {
                        lock ( lst )
                        {
                            expectedModCount = lst.modCount;
                            a                = array = lst.elementData;
                            hi               = fence = lst.elementCount;
                        }
                    }
                    else
                    {
                        a = array;
                    }

                    if ( a != null && ( i = index ) >= 0 && ( index = hi ) <= a.Length )
                    {
                        while ( i < hi )
                        {
                            action( ( E )a[ i++ ] );
                        }

                        if ( lst.modCount == expectedModCount )
                        {
                            return;
                        }
                    }
                }

                throw new ConcurrentModificationException();
            }

            public long EstimateSize()
            {
                return ( long )( GetFence() - index );
            }

            public int Characteristics()
            {
                return Spliterator.ORDERED | Spliterator.SIZED | Spliterator.SUBSIZED;
            }
        }
    }
}
