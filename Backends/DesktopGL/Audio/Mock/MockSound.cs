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

public class MockSound : ISound
{

    /// <inheritdoc />
    public long Play( float volume ) => 0;

    /// <inheritdoc />
    public long Play( float volume, float pitch, float pan ) => 0;

    /// <inheritdoc />
    public long Loop( float volume ) => 0;

    /// <inheritdoc />
    public long Loop( float volume, float pitch, float pan ) => 0;

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
    public long Play() => 0;

    /// <inheritdoc />
    public long Loop() => 0;
}
