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

using Corelib.Lugh.Audio;
using JetBrains.Annotations;

namespace DesktopGLBackend.Audio.Mock;

/// <summary>
/// Audio stubs for use when Audio is disabled, or when Audio failed to initialise.
/// </summary>
[PublicAPI]
public class MockSound : ISound
{
    /// <inheritdoc />
    public long Play( float volume )
    {
        return 0;
    }

    /// <inheritdoc />
    public long Play( float volume, float pitch, float pan )
    {
        return 0;
    }

    /// <inheritdoc />
    public long Loop( float volume )
    {
        return 0;
    }

    /// <inheritdoc />
    public long Loop( float volume, float pitch, float pan )
    {
        return 0;
    }

    /// <inheritdoc />
    public void Stop()
    {
    }

    /// <inheritdoc />
    public void Pause()
    {
    }

    /// <inheritdoc />
    public void Resume()
    {
    }

    /// <inheritdoc />
    public void Stop( long soundId )
    {
    }

    /// <inheritdoc />
    public void Pause( long soundId )
    {
    }

    /// <inheritdoc />
    public void Resume( long soundId )
    {
    }

    /// <inheritdoc />
    public void SetLooping( long soundId, bool looping )
    {
    }

    /// <inheritdoc />
    public void SetPitch( long soundId, float pitch )
    {
    }

    /// <inheritdoc />
    public void SetVolume( long soundId, float volume )
    {
    }

    /// <inheritdoc />
    public void SetPan( long soundId, float pan, float volume )
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }

    public long Play()
    {
        return 0;
    }

    public long Loop()
    {
        return 0;
    }
}
