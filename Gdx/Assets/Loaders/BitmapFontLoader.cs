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

using LibGDXSharp.Graphics.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;

namespace LibGDXSharp.Assets.Loaders;

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

/// <summary>
/// </summary>
public class BitmapFontLoader : AsynchronousAssetLoader< BitmapFont, BitmapFontParameter >, IDisposable
{
    private BitmapFont.BitmapFontData? _data;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    /// <param name="resolver"></param>
    public BitmapFontLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first.
    ///     This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( file );

        var deps = new List< AssetDescriptor >();

        if ( ( ( BitmapFontParameter? )parameter )?.BitmapFontData != null )
        {
            _data = ( ( BitmapFontParameter? )parameter )?.BitmapFontData;

            return deps;
        }

        _data = new BitmapFont.BitmapFontData( file, ( ( BitmapFontParameter? )parameter )!.Flip );

        if ( ( ( BitmapFontParameter? )parameter )?.AtlasName != null )
        {
            deps.Add( new AssetDescriptor( ( ( BitmapFontParameter? )parameter )?.AtlasName,
                                           typeof( TextureAtlas ),
                                           parameter ) );
        }
        else
        {
            for ( var i = 0; i < _data.ImagePaths?.Length; i++ )
            {
                var path = _data.ImagePaths[ i ];

                FileInfo resolved = Resolve( path );

                var textureParams = new TextureLoader.TextureLoaderParameters();

                if ( parameter != null )
                {
                    textureParams.GenMipMaps = ( ( BitmapFontParameter? )parameter )!.GenMipMaps;
                    textureParams.MinFilter  = ( ( BitmapFontParameter? )parameter )!.MinFilter;
                    textureParams.MagFilter  = ( ( BitmapFontParameter? )parameter )!.MagFilter;
                }

                deps.Add( new AssetDescriptor( resolved, typeof( Texture ), textureParams ) );
            }
        }

        return deps;
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public override BitmapFont LoadSync( AssetManager manager,
                                         string? fileName,
                                         FileInfo? file,
                                         BitmapFontParameter? parameter )
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( file );

        if ( parameter?.AtlasName != null )
        {
            var atlas = manager.Get< TextureAtlas >( parameter.AtlasName! );
            var name  = Path.GetFileNameWithoutExtension( _data?.ImagePaths?[ 0 ] );

            TextureRegion? region = atlas.FindRegion( name );

            if ( region == null )
            {
                throw new GdxRuntimeException( $"Could not find font region {name} in atlas "
                                             + $"{parameter.AtlasName}" );
            }

            return new BitmapFont( file, region );
        }

        var n    = ( int )_data?.ImagePaths?.Length!;
        var regs = new List< TextureRegion >( capacity: n );

        for ( var i = 0; i < n; i++ )
        {
            regs.Add( new TextureRegion( manager.Get< Texture >( _data.ImagePaths[ i ] ) ) );
        }

        return new BitmapFont( _data, regs, true );
    }

    // ------------------------------------------------------------------------
    // Dispose pattern
    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _data = null;
        }
    }
}

[PublicAPI]
public class BitmapFontParameter : AssetLoaderParameters
{
    /// <summary>
    ///     Flips the font vertically if <tt>true</tt>. Defaults to <tt>false</tt>.
    /// </summary>
    public bool Flip { get; set; } = false;

    /// <summary>
    ///     Generates mipmaps for the font if <tt>true</tt>. Defaults to <tt>false</tt>.
    /// </summary>
    public bool GenMipMaps { get; set; } = false;

    /// <summary>
    ///     The <see cref="TextureFilter" /> to use when scaling down the <see cref="BitmapFont" />.
    ///     Defaults to <see cref="TextureFilter.Nearest" />.
    /// </summary>
    public TextureFilter MinFilter { get; set; } = TextureFilter.Nearest;

    /// <summary>
    ///     The <see cref="TextureFilter" /> to use when scaling up the <see cref="BitmapFont" />.
    ///     Defaults to <see cref="TextureFilter.Nearest" />.
    /// </summary>
    public TextureFilter MagFilter { get; set; } = TextureFilter.Nearest;

    /// <summary>
    ///     optional <see cref="BitmapFont.BitmapFontData" /> to be used instead of
    ///     loading the <see cref="Texture" /> directly. Use this if your font is
    ///     embedded in a <see cref="Skin" />.
    /// </summary>
    public BitmapFont.BitmapFontData? BitmapFontData { get; set; } = null;

    /// <summary>
    ///     The name of the <see cref="TextureAtlas" /> to load the <see cref="BitmapFont" />.
    ///     if null, will look for a separate image.
    /// </summary>
    public string? AtlasName { get; set; }
}
