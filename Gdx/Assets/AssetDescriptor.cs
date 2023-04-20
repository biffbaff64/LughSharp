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
        public string                 FileName   { get; init; }
        public IAssetLoaderParameters Parameters { get; init; }
        public FileHandle?            File       { get; set; }

        public AssetDescriptor()
        {
            FileName   = string.Empty;
            Type       = null!;
            Parameters = null!;
            File       = null!;
        }

        /// <summary>
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="assetType"></param>
        /// <param name="parameters"></param>
        public AssetDescriptor( string fileName, Type assetType, IAssetLoaderParameters parameters )
        {
            FileName   = fileName.Replace( '\\', '/' );
            Type       = assetType;
            Parameters = parameters;
            File       = null!;
        }

        /// <summary>
        /// </summary>
        /// <param name="file"></param>
        /// <param name="assetType"></param>
        /// <param name="parameters"></param>
        public AssetDescriptor( FileHandle file, Type assetType, IAssetLoaderParameters parameters )
        {
            FileName   = file.Path().Replace( '\\', '/' );
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
            sb.Append( FileName );
            sb.Append( ", " );
            sb.Append( Type.FullName );

            return sb.ToString();
        }
    }
}
