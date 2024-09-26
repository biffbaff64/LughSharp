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


using LughSharp.LibCore.Utils.Buffers.ByteBufferAs;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Utils.Buffers.HeapBuffers;

[PublicAPI]
public class HeapByteBuffer : ByteBuffer
{
    public HeapByteBuffer()
        : base( -1, 0, 0, 0 )
    {
    }
    
    /// <summary>
    /// Creates a new HeapByteBuffer with the given limit and  initial capacity. 
    /// </summary>
    public HeapByteBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new byte[ cap ] )
    {
    }

    public HeapByteBuffer( byte[]? buf, int off, int len )
        : base( -1, off, off + len, buf!.Length, buf )
    {
    }

    protected HeapByteBuffer( byte[]? buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
    }

    // ------------------------------------------------------------------------

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
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow!" );
        }

        Array.Copy( Hb, Ix( Position ), dst, offset, length );
        SetPosition( Position + length );

        return this;
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override ByteBuffer Put( byte b )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        Hb[ Ix( NextPutIndex() ) ] = b;

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( int index, byte b )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        Hb[ Ix( CheckIndex( index ) ) ] = b;

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( byte[] src, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        CheckBounds( offset, length, src.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Overflow!" );
        }

        Array.Copy( src, offset, Hb, Ix( Position ), length );
        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer Put( ByteBuffer src )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

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

            Array.Copy( sb.Hb!, sb.Ix( sb.Position ), Hb, Ix( Position ), n );

            sb.SetPosition( sb.Position + n );
            SetPosition( Position + n );
        }
        else if ( src.IsDirect() )
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

    // ------------------------------------------------------------------------
    
    /// <inheritdoc cref="ByteBuffer.Compact()"/>
    public override ByteBuffer Compact()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        Array.Copy( Hb, Ix( Position ), Hb, Ix( 0 ), Remaining() );

        SetPosition( Remaining() );
        SetLimit( Capacity );
        DiscardMark();

        return this;
    }

    // ------------------------------------------------------------------------
    
    /// <inheritdoc />
    public override char GetChar()
    {
        return BufferUtils.GetChar( this, Ix( NextGetIndex( 2 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override char GetChar( int index )
    {
        return BufferUtils.GetChar( this, Ix( CheckIndex( index, 2 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override short GetShort()
    {
        return BufferUtils.GetShort( this, Ix( NextGetIndex( 2 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override short GetShort( int i )
    {
        return BufferUtils.GetShort( this, Ix( CheckIndex( i, 2 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override int GetInt()
    {
        return BufferUtils.GetInt( this, Ix( NextGetIndex( 4 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override int GetInt( int i )
    {
        return BufferUtils.GetInt( this, Ix( CheckIndex( i, 4 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override long GetLong()
    {
        return BufferUtils.GetLong( this, Ix( NextGetIndex( 8 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override long GetLong( int i )
    {
        return BufferUtils.GetLong( this, Ix( CheckIndex( i, 8 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override double GetDouble()
    {
        return BufferUtils.GetDouble( this, Ix( NextGetIndex( 8 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override double GetDouble( int i )
    {
        return BufferUtils.GetDouble( this, Ix( CheckIndex( i, 8 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override float GetFloat()
    {
        return BufferUtils.GetFloat( this, Ix( NextGetIndex( 4 ) ), BigEndian );
    }

    /// <inheritdoc />
    public override float GetFloat( int i )
    {
        return BufferUtils.GetFloat( this, Ix( CheckIndex( i, 4 ) ), BigEndian );
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public override ByteBuffer PutChar( char value )
    {
        BufferUtils.PutChar( this, Ix( NextPutIndex( 2 ) ), value, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutChar( int index, char value )
    {
        BufferUtils.PutChar( this, Ix( CheckIndex( index, 2 ) ), value, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutShort( short x )
    {
        BufferUtils.PutShort( this, Ix( NextPutIndex( 2 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutShort( int i, short x )
    {
        BufferUtils.PutShort( this, Ix( CheckIndex( i, 2 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutInt( int x )
    {
        BufferUtils.PutInt( this, Ix( NextPutIndex( 4 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutInt( int i, int x )
    {
        BufferUtils.PutInt( this, Ix( CheckIndex( i, 4 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutLong( long x )
    {
        BufferUtils.PutLong( this, Ix( NextPutIndex( 8 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutLong( int i, long x )
    {
        BufferUtils.PutLong( this, Ix( CheckIndex( i, 8 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutFloat( float x )
    {
        BufferUtils.PutFloat( this, Ix( NextPutIndex( 4 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutFloat( int i, float x )
    {
        BufferUtils.PutFloat( this, Ix( CheckIndex( i, 4 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutDouble( double x )
    {
        BufferUtils.PutDouble( this, Ix( NextPutIndex( 8 ) ), x, BigEndian );

        return this;
    }

    /// <inheritdoc />
    public override ByteBuffer PutDouble( int i, double x )
    {
        BufferUtils.PutDouble( this, Ix( CheckIndex( i, 8 ) ), x, BigEndian );

        return this;
    }

    // ------------------------------------------------------------------------
    
    /// <inheritdoc />
    public override CharBuffer AsCharBuffer()
    {
        Logger.CheckPoint();
        
        var size = Remaining() >> 1;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsCharBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsCharBufferL( this, -1, 0, size, size, off );
    }

    /// <inheritdoc />
    public override ShortBuffer AsShortBuffer()
    {
        Logger.CheckPoint();
        
        var size = Remaining() >> 1;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsShortBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsShortBufferL( this, -1, 0, size, size, off );
    }

    /// <inheritdoc />
    public override IntBuffer AsIntBuffer()
    {
        Logger.CheckPoint();
        
        var size = Remaining() >> 2;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsIntBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsIntBufferL( this, -1, 0, size, size, off );
    }

    /// <inheritdoc />
    public override LongBuffer AsLongBuffer()
    {
        var size = Remaining() >> 3;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsLongBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsLongBufferL( this, -1, 0, size, size, off );
    }

    /// <inheritdoc />
    public override FloatBuffer AsFloatBuffer()
    {
        var size = Remaining() >> 2;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsFloatBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsFloatBufferL( this, -1, 0, size, size, off );
    }

    /// <inheritdoc />
    public override DoubleBuffer AsDoubleBuffer()
    {
        var size = Remaining() >> 3;
        var off  = Offset + Position;

        return BigEndian
                   ? new ByteBufferAsDoubleBufferB( this, -1, 0, size, size, off )
                   : new ByteBufferAsDoubleBufferL( this, -1, 0, size, size, off );
    }

    // ------------------------------------------------------------------------
    
    /// <inheritdoc />
    public override bool IsDirect() => false;

    /// <inheritdoc />
    public override bool IsReadOnly => false;

    /// <inheritdoc />
    protected override int Ix( int i ) => i + Offset;
    
    // ------------------------------------------------------------------------
}
