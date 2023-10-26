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

namespace LibGDXSharp.Core;

/// <summary>
/// Environment class holding references to the Application,
/// Graphics, Audio, Files and Input instances.
/// </summary>
[PublicAPI]
public static class Gdx
{
    private static IApplication? _app;
    private static IAudio?       _audio;
    private static IInput?       _input;
    private static IFiles?       _files;
    private static IGraphics?    _graphics;
    private static INet?         _net;
    private static IGL20?        _gl;
    private static IGL20?        _gl20;

    static Gdx()
    {
        _app      = null;
        _graphics = null;
        _audio    = null;
        _input    = null;
        _files    = null;
        _net      = null;
        _gl       = null;
        _gl20     = null;

        GL30 = null;
    }

    public static bool GodMode { get; set; } = false;
    public static bool DevMode { get; set; } = false;

    // ------------------------------------------------------------------------

    public static IApplication App
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _app );

            return _app;
        }
        set => _app = value;
    }

    public static IAudio Audio
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _audio );

            return _audio;
        }
        set => _audio = value;
    }

    public static IInput Input
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _input );

            return _input;
        }
        set => _input = value;
    }

    public static IFiles Files
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _files );

            return _files;
        }
        set => _files = value;
    }

    public static IGraphics Graphics
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _graphics );

            return _graphics;
        }
        set => _graphics = value;
    }

    public static INet Net
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _net );

            return _net;
        }
        set => _net = value;
    }

    public static IGL20 GL
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _gl );

            return _gl;
        }
        set => _gl = value;
    }

    public static IGL20 GL20
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _gl20 );

            return _gl20;
        }
        set => _gl20 = value;
    }

    public static IGL30? GL30 { get; set; }
}
