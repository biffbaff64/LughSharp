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

namespace LibGDXSharp.G2D;

/// <summary>
/// loads <see cref="PolygonRegion"/>s.
/// </summary>
[PublicAPI]
public class PolygonRegionLoader
    : SynchronousAssetLoader< PolygonRegion, PolygonRegionLoader.PolygonRegionParameters >
{
    [PublicAPI]
    public class PolygonRegionParameters : AssetLoaderParameters
    {
        /// <summary>
        /// what the line starts with that contains the file name of the
        /// texture for this <tt>PolygonRegion</tt>.
        /// </summary>
        public string? texturePrefix = "i ";

        /// <summary>
        /// what buffer size of the reader should be used to read the
        /// <tt>texturePrefix</tt> line.
        /// </summary>
        public int readerBuffer = 1024;

        /// <summary>
        /// the possible file name extensions of the texture file.
        /// </summary>
        public readonly string[] textureExtensions =
        {
            "png", "PNG", "jpeg", "JPEG", "jpg", "JPG", "cim", "CIM",
            "etc1", "ETC1", "ktx", "KTX", "zktx", "ZKTX"
        };
    }

    private PolygonRegionParameters _defaultParameters = new();
    private EarClippingTriangulator _triangulator      = new();

    public PolygonRegionLoader()
        : this( new InternalFileHandleResolver() )
    {
    }

    public PolygonRegionLoader( IFileHandleResolver resolver )
        : base( resolver )
    {
    }

    public override PolygonRegion Load( AssetManager manager,
                                        string? fileName,
                                        FileInfo? file,
                                        PolygonRegionParameters parameter )
    {
        ArgumentNullException.ThrowIfNull( fileName );

        var texture = manager.Get< Texture >( manager.GetDependencies( fileName )!.First() );

        return Load( new TextureRegion( texture ), file! );
    }

    /// <summary>
    /// If the PSH file contains a line starting with <see cref="PolygonRegionParameters.texturePrefix"/>,
    /// an <see cref="AssetDescriptor"/> for the file referenced on that line will be added to the returned
    /// Array. Otherwise a sibling of the given file with the same name and the first found extension
    /// in <see cref="PolygonRegionParameters.textureExtensions"/>" will be used. If no suitable file is
    ///
    /// found, the returned Array will be empty.
    /// </summary>
    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters? parameters )
    {
        ArgumentNullException.ThrowIfNull( file );

        parameters ??= _defaultParameters;

        string? image = null;

        try
        {
            var reader = new StreamReader( file.FullName );

            for ( var line = reader.ReadLine(); line != null; line = reader.ReadLine() )
            {
                if ( line.StartsWith( ( ( PolygonRegionParameters )parameters ).texturePrefix! ) )
                {
                    image = line.Substring( ( ( PolygonRegionParameters )parameters ).texturePrefix!.Length );

                    break;
                }
            }

            reader.Close();
        }
        catch ( IOException e )
        {
            throw new GdxRuntimeException( "Error reading " + fileName, e );
        }

        string siblingFilePath = string.Empty;
        
        if ( image == null )
        {
            var directory = Path.GetDirectoryName( fileName );
            var fileNoExt = Path.GetFileNameWithoutExtension( fileName );
            
            foreach ( var extension in ( ( PolygonRegionParameters )parameters ).textureExtensions )
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
            List< AssetDescriptor > deps = new( 1 )
            {
                new AssetDescriptor( new FileInfo( siblingFilePath ), typeof( Texture ) )
            };

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
}
