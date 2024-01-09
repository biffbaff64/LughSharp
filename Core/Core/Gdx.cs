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
///     Environment class holding references to the Application,
///     Graphics, Audio, Files and Input instances.
/// </summary>
public static class Gdx
{
    private static IApplication? _app       = null;
    private static IAudio?       _audio     = null;
    private static IInput?       _input     = null;
    private static IFiles?       _files     = null;
    private static IGraphics?    _graphics  = null;
    private static INet?         _net       = null;
    private static IGL20?        _gl        = null;
    private static IGL20?        _gl20      = null;
    private static IGL30?        _gl30      = null;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// From Wiktionary...
    /// <para>
    /// "1. (video games) A game mode where the player character is invulnerable to damage,
    /// typically activated by entering a cheat code."
    /// </para>
    /// <para>
    /// "2. (video games) A mode of play in (mostly) roguelike games, allowing the player to
    /// create objects on demand, to be resurrected in the case of death, etc."
    /// </para>
    /// <para>
    /// Note: Only the flag is provided by this library. It is intended for use in your local
    /// game code.
    /// </para>
    /// </summary>
    /// <seealso cref="https://en.wiktionary.org/wiki/god_mode"/>
    [PublicAPI]
    public static bool GodMode { get; set; } = false;

    /// <summary>
    /// Test mode flag which, when TRUE, means that all developer options are enabled.
    /// This must, however, mean that software with this enabled cannot be published.
    /// </summary>
    [PublicAPI]
    public static bool DevMode { get; set; } = false;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <inheritdoc cref="IApplication"/>
    public static IApplication App
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _app );

            return _app;
        }
        set => _app = value;
    }

    /// <inheritdoc cref="IAudio"/>
    public static IAudio Audio
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _audio );

            return _audio;
        }
        set => _audio = value;
    }

    /// <inheritdoc cref="IInput"/>
    public static IInput Input
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _input );

            return _input;
        }
        set => _input = value;
    }

    /// <inheritdoc cref="IFiles"/>
    public static IFiles Files
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _files );

            return _files;
        }
        set => _files = value;
    }

    /// <inheritdoc cref="IGraphics"/>
    public static IGraphics Graphics
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _graphics );

            return _graphics;
        }
        set => _graphics = value;
    }

    /// <inheritdoc cref="INet"/>
    public static INet Net
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _net );

            return _net;
        }
        set => _net = value;
    }

    /// <inheritdoc cref="IGL20"/>
    public static IGL20 GL
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _gl );

            return _gl;
        }
        set => _gl = value;
    }

    /// <inheritdoc cref="IGL20"/>
    public static IGL20 GL20
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _gl20 );

            return _gl20;
        }
        set => _gl20 = value;
    }

    /// <inheritdoc cref="IGL30"/>
    public static IGL30? GL30
    {
        get
        {
            ArgumentNullException.ThrowIfNull( _gl30 );

            return _gl30;
        }
        set => _gl30 = value;
    }
}
