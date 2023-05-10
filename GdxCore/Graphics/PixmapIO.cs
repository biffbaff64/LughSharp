using System.Diagnostics.CodeAnalysis;

namespace LibGDXSharp.Graphics;

/// <summary>
/// Write Pixmaps to various formats.
/// </summary>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public class PixmapIO
{
    /// <summary>
    /// Writes the <see cref="Pixmap"/> to the given file using a custom compression
    /// scheme. First three integers define the width, height and format, remaining
    /// bytes are zlib compressed pixels. To be able to load the Pixmap to a Texture,
    /// use ".cim" as the file suffix.
    /// <para>
    /// Throws a GdxRuntimeException if the Pixmap couldn't be written to the file.
    /// </para>
    /// </summary>
    /// <param name="file">The file to write the Pixmap to.</param>
    /// <param name="pixmap"></param>
    public static void WriteCIM( FileInfo file, Pixmap pixmap )
    {
        CIM.write( file, pixmap );
    }

    /// <summary>
    /// Reads the <see cref="Pixmap"/> from the given file, assuming the Pixmap was
    /// written with the <see cref="PixmapIO.WriteCIM(FileInfo, Pixmap)"/> method.
    /// <para>
    /// Throws a GdxRuntimeException in case the file couldn't be read.
    /// </para>
    /// </summary>
    /// <param name="file"> the file to read the Pixmap from  </param>
    public static Pixmap ReadCIM( FileInfo file )
    {
        return CIM.read( file );
    }

    /// <summary>
    /// Writes the pixmap as a PNG. See <see cref="PNG"/> to write out multiple PNGs
    /// with minimal allocation.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="pixmap"></param>
    /// <param name="compression">
    /// Sets the deflate compression level. Default is <see cref="Deflater.DEFAULT_COMPRESSION"/>
    /// </param>
    /// <param name="flipY">Flips the Pixmap vertically if true</param>
    public static void WritePNG( FileInfo file, Pixmap pixmap, int compression, bool flipY )
    {
        try
        {
            // Guess at deflated size.
            PNG writer = new PNG( ( int )( pixmap.Width * pixmap.Height * 1.5f ) );

            try
            {
                writer.setFlipY( flipY );
                writer.setCompression( compression );
                writer.write( file, pixmap );
            }
            finally
            {
                writer.dispose();
            }
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( "Error writing PNG: " + file, ex );
        }
    }

    /// <summary>
    /// Writes the pixmap as a PNG with compression. See <see cref="PNG"/> to configure the compression level, more efficiently flip the
    /// pixmap vertically, and to write out multiple PNGs with minimal allocation. 
    /// </summary>
    public static void WritePNG( FileInfo file, Pixmap pixmap )
    {
        WritePNG( file, pixmap, Deflater.DEFAULT_COMPRESSION, false );
    }
}
