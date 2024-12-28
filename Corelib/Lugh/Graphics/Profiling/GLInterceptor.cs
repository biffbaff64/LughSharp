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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MinSampleShading( value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationi( uint buf, int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendEquationi( buf, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparatei( uint buf, int modeRGB, int modeAlpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendEquationSeparatei( buf, modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunci( uint buf, int src, int dst )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendFunci( buf, src, dst );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparatei( uint buf, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendFuncSeparatei( buf, srcRGB, dstRGB, srcAlpha, dstAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawArraysIndirect( int mode, void* indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawArraysIndirect( mode, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsIndirect( int mode, int type, void* indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsIndirect( mode, type, indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1d( int location, double x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1d( location, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2d( int location, double x, double y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2d( location, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3d( int location, double x, double y, double z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3d( location, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4d( int location, double x, double y, double z, double w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4d( location, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1dv( int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1dv( int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2dv( int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2dv( int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3dv( int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3dv( int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4dv( int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4dv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4dv( int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4dv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x4dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x4dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x2dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x2dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3dv( int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x3dv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3dv( int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x3dv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformdv( uint program, int location, double* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformdv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformdv( uint program, int location, ref double[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformdv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetSubroutineUniformLocation( uint program, int shadertype, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetSubroutineUniformLocation( uint program, int shadertype, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetSubroutineUniformLocation( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetSubroutineIndex( uint program, int shadertype, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetSubroutineIndex( uint program, int shadertype, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetSubroutineIndex( program, shadertype, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, int* values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveSubroutineUniformiv( program, shadertype, index, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveSubroutineUniformiv( uint program, int shadertype, uint index, int pname, ref int[] values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveSubroutineUniformiv( program, shadertype, index, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveSubroutineUniformName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineUniformName( uint program, int shadertype, uint index, int bufsize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveSubroutineUniformName( program, shadertype, index, bufsize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize, int* length, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveSubroutineName( program, shadertype, index, bufsize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveSubroutineName( uint program, int shadertype, uint index, int bufsize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveSubroutineName( program, shadertype, index, bufsize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void UniformSubroutinesuiv( int shadertype, int count, uint* indices )
    {
        Logger.Checkpoint();
        Calls++;
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformSubroutinesuiv( int shadertype, uint[] indices )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformSubroutinesuiv( shadertype, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformSubroutineuiv( int shadertype, int location, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformSubroutineuiv( shadertype, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformSubroutineuiv( int shadertype, int location, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformSubroutineuiv( shadertype, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramStageiv( uint program, int shadertype, int pname, int* values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramStageiv( program, shadertype, pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramStageiv( uint program, int shadertype, int pname, ref int[] values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramStageiv( program, shadertype, pname, ref values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameteri( int pname, int value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PatchParameteri( pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PatchParameterfv( int pname, float* values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PatchParameterfv( int pname, float[] values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PatchParameterfv( pname, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTransformFeedback( int target, uint id )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindTransformFeedback( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTransformFeedbacks( int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTransformFeedbacks( params uint[] ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteTransformFeedbacks( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTransformFeedbacks( int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTransformFeedbacks( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenTransformFeedbacks( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTransformFeedback()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenTransformFeedback();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTransformFeedback( uint id )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsTransformFeedback( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void PauseTransformFeedback()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PauseTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void ResumeTransformFeedback()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ResumeTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedback( int mode, uint id )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawTransformFeedback( mode, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStream( int mode, uint id, uint stream )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawTransformFeedbackStream( mode, id, stream );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginQueryIndexed( int target, uint index, uint id )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BeginQueryIndexed( target, index, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQueryIndexed( int target, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EndQueryIndexed( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryIndexediv( int target, uint index, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryIndexediv( target, index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryIndexediv( int target, uint index, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryIndexediv( target, index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReleaseShaderCompiler()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ReleaseShaderCompiler();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderBinary( int count, uint* shaders, int binaryformat, void* binary, int length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ShaderBinary( count, shaders, binaryformat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderBinary( uint[] shaders, int binaryformat, byte[] binary )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ShaderBinary( shaders, binaryformat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderPrecisionFormat( int shaderType, int precisionType, int* range, int* precision )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderPrecisionFormat( shaderType, precisionType, range, precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderPrecisionFormat( int shaderType, int precisionType, ref int[] range, ref int precision )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderPrecisionFormat( shaderType, precisionType, ref range, ref precision );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangef( float n, float f )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthRangef( n, f );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepthf( float d )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearDepthf( d );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramBinary( uint program, int bufSize, int* length, int* binaryFormat, void* binary )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramBinary( program, bufSize, length, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetProgramBinary( uint program, int bufSize, out int binaryFormat )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramBinary( program, bufSize, out binaryFormat );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void ProgramBinary( uint program, int binaryFormat, void* binary, int length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramBinary( program, binaryFormat, binary, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramBinary( uint program, int binaryFormat, byte[] binary )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramBinary( program, binaryFormat, binary );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramParameteri( uint program, int pname, int value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramParameteri( program, pname, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgramStages( uint pipeline, uint stages, uint program )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UseProgramStages( pipeline, stages, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveShaderProgram( uint pipeline, uint program )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ActiveShaderProgram( pipeline, program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint CreateShaderProgramv( int type, int count, byte** strings )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateShaderProgramv( type, count, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShaderProgramv( int type, string[] strings )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateShaderProgramv( type, strings );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindProgramPipeline( uint pipeline )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindProgramPipeline( pipeline );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteProgramPipelines( int n, uint* pipelines )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteProgramPipelines( params uint[] pipelines )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteProgramPipelines( pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenProgramPipelines( int n, uint* pipelines )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenProgramPipelines( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenProgramPipeline()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsProgramPipeline( uint pipeline )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsProgramPipeline( pipeline );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineiv( uint pipeline, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramPipelineiv( pipeline, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramPipelineiv( uint pipeline, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramPipelineiv( pipeline, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1i( uint program, int location, int v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1i( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1iv( uint program, int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1iv( uint program, int location, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1f( uint program, int location, float v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1f( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1fv( uint program, int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1fv( uint program, int location, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1d( uint program, int location, double v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1d( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1dv( uint program, int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1dv( uint program, int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1ui( uint program, int location, uint v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1ui( program, location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform1uiv( uint program, int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform1uiv( uint program, int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform1uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2i( uint program, int location, int v0, int v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2i( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2iv( uint program, int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2iv( uint program, int location, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2f( uint program, int location, float v0, float v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2f( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2fv( uint program, int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2fv( uint program, int location, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2d( uint program, int location, double v0, double v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2d( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2dv( uint program, int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2dv( uint program, int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2ui( uint program, int location, uint v0, uint v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2ui( program, location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform2uiv( uint program, int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform2uiv( uint program, int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform2uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3i( uint program, int location, int v0, int v1, int v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3i( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3iv( uint program, int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3iv( uint program, int location, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3f( uint program, int location, float v0, float v1, float v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3f( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3fv( uint program, int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3fv( uint program, int location, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3d( uint program, int location, double v0, double v1, double v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3d( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3dv( uint program, int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3dv( uint program, int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3ui( uint program, int location, uint v0, uint v1, uint v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3ui( program, location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform3uiv( uint program, int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform3uiv( uint program, int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform3uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4i( uint program, int location, int v0, int v1, int v2, int v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4i( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4iv( uint program, int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4iv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4iv( uint program, int location, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4iv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4f( uint program, int location, float v0, float v1, float v2, float v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4f( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4fv( uint program, int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4fv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4fv( uint program, int location, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4fv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4d( uint program, int location, double v0, double v1, double v2, double v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4d( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4dv( uint program, int location, int count, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4dv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4dv( uint program, int location, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4dv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4ui( uint program, int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4ui( program, location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniform4uiv( uint program, int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4uiv( program, location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniform4uiv( uint program, int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniform4uiv( program, location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x2fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x2fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x4fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x4fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3fv( uint program, int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x3fv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3fv( uint program, int location, bool transpose, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x3fv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x3dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x2dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix2x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix2x4dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix2x4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x2dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x2dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x2dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x2dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix3x4dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x4dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix3x4dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix3x4dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ProgramUniformMatrix4x3dv( uint program, int location, int count, bool transpose, double* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x3dv( program, location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProgramUniformMatrix4x3dv( uint program, int location, bool transpose, double[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProgramUniformMatrix4x3dv( program, location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ValidateProgramPipeline( uint pipeline )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ValidateProgramPipeline( pipeline );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramPipelineInfoLog( uint pipeline, int bufSize, int* length, byte* infoLog )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramPipelineInfoLog( pipeline, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramPipelineInfoLog( uint pipeline, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramPipelineInfoLog( pipeline, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void VertexAttribL1d( uint index, double x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL1d( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2d( uint index, double x, double y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL2d( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3d( uint index, double x, double y, double z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL3d( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4d( uint index, double x, double y, double z, double w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL4d( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL1dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL1dv( uint index, double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL2dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL2dv( uint index, double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL3dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL3dv( uint index, double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribL4dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribL4dv( uint index, double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribL4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribLPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribLPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLPointer( uint index, int size, int type, int stride, int pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribLPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribLdv( uint index, int pname, double* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribLdv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribLdv( uint index, int pname, ref double[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribLdv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportArrayv( uint first, int count, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ViewportArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportArrayv( uint first, int count, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ViewportArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedf( uint index, float x, float y, float w, float h )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ViewportIndexedf( index, x, y, w, h );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ViewportIndexedfv( uint index, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ViewportIndexedfv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ViewportIndexedfv( uint index, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ViewportIndexedfv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorArrayv( uint first, int count, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ScissorArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorArrayv( uint first, int count, params int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ScissorArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexed( uint index, int left, int bottom, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ScissorIndexed( index, left, bottom, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ScissorIndexedv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ScissorIndexedv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ScissorIndexedv( uint index, params int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ScissorIndexedv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DepthRangeArrayv( uint first, int count, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthRangeArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeArrayv( uint first, int count, params double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthRangeArrayv( first, count, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthRangeIndexed( uint index, double n, double f )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthRangeIndexed( index, n, f );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFloati_v( int target, uint index, float* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFloati_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloati_v( int target, uint index, ref float[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFloati_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublei_v( int target, uint index, double* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetDoublei_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublei_v( int target, uint index, ref double[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetDoublei_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArraysInstancedBaseInstance( int mode, int first, int count, int instancecount, uint baseinstance )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawArraysInstancedBaseInstance( mode, first, count, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseInstance( int mode, int count, int type, void* indices, int instancecount, uint baseinstance )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, uint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseInstance( mode, count, type, indices, instancecount, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertexBaseInstance( int mode, int count, int type, void* indices, int instancecount, int basevertex, uint baseinstance )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertexBaseInstance< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex, uint baseinstance )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseVertexBaseInstance( mode, count, type, indices, instancecount, basevertex, baseinstance );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformativ( int target, int internalformat, int pname, int bufSize, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInternalformativ( target, internalformat, pname, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformativ( int target, int internalformat, int pname, int bufSize, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInternalformativ( target, internalformat, pname, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveAtomicCounterBufferiv( program, bufferIndex, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveAtomicCounterBufferiv( uint program, uint bufferIndex, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveAtomicCounterBufferiv( program, bufferIndex, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTexture( uint unit, uint texture, int level, bool layered, int layer, int access, int format )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindImageTexture( unit, texture, level, layered, layer, access, format );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrier( uint barriers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MemoryBarrier( barriers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage1D( int target, int levels, int internalformat, int width )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexStorage1D( target, levels, internalformat, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2D( int target, int levels, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexStorage2D( target, levels, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3D( int target, int levels, int internalformat, int width, int height, int depth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexStorage3D( target, levels, internalformat, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackInstanced( int mode, uint id, int instancecount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawTransformFeedbackInstanced( mode, id, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawTransformFeedbackStreamInstanced( int mode, uint id, uint stream, int instancecount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawTransformFeedbackStreamInstanced( mode, id, stream, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetPointerv( int pname, void** parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetPointerv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetPointerv( int pname, ref IntPtr[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetPointerv( pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferData( int target, int internalformat, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferData( target, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferData< T >( int target, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferData( target, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferSubData( int target, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferSubData( target, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferSubData< T >( int target, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferSubData( target, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchCompute( uint numGroupsX, uint numGroupsY, uint numGroupsZ )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DispatchCompute( numGroupsX, numGroupsY, numGroupsZ );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DispatchComputeIndirect( void* indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DispatchComputeIndirect( indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DispatchComputeIndirect( GLBindings.DispatchIndirectCommand indirect )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DispatchComputeIndirect( indirect );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyImageSubData( uint srcName, int srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, int dstTarget, int dstLevel, int dstX, int dstY, int dstZ, int srcWidth, int srcHeight, int srcDepth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyImageSubData( srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferParameteri( int target, int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferParameteriv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFramebufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFramebufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInternalformati64v( int target, int internalformat, int pname, int count, long* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInternalformati64v( target, internalformat, pname, count, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInternalformati64v( int target, int internalformat, int pname, int count, ref long[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInternalformati64v( target, internalformat, pname, count, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateTexSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateTexImage( uint texture, int level )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateTexImage( texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferSubData( uint buffer, int offset, int length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateBufferSubData( buffer, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateBufferData( uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateBufferData( buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateFramebuffer( int target, int numAttachments, int* attachments )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateFramebuffer( target, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateFramebuffer( int target, int numAttachments, int[] attachments )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateFramebuffer( target, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateSubFramebuffer( int target, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateSubFramebuffer( int target, int numAttachments, int[] attachments, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateSubFramebuffer( target, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirect( int mode, void* indirect, int drawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArraysIndirect( mode, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirect( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArraysIndirect( mode, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirect( int mode, int type, void* indirect, int drawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirect( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsIndirect( mode, type, indirect, drawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInterfaceiv( uint program, int programInterface, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramInterfaceiv( program, programInterface, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramInterfaceiv( uint program, int programInterface, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramInterfaceiv( program, programInterface, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetProgramResourceIndex( uint program, int programInterface, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetProgramResourceIndex( uint program, int programInterface, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceName( uint program, int programInterface, uint index, int bufSize, int* length, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramResourceName( program, programInterface, index, bufSize, length, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramResourceName( uint program, int programInterface, uint index, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceName( program, programInterface, index, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramResourceiv( uint program, int programInterface, uint index, int propCount, int* props, int bufSize, int* length, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramResourceiv( program, programInterface, index, propCount, props, bufSize, length, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramResourceiv( uint program, int programInterface, uint index, int[] props, int bufSize, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramResourceiv( program, programInterface, index, props, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocation( uint program, int programInterface, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceLocation( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetProgramResourceLocation( uint program, int programInterface, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceLocation( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetProgramResourceLocationIndex( uint program, int programInterface, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceLocationIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetProgramResourceLocationIndex( uint program, int programInterface, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramResourceLocationIndex( program, programInterface, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void ShaderStorageBlockBinding( uint program, uint storageBlockIndex, uint storageBlockBinding )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ShaderStorageBlockBinding( program, storageBlockIndex, storageBlockBinding );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexBufferRange( int target, int internalformat, uint buffer, int offset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexBufferRange( target, internalformat, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexStorage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexStorage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexStorage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureView( uint texture, int target, uint origtexture, int internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureView( texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffer( uint bindingindex, uint buffer, int offset, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindVertexBuffer( bindingindex, buffer, offset, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribFormat( uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribFormat( attribindex, size, type, normalized, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribIFormat( attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribLFormat( uint attribindex, int size, int type, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribLFormat( attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribBinding( uint attribindex, uint bindingindex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribBinding( attribindex, bindingindex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexBindingDivisor( uint bindingindex, uint divisor )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexBindingDivisor( bindingindex, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageControl( int source, int type, int severity, int count, uint* ids, bool enabled )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageControl( source, type, severity, count, ids, enabled );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageControl( int source, int type, int severity, uint[] ids, bool enabled )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageControl( source, type, severity, ids, enabled );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageInsert( int source, int type, uint id, int severity, int length, byte* buf )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageInsert( source, type, id, severity, length, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DebugMessageInsert( int source, int type, uint id, int severity, string buf )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageInsert( source, type, id, severity, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROC callback, void* userParam )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageCallback( callback, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DebugMessageCallback( GLBindings.GLDEBUGPROCSAFE callback, void* userParam )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DebugMessageCallback( callback, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe uint GetDebugMessageLog( uint count, int bufsize, int* sources, int* types, uint* ids, int* severities, int* lengths, byte* messageLog )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetDebugMessageLog( count, bufsize, sources, types, ids, severities, lengths, messageLog );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetDebugMessageLog( uint count, int bufSize, out int[] sources, out int[] types, out uint[] ids, out int[] severities, out string[] messageLog )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetDebugMessageLog( count, bufSize, out sources, out types, out ids, out severities, out messageLog );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void PushDebugGroup( int source, uint id, int length, byte* message )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PushDebugGroup( source, id, length, message );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PushDebugGroup( int source, uint id, string message )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PushDebugGroup( source, id, message );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PopDebugGroup()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PopDebugGroup();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ObjectLabel( int identifier, uint name, int length, byte* label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ObjectLabel( identifier, name, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectLabel( int identifier, uint name, string label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ObjectLabel( identifier, name, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectLabel( int identifier, uint name, int bufSize, int* length, byte* label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetObjectLabel( identifier, name, bufSize, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectLabel( int identifier, uint name, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetObjectLabel( identifier, name, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void ObjectPtrLabel( void* ptr, int length, byte* label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ObjectPtrLabel( ptr, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ObjectPtrLabel( IntPtr ptr, string label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ObjectPtrLabel( ptr, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetObjectPtrLabel( void* ptr, int bufSize, int* length, byte* label )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetObjectPtrLabel( ptr, bufSize, length, label );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetObjectPtrLabel( IntPtr ptr, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetObjectPtrLabel( ptr, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void BufferStorage( int target, int size, void* data, uint flags )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferStorage( target, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferStorage< T >( int target, T[] data, uint flags ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferStorage( target, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexImage( uint texture, int level, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearTexImage( texture, level, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexImage< T >( uint texture, int level, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearTexImage( texture, level, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearTexSubImage( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearTexSubImage< T >( uint texture, int level, int xOffset, int yOffset, int zOffset, int width, int height, int depth, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearTexSubImage( texture, level, xOffset, yOffset, zOffset, width, height, depth, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersBase( int target, uint first, int count, uint* buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBuffersBase( target, first, count, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersBase( int target, uint first, uint[] buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBuffersBase( target, first, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindBuffersRange( int target, uint first, int count, uint* buffers, int* offsets, int* sizes )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBuffersRange( target, first, count, buffers, offsets, sizes );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffersRange( int target, uint first, uint[] buffers, int[] offsets, int[] sizes )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBuffersRange( target, first, buffers, offsets, sizes );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindTextures( uint first, int count, uint* textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindTextures( first, count, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextures( uint first, uint[] textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindTextures( first, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindSamplers( uint first, int count, uint* samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindSamplers( first, count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindSamplers( uint first, uint[] samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindSamplers( first, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindImageTextures( uint first, int count, uint* textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindImageTextures( first, count, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindImageTextures( uint first, uint[] textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindImageTextures( first, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindVertexBuffers( uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindVertexBuffers( first, count, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexBuffers( uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindVertexBuffers( first, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClipControl( int origin, int depth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClipControl( origin, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTransformFeedbacks( int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateTransformFeedbacks( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTransformFeedbacks( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateTransformFeedbacks( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateTransformFeedbacks()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateTransformFeedbacks();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferBase( uint xfb, uint index, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TransformFeedbackBufferBase( xfb, index, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackBufferRange( uint xfb, uint index, uint buffer, int offset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TransformFeedbackBufferRange( xfb, index, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackiv( uint xfb, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbackiv( xfb, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbackiv( uint xfb, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbackiv( xfb, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki_v( uint xfb, int pname, uint index, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbacki_v( xfb, pname, index, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki_v( uint xfb, int pname, uint index, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbacki_v( xfb, pname, index, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, long* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbacki64_v( xfb, pname, index, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTransformFeedbacki64_v( uint xfb, int pname, uint index, ref long[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbacki64_v( xfb, pname, index, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateBuffers( int n, uint* buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateBuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateBuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateBuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateBuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void NamedBufferStorage( uint buffer, int size, void* data, uint flags )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferStorage( buffer, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferStorage< T >( uint buffer, int size, T[] data, uint flags ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferStorage( buffer, size, data, flags );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferData( uint buffer, int size, void* data, int usage )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferData( buffer, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferData< T >( uint buffer, int size, T[] data, int usage ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferData( buffer, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedBufferSubData< T >( uint buffer, int offset, int size, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyNamedBufferSubData( uint readBuffer, uint writeBuffer, int readOffset, int writeOffset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyNamedBufferSubData( readBuffer, writeBuffer, readOffset, writeOffset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferData( uint buffer, int internalformat, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedBufferData( buffer, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferData< T >( uint buffer, int internalformat, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedBufferData( buffer, internalformat, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedBufferSubData( uint buffer, int internalformat, int offset, int size, int format, int type, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedBufferSubData< T >( uint buffer, int internalformat, int offset, int size, int format, int type, T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedBufferSubData( buffer, internalformat, offset, size, format, type, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBuffer( uint buffer, int access )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.MapNamedBuffer( buffer, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapNamedBuffer< T >( uint buffer, int access ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.MapNamedBuffer< T >( buffer, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void* MapNamedBufferRange( uint buffer, int offset, int length, uint access )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.MapNamedBufferRange( buffer, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapNamedBufferRange< T >( uint buffer, int offset, int length, uint access ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.MapNamedBufferRange< T >( buffer, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool UnmapNamedBuffer( uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.UnmapNamedBuffer( buffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FlushMappedNamedBufferRange( uint buffer, int offset, int length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FlushMappedNamedBufferRange( buffer, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteriv( uint buffer, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferParameteriv( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteriv( uint buffer, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferParameteriv( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferParameteri64v( uint buffer, int pname, long* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferParameteri64v( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferParameteri64v( uint buffer, int pname, ref long[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferParameteri64v( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferPointerv( uint buffer, int pname, void** parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferPointerv( buffer, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedBufferPointerv( uint buffer, int pname, ref IntPtr[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferPointerv( buffer, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedBufferSubData( uint buffer, int offset, int size, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedBufferSubData( buffer, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetNamedBufferSubData< T >( uint buffer, int offset, int size ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetNamedBufferSubData< T >( buffer, offset, size );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateFramebuffers( int n, uint* framebuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateFramebuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateFramebuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateFramebuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateFramebuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void NamedFramebufferRenderbuffer( uint framebuffer, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferRenderbuffer( framebuffer, attachment, renderbuffertarget, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferParameteri( uint framebuffer, int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferParameteri( framebuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTexture( uint framebuffer, int attachment, uint texture, int level )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferTexture( framebuffer, attachment, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferTextureLayer( uint framebuffer, int attachment, uint texture, int level, int layer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferTextureLayer( framebuffer, attachment, texture, level, layer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffer( uint framebuffer, int buf )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferDrawBuffer( framebuffer, buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void NamedFramebufferDrawBuffers( uint framebuffer, int n, int* bufs )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferDrawBuffers( framebuffer, n, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferDrawBuffers( uint framebuffer, int[] bufs )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferDrawBuffers( framebuffer, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedFramebufferReadBuffer( uint framebuffer, int src )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedFramebufferReadBuffer( framebuffer, src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferData( uint framebuffer, int numAttachments, int* attachments )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateNamedFramebufferData( framebuffer, numAttachments, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferData( uint framebuffer, int[] attachments )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateNamedFramebufferData( framebuffer, attachments );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void InvalidateNamedFramebufferSubData( uint framebuffer, int numAttachments, int* attachments, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateNamedFramebufferSubData( framebuffer, numAttachments, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void InvalidateNamedFramebufferSubData( uint framebuffer, int[] attachments, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.InvalidateNamedFramebufferSubData( framebuffer, attachments, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferiv( uint framebuffer, int buffer, int drawbuffer, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferuiv( uint framebuffer, int buffer, int drawbuffer, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferuiv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfv( uint framebuffer, int buffer, int drawbuffer, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferfv( framebuffer, buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearNamedFramebufferfi( uint framebuffer, int buffer, float depth, int stencil )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearNamedFramebufferfi( framebuffer, buffer, depth, stencil );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlitNamedFramebuffer( uint readFramebuffer, uint drawFramebuffer, int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlitNamedFramebuffer( readFramebuffer, drawFramebuffer, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        CheckErrors();
    }

    /// <inheritdoc />
    public int CheckNamedFramebufferStatus( uint framebuffer, int target )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CheckNamedFramebufferStatus( framebuffer, target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferParameteriv( uint framebuffer, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedFramebufferParameteriv( framebuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferParameteriv( uint framebuffer, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedFramebufferParameteriv( framebuffer, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedFramebufferAttachmentParameteriv( uint framebuffer, int attachment, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedFramebufferAttachmentParameteriv( framebuffer, attachment, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateRenderbuffers( int n, uint* renderbuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateRenderbuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateRenderbuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateRenderbuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateRenderbuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorage( uint renderbuffer, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedRenderbufferStorage( renderbuffer, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void NamedRenderbufferStorageMultisample( uint renderbuffer, int samples, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.NamedRenderbufferStorageMultisample( renderbuffer, samples, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedRenderbufferParameteriv( renderbuffer, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetNamedRenderbufferParameteriv( uint renderbuffer, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetNamedRenderbufferParameteriv( renderbuffer, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateTextures( int target, int n, uint* textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateTextures( target, n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateTextures( int target, int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateTextures( target, n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateTexture( int target )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateTexture( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TextureBuffer( uint texture, int internalformat, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureBuffer( texture, internalformat, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureBufferRange( uint texture, int internalformat, uint buffer, int offset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureBufferRange( texture, internalformat, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage1D( uint texture, int levels, int internalformat, int width )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureStorage1D( texture, levels, internalformat, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2D( uint texture, int levels, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureStorage2D( texture, levels, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3D( uint texture, int levels, int internalformat, int width, int height, int depth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureStorage3D( texture, levels, internalformat, width, height, depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage2DMultisample( uint texture, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureStorage2DMultisample( texture, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureStorage3DMultisample( uint texture, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureStorage3DMultisample( texture, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage1D< T >( uint texture, int level, int xoffset, int width, int format, int type, T[] pixels ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage1D( texture, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage2D< T >( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureSubImage3D< T >( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, T[] pixels ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage1D( uint texture, int level, int xoffset, int width, int format, int imageSize, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage1D( texture, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int width, int height, int format, int imageSize, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage2D( texture, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int imageSize, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage1D( uint texture, int level, int xoffset, int x, int y, int width )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTextureSubImage1D( texture, level, xoffset, x, y, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage2D( uint texture, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTextureSubImage2D( texture, level, xoffset, yoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTextureSubImage3D( uint texture, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTextureSubImage3D( texture, level, xoffset, yoffset, zoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterf( uint texture, int pname, float param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterf( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterfv( uint texture, int pname, float* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterfv( uint texture, int pname, float[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteri( uint texture, int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameteri( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIiv( uint texture, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIiv( uint texture, int pname, int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameterIuiv( uint texture, int pname, uint[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TextureParameteriv( uint texture, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TextureParameteriv( uint texture, int pname, int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateTextureMipmap( uint texture )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenerateTextureMipmap( texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTextureUnit( uint unit, uint texture )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindTextureUnit( unit, texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureImage( uint texture, int level, int format, int type, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureImage( texture, level, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public T[] GetTextureImage< T >( uint texture, int level, int format, int type, int bufSize ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetTextureImage< T >( texture, level, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureImage( uint texture, int level, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetCompressedTextureImage( texture, level, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureImage( uint texture, int level, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetCompressedTextureImage( texture, level, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameterfv( uint texture, int level, int pname, float* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureLevelParameterfv( texture, level, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameterfv( uint texture, int level, int pname, ref float[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureLevelParameterfv( texture, level, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureLevelParameteriv( uint texture, int level, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureLevelParameteriv( texture, level, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureLevelParameteriv( uint texture, int level, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureLevelParameteriv( texture, level, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterfv( uint texture, int pname, float* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterfv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterfv( uint texture, int pname, ref float[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterfv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIiv( uint texture, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterIiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIiv( uint texture, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterIiv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameterIuiv( uint texture, int pname, uint* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterIuiv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameterIuiv( uint texture, int pname, ref uint[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameterIuiv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureParameteriv( uint texture, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameteriv( texture, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTextureParameteriv( uint texture, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureParameteriv( texture, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateVertexArrays( int n, uint* arrays )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateVertexArrays( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateVertexArrays( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateVertexArray()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateVertexArray();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DisableVertexArrayAttrib( uint vaobj, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DisableVertexArrayAttrib( vaobj, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexArrayAttrib( uint vaobj, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EnableVertexArrayAttrib( vaobj, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayElementBuffer( uint vaobj, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayElementBuffer( vaobj, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffer( uint vaobj, uint bindingindex, uint buffer, int offset, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayVertexBuffer( vaobj, bindingindex, buffer, offset, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexArrayVertexBuffers( uint vaobj, uint first, int count, uint* buffers, int* offsets, int* strides )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayVertexBuffers( vaobj, first, count, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayVertexBuffers( uint vaobj, uint first, uint[] buffers, int[] offsets, int[] strides )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayVertexBuffers( vaobj, first, buffers, offsets, strides );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribBinding( uint vaobj, uint attribindex, uint bindingindex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayAttribBinding( vaobj, attribindex, bindingindex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribFormat( uint vaobj, uint attribindex, int size, int type, bool normalized, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayAttribFormat( vaobj, attribindex, size, type, normalized, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribIFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayAttribIFormat( vaobj, attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayAttribLFormat( uint vaobj, uint attribindex, int size, int type, uint relativeoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayAttribLFormat( vaobj, attribindex, size, type, relativeoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexArrayBindingDivisor( uint vaobj, uint bindingindex, uint divisor )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexArrayBindingDivisor( vaobj, bindingindex, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayiv( uint vaobj, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayiv( vaobj, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayiv( uint vaobj, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayiv( vaobj, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayIndexediv( vaobj, index, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexediv( uint vaobj, uint index, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayIndexediv( vaobj, index, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, long* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayIndexed64iv( vaobj, index, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexArrayIndexed64iv( uint vaobj, uint index, int pname, ref long[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexArrayIndexed64iv( vaobj, index, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CreateSamplers( int n, uint* samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateSamplers( n, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateSamplers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateSamplers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateSamplers()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateSamplers();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateProgramPipelines( int n, uint* pipelines )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateProgramPipelines( n, pipelines );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateProgramPipelines( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateProgramPipelines( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateProgramPipeline()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateProgramPipeline();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void CreateQueries( int target, int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CreateQueries( target, n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] CreateQueries( int target, int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateQueries( target, n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateQuery( int target )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateQuery( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void GetQueryBufferObjecti64v( uint id, uint buffer, int pname, int offset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryBufferObjecti64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectiv( uint id, uint buffer, int pname, int offset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryBufferObjectiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectui64v( uint id, uint buffer, int pname, int offset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryBufferObjectui64v( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryBufferObjectuiv( uint id, uint buffer, int pname, int offset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryBufferObjectuiv( id, buffer, pname, offset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MemoryBarrierByRegion( uint barriers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MemoryBarrierByRegion( barriers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int format, int type, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetCompressedTextureSubImage( uint texture, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetCompressedTextureSubImage( texture, level, xoffset, yoffset, zoffset, width, height, depth, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetGraphicsResetStatus()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetGraphicsResetStatus();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnCompressedTexImage( int target, int lod, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnCompressedTexImage( target, lod, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnCompressedTexImage( int target, int lod, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetnCompressedTexImage( target, lod, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnTexImage( int target, int level, int format, int type, int bufSize, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnTexImage( target, level, format, type, bufSize, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] GetnTexImage( int target, int level, int format, int type, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetnTexImage( target, level, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetnUniformdv( uint program, int location, int bufSize, double* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformdv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformdv( uint program, int location, int bufSize, ref double[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformdv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformfv( uint program, int location, int bufSize, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformfv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformfv( uint program, int location, int bufSize, ref float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformfv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformiv( uint program, int location, int bufSize, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformiv( program, location, bufSize, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformiv( uint program, int location, int bufSize, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetnUniformuiv( uint program, int location, int bufSize, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformiv( program, location, bufSize, ( int* )parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetnUniformuiv( uint program, int location, int bufSize, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetnUniformuiv( program, location, bufSize, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ReadnPixels( x, y, width, height, format, type, bufSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public byte[] ReadnPixels( int x, int y, int width, int height, int format, int type, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.ReadnPixels( x, y, width, height, format, type, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void TextureBarrier()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TextureBarrier();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SpecializeShader( uint shader, byte* pEntryPoint, uint numSpecializationConstants, uint* pConstantIndex, uint* pConstantValue )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SpecializeShader( uint shader, string pEntryPoint, uint numSpecializationConstants, uint[] pConstantIndex, uint[] pConstantValue )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SpecializeShader( shader, pEntryPoint, numSpecializationConstants, pConstantIndex, pConstantValue );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArraysIndirectCount( int mode, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArraysIndirectCount( int mode, GLBindings.DrawArraysIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArraysIndirectCount( mode, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsIndirectCount( int mode, int type, void* indirect, int drawcount, int maxdrawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsIndirectCount( int mode, int type, GLBindings.DrawElementsIndirectCommand indirect, int drawcount, int maxdrawcount, int stride )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsIndirectCount( mode, type, indirect, drawcount, maxdrawcount, stride );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffsetClamp( float factor, float units, float clamp )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PolygonOffsetClamp( factor, units, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public (int major, int minor) GetOpenGLVersion()
    {
        Logger.Checkpoint();
        Calls++;
        var (major, minor) = GdxApi.Bindings.GetOpenGLVersion();
        CheckErrors();

        return ( major, minor );
    }

    /// <inheritdoc />
    public unsafe void MessageCallback( int source, int type, uint id, int severity, int length, string message, void* userParam )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MessageCallback( source, type, id, severity, length, message, userParam );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CullFace( int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CullFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FrontFace( int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FrontFace( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Hint( int target, int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Hint( target, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LineWidth( float width )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.LineWidth( width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointSize( float size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointSize( size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonMode( int face, int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PolygonMode( face, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Scissor( int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Scissor( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterf( int target, int pname, float param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterf( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterfv( int target, int pname, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterfv( int target, int pname, float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteri( int target, int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameteri( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameteriv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameteriv( int target, int pname, int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage1D( int target, int level, int internalformat, int width, int border, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage1D< T >( int target, int level, int internalformat, int width, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage1D( target, level, internalformat, width, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexImage2D( int target, int level, int internalformat, int width, int height, int border, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2D< T >( int target, int level, int internalformat, int width, int height, int border, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage2D( target, level, internalformat, width, height, border, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffer( int buf )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawBuffer( buf );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Clear( uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Clear( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearColor( float red, float green, float blue, float alpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearStencil( int s )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearStencil( s );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearDepth( double depth )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearDepth( depth );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMask( uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilMask( mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMask( bool red, bool green, bool blue, bool alpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ColorMask( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthMask( bool flag )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthMask( flag );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disable( int cap )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Disable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enable( int cap )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Enable( cap );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Finish()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Finish();
        CheckErrors();
    }

    /// <inheritdoc />
    public void Flush()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Flush();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFunc( int sfactor, int dfactor )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendFunc( sfactor, dfactor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void LogicOp( int opcode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.LogicOp( opcode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFunc( int func, int @ref, uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilFunc( func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOp( int fail, int zfail, int zpass )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilOp( fail, zfail, zpass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DepthFunc( int func )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthFunc( func );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStoref( int pname, float param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PixelStoref( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PixelStorei( int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PixelStorei( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadBuffer( int src )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ReadBuffer( src );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ReadPixels( int x, int y, int width, int height, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ReadPixels( x, y, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ReadPixels< T >( int x, int y, int width, int height, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ReadPixels( x, y, width, height, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleanv( int pname, bool* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBooleanv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleanv( int pname, ref bool[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBooleanv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetDoublev( int pname, double* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetDoublev( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetDoublev( int pname, ref double[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetDoublev( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public int GetError()
    {
        Logger.Checkpoint();
        Calls++;
        var err = GdxApi.Bindings.GetError();
        CheckErrors();

        return err;
    }

    /// <inheritdoc />
    public unsafe void GetFloatv( int pname, float* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFloatv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFloatv( int pname, ref float[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFloatv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegerv( int pname, int* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetIntegerv( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegerv( int pname, ref int[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetIntegerv( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetString( int name )
    {
        Logger.Checkpoint();
        Calls++;
        var str = GdxApi.Bindings.GetString( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public string GetStringSafe( int name )
    {
        Logger.Checkpoint();
        Calls++;
        var str = GdxApi.Bindings.GetStringSafe( name );
        CheckErrors();

        return str;
    }

    /// <inheritdoc />
    public unsafe void GetTexImage( int target, int level, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexImage( target, level, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexImage< T >( int target, int level, int format, int type, ref T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexImage( target, level, format, type, ref pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterfv( int target, int pname, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterfv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterfv( int target, int pname, ref float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterfv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameteriv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameteriv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameterfv( int target, int level, int pname, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexLevelParameterfv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameterfv( int target, int level, int pname, ref float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexLevelParameterfv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexLevelParameteriv( int target, int level, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexLevelParameteriv( target, level, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexLevelParameteriv( int target, int level, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexLevelParameteriv( target, level, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabled( int cap )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsEnabled( cap );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DepthRange( double near, double far )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DepthRange( near, far );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Viewport( int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Viewport( x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawArrays( int mode, int first, int count )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElements( int mode, int count, int type, void* indices )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElements< T >( int mode, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PolygonOffset( float factor, float units )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PolygonOffset( factor, units );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage1D( int target, int level, int internalformat, int x, int y, int width, int border )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTexImage1D( target, level, internalformat, x, y, width, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexImage2D( int target, int level, int internalformat, int x, int y, int width, int height, int border )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTexImage2D( target, level, internalformat, x, y, width, height, border );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage1D( int target, int level, int xoffset, int x, int y, int width )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTexSubImage1D( target, level, xoffset, x, y, width );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage2D( int target, int level, int xoffset, int yoffset, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTexSubImage2D( target, level, xoffset, yoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage1D( int target, int level, int xoffset, int width, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage1D< T >( int target, int level, int xoffset, int width, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage1D( target, level, xoffset, width, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, void* pixels )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexSubImage2D< T >( int target, int level, int xoffset, int yoffset, int width, int height, int format, int type, T[] pixels )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage2D( target, level, xoffset, yoffset, width, height, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindTexture( int target, uint texture )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindTexture( target, texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteTextures( int n, uint* textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteTextures( params uint[] textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteTextures( textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenTextures( int n, uint* textures )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenTextures( n, textures );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenTextures( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenTextures( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenTexture()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenTexture();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsTexture( uint texture )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsTexture( texture );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElements( int mode, uint start, uint end, int count, int type, void* indices )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawRangeElements( mode, start, end, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElements< T >( int mode, uint start, uint end, int count, int type, T[] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawRangeElements( mode, start, end, count, type, indices );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage3D( target, level, internalformat, width, height, depth, border, format, type, pixels );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyTexSubImage3D( int target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyTexSubImage3D( target, level, xoffset, yoffset, zoffset, x, y, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ActiveTexture( int texture )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ActiveTexture( texture );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleCoverage( float value, bool invert )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SampleCoverage( value, invert );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage3D( int target, int level, int internalformat, int width, int height, int depth, int border, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage3D( target, level, internalformat, width, height, depth, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage2D( target, level, internalformat, width, height, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage2D( int target, int level, int internalformat, int width, int height, int border, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage2D( target, level, internalformat, width, height, border, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage1D( target, level, internalformat, width, border, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexImage1D( int target, int level, int internalformat, int width, int border, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexImage1D( target, level, internalformat, width, border, data );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage3D( target, level, xoffset, yoffset, zoffset, width, height, depth, format, data );
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
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage2D( int target, int level, int xoffset, int yoffset, int width, int height, int format, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage2D( target, level, xoffset, yoffset, width, height, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, int imageSize, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage1D( target, level, xoffset, width, format, imageSize, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompressedTexSubImage1D( int target, int level, int xoffset, int width, int format, byte[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompressedTexSubImage1D( target, level, xoffset, width, format, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetCompressedTexImage( int target, int level, void* img )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetCompressedTexImage( target, level, img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetCompressedTexImage( int target, int level, ref byte[] img )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetCompressedTexImage( target, level, ref img );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendFuncSeparate( int sfactorRGB, int dfactorRGB, int sfactorAlpha, int dfactorAlpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendFuncSeparate( sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawArrays( int mode, int* first, int* count, int drawcount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArrays( mode, first, count, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawArrays( int mode, int[] first, int[] count )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawArrays( mode, first, count );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElements( int mode, int* count, int type, void** indices, int drawcount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElements( mode, count, type, indices, drawcount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElements< T >( int mode, int[] count, int type, T[][] indices )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElements( mode, count, type, indices );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterf( int pname, float param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameterf( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameterfv( int pname, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameterfv( int pname, float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameterfv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteri( int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameteri( pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void PointParameteriv( int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PointParameteriv( int pname, int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PointParameteriv( pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendColor( float red, float green, float blue, float alpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendColor( red, green, blue, alpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquation( int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendEquation( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenQueries( int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenQueries( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenQueries( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenQuery()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenQuery();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteQueries( int n, uint* ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteQueries( n, ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteQueries( params uint[] ids )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteQueries( ids );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsQuery( uint id )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsQuery( id );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginQuery( int target, uint id )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BeginQuery( target, id );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndQuery( int target )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EndQuery( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryiv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryiv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectiv( uint id, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectiv( uint id, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectuiv( uint id, int pname, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectuiv( id, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectuiv( uint id, int pname, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectuiv( id, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBuffer( int target, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBuffer( target, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteBuffers( int n, uint* buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteBuffers( params uint[] buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteBuffers( buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenBuffers( int n, uint* buffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenBuffers( n, buffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenBuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenBuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenBuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenBuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsBuffer( uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsBuffer( buffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void BufferData( int target, int size, void* data, int usage )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferData( target, size, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferData< T >( int target, T[] data, int usage )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferData( target, data, usage );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BufferSubData( int target, int offset, int size, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BufferSubData< T >( int target, int offsetCount, T[] data )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BufferSubData( target, offsetCount, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferSubData( int target, int offset, int size, void* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferSubData( target, offset, size, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferSubData< T >( int target, int offsetCount, int count, ref T[] data ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferSubData( target, offsetCount, count, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBuffer( int target, int access )
    {
        Logger.Checkpoint();
        Calls++;
        void* result = GdxApi.Bindings.MapBuffer( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBuffer< T >( int target, int access ) where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        Span< T > result = GdxApi.Bindings.MapBuffer< T >( target, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool UnmapBuffer( int target )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.UnmapBuffer( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteriv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferPointerv( int target, int pname, void** parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferPointerv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferPointerv( int target, int pname, ref IntPtr[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferPointerv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlendEquationSeparate( int modeRGB, int modeAlpha )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlendEquationSeparate( modeRGB, modeAlpha );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawBuffers( int n, int* bufs )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawBuffers( n, bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawBuffers( params int[] bufs )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawBuffers( bufs );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilOpSeparate( int face, int sfail, int dpfail, int dppass )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilOpSeparate( face, sfail, dpfail, dppass );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilFuncSeparate( int face, int func, int @ref, uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilFuncSeparate( face, func, @ref, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void StencilMaskSeparate( int face, uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.StencilMaskSeparate( face, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public void AttachShader( uint program, uint shader )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.AttachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindAttribLocation( uint program, uint index, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindAttribLocation( uint program, uint index, string name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindAttribLocation( program, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CompileShader( uint shader )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CompileShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint CreateProgram()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateProgram();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint CreateShader( int type )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CreateShader( type );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DeleteProgram( uint program )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteShader( uint shader )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteShader( shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DetachShader( uint program, uint shader )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DetachShader( program, shader );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DisableVertexAttribArray( uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DisableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EnableVertexAttribArray( uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EnableVertexAttribArray( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveAttrib( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveAttrib( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveAttrib( uint program, uint index, int bufSize, out int size, out int type )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveAttrib( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniform( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniform( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniform( uint program, uint index, int bufSize, out int size, out int type )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveUniform( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetAttachedShaders( uint program, int maxCount, int* count, uint* shaders )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetAttachedShaders( program, maxCount, count, shaders );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetAttachedShaders( uint program, int maxCount )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetAttachedShaders( program, maxCount );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetAttribLocation( uint program, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetAttribLocation( uint program, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetAttribLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetProgramiv( uint program, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramiv( program, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetProgramiv( uint program, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramiv( program, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetProgramInfoLog( uint program, int bufSize, int* length, byte* infoLog )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetProgramInfoLog( program, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetProgramInfoLog( uint program, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetProgramInfoLog( program, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderiv( uint shader, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderiv( shader, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetShaderiv( uint shader, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderiv( shader, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetShaderInfoLog( uint shader, int bufSize, int* length, byte* infoLog )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderInfoLog( shader, bufSize, length, infoLog );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderInfoLog( uint shader, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetShaderInfoLog( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetShaderSource( uint shader, int bufSize, int* length, byte* source )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetShaderSource( shader, bufSize, length, source );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetShaderSource( uint shader, int bufSize = 4096 )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetShaderSource( shader, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe int GetUniformLocation( uint program, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetUniformLocation( uint program, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetUniformLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetUniformfv( uint program, int location, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformfv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformfv( uint program, int location, ref float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformfv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformiv( uint program, int location, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformiv( uint program, int location, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribdv( uint index, int pname, double* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribdv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribdv( uint index, int pname, ref double[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribdv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribfv( uint index, int pname, float* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribfv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribfv( uint index, int pname, ref float[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribfv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribiv( uint index, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribiv( uint index, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribPointerv( uint index, int pname, void** pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribPointerv( index, pname, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribPointerv( uint index, int pname, ref uint[] pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribPointerv( index, pname, ref pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsProgram( uint program )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsShader( uint shader )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsShader( shader );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void LinkProgram( uint program )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.LinkProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ShaderSource( uint shader, int count, byte** str, int* length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ShaderSource( shader, count, str, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ShaderSource( uint shader, params string[] @string )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ShaderSource( shader, @string );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UseProgram( uint program )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UseProgram( program );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1f( int location, float v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1f( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2f( int location, float v0, float v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2f( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3f( int location, float v0, float v1, float v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3f( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4f( int location, float v0, float v1, float v2, float v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4f( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1i( int location, int v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1i( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2i( int location, int v0, int v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2i( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3i( int location, int v0, int v1, int v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3i( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4i( int location, int v0, int v1, int v2, int v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4i( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1fv( int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1fv( int location, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2fv( int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2fv( int location, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3fv( int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3fv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3fv( int location, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4fv( int location, int count, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4fv( location );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4fv( int location, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4fv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1iv( int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1iv( int location, params int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2iv( int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2iv( int location, params int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3iv( int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3iv( int location, params int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4iv( int location, int count, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4iv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4iv( int location, params int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4iv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool ValidateProgram( uint program )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.ValidateProgram( program );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void VertexAttrib1d( uint index, double x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1d( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1dv( uint index, params double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1f( uint index, float x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1f( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1fv( uint index, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1fv( uint index, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1s( uint index, short x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1s( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib1sv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib1sv( uint index, params short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib1sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2d( uint index, double x, double y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2d( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2dv( uint index, params double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2f( uint index, float x, float y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2f( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2fv( uint index, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2fv( uint index, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2s( uint index, short x, short y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2s( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib2sv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib2sv( uint index, params short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib2sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3d( uint index, double x, double y, double z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3d( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3dv( uint index, params double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3f( uint index, float x, float y, float z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3f( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3fv( uint index, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3fv( uint index, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3s( uint index, short x, short y, short z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3s( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib3sv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib3sv( uint index, params short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib3sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nbv( uint index, sbyte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nbv( uint index, params sbyte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nbv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Niv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Niv( uint index, params int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Niv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nsv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nsv( uint index, params short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nsv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nub( uint index, byte x, byte y, byte z, byte w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nub( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nubv( uint index, byte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nubv( uint index, params byte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nuiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nuiv( uint index, params uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nuiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4Nusv( uint index, ushort* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4Nusv( uint index, params ushort[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4Nusv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4bv( uint index, sbyte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4bv( uint index, params sbyte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4d( uint index, double x, double y, double z, double w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4d( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4dv( uint index, double* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4dv( uint index, params double[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4dv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4f( uint index, float x, float y, float z, float w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4f( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4fv( uint index, float* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4fv( uint index, params float[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4fv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4iv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4iv( uint index, params int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4s( uint index, short x, short y, short z, short w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4s( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4sv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4sv( uint index, params short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4ubv( uint index, byte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4ubv( uint index, params byte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4ubv( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4uiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4uiv( uint index, params uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttrib4usv( uint index, ushort* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttrib4usv( uint index, params ushort[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttrib4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, void* pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribPointer( uint index, int size, int type, bool normalized, int stride, uint pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribPointer( index, size, type, normalized, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x3fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x3fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x2fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x2fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix2x4fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix2x4fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix2x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x2fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x2fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x2fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x2fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix3x4fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x4fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix3x4fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix3x4fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void UniformMatrix4x3fv( int location, int count, bool transpose, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x3fv( location, count, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void UniformMatrix4x3fv( int location, bool transpose, params float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformMatrix4x3fv( location, transpose, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ColorMaski( uint index, bool r, bool g, bool b, bool a )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ColorMaski( index, r, g, b, a );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBooleani_v( int target, uint index, bool* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBooleani_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBooleani_v( int target, uint index, ref bool[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBooleani_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetIntegeri_v( int target, uint index, int* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetIntegeri_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetIntegeri_v( int target, uint index, ref int[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetIntegeri_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Enablei( int target, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Enablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Disablei( int target, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Disablei( target, index );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsEnabledi( int target, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsEnabledi( target, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BeginTransformFeedback( int primitiveMode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BeginTransformFeedback( primitiveMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndTransformFeedback()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EndTransformFeedback();
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferRange( int target, uint index, uint buffer, int offset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBufferRange( target, index, buffer, offset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindBufferBase( int target, uint index, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindBufferBase( target, index, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TransformFeedbackVaryings( uint program, int count, byte** varyings, int bufferMode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TransformFeedbackVaryings( program, count, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TransformFeedbackVaryings( uint program, string[] varyings, int bufferMode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TransformFeedbackVaryings( program, varyings, bufferMode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTransformFeedbackVarying( uint program, uint index, int bufSize, int* length, int* size, int* type, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTransformFeedbackVarying( program, index, bufSize, length, size, type, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetTransformFeedbackVarying( uint program, uint index, int bufSize, out int size, out int type )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetTransformFeedbackVarying( program, index, bufSize, out size, out type );
        CheckErrors();

        size = 0;
        type = 0;

        return result;
    }

    /// <inheritdoc />
    public void ClampColor( int target, int clamp )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClampColor( target, clamp );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BeginConditionalRender( uint id, int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BeginConditionalRender( id, mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public void EndConditionalRender()
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.EndConditionalRender();
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribIPointer( uint index, int size, int type, int stride, void* pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribIPointer( uint index, int size, int type, int stride, uint pointer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribIPointer( index, size, type, stride, pointer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIiv( uint index, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribIiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIiv( uint index, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribIiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetVertexAttribIuiv( uint index, int pname, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribIuiv( index, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetVertexAttribIuiv( uint index, int pname, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetVertexAttribIuiv( index, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1i( uint index, int x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1i( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2i( uint index, int x, int y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2i( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3i( uint index, int x, int y, int z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3i( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4i( uint index, int x, int y, int z, int w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4i( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1ui( uint index, uint x )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1ui( index, x );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2ui( uint index, uint x, uint y )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2ui( index, x, y );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3ui( uint index, uint x, uint y, uint z )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3ui( index, x, y, z );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ui( uint index, uint x, uint y, uint z, uint w )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4ui( index, x, y, z, w );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1iv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1iv( uint index, int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2iv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2iv( uint index, int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3iv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3iv( uint index, int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4iv( uint index, int* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4iv( uint index, int[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4iv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI1uiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI1uiv( uint index, uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI1uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI2uiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI2uiv( uint index, uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI2uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI3uiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI3uiv( uint index, uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI3uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4uiv( uint index, uint* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4uiv( uint index, uint[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4uiv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4bv( uint index, sbyte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4bv( uint index, sbyte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4bv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4sv( uint index, short* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4sv( uint index, short[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4sv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4ubv( uint index, byte* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4ubv( uint index, byte[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4ubv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribI4usv( uint index, ushort* v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribI4usv( uint index, ushort[] v )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribI4usv( index, v );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformuiv( uint program, int location, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformuiv( program, location, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetUniformuiv( uint program, int location, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformuiv( program, location, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocation( uint program, uint color, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocation( uint program, uint color, string name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindFragDataLocation( program, color, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataLocation( uint program, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataLocation( uint program, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetFragDataLocation( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void Uniform1ui( int location, uint v0 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1ui( location, v0 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2ui( int location, uint v0, uint v1 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2ui( location, v0, v1 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3ui( int location, uint v0, uint v1, uint v2 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3ui( location, v0, v1, v2 );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4ui( int location, uint v0, uint v1, uint v2, uint v3 )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4ui( location, v0, v1, v2, v3 );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform1uiv( int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform1uiv( int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform1uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform2uiv( int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform2uiv( int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform2uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform3uiv( int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform3uiv( int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform3uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void Uniform4uiv( int location, int count, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4uiv( location, count, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void Uniform4uiv( int location, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.Uniform4uiv( location, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIiv( int target, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIiv( int target, int pname, int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterIiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void TexParameterIuiv( int target, int pname, uint* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexParameterIuiv( int target, int pname, uint[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexParameterIuiv( target, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIiv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterIiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIiv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterIiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetTexParameterIuiv( int target, int pname, uint* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterIuiv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetTexParameterIuiv( int target, int pname, ref uint[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetTexParameterIuiv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferiv( int buffer, int drawbuffer, int* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferiv( int buffer, int drawbuffer, int[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferuiv( int buffer, int drawbuffer, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferuiv( int buffer, int drawbuffer, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferuiv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void ClearBufferfv( int buffer, int drawbuffer, float* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfv( int buffer, int drawbuffer, float[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferfv( buffer, drawbuffer, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ClearBufferfi( int buffer, int drawbuffer, float depth, int stencil )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ClearBufferfi( buffer, drawbuffer, depth, stencil );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe byte* GetStringi( int name, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetStringi( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public string GetStringiSafe( int name, uint index )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetStringiSafe( name, index );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsRenderbuffer( uint renderbuffer )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsRenderbuffer( renderbuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindRenderbuffer( int target, uint renderbuffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindRenderbuffer( target, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteRenderbuffers( int n, uint* renderbuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteRenderbuffers( params uint[] renderbuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteRenderbuffers( renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenRenderbuffers( int n, uint* renderbuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenRenderbuffers( n, renderbuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenRenderbuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenRenderbuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenRenderbuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenRenderbuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void RenderbufferStorage( int target, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.RenderbufferStorage( target, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetRenderbufferParameteriv( int target, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetRenderbufferParameteriv( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetRenderbufferParameteriv( int target, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetRenderbufferParameteriv( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsFramebuffer( uint framebuffer )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsFramebuffer( framebuffer );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindFramebuffer( int target, uint framebuffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindFramebuffer( target, framebuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteFramebuffers( int n, uint* framebuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteFramebuffers( params uint[] framebuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteFramebuffers( framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenFramebuffers( int n, uint* framebuffers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenFramebuffers( n, framebuffers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenFramebuffers( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenFramebuffers( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenFramebuffer()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenFramebuffer();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int CheckFramebufferStatus( int target )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.CheckFramebufferStatus( target );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FramebufferTexture1D( int target, int attachment, int textarget, uint texture, int level )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferTexture1D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture2D( int target, int attachment, int textarget, uint texture, int level )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferTexture2D( target, attachment, textarget, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture3D( int target, int attachment, int textarget, uint texture, int level, int zoffset )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferTexture3D( target, attachment, textarget, texture, level, zoffset );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferRenderbuffer( int target, int attachment, int renderbuffertarget, uint renderbuffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferRenderbuffer( target, attachment, renderbuffertarget, renderbuffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFramebufferAttachmentParameteriv( target, attachment, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetFramebufferAttachmentParameteriv( int target, int attachment, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetFramebufferAttachmentParameteriv( target, attachment, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GenerateMipmap( int target )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenerateMipmap( target );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BlitFramebuffer( int srcX0, int srcY0, int srcX1, int srcY1, int dstX0, int dstY0, int dstX1, int dstY1, uint mask, int filter )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BlitFramebuffer( srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter );
        CheckErrors();
    }

    /// <inheritdoc />
    public void RenderbufferStorageMultisample( int target, int samples, int internalformat, int width, int height )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.RenderbufferStorageMultisample( target, samples, internalformat, width, height );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTextureLayer( int target, int attachment, uint texture, int level, int layer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferTextureLayer( target, attachment, texture, level, layer );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* MapBufferRange( int target, int offset, int length, uint access )
    {
        Logger.Checkpoint();
        Calls++;
        void* result = GdxApi.Bindings.MapBufferRange( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public Span< T > MapBufferRange< T >( int target, int offset, int length, uint access )
        where T : unmanaged
    {
        Logger.Checkpoint();
        Calls++;
        Span< T > result = GdxApi.Bindings.MapBufferRange< T >( target, offset, length, access );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void FlushMappedBufferRange( int target, int offset, int length )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FlushMappedBufferRange( target, offset, length );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindVertexArray( uint array )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindVertexArray( array );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DeleteVertexArrays( int n, uint* arrays )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteVertexArrays( params uint[] arrays )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteVertexArrays( arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GenVertexArrays( int n, uint* arrays )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenVertexArrays( n, arrays );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenVertexArrays( int n )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenVertexArrays( n );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenVertexArray()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenVertexArray();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsVertexArray( uint array )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsVertexArray( array );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void DrawArraysInstanced( int mode, int first, int count, int instancecount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawArraysInstanced( mode, first, count, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstanced( int mode, int count, int type, void* indices, int instancecount )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstanced< T >( int mode, int count, int type, T[] indices, int instancecount )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstanced( mode, count, type, indices, instancecount );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexBuffer( int target, int internalformat, uint buffer )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexBuffer( target, internalformat, buffer );
        CheckErrors();
    }

    /// <inheritdoc />
    public void PrimitiveRestartIndex( uint index )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.PrimitiveRestartIndex( index );
        CheckErrors();
    }

    /// <inheritdoc />
    public void CopyBufferSubData( int readTarget, int writeTarget, int readOffset, int writeOffset, int size )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.CopyBufferSubData( readTarget, writeTarget, readOffset, writeOffset, size );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetUniformIndices( uint program, int uniformCount, byte** uniformNames, uint* uniformIndices )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetUniformIndices( program, uniformCount, uniformNames, uniformIndices );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GetUniformIndices( uint program, params string[] uniformNames )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetUniformIndices( program, uniformNames );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformsiv( uint program, int uniformCount, uint* uniformIndices, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniformsiv( program, uniformCount, uniformIndices, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetActiveUniformsiv( uint program, int pname, params uint[] uniformIndices )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveUniformsiv( program, pname, uniformIndices );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformName( uint program, uint uniformIndex, int bufSize, int* length, byte* uniformName )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniformName( program, uniformIndex, bufSize, length, uniformName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformName( uint program, uint uniformIndex, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveUniformName( program, uniformIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe uint GetUniformBlockIndex( uint program, byte* uniformBlockName )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GetUniformBlockIndex( uint program, string uniformBlockName )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetUniformBlockIndex( program, uniformBlockName );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, int* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetActiveUniformBlockiv( uint program, uint uniformBlockIndex, int pname, ref int[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniformBlockiv( program, uniformBlockIndex, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize, int* length, byte* uniformBlockName )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize, length, uniformBlockName );
        CheckErrors();
    }

    /// <inheritdoc />
    public string GetActiveUniformBlockName( uint program, uint uniformBlockIndex, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetActiveUniformBlockName( program, uniformBlockIndex, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void UniformBlockBinding( uint program, uint uniformBlockIndex, uint uniformBlockBinding )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.UniformBlockBinding( program, uniformBlockIndex, uniformBlockBinding );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsBaseVertex( int mode, int count, int type, void* indices, int basevertex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsBaseVertex< T >( int mode, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsBaseVertex( mode, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawRangeElementsBaseVertex( int mode, uint start, uint end, int count, int type, void* indices, int basevertex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawRangeElementsBaseVertex< T >( int mode, uint start, uint end, int count, int type, T[] indices, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawRangeElementsBaseVertex( mode, start, end, count, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void DrawElementsInstancedBaseVertex( int mode, int count, int type, void* indices, int instancecount, int basevertex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DrawElementsInstancedBaseVertex< T >( int mode, int count, int type, T[] indices, int instancecount, int basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DrawElementsInstancedBaseVertex( mode, count, type, indices, instancecount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void MultiDrawElementsBaseVertex( int mode, int* count, int type, void** indices, int drawcount, int* basevertex )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsBaseVertex( mode, count, type, indices, drawcount, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void MultiDrawElementsBaseVertex< T >( int mode, int type, T[][] indices, int[] basevertex )
        where T : unmanaged, IUnsignedNumber< T >
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.MultiDrawElementsBaseVertex( mode, type, indices, basevertex );
        CheckErrors();
    }

    /// <inheritdoc />
    public void ProvokingVertex( int mode )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.ProvokingVertex( mode );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void* FenceSync( int condition, uint flags )
    {
        Logger.Checkpoint();
        Calls++;
        void* result = GdxApi.Bindings.FenceSync( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public IntPtr FenceSyncSafe( int condition, uint flags )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.FenceSyncSafe( condition, flags );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe bool IsSync( void* sync )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsSync( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public bool IsSyncSafe( IntPtr sync )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsSyncSafe( sync );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSync( void* sync )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteSync( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSyncSafe( IntPtr sync )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteSyncSafe( sync );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int ClientWaitSync( void* sync, uint flags, ulong timeout )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.ClientWaitSync( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int ClientWaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.ClientWaitSyncSafe( sync, flags, timeout );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void WaitSync( void* sync, uint flags, ulong timeout )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.WaitSync( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public void WaitSyncSafe( IntPtr sync, uint flags, ulong timeout )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.WaitSyncSafe( sync, flags, timeout );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetInteger64v( int pname, long* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInteger64v( pname, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64v( int pname, ref long[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInteger64v( pname, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSynciv( void* sync, int pname, int bufSize, int* length, int* values )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSynciv( sync, pname, bufSize, length, values );
        CheckErrors();
    }

    /// <inheritdoc />
    public int[] GetSynciv( IntPtr sync, int pname, int bufSize )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetSynciv( sync, pname, bufSize );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GetInteger64i_v( int target, uint index, long* data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInteger64i_v( target, index, data );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetInteger64i_v( int target, uint index, ref long[] data )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetInteger64i_v( target, index, ref data );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetBufferParameteri64v( int target, int pname, long* parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferParameteri64v( target, pname, parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetBufferParameteri64v( int target, int pname, ref long[] parameters )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetBufferParameteri64v( target, pname, ref parameters );
        CheckErrors();
    }

    /// <inheritdoc />
    public void FramebufferTexture( int target, int attachment, uint texture, int level )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.FramebufferTexture( target, attachment, texture, level );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage2DMultisample( int target, int samples, int internalformat, int width, int height, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage2DMultisample( target, samples, internalformat, width, height, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public void TexImage3DMultisample( int target, int samples, int internalformat, int width, int height, int depth, bool fixedsamplelocations )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.TexImage3DMultisample( target, samples, internalformat, width, height, depth, fixedsamplelocations );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetMultisamplefv( int pname, uint index, float* val )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetMultisamplefv( pname, index, val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetMultisamplefvSafe( int pname, uint index, ref float[] val )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetMultisamplefvSafe( pname, index, ref val );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SampleMaski( uint maskNumber, uint mask )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SampleMaski( maskNumber, mask );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public void BindFragDataLocationIndexed( uint program, uint colorNumber, uint index, string name )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindFragDataLocationIndexed( program, colorNumber, index, name );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe int GetFragDataIndex( uint program, byte* name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public int GetFragDataIndex( uint program, string name )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GetFragDataIndex( program, name );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void GenSamplers( int count, uint* samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GenSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public uint[] GenSamplers( int count )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenSamplers( count );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public uint GenSampler()
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.GenSampler();
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public unsafe void DeleteSamplers( int count, uint* samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteSamplers( count, samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public void DeleteSamplers( params uint[] samplers )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.DeleteSamplers( samplers );
        CheckErrors();
    }

    /// <inheritdoc />
    public bool IsSampler( uint sampler )
    {
        Logger.Checkpoint();
        Calls++;
        var result = GdxApi.Bindings.IsSampler( sampler );
        CheckErrors();

        return result;
    }

    /// <inheritdoc />
    public void BindSampler( uint unit, uint sampler )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.BindSampler( unit, sampler );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteri( uint sampler, int pname, int param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameteri( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameteriv( uint sampler, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameteriv( uint sampler, int pname, int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterf( uint sampler, int pname, float param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameterf( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterfv( uint sampler, int pname, float* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterfv( uint sampler, int pname, float[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIiv( uint sampler, int pname, int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void SamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void SamplerParameterIuiv( uint sampler, int pname, uint[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.SamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameteriv( uint sampler, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameteriv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameteriv( uint sampler, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameteriv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIiv( uint sampler, int pname, int* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIiv( uint sampler, int pname, ref int[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterfv( uint sampler, int pname, float* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterfv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterfv( uint sampler, int pname, ref float[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterfv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetSamplerParameterIuiv( uint sampler, int pname, uint* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIuiv( sampler, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetSamplerParameterIuiv( uint sampler, int pname, ref uint[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetSamplerParameterIuiv( sampler, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void QueryCounter( uint id, int target )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.QueryCounter( id, target );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjecti64v( uint id, int pname, long* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjecti64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjecti64v( uint id, int pname, ref long[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjecti64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void GetQueryObjectui64v( uint id, int pname, ulong* param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectui64v( id, pname, param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void GetQueryObjectui64v( uint id, int pname, ref ulong[] param )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.GetQueryObjectui64v( id, pname, ref param );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribDivisor( uint index, uint divisor )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribDivisor( index, divisor );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1ui( uint index, int type, bool normalized, uint value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP1ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP1uiv( uint index, int type, bool normalized, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP1uiv( uint index, int type, bool normalized, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP1uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2ui( uint index, int type, bool normalized, uint value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP2ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP2uiv( uint index, int type, bool normalized, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP2uiv( uint index, int type, bool normalized, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP2uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3ui( uint index, int type, bool normalized, uint value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP3ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP3uiv( uint index, int type, bool normalized, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP3uiv( uint index, int type, bool normalized, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP3uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4ui( uint index, int type, bool normalized, uint value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP4ui( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public unsafe void VertexAttribP4uiv( uint index, int type, bool normalized, uint* value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

    /// <inheritdoc />
    public void VertexAttribP4uiv( uint index, int type, bool normalized, uint[] value )
    {
        Logger.Checkpoint();
        Calls++;
        GdxApi.Bindings.VertexAttribP4uiv( index, type, normalized, value );
        CheckErrors();
    }

//    /// <inheritdoc />
//    public void Import()
//    {
//        Logger.Checkpoint();
//        Calls++;
//        GdxApi.Bindings.Import();
//        CheckErrors();
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public unsafe string GetActiveUniform( int handle, int u, int* bufSize, Buffer buffer )
//    {
//        Logger.Checkpoint(); Calls++;
//        var result = GdxApi.GL.GetActiveUniform( handle, u, bufSize, buffer );
//        CheckErrors();
//
//        return result;
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void VertexAttribPointer( int location, int size, int type, bool normalized, int stride, Buffer buffer )
//    {
//        Logger.Checkpoint(); Calls++;
//        GdxApi.GL.VertexAttribPointer( location, size, type, normalized, stride, buffer );
//        CheckErrors();
//    }

//TODO: Unsupported method    
//    /// <inheritdoc />
//    public void UniformMatrix4fv( int location, int count, bool transpose, Buffer buffer )
//    {
//        Logger.Checkpoint(); Calls++;
//        GdxApi.GL.UniformMatrix4fv( location, count, transpose, buffer );
//        CheckErrors();
//    }
}