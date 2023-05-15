using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public partial class TextureAtlasData
{
    public List< Page >   Pages   { get; set; } = new();
    public List< Region > Regions { get; set; } = new();

    public TextureAtlasData()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="packFile"></param>
    /// <param name="imagesDir"></param>
    /// <param name="flip"></param>
    public TextureAtlasData( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
    {
        Load( packFile, imagesDir, flip );
    }

    public void Load( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
    {
        var entry = new string[ 5 ];
        bool[] hasIndexes = { false };

        //@formatter:off
        Dictionary< string, IField< Page > > pageFields = new(15)
        {
            { "size",       new PageFieldParseClass()  },
            { "format",     new PageFieldFormatClass() },
            { "filter",     new PageFieldFilterClass() },
            { "repeat",     new PageFieldRepeatClass() },
            { "pma",        new PageFieldPmaClass()    }
        };

        Dictionary< string, IField< Region > > regionFields = new(127)
        {
            { "xy",         new RegionFieldXYClass()      },
            { "size",       new RegionFieldSizeClass()    },
            { "bounds",     new RegionFieldBoundsClass()  },
            { "offset",     new RegionFieldOffsetClass()  },
            { "orig",       new RegionFieldOrigClass()    },
            { "offsets",    new RegionFieldOffsetsClass() },
            { "rotate",     new RegionFieldRotateClass()  },
            { "index",      new RegionFieldIndexClass()   },
        };
        //@formatter:on

        var reader = new StreamReader( new StreamReader( packFile.Read() ), 1024 );

        try
        {
            var line = reader.ReadLine();

            // Ignore empty lines before first entry.
            while ( !string.ReferenceEquals( line, null ) && ( line.Trim().Length == 0 ) )
            {
                line = reader.ReadLine();
            }

            // Header entries.
            while ( true )
            {
                if ( string.ReferenceEquals( line, null ) || ( line.Trim().Length == 0 ) )
                {
                    break;
                }

                if ( ReadEntry( entry, line ) == 0 )
                {
                    break; // Silently ignore all header fields.
                }

                line = reader.ReadLine();
            }

            // Page and region entries.
            Page?           page   = null;
            List< object >? names  = null;
            List< object >? values = null;

            while ( true )
            {
                if ( string.ReferenceEquals( line, null ) )
                {
                    break;
                }

                if ( line.Trim().Length == 0 )
                {
                    page = null;
                    line = reader.ReadLine();
                }
                else if ( page == null )
                {
                    page = new Page
                    {
                        textureFile = imagesDir.Child( line )
                    };

                    while ( true )
                    {
                        if ( ReadEntry( entry, line = reader.ReadLine() ) == 0 )
                        {
                            break;
                        }

                        IField? field = pageFields?[ entry[ 0 ] ];

                        field?.Parse( ref page ); // Silently ignore unknown page fields.
                    }

                    Pages.Add( page );
                }
                else
                {
                    var region = new Region
                    {
                        Page = page,
                        Name = line.Trim()
                    };

                    if ( flip )
                    {
                        region.Flip = true;
                    }

                    while ( true )
                    {
                        int count = ReadEntry( entry, line = reader.ReadLine() );

                        if ( count == 0 )
                        {
                            break;
                        }

                        IField field = regionFields.get( entry[ 0 ] );

                        if ( field != null )
                        {
                            field.parse( region );
                        }
                        else
                        {
                            if ( names == null )
                            {
                                names  = new Array( 8 );
                                values = new Array( 8 );
                            }

                            names.add( entry[ 0 ] );
                            var entryValues = new int[ count ];

                            for ( var i = 0; i < count; i++ )
                            {
                                try
                                {
                                    entryValues[ i ] = int.Parse( entry[ i + 1 ] );
                                }
                                catch ( System.FormatException )
                                {
                                    // Silently ignore non-integer values.
                                }
                            }

                            values.add( entryValues );
                        }
                    }

                    if ( region.originalWidth == 0 && region.originalHeight == 0 )
                    {
                        region.originalWidth  = region.width;
                        region.originalHeight = region.height;
                    }

                    if ( names != null && names.size > 0 )
                    {
                        region.names  = names.toArray( typeof(string) );
                        region.values = values.toArray( typeof(int[]) );
                        names.clear();
                        values.clear();
                    }

                    regions.add( region );
                }
            }
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error reading texture atlas file: " + packFile, ex );
        }
        finally
        {
            StreamUtils.closeQuietly( reader );
        }

        if ( hasIndexes[ 0 ] )
        {
            Regions.Sort( new ComparatorAnonymousInnerClass( this ) );
        }
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

    // ######################################################################
    //      Companions.
    // ######################################################################

    protected interface IField<T>
    {
        void Parse( ref T obj, params string[] entry );
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

    [SuppressMessage( "ReSharper", "UnusedAutoPropertyAccessor.Global" )]
    public class Region
    {
        public Page?     Page           { get; init; }
        public string?   Name           { get; init; }
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
    }
}
