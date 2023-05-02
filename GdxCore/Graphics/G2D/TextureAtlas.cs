using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.G2D
{
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    [SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
    public class TextureAtlas
    {
        public List< AtlasRegion? > Regions  { get; set; } = new();
        public List< Texture >      Textures { get; set; } = new(4);

        /// <summary>
        /// Loads the specified pack file using <see cref="FileType.Internal"/>,
        /// using the parent directory of the pack file to find the page images. 
        /// </summary>
        public TextureAtlas( string internalPackFile )
            : this( Gdx.Files.Internal( internalPackFile ) )
        {
        }

        /// <summary>
        /// Loads the specified pack file, using the parent directory of the
        /// pack file to find the page images.
        /// </summary>
        public TextureAtlas( FileInfo packFile ) : this( packFile, packFile.Directory )
        {
        }

        /// <param name="packFile"></param>
        /// <param name="flip">
        /// If true, all regions loaded will be flipped for use with a perspective
        /// where 0,0 is the upper left corner.
        /// </param>
        public TextureAtlas( FileInfo packFile, bool flip )
            : this( packFile, packFile.Directory, flip )
        {
        }

        /// <param name="packFile"></param>
        /// <param name="imagesDir"></param>
        /// <param name="flip">
        /// If true, all regions loaded will be flipped for use with a perspective
        /// where 0,0 is the upper left corner.
        /// </param>
        public TextureAtlas( FileInfo packFile, DirectoryInfo? imagesDir, bool flip = false )
            : this( new TextureAtlasData( packFile, imagesDir, flip ) )
        {
        }

        public TextureAtlas( TextureAtlasData data )
        {
            Load( data );
        }

        /// <summary>
        /// Adds the textures and regions from the specified <see cref="TextureAtlasData"/>
        /// </summary>
        public void Load( TextureAtlasData data )
        {
            Textures.EnsureCapacity( data.Pages.Count );

            foreach ( TextureAtlasData.Page page in data.Pages )
            {
                page.texture ??= new Texture( page.textureFile, page.Format, page.UseMipMaps );

                page.texture.SetFilter( page.MinFilter, page.MagFilter );
                page.texture.SetWrap( page.UWrap, page.VWrap );

                Textures.Add( page.texture );
            }

            Regions.EnsureCapacity( data.Regions.Count );

            foreach ( TextureAtlasData.Region region in data.Regions )
            {
                var atlasRegion = new AtlasRegion
                    (
                     region.Page?.texture,
                     region.Left,
                     region.Top,
                     region.Rotate ? region.Height : region.Width,
                     region.Rotate ? region.Width : region.Height
                    )
                    {
                        Index          = region.Index,
                        Name           = region.Name,
                        OffsetX        = region.OffsetX,
                        OffsetY        = region.OffsetY,
                        OriginalHeight = region.OriginalHeight,
                        OriginalWidth  = region.OriginalWidth,
                        Rotate         = region.Rotate,
                        Degrees        = region.Degrees,
                        Names          = region.Names,
                        values         = region.Values
                    };

                if ( region.Flip )
                {
                    atlasRegion.Flip( false, true );
                }

                Regions.Add( atlasRegion );
            }
        }

        /// <summary>
        /// Adds a region to the atlas. The specified texture will be disposed
        /// when the atlas is disposed.
        /// </summary>
        public AtlasRegion AddRegion( string name, Texture texture, int x, int y, int width, int height )
        {
            Textures.Add( texture );

            var region = new AtlasRegion( texture, x, y, width, height )
            {
                Name = name
            };

            Regions.Add( region );

            return region;
        }

        /// <summary>
        /// Adds a region to the atlas. The texture for the specified region will
        /// be disposed when the atlas is disposed.
        /// </summary>
        public AtlasRegion AddRegion( string name, TextureRegion textureRegion )
        {
            if ( textureRegion.Texture == null )
                throw new GdxRuntimeException( "cannot add null texture!" );

            Textures.Add( textureRegion.Texture );

            var region = new AtlasRegion( textureRegion )
            {
                Name = name
            };

            Regions.Add( region );

            return region;
        }

        /// <summary>
        /// Returns the first region found with the specified name. This method uses
        /// string comparison to find the region, so the result should be cached
        /// rather than calling this method multiple times. 
        /// </summary>
        public AtlasRegion? FindRegion( string name )
        {
            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                if ( Regions[ i ]?.Name == name )
                {
                    return Regions[ i ];
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first region found with the specified name and index. This method
        /// uses string comparison to find the region, so the result should be cached
        /// rather than calling this method multiple times. 
        /// </summary>
        public AtlasRegion? FindRegion( string name, int index )
        {
            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                AtlasRegion? region = Regions[ i ];

                if ( ( region?.Name != name ) || ( region.Index != index ) )
                {
                    continue;
                }

                return region;
            }

            return null;
        }

        /// <summary>
        /// Returns all regions with the specified name, ordered by smallest to
        /// largest index. This method uses string comparison to find the regions,
        /// so the result should be cached rather than calling this method multiple
        /// times. 
        /// </summary>
        public List< AtlasRegion > FindRegions( string name )
        {
            var matched = new List< AtlasRegion >();

            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                AtlasRegion? region = Regions[ i ];

                if ( region?.Name == name )
                {
                    matched.Add( new AtlasRegion( region ) );
                }
            }

            return matched;
        }

        /// <summary>
        /// Returns all regions in the atlas as sprites. This method creates a new
        /// sprite for each region, so the result should be stored rather than calling
        /// this method multiple times.
        /// </summary>
        public List< Sprite > CreateSprites()
        {
            var sprites = new List< Sprite >();

            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                sprites.Add( NewSprite( Regions[ i ] ) );
            }

            return sprites;
        }

        /// <summary>
        /// Returns the first region found with the specified name as a sprite.
        /// If whitespace was stripped from the region when it was packed, the
        /// sprite is automatically positioned as if whitespace had not been
        /// stripped. This method uses string comparison to find the region and
        /// constructs a new sprite, so the result should be cached rather than
        /// calling this method multiple times. 
        /// </summary>
        public Sprite? CreateSprite( string name )
        {
            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                if ( Regions[ i ]?.Name == name )
                {
                    return NewSprite( Regions[ i ] );
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the first region found with the specified name and index as a
        /// sprite. This method uses string comparison to find the region and
        /// constructs a new sprite, so the result should be cached rather than
        /// calling this method multiple times.
        /// </summary>
        public Sprite? CreateSprite( string name, int index )
        {
            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                AtlasRegion? region = Regions[ i ];

                if ( ( region?.Index != index ) || ( region.Name != name ) )
                {
                    continue;
                }

                return NewSprite( Regions[ i ] );
            }

            return null;
        }

        /// <summary>
        /// Returns all regions with the specified name as sprites, ordered by smallest
        /// to largest index.
        /// <para>
        /// This method uses string comparison to find the regions and constructs new
        /// sprites, so the result should be cached rather than calling this method
        /// multiple times.
        /// </para>
        /// </summary>
        public List< Sprite > CreateSprites( string name )
        {
            var matched = new List< Sprite >();

            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                AtlasRegion? region = Regions[ i ];

                if ( region?.Name == name )
                {
                    matched.Add( NewSprite( region ) );
                }
            }

            return matched;
        }

        /// <summary>
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private static Sprite NewSprite( AtlasRegion? region )
        {
            if ( region == null ) throw new GdxRuntimeException( "atlas region cannot be null!" );

            if ( ( region.PackedWidth == region.OriginalWidth )
                 && ( region.PackedHeight == region.OriginalHeight ) )
            {
                if ( region.Rotate )
                {
                    var sprite = new Sprite( region );

                    sprite.SetBounds( 0, 0, region.RegionHeight, region.RegionWidth );
                    sprite.Rotate90( true );

                    return sprite;
                }

                return new Sprite( region );
            }

            return new AtlasSprite( region );
        }

        /// <summary>
        /// Returns the first region found with the specified name as
        /// a <see cref="NinePatch"/>. The region must have been packed with
        /// ninepatch splits. This method uses string comparison to find the region
        /// and constructs a new ninepatch, so the result should
        /// be cached rather than calling this method multiple times. 
        /// </summary>
        public NinePatch? CreatePatch( string name )
        {
            for ( int i = 0, n = Regions.Count; i < n; i++ )
            {
                AtlasRegion? region = Regions[ i ];

                if ( region?.Name == name )
                {
                    var splits = region.FindValue( "split" );

                    if ( splits == null )
                    {
                        throw new System.ArgumentException( "Region does not have ninepatch splits: " + name );
                    }

                    var patch = new NinePatch( region, splits[ 0 ], splits[ 1 ], splits[ 2 ], splits[ 3 ] );

                    var pads = region.FindValue( "pad" );

                    if ( pads != null )
                    {
                        patch.SetPadding( pads[ 0 ], pads[ 1 ], pads[ 2 ], pads[ 3 ] );
                    }

                    return patch;
                }
            }

            return null;
        }

        /// <summary>
        /// Releases all resources associated with this TextureAtlas instance.
        /// This releases all the textures backing all TextureRegions and Sprites,
        /// which should no longer be used after calling dispose.
        /// </summary>
        public void Dispose()
        {
            foreach ( Texture texture in Textures )
            {
                texture.Dispose();
            }

            Textures.Clear();
        }

        public class AtlasRegion : TextureRegion
        {
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
            /// each entry corresponding to <see cref="values"/>.
            /// </summary>
            public string?[]? Names { get; set; }

            /// <summary>
            /// Values for name/value pairs other than the fields provided on this class,
            /// each entry corresponding to <see cref="Names"/>.
            /// </summary>
            public int[]?[]? values;

            public AtlasRegion( Texture? texture, int x, int y, int width, int height )
                : base( texture, x, y, width, height )
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
                values         = region.values;
            }

            public AtlasRegion( TextureRegion region )
            {
                SetRegion( region );

                PackedWidth    = region.RegionWidth;
                PackedHeight   = region.RegionHeight;
                OriginalWidth  = PackedWidth;
                OriginalHeight = PackedHeight;
            }

            public new void Flip( bool x, bool y )
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

            /// <summary>
            /// Returns the packed width considering the <see cref="Rotate"/> value,
            /// if it is true then it returns the packedHeight, otherwise it returns
            /// the packedWidth. 
            /// </summary>
            protected float RotatedPackedWidth => Rotate ? PackedHeight : PackedWidth;

            /// <summary>
            /// Returns the packed height considering the <seealso cref="Rotate"/> value,
            /// if it is true then it returns the packedWidth, otherwise it returns the
            /// packedHeight. 
            /// </summary>
            protected float RotatedPackedHeight => Rotate ? PackedWidth : PackedHeight;

            public int[]? FindValue( string name )
            {
                if ( Names != null )
                {
                    for ( int i = 0, n = Names.Length; i < n; i++ )
                    {
                        if ( name.Equals( Names[ i ] ) )
                        {
                            return values?[ i ];
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

        public class AtlasSprite : Sprite
        {
            public AtlasSprite( AtlasRegion region ) : base( region )
            {
            }
        }
    }

}
