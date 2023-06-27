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

using System.Diagnostics.CodeAnalysis;

using File = System.IO.File;

namespace LibGDXSharp.Assets.Loaders;

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
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public sealed class ShaderProgramLoader
    : AsynchronousAssetLoader< ShaderProgram, ShaderProgramLoader.ShaderProgramParameter >
{
    private readonly string _vertexFileSuffix   = ".vert";
    private readonly string _fragmentFileSuffix = ".frag";

    public ShaderProgramLoader( IFileHandleResolver resolver ) : base( resolver )
    {
    }

    public ShaderProgramLoader( IFileHandleResolver resolver, string vertexFileSuffix, string fragmentFileSuffix )
        : base( resolver )
    {
        this._vertexFileSuffix   = vertexFileSuffix;
        this._fragmentFileSuffix = fragmentFileSuffix;
    }

    public override List< AssetDescriptor > GetDependencies( string? fileName,
                                                             FileInfo? file,
                                                             AssetLoaderParameters parameter )
    {
        return null!;
    }

    public override void LoadAsync( AssetManager? manager,
                                    string? fileName,
                                    FileInfo? file,
                                    AssetLoaderParameters parameter )
    {
    }

    public override ShaderProgram LoadSync( AssetManager? manager,
                                            string? fileName,
                                            FileInfo? file,
                                            AssetLoaderParameters? parameter )
    {
        ArgumentNullException.ThrowIfNull( fileName );

        string? vertFileName = null;
        string? fragFileName = null;

        if ( parameter != null )
        {
            if ( !string.ReferenceEquals( ( ( ShaderProgramParameter )parameter ).VertexFile, null ) )
            {
                vertFileName = ( ( ShaderProgramParameter )parameter ).VertexFile;
            }

            if ( !string.ReferenceEquals( ( ( ShaderProgramParameter )parameter ).FragmentFile, null ) )
            {
                fragFileName = ( ( ShaderProgramParameter )parameter ).FragmentFile;
            }
        }

        if ( string.ReferenceEquals( vertFileName, null )
             && fileName.EndsWith( _fragmentFileSuffix, StringComparison.Ordinal ) )
        {
//            vertFileName = fileName.Substring( 0, fileName.Length - _fragmentFileSuffix.Length ) + _vertexFileSuffix;
            vertFileName = fileName[ ..^_fragmentFileSuffix.Length ] + _vertexFileSuffix;
        }

        if ( string.ReferenceEquals( fragFileName, null )
             && fileName.EndsWith( _vertexFileSuffix, StringComparison.Ordinal ) )
        {
//            fragFileName = fileName.Substring( 0, fileName.Length - _vertexFileSuffix.Length ) + _fragmentFileSuffix;
            fragFileName = fileName[ ..^_vertexFileSuffix.Length ] + _fragmentFileSuffix;
        }

        FileInfo? vertexFile   = string.ReferenceEquals( vertFileName, null ) ? file : Resolve( vertFileName );
        FileInfo? fragmentFile = string.ReferenceEquals( fragFileName, null ) ? file : Resolve( fragFileName );

//        var vertexCode   = vertexFile!.ReadString();
//        var fragmentCode = vertexFile!.Equals( fragmentFile ) ? vertexCode : fragmentFile.ReadString();

        var vertexCode = File.ReadAllText( Path.GetFullPath( vertexFile!.Name ) );

        var fragmentCode = vertexFile.Equals( fragmentFile )
            ? vertexCode
            : File.ReadAllText( Path.GetFullPath( fragmentFile!.Name ) );

        if ( parameter != null )
        {
            if ( !string.ReferenceEquals( ( ( ShaderProgramParameter )parameter ).PrependVertexCode, null ) )
            {
                vertexCode = ( ( ShaderProgramParameter )parameter ).PrependVertexCode + vertexCode;
            }

            if ( !string.ReferenceEquals( ( ( ShaderProgramParameter )parameter ).PrependFragmentCode, null ) )
            {
                fragmentCode = ( ( ShaderProgramParameter )parameter ).PrependFragmentCode + fragmentCode;
            }
        }

        var shaderProgram = new ShaderProgram( vertexCode, fragmentCode );

        if ( ( ( parameter == null ) || ( ( ShaderProgramParameter )parameter ).LogOnCompileFailure )
             && !shaderProgram.IsCompiled )
        {
            manager?.Log.Error( "ShaderProgram " + fileName + " failed to compile:\n" + shaderProgram.Log );
        }

        return shaderProgram;
    }

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