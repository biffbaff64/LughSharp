using System.Diagnostics.CodeAnalysis;
using System.Text;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Graphics;

/// <summary>
/// A Texture wraps a standard OpenGL ES texture.
/// <para>
/// A Texture can be managed. If the OpenGL context is lost all managed textures
/// get invalidated. This happens when a user switches to another application or
/// receives an incoming call. Managed textures get reloaded automatically.
/// </para>
/// <para>
/// A Texture has to be bound via the <see cref="Texture.Bind()"/> method in order
/// for it to be applied to geometry. The texture will be bound to the currently
/// active texture unit specified via <see cref="IGL20.GLActiveTexture(int)"/>.
/// </para>
/// <para>
/// You can draw <see cref="Pixmap"/>s to a texture at any time. The changes will
/// be automatically uploaded to texture memory. This is of course not extremely
/// fast so use it with care. It also only works with unmanaged textures.
/// </para>
/// <para>
/// A Texture must be disposed when it is no longer used
/// </para>
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Texture : GLTexture
{

    #region Properties

    public AssetManager? AssetManager { get; set; } = null;
    public ITextureData? TextureData  { get; set; } = null;

    #endregion

    private readonly Dictionary< IApplication, List< Texture >? > _managedTextures = new();

    #region Constructors

    public Texture( string internalPath )
        : this( Gdx.Files.Internal( internalPath ) )
    {
    }

    public Texture( FileInfo file, bool useMipMaps )
        : this( file, default, useMipMaps )
    {
    }

    public Texture( FileInfo? file, Pixmap.Format format = default, bool useMipMaps = false )
        : this( ITextureData.Factory.LoadFromFile( file, format, useMipMaps ) )
    {
    }

    public Texture( Pixmap pixmap )
        : this( new PixmapTextureData( pixmap, null, false, false ) )
    {
    }

    public Texture( Pixmap pixmap, bool useMipMaps )
        : this( new PixmapTextureData( pixmap, null, useMipMaps, false ) )
    {
    }

    public Texture( Pixmap pixmap, Pixmap.Format format, bool useMipMaps ) : this
        ( new PixmapTextureData( pixmap, format, useMipMaps, false ) )
    {
    }

    /// <summary>
    /// Creates a new Texture with the specified width, height, and Pixmap format.
    /// </summary>
    /// <param name="width">The width in pixels.</param>
    /// <param name="height">The Height in pixels.</param>
    /// <param name="format">The pixmap <see cref="Pixmap.Format"/></param>
    public Texture( int width, int height, Pixmap.Format format )
        : this
            (
             new PixmapTextureData
                 (
                  new Pixmap( width, height, format ),
                  null,
                  false,
                  true
                 )
            )
    {
    }

    public Texture( ITextureData? data )
        : this( IGL20.GL_Texture_2D, Gdx.GL.GLGenTexture(), data )
    {
    }

    protected Texture( int glTarget, int glHandle, ITextureData? data )
        : base( glTarget, glHandle )
    {
        if ( data == null ) throw new GdxRuntimeException( "data cannot be null!" );

        Load( data );

        if ( data.IsManaged() )
        {
            AddManagedTexture( Gdx.App, this );
        }
    }

    #endregion

    public void Load( ITextureData? data )
    {
        if ( ( this.TextureData == null ) || ( data == null ) )
        {
            throw new GdxRuntimeException( "Load failed!: null TextureData supplied" );
        }

        if ( data.IsManaged() != this.TextureData.IsManaged() )
        {
            throw new GdxRuntimeException( "New data must have the same managed status as the old data" );
        }

        this.TextureData = data;

        if ( !data.IsPrepared() ) data.Prepare();

        Bind();

        UploadImageData( IGL20.GL_Texture_2D, data );

        UnsafeSetFilter( MinFilter, MagFilter, true );
        UnsafeSetWrap( UWrap, VWrap, true );
        UnsafeSetAnisotropicFilter( AnisotropicFilterLevel, true );

        Gdx.GL.GLBindTexture( GLTarget, 0 );
    }

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Load(ITextureData)"/>.
    /// </summary>
    protected override void Reload()
    {
        if ( !IsManaged() )
        {
            throw new GdxRuntimeException( "Tried to reload unmanaged Texture" );
        }

        GLHandle = Gdx.GL.GLGenTexture();
        Load( TextureData );
    }

    /// <summary>
    /// Draws the given <seealso cref="Pixmap"/> to the texture at position x, y. No clipping is performed so you have to make sure that you
    /// draw only inside the texture region. Note that this will only draw to mipmap level 0!
    /// </summary>
    /// <param name="pixmap"> The Pixmap </param>
    /// <param name="x"> The x coordinate in pixels </param>
    /// <param name="y"> The y coordinate in pixels  </param>
    public virtual void Draw( Pixmap pixmap, int x, int y )
    {
        if ( ( TextureData != null ) && TextureData.IsManaged() )
        {
            throw new GdxRuntimeException( "can't draw to a managed texture" );
        }

        Bind();

        Gdx.GL.GLTexSubImage2D
            (
             GLTarget,
             0,
             x,
             y,
             pixmap.Width,
             pixmap.Height,
             pixmap.GLFormat,
             pixmap.GLType,
             pixmap.Pixels
            );
    }

    public override int Width  => TextureData?.GetWidth() ?? 0;
    public override int Height => TextureData?.GetHeight() ?? 0;
    public override int Depth  => 0;

    public int NumManagedTextures => _managedTextures[ Gdx.App ]?.Count ?? 0;

    public override bool IsManaged() => ( TextureData != null ) && TextureData.IsManaged();

    /// <summary>
    /// Disposes all resources associated with the texture.
    /// </summary>
    public new void Dispose()
    {
        // this is a hack. reason: we have to set the glHandle to 0 for textures that are
        // reloaded through the asset manager as we first remove (and thus dispose) the texture
        // and then reload it. the glHandle is set to 0 in invalidateAllTextures prior to
        // removal from the asset manager.
        if ( GLHandle == 0 )
        {
            return;
        }

        Delete();

        if ( ( TextureData != null ) && TextureData.IsManaged() )
        {
            if ( _managedTextures[ Gdx.App ] != null )
            {
                _managedTextures[ Gdx.App ]?.Remove( this );
            }
        }
    }

    private void AddManagedTexture( IApplication app, Texture texture )
    {
        List< Texture > managedTextureArray = _managedTextures[ app ] ?? new List< Texture >();

        managedTextureArray.Add( texture );

        _managedTextures[ app ] = managedTextureArray;
    }

    /// <summary>
    /// Invalidate all managed textures. This is an internal method. Do not use it! </summary>
    public void InvalidateAllTextures( IApplication app )
    {
        List< Texture >? managedTextureArray = _managedTextures[ app ];

        if ( managedTextureArray == null )
        {
            return;
        }

        if ( AssetManager == null )
        {
            foreach ( Texture t in managedTextureArray )
            {
                t.Reload();
            }
        }
        else
        {
            // first we have to make sure the AssetManager isn't loading anything anymore,
            // otherwise the ref counting trick below wouldn't work (when a texture is
            // currently on the task stack of the manager.)
            AssetManager.FinishLoading();

            // next we go through each texture and reload either directly or via the
            // asset manager.
            var textures = new List< Texture >( managedTextureArray );

            foreach ( Texture texture in textures )
            {
                var fileName = AssetManager.GetAssetFileName( texture );

                if ( string.ReferenceEquals( fileName, null ) )
                {
                    texture.Reload();
                }
                else
                {
                    // get the ref count of the texture, then set it to 0 so we
                    // can actually remove it from the assetmanager. Also set the
                    // handle to zero, otherwise we might accidentially dispose
                    // already reloaded textures.
                    var refCount = AssetManager.GetReferenceCount( fileName );
                    AssetManager.SetReferenceCount( fileName, 0 );
                    texture.GLHandle = 0;

                    // create the parameters, passing the reference to the texture as
                    // well as a callback that sets the ref count.
                    var parameters = new TextureLoader.TextureParameter
                    {
                        TextureData = texture.TextureData,
                        MinFilter   = texture.MinFilter,
                        MagFilter   = texture.MagFilter,
                        WrapU       = texture.UWrap,
                        WrapV       = texture.VWrap,
                        GenMipMaps = ( texture.TextureData != null )
                                     && texture.TextureData.UseMipMaps(), // not sure about this?
                        Texture        = texture,
                        LoadedCallback = new LoadedCallbackInnerClass( refCount )
                    };

                    // unload the texture, create a new gl handle then reload it.
                    AssetManager.Unload( fileName );
                    texture.GLHandle = Gdx.GL.GLGenTexture();
                    AssetManager.Load( fileName, typeof(Texture), parameters );
                }
            }

            managedTextureArray.Clear();
            managedTextureArray.AddAll( textures );
        }
    }

    private sealed class LoadedCallbackInnerClass : IAssetLoaderParameters.ILoadedCallback
    {
        private readonly int _refCount;

        public LoadedCallbackInnerClass( int refCount )
        {
            this._refCount = refCount;
        }

        public void FinishedLoading( AssetManager assetManager, string? fileName, Type? type )
        {
            assetManager.SetReferenceCount( fileName!, _refCount );
        }
    }

    public string GetManagedStatus()
    {
        var builder = new StringBuilder();

        builder.Append( "Managed textures/app: { " );

        foreach ( IApplication app in _managedTextures.Keys )
        {
            if ( _managedTextures[ app ] != null )
            {
                builder.Append( _managedTextures[ app ]?.Count );
                builder.Append( ' ' );
            }
        }

        builder.Append( '}' );

        return builder.ToString();
    }

    public override string? ToString()
    {
        return TextureData is FileTextureData ? TextureData.ToString() : base.ToString();
    }

    /// <summary>
    /// Clears all managed textures. This is an internal method. Do not use it!
    /// </summary>
    internal void ClearAllTextures( IApplication app )
    {
        _managedTextures.Remove( app );
    }
}
