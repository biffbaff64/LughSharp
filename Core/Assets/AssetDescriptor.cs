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

namespace LibGDXSharp.Assets;

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
    public AssetDescriptor( string? filepath, Type assetType, AssetLoaderParameters parameters )
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
                            [AllowNull] AssetLoaderParameters parameters = null )
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
    public override string ToString() => $"{Filepath}, {AssetType.FullName}";
}
