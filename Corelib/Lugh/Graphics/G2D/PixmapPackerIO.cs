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

using Corelib.Lugh.Graphics.Images;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// PixmapPacker I/O, saves PixmapPackers to files.
/// </summary>
[PublicAPI]
public partial class PixmapPackerIO
{
    /// <summary>
    /// Saves the provided PixmapPacker to the provided file. The resulting
    /// file will use the standard TextureAtlas file format and can be loaded
    /// by TextureAtlas as if it had been created using TexturePacker.
    /// <para>
    /// Default <see cref="SaveParameters"/> will be used.
    /// </para>
    /// </summary>
    /// <param name="file">
    /// the file to which the atlas descriptor will be written,
    /// images will be written as siblings
    /// </param>
    /// <param name="packer"> the PixmapPacker to be written </param>
    /// <exception cref="IOException"> if the atlas file can not be written </exception>
    public static void Save( FileInfo file, PixmapPacker packer )
    {
        Save( file, packer, new SaveParameters() );
    }

    /// <summary>
    /// Saves the provided PixmapPacker to the provided file. The resulting file will use the
    /// standard TextureAtlas file format and can be loaded by TextureAtlas as if it had been
    /// created using TexturePacker.
    /// </summary>
    /// <param name="file">
    /// the file to which the atlas descriptor will be written, images will be written as siblings
    /// </param>
    /// <param name="packer"> the PixmapPacker to be written </param>
    /// <param name="parameters"> the SaveParameters specifying how to save the PixmapPacker </param>
    /// @throws IOException if the atlas file can not be written
    public static void Save( FileInfo file, PixmapPacker packer, SaveParameters parameters )
    {
        var writer = new StreamWriter( file.FullName ); //= file.writer( false );
        var index  = 0;

        foreach ( var page in packer.Pages )
        {
            if ( page.Rects.Count > 0 )
            {
                var pageFile = new FileInfo( $"{file.Name}_{++index}{parameters.Format.Extension}" );

                switch ( parameters.Format.FType )
                {
                    case ImageFormat.FCIM:
                    {
                        PixmapIO.WriteCIM( pageFile, page.Image );

                        break;
                    }

                    case ImageFormat.FPNG:
                    {
                        PixmapIO.WritePNG( pageFile, page.Image );

                        break;
                    }
                }

                writer.Write( "\n" );
                writer.Write( $"{pageFile.Name}\n" );
                writer.Write( $"size: {page.Image.Width}, {page.Image.Height}\n" );
                writer.Write( $"format: {packer.PageFormat.ToString()}\n" );
                writer.Write( $"filter: {parameters.MinFilter}, {parameters.MagFilter}\n" );
                writer.Write( "repeat: none\n" );

                foreach ( var name in page.Rects.Keys )
                {
                    var imageIndex = -1;
                    var imageName  = name;

                    if ( parameters.UseIndexes )
                    {
                        var rx = MyRegex();

                        var matches = rx.Matches( imageName );

                        if ( matches.Count > 0 )
                        {
                            // The image filename
                            imageName = matches[ 1 ].Name;

                            // The number at the end of the image filename, or -1 if none.
                            imageIndex = int.Parse( matches[ 2 ].Name );
                        }
                    }

                    writer.Write( imageName + "\n" );

                    var rect = page.Rects[ name ];

                    if ( rect != null )
                    {
                        writer.Write( "  rotate: false\n" );
                        writer.Write( $"  xy: {( int ) rect.X}, {( int ) rect.Y}\n" );
                        writer.Write( $"  size: {( int ) rect.Width}, {( int ) rect.Height}\n" );

                        if ( rect.Splits != null )
                        {
                            writer.Write( $"  split: {rect.Splits[ 0 ]}, "
                                        + $"{rect.Splits[ 1 ]}, "
                                        + $"{rect.Splits[ 2 ]}, "
                                        + $"{rect.Splits[ 3 ]}\n" );

                            if ( rect.Pads != null )
                            {
                                writer.Write( $"  pad: {rect.Pads[ 0 ]}, "
                                            + $"{rect.Pads[ 1 ]}, "
                                            + $"{rect.Pads[ 2 ]}, "
                                            + $"{rect.Pads[ 3 ]}\n" );
                            }
                        }

                        writer.Write( $"  orig: {rect.OriginalWidth}, {rect.OriginalHeight}\n" );

                        writer.Write( $"  offset: {rect.OffsetX}, "
                                    + $"{( int ) ( rect.OriginalHeight -
                                                   rect.Height - rect.OffsetY )}\n" );

                        writer.Write( $"  index: {imageIndex}\n" );
                    }
                }
            }
        }

        writer.Close();
    }

    // ========================================================================
    // ========================================================================

    [PublicAPI]
    public class ImageFormat
    {
        public const int FCIM = 0;
        public const int FPNG = 1;

        public static readonly ImageFormat CIM = new( ".cim", FCIM );
        public static readonly ImageFormat PNG = new( ".png", FPNG );

        private ImageFormat( string extension, int ftype )
        {
            Extension = extension;
            FType     = ftype;
        }

        public string Extension { get; }
        public int    FType     { get; }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Additional parameters which will be used when writing a PixmapPacker.
    /// </summary>
    [PublicAPI]
    public struct SaveParameters
    {
        public ImageFormat           Format     { get; set; }
        public Texture.TextureFilter MinFilter  { get; set; }
        public Texture.TextureFilter MagFilter  { get; set; }
        public bool                  UseIndexes { get; set; }

        public SaveParameters()
        {
            Format     = ImageFormat.PNG;
            MinFilter  = Texture.TextureFilter.Nearest;
            MagFilter  = Texture.TextureFilter.Nearest;
            UseIndexes = false;
        }

        public SaveParameters( ImageFormat format,
                               Texture.TextureFilter minFilter,
                               Texture.TextureFilter magFilter,
                               bool useIndexes )
        {
            Format     = format;
            MinFilter  = minFilter;
            MagFilter  = magFilter;
            UseIndexes = useIndexes;
        }
    }

    [GeneratedRegex("(.+)_(\\d+)$")]
    private static partial Regex MyRegex();
}
