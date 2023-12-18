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

namespace LibGDXSharp.Graphics;

/// <summary>
/// Open GLES wrapper for TextureArray.
/// </summary>
[PublicAPI]
public class TextureArray : GLTexture
{
    private readonly static Dictionary< IApplication, List< TextureArray > > ManagedTextureArrays = new();

    private ITextureArrayData _data;

    #region constructors
    
    /// <summary>
    /// </summary>
    /// <param name="internalPaths"></param>
    public TextureArray( params string[] internalPaths )
        : this( GetInternalHandles( internalPaths ) )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="files"></param>
    public TextureArray( params FileInfo[] files )
        : this( false, files )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="useMipMaps"></param>
    /// <param name="files"></param>
    public TextureArray( bool useMipMaps, params FileInfo[] files )
        : this( useMipMaps, Pixmap.Format.RGBA8888, files )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="useMipMaps"></param>
    /// <param name="format"></param>
    /// <param name="files"></param>
    public TextureArray( bool useMipMaps, Pixmap.Format format, params FileInfo[] files )
        : this( TextureArrayDataFactory.LoadFromFiles( format, useMipMaps, files ) )
    {
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public TextureArray( ITextureArrayData data )
        : base( IGL30.GL_TEXTURE_2D_ARRAY, ( int )Gdx.GL.GLGenTexture() )
    {
        if ( Gdx.GL30 == null )
        {
            throw new GdxRuntimeException( "TextureArray requires a device"
                                         + "running with GLES 3.0 compatibilty" );
        }

        _data = null!;

        Load( data );

        if ( data.Managed )
        {
            AddManagedTexture( Gdx.App, this );
        }
    }

    #endregion constructors

    /// <summary>
    /// </summary>
    /// <param name="internalPaths"></param>
    /// <returns></returns>
    private static FileInfo[] GetInternalHandles( params string[] internalPaths )
    {
        var handles = new FileInfo[ internalPaths.Length ];

        for ( var i = 0; i < internalPaths.Length; i++ )
        {
            handles[ i ] = Gdx.Files.Internal( internalPaths[ i ] );
        }

        return handles;
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    private void Load( ITextureArrayData data )
    {
        if ( ( this._data != null ) && ( data.Managed != this._data.Managed ) )
        {
            throw new GdxRuntimeException( "New data must have the same managed status as the old data" );
        }

        this._data = data;

        Bind();

        Gdx.GL30?.GLTexImage3D( IGL30.GL_TEXTURE_2D_ARRAY,
                                0,
                                data.InternalFormat,
                                data.Width,
                                data.Height,
                                data.Depth,
                                0,
                                data.InternalFormat,
                                data.GLType,
                                null! );

        if ( !data.Prepared )
        {
            data.Prepare();
        }

        data.ConsumeTextureArrayData();

        SetFilter( MinFilter, MagFilter );
        SetWrap( UWrap, VWrap );
        Gdx.GL.GLBindTexture( GLTarget, 0 );
    }

    /// <summary>
    /// </summary>
    /// <exception cref="GdxRuntimeException"></exception>
    protected override void Reload()
    {
        if ( !IsManaged )
        {
            throw new GdxRuntimeException( "Tried to reload an unmanaged TextureArray" );
        }

        GLTextureHandle = ( int )Gdx.GL.GLGenTexture();

        Load( this._data );
    }

    /// <summary>
    /// </summary>
    /// <param name="app"></param>
    /// <param name="texture"></param>
    private static void AddManagedTexture( IApplication app, TextureArray texture )
    {
        List< TextureArray > managedTextureArray = ManagedTextureArrays[ app ];

        ManagedTextureArrays[ app ].Add( texture );
        ManagedTextureArrays[ app ] = managedTextureArray;
    }

    /// <summary>
    /// 
    /// </summary>
    public string ManagedStatus
    {
        get
        {
            var builder = new StringBuilder( "Managed TextureArrays/app: { " );

            foreach ( IApplication app in ManagedTextureArrays.Keys )
            {
                builder.Append( ManagedTextureArrays[ app ].Count );
                builder.Append( ' ' );
            }

            builder.Append( '}' );

            return builder.ToString();
        }
    }

    // ------------------------------------------------------------------------

    /// <summary>
    /// Gets the number of managed TextureArrays currently loaded.
    /// </summary>
    public int NumManagedTextureArrays => ManagedTextureArrays[ Gdx.App ].Count;

    #region aliases
    
    public override int  Width     => _data.Width;
    public override int  Height    => _data.Height;
    public override int  Depth     => _data.Depth;
    public override bool IsManaged => _data.Managed;

    #endregion aliases
    
    // ------------------------------------------------------------------------

    #region internal methods

    /// <summary>
    /// Clears all managed TextureArrays.
    /// </summary>
    internal static void ClearAllTextureArrays( IApplication app )
    {
        ManagedTextureArrays.Remove( app );
    }

    /// <summary>
    /// Invalidate all managed TextureArrays.
    /// </summary>
    internal static void InvalidateAllTextureArrays( IApplication app )
    {
        foreach ( TextureArray textureArray in ManagedTextureArrays[ app ] )
        {
            textureArray.Reload();
        }
    }

    #endregion internal methods

}
