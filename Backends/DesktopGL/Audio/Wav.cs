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

using LibGDXSharp.Files;

namespace LibGDXSharp.Backends.Desktop.Audio;

public class Wav
{
    public class Music : OpenALMusic
    {
        private WavInputStream? _input;

        public Music( OpenALAudio audio, FileInfo file )
            : base( audio, file )
        {
            _input = new WavInputStream( file );

            if ( audio.NoDevice )
            {
                return;
            }

            Setup( _input.channels, _input.sampleRate );
        }

        public override int Read( byte[] buffer )
        {
            if ( _input == null )
            {
                _input = new WavInputStream( file );
                Setup( _input.channels, _input.sampleRate );
            }

            try
            {
                return _input.Read( buffer );
            }
            catch ( IOException ex )
            {
                throw new GdxRuntimeException( "Error reading WAV file: " + file, ex );
            }
        }

        public override void Reset()
        {
//            StreamUtils.closeQuietly( _input );
            _input = null;
        }
    }

    public class Sound : OpenALSound
    {
        public Sound( OpenALAudio audio, FileInfo file )
            : base( audio )
        {
            if ( audio.NoDevice )
            {
                return;
            }

            WavInputStream? input = null;

            try
            {
                input = new WavInputStream( file );
                Setup( StreamUtils.copyStreamToByteArray( input, input.dataRemaining ), input.channels, input.sampleRate );
            }
            catch ( IOException ex )
            {
                throw new GdxRuntimeException( "Error reading WAV file: " + file, ex );
            }
            finally
            {
//                StreamUtils.closeQuietly( input );
            }
        }
    }

    internal class WavInputStream : FilterInputStream
    {
        internal int channels;
        internal int sampleRate;
        internal int dataRemaining;

        internal WavInputStream( FileInfo file )
            : base( file.OpenRead() )
        {
            try
            {
                if ( ( Read() != 'R' ) || ( Read() != 'I' ) || ( Read() != 'F' ) || ( Read() != 'F' ) )
                {
                    throw new GdxRuntimeException( "RIFF header not found: " + file );
                }

                SkipFully( 4 );

                if ( ( Read() != 'W' ) || ( Read() != 'A' ) || ( Read() != 'V' ) || ( Read() != 'E' ) )
                {
                    throw new GdxRuntimeException( "Invalid wave file header: " + file );
                }

                var fmtChunkLength = seekToChunk( 'f', 'm', 't', ' ' );

                var type = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );

                if ( type != 1 )
                {
                    throw new GdxRuntimeException( "WAV files must be PCM: " + type );
                }

                channels = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );

                if ( ( channels != 1 ) && ( channels != 2 ) )
                {
                    throw new GdxRuntimeException( "WAV files must have 1 or 2 channels: " + channels );
                }

                sampleRate = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 ) | ( ( Read() & 0xff ) << 16 ) | ( ( Read() & 0xff ) << 24 );

                SkipFully( 6 );

                var bitsPerSample = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );

                if ( bitsPerSample != 16 )
                {
                    throw new GdxRuntimeException( "WAV files must have 16 bits per sample: " + bitsPerSample );
                }

                SkipFully( fmtChunkLength - 16 );

                dataRemaining = SeekToChunk( 'd', 'a', 't', 'a' );
            }
            catch ( Exception ex )
            {
//                StreamUtils.closeQuietly( this );

                throw new GdxRuntimeException( "Error reading WAV file: " + file, ex );
            }
        }

        private int SeekToChunk( char c1, char c2, char c3, char c4 )
        {
            while ( true )
            {
                var found = Read() == c1;

                found &= Read() == c2;
                found &= Read() == c3;
                found &= Read() == c4;
                
                var chunkLength = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 ) | ( ( Read() & 0xff ) << 16 ) | ( ( Read() & 0xff ) << 24 );

                if ( chunkLength == -1 )
                {
                    throw new IOException( "Chunk not found: " + c1 + c2 + c3 + c4 );
                }

                if ( found )
                {
                    return chunkLength;
                }

                SkipFully( chunkLength );
            }
        }

        private void SkipFully( int count )
        {
            while ( count > 0 )
            {
                long skipped = input.Skip( count );

                if ( skipped <= 0 )
                {
                    throw new EndOfStreamException( "Unable to skip." );
                }
                
                count -= skipped;
            }
        }

        public int Read( byte[] buffer )
        {
            if ( dataRemaining == 0 )
            {
                return -1;
            }

            var offset = 0;

            do
            {
                int length = Math.Min( base.Read( buffer, offset, buffer.Length - offset ), dataRemaining );

                if ( length == -1 )
                {
                    if ( offset > 0 )
                    {
                        return offset;
                    }

                    return -1;
                }

                offset        += length;
                dataRemaining -= length;
            }
            while ( offset < buffer.Length );

            return offset;
        }
    }
}
