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


namespace LughSharp.LibCore.Assets;

[PublicAPI]
public class AssetDescriptor
{
    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Creates an empty AssetDescriptor object.
    ///     Information will need providing before this object can be used.
    ///     - AssetType  - The Type of asset ( Texture, TextureAtlas, Sound, Pixmap etc. )
    ///     - Filepath   - The full path, including filename, of the asset.
    ///     - Parameters - The <see cref="AssetLoaderParameters" /> to use.
    ///     - File       - A <see cref="FileInfo" /> object holding file/path information
    /// </summary>
    public AssetDescriptor()
    {
        AssetType  = null!;
        Filepath   = string.Empty;
        Parameters = null!;
        File       = null!;
    }

    /// <summary>
    ///     Creates a new AssetDescriptor object.
    /// </summary>
    /// <param name="filepath"> The full path, including filename, of the asset. </param>
    /// <param name="assetType"> The Type of asset ( Texture, Pixmap, Audio, Atlas etc ). </param>
    /// <param name="parameters"> The <see cref="AssetLoaderParameters" /> to use. </param>
    public AssetDescriptor( string? filepath, Type assetType, AssetLoaderParameters? parameters )
    {
        ArgumentNullException.ThrowIfNull( filepath );

        AssetType  = assetType;
        Filepath   = filepath.Replace( '\\', '/' );
        Parameters = parameters;
        File       = new FileInfo( Path.GetFileName( filepath ) );
    }

    /// <summary>
    ///     Creates a new AssetDescriptor object.
    /// </summary>
    /// <param name="file"></param>
    /// <param name="assetType"></param>
    /// <param name="parameters">The loader parameters to use. Can be null.</param>
    public AssetDescriptor( FileInfo file,
                            Type assetType,
                            AssetLoaderParameters? parameters = null )
    {
        AssetType  = assetType;
        Filepath   = file.FullName.Replace( '\\', '/' );
        Parameters = parameters;
        File       = file;
    }

    public Type                   AssetType  { get; set; }
    public string                 Filepath   { get; set; }
    public AssetLoaderParameters? Parameters { get; set; }
    public FileInfo               File       { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Filepath}, {AssetType.FullName}";
    }
}
