// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
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

namespace LughSharp.LibCore.Graphics.OpenGL;

[PublicAPI]
public class GLSetup
{
    public static OpenGLProfile? OGLProfile    { get; set; }
    public static GLVersion?     GLVersion     { get; set; }
    public static bool           GLInitialised { get; private set; } = false;

    public static unsafe void InitiateGL()
    {
        // Retrieve OpenGL version and profile from `.csproj` file
        // Does some switching depending on which constants are defined
        var glMajor   = Gdx.GL.GetProjectOpenGLVersionMajor();
        var glMinor   = Gdx.GL.GetProjectOpenGLVersionMinor();
        var glProfile = Gdx.GL.GetProjectOpenGLProfile();

        // Normal GLFW window creation
        Glfw.Init();
        Glfw.WindowHint( Hint.ClientAPI, ClientAPI.OpenGLAPI );
        Glfw.WindowHint( Hint.ContextVersionMajor, glMajor );
        Glfw.WindowHint( Hint.ContextVersionMinor, glMinor );

        OGLProfile = glProfile switch
        {
            "CORE"   => OpenGLProfile.CoreProfile,
            "COMPAT" => OpenGLProfile.CompatProfile,
            _        => throw new Exception( "Invalid OpenGL profile!" )
        };

        var vendorString   = Gdx.GL.glGetString( IGL.GL_VENDOR )->ToString();
        var rendererString = Gdx.GL.glGetString( IGL.GL_RENDERER )->ToString();

        GLVersion = new GLVersion( IApplication.ApplicationType.DesktopGL,
                                   Glfw.GetVersionString(),
                                   vendorString,
                                   rendererString );

        GLInitialised = true;
    }
}