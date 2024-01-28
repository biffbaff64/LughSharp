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

namespace LibGDXSharp.Backends.Desktop.Utils;

public class DesktopGLApplicationLogger : IApplicationLogger
{
    public void Log( string tag, string message ) => Console.WriteLine( $@"[{tag}] {message}" );

    public void Log( string tag, string message, System.Exception exception )
    {
        Console.WriteLine( $@"[{tag}] {message}" );
        Console.WriteLine( exception.StackTrace );
    }

    public void Error( string tag, string message ) => Console.WriteLine( $@"[{tag}] {message}" );

    public void Error( string tag, string message, System.Exception exception )
    {
        Console.WriteLine( $@"[{tag}] {message}" );
        Console.WriteLine( exception.StackTrace );
    }

    public void Debug( string tag, string message ) => Console.WriteLine( $@"[{tag}] {message}" );

    public void Debug( string tag, string message, System.Exception exception )
    {
        Console.WriteLine( $@"[{tag}] {message}" );
        Console.WriteLine( exception.StackTrace );
    }
}
