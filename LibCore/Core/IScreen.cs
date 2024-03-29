﻿// ///////////////////////////////////////////////////////////////////////////////
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


namespace LughSharp.LibCore.Core;

/// <summary>
///     Represents one of many application screens, such as a main menu,
///     a settings menu, the game screen and so on.
///     Note that Dispose() is not called automatically.
/// </summary>
[PublicAPI]
public interface IScreen
{
    /// <summary>
    ///     Called when this screen becomes the current screen for
    ///     a <see cref="Game" />.
    /// </summary>
    void Show();

    /// <summary>
    ///     Called when the screen should render itself.
    /// </summary>
    /// <param name="delta"> The time in seconds since the last render. </param>
    void Render( float delta );

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Resize(int, int)" />
    void Resize( int width, int height );

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Pause()" />
    void Pause();

    /// <summary>
    /// </summary>
    /// <seealso cref="IApplicationListener.Resume()" />
    void Resume();

    /// <summary>
    ///     Called when this screen is no longer the current screen for
    ///     a <see cref="Game" />.
    /// </summary>
    void Hide();

    /// <summary>
    ///     Called when this screen should release all resources.
    /// </summary>
    void Dispose();
}
