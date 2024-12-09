// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Utils.Buffers;

[PublicAPI]
public class HeapShortBuffer : ShortBuffer
{
    public HeapShortBuffer( int capacity, int limit )
        : base( -1, 0, limit, capacity, new short[ capacity ] )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    public HeapShortBuffer( short[] buffer, int offset, int length )
        : base( -1, offset, offset + length, buffer.Length, buffer )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    protected HeapShortBuffer( short[]? buffer, int mark, int pos, int limit, int capacity, int offset )
        : base( mark, pos, limit, capacity, buffer, offset )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <inheritdoc />
    public override ShortBuffer Slice()
    {
        return new HeapShortBuffer( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <inheritdoc />
    public override ShortBuffer Duplicate()
    {
        return new HeapShortBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override ShortBuffer AsReadOnlyBuffer()
    {
        return new HeapShortBufferR( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override short Get()
    {
        return Hb?[ Ix( NextGetIndex() ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override short Get( int index )
    {
        return Hb?[ Ix( CheckIndex( index ) ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override ShortBuffer Get( short[] dst, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow!" );
        }

        System.Array.Copy( Hb, Ix( Position ), dst, offset, length );

        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer Put( short s )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        Hb[ Ix( NextPutIndex() ) ] = s;

        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer Put( int index, short s )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        Hb[ Ix( CheckIndex( index ) ) ] = s;

        return this;
    }

    /// <inheritdoc />
    public override ShortBuffer Put( short[] src, int offset, int length )
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
    public override ShortBuffer Put( ShortBuffer src )
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
        }

        if ( src is HeapShortBuffer sb )
        {
            if ( Equals( src, this ) )
            {
                throw new ArgumentException();
            }

            var n = sb.Remaining();

            if ( n > Remaining() )
            {
                throw new GdxRuntimeException( "Buffer Overflow!" );
            }

            System.Array.Copy( sb.Hb!, sb.Ix( sb.Position ), Hb, Ix( Position ), n );

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
    public override ShortBuffer Compact()
    {
        if ( Hb == null )
        {
            throw new NullReferenceException();
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
