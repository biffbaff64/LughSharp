// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Utils.Buffers;

[PublicAPI]
public class HeapByteBuffer : ByteBuffer
{
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

    /// <inheritdoc/>
    public override ByteBuffer Slice()
    {
        return new HeapByteBuffer( Hb,
                                   -1,
                                   0,
                                   Remaining(),
                                   Remaining(),
                                   Position + Offset );
    }

    /// <inheritdoc/>
    public override ByteBuffer Duplicate()
    {
        return new HeapByteBuffer( Hb,
                                   MarkValue(),
                                   Position,
                                   Limit,
                                   Capacity,
                                   Offset );
    }

    /// <inheritdoc/>
    public override ByteBuffer AsReadOnlyBuffer()
    {
        return new HeapByteBuffer( Hb, MarkValue(), Position, Limit, Capacity, Offset );
    }

    protected int Ix( int i )
    {
        return i + Offset;
    }

    /// <inheritdoc/>
    public override byte Get()
    {
        return Hb?[ Ix( NextGetIndex() ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc/>
    public override byte Get( int index )
    {
        return Hb?[ Ix( CheckIndex( index ) ) ] ?? throw new NullReferenceException();
    }

    /// <inheritdoc/>
    public override ByteBuffer Get( byte[] dst, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
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

    /// <inheritdoc/>
    public override bool IsDirect() => false;

    /// <inheritdoc/>
    public override bool IsReadOnly => false;

    /// <inheritdoc/>
    public override ByteBuffer Put( byte b )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
        }

        Hb[ Ix( NextPutIndex() ) ] = b;

        return this;
    }

    /// <inheritdoc/>
    public override ByteBuffer Put( int index, byte b )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
        }

        Hb[ Ix( CheckIndex( index ) ) ] = b;

        return this;
    }

    /// <inheritdoc/>
    public override ByteBuffer Put( byte[] src, int offset, int length )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
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

    /// <inheritdoc/>
    public override ByteBuffer Put( ByteBuffer src )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
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
            
            System.Array.Copy( sb.Hb!,
                               sb.Ix( sb.Position ),
                               Hb,
                               Ix( Position ), n );
            
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

    /// <inheritdoc cref="ByteBuffer.Compact()" />
    public override ByteBuffer Compact()
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "HB is null!" );
        }

        System.Array.Copy( Hb, Ix( Position ), Hb, Ix( 0 ), Remaining() );

        SetPosition( Remaining() );
        SetLimit( Capacity );
        DiscardMark();
        
        return this;
    }

    /// <inheritdoc/>
    public override char GetChar()
    {
//        return Bits.GetChar( this, Ix( NextGetIndex( 2 ) ), BigEndian );
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override char GetChar( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutChar( char value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutChar( int index, char value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override CharBuffer AsCharBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override short GetShort()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override short GetShort( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutShort( short value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutShort( int index, short value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ShortBuffer AsShortBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetInt()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override int GetInt( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutInt( int value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutInt( int index, int value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override IntBuffer AsIntBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override long GetLong()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override long GetLong( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutLong( long value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutLong( int index, long value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override LongBuffer AsLongBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override float GetFloat()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override float GetFloat( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutFloat( float value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutFloat( int index, float value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override FloatBuffer AsFloatBuffer()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override double GetDouble()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override double GetDouble( int index )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutDouble( double value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override ByteBuffer PutDouble( int index, double value )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override DoubleBuffer AsDoubleBuffer()
    {
        throw new NotImplementedException();
    }
}