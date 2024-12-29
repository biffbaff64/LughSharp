// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Numerics;

using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;

namespace Corelib.Lugh.Graphics.Profiling;

/// <summary>
/// A collection of OpenGL Profiling methods.
/// </summary>
[PublicAPI]
public class GLInterceptor( GLProfiler profiler ) : BaseGLInterceptor( profiler ), IGLBindings
{
    /// <inheritdoc />
    public void MinSampleShading( float value )
    {
        Calls++;
        Bindings.MinSampleShading( value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationi( uint buf, int mode )
    {
        Calls++;
        Bindings.BlendEquationi( buf, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparatei( uint buf, int modeRGB, int modeAlpha )
    {
        Calls++;
        Bindings.BlendEquationSeparatei( buf, modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunci( uint buf, int src, int dst )
    {
        Calls++;
        Bindings.BlendFunci( buf, src, dst );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparatei( uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
    {
        Calls++;
        Bindings.BlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawArraysIndirect( int mode, void* indirect )
    {
        Calls++;
        Bindings.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect )
    {
        Calls++;
        Bindings.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsIndirect( int mode, int type, void* indirect )
    {
        Calls++;
        Bindings.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect )
    {
        Calls++;
        Bindings.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1d( int location, double x )
    {
        Calls++;
        Bindings.Uniform1d( location, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2d( int location, double x, double y )
    {
        Calls++;
        Bindings.Uniform2d( location, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3d( int location, double x, double y, double z )
    {
        Calls++;
        Bindings.Uniform3d( location, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4d( int location, double x, double y, double z, double w )
    {
        Calls++;
        Bindings.Uniform4d( location, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1dv( int location, int count, double* value )
    {
        Calls++;
        Bindings.Uniform1dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1dv( int location, double[] value )
    {
        Calls++;
        Bindings.Uniform1dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2dv( int location, int count, double* value )
    {
        Calls++;
        Bindings.Uniform2dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2dv( int location, double[] value )
    {
        Calls++;
        Bindings.Uniform2dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3dv( int location, int count, double* value )
    {
        Calls++;
        Bindings.Uniform3dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3dv( int location, double[] value )
    {
        Calls++;
        Bindings.Uniform3dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4dv( int location, int count, double* value )
    {
        Calls++;
        Bindings.Uniform4dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4dv( int location, double[] value )
    {
        Calls++;
        Bindings.Uniform4dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix2x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix2x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix2x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix2x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix3x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix3x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix3x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix3x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix4x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix4x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.UniformMatrix4x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.UniformMatrix4x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformdv( uint program, int location, double* parameters )
    {
        Calls++;
        Bindings.GetUniformdv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformdv( uint program, int location, ref double[] parameters )
    {
        Calls++;
        Bindings.GetUniformdv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetSubroutineUniformLocation( uint program, int shadertype, byte* name )
    {
        Calls++;
        var result = Bindings.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetSubroutineUniformLocation( uint program, int shadertype, string name )
    {
        Calls++;
        var result = Bindings.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetSubroutineIndex( uint program, int shadertype, byte* name )
    {
        Calls++;
        var result = Bindings.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetSubroutineIndex( uint program, int shadertype, string name )
    {
        Calls++;
        var result = Bindings.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, int* values )
    {
        Calls++;
        Bindings.GetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, ref int[] values )
    {
        Calls++;
        Bindings.GetActiveSubroutineUniformiv( program, shadertype, index, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Calls++;
        Bindings.GetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize )
    {
        Calls++;
        var result = Bindings.GetActiveSubroutineUniformName( program, shadertype, index, bufsize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Calls++;
        Bindings.GetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize )
    {
        Calls++;
        var result = Bindings.GetActiveSubroutineName( program, shadertype, index, bufsize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void UniformSubroutinesuiv( int shadertype, int count, uint* indices )
    {
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformSubroutinesuiv( int shadertype, uint[] indices )
    {
        Calls++;
        Bindings.UniformSubroutinesuiv( shadertype, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformSubroutineuiv( int shadertype, int location, uint* parameters )
    {
        Calls++;
        Bindings.GetUniformSubroutineuiv( shadertype, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformSubroutineuiv( int shadertype, int location, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetUniformSubroutineuiv( shadertype, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramStageiv( uint program, int shadertype, int pname, int* values )
    {
        Calls++;
        Bindings.GetProgramStageiv( program, shadertype, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramStageiv( uint program, int shadertype, int pname, ref int[] values )
    {
        Calls++;
        Bindings.GetProgramStageiv( program, shadertype, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameteri( int pname, int value )
    {
        Calls++;
        Bindings.PatchParameteri( pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PatchParameterfv( int pname, float* values )
    {
        Calls++;
        Bindings.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameterfv( int pname, float[] values )
    {
        Calls++;
        Bindings.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTransformFeedback( int target, uint id )
    {
        Calls++;
        Bindings.BindTransformFeedback( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Bindings.DeleteTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTransformFeedbacks( params uint[] ids )
    {
        Calls++;
        Bindings.DeleteTransformFeedbacks( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Bindings.GenTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTransformFeedbacks( int n )
    {
        Calls++;
        var result = Bindings.GenTransformFeedbacks( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTransformFeedback()
    {
        Calls++;
        var result = Bindings.GenTransformFeedback();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTransformFeedback( uint id )
    {
        Calls++;
        var result = Bindings.IsTransformFeedback( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void PauseTransformFeedback()
    {
        Calls++;
        Bindings.PauseTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void ResumeTransformFeedback()
    {
        Calls++;
        Bindings.ResumeTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedback( int mode, uint id )
    {
        Calls++;
        Bindings.DrawTransformFeedback( mode, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStream( int mode, uint id, uint stream )
    {
        Calls++;
        Bindings.DrawTransformFeedbackStream( mode, id, stream );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginQueryIndexed( int target, uint index, uint id )
    {
        Calls++;
        Bindings.BeginQueryIndexed( target, index, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQueryIndexed( int target, uint index )
    {
        Calls++;
        Bindings.EndQueryIndexed( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryIndexediv( int target, uint index, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetQueryIndexediv( target, index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryIndexediv( int target, uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetQueryIndexediv( target, index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReleaseShaderCompiler()
    {
        Calls++;
        Bindings.ReleaseShaderCompiler();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderBinary( int count, uint* shaders, int binaryformat, void* binary, int length )
    {
        Calls++;
        Bindings.ShaderBinary( count, shaders, binaryformat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderBinary( uint[] shaders, int binaryformat, byte[] binary )
    {
        Calls++;
        Bindings.ShaderBinary( shaders, binaryformat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderPrecisionFormat( int shaderType, int precisionType, int* range, int* precision )
    {
        Calls++;
        Bindings.GetShaderPrecisionFormat( shaderType, precisionType, range, precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderPrecisionFormat( int shaderType, int precisionType, ref int[] range, ref int precision )
    {
        Calls++;
        Bindings.GetShaderPrecisionFormat( shaderType, precisionType, ref range, ref precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangef( float n, float f )
    {
        Calls++;
        Bindings.DepthRangef( n, f );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepthf( float d )
    {
        Calls++;
        Bindings.ClearDepthf( d );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramBinary( uint program, int bufSize, int* length, int* binaryFormat, void* binary )
    {
        Calls++;
        Bindings.GetProgramBinary( program, bufSize, length, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetProgramBinary( uint program, int bufSize, out int binaryFormat )
    {
        Calls++;
        var result = Bindings.GetProgramBinary( program, bufSize, out binaryFormat );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void ProgramBinary( uint program, int binaryFormat, void* binary, int length )
    {
        Calls++;
        Bindings.ProgramBinary( program, binaryFormat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramBinary( uint program, int binaryFormat, byte[] binary )
    {
        Calls++;
        Bindings.ProgramBinary( program, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramParameteri( uint program, int pname, int value )
    {
        Calls++;
        Bindings.ProgramParameteri( program, pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgramStages( uint pipeline, uint stages, uint program )
    {
        Calls++;
        Bindings.UseProgramStages( pipeline, stages, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveShaderProgram( uint pipeline, uint program )
    {
        Calls++;
        Bindings.ActiveShaderProgram( pipeline, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint CreateShaderProgramv( int type, int count, byte** strings )
    {
        Calls++;
        var result = Bindings.CreateShaderProgramv( type, count, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShaderProgramv( int type, string[] strings )
    {
        Calls++;
        var result = Bindings.CreateShaderProgramv( type, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindProgramPipeline( uint pipeline )
    {
        Calls++;
        Bindings.BindProgramPipeline( pipeline );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Bindings.DeleteProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteProgramPipelines( params uint[] pipelines )
    {
        Calls++;
        Bindings.DeleteProgramPipelines( pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Bindings.GenProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenProgramPipelines( int n )
    {
        Calls++;
        var result = Bindings.GenProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenProgramPipeline()
    {
        Calls++;
        var result = Bindings.GenProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsProgramPipeline( uint pipeline )
    {
        Calls++;
        var result = Bindings.IsProgramPipeline( pipeline );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineiv( uint pipeline, int pname, int* param )
    {
        Calls++;
        Bindings.GetProgramPipelineiv( pipeline, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramPipelineiv( uint pipeline, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetProgramPipelineiv( pipeline, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1i( uint program, int location, int v0 )
    {
        Calls++;
        Bindings.ProgramUniform1i( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Bindings.ProgramUniform1iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1iv( uint program, int location, int[] value )
    {
        Calls++;
        Bindings.ProgramUniform1iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1f( uint program, int location, float v0 )
    {
        Calls++;
        Bindings.ProgramUniform1f( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Bindings.ProgramUniform1fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1fv( uint program, int location, float[] value )
    {
        Calls++;
        Bindings.ProgramUniform1fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1d( uint program, int location, double v0 )
    {
        Calls++;
        Bindings.ProgramUniform1d( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Bindings.ProgramUniform1dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1dv( uint program, int location, double[] value )
    {
        Calls++;
        Bindings.ProgramUniform1dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1ui( uint program, int location, uint v0 )
    {
        Calls++;
        Bindings.ProgramUniform1ui( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Bindings.ProgramUniform1uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Bindings.ProgramUniform1uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2i( uint program, int location, int v0, int v1 )
    {
        Calls++;
        Bindings.ProgramUniform2i( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Bindings.ProgramUniform2iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2iv( uint program, int location, int[] value )
    {
        Calls++;
        Bindings.ProgramUniform2iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2f( uint program, int location, float v0, float v1 )
    {
        Calls++;
        Bindings.ProgramUniform2f( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Bindings.ProgramUniform2fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2fv( uint program, int location, float[] value )
    {
        Calls++;
        Bindings.ProgramUniform2fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2d( uint program, int location, double v0, double v1 )
    {
        Calls++;
        Bindings.ProgramUniform2d( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Bindings.ProgramUniform2dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2dv( uint program, int location, double[] value )
    {
        Calls++;
        Bindings.ProgramUniform2dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2ui( uint program, int location, uint v0, uint v1 )
    {
        Calls++;
        Bindings.ProgramUniform2ui( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Bindings.ProgramUniform2uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Bindings.ProgramUniform2uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3i( uint program, int location, int v0, int v1, int v2 )
    {
        Calls++;
        Bindings.ProgramUniform3i( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Bindings.ProgramUniform3iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3iv( uint program, int location, int[] value )
    {
        Calls++;
        Bindings.ProgramUniform3iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3f( uint program, int location, float v0, float v1, float v2 )
    {
        Calls++;
        Bindings.ProgramUniform3f( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Bindings.ProgramUniform3fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3fv( uint program, int location, float[] value )
    {
        Calls++;
        Bindings.ProgramUniform3fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3d( uint program, int location, double v0, double v1, double v2 )
    {
        Calls++;
        Bindings.ProgramUniform3d( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Bindings.ProgramUniform3dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3dv( uint program, int location, double[] value )
    {
        Calls++;
        Bindings.ProgramUniform3dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3ui( uint program, int location, uint v0, uint v1, uint v2 )
    {
        Calls++;
        Bindings.ProgramUniform3ui( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Bindings.ProgramUniform3uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Bindings.ProgramUniform3uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4i( uint program, int location, int v0, int v1, int v2, int v3 )
    {
        Calls++;
        Bindings.ProgramUniform4i( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Bindings.ProgramUniform4iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4iv( uint program, int location, int[] value )
    {
        Calls++;
        Bindings.ProgramUniform4iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4f( uint program, int location, float v0, float v1, float v2, float v3 )
    {
        Calls++;
        Bindings.ProgramUniform4f( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Bindings.ProgramUniform4fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4fv( uint program, int location, float[] value )
    {
        Calls++;
        Bindings.ProgramUniform4fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4d( uint program, int location, double v0, double v1, double v2, double v3 )
    {
        Calls++;
        Bindings.ProgramUniform4d( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Bindings.ProgramUniform4dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4dv( uint program, int location, double[] value )
    {
        Calls++;
        Bindings.ProgramUniform4dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4ui( uint program, int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Calls++;
        Bindings.ProgramUniform4ui( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Bindings.ProgramUniform4uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Bindings.ProgramUniform4uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix2x4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix3x4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Bindings.ProgramUniformMatrix4x3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ValidateProgramPipeline( uint pipeline )
    {
        Calls++;
        Bindings.ValidateProgramPipeline( pipeline );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineInfoLog( uint pipeline, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Bindings.GetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramPipelineInfoLog( uint pipeline, int bufSize )
    {
        Calls++;
        var result = Bindings.GetProgramPipelineInfoLog( pipeline, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void VertexAttribL1d( uint index, double x )
    {
        Calls++;
        Bindings.VertexAttribL1d( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2d( uint index, double x, double y )
    {
        Calls++;
        Bindings.VertexAttribL2d( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3d( uint index, double x, double y, double z )
    {
        Calls++;
        Bindings.VertexAttribL3d( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4d( uint index, double x, double y, double z, double w )
    {
        Calls++;
        Bindings.VertexAttribL4d( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL1dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttribL1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL1dv( uint index, double[] v )
    {
        Calls++;
        Bindings.VertexAttribL1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL2dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttribL2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2dv( uint index, double[] v )
    {
        Calls++;
        Bindings.VertexAttribL2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL3dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttribL3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3dv( uint index, double[] v )
    {
        Calls++;
        Bindings.VertexAttribL3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL4dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttribL4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4dv( uint index, double[] v )
    {
        Calls++;
        Bindings.VertexAttribL4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribLPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Calls++;
        Bindings.VertexAttribLPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLPointer( uint index, int size, int type, int stride, int pointer )
    {
        Calls++;
        Bindings.VertexAttribLPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribLdv( uint index, int pname, double* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribLdv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribLdv( uint index, int pname, ref double[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribLdv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportArrayv( uint first, int count, float* v )
    {
        Calls++;
        Bindings.ViewportArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportArrayv( uint first, int count, params float[] v )
    {
        Calls++;
        Bindings.ViewportArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedf( uint index, float x, float y, float w, float h )
    {
        Calls++;
        Bindings.ViewportIndexedf( index, x, y, w, h );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportIndexedfv( uint index, float* v )
    {
        Calls++;
        Bindings.ViewportIndexedfv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedfv( uint index, params float[] v )
    {
        Calls++;
        Bindings.ViewportIndexedfv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorArrayv( uint first, int count, int* v )
    {
        Calls++;
        Bindings.ScissorArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorArrayv( uint first, int count, params int[] v )
    {
        Calls++;
        Bindings.ScissorArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexed( uint index, int left, int bottom, int width, int height )
    {
        Calls++;
        Bindings.ScissorIndexed( index, left, bottom, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorIndexedv( uint index, int* v )
    {
        Calls++;
        Bindings.ScissorIndexedv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexedv( uint index, params int[] v )
    {
        Calls++;
        Bindings.ScissorIndexedv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DepthRangeArrayv( uint first, int count, double* v )
    {
        Calls++;
        Bindings.DepthRangeArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeArrayv( uint first, int count, params double[] v )
    {
        Calls++;
        Bindings.DepthRangeArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeIndexed( uint index, double n, double f )
    {
        Calls++;
        Bindings.DepthRangeIndexed( index, n, f );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFloati_v( int target, uint index, float* data )
    {
        Calls++;
        Bindings.GetFloati_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloati_v( int target, uint index, ref float[] data )
    {
        Calls++;
        Bindings.GetFloati_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublei_v( int target, uint index, double* data )
    {
        Calls++;
        Bindings.GetDoublei_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublei_v( int target, uint index, ref double[] data )
    {
        Calls++;
        Bindings.GetDoublei_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysInstancedBaseInstance( int mode, int first, int count, int instancecount, uint baseinstance )
    {
        Calls++;
        Bindings.DrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseInstance( int mode, int count, int type, void* indices, int instancecount, uint baseinstance )
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, uint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertexBaseInstance( int mode, int count, int type, void* indices, int instancecount, int basevertex, uint baseinstance )
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertexBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex, uint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformativ( int target, int internalformat, int pname, int bufSize, int* parameters )
    {
        Calls++;
        Bindings.GetInternalformativ( target, internalformat, pname, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformativ( int target, int internalformat, int pname, int bufSize, ref int[] parameters )
    {
        Calls++;
        Bindings.GetInternalformativ( target, internalformat, pname, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetActiveAtomicCounterBufferiv( program, bufferIndex, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTexture( uint unit, uint texture, int level, bool layered, int layer, int access, int format )
    {
        Calls++;
        Bindings.BindImageTexture( unit, texture, level, layered, layer, access, format );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrier( uint barriers )
    {
        Calls++;
        Bindings.MemoryBarrier( barriers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage1D( int target, int levels, int internalformat, int width )
    {
        Calls++;
        Bindings.TexStorage1D( target, levels, internalformat, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2D( int target, int levels, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.TexStorage2D( target, levels, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3D( int target, int levels, int internalformat, int width, int height, int depth )
    {
        Calls++;
        Bindings.TexStorage3D( target, levels, internalformat, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackInstanced( int mode, uint id, int instancecount )
    {
        Calls++;
        Bindings.DrawTransformFeedbackInstanced( mode, id, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStreamInstanced( int mode, uint id, uint stream, int instancecount )
    {
        Calls++;
        Bindings.DrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetPointerv( int pname, void** parameters )
    {
        Calls++;
        Bindings.GetPointerv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetPointerv( int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Bindings.GetPointerv( pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferData( int target, int internalformat, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearBufferData( target, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferData< T >( int target, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearBufferData( target, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferSubData( int target, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearBufferSubData( target, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferSubData< T >( int target, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearBufferSubData( target, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchCompute( uint numGroupsX, uint numGroupsY, uint numGroupsZ )
    {
        Calls++;
        Bindings.DispatchCompute( numGroupsX, numGroupsY, numGroupsZ );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DispatchComputeIndirect( void* indirect )
    {
        Calls++;
        Bindings.DispatchComputeIndirect( indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchComputeIndirect( GLBindings.DispatchIndirectCommand indirect )
    {
        Calls++;
        Bindings.DispatchComputeIndirect( indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyImageSubData( uint srcName, int srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, int dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth )
    {
        Calls++;
        Bindings.CopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferParameteri( int target, int pname, int param )
    {
        Calls++;
        Bindings.FramebufferParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetFramebufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetFramebufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformati64v( int target, int internalformat, int pname, int count, long* parameters )
    {
        Calls++;
        Bindings.GetInternalformati64v( target, internalformat, pname, count, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformati64v( int target, int internalformat, int pname, int count, ref long[] parameters )
    {
        Calls++;
        Bindings.GetInternalformati64v( target, internalformat, pname, count, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth )
    {
        Calls++;
        Bindings.InvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexImage( uint texture, int level )
    {
        Calls++;
        Bindings.InvalidateTexImage( texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferSubData( uint buffer, int offset, int length )
    {
        Calls++;
        Bindings.InvalidateBufferSubData( buffer, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferData( uint buffer )
    {
        Calls++;
        Bindings.InvalidateBufferData( buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateFramebuffer( int target, int numAttachments, int* attachments )
    {
        Calls++;
        Bindings.InvalidateFramebuffer( target, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateFramebuffer( int target, int numAttachments, int[] attachments )
    {
        Calls++;
        Bindings.InvalidateFramebuffer( target, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateSubFramebuffer( int target, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.InvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateSubFramebuffer( int target, int numAttachments, int[] attachments, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.InvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirect( int mode, void* indirect, int drawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawArraysIndirect( mode, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawArraysIndirect( mode, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirect( int mode, int type, void* indirect, int drawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInterfaceiv( uint program, int programInterface, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetProgramInterfaceiv( program, programInterface, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramInterfaceiv( uint program, int programInterface, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetProgramInterfaceiv( program, programInterface, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetProgramResourceIndex( uint program, int programInterface, byte* name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetProgramResourceIndex( uint program, int programInterface, string name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceName( uint program, int programInterface, uint index, int bufSize, int* length, byte* name )
    {
        Calls++;
        Bindings.GetProgramResourceName( program, programInterface, index, bufSize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramResourceName( uint program, int programInterface, uint index, int bufSize )
    {
        Calls++;
        var result = Bindings.GetProgramResourceName( program, programInterface, index, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceiv( uint program, int programInterface, uint index, int propCount, int* props, int bufSize, int* length, int* parameters )
    {
        Calls++;
        Bindings.GetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramResourceiv( uint program, int programInterface, uint index, int[] props, int bufSize, ref int[] parameters )
    {
        Calls++;
        Bindings.GetProgramResourceiv( program, programInterface, index, props, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocation( uint program, int programInterface, byte* name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceLocation( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetProgramResourceLocation( uint program, int programInterface, string name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceLocation( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocationIndex( uint program, int programInterface, byte* name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceLocationIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetProgramResourceLocationIndex( uint program, int programInterface, string name )
    {
        Calls++;
        var result = Bindings.GetProgramResourceLocationIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void ShaderStorageBlockBinding( uint program, uint storageBlockIndex, uint storageBlockBinding )
    {
        Calls++;
        Bindings.ShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexBufferRange( int target, int internalformat, uint buffer, int offset, int size )
    {
        Calls++;
        Bindings.TexBufferRange( target, internalformat, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureView( uint texture, int target, uint origtexture, int internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers )
    {
        Calls++;
        Bindings.TextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffer( uint bindingindex, uint buffer, int offset, int stride )
    {
        Calls++;
        Bindings.BindVertexBuffer( bindingindex, buffer, offset, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribFormat( uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexAttribIFormat( attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexAttribLFormat( attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribBinding( uint attribindex, uint bindingindex )
    {
        Calls++;
        Bindings.VertexAttribBinding( attribindex, bindingindex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexBindingDivisor( uint bindingindex, uint divisor )
    {
        Calls++;
        Bindings.VertexBindingDivisor( bindingindex, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageControl( int source, int type, int severity, int count, uint* ids, bool enabled )
    {
        Calls++;
        Bindings.DebugMessageControl( source, type, severity, count, ids, enabled );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageControl( int source, int type, int severity, uint[] ids, bool enabled )
    {
        Calls++;
        Bindings.DebugMessageControl( source, type, severity, ids, enabled );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageInsert( int source, int type, uint id, int severity, int length, byte* buf )
    {
        Calls++;
        Bindings.DebugMessageInsert( source, type, id, severity, length, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageInsert( int source, int type, uint id, int severity, string buf )
    {
        Calls++;
        Bindings.DebugMessageInsert( source, type, id, severity, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROC callback, void* userParam )
    {
        Calls++;
        Bindings.DebugMessageCallback( callback, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROCSAFE callback, void* userParam )
    {
        Calls++;
        Bindings.DebugMessageCallback( callback, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetDebugMessageLog( uint count, int bufsize, int* sources, int* types, uint* ids, int* severities, int* lengths, byte* messageLog )
    {
        Calls++;
        var result = Bindings.GetDebugMessageLog( count, bufsize, sources, types, ids, severities, lengths, messageLog );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetDebugMessageLog( uint count, int bufSize, out int[] sources, out int[] types, out uint[] ids, out int[] severities, out string[] messageLog )
    {
        Calls++;
        var result = Bindings.GetDebugMessageLog( count, bufSize, out sources, out types, out ids, out severities, out messageLog );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void PushDebugGroup( int source, uint id, int length, byte* message )
    {
        Calls++;
        Bindings.PushDebugGroup( source, id, length, message );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PushDebugGroup( int source, uint id, string message )
    {
        Calls++;
        Bindings.PushDebugGroup( source, id, message );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PopDebugGroup()
    {
        Calls++;
        Bindings.PopDebugGroup();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ObjectLabel( int identifier, uint name, int length, byte* label )
    {
        Calls++;
        Bindings.ObjectLabel( identifier, name, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectLabel( int identifier, uint name, string label )
    {
        Calls++;
        Bindings.ObjectLabel( identifier, name, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectLabel( int identifier, uint name, int bufSize, int* length, byte* label )
    {
        Calls++;
        Bindings.GetObjectLabel( identifier, name, bufSize, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectLabel( int identifier, uint name, int bufSize )
    {
        Calls++;
        var result = Bindings.GetObjectLabel( identifier, name, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void ObjectPtrLabel( void* ptr, int length, byte* label )
    {
        Calls++;
        Bindings.ObjectPtrLabel( ptr, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectPtrLabel( IntPtr ptr, string label )
    {
        Calls++;
        Bindings.ObjectPtrLabel( ptr, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectPtrLabel( void* ptr, int bufSize, int* length, byte* label )
    {
        Calls++;
        Bindings.GetObjectPtrLabel( ptr, bufSize, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectPtrLabel( IntPtr ptr, int bufSize )
    {
        Calls++;
        var result = Bindings.GetObjectPtrLabel( ptr, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void BufferStorage( int target, int size, void* data, uint flags )
    {
        Calls++;
        Bindings.BufferStorage( target, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferStorage< T >( int target, T[] data, uint flags ) where T : unmanaged
    {
        Calls++;
        Bindings.BufferStorage( target, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexImage( uint texture, int level, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearTexImage( texture, level, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexImage< T >( uint texture, int level, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearTexImage( texture, level, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexSubImage( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexSubImage< T >( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersBase( int target, uint first, int count, uint* buffers )
    {
        Calls++;
        Bindings.BindBuffersBase( target, first, count, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersBase( int target, uint first, uint[] buffers )
    {
        Calls++;
        Bindings.BindBuffersBase( target, first, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersRange( int target, uint first, int count, uint* buffers, int* offsets, int* sizes )
    {
        Calls++;
        Bindings.BindBuffersRange( target, first, count, buffers, offsets, sizes );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersRange( int target, uint first, uint[] buffers, int[] offsets, int[] sizes )
    {
        Calls++;
        Bindings.BindBuffersRange( target, first, buffers, offsets, sizes );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindTextures( uint first, int count, uint* textures )
    {
        Calls++;
        Bindings.BindTextures( first, count, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextures( uint first, uint[] textures )
    {
        Calls++;
        Bindings.BindTextures( first, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindSamplers( uint first, int count, uint* samplers )
    {
        Calls++;
        Bindings.BindSamplers( first, count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindSamplers( uint first, uint[] samplers )
    {
        Calls++;
        Bindings.BindSamplers( first, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindImageTextures( uint first, int count, uint* textures )
    {
        Calls++;
        Bindings.BindImageTextures( first, count, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTextures( uint first, uint[] textures )
    {
        Calls++;
        Bindings.BindImageTextures( first, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindVertexBuffers( uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Calls++;
        Bindings.BindVertexBuffers( first, count, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffers( uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Calls++;
        Bindings.BindVertexBuffers( first, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClipControl( int origin, int depth )
    {
        Calls++;
        Bindings.ClipControl( origin, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Bindings.CreateTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTransformFeedbacks( int n )
    {
        Calls++;
        var result = Bindings.CreateTransformFeedbacks( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateTransformFeedbacks()
    {
        Calls++;
        var result = Bindings.CreateTransformFeedbacks();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferBase( uint xfb, uint index, uint buffer )
    {
        Calls++;
        Bindings.TransformFeedbackBufferBase( xfb, index, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferRange( uint xfb, uint index, uint buffer, int offset, int size )
    {
        Calls++;
        Bindings.TransformFeedbackBufferRange( xfb, index, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackiv( uint xfb, int pname, int* param )
    {
        Calls++;
        Bindings.GetTransformFeedbackiv( xfb, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbackiv( uint xfb, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetTransformFeedbackiv( xfb, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki_v( uint xfb, int pname, uint index, int* param )
    {
        Calls++;
        Bindings.GetTransformFeedbacki_v( xfb, pname, index, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki_v( uint xfb, int pname, uint index, ref int[] param )
    {
        Calls++;
        Bindings.GetTransformFeedbacki_v( xfb, pname, index, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, long* param )
    {
        Calls++;
        Bindings.GetTransformFeedbacki64_v( xfb, pname, index, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, ref long[] param )
    {
        Calls++;
        Bindings.GetTransformFeedbacki64_v( xfb, pname, index, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateBuffers( int n, uint* buffers )
    {
        Calls++;
        Bindings.CreateBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateBuffers( int n )
    {
        Calls++;
        var result = Bindings.CreateBuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateBuffer()
    {
        Calls++;
        var result = Bindings.CreateBuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void NamedBufferStorage( uint buffer, int size, void* data, uint flags )
    {
        Calls++;
        Bindings.NamedBufferStorage( buffer, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferStorage< T >( uint buffer, int size, T[] data, uint flags ) where T : unmanaged
    {
        Calls++;
        Bindings.NamedBufferStorage( buffer, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferData( uint buffer, int size, void* data, int usage )
    {
        Calls++;
        Bindings.NamedBufferData( buffer, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferData< T >( uint buffer, int size, T[] data, int usage ) where T : unmanaged
    {
        Calls++;
        Bindings.NamedBufferData( buffer, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Calls++;
        Bindings.NamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferSubData< T >( uint buffer, int offset, int size, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.NamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyNamedBufferSubData( uint readBuffer, uint writeBuffer, int readOffset, int writeOffset, int size )
    {
        Calls++;
        Bindings.CopyNamedBufferSubData( readBuffer, writeBuffer, readOffset, writeOffset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferData( uint buffer, int internalformat, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearNamedBufferData( buffer, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferData< T >( uint buffer, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearNamedBufferData( buffer, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferSubData( uint buffer, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Calls++;
        Bindings.ClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferSubData< T >( uint buffer, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.ClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBuffer( uint buffer, int access )
    {
        Calls++;
        var result = Bindings.MapNamedBuffer( buffer, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapNamedBuffer< T >( uint buffer, int access ) where T : unmanaged
    {
        Calls++;
        var result = Bindings.MapNamedBuffer< T >( buffer, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBufferRange( uint buffer, int offset, int length, uint access )
    {
        Calls++;
        var result = Bindings.MapNamedBufferRange( buffer, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapNamedBufferRange< T >( uint buffer, int offset, int length, uint access ) where T : unmanaged
    {
        Calls++;
        var result = Bindings.MapNamedBufferRange< T >( buffer, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool UnmapNamedBuffer( uint buffer )
    {
        Calls++;
        var result = Bindings.UnmapNamedBuffer( buffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FlushMappedNamedBufferRange( uint buffer, int offset, int length )
    {
        Calls++;
        Bindings.FlushMappedNamedBufferRange( buffer, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteriv( uint buffer, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetNamedBufferParameteriv( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteriv( uint buffer, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetNamedBufferParameteriv( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteri64v( uint buffer, int pname, long* parameters )
    {
        Calls++;
        Bindings.GetNamedBufferParameteri64v( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteri64v( uint buffer, int pname, ref long[] parameters )
    {
        Calls++;
        Bindings.GetNamedBufferParameteri64v( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferPointerv( uint buffer, int pname, void** parameters )
    {
        Calls++;
        Bindings.GetNamedBufferPointerv( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferPointerv( uint buffer, int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Bindings.GetNamedBufferPointerv( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Calls++;
        Bindings.GetNamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetNamedBufferSubData< T >( uint buffer, int offset, int size ) where T : unmanaged
    {
        Calls++;
        var result = Bindings.GetNamedBufferSubData< T >( buffer, offset, size );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Bindings.CreateFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateFramebuffers( int n )
    {
        Calls++;
        var result = Bindings.CreateFramebuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateFramebuffer()
    {
        Calls++;
        var result = Bindings.CreateFramebuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void NamedFramebufferRenderbuffer( uint framebuffer, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Calls++;
        Bindings.NamedFramebufferRenderbuffer( framebuffer, attachment, renderbuffertarget, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferParameteri( uint framebuffer, int pname, int param )
    {
        Calls++;
        Bindings.NamedFramebufferParameteri( framebuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTexture( uint framebuffer, int attachment, uint texture, int level )
    {
        Calls++;
        Bindings.NamedFramebufferTexture( framebuffer, attachment, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTextureLayer( uint framebuffer, int attachment, uint texture, int level, int layer )
    {
        Calls++;
        Bindings.NamedFramebufferTextureLayer( framebuffer, attachment, texture, level, layer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffer( uint framebuffer, int buf )
    {
        Calls++;
        Bindings.NamedFramebufferDrawBuffer( framebuffer, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedFramebufferDrawBuffers( uint framebuffer, int n, int* bufs )
    {
        Calls++;
        Bindings.NamedFramebufferDrawBuffers( framebuffer, n, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffers( uint framebuffer, int[] bufs )
    {
        Calls++;
        Bindings.NamedFramebufferDrawBuffers( framebuffer, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferReadBuffer( uint framebuffer, int src )
    {
        Calls++;
        Bindings.NamedFramebufferReadBuffer( framebuffer, src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferData( uint framebuffer, int numAttachments, int* attachments )
    {
        Calls++;
        Bindings.InvalidateNamedFramebufferData( framebuffer, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferData( uint framebuffer, int[] attachments )
    {
        Calls++;
        Bindings.InvalidateNamedFramebufferData( framebuffer, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferSubData( uint framebuffer, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.InvalidateNamedFramebufferSubData( framebuffer, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferSubData( uint framebuffer, int[] attachments, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.InvalidateNamedFramebufferSubData( framebuffer, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int* value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int[] value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint* value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint[] value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float* value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float[] value )
    {
        Calls++;
        Bindings.ClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfi( uint framebuffer, int buffer, float depth, int stencil )
    {
        Calls++;
        Bindings.ClearNamedFramebufferfi( framebuffer, buffer, depth, stencil );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlitNamedFramebuffer( uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Calls++;
        Bindings.BlitNamedFramebuffer( readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        CheckErrors();
    }

    /// <inheritdoc />
    public int CheckNamedFramebufferStatus( uint framebuffer, int target )
    {
        Calls++;
        var result = Bindings.CheckNamedFramebufferStatus( framebuffer, target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferParameteriv( uint framebuffer, int pname, int* param )
    {
        Calls++;
        Bindings.GetNamedFramebufferParameteriv( framebuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferParameteriv( uint framebuffer, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetNamedFramebufferParameteriv( framebuffer, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Bindings.CreateRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateRenderbuffers( int n )
    {
        Calls++;
        var result = Bindings.CreateRenderbuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateRenderbuffer()
    {
        Calls++;
        var result = Bindings.CreateRenderbuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorage( uint renderbuffer, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.NamedRenderbufferStorage( renderbuffer, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorageMultisample( uint renderbuffer, int samples, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.NamedRenderbufferStorageMultisample( renderbuffer, samples, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, int* param )
    {
        Calls++;
        Bindings.GetNamedRenderbufferParameteriv( renderbuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetNamedRenderbufferParameteriv( renderbuffer, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTextures( int target, int n, uint* textures )
    {
        Calls++;
        Bindings.CreateTextures( target, n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTextures( int target, int n )
    {
        Calls++;
        var result = Bindings.CreateTextures( target, n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateTexture( int target )
    {
        Calls++;
        var result = Bindings.CreateTexture( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TextureBuffer( uint texture, int internalformat, uint buffer )
    {
        Calls++;
        Bindings.TextureBuffer( texture, internalformat, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureBufferRange( uint texture, int internalformat, uint buffer, int offset, int size )
    {
        Calls++;
        Bindings.TextureBufferRange( texture, internalformat, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage1D( uint texture, int levels, int internalformat, int width )
    {
        Calls++;
        Bindings.TextureStorage1D( texture, levels, internalformat, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2D( uint texture, int levels, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.TextureStorage2D( texture, levels, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3D( uint texture, int levels, int internalformat, int width, int height, int depth )
    {
        Calls++;
        Bindings.TextureStorage3D( texture, levels, internalformat, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2DMultisample( uint texture, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TextureStorage2DMultisample( texture, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3DMultisample( uint texture, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TextureStorage3DMultisample( texture, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage1D< T >( uint texture, int level, int xoffset, int width, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Bindings.TextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage2D< T >( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Bindings.TextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage3D< T >( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Bindings.TextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, byte[] data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, byte[] data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, byte[] data )
    {
        Calls++;
        Bindings.CompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage1D( uint texture, int level, int xoffset, int x, int y, int width )
    {
        Calls++;
        Bindings.CopyTextureSubImage1D( texture, level, xoffset, x, y, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.CopyTextureSubImage2D( texture, level, xoffset, yoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.CopyTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterf( uint texture, int pname, float param )
    {
        Calls++;
        Bindings.TextureParameterf( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterfv( uint texture, int pname, float* param )
    {
        Calls++;
        Bindings.TextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterfv( uint texture, int pname, float[] param )
    {
        Calls++;
        Bindings.TextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteri( uint texture, int pname, int param )
    {
        Calls++;
        Bindings.TextureParameteri( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIiv( uint texture, int pname, int* param )
    {
        Calls++;
        Bindings.TextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIiv( uint texture, int pname, int[] param )
    {
        Calls++;
        Bindings.TextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Calls++;
        Bindings.TextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIuiv( uint texture, int pname, uint[] param )
    {
        Calls++;
        Bindings.TextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameteriv( uint texture, int pname, int* param )
    {
        Calls++;
        Bindings.TextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteriv( uint texture, int pname, int[] param )
    {
        Calls++;
        Bindings.TextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateTextureMipmap( uint texture )
    {
        Calls++;
        Bindings.GenerateTextureMipmap( texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextureUnit( uint unit, uint texture )
    {
        Calls++;
        Bindings.BindTextureUnit( unit, texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureImage( uint texture, int level, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetTextureImage( texture, level, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetTextureImage< T >( uint texture, int level, int format, int type, int bufSize ) where T : unmanaged
    {
        Calls++;
        var result = Bindings.GetTextureImage< T >( texture, level, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureImage( uint texture, int level, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetCompressedTextureImage( texture, level, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureImage( uint texture, int level, int bufSize )
    {
        Calls++;
        var result = Bindings.GetCompressedTextureImage( texture, level, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameterfv( uint texture, int level, int pname, float* param )
    {
        Calls++;
        Bindings.GetTextureLevelParameterfv( texture, level, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameterfv( uint texture, int level, int pname, ref float[] param )
    {
        Calls++;
        Bindings.GetTextureLevelParameterfv( texture, level, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameteriv( uint texture, int level, int pname, int* param )
    {
        Calls++;
        Bindings.GetTextureLevelParameteriv( texture, level, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameteriv( uint texture, int level, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetTextureLevelParameteriv( texture, level, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterfv( uint texture, int pname, float* param )
    {
        Calls++;
        Bindings.GetTextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterfv( uint texture, int pname, ref float[] param )
    {
        Calls++;
        Bindings.GetTextureParameterfv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIiv( uint texture, int pname, int* param )
    {
        Calls++;
        Bindings.GetTextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIiv( uint texture, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetTextureParameterIiv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Calls++;
        Bindings.GetTextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIuiv( uint texture, int pname, ref uint[] param )
    {
        Calls++;
        Bindings.GetTextureParameterIuiv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameteriv( uint texture, int pname, int* param )
    {
        Calls++;
        Bindings.GetTextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameteriv( uint texture, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetTextureParameteriv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Bindings.CreateVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateVertexArrays( int n )
    {
        Calls++;
        var result = Bindings.CreateVertexArrays( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateVertexArray()
    {
        Calls++;
        var result = Bindings.CreateVertexArray();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DisableVertexArrayAttrib( uint vaobj, uint index )
    {
        Calls++;
        Bindings.DisableVertexArrayAttrib( vaobj, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexArrayAttrib( uint vaobj, uint index )
    {
        Calls++;
        Bindings.EnableVertexArrayAttrib( vaobj, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayElementBuffer( uint vaobj, uint buffer )
    {
        Calls++;
        Bindings.VertexArrayElementBuffer( vaobj, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffer( uint vaobj, uint bindingindex, uint buffer, int offset, int stride )
    {
        Calls++;
        Bindings.VertexArrayVertexBuffer( vaobj, bindingindex, buffer, offset, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexArrayVertexBuffers( uint vaobj, uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Calls++;
        Bindings.VertexArrayVertexBuffers( vaobj, first, count, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffers( uint vaobj, uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Calls++;
        Bindings.VertexArrayVertexBuffers( vaobj, first, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribBinding( uint vaobj, uint attribindex, uint bindingindex )
    {
        Calls++;
        Bindings.VertexArrayAttribBinding( vaobj, attribindex, bindingindex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribFormat( uint vaobj, uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexArrayAttribFormat( vaobj, attribindex, size, type, normalized, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribIFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexArrayAttribIFormat( vaobj, attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribLFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Bindings.VertexArrayAttribLFormat( vaobj, attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayBindingDivisor( uint vaobj, uint bindingindex, uint divisor )
    {
        Calls++;
        Bindings.VertexArrayBindingDivisor( vaobj, bindingindex, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayiv( uint vaobj, int pname, int* param )
    {
        Calls++;
        Bindings.GetVertexArrayiv( vaobj, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayiv( uint vaobj, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetVertexArrayiv( vaobj, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, int* param )
    {
        Calls++;
        Bindings.GetVertexArrayIndexediv( vaobj, index, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetVertexArrayIndexediv( vaobj, index, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, long* param )
    {
        Calls++;
        Bindings.GetVertexArrayIndexed64iv( vaobj, index, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, ref long[] param )
    {
        Calls++;
        Bindings.GetVertexArrayIndexed64iv( vaobj, index, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateSamplers( int n, uint* samplers )
    {
        Calls++;
        Bindings.CreateSamplers( n, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateSamplers( int n )
    {
        Calls++;
        var result = Bindings.CreateSamplers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateSamplers()
    {
        Calls++;
        var result = Bindings.CreateSamplers();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Bindings.CreateProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateProgramPipelines( int n )
    {
        Calls++;
        var result = Bindings.CreateProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateProgramPipeline()
    {
        Calls++;
        var result = Bindings.CreateProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateQueries( int target, int n, uint* ids )
    {
        Calls++;
        Bindings.CreateQueries( target, n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateQueries( int target, int n )
    {
        Calls++;
        var result = Bindings.CreateQueries( target, n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateQuery( int target )
    {
        Calls++;
        var result = Bindings.CreateQuery( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void GetQueryBufferObjecti64v( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Bindings.GetQueryBufferObjecti64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectiv( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Bindings.GetQueryBufferObjectiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectui64v( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Bindings.GetQueryBufferObjectui64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectuiv( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Bindings.GetQueryBufferObjectuiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrierByRegion( uint barriers )
    {
        Calls++;
        Bindings.MemoryBarrierByRegion( barriers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize )
    {
        Calls++;
        var result = Bindings.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize )
    {
        Calls++;
        var result = Bindings.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetGraphicsResetStatus()
    {
        Calls++;
        var result = Bindings.GetGraphicsResetStatus();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnCompressedTexImage( int target, int lod, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetnCompressedTexImage( target, lod, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnCompressedTexImage( int target, int lod, int bufSize )
    {
        Calls++;
        var result = Bindings.GetnCompressedTexImage( target, lod, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnTexImage( int target, int level, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Bindings.GetnTexImage( target, level, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnTexImage( int target, int level, int format, int type, int bufSize )
    {
        Calls++;
        var result = Bindings.GetnTexImage( target, level, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnUniformdv( uint program, int location, int bufSize, double* parameters )
    {
        Calls++;
        Bindings.GetnUniformdv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformdv( uint program, int location, int bufSize, ref double[] parameters )
    {
        Calls++;
        Bindings.GetnUniformdv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformfv( uint program, int location, int bufSize, float* parameters )
    {
        Calls++;
        Bindings.GetnUniformfv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformfv( uint program, int location, int bufSize, ref float[] parameters )
    {
        Calls++;
        Bindings.GetnUniformfv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformiv( uint program, int location, int bufSize, int* parameters )
    {
        Calls++;
        Bindings.GetnUniformiv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformiv( uint program, int location, int bufSize, ref int[] parameters )
    {
        Calls++;
        Bindings.GetnUniformiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformuiv( uint program, int location, int bufSize, uint* parameters )
    {
        Calls++;
        Bindings.GetnUniformiv( program, location, bufSize, ( int* )parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformuiv( uint program, int location, int bufSize, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetnUniformuiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize, void* data )
    {
        Calls++;
        Bindings.ReadnPixels( x, y, width, height, format, type, bufSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize )
    {
        Calls++;
        var result = Bindings.ReadnPixels( x, y, width, height, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TextureBarrier()
    {
        Calls++;
        Bindings.TextureBarrier();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SpecializeShader( uint shader, byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue )
    {
        Calls++;
        Bindings.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SpecializeShader( uint shader, string pEntryPoint, uint numSpecializationConstants, uint[] pConstantIndex, uint[] pConstantValue )
    {
        Calls++;
        Bindings.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirectCount( int mode, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirectCount( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirectCount( int mode, int type, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirectCount( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Bindings.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffsetClamp( float factor, float units, float clamp )
    {
        Calls++;
        Bindings.PolygonOffsetClamp( factor, units, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public (int major, int minor) GetOpenGLVersion()
    {
        Calls++;
        var (major, minor) = Bindings.GetOpenGLVersion();
        CheckErrors();

        return ( major, minor );
    }

    /// <inheritdoc />
    public unsafe void MessageCallback( int source, int type, uint id, int severity, int length, string message, void* userParam )
    {
        Calls++;
        Bindings.MessageCallback( source, type, id, severity, length, message, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CullFace( int mode )
    {
        Calls++;
        Bindings.CullFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FrontFace( int mode )
    {
        Calls++;
        Bindings.FrontFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Hint( int target, int mode )
    {
        Calls++;
        Bindings.Hint( target, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LineWidth( float width )
    {
        Calls++;
        Bindings.LineWidth( width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointSize( float size )
    {
        Calls++;
        Bindings.PointSize( size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonMode( int face, int mode )
    {
        Calls++;
        Bindings.PolygonMode( face, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Scissor( int x, int y, int width, int height )
    {
        Calls++;
        Bindings.Scissor( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterf( int target, int pname, float param )
    {
        Calls++;
        Bindings.TexParameterf( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterfv( int target, int pname, float* parameters )
    {
        Calls++;
        Bindings.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterfv( int target, int pname, float[] parameters )
    {
        Calls++;
        Bindings.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteri( int target, int pname, int param )
    {
        Calls++;
        Bindings.TexParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteriv( int target, int pname, int[] parameters )
    {
        Calls++;
        Bindings.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage1D( int target, int level, int internalformat, int width, int border, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage1D< T >( int target, int level, int internalformat, int width, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2D< T >( int target, int level, int internalformat, int width, int height, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffer( int buf )
    {
        Calls++;
        Bindings.DrawBuffer( buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Clear( uint mask )
    {
        Calls++;
        Bindings.Clear( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        Bindings.ClearColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearStencil( int s )
    {
        Calls++;
        Bindings.ClearStencil( s );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepth( double depth )
    {
        Calls++;
        Bindings.ClearDepth( depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMask( uint mask )
    {
        Calls++;
        Bindings.StencilMask( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Calls++;
        Bindings.ColorMask( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthMask( bool flag )
    {
        Calls++;
        Bindings.DepthMask( flag );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disable( int cap )
    {
        Calls++;
        Bindings.Disable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enable( int cap )
    {
        Calls++;
        Bindings.Enable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Finish()
    {
        Calls++;
        Bindings.Finish();
        CheckErrors();
    }

    /// <inheritdoc />
    public void Flush()
    {
        Calls++;
        Bindings.Flush();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunc( int sfactor, int dfactor )
    {
        Calls++;
        Bindings.BlendFunc( sfactor, dfactor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LogicOp( int opcode )
    {
        Calls++;
        Bindings.LogicOp( opcode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFunc( int func, int @ref, uint mask )
    {
        Calls++;
        Bindings.StencilFunc( func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOp( int fail, int zfail, int zpass )
    {
        Calls++;
        Bindings.StencilOp( fail, zfail, zpass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthFunc( int func )
    {
        Calls++;
        Bindings.DepthFunc( func );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStoref( int pname, float param )
    {
        Calls++;
        Bindings.PixelStoref( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStorei( int pname, int param )
    {
        Calls++;
        Bindings.PixelStorei( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadBuffer( int src )
    {
        Calls++;
        Bindings.ReadBuffer( src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.ReadPixels( x, y, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadPixels< T >( int x, int y, int width, int height, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.ReadPixels( x, y, width, height, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleanv( int pname, bool* data )
    {
        Calls++;
        Bindings.GetBooleanv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleanv( int pname, ref bool[] data )
    {
        Calls++;
        Bindings.GetBooleanv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublev( int pname, double* data )
    {
        Calls++;
        Bindings.GetDoublev( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublev( int pname, ref double[] data )
    {
        Calls++;
        Bindings.GetDoublev( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public int GetError()
    {
        Calls++;
        var err = Bindings.GetError();
        CheckErrors();

        return err;
    }

    /// <inheritdoc />
    public unsafe void GetFloatv( int pname, float* data )
    {
        Calls++;
        Bindings.GetFloatv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloatv( int pname, ref float[] data )
    {
        Calls++;
        Bindings.GetFloatv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegerv( int pname, int* data )
    {
        Calls++;
        Bindings.GetIntegerv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegerv( int pname, ref int[] data )
    {
        Calls++;
        Bindings.GetIntegerv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetString( int name )
    {
        Calls++;
        var str = Bindings.GetString( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public string GetStringSafe( int name )
    {
        Calls++;
        var str = Bindings.GetStringSafe( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public unsafe void GetTexImage( int target, int level, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.GetTexImage( target, level, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexImage< T >( int target, int level, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.GetTexImage( target, level, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterfv( int target, int pname, float* parameters )
    {
        Calls++;
        Bindings.GetTexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterfv( int target, int pname, ref float[] parameters )
    {
        Calls++;
        Bindings.GetTexParameterfv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetTexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetTexParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameterfv( int target, int level, int pname, float* parameters )
    {
        Calls++;
        Bindings.GetTexLevelParameterfv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameterfv( int target, int level, int pname, ref float[] parameters )
    {
        Calls++;
        Bindings.GetTexLevelParameterfv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameteriv( int target, int level, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetTexLevelParameteriv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameteriv( int target, int level, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetTexLevelParameteriv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabled( int cap )
    {
        Calls++;
        var result = Bindings.IsEnabled( cap );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DepthRange( double near, double far )
    {
        Calls++;
        Bindings.DepthRange( near, far );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Viewport( int x, int y, int width, int height )
    {
        Calls++;
        Bindings.Viewport( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArrays( int mode, int first, int count )
    {
        Calls++;
        Bindings.DrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElements( int mode, int count, int type, void* indices )
    {
        Calls++;
        Bindings.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElements< T >( int mode, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffset( float factor, float units )
    {
        Calls++;
        Bindings.PolygonOffset( factor, units );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage1D( int target, int level, int internalformat, int x, int y, int width, int border )
    {
        Calls++;
        Bindings.CopyTexImage1D( target, level, internalformat, x, y, width, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Calls++;
        Bindings.CopyTexImage2D( target, level, internalformat, x, y, width, height, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage1D( int target, int level, int xoffset, int x, int y, int width )
    {
        Calls++;
        Bindings.CopyTexSubImage1D( target, level, xoffset, x, y, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage1D( int target, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage1D< T >( int target, int level, int xoffset, int width, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Bindings.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage2D< T >( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Bindings.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTexture( int target, uint texture )
    {
        Calls++;
        Bindings.BindTexture( target, texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTextures( int n, uint* textures )
    {
        Calls++;
        Bindings.DeleteTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTextures( params uint[] textures )
    {
        Calls++;
        Bindings.DeleteTextures( textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTextures( int n, uint* textures )
    {
        Calls++;
        Bindings.GenTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTextures( int n )
    {
        Calls++;
        var result = Bindings.GenTextures( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTexture()
    {
        Calls++;
        var result = Bindings.GenTexture();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTexture( uint texture )
    {
        Calls++;
        var result = Bindings.IsTexture( texture );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElements( int mode, uint start, uint end, int count, int type, void* indices )
    {
        Calls++;
        Bindings.DrawRangeElements( mode, start, end, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElements< T >( int mode, uint start, uint end, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawRangeElements( mode, start, end, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage3D( int target,
                                   int level,
                                   int internalformat,
                                   int width,
                                   int height,
                                   int depth,
                                   int border,
                                   int format,
                                   int type,
                                   void* pixels )
    {
        Calls++;
        Bindings.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage3D< T >( int target,
                                 int level,
                                 int internalformat,
                                 int width,
                                 int height,
                                 int depth,
                                 int border,
                                 int format,
                                 int type,
                                 T[] pixels ) where T : unmanaged
    {
        Calls++;
        Bindings.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage3D( int target,
                                      int level,
                                      int xoffset,
                                      int yoffset,
                                      int zoffset,
                                      int width,
                                      int height,
                                      int depth,
                                      int format,
                                      int type,
                                      void* pixels )
    {
        Calls++;
        Bindings.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage3D< T >( int target,
                                    int level,
                                    int xoffset,
                                    int yoffset,
                                    int zoffset,
                                    int width,
                                    int height,
                                    int depth,
                                    int format,
                                    int type,
                                    T[] pixels ) where T : unmanaged
    {
        Calls++;
        Bindings.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Calls++;
        Bindings.CopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveTexture( int texture )
    {
        Calls++;
        Bindings.ActiveTexture( texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleCoverage( float value, bool invert )
    {
        Calls++;
        Bindings.SampleCoverage( value, invert );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage3D( int target,
                                             int level,
                                             int internalformat,
                                             int width,
                                             int height,
                                             int depth,
                                             int border,
                                             int imageSize,
                                             void* data )
    {
        Calls++;
        Bindings.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, byte[] data )
    {
        Calls++;
        Bindings.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, byte[] data )
    {
        Calls++;
        Bindings.CompressedTexImage2D( target, level, internalformat, width, height, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, byte[] data )
    {
        Calls++;
        Bindings.CompressedTexImage1D( target, level, internalformat, width, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexSubImage3D( int target,
                                                int level,
                                                int xoffset,
                                                int yoffset,
                                                int zoffset,
                                                int width,
                                                int height,
                                                int depth,
                                                int format,
                                                int imageSize,
                                                void* data )
    {
        Calls++;
        Bindings.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage3D( int target,
                                         int level,
                                         int xoffset,
                                         int yoffset,
                                         int zoffset,
                                         int width,
                                         int height,
                                         int depth,
                                         int format,
                                         byte[] data )
    {
        Calls++;
        Bindings.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexSubImage2D( int target,
                                                int level,
                                                int xoffset,
                                                int yoffset,
                                                int width,
                                                int height,
                                                int format,
                                                int imageSize,
                                                void* data )
    {
        Calls++;
        Bindings.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, byte[] data )
    {
        Calls++;
        Bindings.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Calls++;
        Bindings.CompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, byte[] data )
    {
        Calls++;
        Bindings.CompressedTexSubImage1D( target, level, xoffset, width, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTexImage( int target, int level, void* img )
    {
        Calls++;
        Bindings.GetCompressedTexImage( target, level, img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetCompressedTexImage( int target, int level, ref byte[] img )
    {
        Calls++;
        Bindings.GetCompressedTexImage( target, level, ref img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparate( int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha )
    {
        Calls++;
        Bindings.BlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArrays( int mode, int* first, int* count, int drawcount )
    {
        Calls++;
        Bindings.MultiDrawArrays( mode, first, count, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArrays( int mode, int[] first, int[] count )
    {
        Calls++;
        Bindings.MultiDrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElements( int mode, int* count, int type, void** indices, int drawcount )
    {
        Calls++;
        Bindings.MultiDrawElements( mode, count, type, indices, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElements< T >( int mode, int[] count, int type, T[][] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.MultiDrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterf( int pname, float param )
    {
        Calls++;
        Bindings.PointParameterf( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameterfv( int pname, float* parameters )
    {
        Calls++;
        Bindings.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterfv( int pname, float[] parameters )
    {
        Calls++;
        Bindings.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteri( int pname, int param )
    {
        Calls++;
        Bindings.PointParameteri( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameteriv( int pname, int* parameters )
    {
        Calls++;
        Bindings.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteriv( int pname, int[] parameters )
    {
        Calls++;
        Bindings.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        Bindings.BlendColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquation( int mode )
    {
        Calls++;
        Bindings.BlendEquation( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenQueries( int n, uint* ids )
    {
        Calls++;
        Bindings.GenQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenQueries( int n )
    {
        Calls++;
        var result = Bindings.GenQueries( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenQuery()
    {
        Calls++;
        var result = Bindings.GenQuery();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteQueries( int n, uint* ids )
    {
        Calls++;
        Bindings.DeleteQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteQueries( params uint[] ids )
    {
        Calls++;
        Bindings.DeleteQueries( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsQuery( uint id )
    {
        Calls++;
        var result = Bindings.IsQuery( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginQuery( int target, uint id )
    {
        Calls++;
        Bindings.BeginQuery( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQuery( int target )
    {
        Calls++;
        Bindings.EndQuery( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryiv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetQueryiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryiv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetQueryiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectiv( uint id, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetQueryObjectiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectiv( uint id, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetQueryObjectiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectuiv( uint id, int pname, uint* parameters )
    {
        Calls++;
        Bindings.GetQueryObjectuiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectuiv( uint id, int pname, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetQueryObjectuiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffer( int target, uint buffer )
    {
        Calls++;
        Bindings.BindBuffer( target, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteBuffers( int n, uint* buffers )
    {
        Calls++;
        Bindings.DeleteBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteBuffers( params uint[] buffers )
    {
        Calls++;
        Bindings.DeleteBuffers( buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenBuffers( int n, uint* buffers )
    {
        Calls++;
        Bindings.GenBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenBuffers( int n )
    {
        Calls++;
        var result = Bindings.GenBuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenBuffer()
    {
        Calls++;
        var result = Bindings.GenBuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsBuffer( uint buffer )
    {
        Calls++;
        var result = Bindings.IsBuffer( buffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void BufferData( int target, int size, void* data, int usage )
    {
        Calls++;
        Bindings.BufferData( target, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferData< T >( int target, T[] data, int usage )
        where T : unmanaged
    {
        Calls++;
        Bindings.BufferData( target, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        Bindings.BufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferSubData< T >( int target, int offsetCount, T[] data )
        where T : unmanaged
    {
        Calls++;
        Bindings.BufferSubData( target, offsetCount, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        Bindings.GetBufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferSubData< T >( int target, int offsetCount, int count, ref T[] data ) where T : unmanaged
    {
        Calls++;
        Bindings.GetBufferSubData( target, offsetCount, count, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBuffer( int target, int access )
    {
        Calls++;
        void* result = Bindings.MapBuffer( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBuffer< T >( int target, int access ) where T : unmanaged
    {
        Calls++;
        Span< T > result = Bindings.MapBuffer< T >( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool UnmapBuffer( int target )
    {
        Calls++;
        var result = Bindings.UnmapBuffer( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetBufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetBufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferPointerv( int target, int pname, void** parameters )
    {
        Calls++;
        Bindings.GetBufferPointerv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferPointerv( int target, int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Bindings.GetBufferPointerv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        Calls++;
        Bindings.BlendEquationSeparate( modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawBuffers( int n, int* bufs )
    {
        Calls++;
        Bindings.DrawBuffers( n, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffers( params int[] bufs )
    {
        Calls++;
        Bindings.DrawBuffers( bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOpSeparate( int face, int sfail, int dpfail, int dppass )
    {
        Calls++;
        Bindings.StencilOpSeparate( face, sfail, dpfail, dppass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFuncSeparate( int face, int func, int @ref, uint mask )
    {
        Calls++;
        Bindings.StencilFuncSeparate( face, func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMaskSeparate( int face, uint mask )
    {
        Calls++;
        Bindings.StencilMaskSeparate( face, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void AttachShader( uint program, uint shader )
    {
        Calls++;
        Bindings.AttachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindAttribLocation( uint program, uint index, byte* name )
    {
        Calls++;
        Bindings.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindAttribLocation( uint program, uint index, string name )
    {
        Calls++;
        Bindings.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompileShader( uint shader )
    {
        Calls++;
        Bindings.CompileShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateProgram()
    {
        Calls++;
        var result = Bindings.CreateProgram();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShader( int type )
    {
        Calls++;
        var result = Bindings.CreateShader( type );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DeleteProgram( uint program )
    {
        Calls++;
        Bindings.DeleteProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteShader( uint shader )
    {
        Calls++;
        Bindings.DeleteShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DetachShader( uint program, uint shader )
    {
        Calls++;
        Bindings.DetachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DisableVertexAttribArray( uint index )
    {
        Calls++;
        Bindings.DisableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexAttribArray( uint index )
    {
        Calls++;
        Bindings.EnableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAttrib( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Bindings.GetActiveAttrib( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveAttrib( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Bindings.GetActiveAttrib( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniform( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Bindings.GetActiveUniform( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniform( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Bindings.GetActiveUniform( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetAttachedShaders( uint program, int maxCount, int* count, uint* shaders )
    {
        Calls++;
        Bindings.GetAttachedShaders( program, maxCount, count, shaders );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetAttachedShaders( uint program, int maxCount )
    {
        Calls++;
        var result = Bindings.GetAttachedShaders( program, maxCount );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetAttribLocation( uint program, byte* name )
    {
        Calls++;
        var result = Bindings.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetAttribLocation( uint program, string name )
    {
        Calls++;
        var result = Bindings.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramiv( uint program, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetProgramiv( program, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramiv( uint program, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetProgramiv( program, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInfoLog( uint program, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Bindings.GetProgramInfoLog( program, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramInfoLog( uint program, int bufSize )
    {
        Calls++;
        var result = Bindings.GetProgramInfoLog( program, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderiv( uint shader, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetShaderiv( shader, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderiv( uint shader, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetShaderiv( shader, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderInfoLog( uint shader, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Bindings.GetShaderInfoLog( shader, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderInfoLog( uint shader, int bufSize )
    {
        Calls++;
        var result = Bindings.GetShaderInfoLog( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderSource( uint shader, int bufSize, int* length, byte* source )
    {
        Calls++;
        Bindings.GetShaderSource( shader, bufSize, length, source );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderSource( uint shader, int bufSize = 4096 )
    {
        Calls++;
        var result = Bindings.GetShaderSource( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetUniformLocation( uint program, byte* name )
    {
        Calls++;
        var result = Bindings.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetUniformLocation( uint program, string name )
    {
        Calls++;
        var result = Bindings.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetUniformfv( uint program, int location, float* parameters )
    {
        Calls++;
        Bindings.GetUniformfv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformfv( uint program, int location, ref float[] parameters )
    {
        Calls++;
        Bindings.GetUniformfv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformiv( uint program, int location, int* parameters )
    {
        Calls++;
        Bindings.GetUniformiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformiv( uint program, int location, ref int[] parameters )
    {
        Calls++;
        Bindings.GetUniformiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribdv( uint index, int pname, double* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribdv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribdv( uint index, int pname, ref double[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribdv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribfv( uint index, int pname, float* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribfv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribfv( uint index, int pname, ref float[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribfv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribiv( uint index, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribiv( uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribPointerv( uint index, int pname, void** pointer )
    {
        Calls++;
        Bindings.GetVertexAttribPointerv( index, pname, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribPointerv( uint index, int pname, ref uint[] pointer )
    {
        Calls++;
        Bindings.GetVertexAttribPointerv( index, pname, ref pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsProgram( uint program )
    {
        Calls++;
        var result = Bindings.IsProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsShader( uint shader )
    {
        Calls++;
        var result = Bindings.IsShader( shader );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void LinkProgram( uint program )
    {
        Calls++;
        Bindings.LinkProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderSource( uint shader, int count, byte** str, int* length )
    {
        Calls++;
        Bindings.ShaderSource( shader, count, str, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderSource( uint shader, params string[] @string )
    {
        Calls++;
        Bindings.ShaderSource( shader, @string );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgram( uint program )
    {
        Calls++;
        Bindings.UseProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1f( int location, float v0 )
    {
        Calls++;
        Bindings.Uniform1f( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2f( int location, float v0, float v1 )
    {
        Calls++;
        Bindings.Uniform2f( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3f( int location, float v0, float v1, float v2 )
    {
        Calls++;
        Bindings.Uniform3f( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4f( int location, float v0, float v1, float v2, float v3 )
    {
        Calls++;
        Bindings.Uniform4f( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1i( int location, int v0 )
    {
        Calls++;
        Bindings.Uniform1i( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2i( int location, int v0, int v1 )
    {
        Calls++;
        Bindings.Uniform2i( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3i( int location, int v0, int v1, int v2 )
    {
        Calls++;
        Bindings.Uniform3i( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4i( int location, int v0, int v1, int v2, int v3 )
    {
        Calls++;
        Bindings.Uniform4i( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1fv( int location, int count, float* value )
    {
        Calls++;
        Bindings.Uniform1fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1fv( int location, params float[] value )
    {
        Calls++;
        Bindings.Uniform1fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2fv( int location, int count, float* value )
    {
        Calls++;
        Bindings.Uniform2fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2fv( int location, params float[] value )
    {
        Calls++;
        Bindings.Uniform2fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3fv( int location, int count, float* value )
    {
        Calls++;
        Bindings.Uniform3fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3fv( int location, params float[] value )
    {
        Calls++;
        Bindings.Uniform3fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4fv( int location, int count, float* value )
    {
        Calls++;
        Bindings.Uniform4fv( location );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4fv( int location, params float[] value )
    {
        Calls++;
        Bindings.Uniform4fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1iv( int location, int count, int* value )
    {
        Calls++;
        Bindings.Uniform1iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1iv( int location, params int[] value )
    {
        Calls++;
        Bindings.Uniform1iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2iv( int location, int count, int* value )
    {
        Calls++;
        Bindings.Uniform2iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2iv( int location, params int[] value )
    {
        Calls++;
        Bindings.Uniform2iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3iv( int location, int count, int* value )
    {
        Calls++;
        Bindings.Uniform3iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3iv( int location, params int[] value )
    {
        Calls++;
        Bindings.Uniform3iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4iv( int location, int count, int* value )
    {
        Calls++;
        Bindings.Uniform4iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4iv( int location, params int[] value )
    {
        Calls++;
        Bindings.Uniform4iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool ValidateProgram( uint program )
    {
        Calls++;
        var result = Bindings.ValidateProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void VertexAttrib1d( uint index, double x )
    {
        Calls++;
        Bindings.VertexAttrib1d( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1dv( uint index, params double[] v )
    {
        Calls++;
        Bindings.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1f( uint index, float x )
    {
        Calls++;
        Bindings.VertexAttrib1f( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1fv( uint index, float* v )
    {
        Calls++;
        Bindings.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1fv( uint index, params float[] v )
    {
        Calls++;
        Bindings.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1s( uint index, short x )
    {
        Calls++;
        Bindings.VertexAttrib1s( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1sv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1sv( uint index, params short[] v )
    {
        Calls++;
        Bindings.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2d( uint index, double x, double y )
    {
        Calls++;
        Bindings.VertexAttrib2d( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2dv( uint index, params double[] v )
    {
        Calls++;
        Bindings.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2f( uint index, float x, float y )
    {
        Calls++;
        Bindings.VertexAttrib2f( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2fv( uint index, float* v )
    {
        Calls++;
        Bindings.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2fv( uint index, params float[] v )
    {
        Calls++;
        Bindings.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2s( uint index, short x, short y )
    {
        Calls++;
        Bindings.VertexAttrib2s( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2sv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2sv( uint index, params short[] v )
    {
        Calls++;
        Bindings.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3d( uint index, double x, double y, double z )
    {
        Calls++;
        Bindings.VertexAttrib3d( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3dv( uint index, params double[] v )
    {
        Calls++;
        Bindings.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3f( uint index, float x, float y, float z )
    {
        Calls++;
        Bindings.VertexAttrib3f( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3fv( uint index, float* v )
    {
        Calls++;
        Bindings.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3fv( uint index, params float[] v )
    {
        Calls++;
        Bindings.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3s( uint index, short x, short y, short z )
    {
        Calls++;
        Bindings.VertexAttrib3s( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3sv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3sv( uint index, params short[] v )
    {
        Calls++;
        Bindings.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nbv( uint index, sbyte* v )
    {
        Calls++;
        Bindings.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nbv( uint index, params sbyte[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Niv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Niv( uint index, params int[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nsv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nsv( uint index, params short[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nub( uint index, byte x, byte y, byte z, byte w )
    {
        Calls++;
        Bindings.VertexAttrib4Nub( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nubv( uint index, byte* v )
    {
        Calls++;
        Bindings.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nubv( uint index, params byte[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nuiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nuiv( uint index, params uint[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nusv( uint index, ushort* v )
    {
        Calls++;
        Bindings.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nusv( uint index, params ushort[] v )
    {
        Calls++;
        Bindings.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4bv( uint index, sbyte* v )
    {
        Calls++;
        Bindings.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4bv( uint index, params sbyte[] v )
    {
        Calls++;
        Bindings.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4d( uint index, double x, double y, double z, double w )
    {
        Calls++;
        Bindings.VertexAttrib4d( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4dv( uint index, double* v )
    {
        Calls++;
        Bindings.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4dv( uint index, params double[] v )
    {
        Calls++;
        Bindings.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4f( uint index, float x, float y, float z, float w )
    {
        Calls++;
        Bindings.VertexAttrib4f( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4fv( uint index, float* v )
    {
        Calls++;
        Bindings.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4fv( uint index, params float[] v )
    {
        Calls++;
        Bindings.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4iv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4iv( uint index, params int[] v )
    {
        Calls++;
        Bindings.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4s( uint index, short x, short y, short z, short w )
    {
        Calls++;
        Bindings.VertexAttrib4s( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4sv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4sv( uint index, params short[] v )
    {
        Calls++;
        Bindings.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4ubv( uint index, byte* v )
    {
        Calls++;
        Bindings.VertexAttrib4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4ubv( uint index, params byte[] v )
    {
        Calls++;
        Bindings.VertexAttrib4ubv( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4uiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4uiv( uint index, params uint[] v )
    {
        Calls++;
        Bindings.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4usv( uint index, ushort* v )
    {
        Calls++;
        Bindings.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4usv( uint index, params ushort[] v )
    {
        Calls++;
        Bindings.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, void* pointer )
    {
        Calls++;
        Bindings.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, uint pointer )
    {
        Calls++;
        Bindings.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix2x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix2x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix3x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix3x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix2x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix2x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix4x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix4x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix3x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix3x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Bindings.UniformMatrix4x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Bindings.UniformMatrix4x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMaski( uint index, bool r, bool g, bool b, bool a )
    {
        Calls++;
        Bindings.ColorMaski( index, r, g, b, a );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleani_v( int target, uint index, bool* data )
    {
        Calls++;
        Bindings.GetBooleani_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleani_v( int target, uint index, ref bool[] data )
    {
        Calls++;
        Bindings.GetBooleani_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegeri_v( int target, uint index, int* data )
    {
        Calls++;
        Bindings.GetIntegeri_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegeri_v( int target, uint index, ref int[] data )
    {
        Calls++;
        Bindings.GetIntegeri_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enablei( int target, uint index )
    {
        Calls++;
        Bindings.Enablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disablei( int target, uint index )
    {
        Calls++;
        Bindings.Disablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabledi( int target, uint index )
    {
        Calls++;
        var result = Bindings.IsEnabledi( target, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginTransformFeedback( int primitiveMode )
    {
        Calls++;
        Bindings.BeginTransformFeedback( primitiveMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndTransformFeedback()
    {
        Calls++;
        Bindings.EndTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferRange( int target, uint index, uint buffer, int offset, int size )
    {
        Calls++;
        Bindings.BindBufferRange( target, index, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferBase( int target, uint index, uint buffer )
    {
        Calls++;
        Bindings.BindBufferBase( target, index, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TransformFeedbackVaryings( uint program, int count, byte** varyings, int bufferMode )
    {
        Calls++;
        Bindings.TransformFeedbackVaryings( program, count, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackVaryings( uint program, string[] varyings, int bufferMode )
    {
        Calls++;
        Bindings.TransformFeedbackVaryings( program, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackVarying( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Bindings.GetTransformFeedbackVarying( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetTransformFeedbackVarying( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Bindings.GetTransformFeedbackVarying( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public void ClampColor( int target, int clamp )
    {
        Calls++;
        Bindings.ClampColor( target, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginConditionalRender( uint id, int mode )
    {
        Calls++;
        Bindings.BeginConditionalRender( id, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndConditionalRender()
    {
        Calls++;
        Bindings.EndConditionalRender();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribIPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Calls++;
        Bindings.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIPointer( uint index, int size, int type, int stride, uint pointer )
    {
        Calls++;
        Bindings.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIiv( uint index, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribIiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIiv( uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribIiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIuiv( uint index, int pname, uint* parameters )
    {
        Calls++;
        Bindings.GetVertexAttribIuiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIuiv( uint index, int pname, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetVertexAttribIuiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1i( uint index, int x )
    {
        Calls++;
        Bindings.VertexAttribI1i( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2i( uint index, int x, int y )
    {
        Calls++;
        Bindings.VertexAttribI2i( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3i( uint index, int x, int y, int z )
    {
        Calls++;
        Bindings.VertexAttribI3i( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4i( uint index, int x, int y, int z, int w )
    {
        Calls++;
        Bindings.VertexAttribI4i( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1ui( uint index, uint x )
    {
        Calls++;
        Bindings.VertexAttribI1ui( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2ui( uint index, uint x, uint y )
    {
        Calls++;
        Bindings.VertexAttribI2ui( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3ui( uint index, uint x, uint y, uint z )
    {
        Calls++;
        Bindings.VertexAttribI3ui( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ui( uint index, uint x, uint y, uint z, uint w )
    {
        Calls++;
        Bindings.VertexAttribI4ui( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1iv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1iv( uint index, int[] v )
    {
        Calls++;
        Bindings.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2iv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2iv( uint index, int[] v )
    {
        Calls++;
        Bindings.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3iv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3iv( uint index, int[] v )
    {
        Calls++;
        Bindings.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4iv( uint index, int* v )
    {
        Calls++;
        Bindings.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4iv( uint index, int[] v )
    {
        Calls++;
        Bindings.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1uiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1uiv( uint index, uint[] v )
    {
        Calls++;
        Bindings.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2uiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2uiv( uint index, uint[] v )
    {
        Calls++;
        Bindings.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3uiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3uiv( uint index, uint[] v )
    {
        Calls++;
        Bindings.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4uiv( uint index, uint* v )
    {
        Calls++;
        Bindings.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4uiv( uint index, uint[] v )
    {
        Calls++;
        Bindings.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4bv( uint index, sbyte* v )
    {
        Calls++;
        Bindings.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4bv( uint index, sbyte[] v )
    {
        Calls++;
        Bindings.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4sv( uint index, short* v )
    {
        Calls++;
        Bindings.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4sv( uint index, short[] v )
    {
        Calls++;
        Bindings.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4ubv( uint index, byte* v )
    {
        Calls++;
        Bindings.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ubv( uint index, byte[] v )
    {
        Calls++;
        Bindings.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4usv( uint index, ushort* v )
    {
        Calls++;
        Bindings.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4usv( uint index, ushort[] v )
    {
        Calls++;
        Bindings.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformuiv( uint program, int location, uint* parameters )
    {
        Calls++;
        Bindings.GetUniformuiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformuiv( uint program, int location, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetUniformuiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocation( uint program, uint color, byte* name )
    {
        Calls++;
        Bindings.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocation( uint program, uint color, string name )
    {
        Calls++;
        Bindings.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataLocation( uint program, byte* name )
    {
        Calls++;
        var result = Bindings.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataLocation( uint program, string name )
    {
        Calls++;
        var result = Bindings.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void Uniform1ui( int location, uint v0 )
    {
        Calls++;
        Bindings.Uniform1ui( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2ui( int location, uint v0, uint v1 )
    {
        Calls++;
        Bindings.Uniform2ui( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3ui( int location, uint v0, uint v1, uint v2 )
    {
        Calls++;
        Bindings.Uniform3ui( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4ui( int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Calls++;
        Bindings.Uniform4ui( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1uiv( int location, int count, uint* value )
    {
        Calls++;
        Bindings.Uniform1uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1uiv( int location, uint[] value )
    {
        Calls++;
        Bindings.Uniform1uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2uiv( int location, int count, uint* value )
    {
        Calls++;
        Bindings.Uniform2uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2uiv( int location, uint[] value )
    {
        Calls++;
        Bindings.Uniform2uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3uiv( int location, int count, uint* value )
    {
        Calls++;
        Bindings.Uniform3uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3uiv( int location, uint[] value )
    {
        Calls++;
        Bindings.Uniform3uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4uiv( int location, int count, uint* value )
    {
        Calls++;
        Bindings.Uniform4uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4uiv( int location, uint[] value )
    {
        Calls++;
        Bindings.Uniform4uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIiv( int target, int pname, int* param )
    {
        Calls++;
        Bindings.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIiv( int target, int pname, int[] param )
    {
        Calls++;
        Bindings.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIuiv( int target, int pname, uint* param )
    {
        Calls++;
        Bindings.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIuiv( int target, int pname, uint[] param )
    {
        Calls++;
        Bindings.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIiv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetTexParameterIiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIiv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetTexParameterIiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIuiv( int target, int pname, uint* parameters )
    {
        Calls++;
        Bindings.GetTexParameterIuiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIuiv( int target, int pname, ref uint[] parameters )
    {
        Calls++;
        Bindings.GetTexParameterIuiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferiv( int buffer, int drawbuffer, int* value )
    {
        Calls++;
        Bindings.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferiv( int buffer, int drawbuffer, int[] value )
    {
        Calls++;
        Bindings.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferuiv( int buffer, int drawbuffer, uint* value )
    {
        Calls++;
        Bindings.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferuiv( int buffer, int drawbuffer, uint[] value )
    {
        Calls++;
        Bindings.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferfv( int buffer, int drawbuffer, float* value )
    {
        Calls++;
        Bindings.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfv( int buffer, int drawbuffer, float[] value )
    {
        Calls++;
        Bindings.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
        Calls++;
        Bindings.ClearBufferfi( buffer, drawbuffer, depth, stencil );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetStringi( int name, uint index )
    {
        Calls++;
        var result = Bindings.GetStringi( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public string GetStringiSafe( int name, uint index )
    {
        Calls++;
        var result = Bindings.GetStringiSafe( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsRenderbuffer( uint renderbuffer )
    {
        Calls++;
        var result = Bindings.IsRenderbuffer( renderbuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindRenderbuffer( int target, uint renderbuffer )
    {
        Calls++;
        Bindings.BindRenderbuffer( target, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Bindings.DeleteRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteRenderbuffers( params uint[] renderbuffers )
    {
        Calls++;
        Bindings.DeleteRenderbuffers( renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Bindings.GenRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenRenderbuffers( int n )
    {
        Calls++;
        var result = Bindings.GenRenderbuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenRenderbuffer()
    {
        Calls++;
        var result = Bindings.GenRenderbuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void RenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.RenderbufferStorage( target, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetRenderbufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetRenderbufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetRenderbufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetRenderbufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsFramebuffer( uint framebuffer )
    {
        Calls++;
        var result = Bindings.IsFramebuffer( framebuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindFramebuffer( int target, uint framebuffer )
    {
        Calls++;
        Bindings.BindFramebuffer( target, framebuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Bindings.DeleteFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteFramebuffers( params uint[] framebuffers )
    {
        Calls++;
        Bindings.DeleteFramebuffers( framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Bindings.GenFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenFramebuffers( int n )
    {
        Calls++;
        var result = Bindings.GenFramebuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenFramebuffer()
    {
        Calls++;
        var result = Bindings.GenFramebuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int CheckFramebufferStatus( int target )
    {
        Calls++;
        var result = Bindings.CheckFramebufferStatus( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FramebufferTexture1D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        Bindings.FramebufferTexture1D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        Bindings.FramebufferTexture2D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture3D( int target, int attachment, int textarget, uint texture, int level, int zoffset )
    {
        Calls++;
        Bindings.FramebufferTexture3D( target, attachment, textarget, texture, level, zoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Calls++;
        Bindings.FramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetFramebufferAttachmentParameteriv( target, attachment, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateMipmap( int target )
    {
        Calls++;
        Bindings.GenerateMipmap( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Calls++;
        Bindings.BlitFramebuffer( srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        CheckErrors();
    }

    /// <inheritdoc />
    public void RenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
        Calls++;
        Bindings.RenderbufferStorageMultisample( target, samples, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTextureLayer( int target, int attachment, uint texture, int level, int layer )
    {
        Calls++;
        Bindings.FramebufferTextureLayer( target, attachment, texture, level, layer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBufferRange( int target, int offset, int length, uint access )
    {
        Calls++;
        void* result = Bindings.MapBufferRange( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBufferRange< T >( int target, int offset, int length, uint access )
        where T : unmanaged
    {
        Calls++;
        Span< T > result = Bindings.MapBufferRange< T >( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FlushMappedBufferRange( int target, int offset, int length )
    {
        Calls++;
        Bindings.FlushMappedBufferRange( target, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexArray( uint array )
    {
        Calls++;
        Bindings.BindVertexArray( array );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Bindings.DeleteVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteVertexArrays( params uint[] arrays )
    {
        Calls++;
        Bindings.DeleteVertexArrays( arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Bindings.GenVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenVertexArrays( int n )
    {
        Calls++;
        var result = Bindings.GenVertexArrays( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenVertexArray()
    {
        Calls++;
        var result = Bindings.GenVertexArray();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsVertexArray( uint array )
    {
        Calls++;
        var result = Bindings.IsVertexArray( array );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DrawArraysInstanced( int mode, int first, int count, int instancecount )
    {
        Calls++;
        Bindings.DrawArraysInstanced( mode, first, count, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstanced( int mode, int count, int type, void* indices, int instancecount )
    {
        Calls++;
        Bindings.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstanced< T >( int mode, int count, int type, T[] indices, int instancecount )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexBuffer( int target, int internalformat, uint buffer )
    {
        Calls++;
        Bindings.TexBuffer( target, internalformat, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PrimitiveRestartIndex( uint index )
    {
        Calls++;
        Bindings.PrimitiveRestartIndex( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
        Calls++;
        Bindings.CopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformIndices( uint program, int uniformCount, byte** uniformNames, uint* uniformIndices )
    {
        Calls++;
        Bindings.GetUniformIndices( program, uniformCount, uniformNames, uniformIndices );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetUniformIndices( uint program, params string[] uniformNames )
    {
        Calls++;
        var result = Bindings.GetUniformIndices( program, uniformNames );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformsiv( uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetActiveUniformsiv( uint program, int pname, params uint[] uniformIndices )
    {
        Calls++;
        var result = Bindings.GetActiveUniformsiv( program, pname, uniformIndices );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformName( uint program, uint uniformIndex, int bufSize, int* length, byte* uniformName )
    {
        Calls++;
        Bindings.GetActiveUniformName( program, uniformIndex, bufSize, length, uniformName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformName( uint program, uint uniformIndex, int bufSize )
    {
        Calls++;
        var result = Bindings.GetActiveUniformName( program, uniformIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetUniformBlockIndex( uint program, byte* uniformBlockName )
    {
        Calls++;
        var result = Bindings.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetUniformBlockIndex( uint program, string uniformBlockName )
    {
        Calls++;
        var result = Bindings.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, int* parameters )
    {
        Calls++;
        Bindings.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, ref int[] parameters )
    {
        Calls++;
        Bindings.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName )
    {
        Calls++;
        Bindings.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize, length, uniformBlockName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize )
    {
        Calls++;
        var result = Bindings.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void UniformBlockBinding( uint program, uint uniformBlockIndex, uint uniformBlockBinding )
    {
        Calls++;
        Bindings.UniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsBaseVertex( int mode, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        Bindings.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsBaseVertex< T >( int mode, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElementsBaseVertex( int mode, uint start, uint end, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        Bindings.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElementsBaseVertex< T >( int mode, uint start, uint end, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertex( int mode, int count, int type, void* indices, int instancecount, int basevertex )
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertex< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsBaseVertex( int mode, int* count, int type, void** indices, int drawcount, int* basevertex )
    {
        Calls++;
        Bindings.MultiDrawElementsBaseVertex( mode, count, type, indices, drawcount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsBaseVertex< T >( int mode, int type, T[][] indices, int[] basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Bindings.MultiDrawElementsBaseVertex( mode, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProvokingVertex( int mode )
    {
        Calls++;
        Bindings.ProvokingVertex( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* FenceSync( int condition, uint flags )
    {
        Calls++;
        void* result = Bindings.FenceSync( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public IntPtr FenceSyncSafe( int condition, uint flags )
    {
        Calls++;
        var result = Bindings.FenceSyncSafe( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe bool IsSync( void* sync )
    {
        Calls++;
        var result = Bindings.IsSync( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsSyncSafe( IntPtr sync )
    {
        Calls++;
        var result = Bindings.IsSyncSafe( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSync( void* sync )
    {
        Calls++;
        Bindings.DeleteSync( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSyncSafe( IntPtr sync )
    {
        Calls++;
        Bindings.DeleteSyncSafe( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int ClientWaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        var result = Bindings.ClientWaitSync( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int ClientWaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        var result = Bindings.ClientWaitSyncSafe( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void WaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        Bindings.WaitSync( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public void WaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        Bindings.WaitSyncSafe( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInteger64v( int pname, long* data )
    {
        Calls++;
        Bindings.GetInteger64v( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64v( int pname, ref long[] data )
    {
        Calls++;
        Bindings.GetInteger64v( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSynciv( void* sync, int pname, int bufSize, int* length, int* values )
    {
        Calls++;
        Bindings.GetSynciv( sync, pname, bufSize, length, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetSynciv( IntPtr sync, int pname, int bufSize )
    {
        Calls++;
        var result = Bindings.GetSynciv( sync, pname, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetInteger64i_v( int target, uint index, long* data )
    {
        Calls++;
        Bindings.GetInteger64i_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64i_v( int target, uint index, ref long[] data )
    {
        Calls++;
        Bindings.GetInteger64i_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteri64v( int target, int pname, long* parameters )
    {
        Calls++;
        Bindings.GetBufferParameteri64v( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteri64v( int target, int pname, ref long[] parameters )
    {
        Calls++;
        Bindings.GetBufferParameteri64v( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture( int target, int attachment, uint texture, int level )
    {
        Calls++;
        Bindings.FramebufferTexture( target, attachment, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TexImage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Bindings.TexImage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetMultisamplefv( int pname, uint index, float* val )
    {
        Calls++;
        Bindings.GetMultisamplefv( pname, index, val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetMultisamplefvSafe( int pname, uint index, ref float[] val )
    {
        Calls++;
        Bindings.GetMultisamplefvSafe( pname, index, ref val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleMaski( uint maskNumber, uint mask )
    {
        Calls++;
        Bindings.SampleMaski( maskNumber, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, byte* name )
    {
        Calls++;
        Bindings.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, string name )
    {
        Calls++;
        Bindings.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataIndex( uint program, byte* name )
    {
        Calls++;
        var result = Bindings.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataIndex( uint program, string name )
    {
        Calls++;
        var result = Bindings.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GenSamplers( int count, uint* samplers )
    {
        Calls++;
        Bindings.GenSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenSamplers( int count )
    {
        Calls++;
        var result = Bindings.GenSamplers( count );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenSampler()
    {
        Calls++;
        var result = Bindings.GenSampler();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSamplers( int count, uint* samplers )
    {
        Calls++;
        Bindings.DeleteSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSamplers( params uint[] samplers )
    {
        Calls++;
        Bindings.DeleteSamplers( samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsSampler( uint sampler )
    {
        Calls++;
        var result = Bindings.IsSampler( sampler );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindSampler( uint unit, uint sampler )
    {
        Calls++;
        Bindings.BindSampler( unit, sampler );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteri( uint sampler, int pname, int param )
    {
        Calls++;
        Bindings.SamplerParameteri( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        Bindings.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteriv( uint sampler, int pname, int[] param )
    {
        Calls++;
        Bindings.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterf( uint sampler, int pname, float param )
    {
        Calls++;
        Bindings.SamplerParameterf( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        Bindings.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterfv( uint sampler, int pname, float[] param )
    {
        Calls++;
        Bindings.SamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        Bindings.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIiv( uint sampler, int pname, int[] param )
    {
        Calls++;
        Bindings.SamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        Bindings.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIuiv( uint sampler, int pname, uint[] param )
    {
        Calls++;
        Bindings.SamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        Bindings.GetSamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameteriv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetSamplerParameteriv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        Bindings.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIiv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        Bindings.GetSamplerParameterIiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        Bindings.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterfv( uint sampler, int pname, ref float[] param )
    {
        Calls++;
        Bindings.GetSamplerParameterfv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        Bindings.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIuiv( uint sampler, int pname, ref uint[] param )
    {
        Calls++;
        Bindings.GetSamplerParameterIuiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void QueryCounter( uint id, int target )
    {
        Calls++;
        Bindings.QueryCounter( id, target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjecti64v( uint id, int pname, long* param )
    {
        Calls++;
        Bindings.GetQueryObjecti64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjecti64v( uint id, int pname, ref long[] param )
    {
        Calls++;
        Bindings.GetQueryObjecti64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectui64v( uint id, int pname, ulong* param )
    {
        Calls++;
        Bindings.GetQueryObjectui64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectui64v( uint id, int pname, ref ulong[] param )
    {
        Calls++;
        Bindings.GetQueryObjectui64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribDivisor( uint index, uint divisor )
    {
        Calls++;
        Bindings.VertexAttribDivisor( index, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Bindings.VertexAttribP1ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP1uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Bindings.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Bindings.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Bindings.VertexAttribP2ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP2uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Bindings.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Bindings.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Bindings.VertexAttribP3ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP3uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Bindings.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Bindings.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Bindings.VertexAttribP4ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP4uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Bindings.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Bindings.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

//    /// <inheritdoc />
//    public void Import()
//    {
//        
//        Calls++;
//        Bindings.Import();
//        CheckErrors();
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public unsafe string GetActiveUniform( int handle, int u, int* bufSize, Buffer buffer )
//    {
//         Calls++;
//        var result = GdxApi.GL.GetActiveUniform( handle, u, bufSize, buffer );
//        CheckErrors();
//
//        return result;
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void VertexAttribPointer( int location, int size, int type, bool normalized, int stride, Buffer buffer )
//    {
//         Calls++;
//        GdxApi.GL.VertexAttribPointer( location, size, type, normalized, stride, buffer );
//        CheckErrors();
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void UniformMatrix4fv( int location, int count, bool transpose, Buffer buffer )
//    {
//         Calls++;
//        GdxApi.GL.UniformMatrix4fv( location, count, transpose, buffer );
//        CheckErrors();
//    }
}