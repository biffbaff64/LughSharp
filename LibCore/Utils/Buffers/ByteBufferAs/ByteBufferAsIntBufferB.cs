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
public class ByteBufferAsIntBufferB : IntBuffer
{
    /// <inheritdoc />
    public ByteBufferAsIntBufferB( ByteBuffer bb, int mark, int pos, int lim, int cap, int offset = 0 )
        : base( mark, pos, lim, cap )
    {
        Logger.CheckPoint();
    }

    /// <inheritdoc />
    public override bool IsDirect()
    {
        return false;
    }

    /// <inheritdoc />
    public override IntBuffer Slice()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override IntBuffer Duplicate()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override IntBuffer asReadOnlyBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override int Get()
    {
        return 0;
    }

    /// <inheritdoc />
    public override IntBuffer Put( int i )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override int Get( int index )
    {
        return 0;
    }

    /// <inheritdoc />
    public override IntBuffer Put( int index, int i )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override IntBuffer Compact()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        throw new NotImplementedException();
    }
}
