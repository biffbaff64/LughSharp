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


using LibGDXSharp.LibCore.Assets.Loaders.Resolvers;
using LibGDXSharp.LibCore.Graphics.G2D;
using LibGDXSharp.LibCore.Scenes.Scene2D.UI;

namespace LibGDXSharp.LibCore.Assets.Loaders;

[PublicAPI]
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
        List< AssetDescriptor > deps = new();

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

    private static Skin NewSkin( TextureAtlas atlas )
    {
        return new Skin( atlas );
    }

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
