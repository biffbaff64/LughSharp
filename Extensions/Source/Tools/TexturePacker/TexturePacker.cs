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

using JetBrains.Annotations;

namespace Extensions.Source.Tools.TexturePacker;

[PublicAPI]
public class TexturePacker
{
    [PublicAPI]
    public class Page
    {
        public string?       ImageName      { get; set; }
        public List< Rect >? OutputRects    { get; set; }
        public List< Rect >? RemainingRects { get; set; }
        public float         Occupancy      { get; set; }
        public int           X              { get; set; }
        public int           Y              { get; set; }
        public int           Width          { get; set; }
        public int           Height         { get; set; }
        public int           ImageWidth     { get; set; }
        public int           ImageHeight    { get; set; }
    }

    [PublicAPI]
    public class Alias( Rect rect ) : IComparable< Alias >
    {
        public string Name           = rect.Name;
        public int    Index          = rect.Index;
        public int[]  Splits         = rect.Splits;
        public int[]  Pads           = rect.Pads;
        public int    OffsetX        = rect.OffsetX;
        public int    OffsetY        = rect.OffsetY;
        public int    OriginalWidth  = rect.OriginalWidth;
        public int    OriginalHeight = rect.OriginalHeight;

        public void Apply( Rect rect )
        {
            rect.Name           = Name;
            rect.Index          = Index;
            rect.Splits         = Splits;
            rect.Pads           = Pads;
            rect.OffsetX        = OffsetX;
            rect.OffsetY        = OffsetY;
            rect.OriginalWidth  = OriginalWidth;
            rect.OriginalHeight = OriginalHeight;
        }

        public int CompareTo( Alias? o )
        {
            return string.Compare( Name, o?.Name, StringComparison.Ordinal );
        }
    }

    [PublicAPI]
    public class Rect : IComparable< Rect >
    {
        public string Name = string.Empty;
        public int    OffsetX;
        public int    OffsetY;
        public int    RegionWidth;
        public int    RegionHeight;
        public int    OriginalWidth;
        public int    OriginalHeight;
        public int    X;
        public int    Y;
        public int    Width;  // Portion of page taken by this region, including padding.
        public int    Height; // Portion of page taken by this region, including padding.
        public int    Index;
        public bool   Rotated;

//        public Set< Alias > aliases = new HashSet< Alias >();
        public int[] Splits    = null!;
        public int[] Pads      = null!;
        public bool  CanRotate = true;

        private bool _isPatch;

//        private BufferedImage image;
        private FileInfo _file = null!;
        private int      _score1;
        private int      _score2;

        public Rect()
        {
        }

        public Rect( Rect rect )
        {
            X      = rect.X;
            Y      = rect.Y;
            Width  = rect.Width;
            Height = rect.Height;
        }

//        public Rect( BufferedImage source, int left, int top, int newWidth, int newHeight, bool isPatch )
//        {
//            image = new BufferedImage( source.getColorModel(),
//                                       source.getRaster().createWritableChild( left, top, newWidth, newHeight, 0, 0, null ),
//                                       source.getColorModel().isAlphaPremultiplied(), null );
//            offsetX        = left;
//            offsetY        = top;
//            regionWidth    = newWidth;
//            regionHeight   = newHeight;
//            originalWidth  = source.getWidth();
//            originalHeight = source.getHeight();
//            width          = newWidth;
//            height         = newHeight;
//            this.isPatch   = isPatch;
//        }

        /** Clears the image for this rect, which will be loaded from the specified file by {@link #getImage(ImageProcessor)}. */
        public void UnloadImage( FileInfo fileInfo )
        {
            this._file = fileInfo;

//            image     = null;
        }

//        public BufferedImage getImage( ImageProcessor imageProcessor )
//        {
//            if ( image != null ) return image;
//
//            BufferedImage image;
//            try
//            {
//                image = ImageIO.read( file );
//            }
//            catch ( IOException ex )
//            {
//                throw new RuntimeException( "Error reading image: " + file, ex );
//            }
//
//            if ( image == null ) throw new GdxRuntimeException( "Unable to read image: " + file );
//            string name         = this.name;
//            if ( isPatch ) name += ".9";
//            return imageProcessor.ProcessImage( image, name ).getImage( null );
//        }

        protected void Set( Rect rect )
        {
            Name = rect.Name;

//            image          = rect.image;
            OffsetX        = rect.OffsetX;
            OffsetY        = rect.OffsetY;
            RegionWidth    = rect.RegionWidth;
            RegionHeight   = rect.RegionHeight;
            OriginalWidth  = rect.OriginalWidth;
            OriginalHeight = rect.OriginalHeight;
            X              = rect.X;
            Y              = rect.Y;
            Width          = rect.Width;
            Height         = rect.Height;
            Index          = rect.Index;
            Rotated        = rect.Rotated;

//            aliases        = rect.aliases;
            Splits    = rect.Splits;
            Pads      = rect.Pads;
            CanRotate = rect.CanRotate;
            _score1    = rect._score1;
            _score2    = rect._score2;
            _file      = rect._file;
            _isPatch   = rect._isPatch;
        }

        public int CompareTo( Rect? o )
        {
            return string.Compare( Name, o?.Name, StringComparison.Ordinal );
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals( object? obj )
        {
//            if ( this == obj ) return true;
//            if ( obj == null ) return false;
////            if ( getClass() != obj.getClass() ) return false;
//
//            var other = ( Rect )obj;
//
//            if ( name == null )
//            {
//                if ( other.name != null ) return false;
//            }
//            else
//            {
//                if ( !name.Equals( other.name ) ) return false;
//            }

            return true;
        }

        public override string ToString()
        {
            return Name + ( Index != -1 ? "_" + Index : "" ) + "[" + X + "," + Y + " " + Width + "x" + Height + "]";
        }

        public static string GetAtlasName( string name, bool flattenPaths )
        {
            return flattenPaths ? new FileInfo( name ).Name : name;
        }
    }

//    public enum Resampling
//    {
//        Nearest  = RenderingHints.VALUE_INTERPOLATION_NEAREST_NEIGHBOR,
//        Bilinear = RenderingHints.VALUE_INTERPOLATION_BILINEAR,
//        Bicubic  = RenderingHints.VALUE_INTERPOLATION_BICUBIC;
//    }
}