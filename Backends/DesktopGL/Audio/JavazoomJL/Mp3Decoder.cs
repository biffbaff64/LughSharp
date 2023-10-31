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

namespace LibGDXSharp.Backends.Desktop.Audio;

/// <summary>
/// The <tt>Decoder</tt> class encapsulates the details of decoding an MPEG audio frame.
/// </summary>
public class Mp3Decoder
{
    // The OutputBuffer instance that will receive the decoded PCM samples.
    private OutputBuffer _output;

    // Synthesis filter for the left channel.
    private SynthesisFilter _filter1;

    // Sythesis filter for the right channel.
    private SynthesisFilter _filter2;

    private LayerIIIDecoder _l3decoder;
    private LayerIIDecoder  _l2decoder;
    private LayerIDecoder   _l1decoder;

    private int  _outputFrequency;
    private int  _outputChannels;
    private bool _initialised;

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
    public OutputBuffer DecodeFrame( Header header, Bitstream stream )
    {
        if ( !_initialised )
        {
            Initialise( header );
        }

        int layer = header.Layer();

        FrameDecoder decoder = RetrieveDecoder( header, stream, layer );

        decoder.DecodeFrame();

        return _output;
    }

    /**
     * Changes the output buffer. This will take effect the next time decodeFrame() is called.
     */
    public void SetOutputBuffer( OutputBuffer outbuf )
    {
        _output = outbuf;
    }

    /**
     * Retrieves the sample frequency of the PCM samples output by this decoder. This typically corresponds to the sample rate
     * encoded in the MPEG audio stream.
     *
     * @param the sample rate (in Hz) of the samples written to the output buffer when decoding.
     */
    public int GetOutputFrequency()
    {
        return _outputFrequency;
    }

    /**
     * Retrieves the number of channels of PCM samples output by this decoder. This usually corresponds to the number of channels
     * in the MPEG audio stream, although it may differ.
     *
     * @return The number of output channels in the decoded samples: 1 for mono, or 2 for stereo.
     *
     */
    public int GetOutputChannels()
    {
        return _outputChannels;
    }

    protected DecoderException NewDecoderException( int errorcode )
    {
        return new DecoderException( errorcode, null );
    }

    protected DecoderException NewDecoderException( int errorcode, Throwable throwable )
    {
        return new DecoderException( errorcode, throwable );
    }

    public FrameDecoder RetrieveDecoder( Header header, Bitstream stream, int layer )
    {
        FrameDecoder decoder = null;

        // REVIEW: allow channel output selection type
        // (LEFT, RIGHT, BOTH, DOWNMIX)
        switch ( layer )
        {
            case 3:
                if ( _l3decoder == null )
                {
                    _l3decoder = new LayerIIIDecoder
                        ( stream, header, _filter1, _filter2, _output, OutputChannels.BOTH_CHANNELS );
                }

                decoder = _l3decoder;

                break;

            case 2:
                if ( _l2decoder == null )
                {
                    _l2decoder = new LayerIIDecoder();
                    _l2decoder.Create( stream, header, _filter1, _filter2, _output, OutputChannels.BOTH_CHANNELS );
                }

                decoder = _l2decoder;

                break;

            case 1:
                if ( l1decoder == null )
                {
                    l1decoder = new LayerIDecoder();
                    l1decoder.create( stream, header, filter1, filter2, output, OutputChannels.BOTH_CHANNELS );
                }

                decoder = l1decoder;

                break;
        }

        if ( decoder == null )
        {
            throw NewDecoderException( UNSUPPORTED_LAYER, null );
        }

        return decoder;

    }

    private void Initialise( Header header )
    {

    // REVIEW: allow customizable scale factor
    float scalefactor = 32700.0f;

    int mode = header.mode();
    header.layer();
    int channels = mode == Header.SINGLE_CHANNEL ? 1 : 2;

        // set up output buffer if not set up by client.
        if ( output == null ) throw new RuntimeException( "Output buffer was not set." );

    filter1 = new SynthesisFilter( 0, scalefactor, null );

    // REVIEW: allow mono output for stereo
    if ( channels == 2 ) filter2 = new SynthesisFilter( 1, scalefactor, null );

    outputChannels = channels;
    outputFrequency = header.frequency();

    initialized = true;
}

/**
 * The first decoder error code. See the {@link DecoderErrors DecoderErrors} interface for other decoder error codes.
 */
static public final int DECODER_ERROR = 0x200;
static public final int UNKNOWN_ERROR = DECODER_ERROR + 0;
/**
 * Layer not supported by the decoder.
 */
static public final int UNSUPPORTED_LAYER = DECODER_ERROR + 1;
/**
 * Illegal allocation in subband layer. Indicates a corrupt stream.
 */
static public final int ILLEGAL_SUBBAND_ALLOCATION = DECODER_ERROR + 2;

}
