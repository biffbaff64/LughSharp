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

/// <summary>
/// Extend this class to implement a material attribute. Register the attribute type by
/// statically calling the <see cref="Register(string)"/> method, whose return value should
/// be used to instantiate the attribute. A class can implement multiple types
/// </summary>
public abstract class Attribute : IComparable< Attribute >
{
    // The type of this attribute
    public readonly long type;
    private readonly int  _typeBit;

    /// <summary>
    /// The registered type aliases
    /// </summary>
    private readonly static List< string > Types = new();

    protected Attribute( long type )
    {
        this.type     = type;
        this._typeBit = ( int )long.TrailingZeroCount( type );
    }

    /// <summary>
    /// Returns An exact copy of this attribute.
    /// </summary>
    public abstract Attribute Copy();

    /// <summary>
    /// Returns the ID of the specified attribute type, or zero if not available
    /// </summary>
    public static long GetAttributeType( string alias )
    {
        for ( var i = 0; i < Types.Count; i++ )
        {
            if ( string.Compare( Types[ i ], alias, StringComparison.Ordinal ) == 0 )
            {
                return 1L << i;
            }
        }

        return 0;
    }

    /// <summary>
    /// Returns the alias of the specified attribute type, or zero if not available
    /// </summary>
    public static string? GetAttributeAlias( long type )
    {
        var idx = -1;

        while ( ( type != 0 ) && ( ++idx < 63 ) && ( ( ( type >> idx ) & 1 ) == 0 ) )
        {
        }

        return ( ( idx >= 0 ) && ( idx < Types.Count ) ) ? Types[ idx ] : null;
    }

    /// <summary>
    /// Call this method to register a custom attribute type. If the alias already
    /// exists, then that ID will be reused. The alias should be unambiguous and will
    /// by default be returned by the call to <see cref="ToString()"/>.
    /// </summary>
    /// <param name="alias">
    /// The alias of the type to register, must be different for each dirrect type,
    /// will be used for debugging.
    /// </param>
    /// <returns>
    /// the ID of the newly registered type, or the ID of the existing type if the
    /// alias was already registered.
    /// </returns>
    protected static long Register( string alias )
    {
        var result = GetAttributeType( alias );

        if ( result > 0 )
        {
            return result;
        }

        Types.Add( alias );

        return 1L << ( Types.Count - 1 );
    }

    protected bool Equals( Attribute other )
    {
        return other.GetHashCode() == this.GetHashCode();
    }

    /// <inheritdoc />
    public int CompareTo( Attribute? other )
    {
        if ( other == null )
        {
            return 1;
        }

        return _typeBit - other._typeBit;
    }
    
    /// <inheritdoc />
    public override bool Equals( object? obj )
    {
        if ( obj == null )
        {
            return false;
        }

        if ( obj == this )
        {
            return true;
        }

        if ( obj is not Attribute other )
        {
            return false;
        }

        return ( this.type == other.type ) && Equals( other );
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return GetAttributeAlias( type );
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 7489 * _typeBit;
    }
}
