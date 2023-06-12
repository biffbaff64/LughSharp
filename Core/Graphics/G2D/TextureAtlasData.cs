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

using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public partial record TextureAtlasData
{
    public List< Page >   Pages   { get; set; } = new();
    public List< Region > Regions { get; set; } = new();
    public string[]       Entry   { get; set; } = new string[ 5 ];

    internal readonly static bool[] HasIndexes = { false };

    #region Constructors

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

    #endregion

    #region InternalInterfaces

    protected interface IField<T>
    {
        void Parse( ref T obj, params string[] entry );
    }

    #endregion

    private void Load( FileInfo packFile, DirectoryInfo? imagesDir, bool flip )
    {
        //@formatter:off
        Dictionary< string, IField< Page > > pageFields = new( 15 )
        {
            { "pma",        new PageFieldPmaClass()    },
            { "size",       new PageFieldParseClass()  },
            { "format",     new PageFieldFormatClass() },
            { "filter",     new PageFieldFilterClass() },
            { "repeat",     new PageFieldRepeatClass() },
        };

        Dictionary< string, IField< Region > > regionFields = new( 127 )
        {
            { "rotate",     new RegionFieldRotateClass()  },
            { "xy",         new RegionFieldXYClass()      },
            { "size",       new RegionFieldSizeClass()    },
            { "bounds",     new RegionFieldBoundsClass()  },
            { "orig",       new RegionFieldOrigClass()    },
            { "offset",     new RegionFieldOffsetClass()  },
            { "offsets",    new RegionFieldOffsetsClass() },
            { "index",      new RegionFieldIndexClass()   },
        };
        //@formatter:on

        var reader = new StreamReader( packFile.Name, false );

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

                if ( ReadEntry( line ) == 0 )
                {
                    break; // Silently ignore all header fields.
                }

                line = reader.ReadLine();
            }

            // Page and region entries.
            Page?           page   = null;
            List< string >? names  = null;
            List< int[] >?  values = null;

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
                        if ( ReadEntry( line = reader.ReadLine() ) == 0 )
                        {
                            break;
                        }

                        pageFields?[ Entry[ 0 ] ].Parse( ref page );
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
                        var count = ReadEntry( line = reader.ReadLine() );

                        if ( count == 0 )
                        {
                            break;
                        }

                        if ( regionFields?[ Entry[ 0 ] ] != null )
                        {
                            regionFields[ Entry[ 0 ] ].Parse( ref region );
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
                                catch ( System.FormatException )
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
        if ( line == null ) return 0;

        line = line.Trim();

        if ( line.Length == 0 ) return 0;

        var colon = line.IndexOf( ':' );

        if ( colon == -1 ) return 0;

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

            if ( i == 4 ) return 4;
        }
    }

    // ######################################################################
    //      Companions.
    // ######################################################################

    #region Companions

    [SuppressMessage( "ReSharper", "UnusedAutoPropertyAccessor.Global" )]
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

    #endregion
}