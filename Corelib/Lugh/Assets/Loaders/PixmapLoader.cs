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
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.Images;

namespace Corelib.Lugh.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="Pixmap"/> instances.
/// The Pixmap is loaded asynchronously.
/// </summary>
[PublicAPI]
public class PixmapLoader : AsynchronousAssetLoader
{
    private Pixmap? _pixmap;

    /// <summary>
    /// Creates a new PixmapLoader using the provided <see cref="IFileHandleResolver"/>
    /// </summary>
    /// <param name="resolver"> The resolver to use. </param>
    public PixmapLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        _pixmap = default( Pixmap )!;
    }

    /// <inheritdoc />
    public override List< AssetDescriptor > GetDependencies< TP >( string filename,
                                                                   FileInfo file,
                                                                   TP? parameter ) where TP : class
    {
        return null!;
    }

    /// <inheritdoc />
    public override void LoadAsync< TP >( AssetManager manager,
                                          FileInfo file,
                                          TP? parameter ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( file );

        _pixmap = new Pixmap( file );
    }

    /// <inheritdoc />
    public override object? LoadSync< TP >( AssetManager manager,
                                            FileInfo file,
                                            TP? parameter ) where TP : class
    {
        var pixmap = _pixmap;

        _pixmap = null;

        return pixmap;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose( true );
    }

    /// <summary>
    /// Releases the unmanaged resources used by the texture loader.
    /// </summary>
    /// <param name="disposing">
    /// True to release both managed and unmanaged resources; false to release only unmanaged resources.
    /// </param>
    protected void Dispose( bool disposing )
    {
        if ( disposing )
        {
            _pixmap?.Dispose();
            _pixmap = null!;
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Parameters for loading Pixmap assets. The default class provides no extra
    /// parameters and acts as a placeholder for possible future extensions.
    /// </summary>
    [PublicAPI]
    public class PixmapLoaderParameter : AssetLoaderParameters
    {
    }
}
