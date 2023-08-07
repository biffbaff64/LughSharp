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

using LibGDXSharp.G2D;
using LibGDXSharp.Scenes.Scene2D.UI;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Assets.Loaders;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class SkinLoader : AsynchronousAssetLoader
{
    public SkinLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameter )
    {
        List< AssetDescriptor > deps = new();

        if ( ( parameter == null ) || ( ( ( SkinParameter )parameter ).textureAtlasPath == null ) )
        {
            var path = Path.ChangeExtension( file?.FullName, ".atlas" );

            deps.Add( new AssetDescriptor( path, typeof( TextureAtlas ), new SkinParameter() ) );
        }
        else if ( ( ( SkinParameter )parameter ).textureAtlasPath != null )
        {
            deps.Add
                (
                 new AssetDescriptor
                     (
                      ( ( SkinParameter )parameter ).textureAtlasPath,
                      typeof( TextureAtlas ),
                      parameter
                     )
                );
        }

        return deps;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
    }

    public override object LoadSync( AssetManager? manager,
                                     string? fileName,
                                     FileInfo? file,
                                     AssetLoaderParameters? parameter )
    {
        var textureAtlasPath = Path.ChangeExtension( file?.FullName, ".atlas" );

        Dictionary< string, object >? resources = null;

        if ( parameter != null )
        {
            if ( ( ( SkinParameter )parameter ).textureAtlasPath != null )
            {
                textureAtlasPath = ( ( SkinParameter )parameter ).textureAtlasPath;
            }

            if ( ( ( SkinParameter )parameter! ).resources != null )
            {
                resources = ( ( SkinParameter )parameter ).resources;
            }
        }

        if ( manager == null )
        {
            throw new GdxRuntimeException( "manager cannot be NULL!" );
        }

        var  atlas = manager.Get< TextureAtlas >( textureAtlasPath! );
        Skin skin  = NewSkin( atlas );

        if ( resources != null )
        {
            foreach ( KeyValuePair< string, object > entry in resources )
            {
                skin.Add( entry.Key, entry.Value );
            }
        }

        skin.Load( file );

        return skin;
    }

    private Skin NewSkin( TextureAtlas atlas )
    {
        return new Skin( atlas );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public class SkinParameter : AssetLoaderParameters
    {
        public readonly string?                       textureAtlasPath;
        public readonly Dictionary< string, object >? resources;

        public SkinParameter() : this( null, null )
        {
        }

        public SkinParameter( Dictionary< string, object > resources )
            : this( null, resources )
        {
        }

        public SkinParameter( string? textureAtlasPath, Dictionary< string, object >? resources = null )
        {
            this.textureAtlasPath = textureAtlasPath;
            this.resources        = resources;
        }
    }
}