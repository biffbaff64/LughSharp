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

namespace LibGDXSharp.Graphics;

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
    ///     The attribute <see cref="usage" />, used for identification.
    /// </summary>
    public readonly int usage;

    /// <summary>
    ///     optional unit/index specifier, used for texture coordinates and bone weights.
    /// </summary>
    public int unit;

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
    public VertexAttribute Copy() => new( usage, numComponents, type, normalized, alias, unit );

    public static VertexAttribute Position() => new(
        VertexAttributes.Usage.POSITION,
        3,
        ShaderProgram.POSITION_ATTRIBUTE
        );

    public static VertexAttribute TexCoords( int unit ) => new(
        VertexAttributes.Usage.TEXTURE_COORDINATES,
        2,
        ShaderProgram.TEXCOORD_ATTRIBUTE + unit,
        unit
        );

    public static VertexAttribute Normal() => new( VertexAttributes.Usage.NORMAL,
                                                   3,
                                                   ShaderProgram.NORMAL_ATTRIBUTE );

    public static VertexAttribute ColorPacked() => new( VertexAttributes.Usage.COLOR_PACKED,
                                                        4,
                                                        IGL20.GL_UNSIGNED_BYTE,
                                                        true,
                                                        ShaderProgram.COLOR_ATTRIBUTE );

    public static VertexAttribute ColorUnpacked() => new( VertexAttributes.Usage.COLOR_UNPACKED,
                                                          4,
                                                          IGL20.GL_FLOAT,
                                                          false,
                                                          ShaderProgram.COLOR_ATTRIBUTE );

    public static VertexAttribute Tangent() => new( VertexAttributes.Usage.TANGENT,
                                                    3,
                                                    ShaderProgram.TANGENT_ATTRIBUTE );

    public static VertexAttribute Binormal() => new( VertexAttributes.Usage.BI_NORMAL,
                                                     3,
                                                     ShaderProgram.BINORMAL_ATTRIBUTE );

    public static VertexAttribute BoneWeight( int unit ) => new( VertexAttributes.Usage.BONE_WEIGHT,
                                                                 2,
                                                                 ShaderProgram.BONEWEIGHT_ATTRIBUTE + unit,
                                                                 unit );

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

    public bool Equals( VertexAttribute? other ) => ( other != null )
                                                 && ( usage == other.usage )
                                                 && ( numComponents == other.numComponents )
                                                 && ( type == other.type )
                                                 && ( normalized == other.normalized )
                                                 && alias.Equals( other.alias )
                                                 && ( unit == other.unit );

    /// <returns>
    ///     A unique number specifying the usage index (3 MSB) and unit (1 LSB).
    /// </returns>
    public int GetKey() => ( _usageIndex << 8 ) + ( unit & 0xFF );

    /// <summary>
    /// </summary>
    /// <returns>How many bytes this attribute uses.</returns>
    public int GetSizeInBytes() => type switch
                                   {
                                       IGL20.GL_FLOAT          => 4 * numComponents,
                                       IGL20.GL_FIXED          => 4 * numComponents,
                                       IGL20.GL_UNSIGNED_SHORT => 2 * numComponents,
                                       IGL20.GL_SHORT          => 2 * numComponents,
                                       IGL20.GL_UNSIGNED_BYTE  => numComponents,
                                       IGL20.GL_BYTE           => numComponents,
                                       _                       => 0
                                   };

    public int HashCode()
    {
        var result = GetKey();

        result = ( 541 * result ) + numComponents;
        result = ( 541 * result ) + ( alias.Length * unit );

        return result;
    }
}
