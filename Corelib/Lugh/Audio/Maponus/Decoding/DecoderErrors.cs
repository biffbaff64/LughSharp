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


namespace Corelib.Lugh.Audio.Maponus.Decoding;

/// <summary>
/// Defines error codes for decoder-related errors.
/// </summary>
/// <remarks>
/// This struct provides constant error codes that are used to indicate specific errors
/// encountered during decoding. The error codes are built upon the base error code
/// defined in <see cref="BitstreamErrors.DECODER_ERROR"/>.
/// </remarks>
[PublicAPI]
public struct DecoderErrors
{
    /// <summary>
    /// Indicates an unknown decoder error.
    /// </summary>
    public const int UNKNOWN_ERROR = BitstreamErrors.DECODER_ERROR + 0;

    /// <summary>
    /// Indicates that the layer specified in the bitstream is unsupported by the decoder.
    /// </summary>
    public const int UNSUPPORTED_LAYER = BitstreamErrors.DECODER_ERROR + 1;
}
