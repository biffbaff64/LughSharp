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
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Extensions.Source.Tools.TexturePacker;

[PublicAPI]
public class ColorBleedEffect
{
    private static readonly int[] offsets = [ -1, -1, 0, -1, 1, -1, -1, 0, 1, 0, -1, 1, 0, 1, 1, 1 ];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="maxIterations"></param>
    /// <returns></returns>
    public static Image< Rgba32 > ProcessImage( Image< Rgba32 > image, int maxIterations )
    {
        var width  = image.Width;
        var height = image.Height;

        var processedImage = image.Clone();

        processedImage.Mutate( context =>
        {
            var pixels = new Rgba32[ width * height ];
            processedImage.CopyPixelDataTo( pixels );

            var mask        = new Mask( pixels );
            var iterations  = 0;
            var lastPending = -1;

            while ( mask.PendingSize > 0 && mask.PendingSize != lastPending && iterations < maxIterations )
            {
                lastPending = mask.PendingSize;
                ExecuteIteration( pixels, mask, width, height );
                iterations++;
            }

            // Copy the processed pixels back to the image
            for ( var y = 0; y < height; y++ )
            {
                for ( var x = 0; x < width; x++ )
                {
                    processedImage[ x, y ] = pixels[ y * width + x ];
                }
            }
        } );

        return processedImage;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pixels"></param>
    /// <param name="mask"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private static void ExecuteIteration( Rgba32[] pixels, Mask mask, int width, int height )
    {
        var iterator = new Mask.MaskIterator( mask );

        while ( iterator.HasNext() )
        {
            var pixelIndex = iterator.Next();
            var x          = pixelIndex % width;
            var y          = pixelIndex / width;
            var r          = 0;
            var g          = 0;
            var b          = 0;
            var count      = 0;

            for ( var i = 0; i < offsets.Length; i += 2 )
            {
                var column = x + offsets[ i ];
                var row    = y + offsets[ i + 1 ];

                if ( column < 0 || column >= width || row < 0 || row >= height )
                {
                    column = x;
                    row    = y;
                    continue;
                }

                var currentPixelIndex = GetPixelIndex( width, column, row );

                if ( !mask.IsBlank( currentPixelIndex ) )
                {
                    var pixel = pixels[ currentPixelIndex ];

                    r += pixel.R;
                    g += pixel.G;
                    b += pixel.B;

                    count++;
                }
            }

            if ( count != 0 )
            {
                pixels[ pixelIndex ] = new Rgba32( ( byte )( r / count ), ( byte )( g / count ), ( byte )( b / count ), 0 );
                iterator.MarkAsInProgress();
            }
        }

        iterator.Reset();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private static int GetPixelIndex( int width, int x, int y )
    {
        return y * width + x;
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    private class Mask
    {
        public int PendingSize { get; private set; }

        private readonly bool[] blank;
        private readonly int[]  pending;
        private readonly int[]  changing;
        private          int    changingSize;

        public Mask( Rgba32[] pixels )
        {
            var n = pixels.Length;

            blank    = new bool[ n ];
            pending  = new int[ n ];
            changing = new int[ n ];

            for ( var i = 0; i < n; i++ )
            {
                if ( pixels[ i ].A == 0 )
                {
                    blank[ i ]             = true;
                    pending[ PendingSize ] = i;
                    PendingSize++;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsBlank( int index )
        {
            return blank[ index ];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private int RemoveIndex( int index )
        {
            if ( index >= PendingSize )
            {
                throw new IndexOutOfRangeException( index.ToString() );
            }

            var value = pending[ index ];

            PendingSize--;
            pending[ index ] = pending[ PendingSize ];

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mask"></param>
        [PublicAPI]
        public class MaskIterator( Mask mask )
        {
            private int index;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool HasNext()
            {
                return index < mask.PendingSize;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public int Next()
            {
                if ( index >= mask.PendingSize )
                {
                    throw new InvalidOperationException( index.ToString() );
                }

                return mask.pending[ index++ ];
            }

            /// <summary>
            /// 
            /// </summary>
            public void MarkAsInProgress()
            {
                index--;
                mask.changing[ mask.changingSize ] = mask.RemoveIndex( index );
                mask.changingSize++;
            }

            /// <summary>
            /// 
            /// </summary>
            public void Reset()
            {
                index = 0;

                for ( var i = 0; i < mask.changingSize; i++ )
                {
                    mask.blank[ mask.changing[ i ] ] = false;
                }

                mask.changingSize = 0;
            }
        }
    }
}