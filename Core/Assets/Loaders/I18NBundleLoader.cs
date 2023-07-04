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

namespace LibGDXSharp.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="I18NBundle"/> instances.
/// The I18NBundle is loaded asynchronously.
/// <para>
/// Note that you can't load two bundles with the same base name and different
/// locale or encoding using the same <see cref="AssetManager"/>.
/// </para>
/// <para>
/// For example, if you try to load the 2 bundles below:-
/// </para>
/// 
/// <code>
/// manager.load(&quot;i18n/message&quot;, I18NBundle.class, new I18NBundleParameter(Locale.ITALIAN));
/// manager.load(&quot;i18n/message&quot;, I18NBundle.class, new I18NBundleParameter(Locale.ENGLISH));
/// </code>
/// 
/// <para>
/// the English bundle won't be loaded because the asset manager thinks they are
/// the same bundle since they have the same name.
/// </para>
/// <para>
/// If you want to load the English bundle to replace the Italian bundle you have to unload the Italian bundle first.
/// If you want to load the English bundle without replacing the Italian bundle you should use a second asset manager.
/// </para>
/// </summary>
public class I18NBundleLoader : AsynchronousAssetLoader< I18NBundle, I18NBundleLoader.I18NBundleParameter >
{
    private I18NBundle? _bundle;

    public I18NBundleLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public override void LoadAsync( AssetManager? manager, string? fileName, FileInfo? file, AssetLoaderParameters? parameter )
    {
        this._bundle = null;

        CultureInfo? locale;
        string?      encoding;

        if ( parameter == null )
        {
            locale   = CultureInfo.DefaultThreadCurrentCulture;
            encoding = null;
        }
        else
        {
            locale   = ( ( I18NBundleParameter )parameter ).Locale ?? CultureInfo.DefaultThreadCurrentCulture;
            encoding = ( ( I18NBundleParameter )parameter ).Encoding;
        }

        if ( encoding == null )
        {
            this._bundle = I18NBundle.CreateBundle( file, locale );
        }
        else
        {
            this._bundle = I18NBundle.CreateBundle( file, locale, encoding );
        }
    }

    public override I18NBundle LoadSync( AssetManager? manager,
                                         string? fileName,
                                         FileInfo? file,
                                         AssetLoaderParameters parameter )
    {
        I18NBundle? bundle = this._bundle;
        this._bundle = null;

        return bundle!;
    }

    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    [SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
    public sealed class I18NBundleParameter : AssetLoaderParameters
    {
        public CultureInfo? Locale   { get; private set; }
        public string?      Encoding { get; private set; }

        public I18NBundleParameter() : this( null )
        {
        }

        public I18NBundleParameter( CultureInfo? locale, string? encoding = null )
        {
            this.Locale   = locale;
            this.Encoding = encoding;
        }
    }
}
