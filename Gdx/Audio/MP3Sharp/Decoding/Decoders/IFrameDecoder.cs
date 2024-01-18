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

namespace LibGDXSharp.Audio.MP3Sharp.Decoders;

/// <summary>
///     Implementations of FrameDecoder are responsible for decoding
///     an MPEG audio frame.
/// </summary>

//TODO: the interface currently is too thin. There should be
// methods to specify the output buffer, the synthesis filters and
// possibly other objects used by the decoder. 
public interface IFrameDecoder
{
    /// <summary>
    ///     Decodes one frame of MPEG audio.
    /// </summary>
    void DecodeFrame();
}
