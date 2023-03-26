namespace LibGDXSharp.Utils
{
    /// <summary>
    /// <p>
    /// <b>A container for data of a specific primitive type.</b>
    /// </p>
    /// <p>
    /// A buffer is a linear, finite sequence of elements of a specific primitive type.
    /// Aside from its content, the essential properties of a buffer are its capacity,
    /// limit, and position:
    /// </p>
    /// <p></p>
    /// <p></p>
    ///
    /// <p>
    ///     A buffer's <b><i>capacity</i></b> is the number of elements it contains. The capacity
    ///     of a buffer is never negative and never changes.
    /// </p>
    /// <p></p>
    /// <p>
    ///     A buffer's <b><i>limit</i></b> is the index of the first element that should not be
    ///     read or written. A buffer's limit is never negative and is never greater than
    ///     its capacity.
    /// </p>
    /// <p></p>
    /// <p>
    ///     A buffer's <b><i>position</i></b> is the index of the next element to be read or written.
    ///     A buffer's position is never negative and is never greater than its limit.
    /// </p>
    /// <p></p>
    /// <p></p>
    ///
    /// <p>
    /// There is one subclass of this class for each non-boolean primitive type.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Transferring data</b>
    /// <p><b>-----------------</b></p>
    /// Each subclass of this class defines two categories of get and put operations:
    /// Relative operations read or write one or more elements starting at the current
    /// position and then increment the position by the number of elements transferred.
    /// If the requested transfer exceeds the limit then a relative get operation
    /// throws a BufferUnderflowException and a relative put operation throws a
    /// BufferOverflowException; in either case, no data is transferred.
    /// Absolute operations take an explicit element index and do not affect the position.
    /// Absolute get and put operations throw an IndexOutOfBoundsException if the index
    /// argument exceeds the limit.
    /// Data may also, of course, be transferred in to or out of a buffer by the I/O
    /// operations of an appropriate channel, which are always relative to the current
    /// position.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Marking and resetting</b>
    /// <p><b>---------------------</b></p>
    /// A buffer's mark is the index to which its position will be reset when the reset
    /// method is invoked. The mark is not always defined, but when it is defined it is
    /// never negative and is never greater than the position.
    /// If the mark is defined then it is discarded when the position or the limit is
    /// adjusted to a value smaller than the mark. If the mark is not defined then invoking
    /// the reset method causes an InvalidMarkException to be thrown.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Invariants</b>
    /// <p><b>----------</b></p>
    /// The following invariant holds for the mark, position, limit, and capacity values:
    /// <p></p>
    /// 
    /// 0 &lt;= mark &lt;= position &lt;= limit &lt;= capacity
    /// 
    /// <p></p>
    /// A newly-created buffer always has a position of zero and a mark that is undefined.
    /// The initial limit may be zero, or it may be some other value that depends upon the
    /// type of the buffer and the manner in which it is constructed. Each element of a
    /// newly-allocated buffer is initialized to zero.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Clearing, flipping, and rewinding</b>
    /// <p><b>---------------------------------</b></p>
    /// In addition to methods for accessing the position, limit, and capacity values and
    /// for marking and resetting, this class also defines the following operations upon
    /// buffers:
    /// <b>clear</b> makes a buffer ready for a new sequence of channel-read or relative put
    /// operations: It sets the limit to the capacity and the position to zero.
    /// <b>flip</b> makes a buffer ready for a new sequence of channel-write or relative get
    /// operations: It sets the limit to the current position and then sets the position to
    /// zero.
    /// <b>rewind</b> makes a buffer ready for re-reading the data that it already contains:
    /// It leaves the limit unchanged and sets the position to zero.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Read-only buffers</b>
    /// <p><b>-----------------</b></p>
    /// Every buffer is readable, but not every buffer is writable. The mutation methods of
    /// each buffer class are specified as optional operations that will throw a
    /// ReadOnlyBufferException when invoked upon a read-only buffer. A read-only buffer
    /// does not allow its content to be changed, but its mark, position, and limit values
    /// are mutable. Whether or not a buffer is read-only may be determined by invoking its
    /// IsReadOnly method.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Thread safety</b>
    /// <p><b>-------------</b></p>
    /// Buffers are not safe for use by multiple concurrent threads. If a buffer is to be
    /// used by more than one thread then access to the buffer should be controlled by
    /// appropriate synchronization.
    /// </p>
    /// <p></p>
    ///
    /// <p>
    /// <b>Invocation chaining</b>
    /// <p><b>-------------------</b></p>
    /// Methods in this class that do not otherwise have a value to return are specified
    /// to return the buffer upon which they are invoked. This allows method invocations
    /// to be chained; for example, the sequence of statements
    /// <p></p>
    /// <p>  b.flip();</p>
    /// <p>  b.position(23);</p>
    /// <p>  b.limit(42);</p>
    /// <p></p>
    /// <p>
    /// can be replaced by the single, more compact statement
    /// </p>
    /// <p></p>
    /// <p>
    ///   b.flip().position(23).limit(42);
    /// </p>
    /// </p>
    /// <p></p>
    /// </summary>
    public abstract class Buffer
    {
        // The characteristics of Spliterators that traverse and split elements
        // maintained in Buffers.
//        private readonly static int spliteratorCharacteristics =
//            Spliterator.SIZED | Spliterator.SUBSIZED | Spliterator.ORDERED;

        // Invariants: mark <= position <= limit <= capacity
        private int _mark     = -1;
        private int _position = 0;
        private int _limit;
        private int _capacity;

        // Used only by direct buffers
        // NOTE: hoisted here for speed in JNI GetDirectBufferAddress
        long address;

    }
}
