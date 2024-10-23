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

using Corelib.LibCore.Utils.Exceptions;
using Exception = System.Exception;

namespace Corelib.LibCore.Graphics.G2D;

[PublicAPI]
public partial record TextureAtlasData
{
    internal static readonly bool[] HasIndexes = [ false ];

    // ------------------------------------------------------------------------

    #region Constructors

    /// <summary>
    /// </summary>
    /// <param name="packFile"></param>
    /// <param name="imagesDir"></param>
    /// <param name="flip"></param>
    public TextureAtlasData( FileInfo? packFile = null, DirectoryInfo? imagesDir = null, bool flip = false )
    {
        if ( packFile != null )
        {
            Load( packFile, imagesDir, flip );
        }
    }

    #endregion

    public List< Page >   Pages   { get; set; } = [ ];
    public List< Region > Regions { get; set; } = [ ];
    public string[]       Entry   { get; set; } = new string[ 5 ];

    // ------------------------------------------------------------------------

    private void Load( FileInfo? packFile, DirectoryInfo? imagesDir, bool flip )
    {
        ArgumentNullException.ThrowIfNull( packFile );
        
        //@formatter:off
        Dictionary< string, IField< Page > > pageFields = new( 15 )
        {
            { "pma",        new PageFieldPma()    },
            { "size",       new PageFieldParse()  },
            { "format",     new PageFieldFormat() },
            { "filter",     new PageFieldFilter() },
            { "repeat",     new PageFieldRepeat() },
        };

        Dictionary< string, IField< Region > > regionFields = new( 127 )
        {
            { "rotate",     new RegionFieldRotate()  },
            { "xy",         new RegionFieldXY()      },
            { "size",       new RegionFieldSize()    },
            { "bounds",     new RegionFieldBounds()  },
            { "orig",       new RegionFieldOrig()    },
            { "offset",     new RegionFieldOffset()  },
            { "offsets",    new RegionFieldOffsets() },
            { "index",      new RegionFieldIndex()   },
        };
        //@formatter:on

        var reader = new StreamReader( packFile.Name, false );

        try
        {
            var line = reader.ReadLine();

            // Ignore empty lines before first entry.
            while ( ( line != null ) && ( line.Trim().Length == 0 ) )
            {
                line = reader.ReadLine();
            }

            // Header entries.
            while ( true )
            {
                if ( ( line == null ) || ( line.Trim().Length == 0 ) )
                {
                    break;
                }

                if ( ReadEntry( line ) == 0 )
                {
                    // Silently ignore all header fields.
                    break;
                }

                line = reader.ReadLine();
            }

            // Page and region entries.
            Page?           page   = null;
            List< string >? names  = null;
            List< int[] >?  values = null;

            while ( true )
            {
                if ( line == null )
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
                        TextureFile = new FileInfo( line.Trim() ),
                    };

                    while ( true )
                    {
                        if ( ReadEntry( line = reader.ReadLine() ) == 0 )
                        {
                            break;
                        }

                        pageFields?[ Entry[ 0 ] ].Parse( page );
                    }

                    Pages.Add( page );
                }
                else
                {
                    var region = new Region
                    {
                        Page = page,
                        Name = line.Trim(),
                    };

                    if ( flip )
                    {
                        region.Flip = true;
                    }

                    while ( true )
                    {
                        var count = ReadEntry( line = reader.ReadLine() );

                        if ( count == 0 )
                        {
                            break;
                        }

                        if ( regionFields?[ Entry[ 0 ] ] != null )
                        {
                            regionFields[ Entry[ 0 ] ].Parse( region );
                        }
                        else
                        {
                            if ( names == null )
                            {
                                names  = new List< string >( 8 );
                                values = new List< int[] >( 8 );
                            }

                            names.Add( Entry[ 0 ] );

                            var entryValues = new int[ count ];

                            for ( var i = 0; i < count; i++ )
                            {
                                try
                                {
                                    entryValues[ i ] = int.Parse( Entry[ i + 1 ] );
                                }
                                catch ( FormatException )
                                {
                                    // Silently ignore non-integer values.
                                }
                            }

                            values?.Add( entryValues );
                        }
                    }

                    if ( region is { OriginalWidth: 0, OriginalHeight: 0 } )
                    {
                        region.OriginalWidth  = region.Width;
                        region.OriginalHeight = region.Height;
                    }

                    // ( if names != null and names.Count > 0 )
                    if ( names is { Count: > 0 } )
                    {
                        region.Names  = names.ToArray();
                        region.Values = values?.ToArray();
                        names.Clear();
                        values?.Clear();
                    }

                    Regions.Add( region );
                }
            }
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error reading texture atlas file: " + packFile, ex );
        }
        finally
        {
            reader.Close();
        }

        if ( HasIndexes[ 0 ] )
        {
            Regions.Sort( new ComparatorAnonymousInnerClass( this ) );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="line"></param>
    /// <returns></returns>
    private int ReadEntry( string? line )
    {
        if ( line == null )
        {
            return 0;
        }

        line = line.Trim();

        if ( line.Length == 0 )
        {
            return 0;
        }

        var colon = line.IndexOf( ':' );

        if ( colon == -1 )
        {
            return 0;
        }

        Entry[ 0 ] = line.Substring( 0, colon ).Trim();

        for ( int i = 1, lastMatch = colon + 1;; i++ )
        {
            var comma = line.IndexOf( ',', lastMatch );

            if ( comma == -1 )
            {
                Entry[ i ] = line.Substring( lastMatch ).Trim();

                return i;
            }

            Entry[ i ] = line.Substring( lastMatch, comma ).Trim();

            lastMatch = comma + 1;

            if ( i == 4 )
            {
                return 4;
            }
        }
    }

    // ------------------------------------------------------------------------

    protected interface IField< in T >
    {
        void Parse( T obj, params string[] entry );
    }

    // ######################################################################
    //      Companions.
    // ######################################################################

    #region Companions

    [PublicAPI]
    public record Page
    {
        /// <summary>
        /// May be null if the texture is not yet loaded.
        /// </summary>
        public Texture? Texture { get; set; }

        /// <summary>
        /// May be null if this page isn't associated with a file. In that
        /// case, <see cref="Texture"/> must be set.
        /// </summary>
        public FileInfo? TextureFile { get; set; }

        public bool          UseMipMaps         { get; set; }
        public Pixmap.ColorFormat Format             { get; set; } = Pixmap.ColorFormat.RGBA8888;
        public TextureFilter MinFilter          { get; set; } = TextureFilter.Nearest;
        public TextureFilter MagFilter          { get; set; } = TextureFilter.Nearest;
        public TextureWrap   UWrap              { get; set; } = TextureWrap.ClampToEdge;
        public TextureWrap   VWrap              { get; set; } = TextureWrap.ClampToEdge;
        public float         Width              { get; set; }
        public float         Height             { get; set; }
        public bool          PreMultipliedAlpha { get; set; }
    }

    [PublicAPI]
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

    #endregion
}
