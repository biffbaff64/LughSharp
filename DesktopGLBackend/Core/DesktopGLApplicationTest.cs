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

using Corelib.Lugh.Core;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;

using Platform = Corelib.Lugh.Core.Platform;

namespace DesktopGLBackend.Core;

public class DesktopGLApplicationTest : IApplication
{
    public DesktopGLApplicationTest()
    {
        GdxApi.Initialise( this );
        GdxApi.Bindings = new GLBindings();

        Glfw.SetErrorCallback( ErrorCallback );
        Glfw.InitHint( InitHint.JoystickHatButtons, false );

        if ( !Glfw.Init() )
        {
            Glfw.GetError( out var error );
            Logger.Debug( $"Failed to initialise Glfw: {error}" );
            System.Environment.Exit( 1 );
        }
        
        return;

        static void ErrorCallback( ErrorCode error, string description )
        {
            Logger.Checkpoint();
            Logger.Error( $"ErrorCode: {error}, {description}" );
        }
        
    }

    /// <inheritdoc />
    public Platform.ApplicationType AppType   { get; set; }

    /// <inheritdoc />
    public IClipboard?              Clipboard { get; set; }

    /// <inheritdoc />
    public int GetVersion()
    {
        return 0;
    }

    /// <inheritdoc />
    public IPreferences GetPreferences( string name )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public void AddLifecycleListener( ILifecycleListener listener )
    {
    }

    /// <inheritdoc />
    public void RemoveLifecycleListener( ILifecycleListener listener )
    {
    }

    /// <inheritdoc />
    public void PostRunnable( IRunnable.Runnable runnable )
    {
    }

    /// <inheritdoc />
    public void Exit()
    {
    }
}