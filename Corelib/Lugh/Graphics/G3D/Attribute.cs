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


namespace Corelib.Lugh.Graphics.G3D;

/// <summary>
/// Extend this class to implement a material attribute. Register the attribute type by
/// statically calling the <see cref="Register(string)"/> method, whose return value should
/// be used to instantiate the attribute. A class can implement multiple types
/// </summary>
public abstract class Attribute : IComparable< Attribute >
{
    /// <summary>
    /// The registered type aliases
    /// </summary>
    private static readonly List< string > Types = new();

    private readonly int _typeBit;

    // The type of this attribute
    public readonly long type;

    protected Attribute( long type )
    {
        this.type = type;
        _typeBit  = ( int ) long.TrailingZeroCount( type );
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

        return ( idx >= 0 ) && ( idx < Types.Count ) ? Types[ idx ] : null;
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
        return other.GetHashCode() == GetHashCode();
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

        return ( type == other.type ) && Equals( other );
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
