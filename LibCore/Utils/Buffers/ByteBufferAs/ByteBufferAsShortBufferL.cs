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


namespace LughSharp.LibCore.Utils.Buffers.ByteBufferAs;

[PublicAPI]
public class ByteBufferAsShortBufferL : ShortBuffer
{
    public ByteBufferAsShortBufferL( ByteBuffer bb, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap )
    {
    }

    /// <inheritdoc />
    public override bool IsDirect()
    {
        return false;
    }

    /// <inheritdoc />
    public override ShortBuffer Slice()
    {
        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer Duplicate()
    {
        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer AsReadOnlyBuffer()
    {
        return this;
    }

    /// <inheritdoc />
    public override short Get()
    {
        return 0;
    }

    /// <inheritdoc />
    public override ShortBuffer Put( short s )
    {
        return this;
    }

    /// <inheritdoc />
    public override short Get( int index )
    {
        return 0;
    }

    /// <inheritdoc />
    public override ShortBuffer Put( int index, short s )
    {
        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer Compact()
    {
        return this;
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }
}
