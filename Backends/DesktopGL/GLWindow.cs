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

using LibGDXSharp.Utils;
using LibGDXSharp.Utils.Buffers;
using LibGDXSharp.Utils.Collections.Extensions;

namespace LibGDXSharp.Backends.Desktop;

[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class GLWindow : IDisposable
{
    public        IGLWindowListener?         WindowListener      { get; set; }
    public unsafe Window*                    WindowHandle        { get; set; }
    public        IApplicationListener       Listener            { get; set; }
    public        IGLInput                   Input               { get; set; } = null!;
    public        GLGraphics                 Graphics            { get; set; } = null!;
    public        GLApplicationConfiguration Config              { get; set; }
    public        bool                       ListenerInitialised { get; set; } = false;

    private readonly GLApplicationBase _application;
    private readonly List< IRunnable > _runnables         = new();
    private readonly List< IRunnable > _executedRunnables = new();
    private          IntBuffer         _tmpBuffer;
    private          IntBuffer         _tmpBuffer2;
    private          bool              _iconified        = false;
    private          bool              _requestRendering = false;

    // ------------------------------------------------------------------------

//    #region Callbacks

    
//    #endregion Callbacks

    // ------------------------------------------------------------------------

    public GLWindow( IApplicationListener listener,
                     GLApplicationConfiguration config,
                     GLApplicationBase application )
    {
        this.Listener       = listener;
        this.WindowListener = config.WindowListener;
        this.Config         = config;
        this._application   = application;
        this._tmpBuffer     = BufferUtils.NewIntBuffer( 1 );
        this._tmpBuffer2    = BufferUtils.NewIntBuffer( 1 );
    }

    /// <summary>
    /// </summary>
    /// <param name="windowHandle"></param>
    public unsafe void Create( Window* windowHandle )
    {
        this.WindowHandle = windowHandle;
        this.Input        = _application.CreateInput( this );
        this.Graphics     = new GLGraphics( this );

        GLFW.SetWindowFocusCallback( windowHandle, FocusCallback );
        GLFW.SetWindowIconifyCallback( windowHandle, IconifyCallback );
        GLFW.SetWindowMaximizeCallback( windowHandle, MaximizeCallback );
        GLFW.SetWindowCloseCallback( windowHandle, CloseCallback );
        GLFW.SetDropCallback( windowHandle, DropCallback );
        GLFW.SetWindowRefreshCallback( windowHandle, RefreshCallback );

        if ( WindowListener != null )
        {
            WindowListener.Created( this );
        }
    }

    public bool Update()
    {
        if ( !ListenerInitialised )
        {
            InitialiseListener();
        }

        lock ( _runnables )
        {
            _executedRunnables.AddAll( _runnables );
            _runnables.Clear();
        }

        foreach ( IRunnable runnable in _executedRunnables )
        {
            runnable.Run();
        }

        var shouldRender = ( ( _executedRunnables.Count > 0 ) || ( Graphics.IsContinuousRendering() ) );

        _executedRunnables.Clear();

        if ( !_iconified )
        {
            Input.Update();
        }

        lock ( this )
        {
            shouldRender      |= ( _requestRendering && !_iconified );
            _requestRendering =  false;
        }

        if ( shouldRender )
        {
            unsafe
            {
                Graphics.Update();
                Listener.Render();

                GLFW.SwapBuffers( WindowHandle );
            }

        }

        if ( !_iconified )
        {
            Input.PrepareNext();
        }

        return shouldRender;
    }

    private void InitialiseListener()
    {
        if ( !ListenerInitialised )
        {
            Listener.Create();
            Listener.Resize( Graphics.Width, Graphics.Height );
            ListenerInitialised = true;
        }
    }

    public unsafe void MakeCurrent()
    {
        Gdx.Graphics = this.Graphics;
        Gdx.GL30     = this.Graphics.GetGL30();
        Gdx.GL20     = Gdx.GL30 != null ? Gdx.GL30 : Graphics.GetGL20();
        Gdx.GL       = Gdx.GL30 != null ? Gdx.GL30 : Gdx.GL20;
        Gdx.Input    = this.Input;

        GLFW.MakeContextCurrent( WindowHandle );
    }

    private void RequestRendering()
    {
        lock ( this )
        {
            this._requestRendering = true;
        }
    }

    public bool ShouldClose() => false;

    public void Dispose()
    {
        Listener.Pause();
        Listener.Dispose();
        GLCursor.Dispose( this );
        Graphics.Dispose();
        Input.Dispose();

        unsafe
        {
            GLFW.SetWindowFocusCallback( WindowHandle, null );
            GLFW.SetWindowIconifyCallback( WindowHandle, null );
            GLFW.SetWindowCloseCallback( WindowHandle, null );
            GLFW.SetDropCallback( WindowHandle, null );
            GLFW.DestroyWindow( WindowHandle );
        }

        focusCallback.free();
        iconifyCallback.free();
        maximizeCallback.free();
        closeCallback.free();
        dropCallback.free();
        refreshCallback.free();
    }
}