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

using System.Text.RegularExpressions;

namespace LibGDXSharp.Graphics.G2D;

/// <summary>
/// PixmapPacker I/O, saves PixmapPackers to files.
/// </summary>
[PublicAPI]
public class PixmapPackerIO
{
    [PublicAPI]
    public class ImageFormat
    {
        public const int FCIM = 0;
        public const int FPNG = 1;

        public readonly static ImageFormat CIM = new( ".cim", FCIM );
        public readonly static ImageFormat PNG = new( ".png", FPNG );

        public string Extension { get; }
        public int    FType     { get; }

        private ImageFormat( string extension, int ftype )
        {
            Extension = extension;
            FType     = ftype;
        }
    }

    /// <summary>
    /// Additional parameters which will be used when writing a PixmapPacker.
    /// </summary>
    [PublicAPI]
    public struct SaveParameters
    {
        public PixmapPackerIO.ImageFormat Format     { get; set; }
        public TextureFilter              MinFilter  { get; set; }
        public TextureFilter              MagFilter  { get; set; }
        public bool                       UseIndexes { get; set; }

        public SaveParameters()
        {
            this.Format     = ImageFormat.PNG;
            this.MinFilter  = TextureFilter.Nearest;
            this.MagFilter  = TextureFilter.Nearest;
            this.UseIndexes = false;
        }

        public SaveParameters( ImageFormat format,
                               TextureFilter minFilter,
                               TextureFilter magFilter,
                               bool useIndexes )
        {
            this.Format     = format;
            this.MinFilter  = minFilter;
            this.MagFilter  = magFilter;
            this.UseIndexes = useIndexes;
        }
    }

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
    public void Save( FileInfo file, PixmapPacker packer )
    {
        Save( file, packer, new SaveParameters() );
    }

    /// <summary>
    /// Saves the provided PixmapPacker to the provided file. The resulting file will use the standard TextureAtlas file format and
    /// can be loaded by TextureAtlas as if it had been created using TexturePacker.
    /// </summary>
    /// <param name="file">
    /// the file to which the atlas descriptor will be written, images will be written as siblings
    /// </param>
    /// <param name="packer"> the PixmapPacker to be written </param>
    /// <param name="parameters"> the SaveParameters specifying how to save the PixmapPacker </param>
    /// @throws IOException if the atlas file can not be written
    public void Save( FileInfo file, PixmapPacker packer, SaveParameters parameters )
    {
        var writer = new StreamWriter( file.FullName ); //= file.writer( false );
        var index  = 0;

        foreach ( PixmapPacker.Page page in packer.Pages )
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
                        var rx = new Regex( "(.+)_(\\d+)$" );

                        MatchCollection matches = rx.Matches( imageName );

                        if ( matches.Count > 0 )
                        {
                            // The image filename
                            imageName = matches[ 1 ].Name;

                            // The number at the end of the image filename, or -1 if none.
                            imageIndex = int.Parse( matches[ 2 ].Name );
                        }
                    }

                    writer.Write( imageName + "\n" );
                    
                    PixmapPacker.PixmapPackerRectangle? rect = page.Rects[ name ];

                    if ( rect != null )
                    {
                        writer.Write( "  rotate: false\n" );
                        writer.Write( $"  xy: {( int )rect.X}, {( int )rect.Y}\n" );
                        writer.Write( $"  size: {( int )rect.Width}, {( int )rect.Height}\n" );

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
                                    + $"{( int )( rect.OriginalHeight - 
                                                  rect.Height - rect.OffsetY )}\n" );
                        writer.Write( $"  index: {imageIndex}\n" );
                    }
                }
            }
        }

        writer.Close();
    }
}
