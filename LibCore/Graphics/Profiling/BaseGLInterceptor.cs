// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using Buffer = LughSharp.LibCore.Utils.Buffers.Buffer;

namespace LughSharp.LibCore.Graphics.Profiling;

[PublicAPI]
public abstract class BaseGLInterceptor
{
    protected readonly GLProfiler GLProfiler;

    protected BaseGLInterceptor( GLProfiler profiler )
    {
        GLProfiler = profiler;
    }

    public int          Calls           { get; set; }
    public int          TextureBindings { get; set; }
    public int          DrawCalls       { get; set; }
    public int          ShaderSwitches  { get; set; }
    public FloatCounter VertexCount     { get; set; } = new( 0 );

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public static string ResolveErrorNumber( int error )
    {
        return error switch
        {
            IGL.GL_INVALID_VALUE                 => "InvalidValue",
            IGL.GL_INVALID_OPERATION             => "InvalidOperation",
            IGL.GL_INVALID_FRAMEBUFFER_OPERATION => "InvalidFramebufferOperation",
            IGL.GL_INVALID_ENUM                  => "InvalidEnum",
            IGL.GL_OUT_OF_MEMORY                 => "OutOfMemory",
            IGL.GL_NO_ERROR                      => "NoError",
            _                                    => throw new ArgumentOutOfRangeException( nameof( error ), error, null )
        };
    }

    public void Reset()
    {
        Calls           = 0;
        TextureBindings = 0;
        DrawCalls       = 0;
        ShaderSwitches  = 0;
        VertexCount.Reset();
    }
}