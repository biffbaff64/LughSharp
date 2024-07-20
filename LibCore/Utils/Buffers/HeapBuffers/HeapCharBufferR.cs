// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Utils.Buffers.HeapBuffers;

/// <summary>
/// A read-only HeapCharBuffer.  This class extends the corresponding read/write class,
/// overriding the mutation methods to throw a <see cref="ReadOnlyBufferException"/> and
/// overriding the view-buffer methods to return an instance of this class rather than
/// of the superclass.
/// </summary>
[PublicAPI]
public class HeapCharBufferR : HeapCharBuffer
{
    public HeapCharBufferR( int cap, int lim )
        : base( cap, lim )
    {
    }

    public HeapCharBufferR( char[] buf, int off, int len )
        : base( buf, off, len )
    {
    }

    protected HeapCharBufferR( char[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( buf, mark, pos, lim, cap, off )
    {
    }

    /// <inheritdoc />
    public override bool IsReadOnly => true;

    /// <inheritdoc />
    public override CharBuffer Slice()
    {
        return new HeapCharBufferR( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <inheritdoc />
    public override CharBuffer Duplicate()
    {
        return new HeapCharBufferR( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override CharBuffer AsReadOnlyBuffer()
    {
        return Duplicate();
    }

    /// <inheritdoc />
    protected override string ToString( int start, int end )
    {
        try
        {
            return new string( Hb!, start + Offset, end - start );
        }
        catch ( Exception )
        {
            throw new IndexOutOfRangeException();
        }
    }

    /// <inheritdoc />
    public override CharBuffer SubSequence( int start, int end )
    {
        if ( ( start < 0 ) || ( end > Length() ) || ( start > end ) )
        {
            throw new IndexOutOfRangeException();
        }

        return new HeapCharBufferR( Hb, -1, Position + start, Position + end, Capacity, Offset );
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public override CharBuffer Put( char[] src, int offset, int length )
    {
        throw new ReadOnlyBufferException();
    }

    public override CharBuffer Put( CharBuffer src )
    {
        throw new ReadOnlyBufferException();
    }

    protected override CharBuffer Put( char x )
    {
        throw new ReadOnlyBufferException();
    }

    public override CharBuffer Put( int i, char x )
    {
        throw new ReadOnlyBufferException();
    }

    public override CharBuffer Compact()
    {
        throw new ReadOnlyBufferException();
    }
}
