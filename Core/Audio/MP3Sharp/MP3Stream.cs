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
///     Provides a view of the sequence of bytes that are produced during the conversion of an MP3 stream
///     into a 16-bit PCM-encoded ("WAV" format) stream.
/// </summary>
public class Mp3Stream : Stream
{
    private const int BACK_STREAM_BYTE_COUNT_REP = 0;

    private readonly Bitstream         _bitStream;
    private readonly Buffer16BitStereo _buffer;
    private readonly Decoder           _decoder = new( Decoder.DefaultParams );
    private readonly Stream            _sourceStream;

    /// <summary>
    ///     Creates a new stream instance using the provided filename, and the default chunk size of 4096 bytes.
    /// </summary>
    public Mp3Stream( string fileName )
        : this( new FileStream( fileName, FileMode.Open, FileAccess.Read ) )
    {
    }

    /// <summary>
    ///     Creates a new stream instance using the provided filename and chunk size.
    /// </summary>
    public Mp3Stream( string fileName, int chunkSize )
        : this( new FileStream( fileName, FileMode.Open, FileAccess.Read ), chunkSize )
    {
    }

    /// <summary>
    ///     Creates a new stream instance using the provided stream as a sourc,
    ///     and a chunk size, which has a default size of 4096.
    ///     Will also read the first frame of the MP3 into the public buffer.
    /// </summary>
    public Mp3Stream( Stream sourceStream, int chunkSize = 4096 )
    {
        IsEof                 = false;
        _sourceStream         = sourceStream;
        _bitStream            = new Bitstream( new PushbackStream( _sourceStream, chunkSize ) );
        _buffer               = new Buffer16BitStereo();
        _decoder.OutputBuffer = _buffer;

        // read the first frame. This will fill the initial buffer with data, and get our frequency!
        IsEof |= !ReadFrame();

        Format = ChannelCount switch
                 {
                     1 => SoundFormat.Pcm16BitMono,
                     2 => SoundFormat.Pcm16BitStereo,
                     _ => throw new Mp3SharpException
                         ( $"Unhandled channel count rep: {ChannelCount} "
                         + $"(allowed values are 1-mono and 2-stereo)." )
                 };

        if ( Format == SoundFormat.Pcm16BitMono )
        {
            _buffer.DoubleMonoToStereo = true;
        }
    }

    public bool IsEof { get; protected set; }

    /// <summary>
    ///     Gets the chunk size.
    /// </summary>
    [PublicAPI( "May be used externally" )]
    public static int ChunkSize => BACK_STREAM_BYTE_COUNT_REP;

    /// <summary>
    ///     Gets a value indicating whether the current stream supports reading.
    /// </summary>
    public override bool CanRead => _sourceStream.CanRead;

    /// <summary>
    ///     Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    public override bool CanSeek => _sourceStream.CanSeek;

    /// <summary>
    ///     Gets a value indicating whether the current stream supports writing.
    /// </summary>
    public override bool CanWrite => _sourceStream.CanWrite;

    /// <summary>
    ///     Gets the length in bytes of the stream.
    /// </summary>
    public override long Length => _sourceStream.Length;

    /// <summary>
    ///     Gets or sets the position of the source stream.  This is relative to the number of bytes in the MP3 file, rather
    ///     than the total number of PCM bytes (typically signicantly greater) contained in the Mp3Stream's output.
    /// </summary>
    public override long Position
    {
        get => _sourceStream.Position;
        set
        {
            if ( value < 0 )
            {
                value = 0;
            }

            if ( value > _sourceStream.Length )
            {
                value = _sourceStream.Length;
            }

            _sourceStream.Position =  value;
            IsEof                  =  false;
            IsEof                  |= !ReadFrame();
        }
    }

    /// <summary>
    ///     Gets the frequency of the audio being decoded. Updated every call to Read() or DecodeFrames(),
    ///     to reflect the most recent header information from the MP3 Stream.
    /// </summary>
    public int Frequency { get; private set; } = -1;

    /// <summary>
    ///     Gets the number of channels available in the audio being decoded.
    ///     Updated every call to Read() or DecodeFrames(), to reflect the most
    ///     recent header information from the MP3 Stream.
    /// </summary>
    public short ChannelCount { get; private set; } = -1;

    /// <summary>
    ///     Gets the PCM output format of this stream.
    /// </summary>
    public SoundFormat Format { get; }

    /// <summary>
    ///     Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
    /// </summary>
    public override void Flush() => _sourceStream.Flush();

    /// <summary>
    ///     Sets the position of the source stream.
    /// </summary>
    public override long Seek( long offset, SeekOrigin origin ) => _sourceStream.Seek( offset, origin );

    /// <summary>
    ///     This method is not valid for an Mp3Stream.
    /// </summary>
    public override void SetLength( long value ) => throw new InvalidOperationException();

    /// <summary>
    ///     This method is not valid for an Mp3Stream.
    /// </summary>
    public override void Write( byte[] buffer, int offset, int count ) => throw new InvalidOperationException();

    /// <summary>
    ///     Decodes the requested number of frames from the MP3 stream and caches their PCM-encoded bytes.
    ///     These can subsequently be obtained using the Read method.
    ///     Returns the number of frames that were successfully decoded.
    /// </summary>
    [PublicAPI( "May be used externally" )]
    public int DecodeFrames( int frameCount )
    {
        var framesDecoded = 0;
        var aFrameWasRead = true;

        while ( ( framesDecoded < frameCount ) && aFrameWasRead )
        {
            aFrameWasRead = ReadFrame();

            if ( aFrameWasRead )
            {
                framesDecoded++;
            }
        }

        return framesDecoded;
    }

    /// <summary>
    ///     Reads the MP3 stream as PCM-encoded bytes.  Decodes a portion of the stream if necessary.
    ///     Returns the number of bytes read.
    /// </summary>
    public override int Read( byte[] buffer, int offset, int count )
    {
        // Copy from queue buffers, reading new ones as necessary,
        // until we can't read more or we have read "count" bytes
        if ( IsEof )
        {
            return 0;
        }

        var bytesRead = 0;

        while ( true )
        {
            if ( _buffer.BytesLeft <= 0 )
            {
                if ( !ReadFrame() ) // out of frames or end of stream?
                {
                    IsEof = true;
                    _bitStream.CloseFrame();

                    break;
                }
            }

            // Copy as much as we can from the current buffer:
            bytesRead += _buffer.Read( buffer,
                                       offset + bytesRead,
                                       count - bytesRead );

            if ( bytesRead >= count )
            {
                break;
            }
        }

        return bytesRead;
    }

    /// <summary>
    ///     Closes the source stream and releases any associated resources.
    ///     If you don't call this, you may be leaking file descriptors.
    /// </summary>
    public override void Close() => _bitStream.Close(); // This should close SourceStream as well.

    /// <summary>
    ///     Reads a frame from the MP3 stream.  Returns whether the operation was successful.  If it wasn't,
    ///     the source stream is probably at its end.
    /// </summary>
    private bool ReadFrame()
    {
        // Read a frame from the bitstream.
        Header? header = _bitStream.ReadFrame();

        if ( header == null )
        {
            return false;
        }

        try
        {
            // Set the channel count and frequency values for the stream.
            ChannelCount = header.Mode() == Header.SINGLE_CHANNEL ? ( short )1 : ( short )2;
            Frequency    = header.Frequency();

            // Decode the frame.
            ABuffer? decoderOutput = _decoder.DecodeFrame( header, _bitStream );

            if ( decoderOutput != _buffer )
            {
                throw new ApplicationException( "Output buffers are different." );
            }
        }
        finally
        {
            // No resource leaks please!
            _bitStream.CloseFrame();
        }

        return true;
    }
}
