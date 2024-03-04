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

namespace LibGDXSharp.LibCore.Assets.Loaders;

[PublicAPI]
public class PixmapLoader : AsynchronousAssetLoader< Pixmap, PixmapLoader.PixmapLoaderParameter >
{
    private Pixmap _pixmap;

    public PixmapLoader( IFileHandleResolver resolver ) : base( resolver ) => _pixmap = default( Pixmap )!;

    /// <inheritdoc />
    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? file,
                               PixmapLoaderParameter? parameter )
    {
        ArgumentNullException.ThrowIfNull( file );

        _pixmap = new Pixmap( file );
    }

    /// <summary>
    ///     Performs application-defined tasks associated with freeing,
    ///     releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _pixmap.Dispose();
            _pixmap = null!;
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
