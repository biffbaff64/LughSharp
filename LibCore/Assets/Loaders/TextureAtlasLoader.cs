// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Assets.Loaders;

/// <summary>
/// AssetLoader to load TextureAtlas instances. Passing a <see cref="TextureAtlasParameter"/> to
/// <see cref="AssetManager.Load(String, Type, AssetLoaderParameters)"/> allows to specify whether
/// the atlas regions should be flipped on the y-axis or not.
/// </summary>
[PublicAPI]
public class TextureAtlasLoader
    : SynchronousAssetLoader< TextureAtlas, TextureAtlasLoader.TextureAtlasParameter >, IDisposable
{
    private TextureAtlasData? _data;

    /// <summary>
    /// Creates a new TextureAtlasLoader using the supplied resolver.
    /// </summary>
    public TextureAtlasLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
        Logger.CheckPoint();
    }

    /// <summary>
    /// Loads the texture atlas using the provided asset manager, file, and parameters.
    /// </summary>
    /// <param name="assetManager">The asset manager responsible for loading the assets.</param>
    /// <param name="file">The file information of the texture atlas.</param>
    /// <param name="parameter">The parameters for loading the texture atlas.</param>
    /// <returns>The loaded texture atlas.</returns>
    /// <exception cref="GdxRuntimeException">Thrown if the texture atlas data is null.</exception>
    public override TextureAtlas Load( AssetManager assetManager, FileInfo? file, TextureAtlasParameter? parameter )
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

        var atlas = new TextureAtlas( _data );

        _data = null;

        return atlas;
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
                    : new TextureAtlasData( atlasFile, imgDir );

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
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    /// Releases the unmanaged resources used by the texture loader.
    /// </summary>
    /// <param name="disposing">
    /// True to release both managed and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _data = null;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Parameters for loading a <see cref="TextureAtlas"/>
    /// </summary>
    [PublicAPI]
    public class TextureAtlasParameter( bool flip ) : AssetLoaderParameters
    {
        public TextureAtlasParameter() : this( false )
        {
        }

        public bool FlipVertically { get; } = flip;
    }
}
