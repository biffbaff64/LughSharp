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
public interface IDataLine : ILine
{
    /**
     * Drains queued data from the line by continuing data I/O until the
     * data line's internal buffer has been emptied.
     * This method blocks until the draining is complete.  Because this is a
     * blocking method, it should be used with care.  If <code>drain()</code>
     * is invoked on a stopped line that has data in its queue, the method will
     * block until the line is running and the data queue becomes empty.  If
     * <code>drain()</code> is invoked by one thread, and another continues to
     * fill the data queue, the operation will not complete.
     * This method always returns when the data line is closed.
     *
     * @see #flush()
     */
    void Drain();

    /**
     * Flushes queued data from the line.  The flushed data is discarded.
     * In some cases, not all queued data can be discarded.  For example, a
     * mixer can flush data from the buffer for a specific input line, but any
     * unplayed data already in the output buffer (the result of the mix) will
     * still be played.  You can invoke this method after pausing a line (the
     * normal case) if you want to skip the "stale" data when you restart
     * playback or capture. (It is legal to flush a line that is not stopped,
     * but doing so on an active line is likely to cause a discontinuity in the
     * data, resulting in a perceptible click.)
     *
     * @see #stop()
     * @see #drain()
     */
    void Flush();

    /**
     * Allows a line to engage in data I/O.  If invoked on a line
     * that is already running, this method does nothing.  Unless the data in
     * the buffer has been flushed, the line resumes I/O starting
     * with the first frame that was unprocessed at the time the line was
     * stopped. When audio capture or playback starts, a
     * <code>{@link LineEvent.Type#START START}</code> event is generated.
     *
     * @see #stop()
     * @see #isRunning()
     * @see LineEvent
     */
    void Start();

    /**
     * Stops the line.  A stopped line should cease I/O activity.
     * If the line is open and running, however, it should retain the resources required
     * to resume activity.  A stopped line should retain any audio data in its buffer
     * instead of discarding it, so that upon resumption the I/O can continue where it left off,
     * if possible.  (This doesn't guarantee that there will never be discontinuities beyond the
     * current buffer, of course; if the stopped condition continues
     * for too long, input or output samples might be dropped.)  If desired, the retained data can be
     * discarded by invoking the <code>flush</code> method.
     * When audio capture or playback stops, a <code>{@link LineEvent.Type#STOP STOP}</code> event is generated.
     *
     * @see #start()
     * @see #isRunning()
     * @see #flush()
     * @see LineEvent
     */
    void Stop();

    /**
     * Indicates whether the line is running.  The default is <code>false</code>.
     * An open line begins running when the first data is presented in response to an
     * invocation of the <code>start</code> method, and continues
     * until presentation ceases in response to a call to <code>stop</code> or
     * because playback completes.
     * @return <code>true</code> if the line is running, otherwise <code>false</code>
     * @see #start()
     * @see #stop()
     */
    bool IsRunning();

    /**
     * Indicates whether the line is engaging in active I/O (such as playback
     * or capture).  When an inactive line becomes active, it sends a
     * <code>{@link LineEvent.Type#START START}</code> event to its listeners.  Similarly, when
     * an active line becomes inactive, it sends a
     * <code>{@link LineEvent.Type#STOP STOP}</code> event.
     * @return <code>true</code> if the line is actively capturing or rendering
     * sound, otherwise <code>false</code>
     * @see #isOpen
     * @see #addLineListener
     * @see #removeLineListener
     * @see LineEvent
     * @see LineListener
     */
    bool IsActive();

    /**
     * Obtains the current format (encoding, sample rate, number of channels,
     * etc.) of the data line's audio data.
     *
     * <para>If the line is not open and has never been opened, it returns
     * the default format. The default format is an implementation
     * specific audio format, or, if the <code>DataLine.Info</code>
     * object, which was used to retrieve this <code>DataLine</code>,
     * specifies at least one fully qualified audio format, the
     * last one will be used as the default format. Opening the
     * line with a specific audio format (e.g.
     * {@link SourceDataLine#open(AudioFormat)}) will override the
     * default format.
     *
     * @return current audio data format
     * @see AudioFormat
     */
    AudioFormat GetFormat();

    /**
     * Obtains the maximum number of bytes of data that will fit in the data line's
     * internal buffer.  For a source data line, this is the size of the buffer to
     * which data can be written.  For a target data line, it is the size of
     * the buffer from which data can be read.  Note that
     * the units used are bytes, but will always correspond to an integral
     * number of sample frames of audio data.
     *
     * @return the size of the buffer in bytes
     */
    int GetBufferSize();

    /**
     * Obtains the number of bytes of data currently available to the
     * application for processing in the data line's internal buffer.  For a
     * source data line, this is the amount of data that can be written to the
     * buffer without blocking.  For a target data line, this is the amount of data
     * available to be read by the application.  For a clip, this value is always
     * 0 because the audio data is loaded into the buffer when the clip is opened,
     * and persists without modification until the clip is closed.
     * <para>
     * Note that the units used are bytes, but will always
     * correspond to an integral number of sample frames of audio data.
     * <para>
     * An application is guaranteed that a read or
     * write operation of up to the number of bytes returned from
     * <code>available()</code> will not block; however, there is no guarantee
     * that attempts to read or write more data will block.
     *
     * @return the amount of data available, in bytes
     */
    int Available();

    /**
     * Obtains the current position in the audio data, in sample frames.
     * The frame position measures the number of sample
     * frames captured by, or rendered from, the line since it was opened.
     * This return value will wrap around after 2^31 frames. It is recommended
     * to use <code>getLongFramePosition</code> instead.
     *
     * @return the number of frames already processed since the line was opened
     * @see #getLongFramePosition()
     */
    int GetFramePosition();


    /**
     * Obtains the current position in the audio data, in sample frames.
     * The frame position measures the number of sample
     * frames captured by, or rendered from, the line since it was opened.
     *
     * @return the number of frames already processed since the line was opened
     * @since 1.5
     */
    long GetLongFramePosition();


    /**
     * Obtains the current position in the audio data, in microseconds.
     * The microsecond position measures the time corresponding to the number
     * of sample frames captured by, or rendered from, the line since it was opened.
     * The level of precision is not guaranteed.  For example, an implementation
     * might calculate the microsecond position from the current frame position
     * and the audio sample frame rate.  The precision in microseconds would
     * then be limited to the number of microseconds per sample frame.
     *
     * @return the number of microseconds of data processed since the line was opened
     */
    long GetMicrosecondPosition();

    /**
     * Obtains the current volume level for the line.  This level is a measure
     * of the signal's current amplitude, and should not be confused with the
     * current setting of a gain control. The range is from 0.0 (silence) to
     * 1.0 (maximum possible amplitude for the sound waveform).  The units
     * measure linear amplitude, not decibels.
     *
     * @return the current amplitude of the signal in this line, or
     * <code>{@link AudioSystem#NOT_SPECIFIED}</code>
     */
    float GetLevel();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------
    
    /**
     * Besides the class information inherited from its superclass,
     * <code>DataLine.Info</code> provides additional information specific to data lines.
     * This information includes:
     * <ul>
     * <li> the audio formats supported by the data line
     * <li> the minimum and maximum sizes of its internal buffer
     * </ul>
     * Because a <code>Line.Info</code> knows the class of the line its describes, a
     * <code>DataLine.Info</code> object can describe <code>DataLine</code>
     * subinterfaces such as <code>{@link SourceDataLine}</code>,
     * <code>{@link TargetDataLine}</code>, and <code>{@link Clip}</code>.
     * You can query a mixer for lines of any of these types, passing an appropriate
     * instance of <code>DataLine.Info</code> as the argument to a method such as
     * <code>{@link Mixer#getLine Mixer.getLine(Line.Info)}</code>.
     */
    public class Info : ILine.Info
    {
        private readonly AudioFormat[]? _formats;
        private readonly int            _minBufferSize;
        private readonly int            _maxBufferSize;

        /// <summary>
        /// Constructs a data line's info object from the specified information, which
        /// includes a set of supported audio formats and a range for the buffer size.
        /// This constructor is typically used by mixer implementations when returning
        /// information about a supported line.
        /// </summary>
        /// <param name="lineClass"> the class of the data line described by the info object </param>
        /// <param name="formats"> set of formats supported </param>
        /// <param name="minBufferSize"> minimum buffer size supported by the data line, in bytes </param>
        /// <param name="maxBufferSize"> maximum buffer size supported by the data line, in bytes </param>
        public Info( Type lineClass, AudioFormat[]? formats, int minBufferSize, int maxBufferSize )
            : base( lineClass )
        {
            if ( formats == null )
            {
                this._formats = Array.Empty< AudioFormat >();
            }
            else
            {
                Array.Copy( formats, this._formats!, formats.Length );
            }

            this._minBufferSize = minBufferSize;
            this._maxBufferSize = maxBufferSize;
        }

        /// <summary>
        /// Constructs a data line's info object from the specified information, which includes
        /// a single audio format and a desired buffer size. This constructor is typically used
        /// by an application to describe a desired line.
        /// </summary>
        /// <param name="lineClass"> the class of the data line described by the info object </param>
        /// <param name="format"> desired format </param>
        /// <param name="bufferSize">
        /// desired buffer size in bytes. Default is <see cref="AudioSystem.NOT_SPECIFIED"/>.
        /// </param>
        public Info( Type lineClass, AudioFormat? format, int bufferSize = AudioSystem.NOT_SPECIFIED )
            : base( lineClass )
        {
            if ( format == null )
            {
                this._formats = Array.Empty< AudioFormat >();
            }
            else
            {
                this._formats = new[] { format };
            }

            this._minBufferSize = bufferSize;
            this._maxBufferSize = bufferSize;
        }

        /// <summary>
        /// Obtains a set of audio formats supported by the data line.
        /// Note that <code>isFormatSupported(AudioFormat)</code> might return
        /// <code>true</code> for certain additional formats that are missing from
        /// the set returned by <code>getFormats()</code>.  The reverse is not
        /// the case: <code>isFormatSupported(AudioFormat)</code> is guaranteed to return
        /// <code>true</code> for all formats returned by <code>getFormats()</code>.
        /// <para>
        /// Some fields in the AudioFormat instances can be set to
        /// {@link javax.sound.sampled.AudioSystem#NOT_SPECIFIED NOT_SPECIFIED}
        /// if that field does not apply to the format,
        /// or if the format supports a wide range of values for that field.
        /// For example, a multi-channel device supporting up to
        /// 64 channels, could set the channel field in the
        /// <code>AudioFormat</code> instances returned by this
        /// method to <code>NOT_SPECIFIED</code>.
        /// </para>
        /// </summary>
        /// <returns> a set of supported audio formats. </returns>
        /// <seealso cref="IsFormatSupported(AudioFormat)"/>
        public override AudioFormat[] GetFormats()
        {
            return Arrays.copyOf( _formats, _formats.length );
        }

        /**
         * Indicates whether this data line supports a particular audio format.
         * The default implementation of this method simply returns <code>true</code> if
         * the specified format matches any of the supported formats.
         *
         * @param format the audio format for which support is queried.
         * @return <code>true</code> if the format is supported, otherwise <code>false</code>
         * @see #getFormats
         * @see AudioFormat#matches
         */
        public boolean isFormatSupported( AudioFormat format )
        {
            for ( int i = 0; i < _formats.length; i++ )
            {
                if ( format.matches( _formats[ i ] ) )
                {
                    return true;
                }
            }

            return false;
        }

        /**
         * Obtains the minimum buffer size supported by the data line.
         * @return minimum buffer size in bytes, or <code>AudioSystem.NOT_SPECIFIED</code>
         */
        public int getMinBufferSize()
        {
            return _minBufferSize;
        }


        /**
         * Obtains the maximum buffer size supported by the data line.
         * @return maximum buffer size in bytes, or <code>AudioSystem.NOT_SPECIFIED</code>
         */
        public int getMaxBufferSize()
        {
            return _maxBufferSize;
        }


        /**
         * Determines whether the specified info object matches this one.
         * To match, the superclass match requirements must be met.  In
         * addition, this object's minimum buffer size must be at least as
         * large as that of the object specified, its maximum buffer size must
         * be at most as large as that of the object specified, and all of its
         * formats must match formats supported by the object specified.
         * @return <code>true</code> if this object matches the one specified,
         * otherwise <code>false</code>.
         */
        public boolean matches( Line.Info info )
        {
            if ( !( super.matches( info ) ) )
            {
                return false;
            }

            Info dataLineInfo = ( Info ) info;

            // treat anything < 0 as NOT_SPECIFIED
            // demo code in old Java Sound Demo used a wrong buffer calculation
            // that would lead to arbitrary negative values
            if ( ( getMaxBufferSize() >= 0 ) && ( dataLineInfo.getMaxBufferSize() >= 0 ) )
            {
                if ( getMaxBufferSize() > dataLineInfo.getMaxBufferSize() )
                {
                    return false;
                }
            }

            if ( ( getMinBufferSize() >= 0 ) && ( dataLineInfo.getMinBufferSize() >= 0 ) )
            {
                if ( getMinBufferSize() < dataLineInfo.getMinBufferSize() )
                {
                    return false;
                }
            }

            AudioFormat[] localFormats = getFormats();

            if ( localFormats != null )
            {
                for ( int i = 0; i < localFormats.length; i++ )
                {
                    if ( !( localFormats[ i ] == null ) )
                    {
                        if ( !( dataLineInfo.isFormatSupported( localFormats[ i ] ) ) )
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /**
         * Obtains a textual description of the data line info.
         * @return a string description
         */
        public String toString()
        {
            StringBuffer buf = new StringBuffer();

            if ( ( _formats.length == 1 ) && ( _formats[ 0 ] != null ) )
            {
                buf.append( " supporting format " + _formats[ 0 ] );
            }
            else if ( getFormats().length > 1 )
            {
                buf.append( " supporting " + getFormats().length + " audio formats" );
            }

            if ( ( _minBufferSize != AudioSystem.NOT_SPECIFIED ) && ( _maxBufferSize != AudioSystem.NOT_SPECIFIED ) )
            {
                buf.append( ", and buffers of " + _minBufferSize + " to " + _maxBufferSize + " bytes" );
            }
            else if ( ( _minBufferSize != AudioSystem.NOT_SPECIFIED ) && ( _minBufferSize > 0 ) )
            {
                buf.append( ", and buffers of at least " + _minBufferSize + " bytes" );
            }
            else if ( _maxBufferSize != AudioSystem.NOT_SPECIFIED )
            {
                buf.append( ", and buffers of up to " + _minBufferSize + " bytes" );
            }

            return new String( super.toString() + buf );
        }
    } // class Info
}
