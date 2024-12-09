// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.G2D;

// ============================================================================
// ============================================================================

public partial class TextureAtlasData
{
    // ========================================================================

    public class PageFieldSize : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.Width  = int.Parse( entry[ 1 ] );
            page.Height = int.Parse( entry[ 2 ] );
        }
    }

    // ========================================================================

    public class PageFieldFormat : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.Format = Pixmap.FormatFromString( entry[ 1 ] );
        }
    }

    // ========================================================================

    public class PageFieldFilter : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.MinFilter  = Enum.Parse< Texture.TextureFilter >( entry[ 1 ] );
            page.MagFilter  = Enum.Parse< Texture.TextureFilter >( entry[ 2 ] );
            page.UseMipMaps = Texture.Utils.IsMipMap( page.MinFilter );
        }
    }

    // ========================================================================

    public class PageFieldRepeat : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            if ( entry[ 1 ].Contains('x'))
            {
                page.UWrap = Texture.TextureWrap.Repeat;
            }

            if ( entry[ 1 ].Contains('y'))
            {
                page.VWrap = Texture.TextureWrap.Repeat;
            }
        }
    }

    // ========================================================================

    public class PageFieldPma : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.PreMultipliedAlpha = entry[ 1 ].Equals( "true" );
        }
    }

    // ========================================================================

    public class RegionFieldXY : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Left = int.Parse( entry[ 1 ] );
            region.Top  = int.Parse( entry[ 2 ] );
        }
    }

    // ========================================================================

    public class RegionFieldSize : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Width  = int.Parse( entry[ 1 ] );
            region.Height = int.Parse( entry[ 2 ] );
        }
    }

    // ========================================================================

    public class RegionFieldBounds : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Left   = int.Parse( entry[ 1 ] );
            region.Top    = int.Parse( entry[ 2 ] );
            region.Width  = int.Parse( entry[ 3 ] );
            region.Height = int.Parse( entry[ 4 ] );
        }
    }

    // ========================================================================

    public class RegionFieldOffset : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OffsetX = int.Parse( entry[ 1 ] );
            region.OffsetY = int.Parse( entry[ 2 ] );
        }
    }

    // ========================================================================

    public class RegionFieldOrig : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OriginalWidth  = int.Parse( entry[ 1 ] );
            region.OriginalHeight = int.Parse( entry[ 2 ] );
        }
    }

    // ========================================================================

    public class RegionFieldOffsets : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OffsetX        = int.Parse( entry[ 1 ] );
            region.OffsetY        = int.Parse( entry[ 2 ] );
            region.OriginalWidth  = int.Parse( entry[ 3 ] );
            region.OriginalHeight = int.Parse( entry[ 4 ] );
        }
    }

    // ========================================================================

    public class RegionFieldRotate : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            var value = entry[ 1 ];

            if ( value.Equals( "true" ) )
            {
                region.Degrees = 90;
            }
            else if ( !value.Equals( "false" ) )
            {
                region.Degrees = int.Parse( value );
            }

            region.Rotate = region.Degrees == 90;
        }
    }

    // ========================================================================

    public class RegionFieldIndex : IField< Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Index = int.Parse( entry[ 1 ] );

            if ( region.Index != -1 )
            {
                HasIndexes[ 0 ] = true;
            }
        }
    }
}

// ============================================================================
// ============================================================================

[PublicAPI]
public class ComparatorAnonymousInnerClass : IComparer< TextureAtlasData.Region >
{
    public ComparatorAnonymousInnerClass( TextureAtlasData textureAtlasData )
    {
    }

    /// <summary>
    /// Compares two objects and returns a value indicating whether one is
    /// less than, equal to, or greater than the other.
    /// </summary>
    /// <param name="region1">The first object to compare.</param>
    /// <param name="region2">The second object to compare.</param>
    /// <returns></returns>
    public int Compare( TextureAtlasData.Region? region1, TextureAtlasData.Region? region2 )
    {
        if ( ( region1 == null ) || ( region2 == null ) )
        {
            throw new GdxRuntimeException( "Cannot compare null region" );
        }

        var i1 = region1.Index;
        var i2 = region2.Index;

        if ( i1 == -1 )
        {
            i1 = int.MaxValue;
        }

        if ( i2 == -1 )
        {
            i2 = int.MaxValue;
        }

        return i1 - i2;
    }
}
