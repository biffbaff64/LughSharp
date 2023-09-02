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

using JetBrains.Annotations;

namespace LibGDXSharp.Assets;

[PublicAPI]
public class AssetDescriptor
{
    public Type                   Type       { get; set; }
    public string?                FilePath   { get; set; }
    public AssetLoaderParameters Parameters { get; set; }
    public FileInfo?              File       { get; set; }

    public AssetDescriptor()
    {
        Type       = null!;
        FilePath   = string.Empty;
        Parameters = null!;
        File       = null!;
    }

    /// <summary>
    /// </summary>
    /// <param name="filepath"></param>
    /// <param name="assetType"></param>
    /// <param name="parameters"></param>
    public AssetDescriptor( string? filepath, Type assetType, AssetLoaderParameters parameters )
    {
        ArgumentNullException.ThrowIfNull( filepath );

        Type       = assetType;
        FilePath   = filepath.Replace( '\\', '/' );
        Parameters = parameters;
        File       = new FileInfo( Path.GetFileName( filepath ) );
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="assetType"></param>
    /// <param name="parameters"></param>
    public AssetDescriptor( FileInfo file, Type assetType, AssetLoaderParameters parameters )
    {
        Type       = assetType;
        FilePath   = file.FullName.Replace( '\\', '/' );
        Parameters = parameters;
        File       = file;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"{FilePath}, {Type.FullName}";
    }
}
