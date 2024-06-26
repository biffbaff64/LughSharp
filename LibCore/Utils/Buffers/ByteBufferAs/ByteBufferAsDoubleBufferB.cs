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
public class ByteBufferAsDoubleBufferB : DoubleBuffer
{
    /// <inheritdoc />
    public ByteBufferAsDoubleBufferB( ByteBuffer bb, int mark, int pos, int lim, int cap, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
    }

    /// <inheritdoc />
    public override bool IsDirect()
    {
        return false;
    }

    /// <inheritdoc />
    public override DoubleBuffer Slice()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DoubleBuffer Duplicate()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DoubleBuffer asReadOnlyBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override double Get()
    {
        return 0;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( double d )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override double Get( int index )
    {
        return 0;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( int index, double d )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override DoubleBuffer Compact()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        throw new NotImplementedException();
    }
}
