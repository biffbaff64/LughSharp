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
using LibGDXSharp.LibCore.Graphics;
using LibGDXSharp.LibCore.Graphics.G3D;
using LibGDXSharp.LibCore.Graphics.G3D.Models.Data;
using LibGDXSharp.LibCore.Graphics.G3D.Utils;
using LibGDXSharp.LibCore.Utils.Collections;

namespace LibGDXSharp.LibCore.Assets.Loaders;

[PublicAPI]
public abstract class ModelLoader<TP> : AsynchronousAssetLoader< Model, TP >
    where TP : ModelLoader< TP >.ModelLoaderParameters
{
    protected readonly ModelLoaderParameters defaultLoaderParameters = new();

    // Check this declaration.
    // Original Java was Array< ObjectMap.Entry< string, ModelData > > items...
    protected readonly List< ObjectMap< string, ModelData >.Entry > items = new();

    protected ModelLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    /// <summary>
    ///     Directly load the raw model data on the calling thread.
    /// </summary>
    protected virtual ModelData? LoadModelData<T>( in FileInfo fileHandle, T? parameters )
        where T : ModelLoaderParameters => default( ModelData );

    /// <summary>
    ///     Directly load the raw model data on the calling thread.
    /// </summary>
    public ModelData? LoadModelData( in FileInfo fileHandle ) => LoadModelData< ModelLoaderParameters >( fileHandle, null );

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
        => LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), parameters );

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle, ITextureProvider textureProvider )
        => LoadModel< ModelLoaderParameters >( fileHandle, textureProvider, null );

    /// <summary>
    ///     Directly load the model on the calling thread.
    ///     The model with not be managed by an <see cref="AssetManager" />.
    /// </summary>
    public Model? LoadModel( in FileInfo fileHandle ) => LoadModel< ModelLoaderParameters >( fileHandle, new ITextureProvider.FileTextureProvider(), null );

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

        List< AssetDescriptor > deps = new();

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
        public ModelLoaderParameters()
        {
            TextureLoaderParameters           = new TextureLoader.TextureLoaderParameters();
            TextureLoaderParameters.MinFilter = TextureLoaderParameters.MagFilter = TextureFilter.Linear;
            TextureLoaderParameters.WrapU     = TextureLoaderParameters.WrapV     = TextureWrap.Repeat;
        }

        public TextureLoader.TextureLoaderParameters TextureLoaderParameters { get; set; }
    }
}
