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


using System.Runtime.Serialization;

using LibGDXSharp.Gdx.Audio.MP3Sharp.Support;

using Exception = System.Exception;

namespace LibGDXSharp.Gdx.Audio.MP3Sharp;

/// <summary>
///     MP3SharpException is the base class for all API-level
///     exceptions thrown by MP3Sharp. To facilitate conversion and
///     common handling of exceptions from other domains, the class
///     can delegate some functionality to a contained Throwable instance.
/// </summary>
[Serializable]
public class Mp3SharpException : Exception
{
    public Mp3SharpException()
    {
    }

    public Mp3SharpException( string message )
        : base( message )
    {
    }

    public Mp3SharpException( string message, Exception? inner )
        : base( message, inner )
    {
    }

    protected Mp3SharpException( SerializationInfo info, StreamingContext context )
        : base( info, context )
    {
    }

    public void PrintStackTrace() => SupportClass.WriteStackTrace( this, Console.Error );

    public void PrintStackTrace( StreamWriter ps )
    {
        if ( InnerException == null )
        {
            SupportClass.WriteStackTrace( this, ps );
        }
        else
        {
            SupportClass.WriteStackTrace( InnerException, Console.Error );
        }
    }
}
