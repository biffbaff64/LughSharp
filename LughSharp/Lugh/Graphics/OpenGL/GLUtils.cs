// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
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

using LughSharp.Lugh.Utils;

namespace LughSharp.Lugh.Graphics.OpenGL;

[PublicAPI]
public class GLUtils
{
    public const int           DEFAULT_GL_MAJOR       = 3;
    public const int           DEFAULT_GL_MINOR       = 3;
    public const ClientAPI     DEFAULT_CLIENT_API     = ClientAPI.OpenGLAPI;
    public const OpenGLProfile DEFAULT_OPENGL_PROFILE = OpenGLProfile.CoreProfile;

    // ========================================================================

    [PublicAPI]
    public enum ShaderParameterName : int
    {
        CompileStatus = IGL.GL_COMPILE_STATUS,
        InfoLogLength = IGL.GL_INFO_LOG_LENGTH,
        ShaderType    = IGL.GL_SHADER_TYPE,
    }

    [PublicAPI]
    public enum ProgramProperty : int
    {
        LinkStatus      = IGL.GL_LINK_STATUS,
        InfoLogLength   = IGL.GL_INFO_LOG_LENGTH,
        AttachedShaders = IGL.GL_ATTACHED_SHADERS,
    }

    // ========================================================================
    // ========================================================================

    public static string GetErrorString( int errorCode )
    {
        return errorCode switch
        {
            IGL.GL_INVALID_ENUM                  => "Invalid Enum",
            IGL.GL_INVALID_OPERATION             => "Invalid Operation",
            IGL.GL_INVALID_VALUE                 => "Invalid Value",
            IGL.GL_OUT_OF_MEMORY                 => "Out of memory",
            IGL.GL_INVALID_FRAMEBUFFER_OPERATION => "Invalid Framebuffer operation",
            IGL.GL_STACK_OVERFLOW                => "Stack Overflow",
            IGL.GL_STACK_UNDERFLOW               => "Stack Underflow",
            IGL.GL_CONTEXT_LOST                  => "Context Lost",
            var _                                => $"Unknown GL Error Code: {errorCode}",
        };
    }

    public static void CreateCapabilities()
    {
    }

    public static void SetupGLDebug()
    {
        Logger.Debug( "Setting up GL Debug" );
            
        unsafe
        {
            Glfw.WindowHint( WindowHint.OpenGLDebugContext, true );

            var debugProc = new GLBindings.GLDEBUGPROC( ( source, type, id, severity, length, message, userParam ) =>
            {
                var msg = Marshal.PtrToStringAnsi( ( IntPtr )message, length );

                Logger.Debug( $"OpenGL Debug Message:\n" +
                              $"Source: {source}\n" +
                              $"Type: {type}\n" +
                              $"ID: {id}\n" +
                              $"Severity: {severity}\n" +
                              $"Message: {msg}" );

                if ( severity == DebugSeverity.DEBUG_SEVERITY_HIGH )
                {
                    //Break on high severity errors
                    System.Diagnostics.Debugger.Break();
                }
            } );

            GdxApi.Bindings.DebugMessageCallback( debugProc, null );
            GdxApi.Bindings.Enable( IGL.GL_DEBUG_OUTPUT );
            GdxApi.Bindings.Enable( IGL.GL_DEBUG_OUTPUT_SYNCHRONOUS );
        }
    }
}