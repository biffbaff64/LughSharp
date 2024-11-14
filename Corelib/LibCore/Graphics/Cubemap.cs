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
using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils;
using Corelib.LibCore.Utils.Collections;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics;

/// <summary>
/// Wraps a standard OpenGL ES Cubemap. Must be disposed when it is no longer used.
/// </summary>
[PublicAPI]
public class Cubemap : GLTexture, IManageable
{
    public static AssetManager? AssetManager { get; set; }
    public        ICubemapData  Data         { get; set; }

    public override int Width  => Data.Width;
    public override int Height => Data.Height;
    public override int Depth  => 0;

    public bool IsManaged
    {
        get => Data.IsManaged;
        set { }
    }

    // ========================================================================

    private static readonly Dictionary< IApplication, List< Cubemap >? > _managedCubemaps = new();

    // ========================================================================

    /// <summary>
    /// Construct a Cubemap based on the given CubemapData.
    /// </summary>
    public Cubemap( ICubemapData? data ) : base( IGL.GL_TEXTURE_CUBE_MAP )
    {
        ArgumentNullException.ThrowIfNull( data );

        Data = data;

        Load( data );

        if ( data.IsManaged )
        {
            AddManagedCubemap( Gdx.App, this );
        }
    }

    /// <summary>
    /// Construct a Cubemap with the specified texture files for the sides,
    /// optionally generating mipmaps (Default is do not generate mipmaps).
    /// </summary>
    public Cubemap( FileInfo positiveX,
                    FileInfo negativeX,
                    FileInfo positiveY,
                    FileInfo negativeY,
                    FileInfo positiveZ,
                    FileInfo negativeZ,
                    bool useMipMaps = false )
        : this( TextureDataFactory.LoadFromFile( positiveX, useMipMaps ),
                TextureDataFactory.LoadFromFile( negativeX, useMipMaps ),
                TextureDataFactory.LoadFromFile( positiveY, useMipMaps ),
                TextureDataFactory.LoadFromFile( negativeY, useMipMaps ),
                TextureDataFactory.LoadFromFile( positiveZ, useMipMaps ),
                TextureDataFactory.LoadFromFile( negativeZ, useMipMaps ) )
    {
    }

    /// <summary>
    /// Construct a Cubemap with the specified <see cref="Pixmap"/>s for the sides,
    /// optionally generating mipmaps.
    /// </summary>
    public Cubemap( Pixmap? positiveX,
                    Pixmap? negativeX,
                    Pixmap? positiveY,
                    Pixmap? negativeY,
                    Pixmap? positiveZ,
                    Pixmap? negativeZ,
                    bool useMipMaps = false )
        : this( positiveX == null ? null : new PixmapTextureData( positiveX, null, useMipMaps, false ),
                negativeX == null ? null : new PixmapTextureData( negativeX, null, useMipMaps, false ),
                positiveY == null ? null : new PixmapTextureData( positiveY, null, useMipMaps, false ),
                negativeY == null ? null : new PixmapTextureData( negativeY, null, useMipMaps, false ),
                positiveZ == null ? null : new PixmapTextureData( positiveZ, null, useMipMaps, false ),
                negativeZ == null ? null : new PixmapTextureData( negativeZ, null, useMipMaps, false ) )
    {
    }

    /// <summary>
    /// Construct a Cubemap with <see cref="Pixmap"/>s for each side of the specified size.
    /// </summary>
    public Cubemap( int width, int height, int depth, Pixmap.ColorFormat format )
        : this( new PixmapTextureData( new Pixmap( depth, height, format ), null, false, true ),
                new PixmapTextureData( new Pixmap( depth, height, format ), null, false, true ),
                new PixmapTextureData( new Pixmap( width, depth, format ), null, false, true ),
                new PixmapTextureData( new Pixmap( width, depth, format ), null, false, true ),
                new PixmapTextureData( new Pixmap( width, height, format ), null, false, true ),
                new PixmapTextureData( new Pixmap( width, height, format ), null, false, true ) )
    {
    }

    /// <summary>
    /// Construct a Cubemap with the specified <see cref="ITextureData"/>'s for the sides
    /// </summary>
    public Cubemap( ITextureData? positiveX,
                    ITextureData? negativeX,
                    ITextureData? positiveY,
                    ITextureData? negativeY,
                    ITextureData? positiveZ,
                    ITextureData? negativeZ )
        : this( new FacedCubemapData( positiveX, negativeX, positiveY, negativeY, positiveZ, negativeZ ) )
    {
    }

    /// <summary>
    /// return the number of managed cubemaps currently loaded
    /// </summary>
    public static int NumManagedCubemaps => _managedCubemaps[ Gdx.App ]?.Count ?? 0;

    /// <summary>
    /// Sets the sides of this cubemap to the specified <see cref="ICubemapData"/>.
    /// </summary>
    public void Load( ICubemapData data )
    {
        if ( !data.IsPrepared )
        {
            data.Prepare();
        }

        Bind();

        UnsafeSetFilter( MinFilter, MagFilter, true );
        UnsafeSetWrap( UWrap, VWrap, true );
        UnsafeSetAnisotropicFilter( AnisotropicFilterLevel, true );

        data.ConsumeCubemapData();

        Gdx.GL.glBindTexture( GLTarget, 0 );
    }

    /// <summary>
    /// Used internally to reload after context loss. Creates a new GL handle then
    /// calls <see cref="Load(ICubemapData?)"/>.
    /// </summary>
    public override void Reload()
    {
        if ( !IsManaged )
        {
            throw new GdxRuntimeException( "Tried to reload an unmanaged Cubemap" );
        }

        GLTextureHandle = Gdx.GL.glGenTexture();

        Load( Data );
    }

    /// <summary>
    /// Adds a new entry to the list of managed cubemnaps.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="cubemap"></param>
    private static void AddManagedCubemap( IApplication app, Cubemap cubemap )
    {
        var managedCubemapArray = _managedCubemaps[ app ] ?? new List< Cubemap >();

        managedCubemapArray.Add( cubemap );
        _managedCubemaps.Put( app, managedCubemapArray );
    }

    /// <summary>
    /// Clears all managed cubemaps.
    /// </summary>
    public static void ClearAllCubemaps( IApplication app )
    {
        _managedCubemaps.Remove( app );
    }

    /// <summary>
    /// Invalidate all managed cubemaps. This is an internal method. Do not use it!
    /// </summary>
    public static void InvalidateAllCubemaps( IApplication app )
    {
        var managedCubemapArray = _managedCubemaps[ app ];

        if ( managedCubemapArray == null )
        {
            return;
        }

        if ( AssetManager == null )
        {
            foreach ( var cubemap in managedCubemapArray )
            {
                cubemap.Reload();
            }
        }
        else
        {
            // first we have to make sure the AssetManager isn't loading anything anymore,
            // otherwise the ref counting trick below wouldn't work (when a cubemap is
            // currently on the task stack of the manager.)
            AssetManager.FinishLoading();

            // next we go through each cubemap and reload either directly or via the
            // asset manager.
            var cubemaps = new List< Cubemap >( managedCubemapArray );

            foreach ( var cubemap in cubemaps )
            {
                var fileName = AssetManager.GetAssetFileName( cubemap );

                if ( fileName == null )
                {
                    cubemap.Reload();
                }
                else
                {
                    // get the ref count of the cubemap, then set it to 0 so we
                    // can actually remove it from the assetmanager. Also set the
                    // handle to zero, otherwise we might accidentially dispose
                    // already reloaded cubemaps.
                    var refCount = AssetManager.GetReferenceCount( fileName );

                    AssetManager.SetReferenceCount( fileName, 0 );

                    cubemap.GLTextureHandle = 0;

                    // create the parameters, passing the reference to the cubemap as
                    // well as a callback that sets the ref count.
                    var parameter = new CubemapLoader.CubemapParameter
                    {
                        CubemapData = cubemap.Data,
                        MinFilter   = cubemap.MinFilter,
                        MagFilter   = cubemap.MagFilter,
                        WrapU       = cubemap.UWrap,
                        WrapV       = cubemap.VWrap,

                        // special parameter which will ensure that the references stay the same.
                        Cubemap        = cubemap,
                        LoadedCallback = new DefaultLoadedCallback( refCount ),
                    };

                    // unload the c, create a new gl handle then reload it.
                    AssetManager.Unload( fileName );

                    cubemap.GLTextureHandle = Gdx.GL.glGenTexture();
                    AssetManager.AddToLoadqueue( fileName, typeof( Cubemap ), parameter );
                }
            }

            managedCubemapArray.Clear();
            managedCubemapArray.AddAll( cubemaps );
        }
    }

    /// <summary>
    /// Retuens a string describing the managed status of this Cubemap.
    /// </summary>
    public static string GetManagedStatus()
    {
        var builder = new StringBuilder( "Managed cubemap/app: { " );

        foreach ( var app in _managedCubemaps.Keys )
        {
            builder.Append( _managedCubemaps[ app ]!.Count );
            builder.Append( ' ' );
        }

        builder.Append( '}' );

        return builder.ToString();
    }

    /// <summary>
    /// Disposes all resources associated with the cubemap.
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

            if ( Data.IsManaged )
            {
                if ( _managedCubemaps[ Gdx.App ] != null )
                {
                    _managedCubemaps[ Gdx.App ]?.Remove( this );
                }
            }
        }
    }

    // ========================================================================

    #region cubemapside

    /// <summary>
    /// Enum to identify each side of a Cubemap
    /// </summary>
    [PublicAPI]
    public class CubemapSide
    {
        public enum InnerEnum
        {
            PositiveX,
            NegativeX,
            PositiveY,
            NegativeY,
            PositiveZ,
            NegativeZ,
        }

        // ====================================================================

        /// <summary>
        /// The positive X and first side of the cubemap
        /// </summary>
        public static readonly CubemapSide PositiveX =
            new( "PositiveX", InnerEnum.PositiveX, 0, IGL.GL_TEXTURE_CUBE_MAP_POSITIVE_X, 0, -1, 0, 1, 0, 0 );

        /// <summary>
        /// The negative X and second side of the cubemap
        /// </summary>
        public static readonly CubemapSide NegativeX =
            new( "NegativeX", InnerEnum.NegativeX, 1, IGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_X, 0, -1, 0, -1, 0, 0 );

        /// <summary>
        /// The positive Y and third side of the cubemap
        /// </summary>
        public static readonly CubemapSide PositiveY =
            new( "PositiveY", InnerEnum.PositiveY, 2, IGL.GL_TEXTURE_CUBE_MAP_POSITIVE_Y, 0, 0, 1, 0, 1, 0 );

        /// <summary>
        /// The negative Y and fourth side of the cubemap
        /// </summary>
        public static readonly CubemapSide NegativeY =
            new( "NegativeY", InnerEnum.NegativeY, 3, IGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, 0, 0, -1, 0, -1, 0 );

        /// <summary>
        /// The positive Z and fifth side of the cubemap
        /// </summary>
        public static readonly CubemapSide PositiveZ =
            new( "PositiveZ", InnerEnum.PositiveZ, 4, IGL.GL_TEXTURE_CUBE_MAP_POSITIVE_Z, 0, -1, 0, 0, 0, 1 );

        /// <summary>
        /// The negative Z and sixth side of the cubemap
        /// </summary>
        public static readonly CubemapSide NegativeZ =
            new( "NegativeZ", InnerEnum.NegativeZ, 5, IGL.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, 0, -1, 0, 0, 0, -1 );

        public InnerEnum InnerEnumValue { get; private set; }
        public int       OrdinalValue   { get; private set; }

        /// <summary>
        /// The zero based index of the side in the cubemap
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// The OpenGL target (used for glTexImage2D) of the side.
        /// </summary>
        public int GLTarget { get; set; }

        /// <summary>
        /// The up vector to target the side.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// The direction vector to target the side.
        /// </summary>
        public Vector3 Direction { get; set; }

        // ====================================================================

        private static List< CubemapSide > _valueList   = new();
        private static int                 _nextOrdinal = 0;

        private string _nameValue;

        // ====================================================================

        static CubemapSide()
        {
            _valueList.Add( PositiveX );
            _valueList.Add( NegativeX );
            _valueList.Add( PositiveY );
            _valueList.Add( NegativeY );
            _valueList.Add( PositiveZ );
            _valueList.Add( NegativeZ );
        }

        public CubemapSide( string name,
                            InnerEnum innerEnum,
                            int index,
                            int glTarget,
                            float upX,
                            float upY,
                            float upZ,
                            float directionX,
                            float directionY,
                            float directionZ )
        {
            Index     = index;
            GLTarget  = glTarget;
            Up        = new Vector3( upX, upY, upZ );
            Direction = new Vector3( directionX, directionY, directionZ );

            _nameValue     = name;
            OrdinalValue   = _nextOrdinal++;
            InnerEnumValue = innerEnum;
        }

        /// <summary>
        /// Sets the supplied <see cref="Vector3"/> to the contents of
        /// <see cref="Up"/> and returns it to the caller.
        /// </summary>
        public Vector3 GetUp( Vector3 vec3 )
        {
            return vec3.Set( Up );
        }

        /// <summary>
        /// Sets the supplied <see cref="Vector3"/> to the contents of
        /// <see cref="Direction"/> and returns it to the caller.
        /// </summary>
        public Vector3 GetDirection( Vector3 vec3 )
        {
            return vec3.Set( Direction );
        }

        /// <summary>
        /// Returns an array of all <see cref="CubemapSide"/> values.
        /// </summary>
        public static CubemapSide[] Values()
        {
            return _valueList.ToArray();
        }

        /// <summary>
        /// Returns the <see cref="CubemapSide"/> with the specified name.
        /// </summary>
        /// <param name="name">The name of the cubemap side.</param>
        /// <returns>The <see cref="CubemapSide"/> with the specified name.</returns>
        /// <exception cref="ArgumentException">Thrown if no cubemap side with the specified name exists.</exception>
        public static CubemapSide ValueOf( string name )
        {
            foreach ( var enumInstance in _valueList )
            {
                if ( enumInstance._nameValue == name )
                {
                    return enumInstance;
                }
            }

            throw new ArgumentException( name );
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return _nameValue;
        }
    }

    #endregion cubemapside
}