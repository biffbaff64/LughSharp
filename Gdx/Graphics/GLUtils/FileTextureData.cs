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

public class FileTextureData : ITextureData
{
    private Pixmap.Format? _format;
    private int            _height     = 0;
    private bool           _isPrepared = false;
    private Pixmap?        _pixmap;
    private int            _width = 0;

    public FileTextureData( FileInfo file, Pixmap preloadedPixmap, Pixmap.Format? format, bool useMipMaps )
    {
        File       = file;
        _pixmap    = preloadedPixmap;
        _format    = format;
        UseMipMaps = useMipMaps;

        if ( _pixmap != null )
        {
            _width  = _pixmap.Width;
            _height = _pixmap.Height;

            _format ??= _pixmap.GetFormat();
        }
    }

    public FileInfo? File { get; set; }

    /// <summary>
    ///     Prepares the TextureData for a call to <see cref="ITextureData.ConsumePixmap" /> or
    ///     <see cref="ITextureData.ConsumeCustomData" />. This method can be called from a non
    ///     OpenGL thread and should thus not interact with OpenGL.
    /// </summary>
    public void Prepare()
    {
        if ( IsPrepared )
        {
            throw new GdxRuntimeException( "Already prepared" );
        }

        if ( _pixmap == null )
        {
            if ( ( bool )File?.Extension.Equals( "cim" ) )
            {
                _pixmap = PixmapIO.ReadCIM( File );
            }
            else
            {
                _pixmap = new Pixmap( File );
            }

            _width  = _pixmap.Width;
            _height = _pixmap.Height;

            _format ??= _pixmap.GetFormat();
        }

        IsPrepared = true;
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
    public Pixmap? ConsumePixmap()
    {
        if ( !IsPrepared )
        {
            throw new GdxRuntimeException( "Call prepare() before calling ConsumePixmap()" );
        }

        IsPrepared = false;

        Pixmap? pixmap = _pixmap;
        _pixmap = null;

        return pixmap;
    }

    /// <returns>
    ///     whether the caller of <see cref="ITextureData.ConsumePixmap" /> should dispose the
    ///     Pixmap returned by <see cref="ITextureData.ConsumePixmap" />
    /// </returns>
    public bool DisposePixmap() => true;

    /// <summary>
    ///     Uploads the pixel data to the OpenGL ES texture. The caller must bind an
    ///     OpenGL ES texture. A call to <see cref="ITextureData.Prepare" /> must preceed a call
    ///     to this method.
    ///     <para>
    ///         Any internal data structures created in <see cref="ITextureData.Prepare" /> should be
    ///         disposed of here.
    ///     </para>
    /// </summary>
    public void ConsumeCustomData( int target ) => throw new GdxRuntimeException
        ( "This TextureData implementation does not upload data itself" );

    /// <returns> the width of the pixel data </returns>
    public int Width { get; set; }

    /// <returns> the height of the pixel data </returns>
    public int Height { get; set; }

    /// <returns> whether the TextureData is prepared or not.</returns>
    public bool IsPrepared { get; set; }

    /// <returns> whether to generate mipmaps or not. </returns>
    public bool UseMipMaps { get; set; }

    /// <returns> the <see cref="Pixmap.Format" /> of the pixel data </returns>
    public Pixmap.Format GetFormat() => Pixmap.Format.Alpha;

    /// <returns> whether this implementation can cope with a EGL context loss. </returns>
    public bool IsManaged() => true;

    /// <returns> the <see cref="ITextureData.TextureDataType" /></returns>
    public ITextureData.TextureType TextureDataType => ITextureData.TextureType.Pixmap;
}
