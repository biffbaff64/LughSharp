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

using Corelib.Lugh.Assets.Loaders.Resolvers;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.Images;

namespace Corelib.Lugh.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="Cubemap"/> instances. The pixel data
/// is loaded asynchronously. The texture is then created on the rendering thread,
/// synchronously.
/// <para>
/// Passing a <see cref="CubemapParameter"/> to <see cref="AssetManager.AddToLoadqueue"/>"
/// allows one to specify parameters as can be passed to the various Cubemap
/// constructors, e.g. filtering and so on.
/// </para>
/// </summary>
[PublicAPI]
public class CubemapLoader : AsynchronousAssetLoader
{
    /// <summary>
    /// Information about the cubemap being loaded.
    /// </summary>
    private CubemapLoaderInfo _loaderInfo = new()
    {
        Filename    = "",
        Cubemap     = null!,
        CubemapData = null!,
    };

    /// <summary>
    /// Creates a new CubmapLoader using the supplied resolver.
    /// </summary>
    public CubemapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies< TP >( string fileName,
                                                                   FileInfo file,
                                                                   TP? parameter ) where TP : class
    {
        return default( List< AssetDescriptor > )!;
    }

    /// <inheritdoc />
    public override void LoadAsync< TP >( AssetManager manager,
                                          FileInfo file,
                                          TP? parameter ) where TP : class
    {
    }

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the AssetManager.
    /// </summary>
    /// <param name="manager">The AssetManager to use.</param>
    /// <param name="file">The assets FileInfo object.</param>
    /// <param name="parameter">The AssetLoader parameters object.</param>
    public override object LoadSync< TP >( AssetManager manager, FileInfo file, TP? parameter ) where TP : class
    {
        var p       = parameter as CubemapParameter;
        var cubemap = _loaderInfo.Cubemap;

        if ( cubemap != null )
        {
            cubemap.Load( _loaderInfo.CubemapData! );
        }
        else
        {
            cubemap = new Cubemap( _loaderInfo.CubemapData! );
        }

        if ( p != null )
        {
            cubemap.SetFilter( p.MinFilter, p.MagFilter );
            cubemap.SetWrap( p.WrapU, p.WrapV );
        }

        return cubemap;
    }

    // ========================================================================

    /// <summary>
    /// Contains information about the Cubemap being loaded.
    /// </summary>
    [PublicAPI]
    public struct CubemapLoaderInfo
    {
        public string?       Filename    { get; set; }
        public Cubemap?      Cubemap     { get; init; }
        public ICubemapData? CubemapData { get; init; }
    }

    // ========================================================================

    [PublicAPI]
    public class CubemapParameter : AssetLoaderParameters
    {
        /// <summary>
        /// CubemapData for textures created on the fly, optional.
        /// When set, all format and genMipMaps are ignored
        /// </summary>
        public ICubemapData? CubemapData = null;

        /// <summary>
        /// the format of the final Texture. Uses the source images format if null
        /// </summary>
        public Pixmap.ColorFormat? Format = null;

        public Texture.TextureFilter MagFilter = Texture.TextureFilter.Nearest;
        public Texture.TextureFilter MinFilter = Texture.TextureFilter.Nearest;
        public Texture.TextureWrap   WrapU     = Texture.TextureWrap.ClampToEdge;
        public Texture.TextureWrap   WrapV     = Texture.TextureWrap.ClampToEdge;

        /// <summary>
        /// The texture to put the TextureData in, optional.
        /// </summary>
        public Cubemap? Cubemap { get; set; } = null;
    }
}