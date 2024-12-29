// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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
public class Gdx
{
    // ========================================================================
    // ========================================================================

    private IGLBindings?  _glBindings;
    
    /// <summary>
    /// Globally accessible instance of classes inheriting from the <see cref="IGLBindings"/> interface.
    /// Initially initialised as an instance of <see cref="GLBindings"/>, it can be modified to
    /// reference any class inheriting from IGLBindings.
    /// The property will check internally for null, and initialise itself to reference GLBindings
    /// by default if that is the case.
    /// </summary>
    public IGLBindings Bindings
    {
        get
        {
            if ( _glBindings == null )
            {
                _glBindings = new GLBindings();
                
                Logger.Debug( "Gdx.Bindings is null, initialised to reference GLBindings." );
            }
            
            return _glBindings;
        }
        set => _glBindings = value;
    }

    /// <summary>
    /// Globally accessible reference to the Main <see cref="IApplication"/> class for the
    /// running backend.
    /// </summary>
    public IApplication App { get; set; } = null!;
    
    public IAudio    Audio    { get; set; } = null!;
    public IInput    Input    { get; set; } = null!;
    public IFiles    Files    { get; set; } = null!;
    public IGraphics Graphics { get; set; } = null!;
    public INet      Net      { get; set; } = null!;

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
    public bool GodMode { get; set; } = false;

    /// <summary>
    /// Test mode flag which, when TRUE, means that all developer options are enabled.
    /// This must, however, mean that software with this enabled cannot be published.
    /// </summary>
    public bool DevMode { get; set; } = false;

    // ========================================================================
    // ========================================================================

    public static Gdx GdxApi => Nested.Instance;

    // ========================================================================
    // ========================================================================

    private Gdx()
    {
    }

    // Fully Lazy instantiation.
    private class Nested
    {
        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Nested()
        {
        }

        internal static readonly Gdx Instance = new();
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// Performs essential tasks, which MUST be performed to allow the
    /// framework to work correctly.
    /// </summary>
    /// <param name="app"></param>
    public void Initialise( IApplication app )
    {
        App = app;

        Logger.Initialise( enableWriteToFile: false );
        Logger.EnableDebugLogging();
        Logger.EnableErrorLogging();

        Colors.Reset();
    }

    /// <summary>
    /// Enables <see cref="DevMode"/> if the environment variable "DEV_MODE" is
    /// available and is set to "TRUE" or "true".
    /// </summary>
    /// <returns> This class for chaining. </returns>
    public Gdx CheckEnableDevMode()
    {
        DevMode = CheckEnvironmentVar( "DEV_MODE", "TRUE" );
        
        return this;
    }
    
    /// <summary>
    /// Enables <see cref="GodMode"/> if the environment variable "GOD_MODE" is
    /// available and is set to "TRUE" or "true".
    /// </summary>
    /// <returns> This class for chaining. </returns>
    public Gdx CheckEnableGodMode()
    {
        if ( DevMode )
        {
            GodMode = CheckEnvironmentVar( "GOD_MODE", "TRUE" );
        }
        
        return this;
    }

    private static bool CheckEnvironmentVar( string envVar, string value )
    {
        if ( Environment.GetEnvironmentVariables().Contains( envVar ) )
        {
            return Environment.GetEnvironmentVariable( envVar )!.ToUpper() == value;
        }
        
        return false;
    }
    
    // ========================================================================
    // ========================================================================

    public void GLErrorCheck()
    {
        int error;

        if ( ( error = Bindings.GetError() ) != IGL.GL_NO_ERROR )
        {
            Logger.Error( $"GL Error: {error}" );
        }
    }

    // ========================================================================
    // ========================================================================

    /// <summary>
    /// This is here, in line with my faith.
    /// </summary>
    private const string A_PRAYER_TO_THE_GODDESS =
        "Mother Earth, enlighten what's dark in me. "
        + "Strengthen what's weak in me. "
        + "Mend what's broken in me. "
        + "Bind what's bruised in me. "
        + "Heal what's sick in me. "
        + "Revive whatever peace & love has died in me.";

    // ========================================================================
    // ========================================================================
}


