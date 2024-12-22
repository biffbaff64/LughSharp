// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

namespace Corelib.Lugh.Assets.Loaders.Resolvers;

/// <summary>
/// This <see cref="IFileHandleResolver"/> uses a given list of <see cref="Resolution"/>s to
/// determine the best match based on the current back buffer size. An example of how this
/// resolver works:
/// <para>
/// Let's assume that we have only a single <see cref="Resolution"/>; added to this
/// resolver. This resolution has the following properties:
/// </para>
/// <ul>
///     <li>
///         <tt> portraitWidth = 1920</tt>
///     </li>
///     <li>
///         <tt> portraitHeight = 1080</tt>
///     </li>
///     <li>
///         <tt> folder = "1920x1080"</tt>
///     </li>
/// </ul>
/// <para>
/// One would now supply a file to be found to the resolver. For this example, we assume it is
/// <tt>"textures/walls/brick.png"</tt>. Since there is only a single <see cref="Resolution"/>,
/// this will be the best match for any screen size. The resolver will now try to find the file
/// in the following ways:
/// </para>
/// <ul>
///     <li>
///         <tt>"textures/walls/1920x1080/brick.png"</tt>
///     </li>
///     <li>
///         <tt>"textures/walls/brick.png"</tt>
///     </li>
/// </ul>
/// <para>
/// The files are ultimately resolved via the given <see cref="BaseResolver"/>. In case the
/// first version cannot be resolved, the fallback will try to search for the file without
/// the resolution folder.
/// </para>
/// </summary>
[PublicAPI]
public class ResolutionFileResolver : IFileHandleResolver
{
    protected readonly IFileHandleResolver BaseResolver;
    protected readonly Resolution[]        Descriptors;

    // ========================================================================

    /// <summary>
    /// Creates a <see cref="ResolutionFileResolver"/> based on a given
    /// <see cref="IFileHandleResolver"/> and a list of <see cref="Resolution"/>s.
    /// </summary>
    /// <param name="baseResolver">
    /// The <see cref="IFileHandleResolver"/> that will ultimately used to resolve the file.
    /// </param>
    /// <param name="descs">
    /// A list of <see cref="Resolution"/>s. At least one has to be supplied.
    /// </param>
    public ResolutionFileResolver( IFileHandleResolver baseResolver, params Resolution[] descs )
    {
        if ( descs.Length == 0 )
        {
            throw new ArgumentException( "At least one Resolution needs to be supplied." );
        }

        BaseResolver = baseResolver;
        Descriptors  = descs;
    }

    /// <inheritdoc />
    public FileInfo Resolve( string fileName )
    {
        var      bestResolution = Choose( Descriptors );
        FileInfo originalHandle = new( fileName );
        var      handle         = BaseResolver.Resolve( Resolve( originalHandle, bestResolution.AssetsFolder ) );

        if ( !handle.Exists )
        {
            handle = BaseResolver.Resolve( fileName );
        }

        return handle;
    }

    /// <summary>
    /// Resolves a file path based on the original file handle and a suffix.
    /// </summary>
    /// <param name="originalHandle">The original file handle.</param>
    /// <param name="suffix">The suffix to append to the file path.</param>
    /// <returns>The resolved file path.</returns>
    protected static string Resolve( FileInfo originalHandle, string suffix )
    {
        var parentstring = "";
        var parent       = originalHandle.Directory;

        if ( ( parent != null ) && !parent.Name.Equals( "" ) )
        {
            parentstring = parent + "/";
        }

        return parentstring + suffix + "/" + originalHandle.Name;
    }

    /// <summary>
    /// Chooses the best resolution from the provided resolutions based on the device's back buffer size.
    /// </summary>
    /// <param name="descs">The array of resolutions to choose from.</param>
    /// <returns>The best resolution.</returns>
    public static Resolution Choose( params Resolution[] descs )
    {
        var w = GdxApi.Graphics.BackBufferWidth;
        var h = GdxApi.Graphics.BackBufferHeight;

        // Prefer the shortest side.
        var best = descs[ 0 ];

        if ( w < h )
        {
            for ( int i = 0, n = descs.Length; i < n; i++ )
            {
                var other = descs[ i ];

                if ( ( w >= other.PortraitWidth )
                  && ( other.PortraitWidth >= best.PortraitWidth )
                  && ( h >= other.PortraitHeight )
                  && ( other.PortraitHeight >= best.PortraitHeight ) )
                {
                    best = descs[ i ];
                }
            }
        }
        else
        {
            for ( int i = 0, n = descs.Length; i < n; i++ )
            {
                var other = descs[ i ];

                if ( ( w >= other.PortraitHeight )
                  && ( other.PortraitHeight >= best.PortraitHeight )
                  && ( h >= other.PortraitWidth )
                  && ( other.PortraitWidth >= best.PortraitWidth ) )
                {
                    best = descs[ i ];
                }
            }
        }

        return best;
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Represents a resolution for an application.
    /// </summary>
    [PublicAPI]
    public class Resolution
    {
        /// <summary>
        /// Constructs a new instance of the <see cref="Resolution"/> class with
        /// the specified portrait width, portrait height, and assets folder.
        /// </summary>
        /// <param name="portraitWidth">The width of this resolution in portrait mode.</param>
        /// <param name="portraitHeight">The height of this resolution in portrait mode.</param>
        /// <param name="folder">The name of the folder where the assets which fit this resolution are located.</param>
        public Resolution( int portraitWidth, int portraitHeight, string folder )
        {
            PortraitWidth  = portraitWidth;
            PortraitHeight = portraitHeight;
            AssetsFolder   = folder;
        }

        /// <summary>
        /// Gets the width of this resolution in portrait mode.
        /// </summary>
        public int PortraitWidth { get; }

        /// <summary>
        /// Gets the height of this resolution in portrait mode.
        /// </summary>
        public int PortraitHeight { get; }

        /// <summary>
        /// Gets the folder where the assets for this resolution are located.
        /// </summary>
        public string AssetsFolder { get; }
    }
}
