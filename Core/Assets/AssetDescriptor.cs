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

[PublicAPI]
public class AssetDescriptor
{
    public string                FilePath   { get; set; }
    public FileInfo              File       { get; set; }
    public Type                  Type       { get; set; }
    public AssetLoaderParameters Parameters { get; set; }

    public AssetDescriptor()
    {
        Type       = null!;
        FilePath   = string.Empty;
        Parameters = null!;
        File       = null!;
    }

    public AssetDescriptor( Type? assetType,
                            AssetLoaderParameters? parameters,
                            string? filePath = null,
                            FileInfo? file = null )
    {
        if ( ( file == null ) && ( filePath == null ) )
        {
            throw new ArgumentException
                ( "no file or filepath supplied. At least one is required." );
        }

        ArgumentNullException.ThrowIfNull( assetType );
        ArgumentNullException.ThrowIfNull( parameters );

        this.Type       = assetType;
        this.Parameters = parameters;

        if ( filePath == null )
        {
            // filePath is null, file is not.
            this.FilePath = file!.FullName.Replace( '\\', '/' );
            this.File     = file;
        }
        else
        {
            // file may be null, filePath is not.
            this.FilePath = filePath.Replace( '\\', '/' );
            this.File     = file ?? new FileInfo( Path.GetFileName( filePath ) );
        }
    }

    public override string ToString()
    {
        return $"{FilePath}, {Type.FullName}";
    }
}
