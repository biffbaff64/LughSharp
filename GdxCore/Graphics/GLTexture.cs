namespace LibGDXSharp.Graphics
{
    /// <summary>
    /// Class representing an OpenGL texture by its target and handle.
    /// Keeps track of its state like the TextureFilter and TextureWrap.
    /// Also provides some (protected) static methods to create TextureData
    /// and upload image data.
    /// </summary>
    public abstract class GLTexture
    {
        public int GLTarget { get; set; }

        protected int           glHandle;
        protected TextureFilter minFilter              = TextureFilter.Nearest;
        protected TextureFilter magFilter              = TextureFilter.Nearest;
        protected TextureWrap   uWrap                  = TextureWrap.ClampToEdge;
        protected TextureWrap   vWrap                  = TextureWrap.ClampToEdge;
        protected float         anisotropicFilterLevel = 1.0f;

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
        protected GLTexture( int glTarget )
            : this( glTarget, Gdx.GL.GLGenTexture() )
        {
        }

        protected GLTexture( int glTarget, int glHandle )
        {
            this.GLTarget = glTarget;
            this.glHandle = glHandle;
        }

        /// <returns>whether this texture is managed or not.</returns>
        public abstract bool IsManaged();

        protected abstract void Reload();

        /// <summary>
        /// Binds this texture. The texture will be bound to the currently active
        /// texture unit specified via <see cref="IGL20.GLActiveTexture(int)"/>. 
        /// </summary>
        public void Bind()
        {
            Gdx.GL.GLBindTexture( GLTarget, glHandle );
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
            Gdx.GL.GLBindTexture( GLTarget, glHandle );
        }

        /// <returns> The <see cref="TextureFilter"/> used for minification. </returns>
        public TextureFilter MinFilter => minFilter;

        /// <returns> The <see cref="TextureFilter"/> used for magnification. </returns>
        public TextureFilter MagFilter => magFilter;

        /// <returns> The <see cref="TextureWrap"/> used for horizontal (U) texture coordinates. </returns>
        public TextureWrap UWrap => uWrap;

        /// <returns> The <see cref="TextureWrap"/> used for vertical (V) texture coordinates. </returns>
        public TextureWrap VWrap => vWrap;

        /// <returns> The OpenGL handle for this texture. </returns>
        public int TextureObjectHandle => glHandle;

        /// <summary>
        /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis. Assumes the texture is bound and active! </summary>
        /// <param name="u"> the u wrap </param>
        /// <param name="v"> the v wrap  </param>
        public void UnsafeSetWrap( TextureWrap u, TextureWrap v )
        {
            UnsafeSetWrap( u, v, false );
        }

        /// <summary>
        /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis. Assumes the texture is bound and active! </summary>
        /// <param name="u"> the u wrap </param>
        /// <param name="v"> the v wrap </param>
        /// <param name="force"> True to always set the values, even if they are the same as the current values.  </param>
        public void UnsafeSetWrap( TextureWrap u, TextureWrap v, bool force )
        {
            if ( u != null && ( force || uWrap != u ) )
            {
                Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_WRAP_S, u.getGLEnum() );
                uWrap = u;
            }

            if ( v != null && ( force || vWrap != v ) )
            {
                Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_WRAP_T, v.getGLEnum() );
                vWrap = v;
            }
        }

        /// <summary>
        /// Sets the <see cref="TextureWrap"/> for this texture on the u and v axis.
        /// This will bind this texture!
        /// </summary>
        /// <param name="u"> the u wrap </param>
        /// <param name="v"> the v wrap  </param>
        public void SetWrap( TextureWrap u, TextureWrap v )
        {
            this.uWrap = u;
            this.vWrap = v;
            
            Bind();
            
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_S, u.GetGLEnum() );
            Gdx.GL.GLTexParameteri( GLTarget, IGL20.GL_Texture_Wrap_T, v.GetGLEnum() );
        }

        /// <summary>
        /// Sets the <see cref="TextureFilter"/> for this texture for minification and magnification. Assumes the texture is bound and active! </summary>
        /// <param name="minFilter"> the minification filter </param>
        /// <param name="magFilter"> the magnification filter  </param>
        public void UnsafeSetFilter( TextureFilter minFilter, TextureFilter magFilter )
        {
            UnsafeSetFilter( minFilter, magFilter, false );
        }

        /// <summary>
        /// Sets the <see cref="TextureFilter"/> for this texture for minification and magnification. Assumes the texture is bound and active! </summary>
        /// <param name="minFilter"> the minification filter </param>
        /// <param name="magFilter"> the magnification filter </param>
        /// <param name="force"> True to always set the values, even if they are the same as the current values.  </param>
        public void UnsafeSetFilter( TextureFilter minFilter, TextureFilter magFilter, bool force )
        {
            if ( minFilter != null && ( force || this.minFilter != minFilter ) )
            {
                Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_MIN_FILTER, minFilter.getGLEnum() );
                this.minFilter = minFilter;
            }

            if ( magFilter != null && ( force || this.magFilter != magFilter ) )
            {
                Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_MAG_FILTER, magFilter.getGLEnum() );
                this.magFilter = magFilter;
            }
        }

        /// <summary>
        /// Sets the <see cref="TextureFilter"/> for this texture for minification and magnification. This will bind this texture! </summary>
        /// <param name="minFilter"> the minification filter </param>
        /// <param name="magFilter"> the magnification filter  </param>
        public void SetFilter( TextureFilter minFilter, TextureFilter magFilter )
        {
            this.minFilter = minFilter;
            this.magFilter = magFilter;
            bind();
            Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_MIN_FILTER, minFilter.getGLEnum() );
            Gdx.gl.glTexParameteri( glTarget, IGL20.GL_TEXTURE_MAG_FILTER, magFilter.getGLEnum() );
        }

        /// <summary>
        /// Sets the anisotropic filter level for the texture. Assumes the texture is bound and active!
        /// </summary>
        /// <param name="level"> The desired level of filtering. The maximum level supported by the device up to this value will be used. </param>
        /// <returns> The actual level set, which may be lower than the provided value due to device limitations. </returns>
        public float UnsafeSetAnisotropicFilter( float level )
        {
            return UnsafeSetAnisotropicFilter( level, false );
        }
    }
}
