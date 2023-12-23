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
/// Encapsulates the details of decoding an MPEG audio frame.
/// </summary>
[PublicAPI]
public class Decoder
{
    private readonly static Parameters DecoderDefaultParams = new();

    private Equalizer?       _equalizer;
    private SynthesisFilter? _leftChannelFilter;
    private SynthesisFilter? _rightChannelFilter;
    private LayerIDecoder?   _l1Decoder;
    private LayerIIDecoder?  _l2Decoder;
    private LayerIIIDecoder? _l3Decoder;
    private ABuffer?         _output;

    private bool _isInitialized;
    private int  _outputChannels;
    private int  _outputFrequency;

    /// <summary>
    /// Creates a new Decoder instance with custom parameters.
    /// </summary>
    public Decoder( Parameters? params0 = null )
    {
        _equalizer = new Equalizer();

        params0 ??= DecoderDefaultParams;

        Equalizer? eq = params0.InitialEqualizerSettings;

        if ( eq != null )
        {
            _equalizer.FromEqualizer = eq;
        }
    }

    public static Parameters DefaultParams => ( Parameters )DecoderDefaultParams.Clone();

    public virtual Equalizer? Equalizer
    {
        set
        {
            value ??= Equalizer.PassThruEq;

            _equalizer!.FromEqualizer = value;

            var factors = _equalizer.BandFactors;

            if ( _leftChannelFilter != null )
            {
                _leftChannelFilter.Eq = factors;
            }

            if ( _rightChannelFilter != null )
            {
                _rightChannelFilter.Eq = factors;
            }
        }
    }

    /// <summary>
    /// Changes the output buffer. This will take effect the next time
    /// decodeFrame() is called.
    /// </summary>
    public virtual ABuffer OutputBuffer
    {
        set => _output = value;
    }

    /// <summary>
    /// Retrieves the sample frequency of the PCM samples output
    /// by this decoder. This typically corresponds to the sample
    /// rate encoded in the MPEG audio stream.
    /// </summary>
    public virtual int OutputFrequency => _outputFrequency;

    /// <summary>
    /// Retrieves the number of channels of PCM samples output by
    /// this decoder. This usually corresponds to the number of
    /// channels in the MPEG audio stream.
    /// </summary>
    public virtual int OutputChannels => _outputChannels;

    /// <summary>
    /// Retrieves the maximum number of samples that will be written to
    /// the output buffer when one frame is decoded. This can be used to
    /// help calculate the size of other buffers whose size is based upon
    /// the number of samples written to the output buffer. NB: this is
    /// an upper bound and fewer samples may actually be written, depending
    /// upon the sample rate and number of channels.
    /// </summary>
    public virtual int OutputBlockSize => ABuffer.OBUFFERSIZE;

    /// <summary>
    /// Decodes one frame from an MPEG audio bitstream.
    /// </summary>
    /// <param name="header">
    /// Header describing the frame to decode.
    /// </param>
    /// <param name="stream">
    /// Bistream that provides the bits for the body of the frame.
    /// </param>
    /// <returns>
    /// A SampleBuffer containing the decoded samples.
    /// </returns>
    public virtual ABuffer? DecodeFrame( Header header, Bitstream stream )
    {
        if ( !_isInitialized )
        {
            Initialize( header );
        }

        var layer = header.Layer();

        _output?.ClearBuffer();

        IFrameDecoder decoder = RetrieveDecoder( header, stream, layer );

        decoder.DecodeFrame();
        _output?.WriteBuffer( 1 );

        return _output;
    }

    protected virtual DecoderException NewDecoderException( int errorcode )
    {
        return new DecoderException( errorcode, null );
    }

    protected virtual DecoderException NewDecoderException( int errorcode, System.Exception? throwable )
    {
        return new DecoderException( errorcode, throwable );
    }

    protected virtual IFrameDecoder RetrieveDecoder( Header header, Bitstream stream, int layer )
    {
        IFrameDecoder? decoder = null;

        // TODO: allow channel output selection type
        // (LEFT, RIGHT, BOTH, DOWNMIX)
        switch ( layer )
        {
            case 3:
                _l3Decoder ??= new LayerIIIDecoder( stream,
                                                  header,
                                                  _leftChannelFilter,
                                                  _rightChannelFilter,
                                                  _output,
                                                  ( int )OutputChannelsEnum.BothChannels );

                decoder = _l3Decoder;

                break;

            case 2:
                if ( _l2Decoder == null )
                {
                    _l2Decoder = new LayerIIDecoder();

                    _l2Decoder.Create( stream,
                                       header,
                                       _leftChannelFilter,
                                       _rightChannelFilter,
                                       _output,
                                       ( int )OutputChannelsEnum.BothChannels );
                }

                decoder = _l2Decoder;

                break;

            case 1:
                if ( _l1Decoder == null )
                {
                    _l1Decoder = new LayerIDecoder();

                    _l1Decoder.Create( stream,
                                       header,
                                       _leftChannelFilter,
                                       _rightChannelFilter,
                                       _output,
                                       ( int )OutputChannelsEnum.BothChannels );
                }

                decoder = _l1Decoder;

                break;
        }

        if ( decoder == null )
        {
            throw NewDecoderException( DecoderErrors.UNSUPPORTED_LAYER, null );
        }

        return decoder;
    }

    private void Initialize( Header header )
    {
        // TODO: allow customizable scale factor
        const float SCALEFACTOR = 32700.0f;

        var channels = header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;

        // set up output buffer if not set up by client.
        _output ??= new SampleBuffer( header.Frequency(), channels );

        var factors = _equalizer?.BandFactors;

        _leftChannelFilter = new SynthesisFilter( 0, SCALEFACTOR, factors );

        if ( channels == 2 )
        {
            _rightChannelFilter = new SynthesisFilter( 1, SCALEFACTOR, factors );
        }

        _outputChannels  = channels;
        _outputFrequency = header.Frequency();

        _isInitialized = true;
    }

    /// <summary>
    /// The Params class presents the customizable aspects of the decoder. Instances of
    /// this class are not thread safe.
    /// </summary>
    [PublicAPI]
    public class Parameters : ICloneable
    {
        public virtual OutputChannels? OutputChannels { get; set; }

        /// <summary>
        /// Retrieves the equalizer settings that the decoder's equalizer will be initialized from.
        /// The Equalizer instance returned cannot be changed in real time to affect the decoder output
        /// as it is used only to initialize the decoders EQ settings. To affect the decoder's output
        /// in realtime, use the Equalizer returned from the getEqualizer() method on the decoder.
        /// </summary>
        /// <returns>
        /// The Equalizer used to initialize the EQ settings of the decoder.
        /// </returns>
        public virtual Equalizer? InitialEqualizerSettings { get; } = null;

        public object Clone()
        {
            try
            {
                return MemberwiseClone();
            }
            catch ( System.Exception ex )
            {
                throw new ApplicationException( this + ": " + ex );
            }
        }
    }
}
