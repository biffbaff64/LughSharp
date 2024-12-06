// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Assets;
using Corelib.Lugh.Assets.Loaders;
using Corelib.Lugh.Assets.Loaders.Resolvers;
using Corelib.Lugh.Graphics.Images;
using Corelib.Lugh.Maths;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Graphics.G2D;

/// <summary>
/// loads <see cref="PolygonRegion"/>s.
/// </summary>
[PublicAPI]
public class PolygonRegionLoader( IFileHandleResolver resolver )
    : SynchronousAssetLoader< PolygonRegion, PolygonRegionLoader.PolygonRegionParameters >( resolver )
{
    private readonly PolygonRegionParameters _defaultParameters = new();
    private readonly EarClippingTriangulator _triangulator      = new();

    // ========================================================================
    // ========================================================================

    public PolygonRegionLoader()
        : this( new InternalFileHandleResolver() )
    {
    }

    public override PolygonRegion Load( AssetManager manager,
                                        FileInfo? file,
                                        PolygonRegionParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( file?.Name );

        var texture = manager.Get( manager.GetDependencies( file.Name )!.First() ) as Texture;

        return Load( new TextureRegion( texture! ), file );
    }

    /// <summary>
    /// If the PSH file contains a line starting with <see cref="PolygonRegionParameters.TexturePrefix"/>,
    /// an <see cref="AssetDescriptor"/> for the file referenced on that line will be added to the returned
    /// Array. Otherwise a sibling of the given file with the same name and the first found extension
    /// in <see cref="PolygonRegionParameters.TextureExtensions"/>" will be used. If no suitable file is
    /// found, the returned Array will be empty.
    /// </summary>
    public override List< AssetDescriptor > GetDependencies< TP >( string fileName,
                                                                   FileInfo file,
                                                                   TP? parameters ) where TP : class
    {
        ArgumentNullException.ThrowIfNull( file );

        var p = parameters as PolygonRegionParameters ?? _defaultParameters;

        string? image = null;

        try
        {
            var reader = new StreamReader( file.FullName );

            for ( var line = reader.ReadLine(); line != null; line = reader.ReadLine() )
            {
                if ( line.StartsWith( p.TexturePrefix! ) )
                {
                    image = line.Substring( p.TexturePrefix!.Length );

                    break;
                }
            }

            reader.Close();
        }
        catch ( IOException e )
        {
            throw new GdxRuntimeException( "Error reading " + fileName, e );
        }

        var siblingFilePath = string.Empty;

        if ( image == null )
        {
            var directory = Path.GetDirectoryName( fileName );
            var fileNoExt = Path.GetFileNameWithoutExtension( fileName );

            foreach ( var extension in p.TextureExtensions )
            {
                siblingFilePath = Path.Combine( directory!, fileNoExt + extension );

                if ( File.Exists( siblingFilePath ) )
                {
                    image = siblingFilePath;
                }
            }
        }

        if ( image != null )
        {
            List< AssetDescriptor > deps = [ new( new FileInfo( siblingFilePath ), typeof( Texture ) ) ];

            return deps;
        }

        return null!;
    }

    /// <summary>
    /// Loads a PolygonRegion from a PSH (Polygon SHape) file. The PSH file format defines the polygon vertices before
    /// triangulation:
    /// <para>s 200.0, 100.0, ...</para>
    /// <para>
    /// Lines not prefixed with "s" are ignored. PSH files can be created with external tools, eg:
    /// </para>
    /// <para>http://www.codeandweb.com/physicseditor/</para>
    /// </summary>
    /// <param name="textureRegion"></param>
    /// <param name="file"> file handle to the shape definition file </param>
    public PolygonRegion Load( TextureRegion textureRegion, FileInfo file )
    {
        var reader = new StreamReader( file.Name );

        try
        {
            while ( true )
            {
                var line = reader.ReadLine();

                if ( line == null )
                {
                    break;
                }

                if ( line.StartsWith( "s" ) )
                {
                    // Read shape.
                    var polygonStrings = line.Substring( 1 ).Trim().Split( "," );
                    var vertices       = new float[ polygonStrings.Length ];

                    for ( int i = 0, n = vertices.Length; i < n; i++ )
                    {
                        vertices[ i ] = float.Parse( polygonStrings[ i ] );
                    }

                    // It would probably be better if PSH stored the vertices
                    // and triangles, then we don't have to triangulate here.
                    return new PolygonRegion(
                                             textureRegion,
                                             vertices,
                                             _triangulator.ComputeTriangles( vertices ).ToArray()
                                            );
                }
            }
        }
        catch ( IOException ex )
        {
            throw new GdxRuntimeException( $"Error reading polygon shape file: {file}", ex );
        }
        finally
        {
            reader.Close();
        }

        throw new GdxRuntimeException( "Polygon shape not found: " + file );
    }

    // ========================================================================

    [PublicAPI]
    public class PolygonRegionParameters : AssetLoaderParameters
    {
        /// <summary>
        /// the possible file name extensions of the texture file.
        /// </summary>
        public readonly string[] TextureExtensions =
        [
            "png", "PNG", "jpeg", "JPEG", "jpg", "JPG", "cim", "CIM",
            "etc1", "ETC1", "ktx", "KTX", "zktx", "ZKTX",
        ];

        /// <summary>
        /// what the line starts with that contains the file name of the
        /// texture for this <tt>PolygonRegion</tt>.
        /// </summary>
        public readonly string? TexturePrefix = "i ";

        /// <summary>
        /// what buffer size of the reader should be used to read the
        /// <tt>texturePrefix</tt> line.
        /// </summary>
        public int ReaderBuffer { get; set; } = 1024;
    }
}