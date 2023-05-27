using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Audio;

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="IMusic"/> instances.
/// The Music instance is loaded synchronously.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class MusicLoader : AsynchronousAssetLoader< IMusic, MusicLoader.MusicParameter >, IDisposable
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
                                                             IAssetLoaderParameters parameter )
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
    public override void LoadAsync( AssetManager? manager, string? fileName,
                                    FileInfo? file,
                                    IAssetLoaderParameters parameter )
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
    public override IMusic LoadSync( AssetManager? manager, string? fileName,
                                      FileInfo? file,
                                      IAssetLoaderParameters parameter )
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