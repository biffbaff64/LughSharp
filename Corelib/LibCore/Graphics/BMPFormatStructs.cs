// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

namespace Corelib.LibCore.Graphics;

[PublicAPI]
public class BMPFormatStructs
{
    // ------------------------------------------------------------------------
    /// <summary>
    /// BMP Header Structure.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BMPHeader
    {
        public ushort FileType           { get; set; }
        public uint   FileSize           { get; set; }
        public ushort Reserved1          { get; set; }
        public ushort Reserved2          { get; set; }
        public uint   OffsetToPixelArray { get; set; }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// BMP Info Header Structure.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BMPInfoHeader
    {
        public uint   HeaderSize      { get; set; }
        public int    Width           { get; set; }
        public int    Height          { get; set; }
        public ushort Planes          { get; set; }
        public ushort BitCount        { get; set; }
        public uint   Compression     { get; set; }
        public uint   ImageSize       { get; set; }
        public uint   XPixelsPerMeter { get; set; }
        public uint   YPixelsPerMeter { get; set; }
        public uint   ColorsUsed      { get; set; }
        public uint   ColorsImportant { get; set; }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// BMP Color Header Structure.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BMPColorHeader
    {
        public uint RedMask             { get; set; }
        public uint GreenMask           { get; set; }
        public uint BlueMask            { get; set; }
        public uint AlphaMask           { get; set; }
        public uint ColorSpaceType      { get; set; }
        public uint ColorSpaceEndpoints { get; set; }
        public uint GammaRed            { get; set; }
        public uint GammaGreen          { get; set; }
        public uint GammaBlue           { get; set; }
    }

    // ------------------------------------------------------------------------
    /// <summary>
    /// BMP Pixel Array Structure.
    /// </summary>
    [PublicAPI, StructLayout( LayoutKind.Sequential )]
    public struct BMPPixelArray
    {
        public byte[] PixelData { get; set; } // Array of color data for each pixel in the image.  
    }
}