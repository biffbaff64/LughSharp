// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.Lugh.Audio;
using Corelib.Lugh.Graphics;
using Corelib.Lugh.Graphics.OpenGL;
using Corelib.Lugh.Utils;
using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Core;

/// <summary>
/// Environment class holding references to the Application,
/// Graphics, Audio, Files and Input instances.
/// </summary>
[PublicAPI]
public static class Gdx
{
    private static IApplication? _app       = null;
    private static IAudio?       _audio     = null;
    private static IInput?       _input     = null;
    private static IFiles?       _files     = null;
    private static IGraphics?    _graphics  = null;
    private static INet?         _net       = null;
    private static IGLBindings?  _igl       = null;
    
    // ========================================================================

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// From Wiktionary...
    /// <para>
    /// "1. (video games) A game mode where the player character is invulnerable to
    /// damage, typically activated by entering a cheat code."
    /// </para>
    /// <para>
    /// "2. (video games) A mode of play in (mostly) roguelike games, allowing the
    /// player to create objects on demand, to be resurrected in the case of death,
    /// etc."
    /// </para>
    /// <para>
    /// Note: Only the flag is provided by this library. It is intended for use in
    /// your local game code.
    /// </para>
    /// </summary>
    public static bool GodMode { get; set; } = false;

    /// <summary>
    /// Test mode flag which, when TRUE, means that all developer options are enabled.
    /// This must, however, mean that software with this enabled cannot be published.
    /// </summary>
    public static bool DevMode { get; set; } = false;

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Performs essential tasks, which MUST be performed to allow the
    /// framework to work correctly.
    /// </summary>
    /// <param name="app"></param>
    public static void Initialise( IApplication app )
    {
        App = app;
        
        Logger.Initialise( enableWriteToFile: false );
        Logger.EnableDebugLogging();
        Logger.EnableErrorLogging();
        
        Colors.Reset();
    }
    
    // ========================================================================
    // ========================================================================

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

    /// <inheritdoc cref="IGLBindings"/>
    public static IGLBindings GL
    {
        get
        {
            GdxRuntimeException.ThrowIfNull( _igl );

            return _igl;
        }
        set => _igl = value;
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
    
    // ========================================================================

    /// <summary>
    /// This is here, in line with my faith.
    /// </summary>
    private const string A_PRAYER_TO_THE_GODDESS =
        "Mother Earth, enlighten what's dark in me. " +
        "Strengthen what's weak in me. " +
        "Mend what's broken in me. " +
        "Bind what's bruised in me. " +
        "Heal what's sick in me. " +
        "Revive whatever peace & love has died in me.";

    // ========================================================================
    // Don't Ask...

    /*
     * Common Carp
     * Mirror Carp
     * Leather Carp
     * Grass Carp
     * Orfe
     * Roach
     * Pike
     * Perch
     * Zander
     * Rudd
     * Dace
     * Grayling
     * Bream
     * Rainbow Trout
     * Brown Trout
     * Sea Trout
     * Gudgeon
     * Stone Loach
     * Spined Loach
     * Crucian Carp
     * Tench
     * Chub
     * Barbel
     * Salmon
     * Ruffe
     * StickleBack
     * Minnow
     * Eel
     * Silver Bream
     * Bullhead
     * Smelt
     * Catfish
     * Allis Shad
     * Twaite Shad
     * Brook Lamprey
     * Bitterling
     * PumpkinSeed
     * Ide
     */

    // ========================================================================
}
