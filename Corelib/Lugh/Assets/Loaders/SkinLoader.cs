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

using Corelib.Lugh.Assets.Loaders.Resolvers;
using Corelib.Lugh.Graphics.G2D;
using Corelib.Lugh.Scenes.Scene2D.UI;

namespace Corelib.Lugh.Assets.Loaders;

[PublicAPI]
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
    public override List< AssetDescriptor > GetDependencies< TP >( string fileName,
                                                                   FileInfo file,
                                                                   TP? parameter ) where TP : class
    {
        var p = parameter as SkinLoaderParameters;

        List< AssetDescriptor > deps = [ ];

        if ( p?.TextureAtlasPath == null )
        {
            var path = Path.ChangeExtension( file.FullName, ".atlas" );

            deps.Add( new AssetDescriptor( path, typeof( TextureAtlas ), new SkinLoaderParameters() ) );
        }
        else if ( p.TextureAtlasPath != null )
        {
            deps.Add( new AssetDescriptor( p.TextureAtlasPath, typeof( TextureAtlas ), parameter ) );
        }

        return deps;
    }

    /// <inheritdoc />
    public override void LoadAsync< TP >( AssetManager manager, FileInfo file, TP? parameter ) where TP : class
    {
    }

    /// <inheritdoc />
    public override object LoadSync< TP >( AssetManager manager,
                                           FileInfo file,
                                           TP? parameter ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( manager );
        ArgumentNullException.ThrowIfNull( file );

        var p = parameter as SkinLoaderParameters;
        
        var textureAtlasPath = Path.ChangeExtension( file.FullName, ".atlas" );

        Dictionary< string, object >? resources = null;

        if ( p != null )
        {
            if ( p.TextureAtlasPath != null )
            {
                textureAtlasPath = p.TextureAtlasPath;
            }

            if ( p.Resources != null )
            {
                resources = p.Resources;
            }
        }

        var atlas = manager.Get( textureAtlasPath! ) as TextureAtlas;
        var skin  = NewSkin( atlas );

        if ( resources != null )
        {
            foreach ( var entry in resources )
            {
                skin.Add( entry.Key, entry.Value );
            }
        }

        skin.Load( file );

        return skin;
    }

    /// <summary>
    /// Override to allow subclasses of Skin to be loaded or the skin instance to be configured.
    /// </summary>
    /// <param name="atlas"> The TextureAtlas that the skin will use. </param>
    /// <returns>
    /// A new Skin (or subclass of Skin) instance based on the provided TextureAtlas.
    /// </returns>
    protected virtual Skin NewSkin( TextureAtlas? atlas )
    {
        ArgumentNullException.ThrowIfNull( atlas );

        return new Skin( atlas );
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Parameters for loading skin assets.
    /// </summary>
    [PublicAPI]
    public class SkinLoaderParameters : AssetLoaderParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoaderParameters"/>
        /// class with default values.
        /// </summary>
        public SkinLoaderParameters() : this( null, null )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoaderParameters"/>
        /// class with the specified resources.
        /// </summary>
        /// <param name="resources">The resources to be used by the skin.</param>
        public SkinLoaderParameters( Dictionary< string, object > resources ) : this( null, resources )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkinLoaderParameters"/> class
        /// with the specified texture atlas path and resources.
        /// </summary>
        /// <param name="textureAtlasPath">The file path of the texture atlas to be used by the skin.</param>
        /// <param name="resources">The resources to be used by the skin.</param>
        public SkinLoaderParameters( string? textureAtlasPath, Dictionary< string, object >? resources = null )
        {
            TextureAtlasPath = textureAtlasPath;
            Resources        = resources;
        }

        /// <summary>
        /// Gets or sets the resources to be used by the skin.
        /// </summary>
        public Dictionary< string, object >? Resources { get; set; }

        /// <summary>
        /// Gets or sets the file path of the texture atlas to be used by the skin.
        /// </summary>
        public string? TextureAtlasPath { get; set; }
    }
}