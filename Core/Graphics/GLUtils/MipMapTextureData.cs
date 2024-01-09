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

namespace LibGDXSharp.Graphics.GLUtils;

/// <summary>
///     This class will load each contained TextureData to the chosen
///     mipmap level. All the mipmap levels must be defined and cannot be null.
/// </summary>
public class MipMapTextureData : ITextureData
{
    private readonly ITextureData[] _mips;

    /// <summary>
    /// </summary>
    /// <param name="mipMapData"> must be != null and its length must be >= 1 </param>
    public MipMapTextureData( params ITextureData[] mipMapData )
    {
        _mips = new ITextureData[ mipMapData.Length ];

        Array.Copy( mipMapData, 0, _mips, 0, mipMapData.Length );
    }

    public bool IsPrepared { get; set; }
    public bool UseMipMaps { get; set; }
    public int  Width      { get; set; }
    public int  Height     { get; set; }

    /// <summary>
    ///     Prepares the TextureData for a call to <see cref="ITextureData.ConsumePixmap" /> or
    ///     <see cref="ITextureData.ConsumeCustomData" />. This method can be called from a non
    ///     OpenGL thread and should thus not interact with OpenGL.
    /// </summary>
    public void Prepare()
    {
    }

    /// <summary>
    ///     Returns the <see cref="Pixmap" /> for upload by Texture.
    ///     <para>
    ///         A call to <see cref="ITextureData.Prepare" /> must precede a call to this method. Any
    ///         internal data structures created in <see cref="ITextureData.Prepare" /> should be
    ///         disposed of here.
    ///     </para>
    /// </summary>
    /// <returns> the pixmap.</returns>
    public Pixmap ConsumePixmap() => throw new GdxRuntimeException( "This Texture is compressed, use the compress method." );

    /// <summary>
    ///     Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    ///     OpenGL ES texture. A call to <see cref="ITextureData.Prepare" /> must preceed
    ///     a call to this method.
    ///     <para>
    ///         Any internal data structures created in <see cref="ITextureData.Prepare" />
    ///         should be disposed of here.
    ///     </para>
    /// </summary>
    public void ConsumeCustomData( int target )
    {
        for ( var i = 0; i < _mips.Length; ++i )
        {
            GLTexture.UploadImageData( target, _mips[ i ], i );
        }
    }

    public bool                     DisposePixmap() => false;
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Custom;
    public Pixmap.Format            GetFormat()     => Pixmap.Format.Alpha;
    public bool                     IsManaged()     => false;
}
