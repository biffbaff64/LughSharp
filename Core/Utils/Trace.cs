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

using System.Runtime.CompilerServices;
using System.Text;

namespace LibGDXSharp.Utils;

/// <summary>
///     Object used for creating debug messages which include
///     the calling file and method.
/// </summary>
internal struct CallerID
{
    public string fileName;
    public string methodName;
    public int    lineNumber;
}

/// <summary>
///     A class to write debug messages to console and a text file.
///     Primarily intended for flow tracing, messages will display calling
///     file/class/methods and any provided debug message.
///     <see cref="TraceLevel" /> must be set to either <see cref="LOG_DEBUG" />,
///     <see cref="LOG_INFO" />, or <see cref="LOG_ERROR" /> for messages to work.
///     To enable writing to file, <see cref="EnableWriteToFile" /> must be TRUE
///     and <see cref="OpenDebugFile" /> must be called.
/// </summary>
public static class Trace
{
    private static string _debugFilePath = "";
    private static string _debugFileName = "";

    /// <summary>
    ///     Default initialisation method.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="enableWriteToFile"></param>
    /// <param name="filename"></param>
    public static void Initialise( int logLevel = LOG_NONE,
                                   bool enableWriteToFile = false,
                                   string filename = "" )
    {
        TraceLevel        = logLevel;
        EnableWriteToFile = enableWriteToFile;

        if ( EnableWriteToFile )
        {
            OpenDebugFile( filename, true );
        }
    }

    /// <summary>
    ///     Write a debug string to console and logfile, if enabled.
    ///     The string can contain format options.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    public static void Dbg( [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0,
                            string message = "",
                            params object[] args )
    {
        if ( !IsEnabled( LOG_DEBUG ) )
        {
            return;
        }

        message = string.Join( DEBUG_TAG, message );

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        var str = CreateMessage( message, callerID, args );

        Console.WriteLine( str );

        WriteToFile( str );
    }

    /// <summary>
    ///     Calls <see cref="Debug.Assert(bool, string?)" /> with the given message
    ///     if LOG_ASSERT is enabled and the supplied condition is true.
    ///     The supplied message will have the calling fgile, method and line details
    ///     prepended to it.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <param name="condition"></param>
    /// <param name="message"></param>
    public static void Assert( [CallerFilePath] string callerFilePath = "",
                               [CallerMemberName] string callerMethod = "",
                               [CallerLineNumber] int callerLine = 0,
                               bool condition = false,
                               string message = "" )
    {
        if ( !IsEnabled( LOG_ASSERT ) || !condition )
        {
            return;
        }

        message = string.Join( DEBUG_TAG, message );

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        Debug.Assert( false, CreateMessage( message, callerID ) );
    }

    /// <summary>
    ///     Writes a Debug message, but adds a divider line before and after the message.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    public static void BoxedDbg( [CallerFilePath] string callerFilePath = "",
                                 [CallerMemberName] string callerMethod = "",
                                 [CallerLineNumber] int callerLine = 0,
                                 string message = "",
                                 params object[] args )
    {
        if ( !IsEnabled( LOG_DEBUG ) )
        {
            return;
        }

        Divider();

        message = string.Join( DEBUG_TAG, message );

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        var str = CreateMessage( message, callerID, args );

        Console.WriteLine( str );

        WriteToFile( str );

        Divider();
    }

    /// <summary>
    ///     Write an error string to console and logfile, if enabled.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    public static void Err( [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0,
                            string message = "",
                            params object[] args )
    {
        if ( !IsEnabled( LOG_ERROR ) )
        {
            return;
        }

        message = string.Join( ERROR_TAG, message );

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        var str = CreateMessage( message, callerID, args );

        Console.WriteLine( str );

        WriteToFile( str );
    }

    /// <summary>
    ///     Write a message to console if the supplied condition is TRUE.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    public static void OnCondition( [CallerFilePath] string callerFilePath = "",
                                    [CallerMemberName] string callerMethod = "",
                                    [CallerLineNumber] int callerLine = 0,
                                    bool condition = false,
                                    string message = "",
                                    params object[] args )
    {
        if ( !IsEnabled( LOG_DEBUG ) || !condition )
        {
            return;
        }

        message = string.Join( DEBUG_TAG, message );

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        var str = CreateMessage( message, callerID, args );

        Console.WriteLine( str );

        WriteToFile( str );
    }

    /// <summary>
    ///     Writes a debug message consisting solely of the following:-
    ///     - Current time and date.
    ///     - Calling Class/method/line number information.
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    public static void CheckPoint( [CallerFilePath] string callerFilePath = "",
                                   [CallerMemberName] string callerMethod = "",
                                   [CallerLineNumber] int callerLine = 0 )
    {
        if ( !IsEnabled( LOG_DEBUG ) )
        {
            return;
        }

        var sb = new StringBuilder();

        var callerID = new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };

        sb.Append( GetTimeStampInfo() );
        sb.Append( " : " );
        sb.Append( GetCallerInfo( callerID ) );

        Console.WriteLine( sb.ToString() );

        WriteToFile( sb.ToString() );
    }

    /// <summary>
    ///     This method does the same as Trace#dbg(string, params object[] args)/>
    ///     but does not output time and date information.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Info( string message, params object[] args )
    {
        if ( !IsEnabled( LOG_DEBUG ) )
        {
            return;
        }

        message = string.Join( INFO_TAG, message );

        var sb = new StringBuilder( message );

        if ( args.Length > 0 )
        {
            foreach ( var arg in args )
            {
                sb.Append( ' ' );
                sb.Append( arg );
            }
        }

        message = string.Join( DEBUG_TAG, message );

        Console.WriteLine( message );

        WriteToFile( message );
    }

    /// <summary>
    ///     Creates a debug/error/info message ready for dumping.
    /// </summary>
    /// <param name="formatString">The base message</param>
    /// <param name="cid">Stack trace info from the calling method/file.</param>
    /// <param name="args">Optional additions to the message</param>
    /// <returns></returns>
    private static string CreateMessage( string formatString,
                                         CallerID cid,
                                         params object[] args )
    {
        var sb = new StringBuilder( GetTimeStampInfo() );

        sb.Append( " : " );
        sb.Append( GetCallerInfo( cid ) );
        sb.Append( " : " );

        if ( !string.IsNullOrEmpty( formatString ) )
        {
            sb.Append( formatString );
        }

        foreach ( var arg in args )
        {
            sb.Append( ' ' );
            sb.Append( arg );
        }

        return sb.ToString();
    }

    /// <summary>
    ///     Adds a dividing line to text output.
    /// </summary>
    /// <param name="ch">The character to use, default is '-'</param>
    /// <param name="length">The line length, default is 80.</param>
    public static void Divider( char ch = '-', int length = 80 )
    {
        var sb = new StringBuilder( DEBUG_TAG );

        for ( var i = 0; i < length; i++ )
        {
            sb.Append( ch );
        }

        Console.WriteLine( sb.ToString() );
    }

    /// <summary>
    ///     Opens a physical file for writing copies of debug messages to.
    /// </summary>
    /// <param name="fileName">
    ///     The filename. This should be filename only,
    ///     and the file will be created in the working directory.
    /// </param>
    /// <param name="deleteExisting">
    ///     True to delete existing copies of the file.
    ///     False to append to existing file.
    /// </param>
    public static void OpenDebugFile( string fileName, bool deleteExisting )
    {
        if ( fileName.Equals( string.Empty ) )
        {
            return;
        }

        if ( File.Exists( fileName ) && deleteExisting )
        {
            File.Delete( fileName );
        }

        if ( Gdx.DevMode )
        {
            _debugFilePath = ".//";
        }
        else
        {
            _debugFilePath = Environment.GetFolderPath( Environment.SpecialFolder.UserProfile );
            _debugFilePath = string.Join( _debugFilePath, "//.prefs//" );
        }

        _debugFileName = fileName;

        using FileStream fs = File.Create( _debugFilePath + _debugFileName );

        DateTime dateTime = DateTime.Now;

        var divider = new UTF8Encoding( true )
           .GetBytes( "-----------------------------------------------------" );

        var time = new UTF8Encoding( true ).GetBytes( dateTime.ToShortTimeString() );

        fs.Write( divider, 0, divider.Length );
        fs.Write( time, 0, time.Length );
        fs.Write( divider, 0, divider.Length );

        fs.Close();
    }

    /// <summary>
    ///     Returns a string holding the current time.
    /// </summary>
    private static string GetTimeStampInfo()
    {
        var c  = new GregorianCalendar();
        var sb = new StringBuilder();

        sb.Append( c.GetHour( DateTime.Now ) )
          .Append( ':' )
          .Append( c.GetMinute( DateTime.Now ) )
          .Append( ':' )
          .Append( c.GetSecond( DateTime.Now ) )
          .Append( ':' )
          .Append( c.GetMilliseconds( DateTime.Now ) );

        return sb.ToString();
    }

    /// <summary>
    ///     Returns a string holding the calling filename, method and line number.
    /// </summary>
    private static string GetCallerInfo( CallerID cid )
    {
        var sb = new StringBuilder();

        sb.Append( cid.fileName )
          .Append( "::" )
          .Append( cid.methodName )
          .Append( "::" )
          .Append( cid.lineNumber );

        return sb.ToString();
    }

    /// <summary>
    ///     Writes text to the logFile, if it exists.
    /// </summary>
    /// <param name="text">String holding the text to write.</param>
    private static void WriteToFile( string text )
    {
        if ( !File.Exists( _debugFilePath + _debugFileName ) )
        {
            return;
        }

        using FileStream fs = File.Open( _debugFilePath + _debugFileName, FileMode.Append );

        var debugLine = new UTF8Encoding( true ).GetBytes( text + "\n" );

        fs.Write( debugLine, 0, debugLine.Length );
        fs.Close();
    }

    /// <summary>
    ///     Returns whether the requested trace level is enabled or not.
    /// </summary>
    /// <param name="traceLevel">
    ///     The trace level to check, either LOG_DEBUG, LOG_INFO, LOG_ERROR or LOG_ASSERT
    /// </param>
    /// <returns> True if the level is enabled. </returns>
    private static bool IsEnabled( int traceLevel ) => traceLevel switch
                                                       {
                                                           LOG_DEBUG
                                                               or LOG_INFO
                                                               or LOG_ERROR
                                                               or LOG_ASSERT => ( TraceLevel & traceLevel ) != 0,
                                                           _ => false
                                                       };

    // ------------------------------------------------------------------------

    #region constants

    public const int LOG_NONE   = 0;
    public const int LOG_DEBUG  = 1;
    public const int LOG_INFO   = 2;
    public const int LOG_ERROR  = 4;
    public const int LOG_ASSERT = 8;

    private const string DEBUG_TAG = "[Debug] ";
    private const string INFO_TAG  = "[Info ] ";
    private const string ERROR_TAG = "[ERROR] ";

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public static bool EnableWriteToFile { get; set; } = false;
    public static bool AllowAsserts      { get; set; } = false;
    public static int  TraceLevel        { get; set; } = LOG_NONE;

    #endregion properties
}
