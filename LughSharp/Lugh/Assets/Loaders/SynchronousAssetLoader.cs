// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Assets.Loaders.Resolvers;

namespace LughSharp.Lugh.Assets.Loaders;

/// <summary>
/// Abstract base class for synchronous asset loaders.
/// </summary>
/// <typeparam name="TAssetType">The type of the asset to load.</typeparam>
/// <typeparam name="TParameters">The type of the parameters for loading the asset.</typeparam>
[PublicAPI]
public abstract class SynchronousAssetLoader< TAssetType, TParameters >
    : AssetLoader where TParameters : AssetLoaderParameters
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SynchronousAssetLoader{TAssetType, TParameters}"/>
    /// class with the specified file resolver.
    /// </summary>
    /// <param name="resolver">The file resolver to use for resolving asset file paths.</param>
    protected SynchronousAssetLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        LoaderType = AssetLoaderType.Synchronous;
    }

    /// <summary>
    /// Loads the asset synchronously.
    /// </summary>
    /// <param name="assetManager">The asset manager responsible for loading assets.</param>
    /// <param name="file">The file information of the asset to load.</param>
    /// <param name="parameter">The parameters for loading the asset.</param>
    /// <returns>The loaded asset of type <typeparamref name="TAssetType"/>.</returns>
    public abstract TAssetType Load( AssetManager assetManager, FileInfo file, TParameters parameter );
}
