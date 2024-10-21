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

namespace Corelib.LibCore.Assets.Loaders;

/// <summary>
/// Base class for asynchronous AssetLoader instances. Such loaders try to load parts
/// of an OpenGL resource, like the Pixmap, on a separate thread to then load the actual
/// resource on the thread the OpenGL context is active on.
/// </summary>
[PublicAPI]
public abstract class AsynchronousAssetLoader : AssetLoader
{
    /// <summary>
    /// Creates a new AsynchronousAssetLoader using the specified <see cref="IFileHandleResolver"/>
    /// </summary>
    /// <param name="resolver"> The resolver to use. </param>
    protected AsynchronousAssetLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
        LoaderType = AssetLoaderType.Asynchronous;
    }

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="file"> the resolved file to load </param>
    /// <param name="parameter"></param>
    public abstract object? LoadSync< TP >( AssetManager manager,
                                            FileInfo file,
                                            TP? parameter ) where TP : AssetLoaderParameters;

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the <paramref name="manager"/>.
    /// </summary>
    /// <param name="manager">The asset manager responsible for loading the asset.</param>
    /// <param name="file">The file information of the asset to load.</param>
    /// <param name="parameter">The parameters for loading the asset.</param>
    public abstract void LoadAsync< TP >( AssetManager manager,
                                    FileInfo file,
                                    TP? parameter ) where TP : AssetLoaderParameters;

    /// <summary>
    /// Called if this task is unloaded before <see cref="LoadSync{TP}"/> is called. This method may
    /// be invoked on any thread, but will not be invoked during or after <see cref="LoadSync{TP}"/>.
    /// This method is not invoked when a task is cancelled because it threw an exception, only
    /// when the asset is unloaded before loading is complete. The default implementation does
    /// nothing. Subclasses should release any resources acquired in <see cref="LoadAsync{TP}"/>,
    /// which may or may not have been called before this method, but never during or after this
    /// method. Note that <see cref="LoadAsync{TP}"/> may still be executing when this method is called
    /// and must release any resources it allocated.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public virtual void UnloadAsync< TP >( AssetManager manager,
                                           FileInfo file,
                                           TP? parameter ) where TP : AssetLoaderParameters
    {
    }
}
