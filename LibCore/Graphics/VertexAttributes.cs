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


using System.Text;

namespace LughSharp.LibCore.Graphics;

[PublicAPI]
public class VertexAttributes
{
    /// <summary>
    ///     the attributes in the order they were specified
    /// </summary>
    private readonly VertexAttribute[] _attributes;

    /// <summary>
    ///     cache of the value calculated by <see cref="Mask" />.
    /// </summary>
    private long _mask = -1;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Constructor, sets the vertex attributes in a specific order.
    /// </summary>
    public VertexAttributes( params VertexAttribute[] attributes )
    {
        if ( attributes.Length == 0 ) throw new ArgumentException( "attributes must be >= 1" );

        var list = new VertexAttribute[ attributes.Length ];

        for ( var i = 0; i < attributes.Length; i++ ) list[ i ] = attributes[ i ];

        _attributes = list;
        VertexSize  = CalculateOffsets();
    }

    /// <summary>
    ///     the size of a single vertex in bytes
    /// </summary>
    public int VertexSize { get; private set; }

    /// <summary>
    ///     Gets the number of attributes.
    /// </summary>
    public int Size => _attributes.Length;

    /// <summary>
    ///     Calculates a mask based on the contained <see cref="VertexAttribute" />
    ///     instances. The mask is a bit-wise or of each attributes
    ///     <seealso cref="VertexAttribute.usage" />.
    /// </summary>
    /// <returns> the mask  </returns>
    protected long Mask
    {
        get
        {
            if ( _mask == -1 )
            {
                long result = 0;

                foreach ( var t in _attributes ) result |= ( uint ) t.usage;

                _mask = result;
            }

            return _mask;
        }
    }

    /// <summary>
    ///     Calculates the mask based on <see cref="Mask" /> and packs
    ///     the attributes count into the last 32 bits.
    /// </summary>
    /// <returns>
    ///     the mask with attributes count packed into the last 32 bits.
    /// </returns>
    public virtual long MaskWithSizePacked => Mask | ( ( long ) _attributes.Length << 32 );

    /// <summary>
    ///     Returns the offset for the first VertexAttribute with the specified usage.
    /// </summary>
    /// <param name="usage"> The usage of the VertexAttribute.</param>
    /// <param name="defaultIfNotFound"></param>
    protected int GetOffset( int usage, int defaultIfNotFound )
    {
        var vertexAttribute = FindByUsage( usage );

        if ( vertexAttribute == null ) return defaultIfNotFound;

        return vertexAttribute.Offset / 4;
    }

    /// <summary>
    ///     Returns the offset for the first VertexAttribute with the specified usage.
    /// </summary>
    /// <param name="usage"> The usage of the VertexAttribute.  </param>
    public int GetOffset( int usage )
    {
        return GetOffset( usage, 0 );
    }

    /// <summary>
    ///     Returns the first VertexAttribute for the given usage.
    /// </summary>
    /// <param name="usage"> The usage of the VertexAttribute to find.  </param>
    public VertexAttribute? FindByUsage( int usage )
    {
        var len = Size;

        for ( var i = 0; i < len; i++ )
            if ( Get( i ).usage == usage )
                return Get( i );

        return null;
    }

    private int CalculateOffsets()
    {
        var count = 0;

        foreach ( var attribute in _attributes )
        {
            attribute.Offset =  count;
            count            += attribute.GetSizeInBytes();
        }

        return count;
    }

    /// <summary>
    ///     Gets the VertexAttribute at the given index.
    /// </summary>
    /// <param name="index"> the index </param>
    public VertexAttribute Get( int index )
    {
        return _attributes[ index ];
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append( '[' );

        foreach ( var t in _attributes )
        {
            builder.Append( '(' );
            builder.Append( t.alias );
            builder.Append( ", " );
            builder.Append( t.usage );
            builder.Append( ", " );
            builder.Append( t.numComponents );
            builder.Append( ", " );
            builder.Append( t.Offset );
            builder.Append( ')' );
            builder.Append( '\n' );
        }

        builder.Append( ']' );

        return builder.ToString();
    }

    /// <summary>
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public new bool Equals( object obj )
    {
        if ( obj == this ) return true;

        if ( obj is not VertexAttributes other ) return false;

        if ( _attributes.Length != other._attributes.Length ) return false;

        for ( var i = 0; i < _attributes.Length; i++ )
            if ( !_attributes[ i ].Equals( other._attributes[ i ] ) )
                return false;

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int HashCode()
    {
        long result = 61 * _attributes.Length;

        foreach ( var t in _attributes ) result = ( result * 61 ) + t.HashCode();

        return ( int ) ( result ^ ( result >> 32 ) );
    }

    public int CompareTo( VertexAttributes o )
    {
        if ( _attributes.Length != o._attributes.Length ) return _attributes.Length - o._attributes.Length;

        var m1 = Mask;
        var m2 = o.Mask;

        if ( m1 != m2 ) return m1 < m2 ? -1 : 1;

        for ( var i = _attributes.Length - 1; i >= 0; --i )
        {
            var va0 = _attributes[ i ];
            var va1 = o._attributes[ i ];

            if ( va0.usage != va1.usage ) return va0.usage - va1.usage;

            if ( va0.unit != va1.unit ) return va0.unit - va1.unit;

            if ( va0.numComponents != va1.numComponents ) return va0.numComponents - va1.numComponents;

            if ( va0.normalized != va1.normalized ) return va0.normalized ? 1 : -1;

            if ( va0.type != va1.type ) return va0.type - va1.type;
        }

        return 0;
    }

    public struct Usage
    {
        internal const int POSITION            = 1;
        internal const int COLOR_UNPACKED      = 2;
        internal const int COLOR_PACKED        = 4;
        internal const int NORMAL              = 8;
        internal const int TEXTURE_COORDINATES = 16;
        internal const int GENERIC             = 32;
        internal const int BONE_WEIGHT         = 64;
        internal const int TANGENT             = 128;
        internal const int BI_NORMAL           = 256;
    }
}