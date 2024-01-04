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

using LibGDXSharp.Files.Buffers;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Class representing an OpenGL texture by its target and handle. Keeps track of
/// its state like the TextureFilter and TextureWrap. Also provides some static
/// methods to create TextureData and upload image data.
/// </summary>
[PublicAPI]
public abstract class GLTexture : IDisposable
{
    public virtual int Width  { get; }
    public virtual int Height { get; }
    public virtual int Depth  { get; }

    public int   GLTextureHandle        { get; set; }
    public int   GLTarget               { get; set; }
    public float AnisotropicFilterLevel { get; private set; } = 1.0f;

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

    private static float _maxAnisotropicFilterLevel = 0;

    // ------------------------------------------------------------------------

    protected GLTexture( int glTarget ) : this( glTarget, Gdx.GL.GLGenTexture() )
    {
    }

    protected GLTexture( int glTarget, int glTextureHandle )
    {
        this.GLTarget        = glTarget;
        this.GLTextureHandle = glTextureHandle;
    }

    public virtual bool IsManaged => false;

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Texture.Load"/>.
    /// </summary>
    protected abstract void Reload();

    /// <summary>
    /// Binds this texture. The texture will be bound to the currently active
    /// texture unit specified via <see cref="IGL20.GLActiveTexture"/>. 
    /// </summary>
    public void Bind()
    {
        Gdx.GL.GLBindTexture( GLTarget, GLTextureHandle );
    }

    /// <summary>
    /// Binds the texture to the given texture unit.
    /// <para>
    /// Sets the currently active texture unit via <see cref="IGL20.GLActiveTexture"/>.
    /// </para>
    /// </summary>
    /// <param name="unit"> the unit (0 to MAX_TEXTURE_UNITS).  </param>
    public void Bind( int unit )
    {
        Gdx.GL.GLActiveTexture( IGL20.GL_TEXTURE0 + unit );
        Gdx.GL.GLBindTexture( GLTarget, GLTextureHandle );
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
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_WRAP_S, u.GLEnum );
            UWrap = u;
        }

        if ( ( v != null ) && ( force || ( VWrap != v ) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_WRAP_T, v.GLEnum );
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
        this.UWrap = u;
        this.VWrap = v;

        Bind();

        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_WRAP_S, u.GLEnum );
        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_WRAP_T, v.GLEnum );
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
        if ( ( minFilter != null ) && ( force || ( this.MinFilter != minFilter ) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_MIN_FILTER, minFilter.GLEnum );
            this.MinFilter = minFilter;
        }

        if ( ( magFilter != null ) && ( force || ( this.MagFilter != magFilter ) ) )
        {
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_MAG_FILTER, magFilter.GLEnum );
            this.MagFilter = magFilter;
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
        this.MinFilter = minFilter;
        this.MagFilter = magFilter;

        Bind();

        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_MIN_FILTER, minFilter.GLEnum );
        Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_TEXTURE_MAG_FILTER, magFilter.GLEnum );
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

        Gdx.GL20.GLTexParameterf( IGL20.GL_TEXTURE_2D,
                                  IGL20.GL_TEXTURE_MAX_ANISOTROPY_EXT,
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

        Gdx.GL20.GLTexParameterf( IGL20.GL_TEXTURE_2D,
                                  IGL20.GL_TEXTURE_MAX_ANISOTROPY_EXT,
                                  level );

        return AnisotropicFilterLevel = level;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public float GetMaxAnisotropicFilterLevel()
    {
        if ( _maxAnisotropicFilterLevel > 0 )
        {
            return _maxAnisotropicFilterLevel;
        }

//TODO:
//        if ( Gdx.Graphics.SupportsExtension( "GL_EXT_texture_filter_anisotropic" ) )
//        {
//            FloatBuffer buffer = BufferUtils.NewFloatBuffer( 16 );
//            buffer.SetPosition( 0 );
//            buffer.SetLimit( buffer.Capacity );
//
//            Gdx.GL20.GLGetFloatv( ( GetPName )34046, out buffer ); // IGL20.GL_MAX_TEXTURE_MAX_ANISOTROPY_EXT, buffer );
//
//            return _maxAnisotropicFilterLevel = buffer.Get( 0 );
//        }

        return _maxAnisotropicFilterLevel = 1f;
    }

    /// <summary>
    ///
    /// </summary>
    public void Delete()
    {
        if ( GLTextureHandle != 0 )
        {
            Gdx.GL.GLDeleteTextures( GLTextureHandle );
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
        if ( data == null )
        {
            // TODO: remove texture on target?
            return;
        }

        if ( !data.IsPrepared )
        {
            data.Prepare();
        }

        ITextureData.TextureType type = data.TextureDataType;

        if ( type == ITextureData.TextureType.Custom )
        {
            data.ConsumeCustomData( target );

            return;
        }

        Pixmap? pixmap        = data.ConsumePixmap();
        var     disposePixmap = data.DisposePixmap();

        if ( pixmap == null )
        {
            throw new GdxRuntimeException( "ConsumePixmap() resulted in a null Pixmap!" );
        }

        if ( data.GetFormat() != pixmap.GetFormat() )
        {
            var tmp = new Pixmap( pixmap.Width, pixmap.Height, data.GetFormat() );

            tmp.Blending = Pixmap.BlendTypes.None;
            tmp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

            if ( data.DisposePixmap() )
            {
                pixmap.Dispose();
            }

            pixmap        = tmp;
            disposePixmap = true;
        }

        Gdx.GL.GLPixelStorei( IGL20.GL_UNPACK_ALIGNMENT, 1 );

        if ( data.UseMipMaps )
        {
            MipMapGenerator.GenerateMipMap( target, pixmap, pixmap.Width, pixmap.Height );
        }
        else
        {
            Gdx.GL.GLTexImage2D( target,
                                 miplevel,
                                 pixmap.GLInternalFormat,
                                 pixmap.Width,
                                 pixmap.Height,
                                 0,
                                 pixmap.GLFormat,
                                 pixmap.GLType,
                                 pixmap.Pixels );
        }

        if ( disposePixmap )
        {
            pixmap.Dispose();
        }
    }

    /// <summary>
    /// Convenience method for when 'GLHandle' isn't descriptive enough.
    /// </summary>
    public int GetTextureObjectHandle() => GLTextureHandle;

    /// <inheritdoc cref="IDisposable.Dispose"/>>
    public virtual void Dispose()
    {
        Delete();
    }
}
