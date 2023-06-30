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

using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Core.Utils.Collections.Extensions;
using LibGDXSharp.Graphics.G3D;
using LibGDXSharp.Graphics.G3D.Models.Data;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Assets.Loaders;

public abstract class ModelLoader : AsynchronousAssetLoader< Model, ModelLoader.ModelParameters >
{
    protected ModelLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    // Check this declaration.
    // Original Java was Array< ObjectMap.Entry< string, ModelData > > items...
    protected readonly List< ObjectMap< string, ModelData >.Entry< string, ModelData > > items = new();

    protected ModelParameters defaultParameters = new();

    /// <summary>
    /// Directly load the raw model data on the calling thread.
    /// </summary>
    protected abstract ModelData LoadModelData<TP>( in FileInfo fileHandle, TP? parameters ) where TP : ModelParameters;

    /// <summary>
    /// Directly load the raw model data on the calling thread.
    /// </summary>
    public ModelData LoadModelData( in FileInfo fileHandle )
    {
        return LoadModelData< ModelParameters >( fileHandle, null );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model LoadModel( in FileInfo fileHandle, TextureProvider textureProvider, TP parameters )
    {
        ModelData? data = LoadModelData( fileHandle, parameters );

        return data == null ? null : new Model( data, textureProvider );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model LoadModel( in FileInfo fileHandle, TP parameters )
    {
        return LoadModel( fileHandle, new TextureProvider.FileTextureProvider(), parameters );
    }

    /// <summary>
    /// Directly load the model on the calling thread. The model with not be managed by an <see cref="AssetManager"/>. </summary>
    public Model LoadModel( in FileInfo fileHandle, TextureProvider textureProvider )
    {
        return LoadModel( fileHandle, textureProvider, null );
    }

    /// <summary>
    /// Directly load the model on the calling thread.
    /// The model with not be managed by an <see cref="AssetManager"/>.
    /// </summary>
    public Model LoadModel( in FileInfo fileHandle )
    {
        return LoadModel( fileHandle, new TextureProvider.FileTextureProvider(), null );
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
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the AssetManager.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
    }

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override Model LoadSync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
        ModelData? data = null;

        lock ( items )
        {
            for ( int i = 0; i < items.Count; i++ )
            {
                if ( items[ i ].key!.Equals( fileName ) )
                {
                    data = items[ i ].value;
                    items.RemoveAt( i );
                }
            }
        }

        if ( data == null ) return null!;

        Model result = new( data, new TextureProvider.AssetTextureProvider( manager ) );

        // need to remove the textures from the managed disposables,
        // or ref counting won't work!
        IEnumerator< IDisposable > disposables = result.GetManagedDisposables().GetEnumerator();

        while ( disposables.MoveNext() )
        {
            IDisposable disposable = disposables.Current;

            if ( disposable is Texture )
            {
                disposables.Remove();
            }
        }

        return result;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public class ModelParameters : AssetLoaderParameters
    {
        public TextureLoader.TextureParameter TextureParameter { get; set; }

        public ModelParameters()
        {
            TextureParameter           = new TextureLoader.TextureParameter();
            TextureParameter.MinFilter = TextureParameter.MagFilter = TextureFilter.Linear;
            TextureParameter.WrapU     = TextureParameter.WrapV     = TextureWrap.Repeat;
        }
    }
}