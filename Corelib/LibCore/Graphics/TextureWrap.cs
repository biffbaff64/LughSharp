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

/// <summary>
/// </summary>
[PublicAPI]
public class TextureWrap
{
    public enum InnerEnum
    {
        MirroredRepeat,
        ClampToEdge,
        Repeat,
    }

    // ------------------------------------------------------------------------

    public static readonly TextureWrap MirroredRepeat = new
        (
         "MirroredRepeat",
         InnerEnum.MirroredRepeat,
         IGL.GL_MIRRORED_REPEAT
        );

    public static readonly TextureWrap ClampToEdge = new
        (
         "ClampToEdge",
         InnerEnum.ClampToEdge,
         IGL.GL_CLAMP_TO_EDGE
        );

    public static readonly TextureWrap Repeat = new
        (
         "Repeat",
         InnerEnum.Repeat,
         IGL.GL_REPEAT
        );

    // ------------------------------------------------------------------------

    public int GLEnum       { get; }
    public int OrdinalValue { get; set; }

    // ------------------------------------------------------------------------

    public readonly InnerEnum InnerEnumValue;

    // ------------------------------------------------------------------------

    private static readonly List< TextureWrap > _valueList = new();

    private static   int    _nextOrdinal = 0;
    private readonly string _nameValue;

    // ------------------------------------------------------------------------

    static TextureWrap()
    {
        _valueList.Add( MirroredRepeat );
        _valueList.Add( ClampToEdge );
        _valueList.Add( Repeat );
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="innerEnum"></param>
    /// <param name="glEnum"></param>
    public TextureWrap( string name, InnerEnum innerEnum, int glEnum )
    {
        GLEnum = glEnum;

        _nameValue     = name;
        OrdinalValue   = _nextOrdinal++;
        InnerEnumValue = innerEnum;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static TextureWrap[] Values()
    {
        return _valueList.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static TextureWrap ValueOf( string name )
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