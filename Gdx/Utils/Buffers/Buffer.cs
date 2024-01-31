// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LibGDXSharp.Gdx.Utils.Buffers;

/// <summary>
///     A container for data of a specific primitive type.
///     <para>
///         A buffer is a linear, finite sequence of elements of a specific primitive type.
///         Aside from its content, the essential properties of a buffer are its capacity,
///         limit, and position:
///     </para>
///     <tt>
///         <para>
///             A buffer's <i>capacity</i> is the number of elements it contains. The
///             capacity of a buffer is never negative and never changes.
///         </para>
///         <para>
///             A buffer's <i>limit</i> is the index of the first element that should
///             not be read or written.  A buffer's limit is never negative and is never
///             greater than its capacity.
///         </para>
///         <para>
///             A buffer's <i>position</i> is the index of the next element to be
///             read or written.  A buffer's position is never negative and is never
///             greater than its limit.
///         </para>
///     </tt>
///     <para>
///         There is one subclass of this class for each non-bool primitive type.
///     </para>
///     <para></para>
///     <b> Transferring data </b>
///     <para>
///         Each subclass of this class defines two categories of <i>get</i> and
///         <i>put</i> operations:
///     </para>
///     <tt>
///         <para>
///             <i>Relative</i> operations read or write one or more elements starting
///             at the current position and then increment the position by the number of
///             elements transferred.  If the requested transfer exceeds the limit then a
///             relative <i>Get</i> or <i>Put</i> operation throws a <see cref="GdxRuntimeException" />..
///         </para>
///         <para>
///             <i>Absolute</i> operations take an explicit element index and do not
///             affect the position.  Absolute <i>get</i> and <i>put</i> operations throw
///             an <see cref="IndexOutOfRangeException" /> if the index argument exceeds the
///             limit.
///         </para>
///     </tt>
///     <para>
///         Data may also, of course, be transferred in to or out of a buffer by the
///         I/O operations of an appropriate channel, which are always relative to the
///         current position.
///     </para>
///     <para></para>
///     <b> Marking and resetting </b>
///     <para>
///         A buffer's <i>mark</i> is the index to which its position will be reset
///         when the <see cref="Reset" /> method is invoked.  The mark is not always
///         defined, but when it is defined it is never negative and is never greater
///         than the position.  If the mark is defined then it is discarded when the
///         position or the limit is adjusted to a value smaller than the mark.  If the
///         mark is not defined then invoking the {@link #reset reset} method causes an
///         <see cref="GdxRuntimeException" /> to be thrown.
///     </para>
///     <para></para>
///     <b> Invariants </b>
///     <para>
///         The following invariant holds for the mark, position, limit, and capacity values:
///         <tt>
///             <tt>0</tt> <tt>&lt;=</tt>
///             <i>mark</i> <tt>&lt;=</tt>
///             <i>position</i> <tt>&lt;=</tt>
///             <i>limit</i> <tt>&lt;=</tt>
///             <i>capacity</i>
///         </tt>
///     </para>
///     <para>
///         A newly-created buffer always has a position of zero and a mark that is
///         undefined.  The initial limit may be zero, or it may be some other value
///         that depends upon the type of the buffer and the manner in which it is
///         constructed.  Each element of a newly-allocated buffer is initialized
///         to zero.
///     </para>
///     <para></para>
///     <b> Clearing, flipping, and rewinding </b>
///     <para>
///         In addition to methods for accessing the position, limit, and capacity
///         values and for marking and resetting, this class also defines the following
///         operations upon buffers:
///     </para>
///     <ul>
///         <para>
///             <see cref="Clear" /> makes a buffer ready for a new sequence of channel-read or
///             relative <i>put</i> operations: It sets the limit to the capacity and the position
///             to zero.
///         </para>
///         <para>
///             <see cref="Flip" /> makes a buffer ready for a new sequence of channel-write or
///             relative <i>get</i> operations: It sets the limit to the current position and
///             then sets the position to zero.
///         </para>
///         <para>
///             <see cref="Rewind" /> makes a buffer ready for re-reading the data that it already
///             contains: It leaves the limit unchanged and sets the position to zero.
///         </para>
///     </ul>
///     <para></para>
///     <b> Read-only buffers </b>
///     <para>
///         Every buffer is readable, but not every buffer is writable.  The mutation methods
///         of each buffer class are specified as <i>optional operations</i> that will throw a
///         <see cref="GdxRuntimeException" /> when invoked upon a read-only buffer. A read-only
///         buffer does not allow its content to be changed, but its mark, position, and limit
///         values are mutable. Whether or not a buffer is read-only may be determined by checking
///         its <see cref="IsReadOnly" /> property.
///     </para>
///     <para></para>
///     <b> Thread safety </b>
///     <para>
///         Buffers are not safe for use by multiple concurrent threads.  If a buffer is to be
///         used by more than one thread then access to the buffer should be controlled by
///         appropriate synchronization.
///     </para>
///     <para></para>
///     <b> Invocation chaining </b>
///     <para>
///         Methods in this class that do not otherwise have a value to return are specified to
///         return the buffer upon which they are invoked.  This allows method invocations to be
///         chained; for example, the sequence of statements
///         <code>
///             b.flip();
///             b.position(23);
///             b.limit(42);
///         </code>
///         can be replaced by the single, more compact statement
///         <code>
///             b.flip().position(23).limit(42);
///         </code>
///     </para>
/// </summary>
[PublicAPI]
public abstract class Buffer
{
    /// <summary>
    ///     The characteristics of Spliterators that traverse and split elements
    ///     maintained in Buffers.
    /// </summary>
    #if USING_SPLITERATOR //TODO:
    internal readonly static int SpliteratorCharacteristics =
        Spliterator.SIZED | Spliterator.SUBSIZED | Spliterator.ORDERED;
    #endif

    // Invariants: mark <= position <= limit <= capacity
    private int _mark = -1;

    protected Buffer()
    {
    }

    /// <summary>
    ///     Creates a new buffer with the given mark, position, limit, and capacity,
    ///     after checking invariants.
    /// </summary>
    protected Buffer( int mark, int pos, int lim, int cap )
    {
        if ( cap < 0 )
        {
            throw new ArgumentException( "Negative capacity: " + cap );
        }

        Capacity = cap;

        SetLimit( lim );
        SetPosition( pos );

        if ( mark >= 0 )
        {
            if ( mark > pos )
            {
                throw new ArgumentException( "mark > position: (" + mark + " > " + pos + ")" );
            }

            _mark = mark;
        }
    }

    /// <summary>
    ///     Used only by DirectBuffers
    /// </summary>
    public long Address { get; set; }

    /// <summary>
    ///     Returns this buffers capacity
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    ///     Returns this buffer's limit.
    /// </summary>
    public int Limit { get; set; }

    /// <summary>
    ///     Returns this buffers position
    /// </summary>
    public int Position { get; set; } = 0;

    /// <summary>
    ///     Returns <tt>true</tt> if, and only if, this buffer is read-only
    /// </summary>
    public virtual bool IsReadOnly { get; set; }

    /// <summary>
    ///     Sets this buffer's position.  If the mark is defined and larger than the
    ///     new position then it is discarded.
    /// </summary>
    /// <param name="newPosition">
    ///     The new position value; must be non-negative and no larger than the current limit
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="ArgumentException">
    ///     If the preconditions on <tt>newPosition</tt> do not hold
    /// </exception>
    public Buffer SetPosition( int newPosition )
    {
        if ( ( newPosition > Limit ) || ( newPosition < 0 ) )
        {
            throw new ArgumentException( $"newPosition: {newPosition}, Limit: {Limit}" );
        }

        Position = newPosition;

        if ( _mark > Position )
        {
            _mark = -1;
        }

        return this;
    }

    /// <summary>
    ///     Sets this buffer's limit.  If the position is larger than the new limit
    ///     then it is set to the new limit.  If the mark is defined and larger than
    ///     the new limit then it is discarded.
    /// </summary>
    /// <param name="newLimit">
    ///     The new limit value; must be non-negative and no larger than this buffer's capacity
    /// </param>
    /// <returns>  This buffer </returns>
    /// <exception cref="ArgumentException">
    ///     If the preconditions on <tt>newLimit</tt> do not hold
    /// </exception>
    public Buffer SetLimit( int newLimit )
    {
        if ( ( newLimit > Capacity ) || ( newLimit < 0 ) )
        {
            throw new ArgumentException( $"newLimit: {newLimit}, Capacity: {Capacity}" );
        }

        Limit = newLimit;

        if ( Position > Limit )
        {
            Position = Limit;
        }

        if ( _mark > Limit )
        {
            _mark = -1;
        }

        return this;
    }

    /// <summary>
    ///     Sets this buffer's mark at its position.
    /// </summary>
    /// <returns> This buffer </returns>
    public Buffer Mark()
    {
        _mark = Position;

        return this;
    }

    /// <summary>
    ///     Resets this buffer's position to the previously-marked position.
    ///     <para>
    ///         Invoking this method neither changes nor discards the mark's value.
    ///     </para>
    /// </summary>
    /// <returns> This buffer </returns>
    /// <exception cref="GdxRuntimeException">If the mark has not been set</exception>
    public Buffer Reset()
    {
        int m;

        if ( ( m = _mark ) < 0 )
        {
            throw new GdxRuntimeException( "Mark has not been set" );
        }

        Position = m;

        return this;
    }

    /// <summary>
    ///     Clears this buffer. The position is set to zero, the limit is set to
    ///     the capacity, and the mark is discarded.
    ///     <para>
    ///         Invoke this method before using a sequence of channel-read or <i>put</i>
    ///         operations to fill this buffer.
    ///     </para>
    ///     <para>
    ///         For example:
    ///         <code>
    /// buf.clear();     // Prepare buffer for reading
    /// in.read(buf);    // Read data
    /// </code>
    ///     </para>
    ///     <para>
    ///         This method does not actually erase the data in the buffer, but it
    ///         is named as if it did because it will most often be used in situations
    ///         in which that might as well be the case.
    ///     </para>
    /// </summary>
    /// <returns>  This buffer </returns>
    public Buffer Clear()
    {
        Position = 0;
        Limit    = Capacity;
        _mark    = -1;

        return this;
    }

    /// <summary>
    ///     Flips this buffer. The limit is set to the current position and then
    ///     the position is set to zero. If the mark is defined then it is discarded.
    ///     <para>
    ///         After a sequence of channel-read or <i>put</i> operations, invoke this
    ///         method to prepare for a sequence of channel-write or relative <i>get</i>
    ///         operations.
    ///     </para>
    ///     <para>
    ///         For example:
    ///         <code>
    ///     buf.Put(magic);    // Prepend header
    ///     in.Read(buf);      // Read data into rest of buffer
    ///     buf.Flip();        // Flip buffer
    ///     out.Write(buf);    // Write header + data to channel
    /// </code>
    ///     </para>
    ///     <para>
    ///         This method is often used in conjunction with the <see cref="ByteBuffer.Compact()" />
    ///         method when transferring data from one place to another.
    ///     </para>
    /// </summary>
    /// <returns>  This buffer </returns>
    public Buffer Flip()
    {
        Limit    = Position;
        Position = 0;
        _mark    = -1;

        return this;
    }

    /// <summary>
    ///     Rewinds this buffer. The position is set to zero and the mark is discarded.
    ///     <para>
    ///         Invoke this method before a sequence of channel-write or <i>get</i>
    ///         operations, assuming that the limit has already been set appropriately.
    ///     </para>
    ///     <para>
    ///         For example:
    ///         <code>
    /// out.write(buf);    // Write remaining data
    /// buf.rewind();      // Rewind buffer
    /// buf.get(array);    // Copy data into array
    /// </code>
    ///     </para>
    /// </summary>
    /// <returns>  This buffer </returns>
    public Buffer Rewind()
    {
        Position = 0;
        _mark    = -1;

        return this;
    }

    /// <summary>
    ///     Returns the number of elements between the current position and the limit.
    /// </summary>
    public int Remaining() => Limit - Position;

    /// <summary>
    ///     Returns <tt>true</tt> if, and only if, there is at least one element
    ///     remaining in this buffer
    /// </summary>
    public bool HasRemaining() => Position < Limit;

    #region abstract methods

    /// <summary>
    ///     Tells whether or not this buffer is backed by an accessible array.
    ///     <para>
    ///         If this method returns <tt>true</tt> then the <see cref="Array" />
    ///         and <see cref="ArrayOffset" /> methods may safely be invoked.
    ///     </para>
    /// </summary>
    /// <returns>
    ///     <tt>true</tt> if, and only if, this buffer is backed by an array and is not read-only
    /// </returns>
    public abstract bool HasArray();

    /// <summary>
    ///     Returns the array that backs this buffer <i>(optional operation)</i>.
    ///     <para>
    ///         This method is intended to allow array-backed buffers to be passed to
    ///         native code more efficiently. Concrete subclasses provide more strongly
    ///         typed return values for this method.
    ///     </para>
    ///     <para>
    ///         Modifications to this buffer's content will cause the returned array's
    ///         content to be modified, and vice versa.
    ///     </para>
    ///     <para>
    ///         Invoke the <see cref="HasArray()" /> method before invoking this method in
    ///         order to ensure that this buffer has an accessible backing array.
    ///     </para>
    /// </summary>
    /// <returns>  The array that backs this buffer </returns>
    public abstract object BackingArray();

    /// <summary>
    ///     Returns the offset within this buffer's backing array of the first
    ///     element of the buffer <i>(optional operation)</i>.
    ///     <para>
    ///         If this buffer is backed by an array then buffer position
    ///         <b>
    ///             <i>p</i>
    ///         </b>
    ///         corresponds to array index <b><i>p</i> + <tt>arrayOffset()</tt></b>.
    ///     </para>
    ///     <para>
    ///         Invoke the
    ///         <b>
    ///             <see cref="HasArray()" />
    ///         </b>
    ///         method before invoking this method in
    ///         order to ensure that this buffer has an accessible backing array.
    ///     </para>
    /// </summary>
    /// <returns>
    ///     The offset within this buffer's array of the first element of the buffer
    /// </returns>
    public abstract int ArrayOffset();

    /// <summary>
    ///     Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public abstract bool IsDirect();

    #endregion abstract methods

    // ------------------------------------------------------------------------
    // -- Package-private methods for bounds checking, etc. --
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Checks the current position against the limit, throwing a {@link
    ///     BufferUnderflowException} if it is not smaller than the limit, and then
    ///     increments the position.
    /// </summary>
    /// <returns> The current position value, before it is incremented </returns>
    internal int NextGetIndex()
    {
        if ( Position >= Limit )
        {
            throw new GdxRuntimeException( "Buffer#NextGetIndex: Buffer Overflow!" );
        }

        return Position++;
    }

    internal int NextGetIndex( int nb )
    {
        if ( ( Limit - Position ) < nb )
        {
            throw new GdxRuntimeException( "Buffer#NextGetIndex: Buffer Underflow!" );
        }

        var p = Position;

        Position += nb;

        return p;
    }

    /// <summary>
    ///     Checks the current position against the limit, throwing a {@link
    ///     BufferOverflowException} if it is not smaller than the limit, and then
    ///     increments the position.
    /// </summary>
    /// <returns> The current position value, before it is incremented </returns>
    internal int NextPutIndex()
    {
        if ( Position >= Limit )
        {
            throw new GdxRuntimeException( "Buffer#NextGetIndex: Buffer Overflow!" );
        }

        return Position++;
    }

    internal int NextPutIndex( int nb )
    {
        if ( ( Limit - Position ) < nb )
        {
            throw new GdxRuntimeException( "Buffer#NextGetIndex: Buffer Overflow!" );
        }

        var p = Position;

        Position += nb;

        return p;
    }

    /// <summary>
    ///     Checks the given index against the limit, throwing an {@link
    ///     IndexOutOfRangeException} if it is not smaller than the limit
    ///     or is smaller than zero.
    /// </summary>
    internal int CheckIndex( int i )
    {
        if ( ( i < 0 ) || ( i >= Limit ) )
        {
            throw new IndexOutOfRangeException();
        }

        return i;
    }

    internal int CheckIndex( int i, int nb )
    {
        if ( ( i < 0 ) || ( nb > ( Limit - i ) ) )
        {
            throw new IndexOutOfRangeException();
        }

        return i;
    }

    internal void Truncate()
    {
        _mark    = -1;
        Position = 0;
        Limit    = 0;
        Capacity = 0;
    }

    internal int MarkValue() => _mark;

    internal void DiscardMark() => _mark = -1;

    internal static void CheckBounds( int off, int len, int size )
    {
        if ( ( off | len | ( off + len ) | ( size - ( off + len ) ) ) < 0 )
        {
            throw new IndexOutOfRangeException();
        }
    }
}
