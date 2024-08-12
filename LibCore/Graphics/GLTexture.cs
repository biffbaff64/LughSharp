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


using LughSharp.LibCore.Core;
using LughSharp.LibCore.Graphics.GLUtils;
using LughSharp.LibCore.Graphics.OpenGL;
using LughSharp.LibCore.Maths;
using LughSharp.LibCore.Utils;
using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.LibCore.Graphics;

/// <summary>
/// Class representing an OpenGL texture by its target and handle. Keeps track of
/// its state like the TextureFilter and TextureWrap. Also provides some static
/// methods to create TextureData and upload image data.
/// </summary>
[PublicAPI]
public abstract class GLTexture : IDisposable, IManageable
{
    public virtual int Width  { get; }
    public virtual int Height { get; }
    public virtual int Depth  { get; }

    public uint  GLTextureHandle        { get; set; }
    public int   GLTarget               { get; }
    public float AnisotropicFilterLevel { get; private set; } = 1.0f;

    // ------------------------------------------------------------------------

    private static float _maxAnisotropicFilterLevel = 0;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Returns the <see cref="TextureFilter"/> used for minification.
    /// </summary>
    public TextureFilter MinFilter { get; private set; } = TextureFilter.Nearest;

    /// <summary>
    /// Returns the <see cref="TextureFilter"/> used for magnification.
    /// </summary>
    public TextureFilter MagFilter { get; private set; } = TextureFilter.Nearest;

    /// <summary>
    /// Returns the <see cref="TextureWrap"/> used for horizontal (U) texture coordinates.
    /// </summary>
    public TextureWrap UWrap { get; set; } = TextureWrap.ClampToEdge;

    /// <summary>
    /// Returns the <see cref="TextureWrap"/> used for vertical (V) texture coordinates.
    /// </summary>
    public TextureWrap VWrap { get; set; } = TextureWrap.ClampToEdge;

    public virtual bool IsManaged
    {
        get => false;
        set { }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    protected GLTexture( int glTarget )
        : this( glTarget, Gdx.GL.glGenTexture() )
    {
        Logger.CheckPoint();
    }

    protected GLTexture( int glTarget, uint glTextureHandle )
    {
        Logger.CheckPoint();
        Logger.Debug( $"GLTarget: {glTarget}, GLTextureHandle: {GLTextureHandle}" );

        GLTarget        = glTarget;
        GLTextureHandle = glTextureHandle;
    }

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Texture.Load"/>.
    /// </summary>
    protected abstract void Reload();

    /// <summary>
    /// Binds this texture. The texture will be bound to the currently active
    /// texture unit.
    /// </summary>
    public void Bind()
    {
        Gdx.GL.glBindTexture( GLTarget, GLTextureHandle );
    }

    /// <summary>
    /// Binds the texture to the given texture unit.
    /// Sets the currently active texture unit.
    /// </summary>
    /// <param name="unit"> the unit (0 to MAX_TEXTURE_UNITS).  </param>
    public void Bind( int unit )
    {
        Gdx.GL.glActiveTexture( IGL.GL_TEXTURE0 + unit );
        Gdx.GL.glBindTexture( GLTarget, ( uint ) GLTextureHandle );
    }

    /// <summary>
    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="u"> the u wrap </param>
    /// <param name="v"> the v wrap </param>
    /// <param name="force">
    /// True to always set the values, even if they are the same as the current values.
    /// </param>
    public void UnsafeSetWrap( TextureWrap? u, TextureWrap? v, bool force = false )
    {
        if ( ( u != null ) && ( force || ( UWrap != u ) ) )
        {
            Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_WRAP_S, u.GLEnum );
            UWrap = u;
        }

        if ( ( v != null ) && ( force || ( VWrap != v ) ) )
        {
            Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_WRAP_T, v.GLEnum );
            VWrap = v;
        }
    }

    /// <summary>
    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
    /// This will bind this texture!
    /// </summary>
    /// <param name="u">the u wrap</param>
    /// <param name="v">the v wrap</param>
    public void SetWrap( TextureWrap u, TextureWrap v )
    {
        UWrap = u;
        VWrap = v;

        Bind();

        Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_WRAP_S, u.GLEnum );
        Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_WRAP_T, v.GLEnum );
    }

    /// <summary>
    /// Sets the <see cref="TextureFilter"/> for this texture for minification and
    /// magnification. Assumes the texture is bound and active!
    /// </summary>
    /// <param name="minFilter"> the minification filter </param>
    /// <param name="magFilter"> the magnification filter  </param>
    /// <param name="force">
    /// True to always set the values, even if they are the same as the current values.
    /// Default is false.
    /// </param>
    public void UnsafeSetFilter( TextureFilter? minFilter, TextureFilter? magFilter, bool force = false )
    {
        if ( ( minFilter != null ) && ( force || ( MinFilter != minFilter ) ) )
        {
            Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_MIN_FILTER, minFilter.GLEnum );
            MinFilter = minFilter;
        }

        if ( ( magFilter != null ) && ( force || ( MagFilter != magFilter ) ) )
        {
            Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_MAG_FILTER, magFilter.GLEnum );
            MagFilter = magFilter;
        }
    }

    /// <summary>
    /// Sets the <see cref="TextureFilter"/> for this texture for minification and
    /// magnification. This will bind this texture!
    /// </summary>
    /// <param name="minFilter"> the minification filter </param>
    /// <param name="magFilter"> the magnification filter  </param>
    public void SetFilter( TextureFilter minFilter, TextureFilter magFilter )
    {
        MinFilter = minFilter;
        MagFilter = magFilter;

        Bind();

        Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_MIN_FILTER, minFilter.GLEnum );
        Gdx.GL.glTexParameteri( GLTarget, IGL.GL_TEXTURE_MAG_FILTER, magFilter.GLEnum );
    }

    /// <summary>
    /// Sets the anisotropic filter level for the texture.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="level">
    /// The desired level of filtering. The maximum level supported by
    /// the device up to this value will be used.
    /// </param>
    /// <param name="force"> True to force setting of the level. </param>
    /// <returns>
    /// The actual level set, which may be lower than the provided value
    /// due to device limitations.
    /// </returns>
    public float UnsafeSetAnisotropicFilter( float level, bool force = false )
    {
        var max = GetMaxAnisotropicFilterLevel();

        if ( Math.Abs( max - 1f ) < 0.1f )
        {
            return 1f;
        }

        level = Math.Min( level, max );

        if ( !force && MathUtils.IsEqual( level, AnisotropicFilterLevel, 0.1f ) )
        {
            return AnisotropicFilterLevel;
        }

        Gdx.GL.glTexParameterf( IGL.GL_TEXTURE_2D,
                                IGL.GL_TEXTURE_MAX_ANISOTROPY_EXT,
                                level );

        return AnisotropicFilterLevel = level;
    }

    /// <summary>
    /// Sets, and returns, the Anisotropic Filter Level.
    /// </summary>
    /// <param name="level"> The level. </param>
    /// <returns> A float holding the new level. </returns>
    public float SetAnisotropicFilter( float level )
    {
        var max = GetMaxAnisotropicFilterLevel();

        if ( Math.Abs( max - 1f ) < 0.1f )
        {
            return 1f;
        }

        level = Math.Min( level, max );

        if ( MathUtils.IsEqual( level, AnisotropicFilterLevel, 0.1f ) )
        {
            return level;
        }

        Bind();

        Gdx.GL.glTexParameterf( IGL.GL_TEXTURE_2D,
                                IGL.GL_TEXTURE_MAX_ANISOTROPY_EXT,
                                level );

        return AnisotropicFilterLevel = level;
    }

    /// <summary>
    /// Gets the maximum Anisotropic Filter Level, if it is currently &gt; 0.
    /// If it is not, then the level is obtained from OpenGL if the extension
    /// <b> GL_EXT_texture_filter_anisotropic </b> is supported, or 1.0f
    /// is the extension is not supported.
    /// </summary>
    /// <returns></returns>
    public float GetMaxAnisotropicFilterLevel()
    {
        if ( _maxAnisotropicFilterLevel > 0 )
        {
            return _maxAnisotropicFilterLevel;
        }

        if ( Gdx.Graphics.SupportsExtension( "GL_EXT_texture_filter_anisotropic" ) )
        {
            var buffer = new float[ 16 ];

            Gdx.GL.glGetFloatv( IGL.GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, ref buffer );

            return _maxAnisotropicFilterLevel = buffer[ 0 ];
        }

        return _maxAnisotropicFilterLevel = 1f;
    }

    /// <summary>
    /// Delete this GLTexture.
    /// </summary>
    public void Delete()
    {
        if ( GLTextureHandle != 0 )
        {
            Gdx.GL.glDeleteTextures( ( uint ) GLTextureHandle );
            GLTextureHandle = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="data"></param>
    /// <param name="miplevel"></param>
    public static void UploadImageData( int target, ITextureData? data, int miplevel = 0 )
    {
        Logger.CheckPoint();

        if ( data == null )
        {
            // TODO: remove texture on target?
            return;
        }

        Logger.Debug( $"data.Width : {data.Width}" );
        Logger.Debug( $"data.Height: {data.Height}" );
        Logger.Debug( $"data.Format: {data.GetFormat()}" );

        if ( !data.IsPrepared )
        {
            data.Prepare();
        }

        var type = data.TextureDataType;

        if ( type == ITextureData.TextureType.Custom )
        {
            data.ConsumeCustomData( target );

            return;
        }

        var pixmap        = data.ConsumePixmap();
        var disposePixmap = data.ShouldDisposePixmap();

        if ( pixmap == null )
        {
            throw new GdxRuntimeException( "ConsumePixmap() resulted in a null Pixmap!" );
        }

        if ( data.GetFormat() != pixmap.GetFormat() )
        {
            var tmp = new Pixmap( pixmap.Width, pixmap.Height, data.GetFormat() );

            tmp.Blending = Pixmap.BlendTypes.None;
            tmp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

            if ( data.ShouldDisposePixmap() )
            {
                pixmap.Dispose();
            }

            pixmap        = tmp;
            disposePixmap = true;
        }

        Gdx.GL.glPixelStorei( IGL.GL_UNPACK_ALIGNMENT, 1 );

        if ( data.UseMipMaps )
        {
            MipMapGenerator.GenerateMipMap( target, pixmap, pixmap.Width, pixmap.Height );
        }
        else
        {
            if ( Gdx.DevMode )
            {
                Logger.Debug( $"target: {target}" );
                Logger.Debug( $"miplevel: {miplevel}" );
                Logger.Debug( $"pixmap.GLInternalFormat: {pixmap.GLInternalFormat}" );
                Logger.Debug( $"pixmap.Width: {pixmap.Width}" );
                Logger.Debug( $"pixmap.Height: {pixmap.Height}" );
                Logger.Debug( $"pixmap.GLFormat: {pixmap.GLFormat}" );
                Logger.Debug( $"pixmap.GLType: {pixmap.GLType}" );
            }

            var pixels = pixmap.Pixels.BackingArray();

            Gdx.GL.glTexImage2D( target,
                                 miplevel,
                                 pixmap.GLInternalFormat,
                                 pixmap.Width,
                                 pixmap.Height,
                                 border: 0,
                                 pixmap.GLFormat,
                                 pixmap.GLType,
                                 pixels );

            pixmap.Pixels.Put( pixels );

//            unsafe
//            {
//                fixed ( void* ptr = &pixmap.Pixels.BackingArray()[ 0 ] )
//                {
//                    Gdx.GL.glTexImage2D( target,
//                                         miplevel,
//                                         pixmap.GLInternalFormat,
//                                         pixmap.Width,
//                                         pixmap.Height,
//                                         0,
//                                         pixmap.GLFormat,
//                                         pixmap.GLType,
//                                         ptr );
//                }
//            }
        }

        if ( disposePixmap )
        {
            pixmap.Dispose();
        }
    }

    // ------------------------------------------------------------------------

    /// <inheritdoc />
    public virtual void Dispose()
    {
        Dispose( true );
    }

    protected virtual void Dispose( bool disposing )
    {
        if ( disposing )
        {
            Delete();
        }
    }
}