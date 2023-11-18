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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

/// <summary>
/// The <tt>Decoder</tt> class encapsulates the details of decoding an MPEG audio frame.
/// </summary>
[PublicAPI]
public class Mp3Decoder
{
    // The first decoder error code.
    public const int DECODER_ERROR = 0x200;
    public const int UNKNOWN_ERROR = DECODER_ERROR + 0;

    // Layer not supported by the decoder.
    public const int UNSUPPORTED_LAYER = DECODER_ERROR + 1;

    // Illegal allocation in subband layer. Indicates a corrupt stream.
    public const int ILLEGAL_SUBBAND_ALLOCATION = DECODER_ERROR + 2;

    // ------------------------------------------

    /// <summary>
    /// The OutputBuffer instance that will receive the decoded PCM samples.
    /// Setting the output buffer will only take effect the next time
    /// <see cref="DecodeFrame"/> is called.
    /// </summary>
    public OutputBuffer? Output { get; set; }

    /// <summary>
    /// The sample frequency of the PCM samples output by this decoder.
    /// This typically corresponds to the sample rate encoded in the
    /// MPEG audio stream.
    /// </summary>
    public int OutputFrequency { get; set; }

    /// <summary>
    /// the number of channels of PCM samples output by this decoder. This
    /// usually corresponds to the number of channels in the MPEG audio stream,
    /// although it may differ. 1 channel for mono, 2 for stereo.
    /// </summary>
    public int NumOutputChannels { get; set; }

    // ------------------------------------------

    // Synthesis filter for the left channel.
    private SynthesisFilter? _filter1;

    // Sythesis filter for the right channel.
    private SynthesisFilter? _filter2;

    private LayerIIIDecoder? _l3decoder;
    private LayerIIDecoder?  _l2decoder;
    private LayerIDecoder?   _l1decoder;

    private bool _initialised;

    // ------------------------------------------

    /// <summary>
    /// Creates a new <code>Decoder</code> instance with default parameters.
    /// </summary>
    public Mp3Decoder()
    {
    }

    /// <summary>
    /// Decodes one frame from an MPEG audio bitstream.
    /// </summary>
    /// <param name="header"> The header describing the frame to decode. </param>
    /// <param name="stream"> The bistream that provides the bits for the body of the frame. </param>
    /// <returns> A SampleBuffer containing the decoded samples. </returns>
    public OutputBuffer? DecodeFrame( Header header, Bitstream stream )
    {
        if ( !_initialised )
        {
            Initialise( header );
        }

        var layer = header.HLayer;

        IFrameDecoder decoder = RetrieveDecoder( header, stream, layer );

        decoder.DecodeFrame();

        return Output;
    }

    public IFrameDecoder RetrieveDecoder( Header header, Bitstream stream, int layer )
    {
        IFrameDecoder? decoder = null;

        // REVIEW: allow channel output selection type
        // (LEFT, RIGHT, BOTH, DOWNMIX)
        switch ( layer )
        {
            case 3:
                _l3decoder ??= new LayerIIIDecoder();
                _l3decoder.Create( stream, header, _filter1, _filter2, Output, OutputChannels.BOTH_CHANNELS );

                decoder = _l3decoder;

                break;

            case 2:
                _l2decoder ??= new LayerIIDecoder();
                _l2decoder.Create( stream, header, _filter1, _filter2, Output, OutputChannels.BOTH_CHANNELS );

                decoder = _l2decoder;

                break;

            case 1:
                _l1decoder ??= new LayerIDecoder();
                _l1decoder.Create( stream, header, _filter1, _filter2, Output, OutputChannels.BOTH_CHANNELS );

                decoder = _l1decoder;

                break;
        }

        if ( decoder == null )
        {
            throw new DecoderException( UNSUPPORTED_LAYER );
        }

        return decoder;
    }

    private void Initialise( Header header )
    {
        // REVIEW: allow customizable scale factor
        var scalefactor = 32700.0f;

        var mode     = header.HMode;
        var channels = ( mode == Header.SINGLE_CHANNEL ? 1 : 2 );

        // set up output buffer if not set up by client.
        if ( Output == null )
        {
            throw new GdxRuntimeException( "Output buffer was not set." );
        }

        _filter1 = new SynthesisFilter( 0, scalefactor );

        // REVIEW: allow mono output for stereo
        if ( channels == 2 )
        {
            _filter2 = new SynthesisFilter( 1, scalefactor );
        }

        NumOutputChannels = channels;
        OutputFrequency   = header.Frequency();

        _initialised = true;
    }
}
