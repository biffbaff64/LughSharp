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

using Corelib.LibCore.Assets.Loaders.Resolvers;
using Corelib.LibCore.Graphics;
using Corelib.LibCore.Graphics.G3D;
using Corelib.LibCore.Graphics.G3D.Models.Data;
using Corelib.LibCore.Graphics.G3D.Utils;
using Corelib.LibCore.Utils.Collections;

namespace Corelib.LibCore.Assets.Loaders;

/// <summary>
/// Abstract base class for asynchronous model loaders.
/// </summary>
[PublicAPI]
public abstract class ModelLoader : AsynchronousAssetLoader
{
    protected readonly ModelLoaderParameters                        DefaultLoaderParameters = new();
    protected readonly List< ObjectMap< string, ModelData >.Entry > Items                   = [ ];

    /// <summary>
    /// Initializes a new instance of the <see cref="ModelLoader"/> class
    /// with the specified file resolver.
    /// </summary>
    /// <param name="resolver">The file resolver to use for resolving model file paths.</param>
    protected ModelLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    /// Directly load the raw model data on the calling thread.
    /// </summary>
    protected virtual ModelData? LoadModelData< T >( in FileInfo fileHandle, T? parameters )
        where T : ModelLoaderParameters
    {
        return default( ModelData );
    }

    /// <summary>
    /// Directly load the raw model data on the calling thread.
    /// </summary>
    public ModelData? LoadModelData( in FileInfo fileHandle )
    {
        return LoadModelData< ModelLoaderParameters >( fileHandle, null );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model? LoadModel< T >( in FileInfo fileHandle, ITextureProvider textureProvider, T? parameters )
        where T : ModelLoaderParameters
    {
        var data = LoadModelData( fileHandle, parameters );

        return data == null ? null : new Model( data, textureProvider );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model? LoadModel< T >( in FileInfo fileHandle, T parameters ) where T : ModelLoaderParameters
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), parameters );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle, ITextureProvider textureProvider )
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, textureProvider, null );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle )
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), null );
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameters">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies< TP >( string fileName,
                                                                   FileInfo file,
                                                                   TP? parameters ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( file );

        var p = parameters as ModelLoaderParameters;

        List< AssetDescriptor > deps = [ ];

        var data = LoadModelData( file );

        if ( data == null )
        {
            return deps;
        }

        var item = new ObjectMap< string, ModelData >.Entry
        {
            Key   = fileName,
            Value = data,
        };

        lock ( Items )
        {
            Items.Add( item );
        }

        var textureLoaderParameters = p != null
            ? p.TextureLoaderParameters
            : DefaultLoaderParameters.TextureLoaderParameters;

        foreach ( var modelMaterial in data.Materials! )
        {
            if ( modelMaterial.Textures != null )
            {
                foreach ( var modelTexture in modelMaterial.Textures )
                {
                    deps.Add( new AssetDescriptor( modelTexture.FileName, typeof( Texture ), textureLoaderParameters ) );
                }
            }
        }

        return deps;
    }

    /// <inheritdoc />
    public override Model LoadSync< TP >( AssetManager manager, FileInfo file, TP? parameter ) where TP : class
    {
        ModelData? data = null;

        lock ( Items )
        {
            for ( var i = 0; i < Items.Count; i++ )
            {
                if ( Items[ i ].Key!.Equals( file?.Name ) )
                {
                    data = Items[ i ].Value;
                    Items.RemoveAt( i );
                }
            }
        }

        if ( data == null )
        {
            return null!;
        }

        Model result = new( data, new ITextureProvider.AssetTextureProvider( manager ) );

        // need to remove the textures from the managed disposables,
        // or ref counting won't work!
        var disposables = result.GetManagedDisposables().GetEnumerator();

        while ( disposables.MoveNext() )
        {
            var disposable = disposables.Current;

            if ( disposable is Texture )
            {
                disposables.Dispose();
            }
        }

        return result;
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Parameters for loading models.
    /// </summary>
    [PublicAPI]
    public class ModelLoaderParameters : AssetLoaderParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelLoaderParameters"/>
        /// class with default values.
        /// </summary>
        public ModelLoaderParameters()
        {
            TextureLoaderParameters           = new TextureLoader.TextureLoaderParameters();
            TextureLoaderParameters.MinFilter = TextureLoaderParameters.MagFilter = Texture.TextureFilter.Linear;
            TextureLoaderParameters.WrapU     = TextureLoaderParameters.WrapV     = Texture.TextureWrap.Repeat;
        }

        /// <summary>
        /// Gets or sets the texture loader parameters for loading model textures.
        /// </summary>
        public TextureLoader.TextureLoaderParameters TextureLoaderParameters { get; set; }
    }
}