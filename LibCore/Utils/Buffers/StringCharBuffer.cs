// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Utils.Buffers;

[PublicAPI]
public class StringCharBuffer : CharBuffer
{
    private readonly string _string;

    // ------------------------------------------------------------------------
    
    public StringCharBuffer( string csq, int start, int end )
        : base( -1, start, end, csq.Length )
    {
        var n = csq.Length;

        if ( ( start < 0 ) || ( start > n ) || ( end < start ) || ( end > n ) )
        {
            throw new IndexOutOfRangeException();
        }

        _string = csq;
    }

    private StringCharBuffer( string s, int mark, int pos, int limit, int cap, int offset )
        : base( mark, pos, limit, cap, null, offset )
    {
        _string = s;
    }

    /// <summary>
    /// Returns <tt>true</tt> if, and only if, this buffer is read-only.
    /// This overrides the base property to make it only return true.
    /// </summary>
    public override bool IsReadOnly => true;

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
    public override CharBuffer Slice()
    {
        return new StringCharBuffer( _string,
                                     -1,
                                     0,
                                     Remaining(),
                                     Remaining(),
                                     Offset + Position );
    }

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
    public override CharBuffer Duplicate()
    {
        return new StringCharBuffer( _string, Mark, Position, Limit, Capacity, Offset );
    }

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
    /// exactly the same way as the <see cref="Duplicate"/> method.
    /// </para>
    /// </summary>
    /// <returns>  The new, read-only char buffer </returns>
    public override CharBuffer AsReadOnlyBuffer()
    {
        return Duplicate();
    }

    /// <summary>
    /// Relative <i>get</i> method.  Reads the char at this buffer's
    /// current position, and then increments the position.
    /// </summary>
    /// <returns>The char at the buffer's current position</returns>
    /// <exception cref="GdxRuntimeException">
    /// If the buffer's current position is not smaller than its limit
    /// </exception>
    protected override char Get()
    {
        return _string[ NextGetIndex() + Offset ];
    }

    /// <summary>
    /// Absolute <i>get</i> method.  Reads the char at the given index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns> The char at the given index </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If <tt>index</tt> is negative or not smaller than the buffer's limit
    /// </exception>
    protected override char Get( int index )
    {
        return _string[ CheckIndex( index ) + Offset ];
    }

    /// <summary>
    /// Absolute <i>get</i> method. Reads the char at the given index without
    /// any validation of the index.
    /// </summary>
    /// <param name="index">The index from which the char will be read</param>
    /// <returns> The char at the given index </returns>
    public override char GetUnchecked( int index )
    {
        return _string[ index + Offset ];
    }

    /// <summary>
    /// Creates a new character buffer that represents the specified subsequence
    /// of this buffer, relative to the current position.
    /// <para>
    /// The new buffer will share this buffer's content; that is, if the
    /// content of this buffer is mutable then modifications to one buffer will
    /// cause the other to be modified.  The new buffer's capacity will be that
    /// of this buffer, its position will be
    /// <tt>position()</tt> + <tt>start</tt>, and its limit will be
    /// <tt>position()</tt> + <tt>end</tt>.  The new buffer will be
    /// direct if, and only if, this buffer is direct, and it will be read-only
    /// if, and only if, this buffer is read-only.
    /// </para>
    /// </summary>
    /// <param name="start">
    /// The index, relative to the current position, of the first character in the
    /// subsequence; must be non-negative and no larger than <tt>Remaining()</tt>
    /// </param>
    /// <param name="end">
    /// The index, relative to the current position, of the character following the
    /// last character in the subsequence; must be no smaller than <tt>start</tt> and
    /// no larger than <tt>Remaining()</tt>
    /// </param>
    /// <returns> The new character buffer </returns>
    /// <exception cref="IndexOutOfRangeException">
    /// If the preconditions on <tt>start</tt> and <tt>end</tt> do not hold
    /// </exception>
    public override CharBuffer SubSequence( int start, int end )
    {
        try
        {
            return new StringCharBuffer( _string,
                                         -1,
                                         Position + CheckIndex( start, Position ),
                                         Position + CheckIndex( end, Position ),
                                         Capacity,
                                         Offset );
        }
        catch ( ArgumentException )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Tells whether or not this buffer is <i>direct</i>.
    /// </summary>
    /// <returns> <tt>true</tt> if, and only if, this buffer is direct </returns>
    public override bool IsDirect()
    {
        return false;
    }

    /// <summary>
    /// Retrieves this buffer's byte order.
    /// <para>
    /// The byte order of a char buffer created by allocation or by wrapping an existing
    /// <tt>char</tt> array is the <see cref="ByteOrder.NativeOrder"/> of the underlying
    /// hardware.  The byte order of a char buffer created as a view of a byte buffer is
    /// that of the byte buffer at the moment that the view is created.
    /// </para>
    /// </summary>
    /// <returns> This buffer's byte order </returns>
    public new ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }

    // ------------------------------------------------------------------------

    protected override string ToString( int start, int end )
    {
        return _string.Substring( start + Offset, end + Offset );
    }

    // ------------------------------------------------------------------------

    protected override CharBuffer Put( char c )
    {
        throw new GdxRuntimeException( "Buffer is Read Only!" );
    }

    public override CharBuffer Put( int index, char c )
    {
        throw new GdxRuntimeException( "Buffer is Read Only!" );
    }

    public override CharBuffer Compact()
    {
        throw new GdxRuntimeException( "Buffer is Read Only!" );
    }
}
