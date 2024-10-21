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

using Corelib.LibCore.Graphics.GLUtils;
using Corelib.LibCore.Utils;

namespace Corelib.LibCore.Graphics;

/// <summary>
/// Factory class for creating instances of ITextureData based on file types.
/// Provides static methods to instantiate the right implementation (Pixmap, ETC1, KTX).
/// </summary>
[PublicAPI]
public static class TextureDataFactory
{
    /// <summary>
    /// Loads texture data from the specified file with default format and mipmaps settings.
    /// </summary>
    /// <param name="file">The file to load texture data from.</param>
    /// <param name="useMipMaps">Specifies whether to use mipmaps.</param>
    /// <returns>The loaded texture data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the file parameter is null.</exception>
    public static ITextureData LoadFromFile( FileInfo file, bool useMipMaps = true )
    {
        Logger.Checkpoint();

        return LoadFromFile( file, Pixmap.ColorFormat.RGBA8888, useMipMaps );
    }

    /// <summary>
    /// Loads texture data from the specified file with the given format and mipmaps settings.
    /// </summary>
    /// <param name="file">The file to load texture data from.</param>
    /// <param name="format">The format of the texture data.</param>
    /// <param name="useMipMaps">Specifies whether to use mipmaps.</param>
    /// <returns>The loaded texture data.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the file parameter is null.</exception>
    public static ITextureData LoadFromFile( FileInfo file, Pixmap.ColorFormat format, bool useMipMaps = true )
    {
        ArgumentNullException.ThrowIfNull( file );

        Logger.Checkpoint();

        ITextureData data;
        
        switch ( file.Extension.ToLower() )
        {
            // Common Information Model image file format.
            case ".cim":
            {
                data = new FileTextureData( file, PixmapIO.ReadCIM( file ), format, useMipMaps );
                break;
            }
            
            // Compressed Texture format for WebGL and OpenGL ES.
            case ".etc1":
            {
                data = new ETC1TextureData( file, useMipMaps );
                break;
            }
            
            // Kronos TeXture image file format for OpenGL and OpenGL ES.
            case ".ktx" or ".zktx":
            {
                data = new KtxTextureData( file, useMipMaps );
                break;
            }
            
            // Other supported image file formats, PNG, BMP
            // Unsure about JPG/JPEG and TGA
            case var _:
            {
                var pixmap = new Pixmap( file );
                data = new FileTextureData( file, pixmap, format, useMipMaps );
                break;
            }
        }

        return data;
    }
}