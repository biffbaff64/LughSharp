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


namespace LughSharp.LibCore.Graphics;

/// <summary>
///     A single vertex attribute defined by its <see cref="usage" />, its number
///     of components and its shader alias. The Usage is used for uniquely identifying
///     the vertex attribute from among its <see cref="VertexAttributes" /> siblings.
///     The number of components  defines how many components the attribute has. The
///     alias defines to which shader attribute this attribute should bind. The alias
///     is used by a <see cref="Mesh" /> when drawing with a <see cref="ShaderProgram" />.
///     The alias can be changed at any time.
/// </summary>
public class VertexAttribute
{
    private readonly int _usageIndex;

    /// <summary>
    ///     The alias for the attribute used in a <see cref="ShaderProgram" />
    /// </summary>
    public readonly string alias;

    /// <summary>
    ///     For fixed types, whether the values are normalized to either
    ///     -1f and +1f (signed) or 0f and +1f (unsigned)
    /// </summary>
    public readonly bool normalized;

    /// <summary>
    ///     the number of components this attribute has
    /// </summary>
    public readonly int numComponents;

    /// <summary>
    ///     the OpenGL type of each component, e.g. <see cref="IGL20.GL_FLOAT" />
    ///     or <see cref="IGL20.GL_UNSIGNED_BYTE" />
    /// </summary>
    public readonly int type;

    /// <summary>
    ///     optional unit/index specifier, used for texture coordinates and bone weights.
    /// </summary>
    public readonly int unit;
    /// <summary>
    ///     The attribute <see cref="usage" />, used for identification.
    /// </summary>
    public readonly int usage;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Constructs a new VertexAttribute. The GL data type is automatically
    ///     selected based on the usage.
    /// </summary>
    /// <param name="usage">
    ///     The attribute <see cref="usage" />, used to select the <see cref="type" />
    ///     and for identification.
    /// </param>
    /// <param name="numComponents">
    ///     the number of components of this attribute, must be between 1 and 4.
    /// </param>
    /// <param name="alias">
    ///     the alias used in a shader for this attribute. Can be changed after
    ///     construction.
    /// </param>
    /// <param name="unit">
    ///     Optional unit/index specifier, used for texture
    ///     coordinates and bone weights
    /// </param>
    public VertexAttribute( int usage, int numComponents, string alias, int unit = 0 )
        : this( usage,
                numComponents,
                usage == VertexAttributes.Usage.COLOR_PACKED ? IGL20.GL_UNSIGNED_BYTE : IGL20.GL_FLOAT,
                usage == VertexAttributes.Usage.COLOR_PACKED,
                alias,
                unit )
    {
    }

    /// <summary>
    ///     Constructs a new VertexAttribute.
    /// </summary>
    /// <param name="usage">
    ///     The attribute <see cref="usage" />, used for identification.
    /// </param>
    /// <param name="numComponents">
    ///     The number of components of this attribute, must be between 1 and 4.
    /// </param>
    /// <param name="type">
    ///     The OpenGL type of each component, e.g. <see cref="IGL20.GL_FLOAT" />
    ///     or <see cref="IGL20.GL_UNSIGNED_BYTE" />. Since <see cref="Mesh" />
    ///     stores vertex data in 32bit floats, the total size of this attribute
    ///     (type size times number of components) must be a multiple of four bytes.
    /// </param>
    /// <param name="normalized">
    ///     For fixed types, whether the values are normalized to either -1f and
    ///     +1f (signed) or 0f and +1f (unsigned)
    /// </param>
    /// <param name="alias">
    ///     The alias used in a shader for this attribute. Can be changed after
    ///     construction.
    /// </param>
    /// <param name="unit">
    ///     Optional unit/index specifier, used for texture coordinates and bone weights
    /// </param>
    public VertexAttribute( int usage,
                            int numComponents,
                            int type,
                            bool normalized,
                            string alias,
                            int unit = 0 )
    {
        this.usage         = usage;
        this.numComponents = numComponents;
        this.type          = type;
        this.normalized    = normalized;
        this.alias         = alias;
        this.unit          = unit;
        _usageIndex        = int.TrailingZeroCount( usage );
    }

    /// <summary>
    ///     the offset of this attribute in bytes, don't change this!
    /// </summary>
    public int Offset { get; set; }

    /// <returns>
    ///     A copy of this VertexAttribute with the same parameters.
    ///     The <see cref="Offset" /> is not copied and must be recalculated,
    ///     as is typically done by the <see cref="VertexAttributes" /> that
    ///     owns the VertexAttribute.
    /// </returns>
    public VertexAttribute Copy()
    {
        return new VertexAttribute( usage, numComponents, type, normalized, alias, unit );
    }

    public static VertexAttribute Position()
    {
        return new VertexAttribute( VertexAttributes.Usage.POSITION,
                                    3,
                                    ShaderProgram.POSITION_ATTRIBUTE );
    }

    public static VertexAttribute TexCoords( int unit )
    {
        return new VertexAttribute( VertexAttributes.Usage.TEXTURE_COORDINATES,
                                    2,
                                    ShaderProgram.TEXCOORD_ATTRIBUTE + unit,
                                    unit );
    }

    public static VertexAttribute Normal()
    {
        return new VertexAttribute( VertexAttributes.Usage.NORMAL,
                                    3,
                                    ShaderProgram.NORMAL_ATTRIBUTE );
    }

    public static VertexAttribute ColorPacked()
    {
        return new VertexAttribute( VertexAttributes.Usage.COLOR_PACKED,
                                    4,
                                    IGL20.GL_UNSIGNED_BYTE,
                                    true,
                                    ShaderProgram.COLOR_ATTRIBUTE );
    }

    public static VertexAttribute ColorUnpacked()
    {
        return new VertexAttribute( VertexAttributes.Usage.COLOR_UNPACKED,
                                    4,
                                    IGL20.GL_FLOAT,
                                    false,
                                    ShaderProgram.COLOR_ATTRIBUTE );
    }

    public static VertexAttribute Tangent()
    {
        return new VertexAttribute( VertexAttributes.Usage.TANGENT,
                                    3,
                                    ShaderProgram.TANGENT_ATTRIBUTE );
    }

    public static VertexAttribute Binormal()
    {
        return new VertexAttribute( VertexAttributes.Usage.BI_NORMAL,
                                    3,
                                    ShaderProgram.BINORMAL_ATTRIBUTE );
    }

    public static VertexAttribute BoneWeight( int unit )
    {
        return new VertexAttribute( VertexAttributes.Usage.BONE_WEIGHT,
                                    2,
                                    ShaderProgram.BONEWEIGHT_ATTRIBUTE + unit,
                                    unit );
    }

    /// <summary>
    ///     Tests to determine if the passed object was created with the same parameters
    /// </summary>
    public new bool Equals( object obj )
    {
        if ( obj is not VertexAttribute attribute )
        {
            return false;
        }

        return Equals( attribute );
    }

    public bool Equals( VertexAttribute? other )
    {
        return ( other != null )
            && ( usage == other.usage )
            && ( numComponents == other.numComponents )
            && ( type == other.type )
            && ( normalized == other.normalized )
            && alias.Equals( other.alias )
            && ( unit == other.unit );
    }

    /// <returns>
    ///     A unique number specifying the usage index (3 MSB) and unit (1 LSB).
    /// </returns>
    public int GetKey()
    {
        return ( _usageIndex << 8 ) + ( unit & 0xFF );
    }

    /// <summary>
    /// </summary>
    /// <returns>How many bytes this attribute uses.</returns>
    public int GetSizeInBytes()
    {
        return type switch
               {
                   IGL20.GL_FLOAT          => 4 * numComponents,
                   IGL20.GL_FIXED          => 4 * numComponents,
                   IGL20.GL_UNSIGNED_SHORT => 2 * numComponents,
                   IGL20.GL_SHORT          => 2 * numComponents,
                   IGL20.GL_UNSIGNED_BYTE  => numComponents,
                   IGL20.GL_BYTE           => numComponents,
                   _                       => 0
               };
    }

    public int HashCode()
    {
        var result = GetKey();

        result = ( 541 * result ) + numComponents;
        result = ( 541 * result ) + ( alias.Length * unit );

        return result;
    }
}
