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


using File = System.IO.File;

namespace LughSharp.LibCore.Assets.Loaders;

/// <summary>
/// <see cref="AssetLoader"/> for <see cref="ShaderProgram"/> instances loaded from
/// text files. If the file suffix is ".vert", it is assumed to be a vertex shader,
/// and a fragment shader is found using the same file name with a ".frag" suffix.
/// And vice versa if the file suffix is ".frag". These default suffixes can be changed
/// in the ShaderProgramLoader constructor.
/// <para>
/// For all other file suffixes, the same file is used for both (and therefore should
/// internally distinguish between the programs using preprocessor directives and
/// <see cref="ShaderProgram.PrependVertexCode"/> and <see cref="ShaderProgram.PrependFragmentCode"/>).
/// </para>
/// <para>
/// The above default behavior for finding the files can be overridden by explicitly
/// setting the file names in a <see cref="ShaderProgramParameter"/>. The parameter
/// can also be used to prepend code to the programs.
/// </para>
/// </summary>
[PublicAPI]
public class ShaderProgramLoader
    : AsynchronousAssetLoader< ShaderProgram, ShaderProgramLoader.ShaderProgramParameter >
{
    private readonly string _fragmentFileSuffix = ".frag";
    private readonly string _vertexFileSuffix   = ".vert";

    /// <summary>
    /// Creates a new ShaderProgramLoader using the provided <see cref="IFileHandleResolver"/>
    /// </summary>
    public ShaderProgramLoader( IFileHandleResolver resolver ) : base( resolver )
    {
        Logger.CheckPoint();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ShaderProgramLoader"/> class with
    /// the specified <see cref="IFileHandleResolver"/>, vertex shader file suffix, and
    /// fragment shader file suffix.
    /// </summary>
    /// <param name="resolver">The file resolver to use for resolving shader file paths.</param>
    /// <param name="vertexFileSuffix">The suffix of the vertex shader files.</param>
    /// <param name="fragmentFileSuffix">The suffix of the fragment shader files.</param>
    public ShaderProgramLoader( IFileHandleResolver resolver, string vertexFileSuffix, string fragmentFileSuffix )
        : base( resolver )
    {
        _vertexFileSuffix   = vertexFileSuffix;
        _fragmentFileSuffix = fragmentFileSuffix;
    }

    /// <inheritdoc />
    public override List< AssetDescriptor > GetDependencies( string? filename, FileInfo? file, AssetLoaderParameters? p )
    {
        return null!;
    }

    /// <inheritdoc />
    public override void LoadAsync( AssetManager manager, FileInfo? file, ShaderProgramParameter? parameter )
    {
    }

    /// <inheritdoc />
    public override object LoadSync( AssetManager? manager,
                                     FileInfo? file,
                                     ShaderProgramParameter? parameter )
    {
        ArgumentNullException.ThrowIfNull( file?.Name );

        string? vertFileName = null;
        string? fragFileName = null;

        if ( parameter != null )
        {
            if ( parameter.VertexFile != null )
            {
                vertFileName = parameter.VertexFile;
            }

            if ( parameter.FragmentFile != null )
            {
                fragFileName = parameter.FragmentFile;
            }
        }

        if ( ( vertFileName == null ) && file.Name.EndsWith( _fragmentFileSuffix, StringComparison.Ordinal ) )
        {
            vertFileName = file.Name[ ..^_fragmentFileSuffix.Length ] + _vertexFileSuffix;
        }

        if ( ( fragFileName == null ) && file.Name.EndsWith( _vertexFileSuffix, StringComparison.Ordinal ) )
        {
            fragFileName = file.Name[ ..^_vertexFileSuffix.Length ] + _fragmentFileSuffix;
        }

        var vertexFile   = vertFileName == null ? file : Resolve( vertFileName );
        var fragmentFile = fragFileName == null ? file : Resolve( fragFileName );

        var vertexCode = File.ReadAllText( Path.GetFullPath( vertexFile.Name ) );

        var fragmentCode = vertexFile.Equals( fragmentFile )
                               ? vertexCode
                               : File.ReadAllText( Path.GetFullPath( fragmentFile.Name ) );

        if ( parameter != null )
        {
            if ( parameter.PrependVertexCode != null )
            {
                vertexCode = parameter.PrependVertexCode + vertexCode;
            }

            if ( parameter.PrependFragmentCode != null )
            {
                fragmentCode = parameter.PrependFragmentCode + fragmentCode;
            }
        }

        var shaderProgram = new ShaderProgram( vertexCode, fragmentCode );

        if ( ( ( parameter == null ) || parameter.LogOnCompileFailure )
          && !shaderProgram.IsCompiled )
        {
            Logger.Error( $"ShaderProgram {file.Name} failed to compile:\n{shaderProgram.ShaderLog}" );
        }

        return shaderProgram;
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Loading parameters for loading <see cref="ShaderProgram"/>s.
    /// </summary>
    [PublicAPI]
    public class ShaderProgramParameter : AssetLoaderParameters
    {
        /// <summary>
        /// File name to be used for the vertex program instead of the default determined
        /// by the file name used to submit this asset to AssetManager.
        /// </summary>
        public string? VertexFile { get; set; }

        /// <summary>
        /// File name to be used for the fragment program instead of the default
        /// determined by the file name used to submit this asset to AssetManager.
        /// </summary>
        public string? FragmentFile { get; set; }

        /// <summary>
        /// Whether to log (at the error level) the shader's log if it fails to
        /// compile. Default true.
        /// </summary>
        public bool LogOnCompileFailure { get; set; } = true;

        /// <summary>
        /// Code that is always added to the vertex shader code. This is added as-is,
        /// and you should include a newline (`\n`) if needed.
        /// <see cref="ShaderProgram.PrependVertexCode"/> is placed before this code.
        /// </summary>
        public string? PrependVertexCode { get; set; }

        /// <summary>
        /// Code that is always added to the fragment shader code. This is added as-is,
        /// and you should include a newline (`\n`) if needed.
        /// <see cref="ShaderProgram.PrependFragmentCode"/> is placed before this code.
        /// </summary>
        public string? PrependFragmentCode { get; set; }
    }
}
