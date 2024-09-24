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
public class ByteBufferAsShortBufferB : ShortBuffer
{
    protected ByteBuffer Bb;

    public ByteBufferAsShortBufferB( ByteBuffer bb, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap )
    {
        this.Bb      = bb;
        base.Address = bb.Address;
    }

    /// <inheritdoc />
    public override bool IsDirect() => Bb.IsDirect();

    /// <inheritdoc />
    public override bool IsReadOnly => false;

    /// <inheritdoc />
    public override ShortBuffer Slice()
    {
        var pos  = this.Position;
        var lim  = this.Limit;
        var rem  = ( pos <= lim ? lim - pos : 0 );
//        var addr = ByteOffset( pos );

        return new ByteBufferAsShortBufferB( Bb, -1, 0, rem, rem, 0 ); //addr );
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
        var pos = Position;
        var lim = Limit;

        Debug.Assert( pos <= lim );

        var rem = ( pos <= lim ? lim - pos : 0 );

        var db = Bb.Duplicate();

        db.SetLimit( Ix( lim ) );
        db.SetPosition( Ix( 0 ) );

        var sb = db.Slice();

        sb.SetPosition( pos << 1 );
        sb.Compact();

        SetPosition( rem );
        SetLimit( Capacity );
        DiscardMark();

        return this;
    }

    /// <inheritdoc />
    public override ByteOrder Order()
    {
        return ByteOrder.NativeOrder;
    }

    protected long ByteOffset( long i )
    {
        return ( i << 1 ) + Address;
    }

    private int Ix( int i )
    {
        var off = ( int ) ( Address - Bb.Address );

        return ( i << 1 ) + off;
    }
}