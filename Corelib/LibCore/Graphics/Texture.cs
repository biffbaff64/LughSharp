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

using Corelib.LibCore.Assets;
using Corelib.LibCore.Assets.Loaders;
using Corelib.LibCore.Core;
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics;

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
/// active texture unit specified via <see cref="GLBindings.glActiveTexture"/>.
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
[PublicAPI]
public class Texture : GLTexture, IManageable
{
    public AssetManager? AssetManager { get; set; } = null;
    public ITextureData  TextureData  { get; set; }

    // ------------------------------------------------------------------------

    public override int Width  => TextureData.Width;
    public override int Height => TextureData.Height;
    public override int Depth  => 0;

    public bool IsManaged
    {
        get => TextureData is { IsManaged: true };
        set { }
    }

    // ------------------------------------------------------------------------

    public int NumManagedTextures => _managedTextures[ Gdx.App ].Count;

    // ------------------------------------------------------------------------

    private readonly Dictionary< IApplication, List< Texture > > _managedTextures = new();

    // ------------------------------------------------------------------------

    /// <summary>
    /// Create a new Texture from the file at the given path.
    /// </summary>
    /// <param name="internalPath"></param>
    public Texture( string internalPath )
                    : this( Gdx.Files.Internal( internalPath ).File, false )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Create a new Texture from the file described by the given <see cref="FileInfo"/>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="useMipMaps"> Whether or not to generate MipMaps. Default is false. </param>
    public Texture( FileInfo file, bool useMipMaps )
                    : this( file, Pixmap.ColorFormat.Default, useMipMaps )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Create a new Texture from the file specified in the given <see cref="FileInfo"/>.
    /// The Texture pixmap format will be set to the given format, which defaults to
    /// <see cref="Pixmap.ColorFormat.RGBA8888"/>.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="format"> The pixmap format to use. </param>
    /// <param name="useMipMaps"> Whether or not to generate MipMaps. Default is false. </param>
    public Texture( FileInfo file,
                    Pixmap.ColorFormat format = Pixmap.ColorFormat.Default,
                    bool useMipMaps = false )
                    : this( TextureDataFactory.LoadFromFile( file, format, useMipMaps ) )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Creates a new Texture from the supplied <see cref="Pixmap"/>.
    /// </summary>
    /// <param name="pixmap"> The pixmap to use. </param>
    /// <param name="useMipMaps"> Whether or not to generate MipMaps. Default is false. </param>
    public Texture( Pixmap pixmap, bool useMipMaps = false )
                    : this( new PixmapTextureData( pixmap, null, useMipMaps, false ) )
    {
        Logger.Checkpoint();

        Debug();
    }

    /// <summary>
    /// Creates a new Texture from the supplied <see cref="Pixmap"/> and <see cref="Pixmap.ColorFormat"/>
    /// </summary>
    /// <param name="pixmap"> The pixmap to use. </param>
    /// <param name="format"> The pixmap format to use. </param>
    /// <param name="useMipMaps"> Whether or not to generate MipMaps. Default is false. </param>
    public Texture( Pixmap pixmap, Pixmap.ColorFormat format, bool useMipMaps = false )
                    : this( new PixmapTextureData( pixmap, format, useMipMaps, false ) )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Creates a new Texture with the specified width, height, and Pixmap format.
    /// </summary>
    /// <param name="width"> The width in pixels. </param>
    /// <param name="height"> The Height in pixels. </param>
    /// <param name="format"> The pixmap <see cref="Pixmap.ColorFormat"/> </param>
    public Texture( int width, int height, Pixmap.ColorFormat format )
                    : this( new PixmapTextureData( new Pixmap( width, height, format ), null, false, true ) )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Creates a new Texture using the supplied <see cref="ITextureData"/>.
    /// </summary>
    public Texture( ITextureData data )
                    : this( IGL.GL_TEXTURE_2D, Gdx.GL.glGenTexture(), data )
    {
        Logger.Checkpoint();
    }

    /// <summary>
    /// Default constructor. Creates a new Texture from the supplied GLTarget,
    /// GLTextureHandle and TextureData.
    /// </summary>
    /// <param name="glTarget"></param>
    /// <param name="glTextureHandle"></param>
    /// <param name="data"></param>
    protected Texture( int glTarget, uint glTextureHandle, ITextureData data )
                    : base( glTarget, glTextureHandle )
    {
        ArgumentNullException.ThrowIfNull( data );

        Logger.Checkpoint();
        Logger.Debug( $"data.Width: {data.Width}, data.Height: {data.Height}" );
        Logger.Debug( $"data.TextureDataType: {data.TextureDataType}" );
        Logger.Debug( $"data.Format: {data.Format}" );
        Logger.Debug( $"data.IsPrepared: {data.IsPrepared}" );

        TextureData = data;

        Load( data );

        if ( data.IsManaged )
        {
            AddManagedTexture( Gdx.App, this );
        }
    }

    /// <summary>
    /// Load the given <see cref="ITextureData"/> data into this Texture.
    /// </summary>
    /// <param name="data"></param>
    public void Load( ITextureData data )
    {
        Logger.Checkpoint();

        if ( data.IsManaged != TextureData.IsManaged )
        {
            throw new GdxRuntimeException( "New data must have the same managed status as the old data" );
        }

        TextureData = data;

        if ( !data.IsPrepared )
        {
            data.Prepare();
        }

        Bind();

        UploadImageData( IGL.GL_TEXTURE_2D, data );

        UnsafeSetFilter( MinFilter, MagFilter, true );
        UnsafeSetWrap( UWrap, VWrap, true );
        UnsafeSetAnisotropicFilter( AnisotropicFilterLevel, true );

        Gdx.GL.glBindTexture( GLTarget, 0 );
    }

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Load(ITextureData)"/>.
    /// </summary>
    internal override void Reload()
    {
        if ( !IsManaged )
        {
            throw new GdxRuntimeException( "Tried to reload unmanaged Texture" );
        }

        GLTextureHandle = Gdx.GL.glGenTexture();

        Load( TextureData );
    }

    /// <summary>
    /// Draws the given <see cref="Pixmap"/> to the texture at position x, y. No clipping
    /// is performed so it is important to make sure that you drawing is only done inside
    /// the texture region. Note that this will only draw to mipmap level 0!
    /// </summary>
    /// <param name="pixmap"> The Pixmap </param>
    /// <param name="x"> The x coordinate in pixels </param>
    /// <param name="y"> The y coordinate in pixels  </param>
    public void Draw( Pixmap pixmap, int x, int y )
    {
        if ( TextureData is { IsManaged: true } )
        {
            throw new GdxRuntimeException( "can't draw to a managed texture" );
        }

        Bind();

        Gdx.GL.glTexSubImage2D( GLTarget,
                        0, x, y,
                        pixmap.Width,
                        pixmap.Height,
                        pixmap.GLFormat,
                        pixmap.GLType,
                        pixmap.PixelData );
    }

    /// <summary>
    /// Add the supplied MANAGED texture to the list of managed textures.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="texture"></param>
    private void AddManagedTexture( IApplication app, Texture texture )
    {
        List< Texture > managedTextureArray = _managedTextures.TryGetValue( app, out List< Texture >? managedTexture )
                        ? managedTexture
                        : [ ];

        managedTextureArray.Add( texture );

        _managedTextures.Put( app, managedTextureArray );
    }

    /// <summary>
    /// Invalidate all managed textures. This is an internal method. Do not use it!
    /// </summary>
    internal void InvalidateAllTextures( IApplication app )
    {
        if ( AssetManager == null )
        {
            foreach ( var t in _managedTextures[ app ] )
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
            var textures = new List< Texture >( _managedTextures[ app ] );

            foreach ( var texture in textures )
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
                    texture.GLTextureHandle = Gdx.GL.glGenTexture();
                    AssetManager.AddToLoadqueue( fileName, typeof( Texture ), parameters );
                }
            }

            _managedTextures[ app ].Clear();
            _managedTextures[ app ].AddAll( textures );
        }
    }

    /// <summary>
    /// Returns a string detailing the managed status of the textures
    /// within the managed textures list.
    /// </summary>
    public string GetManagedStatus()
    {
        var builder = new StringBuilder( "Managed textures/app: { " );

        foreach ( var app in _managedTextures.Keys )
        {
            builder.Append( _managedTextures[ app ].Count );
            builder.Append( ' ' );
        }

        builder.Append( '}' );

        return builder.ToString();
    }

    /// <summary>
    /// Clears all managed textures.
    /// </summary>
    internal void ClearAllTextures( IApplication app )
    {
        _managedTextures.Remove( app );
    }

    /// <inheritdoc />
    public override string? ToString()
    {
        return TextureData is FileTextureData ? TextureData.ToString() : base.ToString();
    }

    public void Debug()
    {
        Logger.Debug( $"Width: {Width}, Height: {Height}" );
        Logger.Debug( $"Format: {TextureData.Format}" );
        Logger.Debug( $"IsManaged: {IsManaged}" );
        Logger.Debug( $"Depth: {Depth}" );
    }

    /// <summary>
    /// Disposes all resources associated with the texture.
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
            // this is a hack.
            // Reason: we have to set the glHandle to 0 for textures that are
            // reloaded through the asset manager as we first remove (and thus
            // dispose) the texture and then reload it. the glHandle is set to
            // 0 in invalidateAllTextures prior to removal from the asset manager.
            if ( GLTextureHandle == 0 )
            {
                return;
            }

            Delete();

            if ( TextureData is { IsManaged: true } )
            {
                _managedTextures[ Gdx.App ].Remove( this );
            }
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///
    /// </summary>
    private sealed class LoadedCallbackInnerClass( int refCount ) : ILoadedCallback
    {
        public void FinishedLoading( AssetManager assetManager, string? fileName, Type? type )
        {
            assetManager.SetReferenceCount( fileName!, refCount );
        }
    }
}