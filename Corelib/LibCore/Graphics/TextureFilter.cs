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

using Corelib.LibCore.Graphics.OpenGL;

namespace Corelib.LibCore.Graphics;

[PublicAPI]
public class TextureFilter
{
    public enum InnerEnum
    {
        Nearest,
        Linear,
        MipMap,
        MipMapNearestNearest,
        MipMapLinearNearest,
        MipMapNearestLinear,
        MipMapLinearLinear,
    }

    // ========================================================================

    public int GLEnum  { get; }
    public int Ordinal { get; set; }

    // ========================================================================

    private static readonly List< TextureFilter > _valueList = new();
    private readonly        string                _nameValue;
    private static          int                   _nextOrdinal = 0;

    internal readonly InnerEnum InnerEnumValue;

    // ========================================================================

    /// <summary>
    /// Fetch the nearest texel that best maps to the pixel on screen.
    /// </summary>
    public static readonly TextureFilter Nearest = new( "Nearest",
                                                        InnerEnum.Nearest,
                                                        IGL.GL_NEAREST );

    /// <summary>
    /// Fetch four nearest texels that best maps to the pixel on screen.
    /// </summary>
    public static readonly TextureFilter Linear = new( "Linear",
                                                       InnerEnum.Linear,
                                                       IGL.GL_LINEAR );

    public static readonly TextureFilter MipMap = new( "MipMap",
                                                       InnerEnum.MipMap,
                                                       IGL.GL_LINEAR_MIPMAP_LINEAR );

    /// <summary>
    /// Fetch the best fitting image from the mip map chain based on the
    /// pixel/texel ratio and then sample the texels with a nearest filter.
    /// </summary>
    public static readonly TextureFilter MipMapNearestNearest = new( "MipMapNearestNearest",
                                                                     InnerEnum.MipMapNearestNearest,
                                                                     IGL.GL_NEAREST_MIPMAP_NEAREST );

    /// <summary>
    /// Fetch the best fitting image from the mip map chain based on the
    /// pixel/texel ratio and then sample the texels with a linear filter.
    /// </summary>
    public static readonly TextureFilter MipMapLinearNearest = new( "MipMapLinearNearest",
                                                                    InnerEnum.MipMapLinearNearest,
                                                                    IGL.GL_LINEAR_MIPMAP_NEAREST );

    /// <summary>
    /// Fetch the two best fitting images from the mip map chain and then
    /// sample the nearest texel from each of the two images, combining them
    /// to the final output pixel.
    /// </summary>
    public static readonly TextureFilter MipMapNearestLinear = new( "MipMapNearestLinear",
                                                                    InnerEnum.MipMapNearestLinear,
                                                                    IGL.GL_NEAREST_MIPMAP_LINEAR );

    /// <summary>
    /// Fetch the two best fitting images from the mip map chain and then
    /// sample the four nearest texels from each of the two images, combining
    /// them to the final output pixel.
    /// </summary>
    public static readonly TextureFilter MipMapLinearLinear = new( "MipMapLinearLinear",
                                                                   InnerEnum.MipMapLinearLinear,
                                                                   IGL.GL_LINEAR_MIPMAP_LINEAR );

    // ========================================================================
    // ========================================================================

    static TextureFilter()
    {
        _valueList.Add( Nearest );
        _valueList.Add( Linear );
        _valueList.Add( MipMap );
        _valueList.Add( MipMapNearestNearest );
        _valueList.Add( MipMapLinearNearest );
        _valueList.Add( MipMapNearestLinear );
        _valueList.Add( MipMapLinearLinear );
    }

    private TextureFilter( string name, InnerEnum innerEnum, int glEnum )
    {
        GLEnum = glEnum;

        _nameValue     = name;
        Ordinal        = _nextOrdinal++;
        InnerEnumValue = innerEnum;
    }

    // ========================================================================

    public bool IsMipMap()
    {
        return ( GLEnum != IGL.GL_NEAREST ) && ( GLEnum != IGL.GL_LINEAR );
    }

    public static TextureFilter[] Values()
    {
        return _valueList.ToArray();
    }

    public static TextureFilter ValueOf( string name )
    {
        foreach ( var enumInstance in _valueList )
        {
            if ( enumInstance._nameValue == name )
            {
                return enumInstance;
            }
        }

        throw new ArgumentException( name );
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _nameValue;
    }
}