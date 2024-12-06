// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


namespace Corelib.Lugh.Core;

/// <summary>
/// An ILifecycleListener can be added to an Application via
/// Application.AddLifecycleListener(ILifecycleListener). It will receive
/// notification of Pause, Resume and Dispose events. This is mainly meant
/// to be used by extensions that need to manage resources based on the life-cycle.
/// Normally, application level development should rely on the ApplicationListener
/// interface. The methods will be invoked on the rendering thread. The methods
/// will be executed before the ApplicationListener methods are executed.
/// </summary>
[PublicAPI]
public interface ILifecycleListener : IDisposable
{
    /// <summary>
    /// Called when the <see cref="IApplication"/> is about to be paused.
    /// </summary>
    void Pause();

    /// <summary>
    /// Called when the <see cref="IApplication"/> is about to be resumed.
    /// </summary>
    void Resume();
}
