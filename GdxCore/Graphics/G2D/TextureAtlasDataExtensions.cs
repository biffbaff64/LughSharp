using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public partial class TextureAtlasData
{
    public sealed class PageFieldParseClass : IField< Page >
    {
        public void Parse( ref Page page, params string[] entry )
        {
            page.Width  = int.Parse( entry[ 1 ] );
            page.Height = int.Parse( entry[ 2 ] );
        }
    }

    public sealed class PageFieldFormatClass : IField< Page >
    {
        public void Parse( ref Page page, params string[] entry )
        {
            page.Format = Pixmap.FormatFromString( entry[ 1 ] );
        }
    }

    public sealed class PageFieldFilterClass : IField< Page >
    {
        public void Parse( ref Page page, params string[] entry )
        {
            page.MinFilter  = TextureFilter.ValueOf( entry[ 1 ] );
            page.MagFilter  = TextureFilter.ValueOf( entry[ 2 ] );
            page.UseMipMaps = page.MinFilter.IsMipMap();
        }
    }

    public sealed class PageFieldRepeatClass : IField< Page >
    {
        public void Parse( ref Page page, params string[] entry )
        {
            if ( entry[ 1 ].IndexOf( 'x' ) != -1 ) page.UWrap = TextureWrap.Repeat;
            if ( entry[ 1 ].IndexOf( 'y' ) != -1 ) page.VWrap = TextureWrap.Repeat;
        }
    }

    public sealed class PageFieldPmaClass : IField< Page >
    {
        public void Parse( ref Page page, params string[] entry )
        {
            // Note: I'm not sure what 'Pma' stands for. It's called that
            // in Java LibGDX so it's been carried over. Once I've figured
            // it out I'll rename it to something more meaningful.
            page.Pma = entry[ 1 ].Equals( "true" );
        }
    }
    
    public class RegionFieldXYClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.Left = int.Parse( entry[ 1 ] );
            region.Top  = int.Parse( entry[ 2 ] );
        }
    }

    public class RegionFieldSizeClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.Width  = int.Parse(entry[1]);
            region.Height = int.Parse(entry[2]);
        }
    }

    public class RegionFieldBoundsClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.Left   = int.Parse(entry[1]);
            region.Top    = int.Parse(entry[2]);
            region.Width  = int.Parse(entry[3]);
            region.Height = int.Parse(entry[4]);
        }
    }

    public class RegionFieldOffsetClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.OffsetX = int.Parse(entry[1]);
            region.OffsetY = int.Parse(entry[2]);
        }
    }

    public class RegionFieldOrigClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.OriginalWidth  = int.Parse(entry[1]);
            region.OriginalHeight = int.Parse(entry[2]);
        }
    }

    public class RegionFieldOffsetsClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.OffsetX        = int.Parse(entry[1]);
            region.OffsetY        = int.Parse(entry[2]);
            region.OriginalWidth  = int.Parse(entry[3]);
            region.OriginalHeight = int.Parse(entry[4]);
        }
    }

    public class RegionFieldRotateClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            string value = entry[1];
            if (value.Equals("true"))
                region.Degrees = 90;
            else if (!value.Equals("false")) //
                region.Degrees = int.Parse(value);
            region.Rotate = region.Degrees == 90;
        }
    }

    public class RegionFieldIndexClass : IField< TextureAtlasData.Region >
    {
        public void Parse( ref Region region, params string[] entry )
        {
            region.Index = int.Parse(entry[1]);
            if (region.Index != -1) HasIndexes[0] = true;
        }
    }
}
