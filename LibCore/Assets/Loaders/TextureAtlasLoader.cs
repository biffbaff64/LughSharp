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
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Assets.Loaders;

/// <summary>
///     AssetLoader to load TextureAtlas instances. Passing a <see cref="TextureAtlasParameter" /> to
///     <see cref="AssetManager.Load(String, Type, AssetLoaderParameters)" /> allows to specify whether
///     the atlas regions should be flipped on the y-axis or not.
/// </summary>
[PublicAPI]
public class TextureAtlasLoader : AsynchronousAssetLoader< TextureAtlas, TextureAtlasLoader.TextureAtlasParameter >,
                                  IDisposable
{
    private TextureAtlasData? _data;

    public TextureAtlasLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

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
    /// <param name="assetManager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void Load( AssetManager assetManager,
                               string? fileName,
                               FileInfo? file,
                               TextureAtlasParameter? parameter )
    {
        if ( _data == null )
        {
            throw new GdxRuntimeException( "TextureAtlasData cannot be null!" );
        }

        foreach ( var page in _data.Pages )
        {
            if ( page.TextureFile != null )
            {
                var name = page.TextureFile.FullName.Replace( "\\\\", "/" );

                var texture = assetManager.Get< Texture >( name );

                page.Texture = texture;
            }
        }

        //        var atlas = new TextureAtlas( _data );

        _data = null;
    }

    /// <inheritdoc />
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? atlasFile,
                                                             AssetLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( atlasFile );

        var imgDir = atlasFile.Directory;

        _data = parameter != null
                    ? new TextureAtlasData( atlasFile, imgDir, ( ( TextureAtlasParameter ) parameter ).FlipVertically )
                    : new TextureAtlasData( atlasFile, imgDir, false );

        var dependencies = new List< AssetDescriptor >();

        foreach ( var page in _data.Pages )
        {
            var tparams = new TextureLoader.TextureLoaderParameters
            {
                Format     = page.Format,
                GenMipMaps = page.UseMipMaps,
                MinFilter  = page.MinFilter,
                MagFilter  = page.MagFilter
            };

            if ( page.TextureFile != null )
            {
                dependencies.Add( new AssetDescriptor( page.TextureFile, typeof( Texture ), tparams ) );
            }
        }

        return dependencies;
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _data = null;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class TextureAtlasParameter : AssetLoaderParameters
    {
        public TextureAtlasParameter()
        {
            FlipVertically = false;
        }

        public TextureAtlasParameter( bool flip )
        {
            FlipVertically = flip;
        }

        public bool FlipVertically { get; }
    }
}