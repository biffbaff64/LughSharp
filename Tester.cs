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

using LibGDXSharp;
using LibGDXSharp.Backends.Desktop;

namespace LibGDXSharpTest;

public class Tester
{
    public static void Main( string[] args )
    {
//        TextureAtlasBuilder.buildTextureAtlases();
        CreateApplication();
    }

    private static void CreateApplication()
    {
        new GLApplication( new MainGame(), GetDefaultConfiguration() );
    }

    private static GLApplicationConfiguration GetDefaultConfiguration()
    {
        //
        // Application configuration settings
        GLApplicationConfiguration config = new();

        config.Title = "";
        config.VSyncEnabled = true;

        // Limits FPS to the refresh rate of the currently active monitor.
        config.ForegroundFPS = GLApplicationConfiguration.GetDisplayMode().RefreshRate;
        
        config.SetWindowIcon( "icon32x32.png" );
//        config.SetWindowedMode( 640, 480 );
        config.SetFullscreenMode( GLApplicationConfiguration.GetDisplayMode() );

        return config;
    }
}

public sealed class MainGame : IApplicationListener
{
    /// <summary>
    /// Called when the <see cref="IApplication"/> is first created.
    /// </summary>
    public void Create()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is resized. This can
    /// happen at any point during a non-paused state but will never happen
    /// before a call to <see cref="IApplicationListener.Create"/>
    /// </summary>
    /// <param name="width">The new width in pixels.</param>
    /// <param name="height">The new height in pixels.</param>
    public void Resize( int width, int height )
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> should draw itself.
    /// </summary>
    public void Render()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is paused, usually when
    /// it's not active or visible on-screen. An Application is also
    /// paused before it is destroyed.
    /// </summary>
    public void Pause()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is resumed from a paused state,
    /// usually when it regains focus.
    /// </summary>
    public void Resume()
    {
    }

    /// <summary>
    /// Called when the <see cref="IApplication"/> is destroyed.
    /// Preceded by a call to <see cref="IApplicationListener.Pause"/>.
    /// </summary>
    public void Dispose()
    {
    }
}