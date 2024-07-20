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

[PublicAPI]
public class HeapDoubleBuffer : DoubleBuffer
{
    public HeapDoubleBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new double[ cap ] )
    {
    }

    public HeapDoubleBuffer( double[] buf, int off, int len )
        : base( -1, off, off + len, buf.Length, buf )
    {
    }

    /// <inheritdoc />
    public HeapDoubleBuffer( double[]? hb, int mark, int pos, int lim, int cap, int offset = 0 )
        : base( mark, pos, lim, cap, hb, offset )
    {
    }

    /// <inheritdoc />
    public override DoubleBuffer Slice()
    {
        return new HeapDoubleBuffer( Hb, -1, 0, Remaining(), Remaining(), Position + Offset );
    }

    /// <inheritdoc />
    public override DoubleBuffer Duplicate()
    {
        return new HeapDoubleBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override DoubleBuffer asReadOnlyBuffer()
    {
        return new HeapDoubleBuffer( Hb, Mark, Position, Limit, Capacity, Offset );
    }

    /// <inheritdoc />
    public override double Get()
    {
        return Hb?[ Ix( NextGetIndex() ) ] ?? throw new GdxRuntimeException( "Heap buffer is null!" );
    }

    /// <inheritdoc />
    public override double Get( int index )
    {
        return Hb?[ Ix( CheckIndex( index ) ) ] ?? throw new GdxRuntimeException( "Heap buffer is null!" );
    }

    /// <inheritdoc />
    public override DoubleBuffer Get( double[] dst, int offset, int length )
    {
        CheckBounds( offset, length, dst.Length );

        if ( length > Remaining() )
        {
            throw new GdxRuntimeException( "Buffer Underflow!" );
        }

        System.Array.Copy( Hb!, Ix( Position ), dst, offset, length );

        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( double d )
    {
        Put( NextPutIndex(), d );

        return this;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( int index, double d )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        Hb[ Ix( NextPutIndex() ) ] = d;

        return this;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( double[] src, int offset, int length )
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

        System.Array.Copy( src, offset, Hb, Ix( Position ), length );
        SetPosition( Position + length );

        return this;
    }

    /// <inheritdoc />
    public override DoubleBuffer Put( DoubleBuffer src )
    {
        if ( Hb == null )
        {
            throw new GdxRuntimeException( "Heap buffer is null!" );
        }

        if ( src is HeapDoubleBuffer sb )
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

            System.Array.Copy( sb.Hb!, sb.Ix( sb.Position ), Hb, Ix( Position ), n );

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

    /// <inheritdoc />
    public override DoubleBuffer Compact()
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
    public override bool IsDirect()
    {
        return false;
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }

    // ------------------------------------------------------------------------

    protected int Ix( int i )
    {
        return i + Offset;
    }
}
