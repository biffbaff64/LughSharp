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

using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Utils.Buffers;

[PublicAPI]
public class HeapIntBuffer : IntBuffer
{
    public HeapIntBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new int[ cap ] )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    public HeapIntBuffer( int[] cap, int offset, int length )
        : base( -1, offset, offset + length, cap.Length, cap )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    public HeapIntBuffer( int[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <inheritdoc />
    public override IntBuffer Slice()
    {
        return new HeapIntBuffer( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <inheritdoc />
    public override IntBuffer Duplicate()
    {
        return new HeapIntBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override IntBuffer asReadOnlyBuffer()
    {
        return new HeapIntBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override int Get()
    {
        return Hb?[ Ix( NextGetIndex() ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override int Get( int index )
    {
        return Hb?[ Ix( CheckIndex( index ) ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override IntBuffer Get( int[] dst, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow" );
        }

        System.Array.Copy( Hb, Ix( Position ), dst, offset, length );
        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override IntBuffer Put( int x )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        Hb[ Ix( NextPutIndex() ) ] = x;

        return this;
    }

    /// <inheritdoc />
    public override IntBuffer Put( int index, int i )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        Hb[ Ix( CheckIndex( index ) ) ] = i;

        return this;
    }

    /// <inheritdoc />
    public override IntBuffer Put( int[] src, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Overflow!" );
        }

        System.Array.Copy( src, offset, Hb, Ix( Position ), length );
        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override IntBuffer Put( IntBuffer src )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        if ( src is HeapIntBuffer sb )
        {
            if ( Equals( sb, this ) )
            {
                throw new ArgumentException();
            }

            var n = sb.Remaining();

            if ( n > Remaining() )
            {
                throw new GdxRuntimeException( "Buffer Overflow!" );
            }

            System.Array.Copy( sb.Hb!,
                               sb.Ix( sb.Position ),
                               Hb,
                               Ix( Position ),
                               n );

            sb.SetPosition( sb.Position + n );
            SetPosition( Position + n );
        }
        else if ( src.IsDirect )
        {
            var n = src.Remaining();

            if ( n > Remaining() )
            {
                throw new GdxRuntimeException( "Buffer Overflow!" );
            }

            src.Get( Hb, Ix( Position ), n );

            SetPosition( Position + n );
        }
        else
        {
            base.Put( src );
        }

        return this;
    }

    /// <inheritdoc />
    public override IntBuffer Compact()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        System.Array.Copy( Hb, Ix( Position ), Hb, Ix( 0 ), Remaining() );

        SetPosition( Remaining() );
        SetLimit( Capacity );
        DiscardMark();

        return this;
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }
}
