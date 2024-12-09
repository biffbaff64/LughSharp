// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.Images;

[PublicAPI]
public class PNGUtils
{
    /// <summary>
    /// Extracts the <c>Width</c> and <c>Height</c> from a PNG file.
    /// </summary>
    /// <remarks>
    /// Adapted from code obtained elsewhere. I'm not sure of where, and will credit
    /// the original author when I have corrected this.
    /// </remarks>
    public static ( int width, int height ) GetPNGWidthHeight( FileInfo file )
    {
        if ( file.Extension.ToLower() != ".png" )
        {
            throw new GdxRuntimeException( $"PNG files ONLY!: ({file.Name})" );
        }

        var br = new BinaryReader( File.OpenRead( file.Name ) );

        br.BaseStream.Position = 16;

        var widthbytes  = new byte[ sizeof( int ) ];
        var heightbytes = new byte[ sizeof( int ) ];

        for ( var i = 0; i < sizeof( int ); i++ )
        {
            widthbytes[ sizeof( int ) - 1 - i ] = br.ReadByte();
        }

        for ( var i = 0; i < sizeof( int ); i++ )
        {
            heightbytes[ sizeof( int ) - 1 - i ] = br.ReadByte();
        }

        return ( BitConverter.ToInt32( widthbytes, 0 ), BitConverter.ToInt32( heightbytes, 0 ) );
    }

    // Bytes 0 - 7 : Signature. Will always be 0x89504E470D0A1A0A
    // Bytes 8 -   : A series of chunks //TODO:
    // ========================================================================
    
    /// <summary>
    /// The PNG signature is eight bytes in length and contains information
    /// used to identify a file or data stream as conforming to the PNG
    /// specification.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct PngSignature
    {
        public byte[] Signature { get; set; } // Identifier (always 0x89504E470D0A1A0A)
    }

    /// <summary>
    /// PNG File IHDR Structure. The header chunk contains information on the image data
    /// stored in the PNG file. This chunk must be the first chunk in a PNG data stream
    /// and immediately follows the PNG signature. The header chunk data area is 13 bytes
    /// in length.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct IHDRChunk
    {
        public uint Width       { get; set; } // Width of image in pixels
        public uint Height      { get; set; } // Height of image in pixels
        public byte BitDepth    { get; set; } // Bits per pixel or per sample
        public byte ColorType   { get; set; } // Color interpretation indicator
        public byte Compression { get; set; } // Compression type indicator
        public byte Filter      { get; set; } // Filter type indicator
        public byte Interlace   { get; set; } // Type of interlacing scheme used
    }

    /// <summary>
    /// PNG File Chunk Structure.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct PngChunk
    {
        public uint   DataLength { get; set; } // Size of Data field in bytes
        public uint   Type       { get; set; } // Code identifying the type of chunk
        public byte[] Data       { get; set; } // The actual data stored by the chunk
        public uint   Crc        { get; set; } // CRC-32 value of the Type and Data fields
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct PaletteChunkEntry
    {
        public byte Red   { get; set; } // Red component (0 = black, 255 = maximum)
        public byte Green { get; set; } // Green component (0 = black, 255 = maximum)
        public byte Blue  { get; set; } // Blue component (0 = black, 255 = maximum)
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BackgroundChunkEntry
    {
        public byte Index { get; set; } // Index of background color in palette
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct GrayScaleBackgroundChunkEntry
    {
        public ushort Value { get; set; } // Background level value
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TrueColorBackgroundChunkEntry
    {
        public ushort  Red   { get; set; } // Red background sample value
        public ushort  Green { get; set; } // Green background sample value
        public ushort Blue  { get; set; } // Blue background sample value
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct ChromaChunkEntry
    {
        public uint WhitePointX { get; set; } // White Point x value
        public uint WhitePointY { get; set; } // White Point y value  */
        public uint RedX        { get; set; } // Red x value
        public uint RedY        { get; set; } // Red y value
        public uint GreenX      { get; set; } // Green x value
        public uint GreenY      { get; set; } // Green y value
        public uint BlueX       { get; set; } // Blue x value
        public uint BlueY       { get; set; } // Blue y value
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct GammaChunkEntry
    {
        public uint Gamma { get; set; } // Gamma value
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct HistogramChunkEntry
    {
        public ushort[] Histogram { get; set; } // Histogram data
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct PixelsPerUnitChunkEntry
    {
        public uint PixelsPerUnitX { get; set; } // Pixels per unit, X axis
        public uint PixelsPerUnitY { get; set; } // Pixels per unit, Y axis
        public byte UnitSpecifier  { get; set; } // 0 = unknown, 1 = meter
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct SigBitChunkEntry0
    {
        public byte GrayscaleBits { get; set; } // Gray-scale (ColorType 0) significant bits
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct SigBitChunkEntry23
    {
        public byte RedBits   { get; set; } // Red significant bits
        public byte GreenBits { get; set; } // Green significant bits
        public byte BlueBits  { get; set; } // Blue significant bits  */
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct SigBitChunkEntry4
    {
        public byte GrayscaleBits { get; set; } // Gray-scale significant bits
        public byte AlphaBits     { get; set; } // Alpha channel significant bits
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct SigBitChunkEntry6
    {
        public byte RedBits   { get; set; } // Red significant bits
        public byte GreenBits { get; set; } // Green significant bits
        public byte BlueBits  { get; set; } // Blue significant bits  */
        public byte AlphaBits { get; set; } // Alpha channel significant bits
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TextChunkEntry
    {
        public char[] Keyword       { get; set; } // Type of information stored in Text
        public byte   NullSeparator { get; set; } // NULL character used a delimiter
        public char[] Text          { get; set; } // Textual data
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TimeChunkEntry
    {
        public ushort Year   { get; set; } // Year value (such as 1996)
        public byte   Month  { get; set; } // Month value (1-12)
        public byte   Day    { get; set; } // Day value (1-31)
        public byte   Hour   { get; set; } // Hour value (0-23)
        public byte   Minute { get; set; } // Minute value (0-59)
        public byte   Second { get; set; } // Second value (0-60)
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TransparencyChunkEntryGrayScale
    {
        public ushort TransparencyValue { get; set; } // Transparent color
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TransparencyChunkEntryTrueColor
    {
        public ushort RedTransValue   { get; set; } // Red sample of transparent color
        public ushort GreenTransValue { get; set; } // Green sample of transparent color
        public ushort BlueTransValue  { get; set; } // Blue sample of transparent color
    }

    /// <summary>
    /// 
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct TransparencyChunkEntryIndexed
    {
        public byte[] TransparencyValues { get; set; } // Transparent colors
    }
}