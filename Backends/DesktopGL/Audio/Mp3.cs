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

using LibGDXSharp.Backends.Desktop.Audio.MP3Sharp;
using LibGDXSharp.Core.Files;

namespace LibGDXSharp.Backends.Desktop.Audio;

[PublicAPI]
public class Mp3 : OpenALMusic
{
    public class Music : OpenALMusic
    {
        private Bitstream?    _bitstream;
        private SampleBuffer? _outputBuffer;
        private Decoder?      _decoder;

        public Music( OpenALGLAudio audio, FileHandle file )
            : base( audio, file )
        {
            if ( audio.NoDevice )
            {
                return;
            }

            _bitstream = new Bitstream( file.read() );
            _decoder   = new MP3Decoder();

            try
            {
                Header header = _bitstream.readFrame();

                if ( header == null )
                {
                    throw new GdxRuntimeException( "Empty MP3" );
                }

                int channels = header.mode() == Header.SINGLE_CHANNEL ? 1 : 2;
                _outputBuffer = new OutputBuffer( channels, false );
                _decoder.setOutputBuffer( _outputBuffer );
                Setup( channels, header.getSampleRate() );
            }
            catch ( BitstreamException e )
            {
                throw new GdxRuntimeException( "error while preloading mp3", e );
            }

        }

        public int Read( byte[] buffer )
        {
            try
            {
                boolean setup = _bitstream == null;

                if ( setup )
                {
                    _bitstream = new Bitstream( file.read() );
                    _decoder   = new MP3Decoder();
                }

                int totalLength       = 0;
                int minRequiredLength = buffer.length - OutputBuffer.BUFFERSIZE * 2;

                while ( totalLength <= minRequiredLength )
                {
                    Header header = _bitstream.readFrame();

                    if ( header == null )
                    {
                        break;
                    }

                    if ( setup )
                    {
                        int channels = header.mode() == Header.SINGLE_CHANNEL ? 1 : 2;
                        _outputBuffer = new OutputBuffer( channels, false );
                        _decoder.setOutputBuffer( _outputBuffer );
                        setup( channels, header.getSampleRate() );
                        setup = false;
                    }

                    try
                    {
                        _decoder.decodeFrame( header, _bitstream );
                    }
                    catch ( System.Exception ignored )
                    {
                        // JLayer's decoder throws ArrayIndexOutOfBoundsException sometimes!?
                    }

                    _bitstream.closeFrame();

                    int length = _outputBuffer.reset();
                    System.arraycopy( _outputBuffer.getBuffer(), 0, buffer, totalLength, length );
                    totalLength += length;
                }

                return totalLength;
            }
            catch ( Throwable ex )
            {
                Reset();

                throw new GdxRuntimeException( "Error reading audio data.", ex );
            }
        }

        public void Reset()
        {
            if ( _bitstream == null )
            {
                return;
            }

            try
            {
                _bitstream.close();
            }
            catch ( BitstreamException ignored )
            {
            }

            _bitstream = null;
        }
    }

    public class Sound : OpenALSound
    {
        public Sound( OpenALGLAudio audio, FileHandle file )
            : base( audio )
        {
            if ( audio.NoDevice )
            {
                return;
            }

            ByteArrayOutputStream output    = new ByteArrayOutputStream( 4096 );
            Bitstream             bitstream = new Bitstream( file.read() );
            MP3Decoder            decoder   = new MP3Decoder();

            try
            {
                SampleBuffer outputBuffer = null;

                int sampleRate = -1, channels = -1;

                while ( true )
                {
                    Header? header = bitstream.ReadFrame();

                    if ( header == null )
                    {
                        break;
                    }

                    if ( outputBuffer == null )
                    {
                        channels     = header.Mode() == Header.SINGLE_CHANNEL ? 1 : 2;
                        sampleRate   = header.GetSampleFrequency();
                        outputBuffer = new SampleBuffer( sampleRate, channels );

                        decoder.setOutputBuffer( outputBuffer );
                    }

                    try
                    {
                        decoder.decodeFrame( header, bitstream );
                    }
                    catch ( System.Exception )
                    {
                        // Ignored
                    }

                    bitstream.CloseFrame();
                    output.Write( outputBuffer.Buffer, 0, outputBuffer.Reset() );
                }

                bitstream.Close();

                Setup( output.toByteArray(), channels, sampleRate );
            }
            catch ( System.Exception ex )
            {
                throw new GdxRuntimeException( "Error reading audio data.", ex );
            }
        }
    }
}
