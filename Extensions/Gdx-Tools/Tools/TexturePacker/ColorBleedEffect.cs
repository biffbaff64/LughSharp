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

public class ColorBleedEffect
{
//    private readonly static int[] offsets = { -1, -1, 0, -1, 1, -1, -1, 0, 1, 0, -1, 1, 0, 1, 1, 1 };
//
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
//
//    private void ExecuteIteration( int[] rgb, Mask mask, int width, int height )
//    {
//        Mask.MaskIterator iterator = mask.new MaskIterator();
//
//        while ( iterator.HasNext() )
//        {
//            int pixelIndex = iterator.Next();
//            int x          = pixelIndex % width;
//            int y          = pixelIndex / width;
//            int r          = 0, g = 0, b = 0;
//            int count      = 0;
//
//            for ( int i = 0, n = offsets.Length; i < n; i += 2 )
//            {
//                int column = x + offsets[ i ];
//                int row    = y + offsets[ i + 1 ];
//
//                if ( ( column < 0 ) || ( column >= width ) || ( row < 0 ) || ( row >= height ) )
//                {
//                    column = x;
//                    row    = y;
//
//                    continue;
//                }
//
//                int currentPixelIndex = GetPixelIndex( width, column, row );
//
//                if ( !mask.IsBlank( currentPixelIndex ) )
//                {
//                    int argb = rgb[ currentPixelIndex ];
//                    r += Red( argb );
//                    g += Green( argb );
//                    b += Blue( argb );
//                    count++;
//                }
//            }
//
//            if ( count != 0 )
//            {
//                rgb[ pixelIndex ] = ARGB( 0, r / count, g / count, b / count );
//                iterator.MarkAsInProgress();
//            }
//        }
//
//        iterator.Reset();
//    }
//
//    private static int GetPixelIndex( int width, int x, int y )
//    {
//        return ( y * width ) + x;
//    }
//
//    private static int Red( int argb )
//    {
//        return ( argb >> 16 ) & 0xFF;
//    }
//
//    private static int Green( int argb )
//    {
//        return ( argb >> 8 ) & 0xFF;
//    }
//
//    private static int Blue( int argb )
//    {
//        return ( argb >> 0 ) & 0xFF;
//    }
//
//    private static int ARGB( int a, int r, int g, int b )
//    {
//        if ( ( a < 0 )
//             || ( a > 255 )
//             || ( r < 0 )
//             || ( r > 255 )
//             || ( g < 0 )
//             || ( g > 255 )
//             || ( b < 0 )
//             || ( b > 255 ) )
//        {
//            throw new ArgumentException( "Invalid RGBA: " + r + ", " + g + "," + b + "," + a );
//        }
//
//        return ( ( a & 0xFF ) << 24 ) | ( ( r & 0xFF ) << 16 ) | ( ( g & 0xFF ) << 8 ) | ( ( b & 0xFF ) << 0 );
//    }
//
//    private class Mask
//    {
//        private bool[] _blank;
//        private int[]  _pending;
//        private int[]  _changing;
//        private int    _pendingSize;
//        private int    _changingSize;
//
//        public Mask( int[] rgb )
//        {
//            var n = rgb.Length;
//            
//            _blank     = new bool[ n ];
//            _pending  = new int[ n ];
//            _changing = new int[ n ];
//
//            for ( int i = 0; i < n; i++ )
//            {
//                if ( Alpha( rgb[ i ] ) == 0 )
//                {
//                    _blank[ i ]               = true;
//                    _pending[ _pendingSize ] = i;
//                    _pendingSize++;
//                }
//            }
//        }
//
//        public bool IsBlank( int index )
//        {
//            return _blank[ index ];
//        }
//
//        public int RemoveIndex( int index )
//        {
//            if ( index >= _pendingSize ) throw new IndexOutOfRangeException( string.ValueOf( index ) );
//            
//            var value = _pending[ index ];
//            
//            _pendingSize--;
//            _pending[ index ] = _pending[ _pendingSize ];
//
//            return value;
//        }
//
//        public class MaskIterator
//        {
//            private int _index;
//
//            public bool HasNext()
//            {
//                return _index < _pendingSize;
//            }
//
//            public int Next()
//            {
//                if ( _index >= _pendingSize ) throw new NoSuchElementException( String.valueOf( _index ) );
//
//                return _pending[ _index++ ];
//            }
//
//            public void MarkAsInProgress()
//            {
//                _index--;
//                _changing[ _changingSize ] = RemoveIndex( _index );
//                _changingSize++;
//            }
//
//            public void Reset()
//            {
//                _index = 0;
//
//                for ( int i = 0, n = _changingSize; i < n; i++ )
//                {
//                    _blank[ _changing[ i ] ] = false;
//                }
//
//                _changingSize = 0;
//            }
//        }
//
//        public static int Alpha( int argb )
//        {
//            return ( argb >> 24 ) & 0xff;
//        }
//    }
}