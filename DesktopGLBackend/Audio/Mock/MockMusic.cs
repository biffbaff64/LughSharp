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

using Corelib.LibCore.Audio;
using JetBrains.Annotations;

namespace DesktopGLBackend.Audio.Mock;

/// <summary>
/// Audio stubs for use when Audio is disabled, or when Audio failed to initialise.
/// </summary>
[PublicAPI]
public class MockMusic : IMusic
{
    /// <inheritdoc />
    public void Play()
    {
    }

    /// <inheritdoc />
    public void Pause()
    {
    }

    /// <inheritdoc />
    public void Stop()
    {
    }

    /// <inheritdoc />
    public bool IsPlaying { get; set; } = false;

    /// <inheritdoc />
    public bool IsLooping { get; set; } = false;

    /// <inheritdoc />
    public void SetVolume( float volume )
    {
    }

    /// <inheritdoc />
    public float GetVolume()
    {
        return 0;
    }

    /// <inheritdoc />
    public void SetPan( float pan, float volume )
    {
    }

    /// <inheritdoc />
    public void SetPosition( float position )
    {
    }

    /// <inheritdoc />
    public float GetPosition()
    {
        return 0;
    }

    /// <inheritdoc />
    public IMusic.IOnCompletionListener? OnCompletionListener { get; set; }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}
