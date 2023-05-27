using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Audio;

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> to load <see cref="ISound"/> instances.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class SoundLoader : AsynchronousAssetLoader< ISound, SoundLoader.SoundParameter >
{
    /// <summary>
    /// The <see cref="ISound"/> instance currently loaded by this <see cref="SoundLoader"/>.
    /// </summary>
    public ISound LoadedSound { get; set; }

    public SoundLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        LoadedSound = null!;
    }

    /// <summary>
    /// Returns the assets this asset requires to be loaded first.
    /// This method may be called on a thread other than the GL thread.
    /// </summary>
    /// <param name="fileName">name of the asset to load</param>
    /// <param name="file">the resolved file to load</param>
    /// <param name="parameter">parameters for loading the asset</param>
    public override List< AssetDescriptor > GetDependencies( string? fileName, FileInfo? file, IAssetLoaderParameters parameter )
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
    public override void LoadAsync( AssetManager? manager, string? fileName, FileInfo? file, IAssetLoaderParameters parameter )
    {
        LoadedSound = Gdx.Audio.NewSound( file );
    }

    /// <summary>
    /// Loads the OpenGL part of the asset.
    /// </summary>
    /// <param name="manager"></param>
    /// <param name="fileName"></param>
    /// <param name="file"></param>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override ISound LoadSync( AssetManager? manager, string? fileName, FileInfo? file, IAssetLoaderParameters parameter )
    {
        ISound sound = LoadedSound;

        LoadedSound = null!;

        return sound;
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing,
    /// releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }

    public class SoundParameter : AssetLoaderParameters
    {
    }
}