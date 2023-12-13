// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;

/// <summary>
/// This struct describes all error codes that can be thrown
/// in BistreamExceptions.
/// </summary>
[PublicAPI]
public struct BitstreamErrors
{
    public const int UNKNOWN_ERROR       = BITSTREAM_ERROR + 0;
    public const int UNKNOWN_SAMPLE_RATE = BITSTREAM_ERROR + 1;
    public const int STREA_ERROR         = BITSTREAM_ERROR + 2;
    public const int UNEXPECTED_EOF      = BITSTREAM_ERROR + 3;
    public const int STREAM_EOF          = BITSTREAM_ERROR + 4;
    public const int BITSTREAM_LAST      = 0x1ff;

    public const int BITSTREAM_ERROR = 0x100;
    public const int DECODER_ERROR   = 0x200;
}
