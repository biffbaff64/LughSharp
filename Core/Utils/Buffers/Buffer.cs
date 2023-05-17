using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Utils;

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
/// <p>
/// There is one subclass of this class for each non-bool primitive type.
/// </p>
/// <p></p>
/// <p>
/// <b>Transferring data</b>
/// <p><b>-----------------</b></p>
/// Each subclass of this class defines two categories of get and put operations:
/// Relative operations read or write one or more elements starting at the current
/// position and then increment the position by the number of elements transferred.
/// If the requested transfer exceeds the limit then a relative get operation throws
/// a BufferUnderflowException and a relative put operation throws a BufferOverflowException;
/// in either case, no data is transferred. Absolute operations take an explicit element
/// index and do not affect the position. Absolute get and put operations throw an
/// IndexOutOfBoundsException if the index argument exceeds the limit. Data may also,
/// of course, be transferred in to or out of a buffer by the I/O operations of an
/// appropriate channel, which are always relative to the current position.
/// </p>
/// <p></p>
/// <p>
/// <b>Marking and resetting</b>
/// <p><b>---------------------</b></p>
/// A buffer's mark is the index to which its position will be reset when the reset
/// method is invoked. The mark is not always defined, but when it is defined it is
/// never negative and is never greater than the position. If the mark is defined then
/// it is discarded when the position or the limit is adjusted to a value smaller than
/// the mark. If the mark is not defined then invoking the reset method causes an
/// InvalidMarkException to be thrown.
/// </p>
/// <p></p>
/// <p>
/// <b>Invariants</b>
/// <p><b>----------</b></p>
/// The following invariant holds for the mark, position, limit, and capacity values:
/// <p></p>
/// <tt><i>0 &lt;= mark &lt;= position &lt;= limit &lt;= capacity</i></tt>
/// <p></p>
/// A newly-created buffer always has a position of zero and a mark that is undefined.
/// The initial limit may be zero, or it may be some other value that depends upon the
/// type of the buffer and the manner in which it is constructed. Each element of a
/// newly-allocated buffer is initialized to zero.
/// </p>
/// <p></p>
/// <p>
/// <b>Clearing, flipping, and rewinding</b>
/// <p><b>---------------------------------</b></p>
/// In addition to methods for accessing the position, limit, and capacity values and
/// for marking and resetting, this class also defines the following operations upon
/// buffers:
/// <p>
/// <b>Clear</b> makes a buffer ready for a new sequence of channel-read or relative put
/// operations: It sets the limit to the capacity and the position to zero.
/// </p>
/// <p>
/// <b>Flip</b> makes a buffer ready for a new sequence of channel-write or relative get
/// operations: It sets the limit to the current position and then sets the position to
/// zero.
/// </p>
/// <p>
/// <b>Rewind</b> makes a buffer ready for re-reading the data that it already contains:
/// It leaves the limit unchanged and sets the position to zero.
/// </p>
/// </p>
/// <p></p>
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
/// <p>
/// <b>Thread safety</b>
/// <p><b>-------------</b></p>
/// Buffers are not safe for use by multiple concurrent threads. If a buffer is to be
/// used by more than one thread then access to the buffer should be controlled by
/// appropriate synchronization.
/// </p>
/// <p></p>
/// <p>
/// <b>Invocation chaining</b>
/// <p><b>-------------------</b></p>
/// Methods in this class that do not otherwise have a value to return are specified
/// to return the buffer upon which they are invoked. This allows method invocations
/// to be chained; for example, the sequence of statements
/// <p></p>
/// <tt><i>
/// <p>  b.flip();</p>
/// <p>  b.position(23);</p>
/// <p>  b.limit(42);</p>
/// </i></tt>
/// <p></p>
/// <p>
/// can be replaced by the single, more compact statement
/// </p>
/// <p></p>
/// <p>
/// <tt><i>  b.flip().position(23).limit(42);</i></tt>
/// </p>
/// </p>
/// <p></p>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class Buffer
{
    // Invariants: mark <= position <= limit <= capacity
    public int Mark     { get; set; } = -1;
    public int Capacity { get; set; }
    public int Position { get; set; } = 0;
    public int Limit    { get; set; }

    protected Buffer()
    {
    }
        
    /// <summary>
    /// Creates a new buffer with the given mark, position, limit, and capacity,
    /// after checking invariants.
    /// </summary>
    protected Buffer( int mark, int pos, int lim, int cap )
    {
        if ( cap < 0 )
        {
            throw new System.ArgumentException( "Negative capacity: " + cap );
        }

        this.Capacity = cap;
        
        SetLimit( lim );
        SetPosition( pos );

        if ( mark >= 0 )
        {
            if ( mark > pos )
            {
                throw new System.ArgumentException( "mark > position: (" + mark + " > " + pos + ")" );
            }

            this.Mark = mark;
        }
    }

    /// <summary>
    /// Sets this buffer's position. If the mark is defined and larger than the
    /// new position then it is discarded.
    /// </summary>
    /// <param name="newPosition">
    ///         The new position value; must be non-negative
    ///         and no larger than the current limit
    /// </param>
    /// <returns>This buffer</returns>
    /// <exception cref="ArgumentException">
    /// If the preconditions on <tt>newPosition</tt> do not hold
    /// </exception>
    public Buffer SetPosition( int newPosition )
    {
        if ( ( newPosition > Limit ) || ( newPosition < 0 ) )
        {
            throw new System.ArgumentException();
        }

        Position = newPosition;

        if ( Mark > Position )
        {
            Mark = -1;
        }

        return this;
    }

    /// <summary>
    /// Sets this buffer's limit.  If the position is larger than the new limit
    /// then it is set to the new limit.  If the mark is defined and larger than
    /// the new limit then it is discarded.
    /// </summary>
    /// <param name="newLimit">
    ///         The new limit value; must be non-negative
    ///         and no larger than this buffer's capacity
    /// </param>
    /// <returns>  This buffer
    /// </returns>
    /// <exception cref="ArgumentException">
    ///          If the preconditions on <tt>newLimit</tt> do not hold
    /// </exception>
    public Buffer SetLimit( int newLimit )
    {
        if ( ( newLimit > Capacity ) || ( newLimit < 0 ) )
        {
            throw new System.ArgumentException();
        }

        Limit = newLimit;

        if ( Position > Limit ) Position = Limit;

        if ( Mark > Limit ) Mark = -1;

        return this;
    }

    /// <summary>
    /// Sets this buffer's mark at its position.
    /// </summary>
    /// <returns>This buffer</returns>
    public Buffer SetMark()
    {
        Mark = Position;

        return this;
    }

    /// <summary>
    /// Resets this buffer's position to the previously-marked position.
    /// <para>
    /// Invoking this method neither changes nor discards the mark's value.
    /// </para>
    /// </summary>
    /// <returns>This buffer</returns>
    /// <exception cref="InvalidOperationException">
    ///          If the mark has not been set
    /// </exception>
    public Buffer Reset()
    {
        var m = Mark;

        if ( m < 0 ) throw new InvalidOperationException();

        Position = m;

        return this;
    }

    /// <summary>
    /// Clears this buffer.  The position is set to zero, the limit is set to
    /// the capacity, and the mark is discarded.
    /// 
    /// <para>
    /// Invoke this method before using a sequence of channel-read or <i>put</i>
    /// operations to fill this buffer.  For example:
    ///
    /// <para></para>
    /// <para>buf.clear();     // Prepare buffer for reading</para>
    /// <para>in.read(buf);    // Read data</para>
    /// 
    /// </para>
    /// <para>
    /// This method does not actually erase the data in the buffer, but it
    /// is named as if it did because it will most often be used in situations
    /// in which that might as well be the case.
    /// </para>
    /// </summary>
    /// <returns>  This buffer </returns>
    public Buffer Clear()
    {
        Position = 0;
        Limit    = Capacity;
        Mark     = -1;

        return this;
    }

    /// <summary>
    /// Flips this buffer.  The limit is set to the current position and then
    /// the position is set to zero.  If the mark is defined then it is
    /// discarded.
    /// 
    /// <para> After a sequence of channel-read or <i>put</i> operations, invoke
    /// this method to prepare for a sequence of channel-write or relative
    /// <tt><i>get</i></tt> operations.  For example:
    /// </para>
    /// 
    /// <para>buf.put(magic);    // Prepend header</para>
    /// <para>in.read(buf);      // Read data into rest of buffer</para>
    /// <para>buf.flip();        // Flip buffer</para>
    /// <para>out.write(buf);    // Write header + data to channel</para>
    /// <para>
    /// This method is often used in conjunction with the <see cref="ByteBuffer.Compact()"/>
    /// method when transferring data from one place to another.
    /// </para>
    /// </summary>
    /// <returns>This buffer</returns>
    public Buffer Flip()
    {
        Limit    = Position;
        Position = 0;
        Mark     = -1;

        return this;
    }

    /// <summary>
    /// Rewinds this buffer.  The position is set to zero and the mark is
    /// discarded.
    /// <para> Invoke this method before a sequence of channel-write or <i>get</i>
    /// operations, assuming that the limit has already been set
    /// appropriately.  For example:
    /// <para></para>
    /// <para>out.write(buf);    // Write remaining data</para>
    /// <para>buf.rewind();      // Rewind buffer</para>
    /// <para>buf.get(array);    // Copy data into array</para>
    /// 
    /// </para>
    /// </summary>
    /// <returns>  This buffer </returns>
    public Buffer Rewind()
    {
        Position = 0;
        Mark     = -1;

        return this;
    }

    /// <summary>
    /// </summary>
    /// <returns>  The number of elements remaining in this buffer </returns>
    public int Remaining() => ( Limit - Position );

    /// <summary>
    /// Tells whether there are any elements between the current position and
    /// the limit.
    /// </summary>
    /// <returns>
    /// <tt>true</tt> if there is at least one element remaining in this buffer.
    /// </returns>
    public bool HasRemaining() => ( Position < Limit );

    /// <summary>
    /// Tells whether or not this buffer is read-only.
    /// </summary>
    /// <returns>  <tt>true</tt> if this buffer is read-only </returns>
    public abstract bool ReadOnly { get; }

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible
    /// array.
    /// 
    /// <para>
    /// If this method returns <tt>true</tt> then the <see cref="Array()"/>
    /// and <see cref="ArrayOffset()"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>
    /// <tt>true</tt> if this buffer is backed by an array and is not read-only.
    /// </returns>
    public abstract bool HasArray();

    /// <summary>
    /// Returns the array that backs this buffer.
    /// 
    /// <para>
    /// Modifications to this buffer's content will cause the returned array's
    /// content to be modified, and vice versa.
    /// </para>
    /// 
    /// <para>
    /// Invoke the <see cref="HasArray()"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The array that backs this buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public abstract object Array();

    /// <summary>
    /// Returns the offset within this buffer's backing array of the first
    /// element of the buffer <i>(optional operation)</i>.
    /// 
    /// <para>
    /// If this buffer is backed by an array then buffer position <i>p</i>
    /// corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
    /// </para>
    /// <para>
    /// Invoke the <see cref="HasArray"/> method before invoking this method
    /// in order to ensure that this buffer has an accessible backing array.
    /// </para>
    /// </summary>
    /// <returns>
    /// The offset within this buffer's array of the first element of the buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// If this buffer is not backed by an accessible array
    /// </exception>
    public abstract int ArrayOffset();

    /// <summary>
    /// Tells whether or not this buffer is <tt><i>direct</i></tt>
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct</returns>
    public abstract bool Direct { get; }

    // -------------------------------------------------------
    // -- Package-private methods for bounds checking, etc. --
    // -------------------------------------------------------

    /// <summary>
    /// Checks the current position against the limit, throwing a {@link
    /// BufferUnderflowException} if it is not smaller than the limit, and then
    /// increments the position.
    /// </summary>
    /// <returns>The current position value, before it is incremented.</returns>
    internal int NextGetIndex()
    {
        if ( Position >= Limit )
        {
            throw new BufferUnderflowException();
        }

        return Position++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nb"></param>
    /// <returns></returns>
    /// <exception cref="BufferUnderflowException"></exception>
    internal int NextGetIndex( int nb )
    {
        if ( ( Limit - Position ) < nb )
        {
            throw new BufferUnderflowException();
        }

        var p = Position;
        Position += nb;

        return p;
    }

    /// <summary>
    /// Checks the current position against the limit, throwing a {@link
    /// BufferOverflowException} if it is not smaller than the limit, and then
    /// increments the position.
    /// </summary>
    /// <returns>  The current position value, before it is incremented </returns>
    internal int NextPutIndex()
    {
        if ( Position >= Limit )
        {
            throw new BufferOverflowException();
        }

        return Position++;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nb"></param>
    /// <returns></returns>
    /// <exception cref="BufferOverflowException"></exception>
    internal int NextPutIndex( int nb )
    {
        if ( ( Limit - Position ) < nb )
        {
            throw new BufferOverflowException();
        }

        var p = Position;
        Position += nb;

        return p;
    }

    /// <summary>
    /// Checks the given index against the limit, throwing an
    /// <see cref="IndexOutOfRangeException"/> if it is not smaller
    /// than the limit or is smaller than zero.
    /// </summary>
    internal int CheckIndex( int i )
    {
        if ( ( i < 0 ) || ( i >= Limit ) )
        {
            throw new System.IndexOutOfRangeException();
        }

        return i;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="i"></param>
    /// <param name="nb"></param>
    /// <returns></returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    internal int CheckIndex( int i, int nb )
    {
        if ( ( i < 0 ) || ( nb > ( Limit - i ) ) )
        {
            throw new System.IndexOutOfRangeException();
        }

        return i;
    }

    /// <summary>
    /// 
    /// </summary>
    internal void Truncate()
    {
        Mark     = -1;
        Position = 0;
        Limit    = 0;
        Capacity = 0;
    }

    internal void DiscardMark() => Mark = -1;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="off"></param>
    /// <param name="len"></param>
    /// <param name="size"></param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    internal static void CheckBounds( int off, int len, int size )
    {
        if ( ( off | len | ( off + len ) | ( size - ( off + len ) ) ) < 0 )
        {
            throw new System.IndexOutOfRangeException();
        }
    }
}