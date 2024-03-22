// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.Extensions.Gdx_Tools.Tools.TexturePacker;

[PublicAPI]
public class ColorBleedEffect
{
    private readonly static int[] _offsets = { -1, -1, 0, -1, 1, -1, -1, 0, 1, 0, -1, 1, 0, 1, 1, 1 };

//    public BufferedImage ProcessImage( BufferedImage image, int maxIterations )
//    {
//        int width  = image.getWidth();
//        int height = image.getHeight();
//
//        BufferedImage processedImage;
//
//        if ( image.getType() == BufferedImage.TYPE_INT_ARGB )
//        {
//            processedImage = image;
//        }
//        else
//        {
//            processedImage = new BufferedImage( width, height, BufferedImage.TYPE_INT_ARGB );
//        }
//
//        int[] rgb  = image.getRGB( 0, 0, width, height, null, 0, width );
//        Mask  mask = new(rgb);
//
//        int iterations  = 0;
//        int lastPending = -1;
//
//        while ( ( mask._pendingSize > 0 ) && ( mask._pendingSize != lastPending ) && ( iterations < maxIterations ) )
//        {
//            lastPending = mask._pendingSize;
//            ExecuteIteration( rgb, mask, width, height );
//            iterations++;
//        }
//
//        processedImage.setRGB( 0, 0, width, height, rgb, 0, width );
//
//        return processedImage;
//    }

    private void ExecuteIteration( int[] rgb, Mask mask, int width, int height )
    {
        var iterator = new Mask.MaskIterator( mask );

        while ( iterator.HasNext() )
        {
            var pixelIndex = iterator.Next();
            var x          = pixelIndex % width;
            var y          = pixelIndex / width;
            int r          = 0, g = 0, b = 0;
            var count      = 0;

            for ( int i = 0, n = _offsets.Length; i < n; i += 2 )
            {
                var column = x + _offsets[ i ];
                var row    = y + _offsets[ i + 1 ];

                if ( ( column < 0 ) || ( column >= width ) || ( row < 0 ) || ( row >= height ) )
                {
//                    column = x;
//                    row    = y;

                    continue;
                }

                var currentPixelIndex = GetPixelIndex( width, column, row );

                if ( !mask.IsBlank( currentPixelIndex ) )
                {
                    var argb = rgb[ currentPixelIndex ];
                    r += Red( argb );
                    g += Green( argb );
                    b += Blue( argb );
                    count++;
                }
            }

            if ( count != 0 )
            {
                rgb[ pixelIndex ] = ARGB( 0, r / count, g / count, b / count );
                iterator.MarkAsInProgress();
            }
        }

        iterator.Reset();
    }

    private static int GetPixelIndex( int width, int x, int y )
    {
        return ( y * width ) + x;
    }

    private static int Red( int argb )
    {
        return ( argb >> 16 ) & 0xFF;
    }

    private static int Green( int argb )
    {
        return ( argb >> 8 ) & 0xFF;
    }

    private static int Blue( int argb )
    {
        return ( argb >> 0 ) & 0xFF;
    }

    private static int ARGB( int a, int r, int g, int b )
    {
        if ( ( a < 0 )
          || ( a > 255 )
          || ( r < 0 )
          || ( r > 255 )
          || ( g < 0 )
          || ( g > 255 )
          || ( b < 0 )
          || ( b > 255 ) )
        {
            throw new ArgumentException( "Invalid RGBA: " + r + ", " + g + "," + b + "," + a );
        }

        return ( ( a & 0xFF ) << 24 ) | ( ( r & 0xFF ) << 16 ) | ( ( g & 0xFF ) << 8 ) | ( ( b & 0xFF ) << 0 );
    }
    
    private class Mask
    {
        private readonly bool[] _blank;
        private readonly int[]  _changing;
        private readonly int[]  _pending;
        private          int    _changingSize;

        private int _pendingSize;

        public Mask( IReadOnlyList< int > rgb )
        {
            var n = rgb.Count;

            _blank    = new bool[ n ];
            _pending  = new int[ n ];
            _changing = new int[ n ];

            for ( var i = 0; i < n; i++ )
            {
                if ( Alpha( rgb[ i ] ) == 0 )
                {
                    _blank[ i ]              = true;
                    _pending[ _pendingSize ] = i;
                    _pendingSize++;
                }
            }
        }

        public bool IsBlank( int index )
        {
            return _blank[ index ];
        }

        private int RemoveIndex( int index )
        {
            if ( index >= _pendingSize )
            {
                throw new IndexOutOfRangeException( index.ToString() );
            }

            var value = _pending[ index ];

            _pendingSize--;
            _pending[ index ] = _pending[ _pendingSize ];

            return value;
        }

        private static int Alpha( int argb )
        {
            return ( argb >> 24 ) & 0xff;
        }

        internal sealed class MaskIterator
        {
            private readonly Mask _parent;
            private          int  _index;

            public MaskIterator( Mask parent )
            {
                _parent = parent;
            }

            internal bool HasNext()
            {
                return _index < _parent._pendingSize;
            }

            internal int Next()
            {
                if ( _index >= _parent._pendingSize )
                {
                    throw new IndexOutOfRangeException( _index.ToString() );
                }

                return _parent._pending[ _index++ ];
            }

            internal void MarkAsInProgress()
            {
                _index--;
                _parent._changing[ _parent._changingSize ] = _parent.RemoveIndex( _index );
                _parent._changingSize++;
            }

            internal void Reset()
            {
                _index = 0;

                for ( int i = 0, n = _parent._changingSize; i < n; i++ )
                {
                    _parent._blank[ _parent._changing[ i ] ] = false;
                }

                _parent._changingSize = 0;
            }
        }
    }
}
