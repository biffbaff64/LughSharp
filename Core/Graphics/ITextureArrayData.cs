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
/// Before a call to either <see cref="ConsumeTextureArrayData()"/>, TextureArray will
/// bind the OpenGL ES texture.
/// </para>
/// Look at <see cref="FileTextureArrayData"/> for example implementation of this interface.
/// </summary>
public interface ITextureArrayData
{
    /// <returns> whether the TextureArrayData is prepared or not. </returns>
    bool Prepared { get; }

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

    /// <returns> the width of this TextureArray </returns>
    int Width { get; }

    /// <returns> the height of this TextureArray </returns>
    int Height { get; }

    /// <returns> the layer count of this TextureArray </returns>
    int Depth { get; }

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    bool Managed { get; }

    /// <returns> the internal format of this TextureArray </returns>
    int InternalFormat { get; }

    /// <returns> the GL type of this TextureArray </returns>
    int GLType { get; }
}

/// <summary>
/// Provides static method to instantiate the right implementation.
/// </summary>
public class TextureArrayDataFactory
{
    public static ITextureArrayData LoadFromFiles( Pixmap.Format format, bool useMipMaps, params FileInfo[] files )
    {
        return new FileTextureArrayData( format, useMipMaps, files );
    }
}
