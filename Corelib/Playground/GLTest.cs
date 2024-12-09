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

using Corelib.Lugh.Graphics.GLUtils;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;

namespace Corelib.Playground;

public unsafe class GLTest
{
    private readonly float[] vertices =
    [
        -0.5f, -0.5f, 0.0f,
        0.5f, -0.5f, 0.0f,
        0.0f, 0.5f, 0.0f,
    ];

    private readonly uint[] indices =
    [
        0, 1, 3,
        1, 2, 3,
    ];
        
    private uint VBO;
    private uint VAO;
    private uint EBO;
    private uint vertexShader;
    private uint fragmentShader;
    private uint shaderProgram;
    
    private ShaderProgram program;
    
    const string vertexShaderSource =
        "layout (location = 0) in vec3 aPos;\n"
        + "void main()\n"
        + "{\n"
        + "   gl_Position = vec4(aPos.x, aPos.y, aPos.z, 1.0);\n"
        + "}\0";
        
    const string fragmentShaderSource =
        "out vec4 FragColor;\n"
        + "void main()\n"
        + "{\n"
        + "   FragColor = vec4(1.0, 0.5, 0.2, 1.0);\n"
        + "}\0";

    // ========================================================================
    // ========================================================================

    public GLTest()
    {
        Logger.Checkpoint();
    }

    public void Run()
    {
        Logger.Checkpoint();
        
        program = new ShaderProgram( vertexShaderSource, fragmentShaderSource );
        program.Bind();
    }

    public void Draw()
    {
    }
    
    // ========================================================================
    // ========================================================================
    
    public void Run2()
    {
        Logger.Checkpoint();

        vertexShader = Gdx.GL.glCreateShader( IGL.GL_VERTEX_SHADER );
        
        Gdx.GL.glShaderSource( vertexShader, vertexShaderSource );
        Gdx.GL.glCompileShader( vertexShader );

//        var infoLog = new int[ 512 ];
//        Gdx.GL.glGetShaderiv( vertexShader, IGL.GL_COMPILE_STATUS, ref infoLog );
//        Gdx.GL.glGetShaderInfoLog( vertexShader, infoLog.Length );
//        Logger.Debug( $"Error: Compilation failed: {infoLog}" );
        
        fragmentShader = Gdx.GL.glCreateShader( IGL.GL_FRAGMENT_SHADER );
        
        Gdx.GL.glShaderSource( fragmentShader, fragmentShaderSource );
        Gdx.GL.glCompileShader( fragmentShader );
        
        shaderProgram = Gdx.GL.glCreateProgram();
        
        Gdx.GL.glAttachShader( shaderProgram, vertexShader );
        Gdx.GL.glAttachShader( shaderProgram, fragmentShader );
        Gdx.GL.glLinkProgram( shaderProgram );
        Gdx.GL.glUseProgram( shaderProgram );

//        Gdx.GL.glGetProgramiv( shaderProgram, IGL.GL_LINK_STATUS, ref infoLog );
//        Gdx.GL.glGetProgramInfoLog( shaderProgram, infoLog.Length );
//        Logger.Debug( $"Error: Linking failed: {infoLog}" );
        
        Gdx.GL.glDeleteShader( vertexShader );
        Gdx.GL.glDeleteShader( fragmentShader );

        fixed ( uint* vao = &VAO )
        {
            Gdx.GL.glGenVertexArrays( 1, vao );
        }

        fixed ( uint* vbo = &VBO )
        {
            Gdx.GL.glGenBuffers( 1, vbo );
        }

        fixed ( uint* ebo = &EBO )
        {
            Gdx.GL.glGenBuffers( 1, ebo );
        }
        
        Gdx.GL.glBindVertexArray( VAO );
        
        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, VBO );
        Gdx.GL.glBufferData( IGL.GL_ARRAY_BUFFER, vertices, IGL.GL_STATIC_DRAW );

        Gdx.GL.glBindBuffer( IGL.GL_ELEMENT_ARRAY_BUFFER, EBO );
        Gdx.GL.glBufferData( IGL.GL_ELEMENT_ARRAY_BUFFER, indices, IGL.GL_STATIC_DRAW );
        
        Gdx.GL.glVertexAttribPointer( 0, 3, IGL.GL_FLOAT, false, 3 * sizeof( float ), ( void* )0 );
        Gdx.GL.glEnableVertexAttribArray( 0 );
        
        Gdx.GL.glBindBuffer( IGL.GL_ARRAY_BUFFER, 0 );
        Gdx.GL.glBindVertexArray( 0 );
    }

    public void Draw2()
    {
        Gdx.GL.glUseProgram( shaderProgram );
        Gdx.GL.glBindVertexArray( VAO );
        Gdx.GL.glDrawElements( IGL.GL_TRIANGLES, 6, IGL.GL_UNSIGNED_INT, indices );
    }

    public void Dispose()
    {
//        Gdx.GL.glDeleteVertexArrays( 1, &VAO );
//        Gdx.GL.glDeleteBuffers( 1, &VBO );
//        Gdx.GL.glDeleteBuffers( 1, &EBO );
//        Gdx.GL.glDeleteProgram( shaderProgram );
    }
}

