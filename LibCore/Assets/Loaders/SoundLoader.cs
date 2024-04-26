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
///     <see cref="AssetLoaderBase" /> to load <see cref="ISound" /> instances.
/// </summary>
[PublicAPI]
public class SoundLoader : AsynchronousAssetLoader< ISound, SoundLoader.SoundLoaderParameters >, IDisposable
{
    public SoundLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        LoadedSound = null!;
    }

    /// <summary>
    ///     The <see cref="ISound" /> instance currently loaded by this <see cref="SoundLoader" />.
    /// </summary>
    public ISound LoadedSound { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose( true );
    }

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
                               SoundLoaderParameters? parameter )
    {
        LoadedSound = Gdx.Audio.NewSound( file );
    }

    private void Dispose( bool disposing )
    {
        if ( disposing )
        {
            LoadedSound = null!;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class SoundLoaderParameters : AssetLoaderParameters
    {
    }
}