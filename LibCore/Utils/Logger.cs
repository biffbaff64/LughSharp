// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


using Exception = System.Exception;

namespace LibGDXSharp.LibCore.Utils;

/// <summary>
///     Simple logger that uses the <see cref="IApplication" /> logging facilities to
///     output messages.
///     The log level set with <see cref="IApplication.LogLevel" /> overrides
///     the log level set here.
/// </summary>
[PublicAPI]
public class Logger
{
    public const int LOG_NONE  = 0;
    public const int LOG_ERROR = 1;
    public const int LOG_INFO  = 2;
    public const int LOG_DEBUG = 3;

    private readonly string _tag;

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="level"></param>
    public Logger( string tag, int level = LOG_ERROR )
    {
        _tag  = tag;
        Level = level;
    }

    /// <summary>
    ///     Sets the log level.
    ///     <see cref="LOG_NONE" /> will mute all log output.
    ///     <see cref="LOG_ERROR" /> will only let error messages through.
    ///     <see cref="LOG_INFO" /> will let all non-debug messages through.
    ///     <see cref="LOG_DEBUG" /> will let all messages through.
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Debug( string message )
    {
        if ( Level >= LOG_DEBUG )
        {
            Core.Gdx.App.Debug( _tag, message );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Debug( string message, Exception exception )
    {
        if ( Level >= LOG_DEBUG )
        {
            Core.Gdx.App.Debug( _tag, message, exception );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Info( string message )
    {
        if ( Level >= LOG_INFO )
        {
            Core.Gdx.App.Log( _tag, message );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Info( string message, Exception exception )
    {
        if ( Level >= LOG_INFO )
        {
            Core.Gdx.App.Log( _tag, message, exception );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Error( string message )
    {
        if ( Level >= LOG_ERROR )
        {
            Core.Gdx.App.Error( _tag, message );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Error( string message, Exception exception )
    {
        if ( Level >= LOG_ERROR )
        {
            Core.Gdx.App.Error( _tag, message, exception );
        }
    }
}
