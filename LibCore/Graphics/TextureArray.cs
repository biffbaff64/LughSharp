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

using LibGDXSharp.LibCore.Core;
using LibGDXSharp.LibCore.Utils;

namespace LibGDXSharp.LibCore.Graphics;

/// <summary>
///     Open GLES wrapper for TextureArray.
/// </summary>
[PublicAPI]
public class TextureArray : GLTexture
{
    private readonly static Dictionary< IApplication, List< TextureArray > > ManagedTextureArrays = new();

    private ITextureArrayData _data;

    /// <summary>
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
    ///     Gets the number of managed TextureArrays currently loaded.
    /// </summary>
    public int NumManagedTextureArrays => ManagedTextureArrays[ Core.Gdx.App ].Count;

    /// <summary>
    /// </summary>
    /// <param name="internalPaths"></param>
    /// <returns></returns>
    private static FileInfo[] GetInternalHandles( params string[] internalPaths )
    {
        var handles = new FileInfo[ internalPaths.Length ];

        for ( var i = 0; i < internalPaths.Length; i++ )
        {
            handles[ i ] = Core.Gdx.Files.Internal( internalPaths[ i ] );
        }

        return handles;
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    private void Load( ITextureArrayData data )
    {
        if ( ( _data != null ) && ( data.Managed != _data.Managed ) )
        {
            throw new GdxRuntimeException( "New data must have the same managed status as the old data" );
        }

        _data = data;

        Bind();

        Core.Gdx.GL30?.GLTexImage3D( IGL30.GL_TEXTURE_2D_ARRAY,
                                     0,
                                     data.InternalFormat,
                                     data.Width,
                                     data.Height,
                                     data.Depth,
                                     0,
                                     data.InternalFormat,
                                     data.GLType,
                                     0 );

        if ( !data.Prepared )
        {
            data.Prepare();
        }

        data.ConsumeTextureArrayData();

        SetFilter( MinFilter, MagFilter );
        SetWrap( UWrap, VWrap );
        Core.Gdx.GL.GLBindTexture( GLTarget, 0 );
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

        GLTextureHandle = Core.Gdx.GL.GLGenTexture();

        Load( _data );
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
        : base( IGL30.GL_TEXTURE_2D_ARRAY, Core.Gdx.GL.GLGenTexture() )
    {
        if ( Core.Gdx.GL30 == null )
        {
            throw new GdxRuntimeException( "TextureArray requires a device"
                                         + "running with GLES 3.0 compatibilty" );
        }

        _data = null!;

        Load( data );

        if ( data.Managed )
        {
            AddManagedTexture( Core.Gdx.App, this );
        }
    }

    #endregion constructors

    #region aliases

    public override int  Width     => _data.Width;
    public override int  Height    => _data.Height;
    public override int  Depth     => _data.Depth;
    public override bool IsManaged => _data.Managed;

    #endregion aliases

    // ------------------------------------------------------------------------

    #region internal methods

    /// <summary>
    ///     Clears all managed TextureArrays.
    /// </summary>
    internal static void ClearAllTextureArrays( IApplication app ) => ManagedTextureArrays.Remove( app );

    /// <summary>
    ///     Invalidate all managed TextureArrays.
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
