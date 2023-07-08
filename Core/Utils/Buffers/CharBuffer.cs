// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Utils.Buffers;

public abstract class CharBuffer : Buffer
{
    private readonly char[]? _hb;           // Non-null only for heap buffers
    private readonly int     _offset;
    private          bool    _isReadOnly;   // Valid only for heap buffers

    public CharBuffer( int mark, int pos, int lim, int cap, char[] hb, int offset )
        : base( mark, pos, lim, cap )
    {
        this._hb     = hb;
        this._offset = offset;
    }

    /// <summary>
    /// Creates a new buffer with the given mark, position, limit, and capacity
    /// </summary>
    /// <param name="mark"></param>
    /// <param name="pos"></param>
    /// <param name="lim"></param>
    /// <param name="cap"></param>
    public CharBuffer( int mark, int pos, int lim, int cap )
        : this( mark, pos, lim, cap, null, 0 )
    {
    }

    /// <summary>
    /// Allocates a new char buffer.
    /// <para>
    /// The new buffer's position will be zero, its limit will be its
    /// capacity, its mark will be undefined, and each of its elements will be
    /// initialized to zero.  It will have a {@link #array backing array},
    /// and its {@link #arrayOffset array offset} will be zero.
    /// </para>
    /// <param name="capacity"> The new buffer's capacity, in chars </param>
    /// <returns> The new char buffer </returns>
    /// <exception cref="ArgumentException">
    /// If the <tt>capacity</tt> is a negative integer
    /// </exception>
    /// </summary>
    public static CharBuffer Allocate( int capacity )
    {
        if ( capacity < 0 ) throw new ArgumentException();

        return new HeapCharBuffer( capacity, capacity );
    }

    /// <summary>
    /// Wraps a char array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given char array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa.  The new buffer's
    /// capacity will be <tt>array.length</tt>, its position will be <tt>offset</tt>, its
    /// limit will be <tt>offset + length</tt>, and its mark will be undefined. Its
    /// backing array will be the given array, and array offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back the new buffer </param>
    /// <param name="offset">
    /// The offset of the subarray to be used; must be non-negative and no larger than
    /// <tt>array.length</tt>. The new buffer's position will be set to this value.
    /// </param>
    /// <param name="length">
    /// The length of the subarray to be used; must be non-negative and no larger
    /// than <tt>array.length - offset</tt>. The new buffer's limit will be set
    /// to <tt>offset + length</tt>.
    /// </param>
    /// <returns> The new char buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    public static CharBuffer Wrap( char[] array, int offset, int length )
    {
        try
        {
            return new HeapCharBuffer( array, offset, length );
        }
        catch ( ArgumentException x )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a char array into a buffer.
    /// <para>
    /// The new buffer will be backed by the given char array; that is, modifications
    /// to the buffer will cause the array to be modified and vice versa.  The new
    /// buffer's capacity and limit will be <tt>array.length</tt>, its position will be
    /// zero, and its mark will be undefined. Its Backing Array will be the given array,
    /// and its array offset will be zero.
    /// </para>
    /// </summary>
    /// <param name="array"> The array that will back this buffer </param>
    /// <returns> The new char buffer </returns>
    public static CharBuffer Wrap( char[] array )
    {
        return Wrap( array, 0, array.Length );
    }

    /// <summary>
    /// Attempts to read characters into the specified character buffer.
    /// The buffer is used as a repository of characters as-is: the only
    /// changes made are the results of a put operation. No flipping or
    /// rewinding of the buffer is performed.
    /// </summary>
    /// <param name="target"> the buffer to read characters into </param>
    /// <returns>
    /// The number of characters added to the buffer, or -1 if this source
    /// of characters is at its end
    /// </returns>
    /// <exception cref="IOException">if an I/O error occurs</exception>
    /// <exception cref="NullReferenceException">if target is null</exception>
    /// <exception cref="ReadOnlyBufferException">if target is a read only buffer</exception>
    public int Read( CharBuffer target )
    {
        // Determine the number of bytes n that can be transferred
        int targetRemaining = target.Remaining();
        int remaining       = Remaining();

        if ( remaining == 0 ) return -1;

        int n     = Math.Min( remaining, targetRemaining );
        int limit = Limit;

        // Set source limit to prevent target overflow
        if ( targetRemaining < remaining ) Limit = Position + n;

        try
        {
            if ( n > 0 )
            {
                target.Put( this );
            }
        }
        finally
        {
            Limit = limit; // restore real limit
        }

        return n;
    }

    /// <summary>
    /// Wraps a character sequence into a buffer.
    /// <para>
    /// The content of the new, read-only buffer will be the content of the given character
    /// sequence. The buffer's capacity will be <tt>csq.length()</tt>, its position will be
    /// <tt>start</tt>, its limit will be <tt>end</tt>, and its mark will be undefined.
    /// </para>
    /// </summary>
    /// <param name="csq">
    /// The character sequence from which the new character buffer is to be created
    /// </param>
    /// <param name="start">
    /// The index of the first character to be used; must be non-negative and no larger
    /// than <tt>csq.length()</tt>. The new buffer's position will be set to this value.
    /// </param>
    /// <param name="end">
    /// The index of the character following the last character to be used; must be no
    /// smaller than <tt>start</tt> and no larger than <tt>csq.length()</tt>. The new
    /// buffer's limit will be set to this value.
    /// </param>
    /// <returns> The new character buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>start</tt> and <tt>end</tt> parameters do not hold
    /// </exception>
    public static CharBuffer Wrap( string csq, int start, int end )
    {
        try
        {
            return new StringCharBuffer( csq, start, end );
        }
        catch ( ArgumentException x )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Wraps a character sequence into a buffer.
    /// <p>
    /// The content of the new, read-only buffer will be the content of the
    /// given character sequence.  The new buffer's capacity and limit will be
    /// <tt>csq.length()</tt>, its position will be zero, and its mark will be
    /// undefined.
    /// </p>
    /// </summary>
    /// <param name="csq">
    /// The character sequence from which the new character buffer is to be created
    /// </param>
    /// <returns> The new character buffer </returns>
    public static CharBuffer Wrap( string csq )
    {
        return Wrap( csq, 0, csq.Length );
    }

    /// <summary>
    /// Creates a new char buffer whose content is a shared subsequence of
    /// this buffer's content.
    /// <para>
    /// The content of the new buffer will start at this buffer's current
    /// position.  Changes to this buffer's content will be visible in the new
    /// buffer, and vice versa; the two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's position will be zero, its capacity and its limit
    /// will be the number of chars remaining in this buffer, and its mark
    /// will be undefined.  The new buffer will be direct if, and only if, this
    /// buffer is direct, and it will be read-only if, and only if, this buffer
    /// is read-only.
    /// </para>
    /// </summary>
    /// <returns>  The new char buffer </returns>
    public abstract CharBuffer Slice();

    /// <summary>
    /// Creates a new char buffer that shares this buffer's content.
    /// <para>
    /// The content of the new buffer will be that of this buffer.  Changes
    /// to this buffer's content will be visible in the new buffer, and vice
    /// versa; the two buffers' position, limit, and mark values will be
    /// independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer.  The new buffer will be direct if,
    /// and only if, this buffer is direct, and it will be read-only if, and
    /// only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <returns>  The new char buffer </returns>
    public abstract CharBuffer Duplicate();

    /// <summary>
    /// Creates a new, read-only char buffer that shares this buffer's
    /// content.
    /// <para>
    /// The content of the new buffer will be that of this buffer.  Changes
    /// to this buffer's content will be visible in the new buffer; the new
    /// buffer itself, however, will be read-only and will not allow the shared
    /// content to be modified.  The two buffers' position, limit, and mark
    /// values will be independent.
    /// </para>
    /// <para>
    /// The new buffer's capacity, limit, position, and mark values will be
    /// identical to those of this buffer.
    /// </para>
    /// <para>
    /// If this buffer is itself read-only then this method behaves in
    /// exactly the same way as the <seealso cref="duplicate duplicate"/> method.
    /// </para>
    /// </summary>
    /// <returns>  The new, read-only char buffer </returns>
    public abstract CharBuffer AsReadOnlyBuffer();

    #region Singleton get/put methods

    /// <summary>
    /// Relative <i>get</i> method.  Reads the char at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns>The char at the buffer's current position</returns>
    /// <exception cref="BufferUnderflowException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    public abstract char Get();

    /// <summary>
    /// Relative <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given char into this buffer at the current position, and then
    /// increments the position.
    /// </para>
    /// </summary>
    /// <param name="c">The char to be written</param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If this buffer's current position is not smaller than its limit
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">
    /// If this buffer is read-only, which it shouldn't be!
    /// </exception>
    public abstract CharBuffer Put( char c );

    /// <summary>
    /// Absolute <i>get</i> method.  Reads the char at the given index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns> The char at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    public abstract char Get( int index );

    /// <summary>
    /// Absolute <i>get</i> method. Reads the char at the given index without
    /// any validation of the index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns>  The char at the given index </returns>
    public abstract char GetUnchecked( int index ); // package-private

    /// <summary>
    /// Absolute <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// Writes the given char into this buffer at the given index.
    /// </para>
    /// </summary>
    /// <param name="index">
    /// The index at which the char will be written
    /// </param>
    /// <param name="c"> The char value to be written </param>
    /// <returns> This buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    /// <exception cref="ReadOnlyBufferException">If this buffer is read-only </exception>
    public abstract CharBuffer Put( int index, char c );

    #endregion Singleton get/put methods

    #region Bulk get operations

    /// <summary>
    /// Relative bulk <i>get</i> method.
    /// <para>
    /// This method transfers chars from this buffer into the given destination array.
    /// If there are fewer chars remaining in thebuffer than are required to satisfy
    /// the request, that is, if <tt>length</tt> <tt>&gt;</tt> <tt>Remaining()</tt>,
    /// then no chars are transferred and a <see cref="BufferUnderflowException"/> is
    /// thrown. Otherwise, this method copies <tt>length</tt> chars from this buffer
    /// into the given array, starting at the current position of this buffer and at
    /// the given offset in the array.  The position of this buffer is then incremented
    /// by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>src.get(dst, off, len)</tt>
    /// has exactly the same effect as the loop
    /// 
    /// <code>
    ///     for (int i = off; i &lt; off + len; i++)
    ///     {
    ///         dst[i] = src.get():
    ///     }
    /// </code>
    /// except that it first checks that there are sufficient chars in this buffer and it
    /// is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="dst">The array into which chars are to be written</param>
    /// <param name="offset">
    /// The offset within the array of the first char to be written; must be non-negative
    /// and no larger than <tt>dst.length</tt>
    /// </param>
    /// <param name="length">
    /// The maximum number of chars to be written to the given array; must be non-negative
    /// and no larger than <tt>dst.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferUnderflowException">
    /// If there are fewer than <tt>length</tt> chars remaining in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    public CharBuffer Get( char[] dst, int offset, int length )
    {
        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() ) throw new BufferUnderflowException();

        int end = offset + length;

        for ( int i = offset; i < end; i++ )
        {
            dst[ i ] = Get();
        }

        return this;
    }

    /**
 * Relative bulk <i>get</i> method.
 *
 * <p> This method transfers chars from this buffer into the given
 * destination array.  An invocation of this method of the form
 * <tt>src.get(a)</tt> behaves in exactly the same way as the invocation
 *
 * <pre>
 *     src.get(a, 0, a.length) </pre>
 *
 * @param   dst
 *          The destination array
 *
 * @return  This buffer
 *
 * @throws  BufferUnderflowException
 *          If there are fewer than <tt>length</tt> chars
 *          remaining in this buffer
 */
    public CharBuffer Get( char[] dst )
    {
        return Get( dst, 0, dst.Length );
    }

    #endregion Bulk get operations

    #region Bulk put operations

    /**
 * Relative bulk <i>put</i> method  <i>(optional operation)</i>.
 *
 * <p> This method transfers the chars remaining in the given source
 * buffer into this buffer.  If there are more chars remaining in the
 * source buffer than in this buffer, that is, if
 * <tt>src.Remaining()</tt> <tt>&gt;</tt> <tt>Remaining()</tt>,
 * then no chars are transferred and a {@link
 * BufferOverflowException} is thrown.
 *
 * <p> Otherwise, this method copies
 * <i>n</i> = <tt>src.Remaining()</tt> chars from the given
 * buffer into this buffer, starting at each buffer's current position.
 * The positions of both buffers are then incremented by <i>n</i>.
 *
 * <p> In other words, an invocation of this method of the form
 * <tt>dst.put(src)</tt> has exactly the same effect as the loop
 *
 * <pre>
 *     while (src.hasRemaining())
 *         dst.put(src.get()); </pre>
 *
 * except that it first checks that there is sufficient space in this
 * buffer and it is potentially much more efficient.
 *
 * @param  src
 *         The source buffer from which chars are to be read;
 *         must not be this buffer
 *
 * @return  This buffer
 *
 * @throws  BufferOverflowException
 *          If there is insufficient space in this buffer
 *          for the remaining chars in the source buffer
 *
 * @throws  IllegalArgumentException
 *          If the source buffer is this buffer
 *
 * @throws  ReadOnlyBufferException
 *          If this buffer is read-only
 */
    public CharBuffer Put( CharBuffer src )
    {
        if ( src == this ) throw new ArgumentException();

        if ( IsReadOnly() ) throw new ReadOnlyBufferException();

        int n = src.Remaining();

        if ( n > Remaining() ) throw new BufferOverflowException();

        for ( int i = 0; i < n; i++ )
        {
            Put( src.Get() );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers chars into this buffer from the given source array.
    /// If there are more chars to be copied from the array than remain in this
    /// buffer, that is, if <tt>length</tt> <tt>&gt;</tt> <tt>Remaining()</tt>,
    /// then no chars are transferred and a <seealso cref="BufferOverflowException"/>
    /// is thrown.
    /// </para>
    /// <para>
    /// Otherwise, this method copies <tt>length</tt> chars from the given array into
    /// this buffer, starting at the given offset in the array and at the current position
    /// of this buffer.  The position of this buffer is then incremented by <tt>length</tt>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form
    /// <tt>dst.put(src, off, len)</tt> has exactly the same effect as
    /// the loop
    /// 
    /// <code>
    ///     for (int i = off; i &lt; off + len; i++)
    ///     {
    ///         dst.put(a[i]);
    ///     }
    /// </code>
    /// 
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="src"> The array from which chars are to be read </param>
    /// <param name="offset">
    /// The offset within the array of the first char to be read; must be non-negative
    /// and no larger than <tt>array.length</tt>
    /// </param>
    /// <param name="length">
    /// The number of chars to be read from the given array; must be non-negative
    /// and no larger than <tt>array.length - offset</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException"> If there is insufficient space in this buffer </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>offset</tt> and <tt>length</tt> parameters do not hold
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public CharBuffer Put( char[] src, int offset, int length )
    {
        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() ) throw new BufferOverflowException();

        int end = offset + length;

        for ( int i = offset; i < end; i++ )
        {
            this.Put( src[ i ] );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers the entire content of the given source char array
    /// into this buffer. An invocation of this method of the form <tt>dst.put(a)</tt>
    /// behaves in exactly the same way as the invocation
    ///
    /// <code>
    ///     dst.put(a, 0, a.length)
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="src"> The source array </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException"> If there is insufficient space in this buffer </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public CharBuffer Put( char[] src )
    {
        return Put( src, 0, src.Length );
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers chars from the given string into this buffer. If there are
    /// more chars to be copied from the string than remain in this buffer, that is, if
    /// <tt>end - start</tt> <tt>&gt;</tt> <tt>Remaining()</tt>, then no chars are transferred
    /// and a <see cref="BufferOverflowException"/> is thrown. Otherwise, this method copies
    /// <i>n</i> = <tt>end</tt> - <tt>start</tt> chars from the given string into this buffer,
    /// starting at the given <tt>start</tt> index and at the current position of this buffer.
    /// The position of this buffer is then incremented by <i>n</i>.
    /// </para>
    /// <para>
    /// In other words, an invocation of this method of the form <tt>dst.put(src, start, end)</tt>
    /// has exactly the same effect as the loop
    /// 
    /// <code>
    ///     for (int i = start; i &lt; end; i++)
    ///     {
    ///         dst.put(src.charAt(i));
    ///     }
    /// </code>
    /// except that it first checks that there is sufficient space in this
    /// buffer and it is potentially much more efficient.
    /// </para>
    /// </summary>
    /// <param name="src">The string from which chars are to be read</param>
    /// <param name="start">
    /// The offset within the string of the first char to be read; must be non-negative
    /// and no larger than <tt>string.length()</tt>
    /// </param>
    /// <param name="end">
    /// The offset within the string of the last char to be read, plus one; must be
    /// non-negative and no larger than <tt>string.length()</tt>
    /// </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there is insufficient space in this buffer
    /// </exception>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on the <tt>start</tt> and <tt>end</tt> parameters do not hold
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public CharBuffer Put( string src, int start, int end )
    {
        CheckBounds( start, end - start, src.Length );

        if ( IsReadOnly() ) throw new ReadOnlyBufferException();

        if ( ( end - start ) > Remaining() ) throw new BufferOverflowException();

        for ( int i = start; i < end; i++ )
        {
            this.Put( src[ i ] );
        }

        return this;
    }

    /// <summary>
    /// Relative bulk <i>put</i> method  <i>(optional operation)</i>.
    /// <para>
    /// This method transfers the entire content of the given source string into this
    /// buffer. An invocation of this method of the form
    ///
    /// <code>
    ///     <tt>dst.put(s)</tt>
    /// </code>
    /// behaves in exactly the same way as the invocation
    /// <code>
    ///     dst.put(s, 0, s.length())
    /// </code>
    /// </para>
    /// </summary>
    /// <param name="src"> The source string </param>
    /// <returns> This buffer </returns>
    /// <exception cref="BufferOverflowException">
    /// If there is insufficient space in this buffer
    /// </exception>
    /// <exception cref="ReadOnlyBufferException"> If this buffer is read-only </exception>
    public CharBuffer Put( string src )
    {
        return Put( src, 0, src.Length );
    }

    #endregion Bulk put operations

    /// <summary>
    /// Tells whether or not this buffer is backed by an accessible char
    /// array.
    /// 
    /// <para> If this method returns <tt>true</tt> then the <seealso cref="array() array"/>
    /// and <seealso cref="arrayOffset() arrayOffset"/> methods may safely be invoked.
    /// </para>
    /// </summary>
    /// <returns>  <tt>true</tt> if, and only if, this buffer
    ///          is backed by an array and is not read-only </returns>
    public override bool HasArray()
    {
        return ( _hb != null ) && !_isReadOnly;
    }

    /// <summary>
    /// Returns the char array that backs this
    /// buffer  <i>(optional operation)</i>.
    /// 
    /// <para> Modifications to this buffer's content will cause the returned
    /// array's content to be modified, and vice versa.
    /// 
    /// </para>
    /// <para> Invoke the <seealso cref="hasArray hasArray"/> method before invoking this
    /// method in order to ensure that this buffer has an accessible backing
    /// array.  </para>
    /// </summary>
    /// <returns>  The array that backs this buffer
    /// </returns>
    /// <exception cref="ReadOnlyBufferException">
    ///          If this buffer is backed by an array but is read-only
    /// </exception>
    /// <exception cref="UnsupportedOperationException">
    ///          If this buffer is not backed by an accessible array </exception>
    public override char[] BackingArray()
    {
        if ( _hb == null ) throw new UnsupportedOperationException();

        if ( _isReadOnly ) throw new ReadOnlyBufferException();

        return _hb;
    }

    /**
     * Returns the offset within this buffer's backing array of the first
     * element of the buffer  <i>(optional operation)</i>.
     *
     * <p> If this buffer is backed by an array then buffer position <i>p</i>
     * corresponds to array index <i>p</i> + <tt>arrayOffset()</tt>.
     *
     * <p> Invoke the {@link #hasArray hasArray} method before invoking this
     * method in order to ensure that this buffer has an accessible backing
     * array.  </p>
     *
     * @return  The offset within this buffer's array
     *          of the first element of the buffer
     *
     * @throws  ReadOnlyBufferException
     *          If this buffer is backed by an array but is read-only
     *
     * @throws  UnsupportedOperationException
     *          If this buffer is not backed by an accessible array
     */
    public override int ArrayOffset()
    {
        if ( _hb == null ) throw new UnsupportedOperationException();

        if ( _isReadOnly ) throw new ReadOnlyBufferException();

        return _offset;
    }

    /**
     * Compacts this buffer  <i>(optional operation)</i>.
     *
     * <p> The chars between the buffer's current position and its limit,
     * if any, are copied to the beginning of the buffer.  That is, the
     * char at index <i>p</i> = <tt>position()</tt> is copied
     * to index zero, the char at index <i>p</i> + 1 is copied
     * to index one, and so forth until the char at index
     * <tt>limit()</tt> - 1 is copied to index
     * <i>n</i> = <tt>limit()</tt> - <tt>1</tt> - <i>p</i>.
     * The buffer's position is then set to <i>n+1</i> and its limit is set to
     * its capacity.  The mark, if defined, is discarded.
     *
     * <p> The buffer's position is set to the number of chars copied,
     * rather than to zero, so that an invocation of this method can be
     * followed immediately by an invocation of another relative <i>put</i>
     * method. </p>
     *
     * @return  This buffer
     *
     * @throws  ReadOnlyBufferException
     *          If this buffer is read-only
     */
    public abstract CharBuffer Compact();

    /**
     * Returns the current hash code of this buffer.
     *
     * <p> The hash code of a char buffer depends only upon its remaining
     * elements; that is, upon the elements from <tt>position()</tt> up to, and
     * including, the element at <tt>limit()</tt> - <tt>1</tt>.
     *
     * <p> Because buffer hash codes are content-dependent, it is inadvisable
     * to use buffers as keys in hash maps or similar data structures unless it
     * is known that their contents will not change.  </p>
     *
     * @return  The current hash code of this buffer
     */
    public int HashCode()
    {
        int h = 1;
        int p = Position;

        for ( int i = Limit - 1; i >= p; i-- )
        {
            h = ( 31 * h ) + Get( i );
        }

        return h;
    }

    /**
     * Tells whether or not this buffer is equal to another object.
     *
     * <p> Two char buffers are equal if, and only if,
     *
     * <ol>
     *
     *   <li><p> They have the same element type,  </p></li>
     *
     *   <li><p> They have the same number of remaining elements, and
     *   </p></li>
     *
     *   <li><p> The two sequences of remaining elements, considered
     *   independently of their starting positions, are pointwise equal.
     *   </p></li>
     *
     * </ol>
     *
     * <p> A char buffer is not equal to any other type of object.  </p>
     *
     * @param  ob  The object to which this buffer is to be compared
     *
     * @return  <tt>true</tt> if, and only if, this buffer is equal to the
     *           given object
     */
    public new bool Equals( object ob )
    {
        if ( this == ob ) return true;

        if ( !( ob is CharBuffer that ) ) return false;

        if ( this.Remaining() != that.Remaining() ) return false;

        int p = this.Position;

        for ( int i = this.Limit - 1, j = that.Limit - 1; i >= p; i--, j-- )
        {
            if ( !equals( this.Get( i ), that.Get( j ) ) )
            {
                return false;
            }
        }

        return true;
    }

    private static bool Equals( char x, char y )
    {
        return x == y;
    }

    /**
     * Compares this buffer to another.
     *
     * <p> Two char buffers are compared by comparing their sequences of
     * remaining elements lexicographically, without regard to the starting
     * position of each sequence within its corresponding buffer.
     * Pairs of {@code char} elements are compared as if by invoking
     * {@link Character#compare(char,char)}.

     *
     * <p> A char buffer is not comparable to any other type of object.
     *
     * @return  A negative integer, zero, or a positive integer as this buffer
     *          is less than, equal to, or greater than the given buffer
     */
    public int CompareTo( CharBuffer that )
    {
        int n = this.Position + Math.Min( this.Remaining(), that.Remaining() );

        for ( int i = this.Position, j = that.Position; i < n; i++, j++ )
        {
            int cmp = Compare( this.Get( i ), that.Get( j ) );

            if ( cmp != 0 ) return cmp;
        }

        return this.Remaining() - that.Remaining();
    }

    private static int Compare( char x, char y )
    {
        return CharHelper.Compare( x, y );
    }

    /**
     * Returns a string containing the characters in this buffer.
     *
     * <p> The first character of the resulting string will be the character at
     * this buffer's position, while the last character will be the character
     * at index <tt>limit()</tt> - 1.  Invoking this method does not
     * change the buffer's position. </p>
     *
     * @return  The specified string
     */
    public string toString()
    {
        return toString( position(), limit() );
    }

    abstract string toString( int start, int end ); // package-private


// --- Methods to support CharSequence ---

    /**
 * Returns the length of this character buffer.
 *
 * <p> When viewed as a character sequence, the length of a character
 * buffer is simply the number of characters between the position
 * (inclusive) and the limit (exclusive); that is, it is equivalent to
 * <tt>Remaining()</tt>. </p>
 *
 * @return  The length of this character buffer
 */
    public int length()
    {
        return Remaining();
    }

    /**
 * Reads the character at the given index relative to the current
 * position.
 *
 * @param  index
 *         The index of the character to be read, relative to the position;
 *         must be non-negative and smaller than <tt>Remaining()</tt>
 *
 * @return  The character at index
 *          <tt>position() + index</tt>
 *
 * @throws  IndexOutOfRangeException
 *          If the preconditions on <tt>index</tt> do not hold
 */
    public char charAt( int index )
    {
        return get( position() + checkIndex( index, 1 ) );
    }

    /**
 * Creates a new character buffer that represents the specified subsequence
 * of this buffer, relative to the current position.
 *
 * <p> The new buffer will share this buffer's content; that is, if the
 * content of this buffer is mutable then modifications to one buffer will
 * cause the other to be modified.  The new buffer's capacity will be that
 * of this buffer, its position will be
 * <tt>position()</tt> + <tt>start</tt>, and its limit will be
 * <tt>position()</tt> + <tt>end</tt>.  The new buffer will be
 * direct if, and only if, this buffer is direct, and it will be read-only
 * if, and only if, this buffer is read-only.  </p>
 *
 * @param  start
 *         The index, relative to the current position, of the first
 *         character in the subsequence; must be non-negative and no larger
 *         than <tt>Remaining()</tt>
 *
 * @param  end
 *         The index, relative to the current position, of the character
 *         following the last character in the subsequence; must be no
 *         smaller than <tt>start</tt> and no larger than
 *         <tt>Remaining()</tt>
 *
 * @return  The new character buffer
 *
 * @throws  IndexOutOfRangeException
 *          If the preconditions on <tt>start</tt> and <tt>end</tt>
 *          do not hold
 */
    public abstract CharBuffer subSequence( int start, int end );


// --- Methods to support Appendable ---

    /**
 * Appends the specified character sequence  to this
 * buffer  <i>(optional operation)</i>.
 *
 * <p> An invocation of this method of the form <tt>dst.append(csq)</tt>
 * behaves in exactly the same way as the invocation
 *
 * <pre>
 *     dst.put(csq.toString()) </pre>
 *
 * <p> Depending on the specification of <tt>toString</tt> for the
 * character sequence <tt>csq</tt>, the entire sequence may not be
 * appended.  For instance, invoking the {@link CharBuffer#toString()
 * toString} method of a character buffer will return a subsequence whose
 * content depends upon the buffer's position and limit.
 *
 * @param  csq
 *         The character sequence to append.  If <tt>csq</tt> is
 *         <tt>null</tt>, then the four characters <tt>"null"</tt> are
 *         appended to this character buffer.
 *
 * @return  This buffer
 *
 * @throws  BufferOverflowException
 *          If there is insufficient space in this buffer
 *
 * @throws  ReadOnlyBufferException
 *          If this buffer is read-only
 *
 * @since  1.5
 */
    public CharBuffer append( CharSequence csq )
    {
        if ( csq == null )
            return put( "null" );
        else
            return put( csq.toString() );
    }

    /**
 * Appends a subsequence of the  specified character sequence  to this
 * buffer  <i>(optional operation)</i>.
 *
 * <p> An invocation of this method of the form <tt>dst.append(csq, start,
 * end)</tt> when <tt>csq</tt> is not <tt>null</tt>, behaves in exactly the
 * same way as the invocation
 *
 * <pre>
 *     dst.put(csq.subSequence(start, end).toString()) </pre>
 *
 * @param  csq
 *         The character sequence from which a subsequence will be
 *         appended.  If <tt>csq</tt> is <tt>null</tt>, then characters
 *         will be appended as if <tt>csq</tt> contained the four
 *         characters <tt>"null"</tt>.
 *
 * @return  This buffer
 *
 * @throws  BufferOverflowException
 *          If there is insufficient space in this buffer
 *
 * @throws  IndexOutOfRangeException
 *          If <tt>start</tt> or <tt>end</tt> are negative, <tt>start</tt>
 *          is greater than <tt>end</tt>, or <tt>end</tt> is greater than
 *          <tt>csq.length()</tt>
 *
 * @throws  ReadOnlyBufferException
 *          If this buffer is read-only
 *
 * @since  1.5
 */
    public CharBuffer append( CharSequence csq, int start, int end )
    {
        CharSequence cs = ( csq == null ? "null" : csq );

        return put( cs.subSequence( start, end ).toString() );
    }

    /**
 * Appends the specified char  to this
 * buffer  <i>(optional operation)</i>.
 *
 * <p> An invocation of this method of the form <tt>dst.append(c)</tt>
 * behaves in exactly the same way as the invocation
 *
 * <pre>
 *     dst.put(c) </pre>
 *
 * @param  c
 *         The 16-bit char to append
 *
 * @return  This buffer
 *
 * @throws  BufferOverflowException
 *          If there is insufficient space in this buffer
 *
 * @throws  ReadOnlyBufferException
 *          If this buffer is read-only
 *
 * @since  1.5
 */
    public CharBuffer append( char c )
    {
        return Put( c );
    }


// -- Other byte stuff: Access to binary data --


    /**
 * Retrieves this buffer's byte order.
 *
 * <p> The byte order of a char buffer created by allocation or by
 * wrapping an existing <tt>char</tt> array is the {@link
 * ByteOrder#nativeOrder native order} of the underlying
 * hardware.  The byte order of a char buffer created as a <a
 * href="ByteBuffer.html#views">view</a> of a byte buffer is that of the
 * byte buffer at the moment that the view is created.  </p>
 *
 * @return  This buffer's byte order
 */
    public abstract ByteOrder order();


    @Override

    public IntStream chars()
    {
        return StreamSupport.intStream( ( ) -> new CharBufferSpliterator( this ),
        Buffer.SPLITERATOR_CHARACTERISTICS, false);
    }
}