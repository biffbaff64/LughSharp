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
using LughSharp.LibCore.Scenes.Scene2D.UI;

namespace LughSharp.LibCore.Assets.Loaders;

// ------------------------------------------------------------------------
// ------------------------------------------------------------------------

/// <summary>
/// </summary>
[PublicAPI]
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

    // ------------------------------------------------------------------------
    // Dispose pattern
    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
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

        if ( ( ( BitmapFontParameter? ) parameter )?.BitmapFontData != null )
        {
            _data = ( ( BitmapFontParameter? ) parameter )?.BitmapFontData;

            return deps;
        }

        _data = new BitmapFont.BitmapFontData( file, ( ( BitmapFontParameter? ) parameter )!.Flip );

        if ( ( ( BitmapFontParameter? ) parameter )?.AtlasName != null )
        {
            deps.Add( new AssetDescriptor( ( ( BitmapFontParameter? ) parameter )?.AtlasName,
                                           typeof( TextureAtlas ),
                                           parameter ) );
        }
        else
        {
            for ( var i = 0; i < _data.ImagePaths?.Length; i++ )
            {
                var path = _data.ImagePaths[ i ];

                var resolved = Resolve( path );

                var textureParams = new TextureLoader.TextureLoaderParameters();

                if ( parameter != null )
                {
                    textureParams.GenMipMaps = ( ( BitmapFontParameter? ) parameter )!.GenMipMaps;
                    textureParams.MinFilter  = ( ( BitmapFontParameter? ) parameter )!.MinFilter;
                    textureParams.MagFilter  = ( ( BitmapFontParameter? ) parameter )!.MagFilter;
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
    public override void Load( AssetManager manager,
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

//            return new BitmapFont( file, region );
        }

//        var n    = ( int )_data?.ImagePaths?.Length!;
//        var regs = new List< TextureRegion >( capacity: n );

//        for ( var i = 0; i < n; i++ )
//        {
//            regs.Add( new TextureRegion( manager.Get< Texture >( _data.ImagePaths[ i ] ) ) );
//        }

//        return new BitmapFont( _data, regs, true );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing ) _data = null;
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