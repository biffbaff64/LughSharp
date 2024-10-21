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


namespace Corelib.LibCore.Graphics.G2D;

[PublicAPI]
public class AtlasRegion : TextureRegion
{
    public AtlasRegion( Texture? texture, int x, int y, int width, int height )
        : base( texture!, x, y, width, height )
    {
        OriginalWidth  = width;
        OriginalHeight = height;
        PackedWidth    = width;
        PackedHeight   = height;
    }

    public AtlasRegion( AtlasRegion region )
    {
        SetRegion( region );

        Index          = region.Index;
        Name           = region.Name;
        OffsetX        = region.OffsetX;
        OffsetY        = region.OffsetY;
        PackedWidth    = region.PackedWidth;
        PackedHeight   = region.PackedHeight;
        OriginalWidth  = region.OriginalWidth;
        OriginalHeight = region.OriginalHeight;
        Rotate         = region.Rotate;
        Degrees        = region.Degrees;
        Names          = region.Names;
        Values         = region.Values;
    }

    public AtlasRegion( TextureRegion region )
    {
        SetRegion( region );

        PackedWidth    = region.RegionWidth;
        PackedHeight   = region.RegionHeight;
        OriginalWidth  = PackedWidth;
        OriginalHeight = PackedHeight;
    }

    /// <summary>
    /// Values for name/value pairs other than the fields provided on this class,
    /// each entry corresponding to <see cref="Names"/>.
    /// </summary>
    public int[]?[]? Values { get; set; }

    /// <summary>
    /// The number at the end of the original image file name, or -1 if none.
    /// When sprites are packed, if the original file name ends with a number,
    /// it is stored as the index and is not considered as part of the sprite's
    /// name. This is useful for keeping animation frames in order.
    /// </summary>
    public int Index { get; set; } = -1;

    /// <summary>
    /// The name of the original image file, without the file's extension.
    /// If the name ends with an underscore followed by only numbers, that part
    /// is excluded: underscores denote special instructions to the texture packer.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The offset from the left of the original image to the left of the packed
    /// image, after whitespace was removed for packing.
    /// </summary>
    public float OffsetX { get; set; }

    /// <summary>
    /// The offset from the bottom of the original image to the bottom of the packed
    /// image, after whitespace was removed for packing.
    /// </summary>
    public float OffsetY { get; set; }

    /// <summary>
    /// The width of the image, after whitespace was removed for packing.
    /// </summary>
    public int PackedWidth { get; set; }

    /// <summary>
    /// The height of the image, after whitespace was removed for packing.
    /// </summary>
    public int PackedHeight { get; set; }

    /// <summary>
    /// The width of the image, before whitespace was removed and rotation
    /// was applied for packing.
    /// </summary>
    public int OriginalWidth { get; set; }

    /// <summary>
    /// The height of the image, before whitespace was removed for packing.
    /// </summary>
    public int OriginalHeight { get; set; }

    /// <summary>
    /// If true, the region has been rotated 90 degrees counter clockwise.
    /// </summary>
    public bool Rotate { get; set; }

    /// <summary>
    /// The degrees the region has been rotated, counter clockwise between 0
    /// and 359. Most atlas region handling deals only with 0 or 90 degree
    /// rotation (enough to handle rectangles). More advanced texture packing
    /// may support other rotations (eg, for tightly packing polygons).
    /// </summary>
    public int Degrees { get; set; }

    /// <summary>
    /// Names for name/value pairs other than the fields provided on this class,
    /// each entry corresponding to <see cref="Values"/>.
    /// </summary>
    public string?[]? Names { get; set; }

    /// <summary>
    /// Returns the packed width considering the <see cref="Rotate"/> value,
    /// if it is true then it returns the packedHeight, otherwise it returns
    /// the packedWidth.
    /// </summary>
    public float RotatedPackedWidth => Rotate ? PackedHeight : PackedWidth;

    /// <summary>
    /// Returns the packed height considering the <see cref="Rotate"/> value,
    /// if it is true then it returns the packedWidth, otherwise it returns the
    /// packedHeight.
    /// </summary>
    public float RotatedPackedHeight => Rotate ? PackedWidth : PackedHeight;

    public override void Flip( bool x, bool y )
    {
        base.Flip( x, y );

        if ( x )
        {
            OffsetX = OriginalWidth - OffsetX - RotatedPackedWidth;
        }

        if ( y )
        {
            OffsetY = OriginalHeight - OffsetY - RotatedPackedHeight;
        }
    }

    public int[]? FindValue( string name )
    {
        if ( Names != null )
        {
            for ( int i = 0, n = Names.Length; i < n; i++ )
            {
                if ( name.Equals( Names[ i ] ) )
                {
                    return Values?[ i ];
                }
            }
        }

        return null;
    }

    public override string? ToString()
    {
        return Name;
    }
}
