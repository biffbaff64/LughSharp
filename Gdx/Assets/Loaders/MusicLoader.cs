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
using LibGDXSharp.Gdx.Audio;

namespace LibGDXSharp.Gdx.Assets.Loaders;

/// <summary>
///     <see cref="AssetLoader" /> for <see cref="IMusic" /> instances.
///     The Music instance is loaded synchronously.
/// </summary>
public class MusicLoader : AsynchronousAssetLoader< IMusic, AssetLoaderParameters >, IDisposable
{
    public MusicLoader( IFileHandleResolver resolver )
        : base( resolver ) => LoadedMusic = null!;

    public IMusic LoadedMusic { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
        GC.SuppressFinalize( this );
    }

    /// <summary>
    ///     Returns the assets this asset requires to be loaded first.
    ///     This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameter ) => null!;

    /// <summary>
    ///     Loads the non-OpenGL part of the asset and injects any dependencies of
    ///     the asset into the AssetManager.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void Load( AssetManager? manager,
                               string? fileName,
                               FileInfo? file,
                               AssetLoaderParameters? parameter ) => LoadedMusic = Core.Gdx.Audio.NewMusic( file );

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            LoadedMusic = null!;
        }
    }
}
