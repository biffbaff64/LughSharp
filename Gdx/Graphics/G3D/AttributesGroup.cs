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

namespace LibGDXSharp.Graphics.G3D;

public class AttributesGroup : IComparer< Attribute >
{
    private readonly List< Attribute > _attributes = new();

    private long _mask;
    private bool _sorted = true;

    /// <summary>
    /// Sort the attributes by their ID.
    /// </summary>
    public void Sort()
    {
        if ( !_sorted )
        {
            _attributes.Sort( this );
            _sorted = true;
        }
    }

    /// <summary>
    /// Return a bitwise mask of the ID's of all the containing attributes
    /// </summary>
    public long GetMask()
    {
        return _mask;
    }

    public Attribute? Get( long type )
    {
        if ( Has( type ) )
        {
            foreach ( Attribute att in _attributes )
            {
                if ( att.type == type )
                {
                    return att;
                }
            }
        }

        return null;
    }

    public T? Get<T>( long type ) where T : Attribute
    {
        return ( T? )Get( type );
    }

    public List< Attribute > Get( List< Attribute > output, long type )
    {
        foreach ( Attribute att in _attributes )
        {
            if ( ( att.type & type ) != 0 )
            {
                output.Add( att );
            }
        }

        return output;
    }

    public void Clear()
    {
        _mask = 0;
        _attributes.Clear();
    }

    public int Size()
    {
        return _attributes.Count;
    }

    private void Enable( long mask )
    {
        this._mask |= mask;
    }

    private void Disable( long mask )
    {
        this._mask &= ~mask;
    }

    /// <summary>
    /// Add a attribute to this material. If the material already contains an attribute
    /// of the same type it is overwritten.
    /// </summary>
    public void Set( Attribute attribute )
    {
        var idx = IndexOf( attribute.type );

        if ( idx < 0 )
        {
            Enable( attribute.type );
            
            _attributes.Add( attribute );
            _sorted = false;
        }
        else
        {
            _attributes[ idx ] = attribute;
        }

        Sort(); //FIXME: See #4186
    }

    /// <summary>
    /// Add multiple attributes to this material. If the material already contains
    /// an attribute of the same type it is overwritten.
    /// </summary>
    [Obsolete( "Use Set( params Attribute[] attribs ) instead." )]
    public void Set( Attribute attribute1, Attribute attribute2 )
    {
        Set( attribute1 );
        Set( attribute2 );
    }

    /// <summary>
    /// Add multiple attributes to this material. If the material already contains
    /// an attribute of the same type it is overwritten.
    /// </summary>
    [Obsolete( "Use Set( params Attribute[] attribs ) instead." )]
    public void Set( Attribute attribute1, Attribute attribute2, Attribute attribute3 )
    {
        Set( attribute1 );
        Set( attribute2 );
        Set( attribute3 );
    }

    /// <summary>
    /// Add multiple attributes to this material. If the material already contains
    /// an attribute of the same type it is overwritten.
    /// </summary>
    [Obsolete( "Use Set( params Attribute[] attribs ) instead." )]
    public void Set( Attribute attribute1,
                     Attribute attribute2,
                     Attribute attribute3,
                     Attribute attribute4 )
    {
        Set( attribute1 );
        Set( attribute2 );
        Set( attribute3 );
        Set( attribute4 );
    }

    /// <summary>
    /// Add multiple attributes to this material. If the material already contains
    /// an attribute of the same type it is overwritten.
    /// </summary>
    public void Set( params Attribute[] attribs )
    {
        foreach ( Attribute attr in attribs )
        {
            Set( attr );
        }
    }

    /// <summary>
    /// Add an array of attributes to this material. If the material already contains
    /// an attribute of the same type it is overwritten.
    /// </summary>
    public void Set( IEnumerable< Attribute > attribs )
    {
        foreach ( Attribute attr in attribs )
        {
            Set( attr );
        }
    }

    /// <summary>
    /// Removes the attribute from the material, i.e.: Remove(BlendingAttribute.ID);
    /// Can also be used to remove multiple attributes also, i.e. Remove(AttributeA.ID | AttributeB.ID);
    /// </summary>
    public void Remove( long mask )
    {
        for ( var i = _attributes.Count - 1; i >= 0; i-- )
        {
            var type = _attributes[ i ].type;

            if ( ( mask & type ) == type )
            {
                _attributes.RemoveAt( i );
                Disable( type );
                _sorted = false;
            }
        }

        Sort(); //FIXME: See #4186
    }

    /// <summary>
    /// True if this collection has the specified attribute, i.e. Has(ColorAttribute.Diffuse);
    /// Or when multiple attribute types are specified, true if this collection has all specified
    /// attributes;
    /// <para>
    /// i.e. Has(ColorAttribute.Diffuse | ColorAttribute.Specular | TextureAttribute.Diffuse);
    /// </para>
    /// </summary>
    public bool Has( long type )
    {
        return ( type != 0 ) && ( ( this._mask & type ) == type );
    }

    /// <summary>
    /// Returns the index of the attribute with the specified type or negative if not available.
    /// </summary>
    protected int IndexOf( long type )
    {
        if ( Has( type ) )
        {
            for ( var i = 0; i < _attributes.Count; i++ )
            {
                if ( _attributes[ i ].type == type )
                {
                    return i;
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Check if this collection has the same attributes as the other collection. If
    /// compareValues is true, it also compares the values of each attribute.
    /// </summary>
    /// <param name="other"></param>
    /// <param name="compareValues">
    /// True to compare attribute values, false to only compare attribute types
    /// </param>
    /// <returns>
    /// True if this collection contains the same attributes (and optionally attribute values) as the other.
    /// </returns>
    public bool Same( AttributesGroup? other, bool compareValues = false )
    {
        if ( ( other == null ) || ( _mask != other._mask ) )
        {
            return false;
        }

        if ( other.Equals( this ) )
        {
            return true;
        }

        if ( !compareValues )
        {
            return true;
        }

        this.Sort();
        other.Sort();

        for ( var i = 0; i < _attributes.Count; i++ )
        {
            if ( !_attributes[ i ].Equals( other._attributes[ i ] ) )
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Used for sorting attributes by type (not by value)
    /// </summary>
    public int Compare( Attribute? arg0, Attribute? arg1 )
    {
        ArgumentNullException.ThrowIfNull( arg0 );
        ArgumentNullException.ThrowIfNull( arg1 );
        
        return ( int )( arg0.type - arg1.type );
    }

    /// <summary>
    /// Returns a hash code based on only the attribute values, which might be different
    /// compared to <see cref="GetHashCode()"/> because the latter might include other
    /// properties as well, i.e. the material id.
    /// </summary>
    public int AttributesHash()
    {
        Sort();

        var n      = _attributes.Count;
        var result = 71 + _mask;
        var m      = 1;

        for ( var i = 0; i < n; i++ )
        {
            result += _mask * _attributes[ i ].GetHashCode() * ( m = ( m * 7 ) & 0xFFFF );
        }

        return ( int )( result ^ ( result >> 32 ) );
    }

    public override int GetHashCode()
    {
        return AttributesHash();
    }

    public override bool Equals( object? other )
    {
        if ( other is not AttributesGroup group )
        {
            return false;
        }

        return ( group == this ) || Same( group, true );
    }

    public int CompareTo( AttributesGroup other )
    {
        if ( other.Equals( this ) )
        {
            return 0;
        }

        if ( _mask != other._mask )
        {
            return _mask < other._mask ? -1 : 1;
        }

        Sort();
        other.Sort();

        for ( var i = 0; i < _attributes.Count; i++ )
        {
            var c = _attributes[ i ].CompareTo( other._attributes[ i ] );

            if ( c != 0 )
            {
                return c < 0 ? -1 : 1;
            }
        }

        return 0;
    }
}
