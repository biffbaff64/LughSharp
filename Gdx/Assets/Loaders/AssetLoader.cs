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


using LibGDXSharp.Gdx.Assets.Loaders.Resolvers;

namespace LibGDXSharp.Gdx.Assets.Loaders;

/// <summary>
///     Abstract base class for asset loaders.
/// </summary>
public abstract class AssetLoader
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Constructor, sets the FileHandleResolver to use to resolve the file
    ///     associated with the asset name.
    /// </summary>
    protected AssetLoader( IFileHandleResolver resolver ) => Resolver = resolver;

    public IFileHandleResolver Resolver      { get; }
    public bool                IsSynchronous { get; protected init; } = false;

    /// <summary>
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public FileInfo Resolve( string fileName ) => Resolver.Resolve( fileName );

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first. This method may be
    ///     called on a thread other than the GL thread.
    /// </summary>
    /// <param name="filename">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="p">parameters for loading the asset</param>
    public virtual List< AssetDescriptor > GetDependencies( string? filename,
                                                            FileInfo? file,
                                                            AssetLoaderParameters? p ) => null!;
}
