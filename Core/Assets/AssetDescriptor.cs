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

using StringBuilder = System.Text.StringBuilder;

namespace LibGDXSharp.Assets;

// -------------------------------------------------------------------
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
// -------------------------------------------------------------------
public sealed class AssetDescriptor
{
    public Type                  Type       { get; init; }
    public string?               FilePath   { get; init; }
    public AssetLoaderParameters Parameters { get; init; }
    public FileInfo?             File       { get; set; }

    public AssetDescriptor()
    {
        FilePath   = string.Empty;
        Type       = null!;
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
        FilePath   = filepath?.Replace( '\\', '/' );
        Type       = assetType;
        Parameters = parameters;
        File       = null!;
    }

    /// <summary>
    /// </summary>
    /// <param name="file"></param>
    /// <param name="assetType"></param>
    /// <param name="parameters"></param>
    public AssetDescriptor( FileInfo file, Type assetType, AssetLoaderParameters parameters )
    {
        FilePath   = file.FullName.Replace( '\\', '/' );
        File       = file;
        Type       = assetType;
        Parameters = parameters;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public new string ToString()
    {
        var sb = new StringBuilder();
        sb.Append( FilePath );
        sb.Append( ", " );
        sb.Append( Type.FullName );

        return sb.ToString();
    }
}