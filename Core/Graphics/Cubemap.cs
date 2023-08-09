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

using System.Text;

using LibGDXSharp.Utils;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Wraps a standard OpenGL ES Cubemap. Must be disposed when it is no longer used.
/// </summary>
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class Cubemap : GLTexture
{
    private readonly static Dictionary< IApplication, List< Cubemap >? > ManagedCubemaps = new();

    public static AssetManager? AssetManager { get; set; }

    public ICubemapData Data { get; set; }

    /// <summary>
    /// Construct a Cubemap based on the given CubemapData.
    /// </summary>
    public Cubemap( ICubemapData? data ) : base( IGL20.GL_TEXTURE_CUBE_MAP )
    {
        ArgumentNullException.ThrowIfNull( data );
        
        this.Data = data;
        Load( data );

        if ( data.Managed ) AddManagedCubemap( Gdx.App, this );
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
        : this
            (
             ITextureData.Factory.LoadFromFile( positiveX, useMipMaps ),
             ITextureData.Factory.LoadFromFile( negativeX, useMipMaps ),
             ITextureData.Factory.LoadFromFile( positiveY, useMipMaps ),
             ITextureData.Factory.LoadFromFile( negativeY, useMipMaps ),
             ITextureData.Factory.LoadFromFile( positiveZ, useMipMaps ),
             ITextureData.Factory.LoadFromFile( negativeZ, useMipMaps )
            )
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
        : this
            (
             positiveX == null ? null : new PixmapTextureData( positiveX, null, useMipMaps, false ),
             negativeX == null ? null : new PixmapTextureData( negativeX, null, useMipMaps, false ),
             positiveY == null ? null : new PixmapTextureData( positiveY, null, useMipMaps, false ),
             negativeY == null ? null : new PixmapTextureData( negativeY, null, useMipMaps, false ),
             positiveZ == null ? null : new PixmapTextureData( positiveZ, null, useMipMaps, false ),
             negativeZ == null ? null : new PixmapTextureData( negativeZ, null, useMipMaps, false )
            )
    {
    }

    /// <summary>
    /// Construct a Cubemap with <see cref="Pixmap"/>s for each side of the specified size.
    /// </summary>
    public Cubemap( int width, int height, int depth, Pixmap.Format format )
        : this
            (
             new PixmapTextureData( new Pixmap( depth, height, format ), null, false, true ),
             new PixmapTextureData( new Pixmap( depth, height, format ), null, false, true ),
             new PixmapTextureData( new Pixmap( width, depth, format ), null, false, true ),
             new PixmapTextureData( new Pixmap( width, depth, format ), null, false, true ),
             new PixmapTextureData( new Pixmap( width, height, format ), null, false, true ),
             new PixmapTextureData( new Pixmap( width, height, format ), null, false, true )
            )
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
    /// Sets the sides of this cubemap to the specified <see cref="ICubemapData"/>.
    /// </summary>
    public void Load( ICubemapData data )
    {
        if ( !data.Prepared ) data.Prepare();

        Bind();

        UnsafeSetFilter( MinFilter, MagFilter, true );
        UnsafeSetWrap( UWrap, VWrap, true );
        UnsafeSetAnisotropicFilter( AnisotropicFilterLevel, true );

        data.ConsumeCubemapData();

        Gdx.GL.GLBindTexture( GLTarget, 0 );
    }

    public override bool IsManaged() => Data.Managed;

    public override void Reload()
    {
        if ( !IsManaged() ) throw new GdxRuntimeException( "Tried to reload an unmanaged Cubemap" );

        GLHandle = Gdx.GL.GLGenTexture();

        Load( Data );
    }

    public override int Width  => Data.Width;
    public override int Height => Data.Height;
    public override int Depth  => 0;

    /// <summary>
    /// Disposes all resources associated with the cubemap.
    /// </summary>
    public new void Dispose()
    {
        // this is a hack. reason: we have to set the glHandle to 0 for textures that are
        // reloaded through the asset manager as we first remove (and thus dispose) the texture
        // and then reload it. the glHandle is set to 0 in invalidateAllTextures prior to
        // removal from the asset manager.
        if ( GLHandle == 0 ) return;

        Delete();

        if ( Data.Managed )
        {
            if ( ManagedCubemaps[ Gdx.App ] != null )
            {
                ManagedCubemaps[ Gdx.App ]?.Remove( this );
            }
        }
    }

    private static void AddManagedCubemap( IApplication app, Cubemap cubemap )
    {
        List< Cubemap > managedCubemapArray = ManagedCubemaps[ app ] ?? new List< Cubemap >();

        managedCubemapArray.Add( cubemap );
        ManagedCubemaps.Put( app, managedCubemapArray );
    }

    /// <summary>
    /// Clears all managed cubemaps. This is an internal method. Do not use it!
    /// </summary>
    public static void ClearAllCubemaps( IApplication app )
    {
        ManagedCubemaps.Remove( app );
    }

    /// <summary>
    /// Invalidate all managed cubemaps. This is an internal method. Do not use it!
    /// </summary>
    public static void InvalidateAllCubemaps( IApplication app )
    {
        List< Cubemap >? managedCubemapArray = ManagedCubemaps[ app ];

        if ( managedCubemapArray == null ) return;

        if ( AssetManager == null )
        {
            foreach ( Cubemap cubemap in managedCubemapArray )
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

            foreach ( Cubemap cubemap in cubemaps )
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

                    cubemap.GLHandle = 0;

                    // create the parameters, passing the reference to the cubemap as
                    // well as a callback that sets the ref count.
                    var parameter = new CubemapLoader.CubemapParameter
                    {
                        cubemapData = cubemap.Data,
                        minFilter   = cubemap.MinFilter,
                        magFilter   = cubemap.MagFilter,
                        wrapU       = cubemap.UWrap,
                        wrapV       = cubemap.VWrap,
                        // special parameter which will ensure that the references stay the same.
                        cubemap        = cubemap,
                        LoadedCallback = new DefaultLoadedCallbackInnerClass( refCount )
                    };

                    // unload the c, create a new gl handle then reload it.
                    AssetManager.Unload( fileName );
                    cubemap.GLHandle = Gdx.GL.GLGenTexture();
                    AssetManager.Load( fileName, typeof( Cubemap ), parameter );
                }
            }

            managedCubemapArray.Clear();
            managedCubemapArray.AddAll( cubemaps );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static string GetManagedStatus()
    {
        var builder = new StringBuilder();

        builder.Append( "Managed cubemap/app: { " );

        foreach ( IApplication app in ManagedCubemaps.Keys )
        {
            builder.Append( ManagedCubemaps[ app ]!.Count );
            builder.Append( ' ' );
        }

        builder.Append( '}' );

        return builder.ToString();
    }

    /// <summary>
    /// return the number of managed cubemaps currently loaded
    /// </summary>
    public static int NumManagedCubemaps => ManagedCubemaps[ Gdx.App ]?.Count ?? 0;

    /// <summary>
    /// Enum to identify each side of a Cubemap </summary>
    public class CubemapSide
    {
        /// <summary>
        /// The positive X and first side of the cubemap
        /// </summary>
        public readonly static CubemapSide PositiveX = new
            ( "PositiveX", InnerEnum.PositiveX, 0, IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_X, 0, -1, 0, 1, 0, 0 );
        /// <summary>
        /// The negative X and second side of the cubemap
        /// </summary>
        public readonly static CubemapSide NegativeX = new
            ( "NegativeX", InnerEnum.NegativeX, 1, IGL20.GL_TEXTURE_CUBE_MAP_NEGATIVE_X, 0, -1, 0, -1, 0, 0 );
        /// <summary>
        /// The positive Y and third side of the cubemap
        /// </summary>
        public readonly static CubemapSide PositiveY = new
            ( "PositiveY", InnerEnum.PositiveY, 2, IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_Y, 0, 0, 1, 0, 1, 0 );
        /// <summary>
        /// The negative Y and fourth side of the cubemap
        /// </summary>
        public readonly static CubemapSide NegativeY = new
            ( "NegativeY", InnerEnum.NegativeY, 3, IGL20.GL_TEXTURE_CUBE_MAP_NEGATIVE_Y, 0, 0, -1, 0, -1, 0 );
        /// <summary>
        /// The positive Z and fifth side of the cubemap
        /// </summary>
        public readonly static CubemapSide PositiveZ = new
            ( "PositiveZ", InnerEnum.PositiveZ, 4, IGL20.GL_TEXTURE_CUBE_MAP_POSITIVE_Z, 0, -1, 0, 0, 0, 1 );
        /// <summary>
        /// The negative Z and sixth side of the cubemap
        /// </summary>
        public readonly static CubemapSide NegativeZ = new
            ( "NegativeZ", InnerEnum.NegativeZ, 5, IGL20.GL_TEXTURE_CUBE_MAP_NEGATIVE_Z, 0, -1, 0, 0, 0, -1 );

        private readonly static List< CubemapSide > ValueList = new();

        static CubemapSide()
        {
            ValueList.Add( PositiveX );
            ValueList.Add( NegativeX );
            ValueList.Add( PositiveY );
            ValueList.Add( NegativeY );
            ValueList.Add( PositiveZ );
            ValueList.Add( NegativeZ );
        }

        public enum InnerEnum
        {
            PositiveX,
            NegativeX,
            PositiveY,
            NegativeY,
            PositiveZ,
            NegativeZ
        }

        public InnerEnum InnerEnumValue { get; private set; }
        public int       OrdinalValue   { get; private set; }

        private readonly string _nameValue;
        private static   int    _nextOrdinal = 0;

        /// <summary>
        /// The zero based index of the side in the cubemap </summary>
        public int Index { get; set; }

        /// <summary>
        /// The OpenGL target (used for glTexImage2D) of the side. </summary>
        public int GLEnum { get; set; }

        /// <summary>
        /// The up vector to target the side.
        /// </summary>
        public Vector3 Up { get; set; }

        /// <summary>
        /// The direction vector to target the side.
        /// </summary>
        public Vector3 Direction { get; set; }

        public CubemapSide( string name,
                            InnerEnum innerEnum,
                            int index,
                            int glEnum,
                            float upX,
                            float upY,
                            float upZ,
                            float directionX,
                            float directionY,
                            float directionZ )
        {
            this.Index     = index;
            this.GLEnum    = glEnum;
            this.Up        = new Vector3( upX, upY, upZ );
            this.Direction = new Vector3( directionX, directionY, directionZ );

            _nameValue     = name;
            OrdinalValue   = _nextOrdinal++;
            InnerEnumValue = innerEnum;
        }

        /// <summary>
        /// Sets the supplied <see cref="Vector3"/> to the contents of <see cref="Up"/>
        /// and returns it to the caller. 
        /// </summary>
        public Vector3 GetUp( ref Vector3 vec3 )
        {
            return vec3.Set( Up );
        }

        /// <summary>
        /// Sets the supplied <see cref="Vector3"/> to the contents of <see cref="Direction"/>
        /// and returns it to the caller. 
        /// </summary>
        public Vector3 GetDirection( Vector3 vec3 )
        {
            return vec3.Set( Direction );
        }

        public static CubemapSide[] Values()
        {
            return ValueList.ToArray();
        }

        public override string ToString()
        {
            return _nameValue;
        }

        public static CubemapSide ValueOf( string name )
        {
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach ( CubemapSide enumInstance in ValueList )
            {
                if ( enumInstance._nameValue == name )
                {
                    return enumInstance;
                }
            }

            throw new System.ArgumentException( name );
        }
    }
}
