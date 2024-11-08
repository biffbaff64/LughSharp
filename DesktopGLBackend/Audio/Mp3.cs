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

using JetBrains.Annotations;

namespace DesktopGLBackend.Audio;

[PublicAPI]
public class Mp3
{
    [PublicAPI]
    public class Music : OpenALMusic
    {
//        private Bitstream?    _bitstream;
//        private Decoder?      _decoder;
//        private SampleBuffer? _outputBuffer;

        public Music( OpenALAudio audio, FileInfo file )
            : base( audio, file )
        {
//            if ( audio.NoDevice )
//            {
//                return;
//            }
//
//            _bitstream = new Bitstream( file.OpenRead() );
//            _decoder   = new MP3Decoder();
//
//            try
//            {
//                Header header = _bitstream.readFrame();
//
//                if ( header == null )
//                {
//                    throw new GdxRuntimeException( "Empty MP3" );
//                }
//
//                var channels = header.mode() == Header.SINGLE_CHANNEL ? 1 : 2;
//                _outputBuffer = new OutputBuffer( channels, false );
//                _decoder.setOutputBuffer( _outputBuffer );
//                Setup( channels, header.getSampleRate() );
//            }
//            catch ( BitstreamException e )
//            {
//                throw new GdxRuntimeException( "error while preloading mp3", e );
//            }
        }

        public override int Read( byte[] buffer )
        {
//            try
//            {
//                boolean setup = _bitstream == null;
//
//                if ( setup )
//                {
//                    _bitstream = new Bitstream( file.read() );
//                    _decoder   = new MP3Decoder();
//                }
//
//                var totalLength       = 0;
//                var minRequiredLength = buffer.length - ( OutputBuffer.BUFFERSIZE * 2 );
//
//                while ( totalLength <= minRequiredLength )
//                {
//                    Header header = _bitstream.readFrame();
//
//                    if ( header == null )
//                    {
//                        break;
//                    }
//
//                    if ( setup )
//                    {
//                        var channels = header.mode() == Header.SINGLE_CHANNEL ? 1 : 2;
//                        _outputBuffer = new OutputBuffer( channels, false );
//                        _decoder.setOutputBuffer( _outputBuffer );
//                        setup( channels, header.getSampleRate() );
//                        setup = false;
//                    }
//
//                    try
//                    {
//                        _decoder.decodeFrame( header, _bitstream );
//                    }
//                    catch ( Exception ignored )
//                    {
//                        // JLayer's decoder throws ArrayIndexOutOfBoundsException sometimes!?
//                    }
//
//                    _bitstream.closeFrame();
//
//                    int length = _outputBuffer.reset();
//                    System.arraycopy( _outputBuffer.getBuffer(), 0, buffer, totalLength, length );
//                    totalLength += length;
//                }
//
//                return totalLength;
//            }
//            catch ( Throwable ex )
//            {
//                Reset();
//
//                throw new GdxRuntimeException( "Error reading audio data.", ex );
//            }

            return 0;
        }

        public override void Reset()
        {
//            if ( _bitstream == null )
//            {
//                return;
//            }
//
//            try
//            {
//                _bitstream.close();
//            }
//            catch ( BitstreamException ignored )
//            {
//            }
//
//            _bitstream = null;
        }
    }

    [PublicAPI]
    public class Sound : OpenALSound
    {
        public Sound( OpenALAudio audio, FileInfo file )
            : base( audio )
        {
//            if ( audio.NoDevice )
//            {
//                return;
//            }
//
//            ByteArrayOutputStream output    = new ByteArrayOutputStream( 4096 );
//            var                   bitstream = new Bitstream( file.read() );
//            MP3Decoder            decoder   = new MP3Decoder();
//
//            try
//            {
//                SampleBuffer outputBuffer = null;
//
//                int sampleRate = -1, channels = -1;
//
//                while ( true )
//                {
//                    Header? header = bitstream.ReadFrame();
//
//                    if ( header == null )
//                    {
//                        break;
//                    }
//
//                    if ( outputBuffer == null )
//                    {
//                        channels     = header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;
//                        sampleRate   = header.GetSampleFrequency();
//                        outputBuffer = new SampleBuffer( sampleRate, channels );
//
//                        decoder.setOutputBuffer( outputBuffer );
//                    }
//
//                    try
//                    {
//                        decoder.decodeFrame( header, bitstream );
//                    }
//                    catch ( Exception )
//                    {
//                        // Ignored
//                    }
//
//                    bitstream.CloseFrame();
//                    output.Write( outputBuffer.Buffer, 0, outputBuffer.Reset() );
//                }
//
//                bitstream.Close();
//
//                Setup( output.toByteArray(), channels, sampleRate );
//            }
//            catch ( Exception ex )
//            {
//                throw new GdxRuntimeException( "Error reading audio data.", ex );
//            }
        }
    }
}
