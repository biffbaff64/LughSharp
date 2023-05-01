using System.Diagnostics.CodeAnalysis;

using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.G2D
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public class TextureAtlasData
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

        public void Load( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
        {
            var entry = new string[ 5 ];

            var pageFields = new Dictionary< string, IField< Page > >( 15, 0.99f ); // Size needed to avoid collisions.

            pageFields.Put( "size", new IField< Page >()
            {
                public void parse (Page page)
                {
                    page.width = Integer.parseInt(entry[1]);
                    page.height = Integer.parseInt(entry[2]);
                }
            });

            pageFields.Put( "format", new IField< Page >()
            {
                public void parse (Page page)
                {
                    page.format = Format.valueOf(entry[1]);
                }
            });

            pageFields.put
                ( "filter", new Field< Page >()
                {
 
                    public void parse (Page page) {
                    page.minFilter = TextureFilter.valueOf(entry[1]);
                    page.magFilter = TextureFilter.valueOf(entry[2]);
                    page.useMipMaps = page.minFilter.isMipMap();
                }

            });

            pageFields.put
                ( "repeat", new Field< Page >()
                {
 
                    public void parse (Page page) {
                    if (entry[1].indexOf('x') != -1) page.uWrap = TextureWrap.Repeat;
                    if (entry[1].indexOf('y') != -1) page.vWrap = TextureWrap.Repeat;
                }

            });
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

        private interface IField<in T>
        {
            void Parse( T obj );
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
            public string[]? names;
            public int[][]   values;

            public virtual int[]? FindValue( string name )
            {
                if ( names != null )
                {
                    for ( int i = 0, n = names.Length; i < n; i++ )
                    {
                        if ( name.Equals( names[ i ] ) )
                        {
                            return values[ i ];
                        }
                    }
                }

                return null;
            }
        }

        public IEnumerable< Page > GetPages()
        {
            throw new NotImplementedException();
        }
    }
}
