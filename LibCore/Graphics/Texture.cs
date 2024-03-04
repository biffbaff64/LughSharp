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


using System.Text;

using LibGDXSharp.LibCore.Assets;
using LibGDXSharp.LibCore.Assets.Loaders;
using LibGDXSharp.LibCore.Utils.Collections.Extensions;

namespace LibGDXSharp.LibCore.Graphics;

/// <summary>
///     A Texture wraps a standard OpenGL ES texture.
///     <para>
///         A Texture can be managed. If the OpenGL context is lost all managed textures
///         get invalidated. This happens when a user switches to another application or
///         receives an incoming call. Managed textures get reloaded automatically.
///     </para>
///     <para>
///         A Texture has to be bound via the <see cref="Texture.Bind()" /> method in order
///         for it to be applied to geometry. The texture will be bound to the currently
///         active texture unit specified via <see cref="IGL20.GLActiveTexture" />.
///     </para>
///     <para>
///         You can draw <see cref="Pixmap" />s to a texture at any time. The changes will
///         be automatically uploaded to texture memory. This is of course not extremely
///         fast so use it with care. It also only works with unmanaged textures.
///     </para>
///     <para>
///         A Texture must be disposed when it is no longer used
///     </para>
/// </summary>
[PublicAPI]
public class Texture : GLTexture
{
    // ------------------------------------------------------------------------

    private readonly Dictionary< IApplication, List< Texture >? > _managedTextures = new();

    // ------------------------------------------------------------------------

    public Texture( string internalPath )
        : this( Core.Gdx.Files.Internal( internalPath ) )
    {
    }

    public Texture( FileInfo file, bool useMipMaps )
        : this( file, default( Pixmap.Format ), useMipMaps )
    {
    }

    public Texture( FileInfo? file, Pixmap.Format format = default( Pixmap.Format ), bool useMipMaps = false )
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
    ///     Creates a new Texture with the specified width, height, and Pixmap format.
    /// </summary>
    /// <param name="width">The width in pixels.</param>
    /// <param name="height">The Height in pixels.</param>
    /// <param name="format">The pixmap <see cref="Pixmap.Format" /></param>
    public Texture( int width, int height, Pixmap.Format format )
        : this( new PixmapTextureData( new Pixmap( width, height, format ), null, false, true ) )
    {
    }

    /// <summary>
    ///     Creates a new Texture using the supplied <see cref="ITextureData" />.
    /// </summary>
    public Texture( ITextureData? data )
        : this( IGL20.GL_TEXTURE_2D, ( int )Core.Gdx.GL.GLGenTexture(), data )
    {
    }

    protected Texture( int glTarget, int glTextureHandle, ITextureData? data )
        : base( glTarget, glTextureHandle )
    {
        ArgumentNullException.ThrowIfNull( data );

        Load( data );

        if ( data.IsManaged() )
        {
            AddManagedTexture( Core.Gdx.App, this );
        }
    }

    // ------------------------------------------------------------------------

    public AssetManager? AssetManager { get; set; } = null;
    public ITextureData? TextureData  { get; set; } = null;

    public override int  Width     => TextureData?.Width ?? 0;
    public override int  Height    => TextureData?.Height ?? 0;
    public override int  Depth     => 0;
    public override bool IsManaged => ( TextureData != null ) && TextureData.IsManaged();

    public int NumManagedTextures => _managedTextures[ Core.Gdx.App ]?.Count ?? 0;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public void Load( ITextureData? data )
    {
        if ( ( TextureData == null ) || ( data == null ) )
        {
            throw new GdxRuntimeException( "Load failed!: null TextureData supplied" );
        }

        if ( data.IsManaged() != TextureData.IsManaged() )
        {
            throw new GdxRuntimeException( "New data must have the same managed status as the old data" );
        }

        TextureData = data;

        if ( !data.IsPrepared )
        {
            data.Prepare();
        }

        Bind();

        UploadImageData( IGL20.GL_TEXTURE_2D, data );

        UnsafeSetFilter( MinFilter, MagFilter, true );
        UnsafeSetWrap( UWrap, VWrap, true );
        UnsafeSetAnisotropicFilter( AnisotropicFilterLevel, true );

        Core.Gdx.GL.GLBindTexture( GLTarget, 0 );
    }

    /// <summary>
    ///     Used internally to reload after context loss. Creates a new GL handle then
    ///     calls <see cref="Load(ITextureData)" />.
    /// </summary>
    protected override void Reload()
    {
        if ( !IsManaged )
        {
            throw new GdxRuntimeException( "Tried to reload unmanaged Texture" );
        }

        GLTextureHandle = ( int )Core.Gdx.GL.GLGenTexture();

        Load( TextureData );
    }

    /// <summary>
    ///     Draws the given <seealso cref="Pixmap" /> to the texture at position x, y.
    ///     No clipping is performed so you have to make sure that you draw only inside
    ///     the texture region. Note that this will only draw to mipmap level 0!
    /// </summary>
    /// <param name="pixmap"> The Pixmap </param>
    /// <param name="x"> The x coordinate in pixels </param>
    /// <param name="y"> The y coordinate in pixels  </param>
    public void Draw( Pixmap pixmap, int x, int y )
    {
        if ( ( TextureData != null ) && TextureData.IsManaged() )
        {
            throw new GdxRuntimeException( "can't draw to a managed texture" );
        }

        Bind();

        Core.Gdx.GL.GLTexSubImage2D( GLTarget,
                                     0,
                                     x,
                                     y,
                                     pixmap.Width,
                                     pixmap.Height,
                                     pixmap.GLFormat,
                                     pixmap.GLType,
                                     pixmap.Pixels );
    }

    /// <summary>
    ///     Disposes all resources associated with the texture.
    /// </summary>
    public override void Dispose()
    {
        Dispose( true );
    }

    /// <inheritdoc />
    protected override void Dispose( bool disposing )
    {
        if ( disposing )
        {
            // this is a hack. reason: we have to set the glHandle to 0 for textures that are
            // reloaded through the asset manager as we first remove (and thus dispose) the texture
            // and then reload it. the glHandle is set to 0 in invalidateAllTextures prior to
            // removal from the asset manager.
            if ( GLTextureHandle == 0 )
            {
                return;
            }

            Delete();

            if ( ( TextureData != null ) && TextureData.IsManaged() )
            {
                if ( _managedTextures[ Core.Gdx.App ] != null )
                {
                    _managedTextures[ Core.Gdx.App ]?.Remove( this );
                }
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
    ///     Invalidate all managed textures. This is an internal method. Do not use it!
    /// </summary>
    internal void InvalidateAllTextures( IApplication app )
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

                if ( fileName == null )
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
                    texture.GLTextureHandle = 0;

                    // create the parameters, passing the reference to the texture as
                    // well as a callback that sets the ref count.
                    var parameters = new TextureLoader.TextureLoaderParameters
                    {
                        TextureData    = texture.TextureData,
                        MinFilter      = texture.MinFilter,
                        MagFilter      = texture.MagFilter,
                        WrapU          = texture.UWrap,
                        WrapV          = texture.VWrap,
                        GenMipMaps     = texture.TextureData is { UseMipMaps: true },
                        Texture        = texture,
                        LoadedCallback = new LoadedCallbackInnerClass( refCount )
                    };

                    // unload the texture, create a new gl handle then reload it.
                    AssetManager.Unload( fileName );
                    texture.GLTextureHandle = ( int )Core.Gdx.GL.GLGenTexture();
                    AssetManager.Load( fileName, typeof( Texture ), parameters );
                }
            }

            managedTextureArray.Clear();
            managedTextureArray.AddAll( textures );
        }
    }

    public string GetManagedStatus()
    {
        var builder = new StringBuilder( "Managed textures/app: { " );

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

    public override string? ToString() => TextureData is FileTextureData
        ? TextureData.ToString()
        : base.ToString();

    /// <summary>
    ///     Clears all managed textures.
    /// </summary>
    internal void ClearAllTextures( IApplication app ) => _managedTextures.Remove( app );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private sealed class LoadedCallbackInnerClass : ILoadedCallback
    {
        private readonly int _refCount;

        public LoadedCallbackInnerClass( int refCount ) => _refCount = refCount;

        public void FinishedLoading( AssetManager assetManager, string? fileName, Type? type ) => assetManager.SetReferenceCount( fileName!, _refCount );
    }
}
