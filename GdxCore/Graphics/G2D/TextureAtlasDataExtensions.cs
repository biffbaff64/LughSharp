using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public partial class TextureAtlasData
{
    public sealed class PageFieldParseClass : IField
    {
        public void Parse( ref Page page )
        {
            page.Width  = int.Parse( Entry[ 1 ] );
            page.Height = int.Parse( Entry[ 2 ] );
        }
    }

    public sealed class PageFieldFormatClass : IField
    {
        public void Parse( ref Page page )
        {
            page.Format = Pixmap.FormatFromString( Entry[ 1 ] );
        }
    }

    public sealed class PageFieldFilterClass : IField
    {
        public void Parse( ref Page page )
        {
            page.MinFilter  = TextureFilter.ValueOf( Entry[ 1 ] );
            page.MagFilter  = TextureFilter.ValueOf( Entry[ 2 ] );
            page.UseMipMaps = page.MinFilter.IsMipMap();
        }
    }

    public sealed class PageFieldRepeatClass : IField
    {
        public void Parse( ref Page page )
        {
            if ( Entry[ 1 ].IndexOf( 'x' ) != -1 ) page.UWrap = TextureWrap.Repeat;
            if ( Entry[ 1 ].IndexOf( 'y' ) != -1 ) page.VWrap = TextureWrap.Repeat;
        }
    }

    public sealed class PageFieldPmaClass : IField
    {
        public void Parse( ref Page page )
        {
            // Note: I'm no9t sure what 'Pma' stands for. It's called that
            // in Java LibGDX so it's been carried over. Once I've figured
            // it out I'll rename it to something more maeningful.
            page.Pma = Entry[ 1 ].Equals( "true" );
        }
    }
}