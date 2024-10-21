// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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

using Corelib.LibCore.Audio.Maponus.Decoding.Decoders;
using Exception = System.Exception;
using OutputChannelsEnum = Corelib.LibCore.Audio.Maponus.Decoding.OutputChannels.OutputChannelsEnum;

namespace Corelib.LibCore.Audio.Maponus.Decoding;

/// <summary>
/// Encapsulates the details of decoding an MPEG audio frame.
/// </summary>
[PublicAPI]
public partial class Decoder
{
    private const float DEFAULT_SCALE_FACTOR = 32700.0f;

    private static readonly Parameters _decoderDefaultParams = new();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private readonly Equalizer? _equalizer;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    private bool             _isInitialized;
    private LayerIDecoder?   _l1Decoder;
    private LayerIIDecoder?  _l2Decoder;
    private LayerIIIDecoder? _l3Decoder;
    private SynthesisFilter? _leftChannelFilter;
    private AudioBase?       _output;
    private int              _outputChannels;
    private int              _outputFrequency;
    private SynthesisFilter? _rightChannelFilter;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Creates a new Decoder instance with custom parameters.
    /// </summary>
    public Decoder( Parameters? parameters = null )
    {
        _equalizer = new Equalizer();

        parameters ??= _decoderDefaultParams;

        var eq = parameters.InitialEqualizerSettings;

        if ( eq != null )
        {
            _equalizer.SetFromEqualizer = eq;
        }
    }

    /// <summary>
    /// Gets or sets the Equalizer.
    /// If set to null, it defaults to <see cref="Equalizer.PassThruEq"/>. When set, it
    /// updates the associated filters with the new equalizer's band factors.
    /// </summary>
    public virtual Equalizer? Equalizer
    {
        get => _equalizer;
        set
        {
            // If the value is null, set it to a default pass-through equalizer
            value ??= Equalizer.PassThruEq;

            // Ensure _equalizer is not null and set its FromEqualizer property to the new value
            if ( _equalizer != null )
            {
                _equalizer.SetFromEqualizer = value;

                // Get the band factors from the new equalizer
                var factors = _equalizer.BandFactors;

                // Update the left channel filter's equalizer settings if the filter exists
                if ( _leftChannelFilter != null )
                {
                    _leftChannelFilter.Eq = factors;
                }

                // Update the right channel filter's equalizer settings if the filter exists
                if ( _rightChannelFilter != null )
                {
                    _rightChannelFilter.Eq = factors;
                }
            }
        }
    }

    /// <summary>
    /// Changes the output buffer. This will take effect the next time
    /// decodeFrame() is called.
    /// </summary>
    public virtual AudioBase? OutputBuffer
    {
        get => _output;
        set => _output = value;
    }

    /// <summary>
    /// Gets or sets the scale factor. This property determines the scaling applied
    /// to a particular element or calculation.
    /// By default, it is set to <see cref="DEFAULT_SCALE_FACTOR"/>.
    /// </summary>
    public float ScaleFactor { get; set; } = DEFAULT_SCALE_FACTOR;

    /// <summary>
    /// Retrieves the sample frequency of the PCM samples output
    /// by this decoder. This typically corresponds to the sample
    /// rate encoded in the MPEG audio stream.
    /// </summary>
    public virtual int OutputFrequency => _outputFrequency;

    /// <summary>
    /// Retrieves the number of channels of PCM samples output by this decoder.
    /// This usually corresponds to the number of channels in the MPEG audio stream.
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
    public virtual int OutputBlockSize => AudioBase.OBUFFERSIZE;

    /// <summary>
    /// Gets the default parameters for the decoder. This property returns a clone
    /// of the static default parameters, ensuring that changes to the returned
    /// object do not affect the original default parameters.
    /// </summary>
    /// <remarks>
    /// This property ensures that each call returns a new instance, preventing
    /// modifications to the default parameters from unintended side effects.
    /// </remarks>
    /// <value>
    /// A clone of the default <see cref="Parameters"/> object for the decoder.
    /// </value>
    /// <exception cref="InvalidCastException">
    /// Thrown if the cloning process does not return an object of type <see cref="Parameters"/>.
    /// </exception>
    public static Parameters DefaultParams => ( Parameters ) _decoderDefaultParams.Clone();

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
    public virtual AudioBase? DecodeFrame( Header header, Bitstream stream )
    {
        if ( !_isInitialized )
        {
            Initialise( header );
        }

        var layer = header.Layer();

        _output?.ClearBuffer();

        var decoder = RetrieveDecoder( header, stream, layer );

        decoder.DecodeFrame();
        _output?.WriteBuffer( 1 );

        return _output;
    }

    /// <summary>
    /// Creates a new <see cref="DecoderException"/> with the specified error code.
    /// </summary>
    /// <param name="errorcode">The error code representing the specific decoder error.</param>
    /// <returns>
    /// A new instance of <see cref="DecoderException"/> initialized with the specified error code.
    /// </returns>
    /// <remarks>
    /// This method can be overridden in derived classes to provide custom exception handling
    /// based on the error code.
    /// </remarks>
    protected virtual DecoderException NewDecoderException( int errorcode )
    {
        return new DecoderException( errorcode, null );
    }

    /// <summary>
    /// Creates a new <see cref="DecoderException"/> with the specified error code
    /// and a nested exception.
    /// </summary>
    /// <param name="errorcode">The error code representing the specific decoder error.</param>
    /// <param name="throwable">
    /// The exception that caused the decoder error, if any. This parameter can be null.
    /// </param>
    /// <returns>
    /// A new instance of <see cref="DecoderException"/> initialized with the specified error
    /// code and nested exception.
    /// </returns>
    /// <remarks>
    /// This method can be overridden in derived classes to provide custom exception handling
    /// that includes the original cause of the error.
    /// </remarks>
    protected virtual DecoderException NewDecoderException( int errorcode, Exception? throwable )
    {
        return new DecoderException( errorcode, throwable );
    }

    /// <summary>
    /// Retrieves the appropriate frame decoder based on the specified MPEG layer.
    /// </summary>
    /// <param name="header">The header information of the audio frame.</param>
    /// <param name="stream">The bitstream containing the audio data.</param>
    /// <param name="layer">The MPEG layer of the audio frame (1, 2, or 3).</param>
    /// <returns>An <see cref="IFrameDecoder"/> instance corresponding to the specified MPEG layer.</returns>
    /// <exception cref="DecoderException">
    /// Thrown when the specified layer is not supported or if the decoder cannot be retrieved.
    /// </exception>
    /// <remarks>
    /// This method initializes and returns a frame decoder based on the MPEG layer.
    /// If the decoder for the specified layer is already initialized, it will return the existing instance.
    /// Otherwise, it will create a new instance of the appropriate decoder.
    /// </remarks>
    protected virtual IFrameDecoder RetrieveDecoder( Header header, Bitstream stream, int layer )
    {
        IFrameDecoder? decoder = null;

        // TODO: allow channel output selection type (LEFT, RIGHT, BOTH, DOWNMIX)
        switch ( layer )
        {
            case 3:
            {
                _l3Decoder ??= new LayerIIIDecoder( stream,
                                                    header,
                                                    _leftChannelFilter,
                                                    _rightChannelFilter,
                                                    _output,
                                                    ( int ) OutputChannelsEnum.BothChannels );

                decoder = _l3Decoder;

                break;
            }

            case 2:
            {
                if ( _l2Decoder == null )
                {
                    _l2Decoder = new LayerIIDecoder();

                    _l2Decoder.Create( stream,
                                       header,
                                       _leftChannelFilter,
                                       _rightChannelFilter,
                                       _output,
                                       ( int ) OutputChannelsEnum.BothChannels );
                }

                decoder = _l2Decoder;

                break;
            }

            case 1:
            {
                if ( _l1Decoder == null )
                {
                    _l1Decoder = new LayerIDecoder();

                    _l1Decoder.Create( stream,
                                       header,
                                       _leftChannelFilter,
                                       _rightChannelFilter,
                                       _output,
                                       ( int ) OutputChannelsEnum.BothChannels );
                }

                decoder = _l1Decoder;

                break;
            }

            default:
            {
                throw NewDecoderException( DecoderErrors.UNSUPPORTED_LAYER, null );
            }
        }

        return decoder ?? throw NewDecoderException( DecoderErrors.UNSUPPORTED_LAYER, null );
    }

    /// <summary>
    /// Initializes the decoder with the specified header information.
    /// </summary>
    /// <param name="header">The header information of the audio frame.</param>
    /// <remarks>
    /// This method sets up the necessary components for decoding the audio frame based on the header information.
    /// It initializes the output buffer, left and right channel filters, and sets the output channels and frequency.
    /// </remarks>
    private void Initialise( Header header )
    {
        var channels = header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;

        // Set up output buffer if not set up by client.
        _output ??= new SampleBuffer( header.Frequency(), channels );

        var factors = _equalizer?.BandFactors;

        _leftChannelFilter = new SynthesisFilter( 0, ScaleFactor, factors );

        if ( channels == 2 )
        {
            _rightChannelFilter = new SynthesisFilter( 1, ScaleFactor, factors );
        }

        _outputChannels  = channels;
        _outputFrequency = header.Frequency();

        _isInitialized = true;
    }
}
