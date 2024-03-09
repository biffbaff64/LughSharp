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


using System.Runtime.CompilerServices;
using System.Text;

namespace LibGDXSharp.LibCore.Utils;

/// <summary>
///     A class to write debug messages to console and a text file.
///     Primarily intended for flow tracing, messages will display calling
///     file/class/methods and any provided debug message.
///     <see cref="TraceLevel" /> must be set to either <see cref="LOG_DEBUG" />,
///     <see cref="LOG_INFO" />, or <see cref="LOG_ERROR" /> for messages to work.
///     To enable writing to file, <see cref="EnableWriteToFile" /> must be TRUE
///     and <see cref="OpenDebugFile" /> must be called.
/// </summary>
[PublicAPI]
public static class Logger
{
    // ------------------------------------------------------------------------

    #region constants

    public const int LOG_NONE  = 0;
    public const int LOG_SYS   = 1;
    public const int LOG_DEBUG = 2;
    public const int LOG_INFO  = 4;
    public const int LOG_ERROR = 8;

    private const string DEBUG_TAG = "[Debug] ";
    private const string INFO_TAG  = "[Info ] ";
    private const string ERROR_TAG = "[ERROR] ";

    #endregion constants

    // ------------------------------------------------------------------------

    #region properties

    public static bool EnableWriteToFile { get; set; } = false;
    public static int  TraceLevel        { get; set; }

    #endregion properties

    // ------------------------------------------------------------------------

    private static string _debugFilePath = "";
    private static string _debugFileName = "";

    // ------------------------------------------------------------------------

    #region public methods

    /// <summary>
    ///     Default Constructor.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="enableWriteToFile"></param>
    /// <param name="filename"></param>
    public static void Initialise( int logLevel = LOG_DEBUG,
                                   bool enableWriteToFile = true,
                                   string filename = "trace.txt" )
    {
        TraceLevel        = logLevel;
        EnableWriteToFile = enableWriteToFile;

        if ( EnableWriteToFile )
        {
            OpenDebugFile( filename, true );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    public static void Sys( string message = "",
                            [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0 )
    {
        if ( message is "" )
        {
            message = "[[[ Empty Message ]]]";
        }

        CallerID callerID = MakeCallerID( callerFilePath, callerMethod, callerLine );

        var str = CreateMessage( $"SYSLOG: {message}", callerID );

        Console.WriteLine( str );

        WriteToFile( str );
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="boxedDebug"></param>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    public static void Dbg( string message,
                            bool boxedDebug = false,
                            [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0 )
    {
        if ( !IsEnabled( LOG_DEBUG ) )
        {
            return;
        }

        if ( boxedDebug )
        {
            Divider();
        }

        CallerID callerID = MakeCallerID( callerFilePath, callerMethod, callerLine );

        var str = CreateMessage( $"{DEBUG_TAG}{message}", callerID );

        Console.WriteLine( str );

        WriteToFile( str );
        
        if ( boxedDebug )
        {
            Divider();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    public static void Err( string message,
                            [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0 )
    {
        if ( !IsEnabled( LOG_ERROR ) )
        {
            return;
        }

        CallerID callerID = MakeCallerID( callerFilePath, callerMethod, callerLine );

        var str = CreateMessage( $"{ERROR_TAG}{message}", callerID );

        Console.WriteLine( str );

        WriteToFile( str );
    }

    /// <summary>
    ///     Write a message to console if the supplied condition is TRUE.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    public static void OnCondition( string message,
                                    bool condition = false,
                                    [CallerFilePath] string callerFilePath = "",
                                    [CallerMemberName] string callerMethod = "",
                                    [CallerLineNumber] int callerLine = 0 )
    {
        if ( !IsEnabled( LOG_DEBUG ) || !condition )
        {
            return;
        }

        CallerID callerID = MakeCallerID( callerFilePath, callerMethod, callerLine );

        var str = CreateMessage( $"{DEBUG_TAG}{message}", callerID );

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

        CallerID callerID = MakeCallerID( callerFilePath, callerMethod, callerLine );

        var message = $"CP::{GetTimeStampInfo()}:{GetCallerInfo( callerID )}";

        Console.WriteLine( message );

        WriteToFile( message );
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

    #endregion public methods

    // ------------------------------------------------------------------------

    #region private methods

    /// <summary>
    ///     Creates a debug/error/info message ready for dumping.
    /// </summary>
    /// <param name="formatString">The base message</param>
    /// <param name="cid">Stack trace info from the calling method/file.</param>
    /// <returns></returns>
    private static string CreateMessage( string formatString, CallerID cid )
    {
        var sb = new StringBuilder( GetTimeStampInfo() );

        sb.Append( " : " );
        sb.Append( GetCallerInfo( cid ) );
        sb.Append( " : " );

        if ( !string.IsNullOrEmpty( formatString ) )
        {
            sb.Append( formatString );
        }

        return sb.ToString();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="callerFilePath"></param>
    /// <param name="callerMethod"></param>
    /// <param name="callerLine"></param>
    /// <returns></returns>
    private static CallerID MakeCallerID( string callerFilePath, string callerMethod, int callerLine )
    {
        return new CallerID
        {
            fileName   = Path.GetFileNameWithoutExtension( callerFilePath ),
            methodName = callerMethod,
            lineNumber = callerLine
        };
    }

    /// <summary>
    ///     Returns a string holding the current time.
    /// </summary>
    private static string GetTimeStampInfo()
    {
        var c = new GregorianCalendar();

        return $"{c.GetHour( DateTime.Now )}"
             + $":{c.GetMinute( DateTime.Now )}"
             + $":{c.GetSecond( DateTime.Now )}"
             + $":{c.GetMilliseconds( DateTime.Now )}";
    }

    /// <summary>
    ///     Returns a string holding the calling filename, method and line number.
    /// </summary>
    private static string GetCallerInfo( CallerID cid )
    {
        return $"{cid.fileName}::{cid.methodName}::{cid.lineNumber}";
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
    private static bool IsEnabled( int traceLevel )
    {
        return traceLevel switch
               {
                   LOG_SYS
                       or LOG_DEBUG
                       or LOG_INFO
                       or LOG_ERROR => ( TraceLevel & traceLevel ) != 0,
                   _ => false
               };
    }

    #endregion private methods
}

/// <summary>
/// 
/// </summary>
[InterpolatedStringHandler]
[SuppressMessage( "ReSharper", "UnusedMember.Global" )]
public readonly ref struct LogInterpolatedStringHandler
{
    // Storage for the built-up string
    private readonly StringBuilder _builder;

    internal LogInterpolatedStringHandler( int literalLength, int formattedCount )
    {
        _builder = new StringBuilder( literalLength );
    }

    internal void AppendLiteral( string s )
    {
        _builder.Append( s );
    }

    internal void AppendFormatted<T>( T t )
    {
        _builder.Append( t );
    }

    public void AppendFormatted<T>( T t, string format ) where T : IFormattable
    {
        _builder.Append( t.ToString( format, null ) );
    }

    internal string GetFormattedText() => _builder.ToString();
}

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
