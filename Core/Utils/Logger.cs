using LibGDXSharp.Core;

namespace LibGDXSharp.Utils;

/// <summary>
/// Simple logger that uses the <see cref="IApplication"/> logging facilities to
/// output messages.
/// The log level set with <see cref="IApplication.LogLevel"/> overrides
/// the log level set here.
/// </summary>
public class Logger
{
    public const int LogNone  = 0;
    public const int LogError = 1;
    public const int LogInfo  = 2;
    public const int LogDebug = 3;

    private readonly string _tag;

    /// <summary>
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="level"></param>
    public Logger(string tag, int level = LogError )
    {
        this._tag  = tag;
        this.Level = level;
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Debug(string message)
    {
        if (Level >= LogDebug)
        {
            Gdx.App.Debug(_tag, message);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Debug(string message, Exception exception)
    {
        if (Level >= LogDebug)
        {
            Gdx.App.Debug(_tag, message, exception);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Info(string message)
    {
        if (Level >= LogInfo)
        {
            Gdx.App.Log(_tag, message);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Info(string message, Exception exception)
    {
        if (Level >= LogInfo)
        {
            Gdx.App.Log(_tag, message, exception);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    public void Error(string message)
    {
        if (Level >= LogError)
        {
            Gdx.App.Error(_tag, message);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="message"></param>
    /// <param name="exception"></param>
    public void Error(string message, Exception exception)
    {
        if (Level >= LogError)
        {
            Gdx.App.Error(_tag, message, exception);
        }
    }

    /// <summary>
    /// Sets the log level.
    /// <see cref="LogNone"/> will mute all log output.
    /// <see cref="LogError"/> will only let error messages through.
    /// <see cref="LogInfo"/> will let all non-debug messages through.
    /// <see cref="LogDebug"/> will let all messages through.
    /// </summary>
    public int Level { set; get; }
}