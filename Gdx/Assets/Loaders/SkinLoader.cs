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

public class SkinLoader : AsynchronousAssetLoader< Skin, SkinLoader.SkinLoaderParameters >
{
    public SkinLoader( IFileHandleResolver resolver ) : base( resolver )
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
        List< AssetDescriptor > deps = [ ];

        if ( ( ( SkinLoaderParameters? )parameter )?.textureAtlasPath == null )
        {
            var path = Path.ChangeExtension( file?.FullName, ".atlas" );

            deps.Add( new AssetDescriptor( path, typeof( TextureAtlas ), new SkinLoaderParameters() ) );
        }

        else if ( ( ( SkinLoaderParameters? )parameter )?.textureAtlasPath != null )
        {
            deps.Add( new AssetDescriptor( ( ( SkinLoaderParameters? )parameter )?.textureAtlasPath,
                                             typeof( TextureAtlas ),
                                             parameter ) );
        }

        return deps;
    }

    public override void Load( AssetManager manager,
                                   string? fileName,
                                   FileInfo? file,
                                   SkinLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( file );

        var textureAtlasPath = Path.ChangeExtension( file.FullName, ".atlas" );

        Dictionary< string, object >? resources = null;

        if ( parameter != null )
        {
            if ( parameter.textureAtlasPath != null )
            {
                textureAtlasPath = parameter.textureAtlasPath;
            }

            if ( parameter.resources != null )
            {
                resources = parameter.resources;
            }
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
    }

    private static Skin NewSkin( TextureAtlas atlas ) => new( atlas );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class SkinLoaderParameters : AssetLoaderParameters
    {
        public readonly Dictionary< string, object >? resources;
        public readonly string?                       textureAtlasPath;

        public SkinLoaderParameters() : this( null, null )
        {
        }

        public SkinLoaderParameters( Dictionary< string, object > resources )
            : this( null, resources )
        {
        }

        public SkinLoaderParameters( string? textureAtlasPath, Dictionary< string, object >? resources = null )
        {
            this.textureAtlasPath = textureAtlasPath;
            this.resources        = resources;
        }
    }
}
