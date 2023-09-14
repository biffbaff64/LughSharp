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
/// <see cref="AssetLoader"/> for <see cref="IMusic"/> instances.
/// The Music instance is loaded synchronously.
/// </summary>
[PublicAPI]
public class MusicLoader : AsynchronousAssetLoader< IMusic, MusicLoader.MusicParameter >, IDisposable
{
    public IMusic LoadedMusic { get; set; }

    public MusicLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
        LoadedMusic = null!;
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
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    /// <summary>
    /// Loads the non-OpenGL part of the asset and injects any dependencies of
    /// the asset into the AssetManager.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
        LoadedMusic = Gdx.Audio.NewMusic( file );
    }

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override IMusic LoadSync( AssetManager? manager,
                                     string? fileName,
                                     FileInfo? file,
                                     AssetLoaderParameters parameter )
    {
        IMusic music = LoadedMusic;

        LoadedMusic = null!;

        return music;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public class MusicParameter : AssetLoaderParameters
    {
    }
}