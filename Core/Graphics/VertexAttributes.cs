// ///////////////////////////////////////////////////////////////////////////////
// Copyright [2023] [Richard Ikin]
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http: //www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

using System.Text;

namespace LibGDXSharp.Graphics;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class VertexAttributes
{
    public static class Usage
    {
        public const int POSITION            = 1;
        public const int COLOR_UNPACKED      = 2;
        public const int COLOR_PACKED        = 4;
        public const int NORMAL              = 8;
        public const int TEXTURE_COORDINATES = 16;
        public const int GENERIC             = 32;
        public const int BONE_WEIGHT         = 64;
        public const int TANGENT             = 128;
        public const int BI_NORMAL           = 256;
    }

    /// <summary>
    /// the attributes in the order they were specified
    /// </summary>
    private readonly VertexAttribute[] _attributes;

    /// <summary>
    /// the size of a single vertex in bytes
    /// </summary>
    public int VertexSize { get; set; }

    /// <summary>
    /// cache of the value calculated by <see cref="Mask"/>.
    /// </summary>
    private long _mask = -1;

//    private ReadonlyIterable< VertexAttribute > _iterable;

    /// <summary>
    /// Constructor, sets the vertex attributes in a specific order.
    /// </summary>
    public VertexAttributes( params VertexAttribute[] attributes )
    {
        if ( attributes.Length == 0 )
        {
            throw new System.ArgumentException( "attributes must be >= 1" );
        }

        var list = new VertexAttribute[ attributes.Length ];

        for ( var i = 0; i < attributes.Length; i++ )
        {
            list[ i ] = attributes[ i ];
        }

        this._attributes = list;
        VertexSize       = CalculateOffsets();
    }

    /// <summary>
    /// Returns the offset for the first VertexAttribute with the specified usage. </summary>
    /// <param name="usage"> The usage of the VertexAttribute.</param>
    /// <param name="defaultIfNotFound"></param>
    protected int GetOffset( int usage, int defaultIfNotFound )
    {
        VertexAttribute? vertexAttribute = FindByUsage( usage );

        if ( vertexAttribute == null )
        {
            return defaultIfNotFound;
        }

        return vertexAttribute.Offset / 4;
    }

    /// <summary>
    /// Returns the offset for the first VertexAttribute with the specified usage. </summary>
    /// <param name="usage"> The usage of the VertexAttribute.  </param>
    public int GetOffset( int usage )
    {
        return GetOffset( usage, 0 );
    }

    /// <summary>
    /// Returns the first VertexAttribute for the given usage. </summary>
    /// <param name="usage"> The usage of the VertexAttribute to find.  </param>
    public VertexAttribute? FindByUsage( int usage )
    {
        var len = Size;

        for ( var i = 0; i < len; i++ )
        {
            if ( Get( i ).usage == usage )
            {
                return Get( i );
            }
        }

        return null;
    }

    private int CalculateOffsets()
    {
        var count = 0;

        foreach ( VertexAttribute attribute in _attributes )
        {
            attribute.Offset =  count;
            count            += attribute.GetSizeInBytes();
        }

        return count;
    }

    /// <returns> the number of attributes </returns>
    public int Size => _attributes.Length;

    /// <param name="index"> the index </param>
    /// <returns> the VertexAttribute at the given index  </returns>
    public VertexAttribute Get( int index )
    {
        return _attributes[ index ];
    }

    public override string ToString()
    {
        var builder = new StringBuilder();
        builder.Append( '[' );

        foreach ( VertexAttribute t in _attributes )
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

        if ( this._attributes.Length != other._attributes.Length ) return false;

        for ( var i = 0; i < _attributes.Length; i++ )
        {
            if ( !_attributes[ i ].Equals( other._attributes[ i ] ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int HashCode()
    {
        long result = 61 * _attributes.Length;

        foreach ( VertexAttribute t in _attributes )
        {
            result = ( result * 61 ) + t.HashCode();
        }

        return ( int )( result ^ ( result >> 32 ) );
    }

    /// <summary>
    /// Calculates a mask based on the contained <see cref="VertexAttribute"/>
    /// instances. The mask is a bit-wise or of each attributes
    /// <seealso cref="VertexAttribute.usage"/>.
    /// </summary>
    /// <returns> the mask  </returns>
    protected long Mask
    {
        get
        {
            if ( _mask == -1 )
            {
                long result = 0;

                foreach ( VertexAttribute t in _attributes )
                {
                    result |= ( uint )t.usage;
                }

                _mask = result;
            }

            return _mask;
        }
    }

    /// <summary>
    /// Calculates the mask based on <see cref="Mask"/> and packs
    /// the attributes count into the last 32 bits.
    /// </summary>
    /// <returns> the mask with attributes count packed into the last 32 bits.  </returns>
    public virtual long MaskWithSizePacked => Mask | ( ( long )_attributes.Length << 32 );

    public int CompareTo( VertexAttributes o )
    {
        if ( _attributes.Length != o._attributes.Length )
        {
            return _attributes.Length - o._attributes.Length;
        }

        var m1 = Mask;
        var m2 = o.Mask;

        if ( m1 != m2 )
        {
            return m1 < m2 ? -1 : 1;
        }

        for ( var i = _attributes.Length - 1; i >= 0; --i )
        {
            VertexAttribute va0 = _attributes[ i ];
            VertexAttribute va1 = o._attributes[ i ];

            if ( va0.usage != va1.usage )
            {
                return va0.usage - va1.usage;
            }

            if ( va0.unit != va1.unit )
            {
                return va0.unit - va1.unit;
            }

            if ( va0.numComponents != va1.numComponents )
            {
                return va0.numComponents - va1.numComponents;
            }

            if ( va0.normalized != va1.normalized )
            {
                return va0.normalized ? 1 : -1;
            }

            if ( va0.type != va1.type )
            {
                return va0.type - va1.type;
            }
        }

        return 0;
    }
}
