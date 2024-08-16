// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace LughSharp.LibCore.Graphics.Playground;

[PublicAPI]
public class PixelMap
{
    public int        Width         { get; set; }
    public int        Height        { get; set; }
    public int        PixFormat     { get; set; }
    public int        BytesPerPixel { get; set; }
    public Pixel[ , ] Pixels        { get; set; }
    public bool       IsDisposed    { get; set; } = false;

    /// <summary>
    /// Sets the type of <see cref="BlendTypes"/> to be used for all operations.
    /// Default is <see cref="BlendTypes.SourceOver"/>.
    /// </summary>
    public BlendTypes Blending { get; set; } = BlendTypes.SourceOver;

    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new PixelMap of the desired width and height.
    /// </summary>
    /// <param name="width"> The width in pixels. </param>
    /// <param name="height"> The height in pixels. </param>
    /// <param name="format">
    /// The pixel format, defaults to <see cref="Format.RGBA8888"/>.
    /// </param>
    public PixelMap( int width, int height, int format )
    {
        Logger.CheckPoint();

        this.Width         = width;
        this.Height        = height;
        this.PixFormat     = format;
        this.BytesPerPixel = PixmapFormat.Gdx2dBytesPerPixel( PixFormat );
        this.Pixels        = new Pixel[ width, height ];
    }

    /// <summary>
    /// Makes a copy of the supplied PixelMap.
    /// </summary>
    /// <param name="map"> The pixmap to copy. </param>
    public PixelMap( PixelMap map )
    {
        Logger.CheckPoint();

        this.Width         = map.Width;
        this.Height        = map.Height;
        this.PixFormat     = map.PixFormat;
        this.BytesPerPixel = map.BytesPerPixel;
        this.Pixels        = new Pixel[ map.Width, map.Height ];

        map.Pixels?.CopyTo( this.Pixels, 0 );
    }

    /// <summary>
    /// Creates a new PixelMap from the data at the suopplied FileInfo path.
    /// </summary>
    public PixelMap( FileInfo info ) : this( info.Name )
    {
        Logger.CheckPoint();
    }

    /// <summary>
    /// Creates a new PixelMap from the data at the supplied path.
    /// </summary>
    public PixelMap( string path )
    {
        Logger.CheckPoint();

        this.Width         = 0; //TODO:
        this.Height        = 0; //TODO:
        this.PixFormat     = 0; //TODO:
        this.BytesPerPixel = 0; //TODO:
        this.Pixels        = new Pixel[ Width, Height ];
    }

    /// <summary>
    /// Access a Pixel of the PixelMap from its X and Y coordinates.
    /// </summary>
    public Pixel this[ int x, int y ]
    {
        get
            => this.Inside( new Point2D( x, y ) )
                   ? this.Pixels[ x, y ]
                   : this.Pixels[ Math.Max( Math.Min( x, this.Width - 1 ), 0 ), Math.Max( Math.Min( y, this.Height - 1 ), 0 ) ];
        set
        {
            if ( !this.Inside( new Point2D( x, y ) ) )
                return;
            this.Pixels[ x, y ] = value;
        }
    }

    /// <summary>
    /// Access a Pixel of the PixelMap from its X and Y coordinates contained
    /// within a <see cref="Point2D"/>.
    /// </summary>
    public Pixel this[ Point2D point ]
    {
        get => this.Pixels[ point.X, point.Y ];
        set => this.Pixels[ point.X, point.Y ] = value;
    }

    /// <summary>
    /// Determine if a point is within this PixelMap.
    /// </summary>
    public bool Inside( Point2D p ) => p is { X: >= 0, Y: >= 0 } && p.X < this.Width && p.Y < this.Height;

    /// <summary>
    /// Produce a <see cref="Texture"/> from this PixelMap.
    /// </summary>
    public Texture GetTexture()
    {
        var texture = new Texture( this.Width,
                                   this.Height,
                                   PixmapFormat.FromGdx2DPixmapFormat( this.PixFormat ) );

        return texture;
    }

    // ------------------------------------------------------------------------

    #region PixmapEnums

    /// <summary>
    /// Blending functions to be set with <see cref="PixelMap.Blending"/>.
    /// </summary>
    public enum BlendTypes
    {
        None,
        SourceOver
    }

    /// <summary>
    /// Filters to be used with <see cref="DrawPixmap(PixelMap, int, int, int, int, int, int, int, int)"/>.
    /// </summary>
    public enum Filter
    {
        NearestNeighbour,
        BiLinear
    }

    /// <summary>
    /// Available PixelMap pixel formats.
    /// </summary>
    public enum Format
    {
        Alpha,
        Intensity,
        LuminanceAlpha,
        RGB565,
        RGBA4444,
        RGB888,
        RGBA8888
    }

    #endregion
}