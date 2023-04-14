using System.Collections;

namespace LibGDXSharp.Utils
{
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

            this.ElementData       = new T[ initialCapacity ];
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
            ElementData  = collection.ToArray();
            ElementCount = ElementData.Length;

            if ( ElementData.GetType() != typeof(object[]) )
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
                if ( ElementData != null )
                {
                    Array.Copy
                        (
                         ElementData,
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

                var oldCapacity = ElementData.Length;

                if ( ElementCount < oldCapacity )
                {
                    ElementData = Array.CopyOf( ElementData, ElementCount );
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
        public  void EnsureCapacity( int minCapacity )
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
            if ( minCapacity - ElementData?.Length > 0 )
            {
                Grow( minCapacity );
            }
        }

        /// <summary>
        /// The maximum size of array to allocate.
        /// </summary>
        private const int MaxArraySize = int.MaxValue - 8;
    }
}
