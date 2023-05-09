namespace LibGDXSharp.Graphics;

/// <summary>
/// A single vertex attribute defined by its <see cref="usage"/>, its number
/// of components and its shader alias. The Usage is used for uniquely identifying
/// the vertex attribute from among its <see cref="VertexAttributes"/> siblings.
/// The number of components  defines how many components the attribute has. The
/// alias defines to which shader attribute this attribute should bind. The alias 
/// is used by a <see cref="Mesh"/> when drawing with a <see cref="ShaderProgram"/>.
/// The alias can be changed at any time.
/// </summary>
public class VertexAttribute
{
    /// <summary>
    /// The attribute <see cref="usage"/>, used for identification.
    /// </summary>
    public readonly int usage;

    /// <summary>
    /// the number of components this attribute has
    /// </summary>
    public readonly int numComponents;

    /// <summary>
    /// For fixed types, whether the values are normalized to either
    /// -1f and +1f (signed) or 0f and +1f (unsigned)
    /// </summary>
    public readonly bool normalized;

    /// <summary>
    /// the OpenGL type of each component, e.g. <see cref="IGL20.GL_Float"/>
    /// or <see cref="IGL20.GL_Unsigned_Byte"/>
    /// </summary>
    public readonly int type;

    /// <summary>
    /// the offset of this attribute in bytes, don't change this!
    /// </summary>
    public int offset;

    /// <summary>
    /// The alias for the attribute used in a <see cref="ShaderProgram"/>
    /// </summary>
    public string alias;

    /// <summary>
    /// optional unit/index specifier, used for texture coordinates and bone weights.
    /// </summary>
    public int unit;
    private readonly int _usageIndex;

    /// <summary>
    /// Constructs a new VertexAttribute. The GL data type is automatically
    /// selected based on the usage.
    /// </summary>
    /// <param name="usage">
    /// The attribute <see cref="usage"/>, used to select the <see cref="type"/>
    /// and for identification.
    /// </param>
    /// <param name="numComponents">
    /// the number of components of this attribute, must be between 1 and 4.
    /// </param>
    /// <param name="alias">
    /// the alias used in a shader for this attribute. Can be changed after
    /// construction.
    /// </param>
    /// <param name="unit">Optional unit/index specifier, used for texture
    /// coordinates and bone weights
    /// </param>
    public VertexAttribute( int usage, int numComponents, string alias, int unit = 0 )
        : this
            (
             usage,
             numComponents,
             usage == VertexAttributes.Usage.ColorPacked ? IGL20.GL_Unsigned_Byte : IGL20.GL_Float,
             usage == VertexAttributes.Usage.ColorPacked,
             alias,
             unit
            )
    {
    }

    /// <summary>
    /// Constructs a new VertexAttribute.
    /// </summary>
    /// <param name="usage">
    /// The attribute <see cref="usage"/>, used for identification.
    /// </param>
    /// <param name="numComponents">
    /// The number of components of this attribute, must be between 1 and 4.
    /// </param>
    /// <param name="type">
    /// The OpenGL type of each component, e.g. <see cref="IGL20.GL_Float"/>
    /// or <see cref="IGL20.GL_Unsigned_Byte"/>. Since <see cref="Mesh"/>
    /// stores vertex data in 32bit floats, the total size of this attribute
    /// (type size times number of components) must be a multiple of four bytes.
    /// </param>
    /// <param name="normalized">
    /// For fixed types, whether the values are normalized to either -1f and
    /// +1f (signed) or 0f and +1f (unsigned)
    /// </param>
    /// <param name="alias">
    /// The alias used in a shader for this attribute. Can be changed after
    /// construction.
    /// </param>
    /// <param name="unit">
    /// Optional unit/index specifier, used for texture coordinates and bone weights
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
        this._usageIndex   = int.TrailingZeroCount( usage );
    }

    /// <returns>
    /// A copy of this VertexAttribute with the same parameters.
    /// The <see cref="offset"/> is not copied and must be recalculated,
    /// as is typically done by the <see cref="VertexAttributes"/> that
    /// owns the VertexAttribute.
    /// </returns>
    public virtual VertexAttribute Copy()
    {
        return new VertexAttribute( usage, numComponents, type, normalized, alias, unit );
    }

    public static VertexAttribute Position()
    {
        return new VertexAttribute
            (
             VertexAttributes.Usage.Position,
             3,
             ShaderProgram.Position_Attribute
            );
    }

    public static VertexAttribute TexCoords( int unit )
    {
        return new VertexAttribute
            (
             VertexAttributes.Usage.TextureCoordinates,
             2,
             ShaderProgram.Texcoord_Attribute + unit,
             unit
            );
    }

    public static VertexAttribute Normal()
    {
        return new VertexAttribute( VertexAttributes.Usage.Normal, 
                                    3, ShaderProgram.Normal_Attribute );
    }

    public static VertexAttribute ColorPacked()
    {
        return new VertexAttribute( VertexAttributes.Usage.ColorPacked, 
                                    4, IGL20.GL_Unsigned_Byte,
                                    true, ShaderProgram.Color_Attribute );
    }

    public static VertexAttribute ColorUnpacked()
    {
        return new VertexAttribute( VertexAttributes.Usage.ColorUnpacked, 
                                    4, IGL20.GL_Float, 
                                    false, ShaderProgram.Color_Attribute );
    }

    public static VertexAttribute Tangent()
    {
        return new VertexAttribute( VertexAttributes.Usage.Tangent, 
                                    3, ShaderProgram.Tangent_Attribute );
    }

    public static VertexAttribute Binormal()
    {
        return new VertexAttribute( VertexAttributes.Usage.BiNormal, 
                                    3, ShaderProgram.Binormal_Attribute );
    }

    public static VertexAttribute BoneWeight( int unit )
    {
        return new VertexAttribute( VertexAttributes.Usage.BoneWeight, 2, 
                                    ShaderProgram.Boneweight_Attribute + unit, unit );
    }

    /** Tests to determine if the passed object was created with the same parameters */
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
               && ( alias.Equals( other.alias ) )
               && ( unit == other.unit );
    }

    /// <returns>
    /// A unique number specifying the usage index (3 MSB) and unit (1 LSB).
    /// </returns>
    public int GetKey() => ( _usageIndex << 8 ) + ( unit & 0xFF );

    /// <summary>
    /// </summary>
    /// <returns>How many bytes this attribute uses.</returns>
    public int GetSizeInBytes()
    {
        switch ( type )
        {
            case IGL20.GL_Float:
            case IGL20.GL_Fixed:
                return 4 * numComponents;

            case IGL20.GL_Unsigned_Byte:
            case IGL20.GL_Byte:
                return numComponents;

            case IGL20.GL_Unsigned_Short:
            case IGL20.GL_Short:
                return 2 * numComponents;
        }

        return 0;
    }

    public int HashCode()
    {
        var result = GetKey();
        
        result = ( 541 * result ) + numComponents;
        result = ( 541 * result ) + alias.HashCode();

        return result;
    }
}
