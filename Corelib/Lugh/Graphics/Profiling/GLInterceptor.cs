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
        Gdx.GL.MinSampleShading( value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationi( uint buf, int mode )
    {
        Calls++;
        Gdx.GL.BlendEquationi( buf, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparatei( uint buf, int modeRGB, int modeAlpha )
    {
        Calls++;
        Gdx.GL.BlendEquationSeparatei( buf, modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunci( uint buf, int src, int dst )
    {
        Calls++;
        Gdx.GL.BlendFunci( buf, src, dst );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparatei( uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
    {
        Calls++;
        Gdx.GL.BlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawArraysIndirect( int mode, void* indirect )
    {
        Calls++;
        Gdx.GL.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect )
    {
        Calls++;
        Gdx.GL.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsIndirect( int mode, int type, void* indirect )
    {
        Calls++;
        Gdx.GL.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect )
    {
        Calls++;
        Gdx.GL.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1d( int location, double x )
    {
        Calls++;
        Gdx.GL.Uniform1d( location, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2d( int location, double x, double y )
    {
        Calls++;
        Gdx.GL.Uniform2d( location, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3d( int location, double x, double y, double z )
    {
        Calls++;
        Gdx.GL.Uniform3d( location, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4d( int location, double x, double y, double z, double w )
    {
        Calls++;
        Gdx.GL.Uniform4d( location, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1dv( int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.Uniform1dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1dv( int location, double[] value )
    {
        Calls++;
        Gdx.GL.Uniform1dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2dv( int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.Uniform2dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2dv( int location, double[] value )
    {
        Calls++;
        Gdx.GL.Uniform2dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3dv( int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.Uniform3dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3dv( int location, double[] value )
    {
        Calls++;
        Gdx.GL.Uniform3dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4dv( int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.Uniform4dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4dv( int location, double[] value )
    {
        Calls++;
        Gdx.GL.Uniform4dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3dv( int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3dv( int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformdv( uint program, int location, double* parameters )
    {
        Calls++;
        Gdx.GL.GetUniformdv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformdv( uint program, int location, ref double[] parameters )
    {
        Calls++;
        Gdx.GL.GetUniformdv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetSubroutineUniformLocation( uint program, int shadertype, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetSubroutineUniformLocation( uint program, int shadertype, string name )
    {
        Calls++;
        var result = Gdx.GL.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetSubroutineIndex( uint program, int shadertype, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetSubroutineIndex( uint program, int shadertype, string name )
    {
        Calls++;
        var result = Gdx.GL.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, int* values )
    {
        Calls++;
        Gdx.GL.GetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, ref int[] values )
    {
        Calls++;
        Gdx.GL.GetActiveSubroutineUniformiv( program, shadertype, index, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Calls++;
        Gdx.GL.GetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize )
    {
        Calls++;
        var result = Gdx.GL.GetActiveSubroutineUniformName( program, shadertype, index, bufsize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Calls++;
        Gdx.GL.GetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize )
    {
        Calls++;
        var result = Gdx.GL.GetActiveSubroutineName( program, shadertype, index, bufsize );
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
        Gdx.GL.UniformSubroutinesuiv( shadertype, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformSubroutineuiv( int shadertype, int location, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetUniformSubroutineuiv( shadertype, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformSubroutineuiv( int shadertype, int location, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetUniformSubroutineuiv( shadertype, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramStageiv( uint program, int shadertype, int pname, int* values )
    {
        Calls++;
        Gdx.GL.GetProgramStageiv( program, shadertype, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramStageiv( uint program, int shadertype, int pname, ref int[] values )
    {
        Calls++;
        Gdx.GL.GetProgramStageiv( program, shadertype, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameteri( int pname, int value )
    {
        Calls++;
        Gdx.GL.PatchParameteri( pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PatchParameterfv( int pname, float* values )
    {
        Calls++;
        Gdx.GL.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameterfv( int pname, float[] values )
    {
        Calls++;
        Gdx.GL.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTransformFeedback( int target, uint id )
    {
        Calls++;
        Gdx.GL.BindTransformFeedback( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Gdx.GL.DeleteTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTransformFeedbacks( params uint[] ids )
    {
        Calls++;
        Gdx.GL.DeleteTransformFeedbacks( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Gdx.GL.GenTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTransformFeedbacks( int n )
    {
        Calls++;
        var result = Gdx.GL.GenTransformFeedbacks( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTransformFeedback()
    {
        Calls++;
        var result = Gdx.GL.GenTransformFeedback();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTransformFeedback( uint id )
    {
        Calls++;
        var result = Gdx.GL.IsTransformFeedback( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void PauseTransformFeedback()
    {
        Calls++;
        Gdx.GL.PauseTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void ResumeTransformFeedback()
    {
        Calls++;
        Gdx.GL.ResumeTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedback( int mode, uint id )
    {
        Calls++;
        Gdx.GL.DrawTransformFeedback( mode, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStream( int mode, uint id, uint stream )
    {
        Calls++;
        Gdx.GL.DrawTransformFeedbackStream( mode, id, stream );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginQueryIndexed( int target, uint index, uint id )
    {
        Calls++;
        Gdx.GL.BeginQueryIndexed( target, index, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQueryIndexed( int target, uint index )
    {
        Calls++;
        Gdx.GL.EndQueryIndexed( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryIndexediv( int target, uint index, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetQueryIndexediv( target, index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryIndexediv( int target, uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetQueryIndexediv( target, index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReleaseShaderCompiler()
    {
        Calls++;
        Gdx.GL.ReleaseShaderCompiler();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderBinary( int count, uint* shaders, int binaryformat, void* binary, int length )
    {
        Calls++;
        Gdx.GL.ShaderBinary( count, shaders, binaryformat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderBinary( uint[] shaders, int binaryformat, byte[] binary )
    {
        Calls++;
        Gdx.GL.ShaderBinary( shaders, binaryformat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderPrecisionFormat( int shaderType, int precisionType, int* range, int* precision )
    {
        Calls++;
        Gdx.GL.GetShaderPrecisionFormat( shaderType, precisionType, range, precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderPrecisionFormat( int shaderType, int precisionType, ref int[] range, ref int precision )
    {
        Calls++;
        Gdx.GL.GetShaderPrecisionFormat( shaderType, precisionType, ref range, ref precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangef( float n, float f )
    {
        Calls++;
        Gdx.GL.DepthRangef( n, f );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepthf( float d )
    {
        Calls++;
        Gdx.GL.ClearDepthf( d );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramBinary( uint program, int bufSize, int* length, int* binaryFormat, void* binary )
    {
        Calls++;
        Gdx.GL.GetProgramBinary( program, bufSize, length, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetProgramBinary( uint program, int bufSize, out int binaryFormat )
    {
        Calls++;
        var result = Gdx.GL.GetProgramBinary( program, bufSize, out binaryFormat );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void ProgramBinary( uint program, int binaryFormat, void* binary, int length )
    {
        Calls++;
        Gdx.GL.ProgramBinary( program, binaryFormat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramBinary( uint program, int binaryFormat, byte[] binary )
    {
        Calls++;
        Gdx.GL.ProgramBinary( program, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramParameteri( uint program, int pname, int value )
    {
        Calls++;
        Gdx.GL.ProgramParameteri( program, pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgramStages( uint pipeline, uint stages, uint program )
    {
        Calls++;
        Gdx.GL.UseProgramStages( pipeline, stages, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveShaderProgram( uint pipeline, uint program )
    {
        Calls++;
        Gdx.GL.ActiveShaderProgram( pipeline, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint CreateShaderProgramv( int type, int count, byte** strings )
    {
        Calls++;
        var result = Gdx.GL.CreateShaderProgramv( type, count, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShaderProgramv( int type, string[] strings )
    {
        Calls++;
        var result = Gdx.GL.CreateShaderProgramv( type, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindProgramPipeline( uint pipeline )
    {
        Calls++;
        Gdx.GL.BindProgramPipeline( pipeline );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Gdx.GL.DeleteProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteProgramPipelines( params uint[] pipelines )
    {
        Calls++;
        Gdx.GL.DeleteProgramPipelines ( pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Gdx.GL.GenProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenProgramPipelines( int n )
    {
        Calls++;
        var result = Gdx.GL.GenProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenProgramPipeline()
    {
        Calls++;
        var result = Gdx.GL.GenProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsProgramPipeline( uint pipeline )
    {
        Calls++;
        var result = Gdx.GL.IsProgramPipeline( pipeline );
        CheckErrors();
        
        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineiv( uint pipeline, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetProgramPipelineiv( pipeline, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramPipelineiv( uint pipeline, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.GetProgramPipelineiv( pipeline, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1i( uint program, int location, int v0 )
    {
        Calls++;
        Gdx.GL.ProgramUniform1i( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1iv( uint program, int location, int[] value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1f( uint program, int location, float v0 )
    {
        Calls++;
        Gdx.GL.ProgramUniform1f( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1fv( uint program, int location, float[] value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1d( uint program, int location, double v0 )
    {
        Calls++;
        Gdx.GL.ProgramUniform1d( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1dv( uint program, int location, double[] value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1ui( uint program, int location, uint v0 )
    {
        Calls++;
        Gdx.GL.ProgramUniform1ui( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.ProgramUniform1uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2i( uint program, int location, int v0, int v1 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2iv( uint program, int location, int[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2f( uint program, int location, float v0, float v1 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2fv( uint program, int location, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2d( uint program, int location, double v0, double v1 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2dv( uint program, int location, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2ui( uint program, int location, uint v0, uint v1 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3i( uint program, int location, int v0, int v1, int v2 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3iv( uint program, int location, int[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3f( uint program, int location, float v0, float v1, float v2 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3fv( uint program, int location, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3d( uint program, int location, double v0, double v1, double v2 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3dv( uint program, int location, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3ui( uint program, int location, uint v0, uint v1, uint v2 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4i( uint program, int location, int v0, int v1, int v2, int v3 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4iv( uint program, int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4iv( uint program, int location, int[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4f( uint program, int location, float v0, float v1, float v2, float v3 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4fv( uint program, int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4fv( uint program, int location, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4d( uint program, int location, double v0, double v1, double v2, double v3 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4dv( uint program, int location, int count, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4dv( uint program, int location, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4ui( uint program, int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4uiv( uint program, int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4uiv( uint program, int location, uint[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3fv( uint program, int location, bool transpose, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3dv( uint program, int location, bool transpose, double[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ValidateProgramPipeline( uint pipeline )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineInfoLog( uint pipeline, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramPipelineInfoLog( uint pipeline, int bufSize )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL1d( uint index, double x )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2d( uint index, double x, double y )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3d( uint index, double x, double y, double z )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4d( uint index, double x, double y, double z, double w )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL1dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL1dv( uint index, double[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL2dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2dv( uint index, double[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL3dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3dv( uint index, double[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL4dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4dv( uint index, double[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribLPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLPointer( uint index, int size, int type, int stride, int pointer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribLdv( uint index, int pname, double* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribLdv( uint index, int pname, ref double[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportArrayv( uint first, int count, float* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportArrayv( uint first, int count, params float[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedf( uint index, float x, float y, float w, float h )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportIndexedfv( uint index, float* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedfv( uint index, params float[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorArrayv( uint first, int count, int* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorArrayv( uint first, int count, params int[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexed( uint index, int left, int bottom, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorIndexedv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexedv( uint index, params int[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DepthRangeArrayv( uint first, int count, double* v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeArrayv( uint first, int count, params double[] v )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeIndexed( uint index, double n, double f )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFloati_v( int target, uint index, float* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloati_v( int target, uint index, ref float[] data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublei_v( int target, uint index, double* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublei_v( int target, uint index, ref double[] data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysInstancedBaseInstance( int mode, int first, int count, int instancecount, uint baseinstance )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseInstance( int mode, int count, int type, void* indices, int instancecount, uint baseinstance )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, uint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertexBaseInstance( int mode, int count, int type, void* indices, int instancecount, int basevertex, uint baseinstance )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertexBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex, uint baseinstance ) where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformativ( int target, int internalformat, int pname, int bufSize, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformativ( int target, int internalformat, int pname, int bufSize, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTexture( uint unit, uint texture, int level, bool layered, int layer, int access, int format )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrier( uint barriers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage1D( int target, int levels, int internalformat, int width )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2D( int target, int levels, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3D( int target, int levels, int internalformat, int width, int height, int depth )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackInstanced( int mode, uint id, int instancecount )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStreamInstanced( int mode, uint id, uint stream, int instancecount )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetPointerv( int pname, void** parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetPointerv( int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferData( int target, int internalformat, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferData< T >( int target, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferSubData( int target, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferSubData< T >( int target, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchCompute( uint num_groups_x, uint num_groups_y, uint num_groups_z )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DispatchComputeIndirect( void* indirect )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchComputeIndirect( GLBindings.DispatchIndirectCommand indirect )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CopyImageSubData( uint srcName, int srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, int dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferParameteri( int target, int pname, int param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformati64v( int target, int internalformat, int pname, int count, long* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformati64v( int target, int internalformat, int pname, int count, ref long[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexImage( uint texture, int level )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferSubData( uint buffer, int offset, int length )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferData( uint buffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateFramebuffer( int target, int numAttachments, int* attachments )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateFramebuffer( int target, int numAttachments, int[] attachments )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateSubFramebuffer( int target, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateSubFramebuffer( int target, int numAttachments, int[] attachments, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirect( int mode, void* indirect, int drawcount, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirect( int mode, int type, void* indirect, int drawcount, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInterfaceiv( uint program, int programInterface, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramInterfaceiv( uint program, int programInterface, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetProgramResourceIndex( uint program, int programInterface, byte* name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint GetProgramResourceIndex( uint program, int programInterface, string name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceName( uint program, int programInterface, uint index, int bufSize, int* length, byte* name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramResourceName( uint program, int programInterface, uint index, int bufSize )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceiv( uint program, int programInterface, uint index, int propCount, int* props, int bufSize, int* length, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramResourceiv( uint program, int programInterface, uint index, int[] props, int bufSize, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocation( uint program, int programInterface, byte* name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public int GetProgramResourceLocation( uint program, int programInterface, string name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocationIndex( uint program, int programInterface, byte* name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public int GetProgramResourceLocationIndex( uint program, int programInterface, string name )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderStorageBlockBinding( uint program, uint storageBlockIndex, uint storageBlockBinding )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexBufferRange( int target, int internalformat, uint buffer, int offset, int size )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureView( uint texture, int target, uint origtexture, int internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffer( uint bindingindex, uint buffer, int offset, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribFormat( uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribBinding( uint attribindex, uint bindingindex )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexBindingDivisor( uint bindingindex, uint divisor )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageControl( int source, int type, int severity, int count, uint* ids, bool enabled )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageControl( int source, int type, int severity, uint[] ids, bool enabled )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageInsert( int source, int type, uint id, int severity, int length, byte* buf )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageInsert( int source, int type, uint id, int severity, string buf )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROC callback, void* userParam )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROCSAFE callback, void* userParam )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetDebugMessageLog( uint count, int bufsize, int* sources, int* types, uint* ids, int* severities, int* lengths, byte* messageLog )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint GetDebugMessageLog( uint count, int bufSize, out int[] sources, out int[] types, out uint[] ids, out int[] severities, out string[] messageLog )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PushDebugGroup( int source, uint id, int length, byte* message )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void PushDebugGroup( int source, uint id, string message )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void PopDebugGroup()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ObjectLabel( int identifier, uint name, int length, byte* label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectLabel( int identifier, uint name, string label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectLabel( int identifier, uint name, int bufSize, int* length, byte* label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectLabel( int identifier, uint name, int bufSize )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ObjectPtrLabel( void* ptr, int length, byte* label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectPtrLabel( IntPtr ptr, string label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectPtrLabel( void* ptr, int bufSize, int* length, byte* label )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectPtrLabel( IntPtr ptr, int bufSize )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BufferStorage( int target, int size, void* data, uint flags )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BufferStorage< T >( int target, T[] data, uint flags ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexImage( uint texture, int level, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexImage< T >( uint texture, int level, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexSubImage( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexSubImage< T >( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersBase( int target, uint first, int count, uint* buffers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersBase( int target, uint first, uint[] buffers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersRange( int target, uint first, int count, uint* buffers, int* offsets, int* sizes )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersRange( int target, uint first, uint[] buffers, int[] offsets, int[] sizes )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindTextures( uint first, int count, uint* textures )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextures( uint first, uint[] textures )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindSamplers( uint first, int count, uint* samplers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindSamplers( uint first, uint[] samplers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindImageTextures( uint first, int count, uint* textures )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTextures( uint first, uint[] textures )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindVertexBuffers( uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffers( uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClipControl( int origin, int depth )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTransformFeedbacks( int n, uint* ids )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTransformFeedbacks( int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateTransformFeedbacks()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferBase( uint xfb, uint index, uint buffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferRange( uint xfb, uint index, uint buffer, int offset, int size )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackiv( uint xfb, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbackiv( uint xfb, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki_v( uint xfb, int pname, uint index, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki_v( uint xfb, int pname, uint index, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, long* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, ref long[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateBuffers( int n, uint* buffers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateBuffers( int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateBuffer()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferStorage( uint buffer, int size, void* data, uint flags )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferStorage< T >( uint buffer, int size, T[] data, uint flags ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferData( uint buffer, int size, void* data, int usage )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferData< T >( uint buffer, int size, T[] data, int usage ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferSubData< T >( uint buffer, int offset, int size, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CopyNamedBufferSubData( uint readBuffer, uint writeBuffer, int readOffset, int writeOffset, int size )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferData( uint buffer, int internalformat, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferData< T >( uint buffer, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferSubData( uint buffer, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferSubData< T >( uint buffer, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBuffer( uint buffer, int access )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public Span< T > MapNamedBuffer< T >( uint buffer, int access ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBufferRange( uint buffer, int offset, int length, uint access )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public Span< T > MapNamedBufferRange< T >( uint buffer, int offset, int length, uint access ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public bool UnmapNamedBuffer( uint buffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void FlushMappedNamedBufferRange( uint buffer, int offset, int length )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteriv( uint buffer, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteriv( uint buffer, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteri64v( uint buffer, int pname, long* parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteri64v( uint buffer, int pname, ref long[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferPointerv( uint buffer, int pname, void** parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferPointerv( uint buffer, int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetNamedBufferSubData< T >( uint buffer, int offset, int size ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateFramebuffers( int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateFramebuffer()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferRenderbuffer( uint framebuffer, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferParameteri( uint framebuffer, int pname, int param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTexture( uint framebuffer, int attachment, uint texture, int level )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTextureLayer( uint framebuffer, int attachment, uint texture, int level, int layer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffer( uint framebuffer, int buf )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedFramebufferDrawBuffers( uint framebuffer, int n, int* bufs )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffers( uint framebuffer, int[] bufs )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferReadBuffer( uint framebuffer, int src )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferData( uint framebuffer, int numAttachments, int* attachments )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferData( uint framebuffer, int[] attachments )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferSubData( uint framebuffer, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferSubData( uint framebuffer, int[] attachments, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float* value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float[] value )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfi( uint framebuffer, int buffer, float depth, int stencil )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BlitNamedFramebuffer( uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public int CheckNamedFramebufferStatus( uint framebuffer, int target )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferParameteriv( uint framebuffer, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferParameteriv( uint framebuffer, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, int* params_ )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, ref int[] params_ )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateRenderbuffers( int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateRenderbuffer()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorage( uint renderbuffer, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorageMultisample( uint renderbuffer, int samples, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTextures( int target, int n, uint* textures )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTextures( int target, int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateTexture( int target )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureBuffer( uint texture, int internalformat, uint buffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureBufferRange( uint texture, int internalformat, uint buffer, int offset, int size )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage1D( uint texture, int levels, int internalformat, int width )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2D( uint texture, int levels, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3D( uint texture, int levels, int internalformat, int width, int height, int depth )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2DMultisample( uint texture, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3DMultisample( uint texture, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage1D< T >( uint texture, int level, int xoffset, int width, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage2D< T >( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage3D< T >( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, T[] pixels ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, byte[] data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, byte[] data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, byte[] data )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage1D( uint texture, int level, int xoffset, int x, int y, int width )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterf( uint texture, int pname, float param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterfv( uint texture, int pname, float* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterfv( uint texture, int pname, float[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteri( uint texture, int pname, int param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIiv( uint texture, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIiv( uint texture, int pname, int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIuiv( uint texture, int pname, uint[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameteriv( uint texture, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteriv( uint texture, int pname, int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateTextureMipmap( uint texture )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextureUnit( uint unit, uint texture )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureImage( uint texture, int level, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetTextureImage< T >( uint texture, int level, int format, int type, int bufSize ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureImage( uint texture, int level, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureImage( uint texture, int level, int bufSize )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameterfv( uint texture, int level, int pname, float* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameterfv( uint texture, int level, int pname, ref float[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameteriv( uint texture, int level, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameteriv( uint texture, int level, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterfv( uint texture, int pname, float* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterfv( uint texture, int pname, ref float[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIiv( uint texture, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIiv( uint texture, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIuiv( uint texture, int pname, ref uint[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameteriv( uint texture, int pname, int* param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameteriv( uint texture, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateVertexArrays( int n )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateVertexArray()
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void DisableVertexArrayAttrib( uint vaobj, uint index )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexArrayAttrib( uint vaobj, uint index )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayElementBuffer( uint vaobj, uint buffer )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffer( uint vaobj, uint bindingindex, uint buffer, int offset, int stride )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexArrayVertexBuffers( uint vaobj, uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffers( uint vaobj, uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribBinding( uint vaobj, uint attribindex, uint bindingindex )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribFormat( uint vaobj, uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribIFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribLFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Calls++;
        Gdx.GL.CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayBindingDivisor( uint vaobj, uint bindingindex, uint divisor )
    {
        Calls++;
        Gdx.GL.\nCheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayiv( uint vaobj, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayiv( uint vaobj, int pname, int * param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayiv( uint vaobj, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayiv( uint vaobj, int pname, ref int[] param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayIndexediv( uint vaobj, uint index, int pname, int * param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayIndexediv( uint vaobj, uint index, int pname, ref int[] param );
            CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, long* param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayIndexed64iv( vaobj, index, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, ref long[] param )
    {
        Calls++;
        Gdx.GL.GetVertexArrayIndexed64iv( vaobj, index, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateSamplers( int n, uint* samplers )
    {
        Calls++;
        Gdx.GL.CreateSamplers( n, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateSamplers( int n )
    {
        Calls++;
        var result = Gdx.GL.CreateSamplers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateSamplers()
    {
        Calls++;
        var result = Gdx.GL.CreateSamplers();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateProgramPipelines( int n, uint* pipelines )
    {
        Calls++;
        Gdx.GL.CreateProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateProgramPipelines( int n )
    {
        Calls++;
        var result = Gdx.GL.CreateProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateProgramPipeline()
    {
        Calls++;
        var result = Gdx.GL.CreateProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateQueries( int target, int n, uint* ids )
    {
        Calls++;
        Gdx.GL.CreateQueries( target, n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateQueries( int target, int n )
    {
        Calls++;
        var result = Gdx.GL.CreateQueries( target, n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateQuery( int target )
    {
        Calls++;
        var result = Gdx.GL.CreateQuery( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void GetQueryBufferObjecti64v( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Gdx.GL.GetQueryBufferObjecti64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectiv( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Gdx.GL.GetQueryBufferObjectiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectui64v( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Gdx.GL.GetQueryBufferObjectui64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectuiv( uint id, uint buffer, int pname, int offset )
    {
        Calls++;
        Gdx.GL.GetQueryBufferObjectuiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrierByRegion( uint barriers )
    {
        Calls++;
        Gdx.GL.MemoryBarrierByRegion( barriers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize )
    {
        Calls++;
        Gdx.GL.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize )
    {
        Calls++;
        Gdx.GL.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize );
        CheckErrors();
    }

    /// <inheritdoc />
    public int GetGraphicsResetStatus()
    {
        Calls++;
        Gdx.GL.GetGraphicsResetStatus();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnCompressedTexImage( int target, int lod, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.GetnCompressedTexImage( target, lod, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnCompressedTexImage( int target, int lod, int bufSize )
    {
        Calls++;
        Gdx.GL.GetnCompressedTexImage( target, lod, bufSize );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnTexImage( int target, int level, int format, int type, int bufSize, void* pixels )
    {
        Calls++;
        Gdx.GL.GetnTexImage( target, level, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnTexImage( int target, int level, int format, int type, int bufSize )
    {
        Calls++;
        Gdx.GL.GetnTexImage( target, level, format, type, bufSize );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformdv( uint program, int location, int bufSize, double* parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformdv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformdv( uint program, int location, int bufSize, ref double[] parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformdv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformfv( uint program, int location, int bufSize, float* parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformfv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformfv( uint program, int location, int bufSize, ref float[] parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformfv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformiv( uint program, int location, int bufSize, int* parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformiv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformiv( uint program, int location, int bufSize, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformuiv( uint program, int location, int bufSize, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformiv( program, location, bufSize, ( int* )parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformuiv( uint program, int location, int bufSize, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetnUniformuiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize, void* data )
    {
        Calls++;
        Gdx.GL.ReadnPixels( x, y, width, height, format, type, bufSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.ReadnPixels( x, y, width, height, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TextureBarrier()
    {
        Calls++;
        Gdx.GL.TextureBarrier();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SpecializeShader( uint shader, byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue )
    {
        Calls++;
        Gdx.GL.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SpecializeShader( uint shader, string pEntryPoint, uint numSpecializationConstants, uint[] pConstantIndex, uint[] pConstantValue )
    {
        Calls++;
        Gdx.GL.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirectCount( int mode, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Gdx.GL.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirectCount( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Gdx.GL.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirectCount( int mode, int type, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Gdx.GL.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirectCount( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Calls++;
        Gdx.GL.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffsetClamp( float factor, float units, float clamp )
    {
        Calls++;
        Gdx.GL.PolygonOffsetClamp( factor, units, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public (int major, int minor) GetOpenGLVersion()
    {
        Calls++;
        var (major, minor) = Gdx.GL.GetOpenGLVersion();
        CheckErrors();

        return ( major, minor );
    }

    /// <inheritdoc />
    public unsafe void MessageCallback( int source, int type, uint id, int severity, int length, string message, void* userParam )
    {
        Calls++;
        Gdx.GL.MessageCallback( source, type, id, severity, length, message, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CullFace( int mode )
    {
        Calls++;
        Gdx.GL.CullFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FrontFace( int mode )
    {
        Calls++;
        Gdx.GL.FrontFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Hint( int target, int mode )
    {
        Calls++;
        Gdx.GL.Hint( target, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LineWidth( float width )
    {
        Calls++;
        Gdx.GL.LineWidth( width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointSize( float size )
    {
        Calls++;
        Gdx.GL.PointSize( size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonMode( int face, int mode )
    {
        Calls++;
        Gdx.GL.PolygonMode( face, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Scissor( int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.Scissor( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterf( int target, int pname, float param )
    {
        Calls++;
        Gdx.GL.TexParameterf( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterfv( int target, int pname, float* parameters )
    {
        Calls++;
        Gdx.GL.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterfv( int target, int pname, float[] parameters )
    {
        Calls++;
        Gdx.GL.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteri( int target, int pname, int param )
    {
        Calls++;
        Gdx.GL.TexParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteriv( int target, int pname, int[] parameters )
    {
        Calls++;
        Gdx.GL.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage1D( int target, int level, int internalformat, int width, int border, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage1D< T >( int target, int level, int internalformat, int width, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2D< T >( int target, int level, int internalformat, int width, int height, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffer( int buf )
    {
        Calls++;
        Gdx.GL.DrawBuffer( buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Clear( uint mask )
    {
        Calls++;
        Gdx.GL.Clear( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        Gdx.GL.ClearColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearStencil( int s )
    {
        Calls++;
        Gdx.GL.ClearStencil( s );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepth( double depth )
    {
        Calls++;
        Gdx.GL.ClearDepth( depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMask( uint mask )
    {
        Calls++;
        Gdx.GL.StencilMask( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Calls++;
        Gdx.GL.ColorMask( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthMask( bool flag )
    {
        Calls++;
        Gdx.GL.DepthMask( flag );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disable( int cap )
    {
        Calls++;
        Gdx.GL.Disable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enable( int cap )
    {
        Calls++;
        Gdx.GL.Enable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Finish()
    {
        Calls++;
        Gdx.GL.Finish();
        CheckErrors();
    }

    /// <inheritdoc />
    public void Flush()
    {
        Calls++;
        Gdx.GL.Flush();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunc( int sfactor, int dfactor )
    {
        Calls++;
        Gdx.GL.BlendFunc( sfactor, dfactor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LogicOp( int opcode )
    {
        Calls++;
        Gdx.GL.LogicOp( opcode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFunc( int func, int @ref, uint mask )
    {
        Calls++;
        Gdx.GL.StencilFunc( func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOp( int fail, int zfail, int zpass )
    {
        Calls++;
        Gdx.GL.StencilOp( fail, zfail, zpass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthFunc( int func )
    {
        Calls++;
        Gdx.GL.DepthFunc( func );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStoref( int pname, float param )
    {
        Calls++;
        Gdx.GL.PixelStoref( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStorei( int pname, int param )
    {
        Calls++;
        Gdx.GL.PixelStorei( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadBuffer( int src )
    {
        Calls++;
        Gdx.GL.ReadBuffer( src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.ReadPixels( x, y, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadPixels< T >( int x, int y, int width, int height, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.ReadPixels( x, y, width, height, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleanv( int pname, bool* data )
    {
        Calls++;
        Gdx.GL.GetBooleanv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleanv( int pname, ref bool[] data )
    {
        Calls++;
        Gdx.GL.GetBooleanv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublev( int pname, double* data )
    {
        Calls++;
        Gdx.GL.GetDoublev( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublev( int pname, ref double[] data )
    {
        Calls++;
        Gdx.GL.GetDoublev( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public int GetError()
    {
        Calls++;
        var err = Gdx.GL.GetError();
        CheckErrors();

        return err;
    }

    /// <inheritdoc />
    public unsafe void GetFloatv( int pname, float* data )
    {
        Calls++;
        Gdx.GL.GetFloatv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloatv( int pname, ref float[] data )
    {
        Calls++;
        Gdx.GL.GetFloatv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegerv( int pname, int* data )
    {
        Calls++;
        Gdx.GL.GetIntegerv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegerv( int pname, ref int[] data )
    {
        Calls++;
        Gdx.GL.GetIntegerv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetString( int name )
    {
        Calls++;
        var str = Gdx.GL.GetString( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public string GetStringSafe( int name )
    {
        Calls++;
        var str = Gdx.GL.GetStringSafe( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public unsafe void GetTexImage( int target, int level, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.GetTexImage( target, level, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexImage< T >( int target, int level, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.GetTexImage( target, level, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterfv( int target, int pname, float* parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterfv( int target, int pname, ref float[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterfv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameterfv( int target, int level, int pname, float* parameters )
    {
        Calls++;
        Gdx.GL.GetTexLevelParameterfv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameterfv( int target, int level, int pname, ref float[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexLevelParameterfv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameteriv( int target, int level, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetTexLevelParameteriv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameteriv( int target, int level, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexLevelParameteriv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabled( int cap )
    {
        Calls++;
        var result = Gdx.GL.IsEnabled( cap );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DepthRange( double near, double far )
    {
        Calls++;
        Gdx.GL.DepthRange( near, far );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Viewport( int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.Viewport( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArrays( int mode, int first, int count )
    {
        Calls++;
        Gdx.GL.DrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElements( int mode, int count, int type, void* indices )
    {
        Calls++;
        Gdx.GL.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElements< T >( int mode, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffset( float factor, float units )
    {
        Calls++;
        Gdx.GL.PolygonOffset( factor, units );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage1D( int target, int level, int internalformat, int x, int y, int width, int border )
    {
        Calls++;
        Gdx.GL.CopyTexImage1D( target, level, internalformat, x, y, width, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Calls++;
        Gdx.GL.CopyTexImage2D( target, level, internalformat, x, y, width, height, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage1D( int target, int level, int xoffset, int x, int y, int width )
    {
        Calls++;
        Gdx.GL.CopyTexSubImage1D( target, level, xoffset, x, y, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage1D( int target, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage1D< T >( int target, int level, int xoffset, int width, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Calls++;
        Gdx.GL.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage2D< T >( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTexture( int target, uint texture )
    {
        Calls++;
        Gdx.GL.BindTexture( target, texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTextures( int n, uint* textures )
    {
        Calls++;
        Gdx.GL.DeleteTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTextures( params uint[] textures )
    {
        Calls++;
        Gdx.GL.DeleteTextures( textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTextures( int n, uint* textures )
    {
        Calls++;
        Gdx.GL.GenTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTextures( int n )
    {
        Calls++;
        var result = Gdx.GL.GenTextures( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTexture()
    {
        Calls++;
        var result = Gdx.GL.GenTexture();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTexture( uint texture )
    {
        Calls++;
        var result = Gdx.GL.IsTexture( texture );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElements( int mode, uint start, uint end, int count, int type, void* indices )
    {
        Calls++;
        Gdx.GL.DrawRangeElements( mode, start, end, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElements< T >( int mode, uint start, uint end, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawRangeElements( mode, start, end, count, type, indices );
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
        Gdx.GL.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
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
        Gdx.GL.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
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
        Gdx.GL.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
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
        Gdx.GL.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Calls++;
        Gdx.GL.CopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveTexture( int texture )
    {
        Calls++;
        Gdx.GL.ActiveTexture( texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleCoverage( float value, bool invert )
    {
        Calls++;
        Gdx.GL.SampleCoverage( value, invert );
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
        Gdx.GL.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, byte[] data )
    {
        Calls++;
        Gdx.GL.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, byte[] data )
    {
        Calls++;
        Gdx.GL.CompressedTexImage2D( target, level, internalformat, width, height, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, byte[] data )
    {
        Calls++;
        Gdx.GL.CompressedTexImage1D( target, level, internalformat, width, border, data );
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
        Gdx.GL.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
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
        Gdx.GL.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data );
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
        Gdx.GL.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, byte[] data )
    {
        Calls++;
        Gdx.GL.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Calls++;
        Gdx.GL.CompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, byte[] data )
    {
        Calls++;
        Gdx.GL.CompressedTexSubImage1D( target, level, xoffset, width, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTexImage( int target, int level, void* img )
    {
        Calls++;
        Gdx.GL.GetCompressedTexImage( target, level, img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetCompressedTexImage( int target, int level, ref byte[] img )
    {
        Calls++;
        Gdx.GL.GetCompressedTexImage( target, level, ref img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparate( int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha )
    {
        Calls++;
        Gdx.GL.BlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArrays( int mode, int* first, int* count, int drawcount )
    {
        Calls++;
        Gdx.GL.MultiDrawArrays( mode, first, count, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArrays( int mode, int[] first, int[] count )
    {
        Calls++;
        Gdx.GL.MultiDrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElements( int mode, int* count, int type, void** indices, int drawcount )
    {
        Calls++;
        Gdx.GL.MultiDrawElements( mode, count, type, indices, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElements< T >( int mode, int[] count, int type, T[][] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.MultiDrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterf( int pname, float param )
    {
        Calls++;
        Gdx.GL.PointParameterf( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameterfv( int pname, float* parameters )
    {
        Calls++;
        Gdx.GL.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterfv( int pname, float[] parameters )
    {
        Calls++;
        Gdx.GL.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteri( int pname, int param )
    {
        Calls++;
        Gdx.GL.PointParameteri( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameteriv( int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteriv( int pname, int[] parameters )
    {
        Calls++;
        Gdx.GL.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendColor( float red, float green, float blue, float alpha )
    {
        Calls++;
        Gdx.GL.BlendColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquation( int mode )
    {
        Calls++;
        Gdx.GL.BlendEquation( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenQueries( int n, uint* ids )
    {
        Calls++;
        Gdx.GL.GenQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenQueries( int n )
    {
        Calls++;
        var result = Gdx.GL.GenQueries( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenQuery()
    {
        Calls++;
        var result = Gdx.GL.GenQuery();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteQueries( int n, uint* ids )
    {
        Calls++;
        Gdx.GL.DeleteQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteQueries( params uint[] ids )
    {
        Calls++;
        Gdx.GL.DeleteQueries( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsQuery( uint id )
    {
        Calls++;
        var result = Gdx.GL.IsQuery( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginQuery( int target, uint id )
    {
        Calls++;
        Gdx.GL.BeginQuery( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQuery( int target )
    {
        Calls++;
        Gdx.GL.EndQuery( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryiv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetQueryiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryiv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetQueryiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectiv( uint id, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetQueryObjectiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectiv( uint id, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetQueryObjectiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectuiv( uint id, int pname, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetQueryObjectuiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectuiv( uint id, int pname, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetQueryObjectuiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffer( int target, uint buffer )
    {
        Calls++;
        Gdx.GL.BindBuffer( target, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteBuffers( int n, uint* buffers )
    {
        Calls++;
        Gdx.GL.DeleteBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteBuffers( params uint[] buffers )
    {
        Calls++;
        Gdx.GL.DeleteBuffers( buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenBuffers( int n, uint* buffers )
    {
        Calls++;
        Gdx.GL.GenBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenBuffers( int n )
    {
        Calls++;
        var result = Gdx.GL.GenBuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenBuffer()
    {
        Calls++;
        var result = Gdx.GL.GenBuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsBuffer( uint buffer )
    {
        Calls++;
        var result = Gdx.GL.IsBuffer( buffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void BufferData( int target, int size, void* data, int usage )
    {
        Calls++;
        Gdx.GL.BufferData( target, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferData< T >( int target, T[] data, int usage )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.BufferData( target, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        Gdx.GL.BufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferSubData< T >( int target, int offsetCount, T[] data )
        where T : unmanaged
    {
        Calls++;
        Gdx.GL.BufferSubData( target, offsetCount, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferSubData( int target, int offset, int size, void* data )
    {
        Calls++;
        Gdx.GL.GetBufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferSubData< T >( int target, int offsetCount, int count, ref T[] data ) where T : unmanaged
    {
        Calls++;
        Gdx.GL.GetBufferSubData( target, offsetCount, count, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBuffer( int target, int access )
    {
        Calls++;
        void* result = Gdx.GL.MapBuffer( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBuffer< T >( int target, int access ) where T : unmanaged
    {
        Calls++;
        Span< T > result = Gdx.GL.MapBuffer< T >( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool UnmapBuffer( int target )
    {
        Calls++;
        var result = Gdx.GL.UnmapBuffer( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetBufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetBufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferPointerv( int target, int pname, void** parameters )
    {
        Calls++;
        Gdx.GL.GetBufferPointerv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferPointerv( int target, int pname, ref IntPtr[] parameters )
    {
        Calls++;
        Gdx.GL.GetBufferPointerv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        Calls++;
        Gdx.GL.BlendEquationSeparate( modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawBuffers( int n, int* bufs )
    {
        Calls++;
        Gdx.GL.DrawBuffers( n, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffers( params int[] bufs )
    {
        Calls++;
        Gdx.GL.DrawBuffers( bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOpSeparate( int face, int sfail, int dpfail, int dppass )
    {
        Calls++;
        Gdx.GL.StencilOpSeparate( face, sfail, dpfail, dppass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFuncSeparate( int face, int func, int @ref, uint mask )
    {
        Calls++;
        Gdx.GL.StencilFuncSeparate( face, func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMaskSeparate( int face, uint mask )
    {
        Calls++;
        Gdx.GL.StencilMaskSeparate( face, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void AttachShader( uint program, uint shader )
    {
        Calls++;
        Gdx.GL.AttachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindAttribLocation( uint program, uint index, byte* name )
    {
        Calls++;
        Gdx.GL.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindAttribLocation( uint program, uint index, string name )
    {
        Calls++;
        Gdx.GL.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompileShader( uint shader )
    {
        Calls++;
        Gdx.GL.CompileShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateProgram()
    {
        Calls++;
        var result = Gdx.GL.CreateProgram();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShader( int type )
    {
        Calls++;
        var result = Gdx.GL.CreateShader( type );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DeleteProgram( uint program )
    {
        Calls++;
        Gdx.GL.DeleteProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteShader( uint shader )
    {
        Calls++;
        Gdx.GL.DeleteShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DetachShader( uint program, uint shader )
    {
        Calls++;
        Gdx.GL.DetachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DisableVertexAttribArray( uint index )
    {
        Calls++;
        Gdx.GL.DisableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexAttribArray( uint index )
    {
        Calls++;
        Gdx.GL.EnableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAttrib( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Gdx.GL.GetActiveAttrib( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveAttrib( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Gdx.GL.GetActiveAttrib( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniform( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Gdx.GL.GetActiveUniform( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniform( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Gdx.GL.GetActiveUniform( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetAttachedShaders( uint program, int maxCount, int* count, uint* shaders )
    {
        Calls++;
        Gdx.GL.GetAttachedShaders( program, maxCount, count, shaders );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetAttachedShaders( uint program, int maxCount )
    {
        Calls++;
        var result = Gdx.GL.GetAttachedShaders( program, maxCount );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetAttribLocation( uint program, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetAttribLocation( uint program, string name )
    {
        Calls++;
        var result = Gdx.GL.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramiv( uint program, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetProgramiv( program, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramiv( uint program, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetProgramiv( program, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInfoLog( uint program, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Gdx.GL.GetProgramInfoLog( program, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramInfoLog( uint program, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.GetProgramInfoLog( program, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderiv( uint shader, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetShaderiv( shader, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderiv( uint shader, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetShaderiv( shader, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderInfoLog( uint shader, int bufSize, int* length, byte* infoLog )
    {
        Calls++;
        Gdx.GL.GetShaderInfoLog( shader, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderInfoLog( uint shader, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.GetShaderInfoLog( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderSource( uint shader, int bufSize, int* length, byte* source )
    {
        Calls++;
        Gdx.GL.GetShaderSource( shader, bufSize, length, source );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderSource( uint shader, int bufSize = 4096 )
    {
        Calls++;
        var result = Gdx.GL.GetShaderSource( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetUniformLocation( uint program, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetUniformLocation( uint program, string name )
    {
        Calls++;
        var result = Gdx.GL.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetUniformfv( uint program, int location, float* parameters )
    {
        Calls++;
        Gdx.GL.GetUniformfv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformfv( uint program, int location, ref float[] parameters )
    {
        Calls++;
        Gdx.GL.GetUniformfv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformiv( uint program, int location, int* parameters )
    {
        Calls++;
        Gdx.GL.GetUniformiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformiv( uint program, int location, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetUniformiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribdv( uint index, int pname, double* parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribdv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribdv( uint index, int pname, ref double[] parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribdv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribfv( uint index, int pname, float* parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribfv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribfv( uint index, int pname, ref float[] parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribfv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribiv( uint index, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribiv( uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribPointerv( uint index, int pname, void** pointer )
    {
        Calls++;
        Gdx.GL.GetVertexAttribPointerv( index, pname, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribPointerv( uint index, int pname, ref uint[] pointer )
    {
        Calls++;
        Gdx.GL.GetVertexAttribPointerv( index, pname, ref pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsProgram( uint program )
    {
        Calls++;
        var result = Gdx.GL.IsProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsShader( uint shader )
    {
        Calls++;
        var result = Gdx.GL.IsShader( shader );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void LinkProgram( uint program )
    {
        Calls++;
        Gdx.GL.LinkProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderSource( uint shader, int count, byte** str, int* length )
    {
        Calls++;
        Gdx.GL.ShaderSource( shader, count, str, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderSource( uint shader, params string[] @string )
    {
        Calls++;
        Gdx.GL.ShaderSource( shader, @string );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgram( uint program )
    {
        Calls++;
        Gdx.GL.UseProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1f( int location, float v0 )
    {
        Calls++;
        Gdx.GL.Uniform1f( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2f( int location, float v0, float v1 )
    {
        Calls++;
        Gdx.GL.Uniform2f( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3f( int location, float v0, float v1, float v2 )
    {
        Calls++;
        Gdx.GL.Uniform3f( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4f( int location, float v0, float v1, float v2, float v3 )
    {
        Calls++;
        Gdx.GL.Uniform4f( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1i( int location, int v0 )
    {
        Calls++;
        Gdx.GL.Uniform1i( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2i( int location, int v0, int v1 )
    {
        Calls++;
        Gdx.GL.Uniform2i( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3i( int location, int v0, int v1, int v2 )
    {
        Calls++;
        Gdx.GL.Uniform3i( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4i( int location, int v0, int v1, int v2, int v3 )
    {
        Calls++;
        Gdx.GL.Uniform4i( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1fv( int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.Uniform1fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1fv( int location, params float[] value )
    {
        Calls++;
        Gdx.GL.Uniform1fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2fv( int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.Uniform2fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2fv( int location, params float[] value )
    {
        Calls++;
        Gdx.GL.Uniform2fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3fv( int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.Uniform3fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3fv( int location, params float[] value )
    {
        Calls++;
        Gdx.GL.Uniform3fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4fv( int location, int count, float* value )
    {
        Calls++;
        Gdx.GL.Uniform4fv( location );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4fv( int location, params float[] value )
    {
        Calls++;
        Gdx.GL.Uniform4fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1iv( int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.Uniform1iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1iv( int location, params int[] value )
    {
        Calls++;
        Gdx.GL.Uniform1iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2iv( int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.Uniform2iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2iv( int location, params int[] value )
    {
        Calls++;
        Gdx.GL.Uniform2iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3iv( int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.Uniform3iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3iv( int location, params int[] value )
    {
        Calls++;
        Gdx.GL.Uniform3iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4iv( int location, int count, int* value )
    {
        Calls++;
        Gdx.GL.Uniform4iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4iv( int location, params int[] value )
    {
        Calls++;
        Gdx.GL.Uniform4iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool ValidateProgram( uint program )
    {
        Calls++;
        var result = Gdx.GL.ValidateProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void VertexAttrib1d( uint index, double x )
    {
        Calls++;
        Gdx.GL.VertexAttrib1d( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1dv( uint index, params double[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1f( uint index, float x )
    {
        Calls++;
        Gdx.GL.VertexAttrib1f( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1fv( uint index, float* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1fv( uint index, params float[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1s( uint index, short x )
    {
        Calls++;
        Gdx.GL.VertexAttrib1s( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1sv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1sv( uint index, params short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2d( uint index, double x, double y )
    {
        Calls++;
        Gdx.GL.VertexAttrib2d( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2dv( uint index, params double[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2f( uint index, float x, float y )
    {
        Calls++;
        Gdx.GL.VertexAttrib2f( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2fv( uint index, float* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2fv( uint index, params float[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2s( uint index, short x, short y )
    {
        Calls++;
        Gdx.GL.VertexAttrib2s( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2sv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2sv( uint index, params short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3d( uint index, double x, double y, double z )
    {
        Calls++;
        Gdx.GL.VertexAttrib3d( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3dv( uint index, params double[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3f( uint index, float x, float y, float z )
    {
        Calls++;
        Gdx.GL.VertexAttrib3f( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3fv( uint index, float* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3fv( uint index, params float[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3s( uint index, short x, short y, short z )
    {
        Calls++;
        Gdx.GL.VertexAttrib3s( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3sv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3sv( uint index, params short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nbv( uint index, sbyte* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nbv( uint index, params sbyte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Niv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Niv( uint index, params int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nsv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nsv( uint index, params short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nub( uint index, byte x, byte y, byte z, byte w )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nub( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nubv( uint index, byte* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nubv( uint index, params byte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nuiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nuiv( uint index, params uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nusv( uint index, ushort* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nusv( uint index, params ushort[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4bv( uint index, sbyte* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4bv( uint index, params sbyte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4d( uint index, double x, double y, double z, double w )
    {
        Calls++;
        Gdx.GL.VertexAttrib4d( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4dv( uint index, double* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4dv( uint index, params double[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4f( uint index, float x, float y, float z, float w )
    {
        Calls++;
        Gdx.GL.VertexAttrib4f( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4fv( uint index, float* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4fv( uint index, params float[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4iv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4iv( uint index, params int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4s( uint index, short x, short y, short z, short w )
    {
        Calls++;
        Gdx.GL.VertexAttrib4s( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4sv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4sv( uint index, params short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4ubv( uint index, byte* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4ubv( uint index, params byte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4ubv( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4uiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4uiv( uint index, params uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4usv( uint index, ushort* v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4usv( uint index, params ushort[] v )
    {
        Calls++;
        Gdx.GL.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, void* pointer )
    {
        Calls++;
        Gdx.GL.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, uint pointer )
    {
        Calls++;
        Gdx.GL.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix2x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix3x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3fv( int location, int count, bool transpose, float* value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3fv( int location, bool transpose, params float[] value )
    {
        Calls++;
        Gdx.GL.UniformMatrix4x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMaski( uint index, bool r, bool g, bool b, bool a )
    {
        Calls++;
        Gdx.GL.ColorMaski( index, r, g, b, a );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleani_v( int target, uint index, bool* data )
    {
        Calls++;
        Gdx.GL.GetBooleani_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleani_v( int target, uint index, ref bool[] data )
    {
        Calls++;
        Gdx.GL.GetBooleani_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegeri_v( int target, uint index, int* data )
    {
        Calls++;
        Gdx.GL.GetIntegeri_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegeri_v( int target, uint index, ref int[] data )
    {
        Calls++;
        Gdx.GL.GetIntegeri_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enablei( int target, uint index )
    {
        Calls++;
        Gdx.GL.Enablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disablei( int target, uint index )
    {
        Calls++;
        Gdx.GL.Disablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabledi( int target, uint index )
    {
        Calls++;
        var result = Gdx.GL.IsEnabledi( target, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginTransformFeedback( int primitiveMode )
    {
        Calls++;
        Gdx.GL.BeginTransformFeedback( primitiveMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndTransformFeedback()
    {
        Calls++;
        Gdx.GL.EndTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferRange( int target, uint index, uint buffer, int offset, int size )
    {
        Calls++;
        Gdx.GL.BindBufferRange( target, index, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferBase( int target, uint index, uint buffer )
    {
        Calls++;
        Gdx.GL.BindBufferBase( target, index, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TransformFeedbackVaryings( uint program, int count, byte** varyings, int bufferMode )
    {
        Calls++;
        Gdx.GL.TransformFeedbackVaryings( program, count, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackVaryings( uint program, string[] varyings, int bufferMode )
    {
        Calls++;
        Gdx.GL.TransformFeedbackVaryings( program, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackVarying( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Calls++;
        Gdx.GL.GetTransformFeedbackVarying( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetTransformFeedbackVarying( uint program, uint index, int bufSize, out int size, out int type )
    {
        Calls++;
        var result = Gdx.GL.GetTransformFeedbackVarying( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public void ClampColor( int target, int clamp )
    {
        Calls++;
        Gdx.GL.ClampColor( target, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginConditionalRender( uint id, int mode )
    {
        Calls++;
        Gdx.GL.BeginConditionalRender( id, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndConditionalRender()
    {
        Calls++;
        Gdx.GL.EndConditionalRender();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribIPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Calls++;
        Gdx.GL.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIPointer( uint index, int size, int type, int stride, uint pointer )
    {
        Calls++;
        Gdx.GL.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIiv( uint index, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribIiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIiv( uint index, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribIiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIuiv( uint index, int pname, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribIuiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIuiv( uint index, int pname, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetVertexAttribIuiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1i( uint index, int x )
    {
        Calls++;
        Gdx.GL.VertexAttribI1i( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2i( uint index, int x, int y )
    {
        Calls++;
        Gdx.GL.VertexAttribI2i( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3i( uint index, int x, int y, int z )
    {
        Calls++;
        Gdx.GL.VertexAttribI3i( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4i( uint index, int x, int y, int z, int w )
    {
        Calls++;
        Gdx.GL.VertexAttribI4i( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1ui( uint index, uint x )
    {
        Calls++;
        Gdx.GL.VertexAttribI1ui( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2ui( uint index, uint x, uint y )
    {
        Calls++;
        Gdx.GL.VertexAttribI2ui( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3ui( uint index, uint x, uint y, uint z )
    {
        Calls++;
        Gdx.GL.VertexAttribI3ui( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ui( uint index, uint x, uint y, uint z, uint w )
    {
        Calls++;
        Gdx.GL.VertexAttribI4ui( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1iv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1iv( uint index, int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2iv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2iv( uint index, int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3iv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3iv( uint index, int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4iv( uint index, int* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4iv( uint index, int[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1uiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1uiv( uint index, uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2uiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2uiv( uint index, uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3uiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3uiv( uint index, uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4uiv( uint index, uint* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4uiv( uint index, uint[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4bv( uint index, sbyte* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4bv( uint index, sbyte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4sv( uint index, short* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4sv( uint index, short[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4ubv( uint index, byte* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ubv( uint index, byte[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4usv( uint index, ushort* v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4usv( uint index, ushort[] v )
    {
        Calls++;
        Gdx.GL.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformuiv( uint program, int location, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetUniformuiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformuiv( uint program, int location, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetUniformuiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocation( uint program, uint color, byte* name )
    {
        Calls++;
        Gdx.GL.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocation( uint program, uint color, string name )
    {
        Calls++;
        Gdx.GL.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataLocation( uint program, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataLocation( uint program, string name )
    {
        Calls++;
        var result = Gdx.GL.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void Uniform1ui( int location, uint v0 )
    {
        Calls++;
        Gdx.GL.Uniform1ui( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2ui( int location, uint v0, uint v1 )
    {
        Calls++;
        Gdx.GL.Uniform2ui( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3ui( int location, uint v0, uint v1, uint v2 )
    {
        Calls++;
        Gdx.GL.Uniform3ui( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4ui( int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Calls++;
        Gdx.GL.Uniform4ui( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1uiv( int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.Uniform1uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1uiv( int location, uint[] value )
    {
        Calls++;
        Gdx.GL.Uniform1uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2uiv( int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.Uniform2uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2uiv( int location, uint[] value )
    {
        Calls++;
        Gdx.GL.Uniform2uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3uiv( int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.Uniform3uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3uiv( int location, uint[] value )
    {
        Calls++;
        Gdx.GL.Uniform3uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4uiv( int location, int count, uint* value )
    {
        Calls++;
        Gdx.GL.Uniform4uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4uiv( int location, uint[] value )
    {
        Calls++;
        Gdx.GL.Uniform4uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIiv( int target, int pname, int* param )
    {
        Calls++;
        Gdx.GL.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIiv( int target, int pname, int[] param )
    {
        Calls++;
        Gdx.GL.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIuiv( int target, int pname, uint* param )
    {
        Calls++;
        Gdx.GL.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIuiv( int target, int pname, uint[] param )
    {
        Calls++;
        Gdx.GL.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIiv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterIiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIiv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterIiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIuiv( int target, int pname, uint* parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterIuiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIuiv( int target, int pname, ref uint[] parameters )
    {
        Calls++;
        Gdx.GL.GetTexParameterIuiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferiv( int buffer, int drawbuffer, int* value )
    {
        Calls++;
        Gdx.GL.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferiv( int buffer, int drawbuffer, int[] value )
    {
        Calls++;
        Gdx.GL.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferuiv( int buffer, int drawbuffer, uint* value )
    {
        Calls++;
        Gdx.GL.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferuiv( int buffer, int drawbuffer, uint[] value )
    {
        Calls++;
        Gdx.GL.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferfv( int buffer, int drawbuffer, float* value )
    {
        Calls++;
        Gdx.GL.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfv( int buffer, int drawbuffer, float[] value )
    {
        Calls++;
        Gdx.GL.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
        Calls++;
        Gdx.GL.ClearBufferfi( buffer, drawbuffer, depth, stencil );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetStringi( int name, uint index )
    {
        Calls++;
        var result = Gdx.GL.GetStringi( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public string GetStringiSafe( int name, uint index )
    {
        Calls++;
        var result = Gdx.GL.GetStringiSafe( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsRenderbuffer( uint renderbuffer )
    {
        Calls++;
        var result = Gdx.GL.IsRenderbuffer( renderbuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindRenderbuffer( int target, uint renderbuffer )
    {
        Calls++;
        Gdx.GL.BindRenderbuffer( target, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Gdx.GL.DeleteRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteRenderbuffers( params uint[] renderbuffers )
    {
        Calls++;
        Gdx.GL.DeleteRenderbuffers( renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenRenderbuffers( int n, uint* renderbuffers )
    {
        Calls++;
        Gdx.GL.GenRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenRenderbuffers( int n )
    {
        Calls++;
        var result = Gdx.GL.GenRenderbuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenRenderbuffer()
    {
        Calls++;
        var result = Gdx.GL.GenRenderbuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void RenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.RenderbufferStorage( target, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetRenderbufferParameteriv( int target, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetRenderbufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetRenderbufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetRenderbufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsFramebuffer( uint framebuffer )
    {
        Calls++;
        var result = Gdx.GL.IsFramebuffer( framebuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindFramebuffer( int target, uint framebuffer )
    {
        Calls++;
        Gdx.GL.BindFramebuffer( target, framebuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Gdx.GL.DeleteFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteFramebuffers( params uint[] framebuffers )
    {
        Calls++;
        Gdx.GL.DeleteFramebuffers( framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenFramebuffers( int n, uint* framebuffers )
    {
        Calls++;
        Gdx.GL.GenFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenFramebuffers( int n )
    {
        Calls++;
        var result = Gdx.GL.GenFramebuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenFramebuffer()
    {
        Calls++;
        var result = Gdx.GL.GenFramebuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int CheckFramebufferStatus( int target )
    {
        Calls++;
        var result = Gdx.GL.CheckFramebufferStatus( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FramebufferTexture1D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        Gdx.GL.FramebufferTexture1D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level )
    {
        Calls++;
        Gdx.GL.FramebufferTexture2D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture3D( int target, int attachment, int textarget, uint texture, int level, int zoffset )
    {
        Calls++;
        Gdx.GL.FramebufferTexture3D( target, attachment, textarget, texture, level, zoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Calls++;
        Gdx.GL.FramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetFramebufferAttachmentParameteriv( target, attachment, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateMipmap( int target )
    {
        Calls++;
        Gdx.GL.GenerateMipmap( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Calls++;
        Gdx.GL.BlitFramebuffer( srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        CheckErrors();
    }

    /// <inheritdoc />
    public void RenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
        Calls++;
        Gdx.GL.RenderbufferStorageMultisample( target, samples, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTextureLayer( int target, int attachment, uint texture, int level, int layer )
    {
        Calls++;
        Gdx.GL.FramebufferTextureLayer( target, attachment, texture, level, layer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBufferRange( int target, int offset, int length, uint access )
    {
        Calls++;
        void* result = Gdx.GL.MapBufferRange( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBufferRange< T >( int target, int offset, int length, uint access )
        where T : unmanaged
    {
        Calls++;
        Span< T > result = Gdx.GL.MapBufferRange< T >( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FlushMappedBufferRange( int target, int offset, int length )
    {
        Calls++;
        Gdx.GL.FlushMappedBufferRange( target, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexArray( uint array )
    {
        Calls++;
        Gdx.GL.BindVertexArray( array );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Gdx.GL.DeleteVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteVertexArrays( params uint[] arrays )
    {
        Calls++;
        Gdx.GL.DeleteVertexArrays( arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenVertexArrays( int n, uint* arrays )
    {
        Calls++;
        Gdx.GL.GenVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenVertexArrays( int n )
    {
        Calls++;
        var result = Gdx.GL.GenVertexArrays( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenVertexArray()
    {
        Calls++;
        var result = Gdx.GL.GenVertexArray();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsVertexArray( uint array )
    {
        Calls++;
        var result = Gdx.GL.IsVertexArray( array );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DrawArraysInstanced( int mode, int first, int count, int instancecount )
    {
        Calls++;
        Gdx.GL.DrawArraysInstanced( mode, first, count, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstanced( int mode, int count, int type, void* indices, int instancecount )
    {
        Calls++;
        Gdx.GL.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstanced< T >( int mode, int count, int type, T[] indices, int instancecount )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexBuffer( int target, int internalformat, uint buffer )
    {
        Calls++;
        Gdx.GL.TexBuffer( target, internalformat, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PrimitiveRestartIndex( uint index )
    {
        Calls++;
        Gdx.GL.PrimitiveRestartIndex( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
        Calls++;
        Gdx.GL.CopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformIndices( uint program, int uniformCount, byte** uniformNames, uint* uniformIndices )
    {
        Calls++;
        Gdx.GL.GetUniformIndices( program, uniformCount, uniformNames, uniformIndices );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetUniformIndices( uint program, params string[] uniformNames )
    {
        Calls++;
        var result = Gdx.GL.GetUniformIndices( program, uniformNames );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformsiv( uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetActiveUniformsiv( uint program, int pname, params uint[] uniformIndices )
    {
        Calls++;
        var result = Gdx.GL.GetActiveUniformsiv( program, pname, uniformIndices );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformName( uint program, uint uniformIndex, int bufSize, int* length, byte* uniformName )
    {
        Calls++;
        Gdx.GL.GetActiveUniformName( program, uniformIndex, bufSize, length, uniformName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformName( uint program, uint uniformIndex, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.GetActiveUniformName( program, uniformIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetUniformBlockIndex( uint program, byte* uniformBlockName )
    {
        Calls++;
        var result = Gdx.GL.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetUniformBlockIndex( uint program, string uniformBlockName )
    {
        Calls++;
        var result = Gdx.GL.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, int* parameters )
    {
        Calls++;
        Gdx.GL.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, ref int[] parameters )
    {
        Calls++;
        Gdx.GL.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName )
    {
        Calls++;
        Gdx.GL.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize, length, uniformBlockName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void UniformBlockBinding( uint program, uint uniformBlockIndex, uint uniformBlockBinding )
    {
        Calls++;
        Gdx.GL.UniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsBaseVertex( int mode, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        Gdx.GL.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsBaseVertex< T >( int mode, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElementsBaseVertex( int mode, uint start, uint end, int count, int type, void* indices, int basevertex )
    {
        Calls++;
        Gdx.GL.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElementsBaseVertex< T >( int mode, uint start, uint end, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertex( int mode, int count, int type, void* indices, int instancecount, int basevertex )
    {
        Calls++;
        Gdx.GL.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertex< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsBaseVertex( int mode, int* count, int type, void** indices, int drawcount, int* basevertex )
    {
        Calls++;
        Gdx.GL.MultiDrawElementsBaseVertex( mode, count, type, indices, drawcount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsBaseVertex< T >( int mode, int type, T[][] indices, int[] basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Calls++;
        Gdx.GL.MultiDrawElementsBaseVertex( mode, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProvokingVertex( int mode )
    {
        Calls++;
        Gdx.GL.ProvokingVertex( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* FenceSync( int condition, uint flags )
    {
        Calls++;
        void* result = Gdx.GL.FenceSync( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public IntPtr FenceSyncSafe( int condition, uint flags )
    {
        Calls++;
        var result = Gdx.GL.FenceSyncSafe( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe bool IsSync( void* sync )
    {
        Calls++;
        var result = Gdx.GL.IsSync( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsSyncSafe( IntPtr sync )
    {
        Calls++;
        var result = Gdx.GL.IsSyncSafe( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSync( void* sync )
    {
        Calls++;
        Gdx.GL.DeleteSync( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSyncSafe( IntPtr sync )
    {
        Calls++;
        Gdx.GL.DeleteSyncSafe( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int ClientWaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        var result = Gdx.GL.ClientWaitSync( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int ClientWaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        var result = Gdx.GL.ClientWaitSyncSafe( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void WaitSync( void* sync, uint flags, ulong timeout )
    {
        Calls++;
        Gdx.GL.WaitSync( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public void WaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Calls++;
        Gdx.GL.WaitSyncSafe( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInteger64v( int pname, long* data )
    {
        Calls++;
        Gdx.GL.GetInteger64v( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64v( int pname, ref long[] data )
    {
        Calls++;
        Gdx.GL.GetInteger64v( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSynciv( void* sync, int pname, int bufSize, int* length, int* values )
    {
        Calls++;
        Gdx.GL.GetSynciv( sync, pname, bufSize, length, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetSynciv( IntPtr sync, int pname, int bufSize )
    {
        Calls++;
        var result = Gdx.GL.GetSynciv( sync, pname, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetInteger64i_v( int target, uint index, long* data )
    {
        Calls++;
        Gdx.GL.GetInteger64i_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64i_v( int target, uint index, ref long[] data )
    {
        Calls++;
        Gdx.GL.GetInteger64i_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteri64v( int target, int pname, long* parameters )
    {
        Calls++;
        Gdx.GL.GetBufferParameteri64v( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteri64v( int target, int pname, ref long[] parameters )
    {
        Calls++;
        Gdx.GL.GetBufferParameteri64v( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture( int target, int attachment, uint texture, int level )
    {
        Calls++;
        Gdx.GL.FramebufferTexture( target, attachment, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.TexImage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Calls++;
        Gdx.GL.TexImage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetMultisamplefv( int pname, uint index, float* val )
    {
        Calls++;
        Gdx.GL.GetMultisamplefv( pname, index, val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetMultisamplefvSafe( int pname, uint index, ref float[] val )
    {
        Calls++;
        Gdx.GL.GetMultisamplefvSafe( pname, index, ref val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleMaski( uint maskNumber, uint mask )
    {
        Calls++;
        Gdx.GL.SampleMaski( maskNumber, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, byte* name )
    {
        Calls++;
        Gdx.GL.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, string name )
    {
        Calls++;
        Gdx.GL.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataIndex( uint program, byte* name )
    {
        Calls++;
        var result = Gdx.GL.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataIndex( uint program, string name )
    {
        Calls++;
        var result = Gdx.GL.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GenSamplers( int count, uint* samplers )
    {
        Calls++;
        Gdx.GL.GenSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenSamplers( int count )
    {
        Calls++;
        var result = Gdx.GL.GenSamplers( count );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenSampler()
    {
        Calls++;
        var result = Gdx.GL.GenSampler();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSamplers( int count, uint* samplers )
    {
        Calls++;
        Gdx.GL.DeleteSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSamplers( params uint[] samplers )
    {
        Calls++;
        Gdx.GL.DeleteSamplers( samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsSampler( uint sampler )
    {
        Calls++;
        var result = Gdx.GL.IsSampler( sampler );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindSampler( uint unit, uint sampler )
    {
        Calls++;
        Gdx.GL.BindSampler( unit, sampler );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteri( uint sampler, int pname, int param )
    {
        Calls++;
        Gdx.GL.SamplerParameteri( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        Gdx.GL.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteriv( uint sampler, int pname, int[] param )
    {
        Calls++;
        Gdx.GL.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterf( uint sampler, int pname, float param )
    {
        Calls++;
        Gdx.GL.SamplerParameterf( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterfv( uint sampler, int pname, float[] param )
    {
        Calls++;
        Gdx.GL.SamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIiv( uint sampler, int pname, int[] param )
    {
        Calls++;
        Gdx.GL.SamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIuiv( uint sampler, int pname, uint[] param )
    {
        Calls++;
        Gdx.GL.SamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameteriv( uint sampler, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameteriv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameteriv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIiv( uint sampler, int pname, ref int[] param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterfv( uint sampler, int pname, float* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterfv( uint sampler, int pname, ref float[] param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterfv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIuiv( uint sampler, int pname, ref uint[] param )
    {
        Calls++;
        Gdx.GL.GetSamplerParameterIuiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void QueryCounter( uint id, int target )
    {
        Calls++;
        Gdx.GL.QueryCounter( id, target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjecti64v( uint id, int pname, long* param )
    {
        Calls++;
        Gdx.GL.GetQueryObjecti64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjecti64v( uint id, int pname, ref long[] param )
    {
        Calls++;
        Gdx.GL.GetQueryObjecti64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectui64v( uint id, int pname, ulong* param )
    {
        Calls++;
        Gdx.GL.GetQueryObjectui64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectui64v( uint id, int pname, ref ulong[] param )
    {
        Calls++;
        Gdx.GL.GetQueryObjectui64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribDivisor( uint index, uint divisor )
    {
        Calls++;
        Gdx.GL.VertexAttribDivisor( index, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Gdx.GL.VertexAttribP1ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP1uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Gdx.GL.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Gdx.GL.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Gdx.GL.VertexAttribP2ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP2uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Gdx.GL.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Gdx.GL.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Gdx.GL.VertexAttribP3ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP3uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Gdx.GL.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Gdx.GL.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4ui( uint index, int type, bool normalized, uint value )
    {
        Calls++;
        Gdx.GL.VertexAttribP4ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP4uiv( uint index, int type, bool normalized, uint* value )
    {
        Calls++;
        Gdx.GL.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4uiv( uint index, int type, bool normalized, uint[] value )
    {
        Calls++;
        Gdx.GL.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Import( IGLBindings.GetProcAddressHandler loader )
    {
        Calls++;
        Gdx.GL.Import( loader );
        CheckErrors();
    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public unsafe string GetActiveUniform( int handle, int u, int* bufSize, Buffer buffer )
//    {
//        Calls++;
//        var result = Gdx.GL.GetActiveUniform( handle, u, bufSize, buffer );
//        CheckErrors();
//
//        return result;
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void VertexAttribPointer( int location, int size, int type, bool normalized, int stride, Buffer buffer )
//    {
//        Calls++;
//        Gdx.GL.VertexAttribPointer( location, size, type, normalized, stride, buffer );
//        CheckErrors();
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void UniformMatrix4fv( int location, int count, bool transpose, Buffer buffer )
//    {
//        Calls++;
//        Gdx.GL.UniformMatrix4fv( location, count, transpose, buffer );
//        CheckErrors();
//    }

    public void Import()
    {
    }
}