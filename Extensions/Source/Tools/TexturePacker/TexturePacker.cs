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

using JetBrains.Annotations;

namespace Extensions.Source.Tools.TexturePacker;

[PublicAPI]
public class TexturePacker
{
    [PublicAPI]
    public class Page
    {
        public string?       imageName      { get; set; }
        public List< Rect >? outputRects    { get; set; }
        public List< Rect >? remainingRects { get; set; }
        public float         occupancy      { get; set; }
        public int           x              { get; set; }
        public int           y              { get; set; }
        public int           width          { get; set; }
        public int           height         { get; set; }
        public int           imageWidth     { get; set; }
        public int           imageHeight    { get; set; }
    }

    [PublicAPI]
    public class Alias( Rect rect ) : IComparable< Alias >
    {
        public string name           = rect.name;
        public int    index          = rect.index;
        public int[]  splits         = rect.splits;
        public int[]  pads           = rect.pads;
        public int    offsetX        = rect.offsetX;
        public int    offsetY        = rect.offsetY;
        public int    originalWidth  = rect.originalWidth;
        public int    originalHeight = rect.originalHeight;

        public void Apply( Rect rect )
        {
            rect.name           = name;
            rect.index          = index;
            rect.splits         = splits;
            rect.pads           = pads;
            rect.offsetX        = offsetX;
            rect.offsetY        = offsetY;
            rect.originalWidth  = originalWidth;
            rect.originalHeight = originalHeight;
        }

        public int CompareTo( Alias? o )
        {
            return string.Compare( name, o?.name, StringComparison.Ordinal );
        }
    }

    [PublicAPI]
    public class Rect : IComparable< Rect >
    {
        public string name = string.Empty;
        public int    offsetX;
        public int    offsetY;
        public int    regionWidth;
        public int    regionHeight;
        public int    originalWidth;
        public int    originalHeight;
        public int    x,     y;
        public int    width, height; // Portion of page taken by this region, including padding.
        public int    index;

        public bool rotated;

//        public Set< Alias > aliases = new HashSet< Alias >();
        public int[] splits    = null!;
        public int[] pads      = null!;
        public bool  canRotate = true;

        private bool isPatch;

//        private BufferedImage image;
        private FileInfo file = null!;
        private int      score1;
        private int      score2;

        public Rect()
        {
        }

        public Rect( Rect rect )
        {
            x      = rect.x;
            y      = rect.y;
            width  = rect.width;
            height = rect.height;
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
            this.file = fileInfo;
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
            name = rect.name;
//            image          = rect.image;
            offsetX        = rect.offsetX;
            offsetY        = rect.offsetY;
            regionWidth    = rect.regionWidth;
            regionHeight   = rect.regionHeight;
            originalWidth  = rect.originalWidth;
            originalHeight = rect.originalHeight;
            x              = rect.x;
            y              = rect.y;
            width          = rect.width;
            height         = rect.height;
            index          = rect.index;
            rotated        = rect.rotated;
//            aliases        = rect.aliases;
            splits    = rect.splits;
            pads      = rect.pads;
            canRotate = rect.canRotate;
            score1    = rect.score1;
            score2    = rect.score2;
            file      = rect.file;
            isPatch   = rect.isPatch;
        }

        public int CompareTo( Rect? o )
        {
            return string.Compare( name, o?.name, StringComparison.Ordinal );
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
            return name + ( index != -1 ? "_" + index : "" ) + "[" + x + "," + y + " " + width + "x" + height + "]";
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