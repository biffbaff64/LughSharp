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