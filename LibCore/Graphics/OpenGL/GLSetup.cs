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

    // ------------------------------------------------------------------------

    /// <summary>
    /// Sets up an OpenGL context using GLFW, retrieves OpenGL version and profile information
    /// from the project settings, and initializes the OpenGL environment accordingly. 
    /// </summary>
    public static unsafe void InitiateGL()
    {
        // Retrieve OpenGL version and profile from the project settings.
        // These methods read the OpenGL major and minor version numbers
        // and the profile from the `.csproj` file.
        var glMajor   = Gdx.GL.GetProjectOpenGLVersionMajor();
        var glMinor   = Gdx.GL.GetProjectOpenGLVersionMinor();
        var glProfile = Gdx.GL.GetProjectOpenGLProfile();

        // Initialize the GLFW library.
        Glfw.Init();

        // Set the client API to use OpenGL.
        Glfw.WindowHint( Hint.ClientAPI, ClientAPI.OpenGLAPI );

        // Set the OpenGL context version based on the retrieved major and minor version numbers.
        Glfw.WindowHint( Hint.ContextVersionMajor, glMajor );
        Glfw.WindowHint( Hint.ContextVersionMinor, glMinor );

        // Determine the OpenGL profile to use based on the profile string retrieved.
        OGLProfile = glProfile switch
        {
            "CORE"   => OpenGLProfile.CoreProfile,                       // Use the core profile.
            "COMPAT" => OpenGLProfile.CompatProfile,                     // Use the compatibility profile.
            var _    => throw new Exception( "Invalid OpenGL profile!" ) // Throw an exception for an invalid profile.
        };

        // Retrieve the OpenGL vendor and renderer strings.
        var vendorString   = Gdx.GL.glGetString( IGL.GL_VENDOR )->ToString();
        var rendererString = Gdx.GL.glGetString( IGL.GL_RENDERER )->ToString();

        // Create a new GLVersion object with the application type, GLFW version, vendor, and renderer strings.
        GLVersion = new GLVersion( Platform.ApplicationType.WindowsGL,
                                   Glfw.GetVersionString(),
                                   vendorString,
                                   rendererString );

        // Set the flag indicating that OpenGL has been initialized.
        GLInitialised = true;
    }
}
