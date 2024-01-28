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

using LibGDXSharp.Graphics.G3D;
using LibGDXSharp.Graphics.G3D.Models.Data;
using LibGDXSharp.Graphics.G3D.Utils;
using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Assets.Loaders;

[PublicAPI]
public abstract class ModelLoader<TP> : AsynchronousAssetLoader< Model, TP >
    where TP : ModelLoader< TP >.ModelLoaderParameters
{
    protected readonly ModelLoaderParameters defaultLoaderParameters = new();

    // Check this declaration.
    // Original Java was Array< ObjectMap.Entry< string, ModelData > > items...
    protected readonly List< ObjectMap< string, ModelData >.Entry > items = [ ];

    protected ModelLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    ///     Directly load the raw model data on the calling thread.
    /// </summary>
    protected virtual ModelData? LoadModelData<T>( in FileInfo fileHandle, T? parameters )
        where T : ModelLoaderParameters
    {
        return default( ModelData );
    }

    /// <summary>
    ///     Directly load the raw model data on the calling thread.
    /// </summary>
    public ModelData? LoadModelData( in FileInfo fileHandle )
    {
        return LoadModelData< ModelLoaderParameters >( fileHandle, null );
    }

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel<T>( in FileInfo fileHandle, ITextureProvider textureProvider, T? parameters )
        where T : ModelLoaderParameters
    {
        ModelData? data = LoadModelData( fileHandle, parameters );

        return data == null ? null : new Model( data, textureProvider );
    }

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel<T>( in FileInfo fileHandle, T parameters ) where T : ModelLoaderParameters
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), parameters );
    }

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle, ITextureProvider textureProvider )
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, textureProvider, null );
    }

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle )
    {
        return LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), null );
    }

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first.
    ///     This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameters">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameters )
    {
        ArgumentNullException.ThrowIfNull( file );

        List< AssetDescriptor > deps = [ ];

        ModelData? data = LoadModelData( file );

        if ( data == null )
        {
            return deps;
        }

        var item = new ObjectMap< string, ModelData >.Entry
        {
            key   = fileName,
            value = data
        };

        lock ( items )
        {
            items.Add( item );
        }

        TextureLoader.TextureLoaderParameters textureLoaderParameters = parameters != null
            ? ( ( ModelLoaderParameters )parameters ).TextureLoaderParameters
            : defaultLoaderParameters.TextureLoaderParameters;

        foreach ( ModelMaterial? modelMaterial in data.Materials! )
        {
            if ( modelMaterial.Textures != null )
            {
                foreach ( ModelTexture modelTexture in modelMaterial.Textures )
                {
                    deps.Add( new AssetDescriptor( modelTexture.FileName, typeof( Texture ), textureLoaderParameters ) );
                }
            }
        }

        return deps;
    }

    /// <summary>
    ///     Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override void Load( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    TP? parameter )
    {
        ModelData? data = null;

        lock ( items )
        {
            for ( var i = 0; i < items.Count; i++ )
            {
                if ( items[ i ].key!.Equals( fileName ) )
                {
                    data = items[ i ].value;
                    items.RemoveAt( i );
                }
            }
        }

        if ( data == null )
        {
            return;
        }

        Model result = new( data, new ITextureProvider.AssetTextureProvider( manager ) );

        // need to remove the textures from the managed disposables,
        // or ref counting won't work!
        IEnumerator< IDisposable > disposables = result.GetManagedDisposables()
                                                       .GetEnumerator();

        while ( disposables.MoveNext() )
        {
            IDisposable disposable = disposables.Current;

            if ( disposable is Texture )
            {
                disposables.Dispose();
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class ModelLoaderParameters : AssetLoaderParameters
    {
        public TextureLoader.TextureLoaderParameters TextureLoaderParameters { get; set; }

        public ModelLoaderParameters()
        {
            TextureLoaderParameters           = new TextureLoader.TextureLoaderParameters();
            TextureLoaderParameters.MinFilter = TextureLoaderParameters.MagFilter = TextureFilter.Linear;
            TextureLoaderParameters.WrapU     = TextureLoaderParameters.WrapV     = TextureWrap.Repeat;
        }
    }
}
