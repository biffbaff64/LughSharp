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


using LughSharp.LibCore.Assets.Loaders.Resolvers;

namespace LughSharp.LibCore.Assets.Loaders;

/// <summary>
///     <see cref="AssetLoader" /> for <see cref="IMusic" /> instances.
///     The Music instance is loaded synchronously.
/// </summary>
[PublicAPI]
public class MusicLoader : AsynchronousAssetLoader< IMusic, AssetLoaderParameters >, IDisposable
{
    public IMusic LoadedMusic { get; set; }

    public MusicLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
        LoadedMusic = null!;
    }

    /// <inheritdoc/>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameter )
    {
        return null!;
    }

    /// <inheritdoc />
    public override void LoadAsync( AssetManager manager, FileInfo? file, AssetLoaderParameters? parameter )
    {
    }

    /// <inheritdeoc/>
    public override object LoadSync( AssetManager? manager, FileInfo? file, AssetLoaderParameters? parameter )
    {
        LoadedMusic = Gdx.Audio.NewMusic( file );

        return LoadedMusic;
    }

    // ------------------------------------------------------------------------
    
    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            LoadedMusic = null!;
        }
    }
}