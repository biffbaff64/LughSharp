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
        MipMapLinearLinear
    }

    /// <summary>
    ///     Fetch the nearest texel that best maps to the pixel on screen.
    /// </summary>
    public readonly static TextureFilter Nearest = new( "Nearest", InnerEnum.Nearest, IGL20.GL_NEAREST );

    /// <summary>
    ///     Fetch four nearest texels that best maps to the pixel on screen.
    /// </summary>
    public readonly static TextureFilter Linear = new( "Linear", InnerEnum.Linear, IGL20.GL_LINEAR );

    /// <see cref="MipMapLinearLinear " />
    public readonly static TextureFilter MipMap = new( "MipMap", InnerEnum.MipMap, IGL20.GL_LINEAR_MIPMAP_LINEAR );

    /// <summary>
    ///     Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with
    ///     a
    ///     nearest filter.
    /// </summary>
    public readonly static TextureFilter MipMapNearestNearest = new( "MipMapNearestNearest",
                                                                     InnerEnum.MipMapNearestNearest,
                                                                     IGL20.GL_NEAREST_MIPMAP_NEAREST );

    /// <summary>
    ///     Fetch the best fitting image from the mip map chain based on the pixel/texel ratio and then sample the texels with
    ///     a
    ///     linear filter.
    /// </summary>
    public readonly static TextureFilter MipMapLinearNearest = new( "MipMapLinearNearest",
                                                                    InnerEnum.MipMapLinearNearest,
                                                                    IGL20.GL_LINEAR_MIPMAP_NEAREST );

    /// <summary>
    ///     Fetch the two best fitting images from the mip map chain and then sample the nearest texel from each of the two
    ///     images,
    ///     combining them to the final output pixel.
    /// </summary>
    public readonly static TextureFilter MipMapNearestLinear = new( "MipMapNearestLinear",
                                                                    InnerEnum.MipMapNearestLinear,
                                                                    IGL20.GL_NEAREST_MIPMAP_LINEAR );

    /// <summary>
    ///     Fetch the two best fitting images from the mip map chain and then sample the four nearest texels from each of the
    ///     two
    ///     images, combining them to the final output pixel.
    /// </summary>
    public readonly static TextureFilter MipMapLinearLinear = new( "MipMapLinearLinear",
                                                                   InnerEnum.MipMapLinearLinear,
                                                                   IGL20.GL_LINEAR_MIPMAP_LINEAR );

    private readonly static List< TextureFilter > ValueList = new();

    private static int _nextOrdinal = 0;

    private readonly string _nameValue;
    private readonly int    _ordinalValue;

    public readonly InnerEnum innerEnumValue;

    static TextureFilter()
    {
        ValueList.Add( Nearest );
        ValueList.Add( Linear );
        ValueList.Add( MipMap );
        ValueList.Add( MipMapNearestNearest );
        ValueList.Add( MipMapLinearNearest );
        ValueList.Add( MipMapNearestLinear );
        ValueList.Add( MipMapLinearLinear );
    }

    private TextureFilter( string name, InnerEnum innerEnum, int glEnum )
    {
        GLEnum = glEnum;

        _nameValue     = name;
        _ordinalValue  = _nextOrdinal++;
        innerEnumValue = innerEnum;
    }

    public int GLEnum { get; }

    public bool IsMipMap()
    {
        return ( GLEnum != IGL20.GL_NEAREST ) && ( GLEnum != IGL20.GL_LINEAR );
    }

    public static TextureFilter[] Values()
    {
        return ValueList.ToArray();
    }

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
        foreach ( TextureFilter enumInstance in ValueList )
        {
            if ( enumInstance._nameValue == name )
            {
                return enumInstance;
            }
        }

        throw new ArgumentException( name );
    }
}
