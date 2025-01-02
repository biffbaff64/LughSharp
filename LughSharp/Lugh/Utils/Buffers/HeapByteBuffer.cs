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

using LughSharp.Lugh.Utils.Exceptions;

namespace LughSharp.Lugh.Utils.Buffers;

[PublicAPI]
public class HeapByteBuffer : ByteBuffer
{
    /// <summary>
    /// Represents a byte buffer that is backed by a byte array.
    /// </summary>
    public HeapByteBuffer()
        : base( -1, 0, 0, 0 )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Represents a byte buffer with a specified capacity and limit.
    /// </summary>
    public HeapByteBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new byte[ cap ] )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Represents a byte buffer that is backed by a byte array.
    /// </summary>
    public HeapByteBuffer( byte[]? buf, int off, int len )
        : base( -1, off, off + len, buf!.Length, buf )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    /// <summary>
    /// Represents a byte buffer that is backed by a heap-allocated byte array.
    /// </summary>
    protected HeapByteBuffer( byte[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
        SetBufferStatus( READ_WRITE, NOT_DIRECT );
    }

    // ========================================================================
    // ========================================================================

    /// <inheritdoc />
    public override ByteBuffer Slice()
    {
        return new HeapByteBuffer( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <inheritdoc />
    public override ByteBuffer Duplicate()
    {
        return new HeapByteBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override ByteBuffer AsReadOnlyBuffer()
    {
        return new HeapByteBufferR( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override byte Get()
    {
        return Hb?[ Ix( NextGetIndex() ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override byte Get( int index )
    {
        return Hb?[ Ix( CheckIndex( index ) ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc />
    public override ByteBuffer Get( byte[] dst, int offset, int length )
    {
        ValidateBackingArray();

        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow!" );
        }

        Array.Copy( Hb!, Ix( Position ), dst, offset, length );
        SetPosition( Position + length );

        return this;
    }

    // ========================================================================

    /// <inheritdoc />
    public override ByteBuffer Put( byte b )
    {
        if ( IsReadOnly )
        {
            return this;
        }
        
        ValidateBackingArray();

        Hb![ Ix( NextPutIndex() ) ] = b;

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( int index, byte b )
    {
        if ( IsReadOnly )
        {
            return this;
        }
        
        ValidateBackingArray();

        Hb![ Ix( CheckIndex( index ) ) ] = b;

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( byte[] src, int offset, int length )
    {
        if ( IsReadOnly )
        {
            return this;
        }
        
        ValidateBackingArray();

        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Overflow!" );
        }

        Array.Copy( src, offset, Hb!, Ix( Position ), length );
        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( ByteBuffer src )
    {
        if ( IsReadOnly )
        {
            return this;
        }
        
        ValidateBackingArray();

        if ( src is HeapByteBuffer sb )
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

            Array.Copy( sb.Hb!, sb.Ix( sb.Position ), Hb!, Ix( Position ), n );

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

            src.Get( Hb!, Ix( Position ), n );
            SetPosition( Position + n );
        }
        else
        {
            base.Put( src );
        }

        return this;
    }

    // ========================================================================
    
    /// <inheritdoc cref="ByteBuffer.Compact()"/>
    public override ByteBuffer Compact()
    {
        if ( IsReadOnly )
        {
            return this;
        }
        
        ValidateBackingArray();

        Array.Copy( Hb!, Ix( Position ), Hb!, Ix( 0 ), Remaining() );

        SetPosition( Remaining() );
        SetLimit( Capacity );
        DiscardMark();

        return this;
    }
    
    // ========================================================================
    // ========================================================================
    // ========================================================================
}
