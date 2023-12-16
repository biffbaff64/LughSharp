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

namespace LibGDXSharp.Backends.Desktop.Audio.Mock;

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
    public void SetLooping( bool isLooping )
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
    public float GetVolume() => 0;

    /// <inheritdoc />
    public void SetPan( float pan, float volume )
    {
    }

    /// <inheritdoc />
    public void SetPosition( float position )
    {
    }

    /// <inheritdoc />
    public float GetPosition() => 0;

    /// <inheritdoc />
    public void setOnCompletionListener( IMusic.IOnCompletionListener listener )
    {
    }

    /// <inheritdoc />
    public void Dispose()
    {
    }
}