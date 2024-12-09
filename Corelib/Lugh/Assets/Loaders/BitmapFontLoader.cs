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
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Scenes.Scene2D.UI;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Assets.Loaders;

// ========================================================================
// ========================================================================

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="BitmapFont"/> instances. Loads the font
/// description file (.fnt) asynchronously, loads the <see cref="Texture"/> containing
/// the glyphs as a dependency. The <see cref="BitmapFontParameter"/> allows you to
/// set things like texture filters or whether to flip the glyphs vertically.
/// </summary>
[PublicAPI]
public class BitmapFontLoader : AsynchronousAssetLoader, IDisposable
{
    private BitmapFont.BitmapFontData? _data;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Creates a new BitmapFontLoader using the specified <see cref="IFileHandleResolver"/>
    /// </summary>
    public BitmapFontLoader( IFileHandleResolver resolver )
        : base( resolver )
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
        ArgumentNullException.ThrowIfNull( file );

        var p    = parameter as BitmapFontParameter;
        var deps = new List< AssetDescriptor >();

        if ( p?.BitmapFontData != null )
        {
            _data = p.BitmapFontData;

            return deps;
        }

        _data = new BitmapFont.BitmapFontData( file, p!.Flip );

        if ( p.AtlasName != null )
        {
            deps.Add( new AssetDescriptor( p.AtlasName, typeof( TextureAtlas ), parameter ) );
        }
        else
        {
            for ( var i = 0; i < _data.ImagePaths?.Length; i++ )
            {
                var path          = _data.ImagePaths[ i ];
                var resolved      = Resolve( path );
                var textureParams = new TextureLoader.TextureLoaderParameters();

                if ( parameter != null )
                {
                    textureParams.GenMipMaps = p.GenMipMaps;
                    textureParams.MinFilter  = p.MinFilter;
                    textureParams.MagFilter  = p.MagFilter;
                }

                deps.Add( new AssetDescriptor( resolved, typeof( Texture ), textureParams ) );
            }
        }

        return deps;
    }

    /// <inheritdoc />
    public override void LoadAsync< TP >( AssetManager manager, FileInfo file, TP? parameter ) where TP : class
    {
    }

    /// <inheritdoc />
    public override object LoadSync< TP >( AssetManager manager, FileInfo file, TP? parameter ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( file );

        var p = parameter as BitmapFontParameter;

        if ( p?.AtlasName != null )
        {
            var atlas = manager.Get( p.AtlasName! ) as TextureAtlas;
            var name  = Path.GetFileNameWithoutExtension( _data?.ImagePaths?[ 0 ] );

            TextureRegion? region = atlas?.FindRegion( name );

            if ( region == null )
            {
                throw new GdxRuntimeException( $"Could not find font region {name} in atlas {p.AtlasName}" );
            }

            return new BitmapFont( file, region );
        }

        var capacity = ( int )_data?.ImagePaths?.Length!;
        var regs     = new List< TextureRegion >( capacity );

        for ( var i = 0; i < capacity; i++ )
        {
            var texture = manager.Get( _data.ImagePaths[ i ] ) as Texture;
            regs.Add( new TextureRegion( texture! ) );
        }

        return new BitmapFont( _data, regs, true );
    }

    #region dispose pattern

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
    }

    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _data = null;
        }
    }

    #endregion dispose pattern
}

[PublicAPI]
public class BitmapFontParameter : AssetLoaderParameters
{
    /// <summary>
    /// Flips the font vertically if <tt>true</tt>. Defaults to <tt>false</tt>.
    /// </summary>
    public bool Flip { get; set; } = false;

    /// <summary>
    /// Generates mipmaps for the font if <tt>true</tt>. Defaults to <tt>false</tt>.
    /// </summary>
    public bool GenMipMaps { get; set; } = false;

    /// <summary>
    /// The <see cref="Texture.TextureFilter"/> to use when scaling down the <see cref="BitmapFont"/>.
    /// Defaults to <see cref="Texture.TextureFilter.Nearest"/>.
    /// </summary>
    public Texture.TextureFilter MinFilter { get; set; } = Texture.TextureFilter.Nearest;

    /// <summary>
    /// The <see cref="Texture.TextureFilter"/> to use when scaling up the <see cref="BitmapFont"/>.
    /// Defaults to <see cref="Texture.TextureFilter.Nearest"/>.
    /// </summary>
    public Texture.TextureFilter MagFilter { get; set; } = Texture.TextureFilter.Nearest;

    /// <summary>
    /// optional <see cref="BitmapFont.BitmapFontData"/> to be used instead of
    /// loading the <see cref="Texture"/> directly. Use this if your font is
    /// embedded in a <see cref="Skin"/>.
    /// </summary>
    public BitmapFont.BitmapFontData? BitmapFontData { get; set; } = null;

    /// <summary>
    /// The name of the <see cref="TextureAtlas"/> to load the <see cref="BitmapFont"/>.
    /// if null, will look for a separate image.
    /// </summary>
    public string? AtlasName { get; set; }
}