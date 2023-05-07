using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Maths;

using Buffer = Silk.NET.OpenGLES.Buffer;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Class representing an OpenGL texture by its target and handle.
/// Keeps track of its state like the TextureFilter and TextureWrap.
/// Also provides some (protected) static methods to create TextureData
/// and upload image data.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class GLTexture
{
    protected int   GLTarget               { get; set; }
    protected float AnisotropicFilterLevel { get; set; } = 1.0f;

    private TextureFilter _minFilter = TextureFilter.Nearest;
    private TextureFilter _magFilter = TextureFilter.Nearest;
    private TextureWrap   _uWrap     = TextureWrap.ClampToEdge;
    private TextureWrap   _vWrap     = TextureWrap.ClampToEdge;

    private static float _maxAnisotropicFilterLevel = 0;

    /// <returns> the width of the texture in pixels </returns>
    public abstract int Width { get; }

    /// <returns> the height of the texture in pixels </returns>
    public abstract int Height { get; }

    /// <returns> the depth of the texture in pixels </returns>
    public abstract int Depth { get; }

    /// <summary>
    /// Generates a new OpenGL texture with the specified target.
    /// </summary>
    protected GLTexture( int glTarget ) : this( glTarget, Gdx.GL.GLGenTexture() )
    {
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="glTarget"></param>
    /// <param name="glHandle"></param>
    protected GLTexture( int glTarget, int glHandle )
    {
        this.GLTarget  = glTarget;
        this.TextureObjectHandle = glHandle;
    }

    /// <returns>whether this texture is managed or not.</returns>
    public abstract bool IsManaged();

    /// <summary>
    /// </summary>
    protected abstract void Reload();

    /// <summary>
    /// Binds this texture. The texture will be bound to the currently active
    /// texture unit specified via <see cref="IGL20.GLActiveTexture(int)"/>. 
    /// </summary>
    protected void Bind()
    {
        Gdx.GL.GLBindTexture( GLTarget, TextureObjectHandle );
    }

    /// <summary>
    /// Binds the texture to the given texture unit.
    /// <para>
    /// Sets the currently active texture unit via <see cref="IGL20.GLActiveTexture(int)"/>.
    /// </para>
    /// </summary>
    /// <param name="unit"> the unit (0 to MAX_TEXTURE_UNITS).  </param>
    public void Bind( int unit )
    {
        Gdx.GL.GLActiveTexture( IGL20.GL_Texture0 + unit );
        Gdx.GL.GLBindTexture( GLTarget, TextureObjectHandle );
    }

    /// <returns> The <see cref="TextureFilter"/> used for minification. </returns>
    protected TextureFilter MinFilter => _minFilter;

    /// <returns> The <see cref="TextureFilter"/> used for magnification. </returns>
    protected TextureFilter MagFilter => _magFilter;

    /// <returns>
    /// The <see cref="TextureWrap"/> used for horizontal (U) texture coordinates.
    /// </returns>
    protected TextureWrap UWrap => _uWrap;

    /// <returns>
    /// The <see cref="TextureWrap"/> used for vertical (V) texture coordinates.
    /// </returns>
    protected TextureWrap VWrap => _vWrap;

    /// <returns> The OpenGL handle for this texture. </returns>
    protected int TextureObjectHandle { get; set; }

    /// <summary>
    /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="u"> the u wrap </param>
    /// <param name="v"> the v wrap </param>
    /// <param name="force">
    /// True to always set the values, even if they are the same as the current values.
    /// </param>
    protected void UnsafeSetWrap( TextureWrap? u, TextureWrap? v, bool force = false )
    {
        if ( (u != null) && ( force || (_uWrap != u) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_S, u.GLEnum );
            _uWrap = u;
        }

        if ( (v != null) && ( force || (_vWrap != v) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_T, v.GLEnum );
            _vWrap = v;
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
        this._uWrap = u;
        this._vWrap = v;

        Bind();

        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_S, u.GLEnum );
        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_T, v.GLEnum );
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
    protected void UnsafeSetFilter( TextureFilter? minFilter, TextureFilter? magFilter, bool force = false )
    {
        if ( ( minFilter != null ) && ( force || ( this._minFilter != minFilter ) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Min_Filter, minFilter.GLEnum );
            this._minFilter = minFilter;
        }

        if ( ( magFilter != null ) && ( force || ( this._magFilter != magFilter ) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Mag_Filter, magFilter.GLEnum );
            this._magFilter = magFilter;
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
        this._minFilter = minFilter;
        this._magFilter = magFilter;

        Bind();

        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Min_Filter, minFilter.GLEnum );
        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Mag_Filter, magFilter.GLEnum );
    }

    /// <summary>
    /// Sets the anisotropic filter level for the texture.
    /// Assumes the texture is bound and active!
    /// </summary>
    /// <param name="level">
    /// The desired level of filtering. The maximum level supported by
    /// the device up to this value will be used.
    /// </param>
    /// <param name="force"></param>
    /// <returns>
    /// The actual level set, which may be lower than the provided value
    /// due to device limitations.
    /// </returns>
    protected float UnsafeSetAnisotropicFilter( float level, bool force = false )
    {
        var max = GetMaxAnisotropicFilterLevel();

        if ( Math.Abs( max - 1f ) < 0.1f ) return 1f;

        level = Math.Min( level, max );

        if ( !force && MathUtils.IsEqual( level, AnisotropicFilterLevel, 0.1f ) )
        {
            return AnisotropicFilterLevel;
        }

        Gdx.GL20.GLTexParameterf( IGL20.GL_Texture_2D, 
                                  IGL20.GL_Texture_Max_Anisotropy_Ext,
                                  level );

        return AnisotropicFilterLevel = level;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float SetAnisotropicFilter( float level )
    {
        var max = GetMaxAnisotropicFilterLevel();

        if ( Math.Abs( max - 1f ) < 0.1f ) return 1f;

        level = Math.Min( level, max );

        if ( MathUtils.IsEqual( level, AnisotropicFilterLevel, 0.1f ) )
        {
            return level;
        }

        Bind();

        Gdx.GL20.GLTexParameterf( IGL20.GL_Texture_2D, 
                                  IGL20.GL_Texture_Max_Anisotropy_Ext,
                                  level );

        return AnisotropicFilterLevel = level;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static float GetMaxAnisotropicFilterLevel()
    {
        if ( _maxAnisotropicFilterLevel > 0 ) return _maxAnisotropicFilterLevel;

        if ( Gdx.Graphics.SupportsExtension( "GL_EXT_texture_filter_anisotropic" ) )
        {
            FloatBuffer buffer = BufferUtils.NewFloatBuffer( 16 );

            buffer.SetPosition( 0 );
            buffer.SetLimit( buffer.Capacity );

            Gdx.GL20.GLGetFloatv( IGL20.GL_Max_Texture_Max_Anisotropy_Ext, buffer );

            return _maxAnisotropicFilterLevel = buffer.Get( 0 );
        }

        return _maxAnisotropicFilterLevel = 1f;
    }

    /// <summary>
    ///
    /// </summary>
    protected void Delete()
    {
        if ( TextureObjectHandle != 0 )
        {
            Gdx.GL.GLDeleteTexture( TextureObjectHandle );
            TextureObjectHandle = 0;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    /// <param name="data"></param>
    /// <param name="miplevel"></param>
    protected static void UploadImageData( int target, ITextureData? data, int miplevel = 0 )
    {
        if ( data == null )
        {
            // FIXME: remove texture on target?
            return;
        }

        if ( !data.IsPrepared() ) data.Prepare();

        ITextureData.TextureDataType type = data.GetType();

        if ( type == ITextureData.TextureDataType.Custom )
        {
            data.ConsumeCustomData( target );

            return;
        }

        Pixmap pixmap        = data.ConsumePixmap();
        var   disposePixmap = data.DisposePixmap();

        if ( data.GetFormat() != pixmap.PixFormat )
        {
            var tmp = new Pixmap( pixmap.Width, pixmap.Height, data.GetFormat() );

            tmp.Blend = Pixmap.Blending.None;
            tmp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

            if ( data.DisposePixmap() )
            {
                pixmap.Dispose();
            }

            pixmap        = tmp;
            disposePixmap = true;
        }

        Gdx.GL.GLPixelStorei( IGL20.GL_Unpack_Alignment, 1 );

        if ( data.UseMipMaps() )
        {
            MipMapGenerator.GenerateMipMap( target, pixmap, pixmap.Width, pixmap.Height );
        }
        else
        {
            Gdx.GL.GLTexImage2D
                (
                 target,
                 miplevel,
                 pixmap.GLInternalFormat,
                 pixmap.Width,
                 pixmap.Height,
                 0,
                 pixmap.GLFormat,
                 pixmap.GLType,
                 pixmap.Pixels
                );
        }

        if ( disposePixmap ) pixmap.Dispose();
    }

    public void Dispose()
    {
        Delete();
    }
}