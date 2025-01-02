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

using LughSharp.Lugh.Utils;

namespace LughSharp.Lugh.Core;

///////////////////////////////////////////////////////////////////////////////
/// <summary>
/// An Application is the main entry point of your project. It sets up a window and
/// rendering surface and manages the different aspects of your application, namely
/// Graphics, Audio, Input and Files.
/// This interface should be implemented in an application class for each supported
/// backend.
/// Each application class has it's own startup and initialisation methods. Please
/// refer to their documentation for more information.
/// While game programmers are used to having a main loop, LughSharp employs a different
/// concept to accommodate the event based nature of Android applications a little more.
/// Your application logic must be implemented in a ApplicationListener which has
/// methods that get called by the Application when the application is created, resumed,
/// paused, disposed or rendered. As a developer you will simply implement the
/// ApplicationListener interface and fill in the functionality accordingly. The
/// ApplicationListener is provided to a concrete Application instance as a parameter to
/// the constructor or another initialization method. Please refer to the documentation of
/// the Application implementations for more information. Note that the ApplicationListener
/// can be provided to any Application implementation. This means that you only need to write
/// your program logic once and have it run on different platforms by passing it to a concrete
/// Application implementation.
/// The Application interface provides you with a set of modules for graphics, audio, input
/// and file i/o.
/// Graphics offers you various methods to output visuals to the screen.
/// Audio offers you various methods to output and record sound and music.
/// Input offers you various methods to poll user input from the keyboard, touch screen, mouse
/// and accelerometer. Additionally you can implement an InputProcessor and use it with
/// Input.setInputProcessor(InputProcessor) to receive input events.
/// Files offers you various methods to access internal and external files. An internal file is
/// a file that is stored near your application. On the desktop the classpath is first scanned for
/// the specified file. If that fails then the root directory of your application is used for a
/// look up. External files are resources you create in your application and write to an external
/// storage. On the desktop external files are written to a users home directory. If you know what
/// you are doing you can also specify absolute file names. Absolute filenames are not portable,
/// so take great care when using this feature.
/// Net offers you various methods to perform network operations, such as performing HTTP requests,
/// or creating server and client sockets for more elaborate network programming.
/// The Application also has a set of methods that you can use to query specific information such
/// as the operating system the application is currently running on and so forth. This allows you
/// to have operating system dependent code paths. It is however not recommended to use this
/// facilities.
/// </summary>
///////////////////////////////////////////////////////////////////////////////
[PublicAPI]
public interface IApplication
{
    /// <summary>
    /// What <see cref="Platform.ApplicationType"/> the application has.
    /// </summary>
    Platform.ApplicationType AppType { get; set; }

    /// <summary>
    ///
    /// </summary>
    IClipboard? Clipboard { get; set; }

    /// <summary>
    /// Returns the Android API level on Android, the major OS version on iOS (5, 6, 7, ..), or 0 on the desktop.
    /// </summary>
    int GetVersion();

    /// <summary>
    /// Returns the <see cref="IPreferences"/> instance of this Application. It can be
    /// used to store application settings across runs.
    /// </summary>
    /// <param name="name"> the name of the preferences, must be useable as a file name. </param>
    /// <returns> The preferences. </returns>
    IPreferences GetPreferences( string name );

    /// <summary>
    /// Adds a new <see cref="ILifecycleListener"/> to the application. This can be
    /// used by extensions to hook into the lifecycle more easily.
    /// The <see cref="IApplicationListener"/> methods are sufficient for application
    /// level development.
    /// </summary>
    void AddLifecycleListener( ILifecycleListener listener );

    /// <summary>
    /// Removes the specified <see cref="ILifecycleListener"/>
    /// </summary>
    void RemoveLifecycleListener( ILifecycleListener listener );

    /// <summary>
    /// Posts a <see cref="IRunnable"/> to the event queue.
    /// </summary>
    void PostRunnable( IRunnable.Runnable runnable );

    /// <summary>
    /// Schedule an exit from the application. On android, this will cause a call to
    /// <see cref="IApplicationListener.Pause()"/> and <see cref="IDisposable.Dispose()"/>
    /// some time in the future.
    /// <para>
    /// It will not immediately finish your application.
    ///</para>
    /// <para>
    /// On iOS this should be avoided in production as it breaks Apples guidelines
    ///</para>
    /// </summary>
    void Exit();
}