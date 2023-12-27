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

namespace LibGDXSharp.Assets.Loaders;

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

    public TextureAtlasLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="assetManager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override TextureAtlas Load( AssetManager assetManager,
                                       string? fileName,
                                       FileInfo? file,
                                       TextureAtlasParameter parameter )
    {
        if ( _data == null )
        {
            throw new GdxRuntimeException( "TextureAtlasData cannot be null!" );
        }
        
        foreach ( TextureAtlasData.Page page in _data.Pages )
        {
            if ( page.textureFile != null )
            {
                var name = page.textureFile.FullName.Replace( "\\\\", "/" );

                var texture = assetManager.Get< Texture >( name );

                page.texture = texture;
            }
        }

        var atlas = new TextureAtlas( _data );
        _data = null;

        return atlas;
    }

    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? atlasFile,
                                                             AssetLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( atlasFile );

        DirectoryInfo? imgDir = atlasFile.Directory;

        _data = ( parameter != null )
            ? new TextureAtlasData( atlasFile, imgDir, ( ( TextureAtlasParameter )parameter ).FlipVertically )
            : new TextureAtlasData( atlasFile, imgDir, false );

        var dependencies = new List< AssetDescriptor >();

        foreach ( TextureAtlasData.Page page in _data.Pages )
        {
            var tparams = new TextureLoader.TextureParameter
            {
                Format     = page.Format,
                GenMipMaps = page.UseMipMaps,
                MinFilter  = page.MinFilter,
                MagFilter  = page.MagFilter
            };

            if ( page.textureFile != null )
            {
                dependencies.Add( new AssetDescriptor( page.textureFile, typeof( Texture ), tparams ) );
            }
        }

        return dependencies;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    [PublicAPI]
    public class TextureAtlasParameter : AssetLoaderParameters
    {
        public bool FlipVertically { get; private set; }

        public TextureAtlasParameter()
        {
            this.FlipVertically = false;
        }

        public TextureAtlasParameter( bool flip )
        {
            this.FlipVertically = flip;
        }
    }
}