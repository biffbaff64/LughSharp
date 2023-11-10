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
/// The ApplicationLogger provides an interface for a LibGDX Application to
/// log messages and exceptions. A default implementations is provided for
/// each backend, custom implementations can be provided and set using
/// IApplication.ApplicationLogger = ApplicationLogger
/// </summary>
[PublicAPI]
public interface IApplicationLogger
{
    /// <summary>
    /// Logs a message with a tag.
    /// </summary>
    void Log( string tag, string message );

    /// <summary>
    /// Logs a message and exception with a tag.
    /// </summary>
    void Log( string tag, string message, System.Exception exception );

    /// <summary>
    /// Logs an error message with a tag.
    /// </summary>
    void Error( string tag, string message );

    /// <summary>
    /// Logs an error message and exception with a tag.
    /// </summary>
    void Error( string tag, string message, System.Exception exception );

    /// <summary>
    /// Logs a debug message with a tag.
    /// </summary>
    void Debug( string tag, string message );

    /// <summary>
    /// Logs a debug message and exception with a tag.
    /// </summary>
    void Debug( string tag, string message, System.Exception exception );
}