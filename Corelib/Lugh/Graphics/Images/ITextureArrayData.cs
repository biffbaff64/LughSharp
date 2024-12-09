// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Graphics.GLUtils;

namespace Corelib.Lugh.Graphics.Images;

/// <summary>
/// Used by a <see cref="TextureArray"/> to load the pixel data. The TextureArray will
/// request the TextureArrayData to prepare itself through <see cref="Prepare()"/> and
/// upload its data using <see cref="ConsumeTextureArrayData()"/>. These are the first
/// methods to be called by TextureArray.
/// <para>
/// After that the TextureArray will invoke the other methods to find out about the size
/// of the image data, the format, whether the TextureArrayData is able to manage the
/// pixel data if the OpenGL ES context is lost.
/// </para>
/// <para>
/// Before a call to <see cref="ConsumeTextureArrayData()"/>, TextureArray will
/// bind the OpenGL ES texture.
/// </para>
/// Look at <see cref="FileTextureArrayData"/> for example implementation of this interface.
/// </summary>
[PublicAPI]
public interface ITextureArrayData
{
    /// <summary>
    /// Returns whether the TextureArrayData is prepared or not. .
    /// </summary>
    bool Prepared { get; }

    /// <summary>
    /// Returns the width of this TextureArray .
    /// </summary>
    int Width { get; }

    /// <summary>
    /// Returns the height of this TextureArray .
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Returns the layer count of this TextureArray .
    /// </summary>
    int Depth { get; }

    /// <summary>
    /// Returns whether this implementation can cope with a EGL context loss. .
    /// </summary>
    bool Managed { get; }

    /// <summary>
    /// Returns the internal format of this TextureArray .
    /// </summary>
    int InternalFormat { get; }

    /// <summary>
    /// Returns the GL type of this TextureArray .
    /// </summary>
    int GLType { get; }

    /// <summary>
    /// Prepares the TextureArrayData for a call to <see cref="ConsumeTextureArrayData()"/>.
    /// This method can be called from a non OpenGL thread and should thus not interact
    /// with OpenGL.
    /// </summary>
    void Prepare();

    /// <summary>
    /// Uploads the pixel data of the TextureArray layers of the TextureArray to the OpenGL
    /// ES texture. The caller must bind an OpenGL ES texture. A call to <see cref="Prepare()"/>
    /// must preceed a call to this method.
    /// <para>
    /// Any internal data structures created in <see cref="Prepare()"/> should be disposed of here.
    /// </para>
    /// </summary>
    void ConsumeTextureArrayData();
}

/// <summary>
/// Provides static method to instantiate the right implementation.
/// </summary>
[PublicAPI]
public class TextureArrayDataFactory
{
    public static ITextureArrayData LoadFromFiles( Pixmap.ColorFormat format,
                                                   bool useMipMaps,
                                                   params FileInfo[] files )
    {
        return new FileTextureArrayData( format, useMipMaps, files );
    }
}
