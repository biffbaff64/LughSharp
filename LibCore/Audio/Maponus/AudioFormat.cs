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

namespace LughSharp.LibCore.Audio.Maponus;

[PublicAPI]
public class AudioFormat
{
    /**
     * The audio encoding technique used by this format.
     */
    protected EncodingTypes Encoding;

    /**
     * The number of samples played or recorded per second, for sounds that have this format.
     */
    protected float SampleRate;

    /**
     * The number of bits in each sample of a sound that has this format.
     */
    protected int SampleSizeInBits;

    /**
     * The number of audio channels in this format (1 for mono, 2 for stereo).
     */
    protected int Channels;

    /**
     * The number of bytes in each frame of a sound that has this format.
     */
    protected int FrameSize;

    /**
     * The number of frames played or recorded per second, for sounds that have this format.
     */
    protected float FrameRate;

    /**
     * Indicates whether the audio data is stored in big-endian or little-endian order.
     */
    protected bool BigEndian;


    /// The set of properties
    private Dictionary< string, object >? _properties;

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    public AudioFormat( EncodingTypes encoding,
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
        this._properties      = null;
    }

    [PublicAPI]
    public class EncodingTypes
    {
        /// <summary>
        /// Specifies signed, linear PCM data.
        /// </summary>
        public readonly static EncodingTypes PcmSigned = new( "PCM_SIGNED" );

        /// <summary>
        /// Specifies unsigned, linear PCM data.
        /// </summary>
        public readonly static EncodingTypes PcmUnsigned = new( "PCM_UNSIGNED" );

        /// <summary>
        /// Specifies floating-point PCM data.
        /// </summary>
        public readonly static EncodingTypes PcmFloat = new( "PCM_FLOAT" );

        /// <summary>
        /// Specifies u-law encoded data.
        /// </summary>
        public readonly static EncodingTypes Ulaw = new( "ULAW" );

        /// <summary>
        /// Specifies a-law encoded data.
        /// </summary>
        public readonly static EncodingTypes Alaw = new( "ALAW" );

        /// <summary>
        /// Encoding name.
        /// </summary>
        private string? _encodingName;

        // --------------------------------------------------------------------
        // --------------------------------------------------------------------

        /// <summary>
        /// Constructs a new encoding.
        /// </summary>
        /// <param name="name"> the name of the new type of encoding </param>
        public EncodingTypes( string name )
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

            return obj is EncodingTypes && ToString()!.Equals( obj.ToString() );
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
