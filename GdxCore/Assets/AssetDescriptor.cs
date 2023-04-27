using System.Diagnostics.CodeAnalysis;

using StringBuilder = System.Text.StringBuilder;

namespace LibGDXSharp.Assets
{
    /// <summary>
    /// </summary>
    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class AssetDescriptor
    {
        public Type                   Type       { get; init; }
        public string                 FilePath   { get; init; }
        public IAssetLoaderParameters Parameters { get; init; }
        public FileInfo?              File       { get; set; }

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
        public AssetDescriptor( string? filepath, Type assetType, IAssetLoaderParameters parameters )
        {
            FilePath   = filepath.Replace( '\\', '/' );
            Type       = assetType;
            Parameters = parameters;
            File       = null!;
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="assetType"></param>
        /// <param name="parameters"></param>
        public AssetDescriptor( FileInfo file, Type assetType, IAssetLoaderParameters parameters )
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
}
