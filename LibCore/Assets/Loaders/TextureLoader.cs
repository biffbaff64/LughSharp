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


using LughSharp.LibCore.Assets.Loaders.Resolvers;

namespace LughSharp.LibCore.Assets.Loaders;

/// <summary>
///     <see cref="AssetLoaderBase" /> for <see cref="Texture" /> instances.
///     The pixel data is loaded asynchronously. The texture is then created on the
///     rendering thread, synchronously. Passing a <see cref="TextureLoaderParameters" />
///     to <see cref="AssetManager" />.Load() allows one to specify parameters as
///     can be passed to the various Texture constructors, e.g. filtering, whether
///     to generate mipmaps and so on.
/// </summary>
[PublicAPI]
public class TextureLoader : AsynchronousAssetLoader< Texture, TextureLoader.TextureLoaderParameters >,
                             IDisposable
{
    private TextureLoaderInfo _loaderInfo;

    public TextureLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        _loaderInfo = new TextureLoaderInfo();
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? file,
                               TextureLoaderParameters? parameter )
    {
        _loaderInfo.Filename = fileName;

        if ( parameter?.TextureData == null )
        {
            Pixmap.Format? format     = null;
            var            genMipMaps = false;

            _loaderInfo.Texture = null;

            if ( parameter != null )
            {
                format              = parameter.Format;
                genMipMaps          = parameter.GenMipMaps;
                _loaderInfo.Texture = parameter.Texture;
            }

            _loaderInfo.Data = ITextureData.Factory.LoadFromFile( file, format, genMipMaps );
        }
        else
        {
            _loaderInfo.Data    = parameter.TextureData;
            _loaderInfo.Texture = parameter.Texture;
        }

        if ( _loaderInfo.Data is { IsPrepared: false } )
        {
            _loaderInfo.Data.Prepare();
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public class TextureLoaderInfo
    {
        public string?       Filename { get; set; }
        public ITextureData? Data     { get; set; }
        public Texture?      Texture  { get; set; }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    [PublicAPI]
    public class TextureLoaderParameters : AssetLoaderParameters
    {
        /// <summary>
        ///     the format of the final Texture. Uses the source images format if null
        /// </summary>
        public Pixmap.Format? Format { get; set; } = null;

        /// <summary>
        ///     whether to generate mipmaps
        /// </summary>
        public bool GenMipMaps { get; set; } = false;

        /// <summary>
        ///     The texture to put the <see cref="TextureData" /> in, optional.
        /// </summary>
        public Texture? Texture { get; set; } = null;

        /// <summary>
        ///     TextureData for textures created on the fly, optional. When set, all format and genMipMaps are ignored
        /// </summary>
        public ITextureData? TextureData { get; set; } = null;

        public TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;
        public TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;
        public TextureWrap   WrapU     { get; set; } = TextureWrap.ClampToEdge;
        public TextureWrap   WrapV     { get; set; } = TextureWrap.ClampToEdge;
    }

    #region dispose pattern

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _loaderInfo = null!;
        }
    }

    #endregion dispose pattern
}
