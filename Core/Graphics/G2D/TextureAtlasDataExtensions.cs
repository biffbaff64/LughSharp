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

using LibGDXSharp.Utils;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
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
    /// <returns>A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="x" /> is less than <paramref name="y" />.</description></item><item><term> Zero</term><description><paramref name="x" /> equals <paramref name="y" />.</description></item><item><term> Greater than zero</term><description><paramref name="x" /> is greater than <paramref name="y" />.</description></item></list></returns>
    public int Compare( TextureAtlasData.Region? region1, TextureAtlasData.Region? region2 )
    {
        if ( ( region1 == null ) || ( region2 == null ) )
        {
            throw new GdxRuntimeException( "Cannot compare null region" );
        }
        
        var i1 = region1.Index;
        var i2 = region2.Index;

        if ( i1 == -1 ) i1 = int.MaxValue;

        if ( i2 == -1 ) i2 = int.MaxValue;

        return i1 - i2;
    }
}

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public partial record TextureAtlasData
{
    public class PageFieldParseClass : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.Width  = int.Parse( entry[ 1 ] );
            page.Height = int.Parse( entry[ 2 ] );
        }
    }

    public class PageFieldFormatClass : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.Format = Pixmap.FormatFromString( entry[ 1 ] );
        }
    }

    public class PageFieldFilterClass : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            page.MinFilter  = TextureFilter.ValueOf( entry[ 1 ] );
            page.MagFilter  = TextureFilter.ValueOf( entry[ 2 ] );
            page.UseMipMaps = page.MinFilter.IsMipMap();
        }
    }

    public class PageFieldRepeatClass : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            if ( entry[ 1 ].IndexOf( 'x' ) != -1 ) page.UWrap = TextureWrap.Repeat;
            if ( entry[ 1 ].IndexOf( 'y' ) != -1 ) page.VWrap = TextureWrap.Repeat;
        }
    }

    public class PageFieldPmaClass : IField< Page >
    {
        public void Parse( Page page, params string[] entry )
        {
            // Note: I'm not sure what 'Pma' stands for. It's called that
            // in Java LibGDX so it's been carried over. Once I've figured
            // it out I'll rename it to something more meaningful.
            page.Pma = entry[ 1 ].Equals( "true" );
        }
    }

    public class RegionFieldXYClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Left = int.Parse( entry[ 1 ] );
            region.Top  = int.Parse( entry[ 2 ] );
        }
    }

    public class RegionFieldSizeClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Width  = int.Parse( entry[ 1 ] );
            region.Height = int.Parse( entry[ 2 ] );
        }
    }

    public class RegionFieldBoundsClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Left   = int.Parse( entry[ 1 ] );
            region.Top    = int.Parse( entry[ 2 ] );
            region.Width  = int.Parse( entry[ 3 ] );
            region.Height = int.Parse( entry[ 4 ] );
        }
    }

    public class RegionFieldOffsetClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OffsetX = int.Parse( entry[ 1 ] );
            region.OffsetY = int.Parse( entry[ 2 ] );
        }
    }

    public class RegionFieldOrigClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OriginalWidth  = int.Parse( entry[ 1 ] );
            region.OriginalHeight = int.Parse( entry[ 2 ] );
        }
    }

    public class RegionFieldOffsetsClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.OffsetX        = int.Parse( entry[ 1 ] );
            region.OffsetY        = int.Parse( entry[ 2 ] );
            region.OriginalWidth  = int.Parse( entry[ 3 ] );
            region.OriginalHeight = int.Parse( entry[ 4 ] );
        }
    }

    public class RegionFieldRotateClass : IField< TextureAtlasData.Region >
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

    public class RegionFieldIndexClass : IField< TextureAtlasData.Region >
    {
        public void Parse( Region region, params string[] entry )
        {
            region.Index = int.Parse( entry[ 1 ] );

            if ( region.Index != -1 ) HasIndexes[ 0 ] = true;
        }
    }
}