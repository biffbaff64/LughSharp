// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Collections.ObjectModel;

namespace LughSharp.LibCore.Audio.Maponus;

/// <summary>
/// <c>AudioFormat</c> is the class that specifies a particular arrangement of data
/// in a sound stream. By examining the information stored in the audio format, you
/// can discover how to interpret the bits in the binary sound data.
/// <para>
/// Every data line has an audio format associated with its data stream. The audio
/// format of a source (playback) data line indicates what kind of data the data line
/// expects to receive for output. For a target (capture) data line, the audio format
/// specifies the kind of the data that can be read from the line. Sound files also
/// have audio formats, of course. The <see cref="AudioFileFormat"/> class encapsulates
/// an <c>AudioFormat</c> in addition to other, file-specific information. Similarly,
/// an <see cref="AudioInputStream"/> has an <c>AudioFormat</c>.
/// </para>
/// <para>
/// The <c>AudioFormat</c> class accommodates a number of common sound-file encoding
/// techniques, including pulse-code modulation (PCM), mu-law encoding, and a-law
/// encoding. These encoding techniques are predefined, but service providers can
/// create new encoding types.
/// </para>
/// <para>
/// The encoding that a specific format uses is named by its <c>encoding</c> field.
/// </para>
/// <para>
/// In addition to the encoding, the audio format includes other properties that
/// further specify the exact arrangement of the data.
/// </para>
/// These include the number of channels, sample rate, sample size, byte order, frame
/// rate, and frame size. Sounds may have different numbers of audio channels: one for
/// mono, two for stereo. The sample rate measures how many "snapshots" (samples) of
/// the sound pressure are taken per second, per channel. (If the sound is stereo rather
/// than mono, two samples are actually measured at each instant of time: one for the
/// left channel, and another for the right channel; however, the sample rate still
/// measures the number per channel, so the rate is the same regardless of the number
/// of channels. This is the standard use of the term.)
/// <para>
/// The sample size indicates how many bits are used to store each snapshot; 8 and 16
/// are typical values. For 16-bit samples (or any other sample size larger than a byte),
/// byte order is important; the bytes in each sample are arranged in either the "little
/// -endian" or "big-endian" style.
/// </para>
/// <para>
/// For encodings like PCM, a frame consists of the set of samples for all channels at
/// a given point in time, and so the size of a frame (in bytes) is always equal to the
/// size of a sample (in bytes) times the number of channels. However, with some other
/// sorts of encodings a frame can contain a bundle of compressed data for a whole series
/// of samples, as well as additional, non-sample data. For such encodings, the sample
/// rate and sample size refer to the data after it is decoded into PCM, and so they are
/// completely different from the frame rate and frame size.
/// </para>
/// <para>
/// An <c>AudioFormat</c> object can include a set of properties. A property is a pair
/// of key and value: the key is of type <c>string</c>, the associated property value is
/// an arbitrary object. Properties specify additional format specifications, like the
/// bit rate for compressed formats. Properties are mainly used as a means to transport
/// additional information of the audio format to and from the service providers. Therefore,
/// properties are ignored in the <see cref="Matches(AudioFormat)"/> method. However, methods
/// which rely on the installed service providers,
/// like <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/> may consider
/// properties, depending on the respective service provider implementation.
/// </para>
/// </summary>
[PublicAPI]
public class AudioFormat
{
    /// <summary>
    /// The audio encoding technique used by this format.
    /// </summary>
    protected EncodingType Encoding;

    /// <summary>
    /// The number of samples played or recorded per second, for sounds that have this format.
    /// For compressed formats, the return value is the sample rate of the uncompressed audio data.
    /// When this AudioFormat is used for queries (e.g. <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/>
    /// or capabilities (e.g. <see cref="IDataLine.Info.GetFormats()"/>), a sample rate of
    /// <c>AudioSystem.NOT_SPECIFIED</c> means that any sample rate is acceptable. <c>AudioSystem.NOT_SPECIFIED</c>
    /// is also returned when the sample rate is not defined for this audio format.
    /// </summary>
    /// <returns> the number of samples per second, or <c>AudioSystem.NOT_SPECIFIED</c>. </returns>
    protected float SampleRate;

    /// <summary>
    /// The number of bits in each sample of a sound that has this format. For compressed
    /// formats, the return value is the sample size of the uncompressed audio data. When
    /// this AudioFormat is used for queries
    /// <para>
    /// (e.g. <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/>
    /// or capabilities (e.g. <see cref="IDataLine.Info.GetFormats()"/>)
    /// </para>
    /// A sample size of <c>AudioSystem.NOT_SPECIFIED</c> means that any sample size is
    /// acceptable. <c>AudioSystem.NOT_SPECIFIED</c> is also returned when the sample size
    /// is not defined for this audio format.
    /// </summary>
    /// <returns> the number of bits in each sample, or <c>AudioSystem.NOT_SPECIFIED</c>. </returns>
    protected int SampleSizeInBits;

    /// <summary>
    /// The number of audio channels in this format (1 for mono, 2 for stereo). When this
    /// AudioFormat is used for queries
    /// <para>
    /// (e.g. <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/>
    /// or capabilities (e.g. <see cref="IDataLine.Info.GetFormats()"/>)
    /// </para>
    /// A return value of <c>AudioSystem.NOT_SPECIFIED</c> means that any (positive)
    /// number of channels is acceptable.
    /// </summary>
    /// <returns>
    /// The number of channels (1 for mono, 2 for stereo, etc.), or <c>AudioSystem.NOT_SPECIFIED</c>
    /// </returns>
    protected int Channels;

    /// <summary>
    /// The number of bytes in each frame of a sound that has this format. When this
    /// AudioFormat is used for queries
    /// <para>
    /// (e.g. <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/>
    /// or capabilities (e.g. <see cref="IDataLine.Info.GetFormats()"/>)
    /// </para>
    /// A frame size of <c>AudioSystem.NOT_SPECIFIED</c> means that any frame size is
    /// acceptable. <c>AudioSystem.NOT_SPECIFIED</c> is also returned when the frame
    /// size is not defined for this audio format.
    /// </summary>
    /// <returns> the number of bytes per frame, or <c>AudioSystem.NOT_SPECIFIED</c> </returns>
    protected int FrameSize;

    /// <summary>
    /// The number of frames played or recorded per second, for sounds that have this format.
    /// When this AudioFormat is used for queries
    /// <para>
    /// (e.g. <see cref="AudioSystem.IsConversionSupported(AudioFormat, AudioFormat)"/>
    /// or capabilities (e.g. <see cref="IDataLine.Info.GetFormats()"/>)
    /// </para>
    /// A frame rate of <c>AudioSystem.NOT_SPECIFIED</c> means that any frame rate is
    /// acceptable. <c>AudioSystem.NOT_SPECIFIED</c> is also returned when the frame rate
    /// is not defined for this audio format.
    /// </summary>
    /// <returns> the number of frames per second, or <c>AudioSystem.NOT_SPECIFIED</c> </returns>
    protected float FrameRate;

    /// <summary>
    /// Indicates whether the audio data is stored in big-endian or little-endian
    /// byte order.  If the sample size is not more than one byte, the return value is
    /// irrelevant.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the data is stored in big-endian byte order, <c>false</c> if little-endian
    /// </returns>
    protected bool BigEndian;

    /// The set of properties
    private Dictionary< string, object >? _properties = new();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// Constructs an <b>AudioFormat</b> with the given parameters. The encoding specifies
    /// the convention used to represent the data. The other parameters are further explained
    /// in the <see cref="AudioFormat"/> class description.
    /// </summary>
    /// <param name="encoding">The audio encoding technique</param>
    /// <param name="sampleRate">The number of samples per second</param>
    /// <param name="sampleSizeInBits">The number of bits in each sample</param>
    /// <param name="channels">The number of channels (1 for mono, 2 for stereo, and so on)</param>
    /// <param name="frameSize">The number of bytes in each frame</param>
    /// <param name="frameRate">The number of frames per second</param>
    /// <param name="bigEndian">
    /// Indicates whether the data for a single sample is stored in big-endian byte order 
    /// (<b>false</b> means little-endian)
    /// </param>
    public AudioFormat( EncodingType encoding,
                        float sampleRate,
                        int sampleSizeInBits,
                        int channels,
                        int frameSize,
                        float frameRate,
                        bool bigEndian )
    {
        this.Encoding         = encoding;
        this.SampleRate       = sampleRate;
        this.SampleSizeInBits = sampleSizeInBits;
        this.Channels         = channels;
        this.FrameSize        = frameSize;
        this.FrameRate        = frameRate;
        this.BigEndian        = bigEndian;
    }

    /// <summary>
    /// Constructs an <b>AudioFormat</b> with the given parameters. The encoding specifies
    /// the convention used to represent the data. The other parameters are further explained
    /// in the <see cref="AudioFormat"/> class description.
    /// </summary>
    /// <param name="encoding"> the audio encoding technique </param>
    /// <param name="sampleRate"> the number of samples per second </param>
    /// <param name="sampleSizeInBits"> the number of bits in each sample </param>
    /// <param name="channels"> the number of channels (1 for mono, 2 for stereo, and so on) </param>
    /// <param name="frameSize"> the number of bytes in each frame </param>
    /// <param name="frameRate"> the number of frames per second </param>
    /// <param name="bigEndian">
    /// indicates whether the data for a single sample is stored in big-endian byte
    /// order (<c>false</c> means little-endian)
    /// </param>
    /// <param name="properties">
    /// a <c>Dictionary&lt;string,object&gt;</c> object containing format properties
    /// </param>
    public AudioFormat( EncodingType encoding,
                        float sampleRate,
                        int sampleSizeInBits,
                        int channels,
                        int frameSize,
                        float frameRate,
                        bool bigEndian,
                        Dictionary< string, object > properties )
        : this( encoding, sampleRate, sampleSizeInBits, channels, frameSize, frameRate, bigEndian )
    {
        this._properties = new Dictionary< string, object >( properties );
    }


    /// <summary>
    /// Constructs an <c>AudioFormat</c> with a linear PCM encoding and the given parameters.
    /// The frame size is set to the number of bytes required to contain one sample from each
    /// channel, and the frame rate is set to the sample rate.
    /// </summary>
    /// <param name="sampleRate"> the number of samples per second </param>
    /// <param name="sampleSizeInBits"> the number of bits in each sample </param>
    /// <param name="channels"> the number of channels (1 for mono, 2 for stereo, and so on) </param>
    /// <param name="signed"> indicates whether the data is signed or unsigned </param>
    /// <param name="bigEndian">
    /// indicates whether the data for a single sample is stored in big-endian byte order
    /// (<c>false</c> means little-endian)
    /// </param>
    public AudioFormat( float sampleRate, int sampleSizeInBits, int channels, bool signed, bool bigEndian )
        : this( ( signed ? EncodingType.PcmSigned : EncodingType.PcmUnsigned ),
                sampleRate,
                sampleSizeInBits,
                channels,
                ( ( channels == AudioSystem.NOT_SPECIFIED ) || ( sampleSizeInBits == AudioSystem.NOT_SPECIFIED ) )
                    ? AudioSystem.NOT_SPECIFIED
                    : ( ( sampleSizeInBits + 7 ) / 8 ) * channels,
                sampleRate,
                bigEndian )
    {
    }

    /// <summary>
    /// Obtain an unmodifiable map of properties. The concept of properties is further explained in
    /// the <see cref="AudioFileFormat"/> class description.
    /// </summary>
    /// <returns> a <c>Dictionary&lt;string,object&gt;</c> object containing all properties. If no
    /// properties are recognized, an empty dictionary is returned.
    /// </returns>
    public Dictionary< string, object > Properties()
    {
        Dictionary< string, object > ret = _properties ?? new Dictionary< string, object >( 0 );

        var b = new ReadOnlyDictionary< string, object >( ret );

        return new Dictionary< string, object >( b );
    }

    /// <summary>
    /// Obtain the property value specified by the key. The concept of properties is further
    /// explained in the <see cref="AudioFileFormat"/> class description.
    /// <para>
    /// If the specified property is not defined for a particular file format, this method
    /// returns <c>null</c>.
    /// </para>
    /// </summary>
    /// <param name="key"> the key of the desired property </param>
    /// <returns>
    /// the value of the property with the specified key, or <c>null</c> if the property does
    /// not exist.
    /// </returns>
    public object? GetProperty( string key )
    {
        return _properties?[ key ];
    }

    /// <summary>
    /// Indicates whether this format matches the one specified. To match, two formats must
    /// have the same encoding, and consistent values of the number of channels, sample rate,
    /// sample size, frame rate, and frame size. The values of the property are consistent
    /// if they are equal or the specified format has the property value <c>AudioSystem.NOT_SPECIFIED</c>.
    /// The byte order (big-endian or little-endian) must be the same if the sample size is
    /// greater than one byte.
    /// </summary>
    /// <param name="format"> format to test for match </param>
    /// <returns> <c>true</c> if this format matches the one specified, <c>false</c> otherwise. </returns>
    public bool Matches( AudioFormat format )
    {
        return format.Encoding.Equals( Encoding )
            && ( ( format.Channels == AudioSystem.NOT_SPECIFIED )
              || ( format.Channels == Channels ) )
            && ( ( format.SampleRate.Equals( AudioSystem.NOT_SPECIFIED ) )
              || ( format.SampleRate.Equals( SampleRate ) ) )
            && ( ( format.SampleSizeInBits == AudioSystem.NOT_SPECIFIED )
              || ( format.SampleSizeInBits == SampleSizeInBits ) )
            && ( ( format.FrameRate.Equals( AudioSystem.NOT_SPECIFIED ) )
              || ( format.FrameRate.Equals( FrameRate ) ) )
            && ( ( format.FrameSize == AudioSystem.NOT_SPECIFIED )
              || ( format.FrameSize == FrameSize ) )
            && ( ( SampleSizeInBits <= 8 )
              || ( format.BigEndian == BigEndian ) );
    }

    /// <summary>
    /// Returns a string that describes the format, such as:
    /// <b>"PCM SIGNED 22050 Hz 16 bit mono big-endian"</b>. The contents of the string
    /// may vary between implementations of Maponus.
    /// </summary>
    /// <returns>A string that describes the format parameters</returns>
    public override string ToString()
    {
        var sb = new StringBuilder();

        sb.Append( Encoding + "" );

        if ( SampleRate.Equals( AudioSystem.NOT_SPECIFIED ) )
        {
            sb.Append( "unknown sample rate, " );
        }
        else
        {
            sb.Append( "" + SampleRate + " Hz, " );
        }

        if ( SampleSizeInBits.Equals( AudioSystem.NOT_SPECIFIED ) )
        {
            sb.Append( "unknown bits per sample, " );
        }
        else
        {
            sb.Append( "" + SampleSizeInBits + " bit, " );
        }
        
        if ( Channels == 1 )
        {
            sb.Append( "mono, " );
        }
        else if ( Channels == 2 )
        {
            sb.Append( "stereo, " );
        }
        else
        {
            if ( Channels == AudioSystem.NOT_SPECIFIED )
            {
                sb.Append( " unknown number of channels, " );
            }
            else
            {
                sb.Append( "" + Channels + " channels, " );
            }
        }

        if ( FrameSize.Equals( AudioSystem.NOT_SPECIFIED ) )
        {
            sb.Append( "unknown frame size, " );
        }
        else
        {
            sb.Append( "" + FrameSize + " bytes/frame, " );
        }

        if ( Math.Abs( SampleRate - FrameRate ) > 0.00001f )
        {
            if ( FrameRate.Equals( AudioSystem.NOT_SPECIFIED ) )
            {
                sb.Append( "unknown frame rate, " );
            }
            else
            {
                sb.Append( FrameRate + " frames/second, " );
            }
        }

        if ( ( Encoding.Equals( EncodingType.PcmSigned ) || Encoding.Equals( EncodingType.PcmUnsigned ) )
          && SampleSizeInBits is > 8 or AudioSystem.NOT_SPECIFIED )
        {
            sb.Append( BigEndian ? "big-endian" : "little-endian" );
        }

        return sb.ToString();
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    /// The <c>Encoding</c> class names the specific type of data representation used
    /// for an audio stream. The encoding includes aspects of the sound format other
    /// than the number of channels, sample rate, sample size, frame rate, frame size,
    /// and byte order.
    /// <para>
    /// One ubiquitous type of audio encoding is pulse-code modulation (PCM), which is
    /// simply a linear (proportional) representation of the sound waveform. With PCM,
    /// the number stored in each sample is proportional to the instantaneous amplitude
    /// of the sound pressure at that point in time. The numbers may be signed or unsigned
    /// integers or floats. Besides PCM, other encodings include mu-law and a-law, which
    /// are nonlinear mappings of the sound amplitude that are often used for recording speech.
    /// </para>
    /// <para>
    /// You can use a predefined encoding by referring to one of the static objects created
    /// by this class, such as PcmSigned or PcmUnsigned. Service providers can create new
    /// encodings, such as compressed audio formats, and make these available through the
    /// <c><see cref="AudioSystem"/></c> class.
    /// </para>
    /// <para>
    /// The <c>Encoding</c> class is static, so that all <c>AudioFormat</c> objects that
    /// have the same encoding will refer to the same object (rather than different instances
    /// of the same class). This allows matches to be made by checking that two format's encodings
    /// are equal.
    /// </para>
    /// </summary>
    [PublicAPI]
    public class EncodingType
    {
        /// <summary>
        /// Specifies signed, linear PCM data.
        /// </summary>
        public readonly static EncodingType PcmSigned = new( "PCM_SIGNED" );

        /// <summary>
        /// Specifies unsigned, linear PCM data.
        /// </summary>
        public readonly static EncodingType PcmUnsigned = new( "PCM_UNSIGNED" );

        /// <summary>
        /// Specifies floating-point PCM data.
        /// </summary>
        public readonly static EncodingType PcmFloat = new( "PCM_FLOAT" );

        /// <summary>
        /// Specifies u-law encoded data.
        /// </summary>
        public readonly static EncodingType Ulaw = new( "ULAW" );

        /// <summary>
        /// Specifies a-law encoded data.
        /// </summary>
        public readonly static EncodingType Alaw = new( "ALAW" );

        // --------------------------------------------------------------------

        private string? _encodingName;

        // --------------------------------------------------------------------

        /// <summary>
        /// Constructs a new encoding.
        /// </summary>
        /// <param name="name"> the name of the new type of encoding </param>
        public EncodingType( string name )
        {
            this._encodingName = name;
        }

        /// <summary>
        /// Finalizes the equals method
        /// </summary>
        public override bool Equals( object? obj )
        {
            if ( ToString() == null )
            {
                return ( obj != null ) && ( obj.ToString() == null );
            }

            return obj is EncodingType && ToString()!.Equals( obj.ToString() );
        }

        /// <summary>
        /// Finalizes the hashCode method
        /// </summary>
        public override int GetHashCode()
        {
            return ToString()!.GetHashCode();
        }

        /// <summary>
        /// Provides the <b>String</b> representation of the encoding.  This <b>String</b> is
        /// the same name that was passed to the constructor. For the predefined encodings, the
        /// name is similar to the encoding's variable (field) name.
        /// <para>
        /// For example, <b>PcmSigned.ToString()</b> returns the name "PcmSigned".
        /// </para>
        /// </summary>
        /// <returns> the encoding name. </returns>
        public override string? ToString()
        {
            return _encodingName;
        }
    }
}
