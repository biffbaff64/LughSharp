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

[PublicAPI]
public class PixmapLoader : AsynchronousAssetLoader< Pixmap, PixmapLoader.PixmapLoaderParameter >
{
    private Pixmap _pixmap;

    public PixmapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        this._pixmap = default( Pixmap )!;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    PixmapLoaderParameter? parameter )
    {
        ArgumentNullException.ThrowIfNull( file );
        
        _pixmap = new Pixmap( file );
    }

    public override Pixmap LoadSync( AssetManager manager,
                                     string? fileName,
                                     FileInfo? file,
                                     PixmapLoaderParameter? parameter )
    {
        Pixmap pixmap = this._pixmap;

        this._pixmap = null!;

        return pixmap;
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            this._pixmap.Dispose();
            this._pixmap = null!;
        }
    }
    
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    [PublicAPI]
    public class PixmapLoaderParameter : AssetLoaderParameters
    {
        public PixmapLoaderParameter()
        {
        }
    }
}
