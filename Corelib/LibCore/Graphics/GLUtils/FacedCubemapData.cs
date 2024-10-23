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

using Corelib.LibCore.Graphics.OpenGL;
using Corelib.LibCore.Utils.Exceptions;

namespace Corelib.LibCore.Graphics.GLUtils;

[PublicAPI]
public class FacedCubemapData : ICubemapData
{
    private readonly ITextureData?[] _data = new ITextureData[ 6 ];

    /// <summary>
    /// Construct an empty Cubemap. Use the load(...) methods to set the texture
    /// of each side. Every side of the cubemap must be set before it can be used.
    /// </summary>
    public FacedCubemapData() : this( null, null, null, null, null, null )
    {
    }

    /// <summary>
    /// Construct a Cubemap with the specified texture files for the sides, optionally generating mipmaps.
    /// </summary>
    public FacedCubemapData( FileInfo positiveX,
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
    /// Construct a Cubemap with the specified <see cref="Pixmap"/>s for the sides, optionally generating mipmaps.
    /// </summary>
    public FacedCubemapData( Pixmap? positiveX,
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
    public FacedCubemapData( int width, int height, int depth, Pixmap.ColorFormat format )
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
    public FacedCubemapData( ITextureData? positiveX,
                             ITextureData? negativeX,
                             ITextureData? positiveY,
                             ITextureData? negativeY,
                             ITextureData? positiveZ,
                             ITextureData? negativeZ )
    {
        _data[ 0 ] = positiveX;
        _data[ 1 ] = negativeX;
        _data[ 2 ] = positiveY;
        _data[ 3 ] = negativeY;
        _data[ 4 ] = positiveZ;
        _data[ 5 ] = negativeZ;
    }

    /// <summary>
    /// Uploads the pixel data for the 6 faces of the cube to the OpenGL ES texture.
    /// The caller must bind an OpenGL ES texture. A call to <see cref="ICubemapData.Prepare"/>
    /// must preceed a call to this method. Any internal data structures created
    /// in <see cref="ICubemapData.Prepare"/> should be disposed of here.
    /// </summary>
    public void ConsumeCubemapData()
    {
        if ( _data == null )
        {
            throw new NullReferenceException();
        }

        for ( var i = 0; i < _data.Length; i++ )
        {
            if ( _data[ i ]!.TextureDataType == ITextureData.TextureType.Custom )
            {
                _data[ i ]!.ConsumeCustomData( IGL.GL_TEXTURE_CUBE_MAP_POSITIVE_X + i );
            }
            else
            {
                var pixmap        = _data[ i ]!.ConsumePixmap()!;
                var disposePixmap = _data[ i ]!.ShouldDisposePixmap();

                if ( _data[ i ]!.Format != pixmap.Format )
                {
                    var tmp = new Pixmap( pixmap.Width, pixmap.Height, _data[ i ]!.Format );

                    tmp.Blending = Pixmap.BlendTypes.None;
                    tmp.DrawPixmap( pixmap, 0, 0, 0, 0, pixmap.Width, pixmap.Height );

                    if ( _data[ i ]!.ShouldDisposePixmap() )
                    {
                        pixmap.Dispose();
                    }

                    pixmap        = tmp;
                    disposePixmap = true;
                }

                Gdx.GL.glPixelStorei( IGL.GL_UNPACK_ALIGNMENT, 1 );

                Gdx.GL.glTexImage2D( IGL.GL_TEXTURE_CUBE_MAP_POSITIVE_X + i,
                                     0,
                                     pixmap.GLInternalFormat,
                                     pixmap.Width,
                                     pixmap.Height,
                                     0,
                                     pixmap.GLFormat,
                                     pixmap.GLType,
                                     pixmap.ByteBuffer.BackingArray() );

                if ( disposePixmap )
                {
                    pixmap.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// The width of the pixel data.
    /// </summary>
    public int Width
    {
        get
        {
            int tmp;
            var width = 0;

            if ( ( _data[ Cubemap.CubemapSide.PositiveZ.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.PositiveZ.Index ]!.Width ) > width ) )
            {
                width = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.NegativeZ.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.NegativeZ.Index ]!.Width ) > width ) )
            {
                width = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.PositiveY.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.PositiveY.Index ]!.Width ) > width ) )
            {
                width = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.NegativeY.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.NegativeY.Index ]!.Width ) > width ) )
            {
                width = tmp;
            }

            return width;
        }
    }

    /// <summary>
    /// The height of the pixel data.
    /// </summary>
    public int Height
    {
        get
        {
            int tmp;
            var height = 0;
            
            if ( ( _data[ Cubemap.CubemapSide.PositiveZ.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.PositiveZ.Index ]!.Height ) > height ) )
            {
                height = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.NegativeZ.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.NegativeZ.Index ]!.Height ) > height ) )
            {
                height = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.PositiveX.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.PositiveX.Index ]!.Height ) > height ) )
            {
                height = tmp;
            }

            if ( ( _data[ Cubemap.CubemapSide.NegativeX.Index ] != null )
              && ( ( tmp = _data[ Cubemap.CubemapSide.NegativeX.Index ]!.Height ) > height ) )
            {
                height = tmp;
            }

            return height;
        }
    }

    /// <summary>
    /// Returns true if this implementation can cope with a EGL context loss.
    /// </summary>
    public bool IsManaged
    {
        get
        {
            if ( _data == null )
            {
                throw new NullReferenceException();
            }

            foreach ( var data in _data )
            {
                if ( data is { IsManaged: false } )
                {
                    return false;
                }
            }

            return true;
        }
        set { }
    }

    /// <summary>
    /// whether the ITextureData is prepared or not.
    /// </summary>
    public bool IsPrepared => false;

    /// <summary>
    /// Prepares the ITextureData for a call to <see cref="ICubemapData.ConsumeCubemapData"/>.
    /// This method can be called from a non OpenGL thread and should thus not
    /// interact with OpenGL.
    /// </summary>
    public void Prepare()
    {
        if ( !IsComplete() )
        {
            throw new GdxRuntimeException( "Cubemap data must be complete before use!" );
        }

        foreach ( var data in _data )
        {
            if ( data is { IsPrepared: false } )
            {
                data.Prepare();
            }
        }
    }

    /// <summary>
    /// Loads the texture specified using the <see cref="FileInfo"/> and sets it
    /// to specified side, overwriting any previous data set to that side. Note that
    /// you need to reload through <see cref="Cubemap.Load(ICubemapData)"/> any cubemap
    /// using this data for the change to be taken in account.
    /// </summary>
    /// <param name="side"> The <see cref="Cubemap.CubemapSide"/> </param>
    /// <param name="file"> The texture <see cref="FileInfo"/> </param>
    public void Load( Cubemap.CubemapSide side, FileInfo file )
    {
        if ( _data == null )
        {
            throw new GdxRuntimeException( $"Cannot load {file.Name}, _data is null!" );
        }

        _data[ side.Index ] = TextureDataFactory.LoadFromFile( file, false )
                           ?? throw new GdxRuntimeException( $"Error loading {file.Name}" );
    }

    /// <summary>
    /// Sets the specified side of this cubemap to the specified <see cref="Pixmap"/>,
    /// overwriting any previous data set to that side. Note that you need to reload
    /// through <see cref="Cubemap.Load(ICubemapData)"/> any cubemap using this data
    /// for the change to be taken in account.
    /// </summary>
    /// <param name="side"> The <see cref="Cubemap.CubemapSide"/> </param>
    /// <param name="pixmap"> The <see cref="Pixmap"/> </param>
    public void Load( Cubemap.CubemapSide side, Pixmap? pixmap )
    {
        if ( _data == null )
        {
            throw new GdxRuntimeException( "Cannot load pixmap, _data is null!" );
        }

        _data[ side.Index ] = ( pixmap == null ? null : new PixmapTextureData( pixmap, null, false, false ) )
                           ?? throw new GdxRuntimeException( "Error loadin pixmap" );
    }

    /// <summary>
    /// Returns The <see cref="ITextureData"/> for the specified side.
    /// The return value can be null if the cubemap is incomplete.
    /// </summary>
    public ITextureData? TextureData( Cubemap.CubemapSide side )
    {
        return _data[ side.Index ];
    }

    /// <summary>
    /// Return True if all sides of this cubemap are set, false otherwise.
    /// </summary>
    public bool IsComplete()
    {
        foreach ( var data in _data )
        {
            if ( data == null )
            {
                return false;
            }
        }

        return true;
    }
}