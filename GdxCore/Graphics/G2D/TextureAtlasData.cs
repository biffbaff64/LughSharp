using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.G2D
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public partial class TextureAtlasData
    {
        public List< Page >   Pages   { get; set; } = new List< Page >();
        public List< Region > Regions { get; set; } = new List< Region >();

        public TextureAtlasData()
        {
        }

        public TextureAtlasData( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
        {
            Load( packFile, imagesDir, flip );
        }

        protected readonly static string[]                     Entry      = new string[ 5 ];
        protected readonly        Dictionary< string, IField > pageFields = new(15);

        public void Load( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
        {
            Array.Clear( Entry );
            pageFields.Clear();

            pageFields.Put( "size", new PageFieldParseClass() );
            pageFields.Put( "format", new PageFieldFormatClass() );
            pageFields.Put( "filter", new PageFieldFilterClass() );
            pageFields.Put( "repeat", new PageFieldRepeatClass() );
            pageFields.Put( "pma", new PageFieldPmaClass() );
        }

        /// <summary>
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private int ReadEntry( string[] entry, string? line )
        {
            if ( line == null ) return 0;

            line = line.Trim();

            if ( line.Length == 0 ) return 0;

            var colon = line.IndexOf( ':' );

            if ( colon == -1 ) return 0;

            entry[ 0 ] = line.Substring( 0, colon ).Trim();

            for ( int i = 1, lastMatch = colon + 1;; i++ )
            {
                var comma = line.IndexOf( ',', lastMatch );

                if ( comma == -1 )
                {
                    entry[ i ] = line.Substring( lastMatch ).Trim();

                    return i;
                }

                entry[ i ] = line.Substring( lastMatch, comma ).Trim();

                lastMatch = comma + 1;

                if ( i == 4 ) return 4;
            }
        }

        protected interface IField
        {
            void Parse( ref Page page );
        }

        public record Page
        {
            /// <summary>
            /// May be null if this page isn't associated with a file. In that
            /// case, <see cref="texture"/> must be set.
            /// </summary>
            public FileInfo? textureFile;

            /// <summary>
            /// May be null if the texture is not yet loaded.
            /// </summary>
            public Texture? texture;

            public bool          UseMipMaps { get; set; }
            public Pixmap.Format Format     { get; set; } = Pixmap.Format.RGBA8888;
            public TextureFilter MinFilter  { get; set; } = TextureFilter.Nearest;
            public TextureFilter MagFilter  { get; set; } = TextureFilter.Nearest;
            public TextureWrap   UWrap      { get; set; } = TextureWrap.ClampToEdge;
            public TextureWrap   VWrap      { get; set; } = TextureWrap.ClampToEdge;
            public float         Width      { get; set; }
            public float         Height     { get; set; }
            public bool          Pma        { get; set; }
        }

        public class Region
        {
            public Page?     Page           { get; set; }
            public string?   Name           { get; set; }
            public int       Left           { get; set; }
            public int       Top            { get; set; }
            public int       Width          { get; set; }
            public int       Height         { get; set; }
            public float     OffsetX        { get; set; }
            public float     OffsetY        { get; set; }
            public int       OriginalWidth  { get; set; }
            public int       OriginalHeight { get; set; }
            public int       Degrees        { get; set; }
            public bool      Rotate         { get; set; }
            public bool      Flip           { get; set; }
            public int       Index          { get; set; } = -1;
            public string[]? Names          { get; set; }
            public int[]?[]? Values         { get; set; }

            public virtual int[]? FindValue( string name )
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
        }
    }
}
