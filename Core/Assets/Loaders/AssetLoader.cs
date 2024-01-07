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

namespace LibGDXSharp.Assets.Loaders;

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

    public IFileHandleResolver Resolver      { get; set; }
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
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public abstract List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter );
}
