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

namespace LibGDXSharp.Audio.MP3Sharp;

/// <summary>
///     This interface provides constants describing the error
///     codes used by the Decoder to indicate errors.
/// </summary>
public struct DecoderErrors
{
    public const int UNKNOWN_ERROR     = BitstreamErrors.DECODER_ERROR + 0;
    public const int UNSUPPORTED_LAYER = BitstreamErrors.DECODER_ERROR + 1;
}
