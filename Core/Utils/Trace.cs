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

using File = System.IO.File;

namespace LibGDXSharp.Utils;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public struct CallerID
{
    public string fileName;
    public string methodName;
    public int    lineNumber;
}

public static class Trace
{
    public const int LogNone  = 0;
    public const int LogDebug = 1;
    public const int LogInfo  = 2;
    public const int LogError = 3;

    public static bool EnableWriteToFile { get; set; } = false;
    public static int  LogLevel          { get; set; } = LogNone;

    private const string DebugTag = "[Debug] ";
    private const string InfoTag  = "[Info ] ";
    private const string ErrorTag = "[ERROR] ";

    private static string _debugFilePath = "";
    private static string _debugFileName = "";

    /// <summary>
    /// Default initialisation method.
    /// </summary>
    /// <param name="logLevel"></param>
    /// <param name="enableWriteToFile"></param>
    /// <param name="filename"></param>
    public static void Initialise( int logLevel = LogNone, bool enableWriteToFile = false, string filename = "" )
    {
        LogLevel          = logLevel;
        EnableWriteToFile = enableWriteToFile;

        if ( EnableWriteToFile )
        {
            OpenDebugFile( filename, true );
        }
    }

    /// <summary>
    /// Write a debug string to logcat or console.
    /// The string can contain format options.
    /// </summary>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    [SuppressMessage( "ReSharper", "InvalidXmlDocComment" )]
    public static void Dbg( [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0,
                            string message = "",
                            params object[] args )
    {
        if ( LogLevel != LogDebug )
        {
            return;
        }

        message = string.Join( DebugTag, message );

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
    /// Writes a Debug message, but adds a divider line before and after the message.
    /// </summary>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    [SuppressMessage( "ReSharper", "InvalidXmlDocComment" )]
    public static void BoxedDbg( [CallerFilePath] string callerFilePath = "",
                                 [CallerMemberName] string callerMethod = "",
                                 [CallerLineNumber] int callerLine = 0,
                                 string message = "",
                                 params object[] args )
    {
        if ( LogLevel == LogDebug )
        {
            return;
        }

        Divider();

        message = string.Join( DebugTag, message );

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
    /// Write an error string to logcat or console.
    /// </summary>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    [SuppressMessage( "ReSharper", "InvalidXmlDocComment" )]
    public static void Err( [CallerFilePath] string callerFilePath = "",
                            [CallerMemberName] string callerMethod = "",
                            [CallerLineNumber] int callerLine = 0,
                            string message = "",
                            params object[] args )
    {
        if ( LogLevel != LogDebug )
        {
            return;
        }

        message = string.Join( ErrorTag, message );

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
    /// Write a message to logcat or console if the supplied condition is TRUE.
    /// </summary>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="message">The string to write.</param>
    /// <param name="args">Optional extra argumnts for use in format strings.</param>
    [SuppressMessage( "ReSharper", "InvalidXmlDocComment" )]
    public static void _assert( [CallerFilePath] string callerFilePath = "",
                                [CallerMemberName] string callerMethod = "",
                                [CallerLineNumber] int callerLine = 0,
                                bool condition = false,
                                string message = "",
                                params object[] args )
    {
        if ( ( LogLevel == LogDebug ) && condition )
        {
            message = string.Join( DebugTag, message );

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
    }

    /// <summary>
    /// Writes a debug message consisting solely of the following:-
    /// - Current time and date.
    /// - Calling Class/method/line number information.
    /// </summary>
    [SuppressMessage( "ReSharper", "InvalidXmlDocComment" )]
    public static void CheckPoint( [CallerFilePath] string callerFilePath = "",
                                   [CallerMemberName] string callerMethod = "",
                                   [CallerLineNumber] int callerLine = 0 )
    {
        if ( LogLevel != LogDebug )
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
        sb.Append( GetFileInfo( callerID ) );

        Console.WriteLine( sb.ToString() );

        WriteToFile( sb.ToString() );
    }

    /// <summary>
    /// This method does the same as Trace#dbg(string, params object[] args)/>
    /// but does not output time and date information.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public static void Info( string message, params object[] args )
    {
        if ( LogLevel != LogDebug )
        {
            return;
        }

        message = string.Join( InfoTag, message );

        var sb = new StringBuilder( message );

        if ( args.Length > 0 )
        {
            foreach ( var arg in args )
            {
                sb.Append( ' ' );
                sb.Append( arg );
            }
        }

        message = string.Join( DebugTag, message );

        Console.WriteLine( message );

        WriteToFile( message );
    }

    /// <summary>
    /// Creates a debug/error/info message ready for dumping.
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
        sb.Append( GetFileInfo( cid ) );
        sb.Append( " : " );

        if ( !string.IsNullOrEmpty( formatString ) )
        {
            sb.Append( formatString );
        }

        if ( args.Length > 0 )
        {
            foreach ( var arg in args )
            {
                sb.Append( ' ' );
                sb.Append( arg );
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Returns a string holding the current time.
    /// </summary>
    private static string GetTimeStampInfo()
    {
        var c  = new GregorianCalendar();
        var sb = new StringBuilder();

        sb.Append( c.GetHour( DateTime.Now ) ).Append( ':' );
        sb.Append( c.GetMinute( DateTime.Now ) ).Append( ':' );
        sb.Append( c.GetSecond( DateTime.Now ) ).Append( ':' );
        sb.Append( c.GetMilliseconds( DateTime.Now ) );

        return sb.ToString();
    }

    /// <summary>
    /// Returns a string holding the calling filename, method and line number.
    /// </summary>
    private static string GetFileInfo( CallerID cid )
    {
        var sb = new StringBuilder();

        sb.Append( cid.fileName ).Append( "::" );
        sb.Append( cid.methodName ).Append( "::" );
        sb.Append( cid.lineNumber );

        return sb.ToString();
    }

    /// <summary>
    /// Adds a dividing line to text output.
    /// </summary>
    /// <param name="ch">The character to use, default is '-'</param>
    /// <param name="length">The line length, default is 80.</param>
    public static void Divider( char ch = '-', int length = 80 )
    {
        var sb = new StringBuilder( DebugTag );

        for ( var i = 0; i < length; i++ )
        {
            sb.Append( ch );
        }

        Console.WriteLine( sb.ToString() );
    }

    /// <summary>
    /// Opens a physical file for writing copies of debug messages to.
    /// </summary>
    /// <param name="fileName">
    /// The filename. This should be filename only,
    /// and the file will be created in the working directory.
    /// </param>
    /// <param name="deleteExisting">
    /// True to delete existing copies of the file.
    /// False to append to existing file.
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
            _debugFilePath =  Environment.GetFolderPath( Environment.SpecialFolder.UserProfile );
            _debugFilePath += "//.prefs//";
        }

        _debugFileName = fileName;

        using var fs = File.Create( _debugFilePath + _debugFileName );

        var dateTime = DateTime.Now;

        var divider =
            new UTF8Encoding( true )
                .GetBytes( "-----------------------------------------------------" );

        var time = new UTF8Encoding( true ).GetBytes( dateTime.ToShortTimeString() );

        fs.Write( divider, 0, divider.Length );
        fs.Write( time, 0, time.Length );
        fs.Write( divider, 0, divider.Length );

        fs.Close();
    }

    /// <summary>
    /// Writes text to the logFile, if it exists.
    /// </summary>
    /// <param name="text">String holding the text to write.</param>
    private static void WriteToFile( string text )
    {
        if ( File.Exists( _debugFilePath + _debugFileName ) )
        {
            using var fs = File.Open( _debugFilePath + _debugFileName, FileMode.Append );

            var debugLine = new UTF8Encoding( true ).GetBytes( text + "\n" );
            fs.Write( debugLine, 0, debugLine.Length );

            fs.Close();
        }
    }
}