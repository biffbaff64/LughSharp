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

using JetBrains.Annotations;

namespace LibGDXSharp.Assets.Loaders;

[PublicAPI]
public abstract class AsynchronousAssetLoader : AssetLoader
{
    /// <summary>
    /// </summary>
    /// <param name="resolver"></param>
    protected AsynchronousAssetLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        IsSynchronous = false;
    }

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public abstract void LoadAsync( AssetManager manager,
                                    string fileName,
                                    FileInfo file,
                                    AssetLoaderParameters parameter );

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public abstract object LoadSync( AssetManager manager,
                                     string fileName,
                                     FileInfo file,
                                     AssetLoaderParameters parameter );

    /// <summary>
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public void UnloadAsync( AssetManager manager,
                             string fileName,
                             FileInfo file,
                             AssetLoaderParameters parameter )
    {
    }
}

/// <summary>
/// Base class for asynchronous AssetLoader instances. Such loaders try to load parts
/// of an OpenGL resource, like the Pixmap, on a separate thread to then load the actual
/// resource on the thread the OpenGL context is active on.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TP"></typeparam>
[PublicAPI]
public abstract class AsynchronousAssetLoader<T, TP> : AssetLoader where TP : AssetLoaderParameters
{
    /// <summary>
    /// </summary>
    /// <param name="resolver"></param>
    protected AsynchronousAssetLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        IsSynchronous = false;
    }

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the AssetManager.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public abstract void LoadAsync( AssetManager manager,
                                    string fileName,
                                    FileInfo file,
                                    AssetLoaderParameters parameter );

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public abstract T LoadSync( AssetManager manager,
                                string fileName,
                                FileInfo file,
                                AssetLoaderParameters parameter );

    /// <summary>
    /// Called if this task is unloaded before loadSync is called. This method may be
    /// invoked on any thread, but will not be invoked during or after loadSync. This
    /// method is not invoked when a task is cancelled because it threw an exception,
    /// only when the asset is unloaded before loading is complete. The default
    /// implementation does nothing. Subclasses should release any resources acquired
    /// in loadAsync, which may or may not have been called before this method, but
    /// never during or after this method. Note that loadAsync may still be executing
    /// when this method is called and must release any resources it allocated.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public virtual void UnloadAsync( AssetManager manager,
                                     string fileName,
                                     FileInfo file,
                                     AssetLoaderParameters parameter )
    {
    }
}