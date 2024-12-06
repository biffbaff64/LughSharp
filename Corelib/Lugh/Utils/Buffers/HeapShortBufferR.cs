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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Utils.Buffers;

/// <summary>
/// A read-only HeapShortBuffer.  This class extends the corresponding read/write class,
/// overriding the mutation methods to throw a <see cref="ReadOnlyBufferException"/> and
/// overriding the view-buffer methods to return an instance of this class rather than
/// of the superclass.
/// </summary>
[PublicAPI]
public class HeapShortBufferR : HeapShortBuffer
{
    public HeapShortBufferR( int cap, int lim )
        : base( cap, lim )
    {
        SetBufferStatus( READ_ONLY, NOT_DIRECT );
    }

    public HeapShortBufferR( short[] buf, int off, int len )
        : base( buf, off, len )
    {
        SetBufferStatus( READ_ONLY, NOT_DIRECT );
    }

    public HeapShortBufferR( short[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( buf, mark, pos, lim, cap, off )
    {
        SetBufferStatus( READ_ONLY, NOT_DIRECT );
    }

    /// <inheritdoc />
    public override bool IsReadOnly => true;

    /// <override/>
    public override ShortBuffer Slice()
    {
        return new HeapShortBufferR( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <override/>
    public override ShortBuffer Duplicate()
    {
        return new HeapShortBufferR( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override ShortBuffer AsReadOnlyBuffer()
    {
        return Duplicate();
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }

    // ========================================================================
    // ========================================================================

    public override ShortBuffer Put( short x )
    {
        throw new ReadOnlyBufferException();
    }

    public override ShortBuffer Put( int i, short x )
    {
        throw new ReadOnlyBufferException();
    }

    public override ShortBuffer Put( short[] src, int offset, int length )
    {
        throw new ReadOnlyBufferException();
    }

    public override ShortBuffer Put( ShortBuffer src )
    {
        throw new ReadOnlyBufferException();
    }

    public override ShortBuffer Compact()
    {
        throw new ReadOnlyBufferException();
    }
}
