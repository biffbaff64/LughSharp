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
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Graphics;

/// <summary>
/// Used by a <see cref="Cubemap"/> to load the pixel data. The Cubemap will
/// request the CubemapData to prepare itself through <see cref="Prepare()"/>
/// and upload its data using <see cref="ConsumeCubemapData()"/>. These are
/// the first methods to be called by Cubemap.
/// <para>
/// After that the Cubemap will invoke the other methods to find out about the
/// size of the image data, the format, whether the CubemapData is able to
/// manage the pixel data if the OpenGL ES context is lost.
/// </para>
/// <para>
/// Before a call to either <see cref="ConsumeCubemapData()"/>, Cubemap will
/// bind the OpenGL ES texture.
/// </para>
/// Look at <see cref="KtxTextureData"/> for example implementation of this
/// interface.
/// </summary>
[PublicAPI]
public interface ICubemapData : IManaged
{
    /// <summary>
    /// whether the TextureData is prepared or not.
    /// </summary>
    bool IsPrepared { get; }

    /// <summary>
    /// The width of the pixel data.
    /// </summary>
    int Width { get; }

    /// <summary>
    /// The height of the pixel data.
    /// </summary>
    int Height { get; }

    /// <summary>
    /// Prepares the TextureData for a call to <see cref="ConsumeCubemapData()"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL.
    /// </summary>
    void Prepare();

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="Prepare()"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="Prepare()"/> should be disposed of here.
    /// </summary>
    void ConsumeCubemapData();
}
