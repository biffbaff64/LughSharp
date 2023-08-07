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

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class TextureFilter
{
    /// <summary>
    /// Fetch the nearest texel that best maps to the pixel on screen. </summary>
    public readonly static TextureFilter NEAREST = new( "Nearest", InnerEnum.Nearest, IGL20.GL_NEAREST );

    /// <summary>
    /// Fetch four nearest texels that best maps to the pixel on screen. </summary>
    public readonly static TextureFilter LINEAR = new( "Linear", InnerEnum.Linear, IGL20.GL_LINEAR );

    /// <see cref="MIP_MAP_LINEAR_LINEAR "/>
    public readonly static TextureFilter MIP_MAP = new( "MipMap", InnerEnum.MipMap, IGL20.GL_LINEAR_MIPMAP_LINEAR );

    /// <summary>
    /// Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with a
    /// nearest filter. 
    /// </summary>
    public readonly static TextureFilter MIP_MAP_NEAREST_NEAREST = new( "MipMapNearestNearest", InnerEnum.MipMapNearestNearest, IGL20.GL_NEAREST_MIPMAP_NEAREST );

    /// <summary>
    /// Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with a
    /// linear filter. 
    /// </summary>
    public readonly static TextureFilter MIP_MAP_LINEAR_NEAREST = new( "MipMapLinearNearest", InnerEnum.MipMapLinearNearest, IGL20.GL_LINEAR_MIPMAP_NEAREST );

    /// <summary>
    /// Fetch the two best fitting images from the mip map chain and then sample the nearest texel from each of the two images,
    /// combining them to the final output pixel. 
    /// </summary>
    public readonly static TextureFilter MIP_MAP_NEAREST_LINEAR = new( "MipMapNearestLinear", InnerEnum.MipMapNearestLinear, IGL20.GL_NEAREST_MIPMAP_LINEAR );

    /// <summary>
    /// Fetch the two best fitting images from the mip map chain and then sample the four nearest texels from each of the two
    /// images, combining them to the final output pixel. 
    /// </summary>
    public readonly static TextureFilter MIP_MAP_LINEAR_LINEAR = new( "MipMapLinearLinear", InnerEnum.MipMapLinearLinear, IGL20.GL_LINEAR_MIPMAP_LINEAR );

    private readonly static List< TextureFilter > VALUE_LIST = new();

    static TextureFilter()
    {
        VALUE_LIST.Add( NEAREST );
        VALUE_LIST.Add( LINEAR );
        VALUE_LIST.Add( MIP_MAP );
        VALUE_LIST.Add( MIP_MAP_NEAREST_NEAREST );
        VALUE_LIST.Add( MIP_MAP_LINEAR_NEAREST );
        VALUE_LIST.Add( MIP_MAP_NEAREST_LINEAR );
        VALUE_LIST.Add( MIP_MAP_LINEAR_LINEAR );
    }

    public enum InnerEnum
    {
        Nearest,
        Linear,
        MipMap,
        MipMapNearestNearest,
        MipMapLinearNearest,
        MipMapNearestLinear,
        MipMapLinearLinear
    }

    public readonly InnerEnum innerEnumValue;

    private readonly string _nameValue;
    private readonly int    _ordinalValue;

    private static int _nextOrdinal = 0;

    private TextureFilter( string name, InnerEnum innerEnum, int glEnum )
    {
        this.GLEnum = glEnum;

        _nameValue     = name;
        _ordinalValue  = _nextOrdinal++;
        innerEnumValue = innerEnum;
    }

    public bool IsMipMap() => ( GLEnum != IGL20.GL_NEAREST ) && ( GLEnum != IGL20.GL_LINEAR );

    public int GLEnum { get; }

    public static TextureFilter[] Values() => VALUE_LIST.ToArray();

    public int Ordinal()
    {
        return _ordinalValue;
    }

    public override string ToString()
    {
        return _nameValue;
    }

    public static TextureFilter ValueOf( string name )
    {
        foreach ( var enumInstance in TextureFilter.VALUE_LIST )
        {
            if ( enumInstance._nameValue == name )
            {
                return enumInstance;
            }
        }

        throw new System.ArgumentException( name );
    }
}