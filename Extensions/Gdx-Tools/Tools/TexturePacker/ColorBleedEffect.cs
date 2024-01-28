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

namespace LibGDXSharp.Extensions.Tools.TexturePacker;

[PublicAPI]
public class ColorBleedEffect
{
    private readonly static int[] Offsets = { -1, -1, 0, -1, 1, -1, -1, 0, 1, 0, -1, 1, 0, 1, 1, 1 };

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

            for ( int i = 0, n = Offsets.Length; i < n; i += 2 )
            {
                var column = x + Offsets[ i ];
                var row    = y + Offsets[ i + 1 ];

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

    private static int Red( int argb ) => ( argb >> 16 ) & 0xFF;

    private static int Green( int argb ) => ( argb >> 8 ) & 0xFF;

    private static int Blue( int argb ) => ( argb >> 0 ) & 0xFF;

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
        private readonly int[]  _pending;
        private readonly int[]  _changing;

        private int _pendingSize;
        private int _changingSize;

        public Mask( IReadOnlyList< int > rgb )
        {
            var n = rgb.Count;
            
            _blank     = new bool[ n ];
            _pending  = new int[ n ];
            _changing = new int[ n ];

            for ( var i = 0; i < n; i++ )
            {
                if ( Alpha( rgb[ i ] ) == 0 )
                {
                    _blank[ i ]               = true;
                    _pending[ _pendingSize ] = i;
                    _pendingSize++;
                }
            }
        }

        public bool IsBlank( int index ) => _blank[ index ];

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

        internal sealed class MaskIterator( Mask parent )
        {
            private int _index;

            internal bool HasNext()
            {
                return _index < parent._pendingSize;
            }

            internal int Next()
            {
                if ( _index >= parent._pendingSize )
                {
                    throw new IndexOutOfRangeException( _index.ToString() );
                }

                return parent._pending[ _index++ ];
            }

            internal void MarkAsInProgress()
            {
                _index--;
                parent._changing[ parent._changingSize ] = parent.RemoveIndex( _index );
                parent._changingSize++;
            }

            internal void Reset()
            {
                _index = 0;

                for ( int i = 0, n = parent._changingSize; i < n; i++ )
                {
                    parent._blank[ parent._changing[ i ] ] = false;
                }

                parent._changingSize = 0;
            }
        }

        private static int Alpha( int argb ) => ( argb >> 24 ) & 0xff;
    }
}
