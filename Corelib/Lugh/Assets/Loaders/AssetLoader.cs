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

namespace Corelib.Lugh.Assets.Loaders;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public enum AssetLoaderType : int
{
    Synchronous,
    Asynchronous,
}

/// <summary>
/// Abstract base class for asset loaders.
/// </summary>
[PublicAPI]
public abstract class AssetLoader
{
    /// <summary>
    /// <see cref="IFileHandleResolver"/> used to map from plain
    /// asset names to File Handle instances
    /// </summary>
    public IFileHandleResolver Resolver { get; }

    /// <summary>
    /// Indicates whether the child loader class is Async or Sync.
    /// </summary>
    public AssetLoaderType LoaderType { get; set; }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Constructor, sets the FileHandleResolver to use to resolve the file
    /// associated with the asset name.
    /// </summary>
    protected AssetLoader( IFileHandleResolver resolver )
    {
        Resolver = resolver;
    }

    /// <summary>
    /// Resolves the specified filename.
    /// </summary>
    /// <param name="fileName"> The filename to resolve. </param>
    /// <returns>
    /// A handle to the file, as resolved by the <see cref="IFileHandleResolver"/>
    /// set on the loader.
    /// </returns>
    public FileInfo Resolve( string fileName )
    {
        return Resolver.Resolve( fileName );
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first. This method may be
    /// called on a thread other than the GL thread.
    /// </summary>
    /// <param name="filename">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="p">parameters for loading the asset</param>
    public abstract List< AssetDescriptor > GetDependencies< TP >( string filename,
                                                                   FileInfo file,
                                                                   TP? p ) where TP : AssetLoaderParameters;
}
