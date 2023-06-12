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

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Assets.Loaders;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class PixmapLoader : AsynchronousAssetLoader
{
    private Pixmap? _pixmap;

    public PixmapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
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
                                                             IAssetLoaderParameters parameter )
    {
        return null!;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    IAssetLoaderParameters parameter )
    {
        Debug.Assert( file != null, nameof( file ) + " != null" );

        _pixmap = new Pixmap( file );
    }

    public override Pixmap LoadSync( AssetManager? manager,
                                     string? fileName,
                                     FileInfo? file,
                                     IAssetLoaderParameters parameter )
    {
        Pixmap? pixmap = _pixmap;

        _pixmap = null!;

        Debug.Assert( pixmap != null, nameof( pixmap ) + " != null" );

        return pixmap;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        _pixmap?.Dispose();
        _pixmap = null;
    }

    public class PixmapParameter
    {
    }
}