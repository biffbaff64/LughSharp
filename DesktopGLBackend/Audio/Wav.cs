// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
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

using Corelib.Lugh.Utils.Exceptions;
using JetBrains.Annotations;

namespace DesktopGLBackend.Audio;

[PublicAPI]
public class Wav
{
    public class Music : OpenALMusic
    {
        private WavInputStream? _input;

        public Music( OpenALAudio audio, FileInfo file )
            : base( audio, file )
        {
//            _input = new WavInputStream( file );
//
//            if ( audio.NoDevice )
//            {
//                return;
//            }
//
//            Setup( _input.channels, _input.sampleRate );
        }

        public override int Read( byte[] buffer )
        {
//            if ( _input == null )
//            {
//                _input = new WavInputStream( file );
//                Setup( _input.channels, _input.sampleRate );
//            }

            try
            {
                return _input!.Read( buffer );
            }
            catch ( IOException ex )
            {
                throw new GdxRuntimeException( "Error reading WAV file: " + File, ex );
            }
        }

        public override void Reset()
        {
            _input = null;
        }
    }

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
//            WavInputStream? input = null;
//
//            try
//            {
//                input = new WavInputStream( file );
//                Setup( StreamUtils.CopyStreamToByteArray( input, input.dataRemaining ), input.channels, input.sampleRate );
//            }
//            catch ( IOException ex )
//            {
//                throw new GdxRuntimeException( "Error reading WAV file: " + file, ex );
//            }
        }
    }

    internal abstract class WavInputStream : Stream
    {
//        internal int channels;
//        internal int dataRemaining;
//        internal int sampleRate;

        internal WavInputStream( FileInfo file )
        {
//            try
//            {
//                if ( ( Read() != 'R' ) || ( Read() != 'I' ) || ( Read() != 'F' ) || ( Read() != 'F' ) )
//                {
//                    throw new GdxRuntimeException( "RIFF header not found: " + file );
//                }
//
//                SkipFully( 4 );
//
//                if ( ( Read() != 'W' ) || ( Read() != 'A' ) || ( Read() != 'V' ) || ( Read() != 'E' ) )
//                {
//                    throw new GdxRuntimeException( "Invalid wave file header: " + file );
//                }
//
//                var fmtChunkLength = seekToChunk( 'f', 'm', 't', ' ' );
//
//                var type = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );
//
//                if ( type != 1 )
//                {
//                    throw new GdxRuntimeException( "WAV files must be PCM: " + type );
//                }
//
//                channels = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );
//
//                if ( ( channels != 1 ) && ( channels != 2 ) )
//                {
//                    throw new GdxRuntimeException( "WAV files must have 1 or 2 channels: " + channels );
//                }
//
//                sampleRate = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 ) | ( ( Read() & 0xff ) << 16 ) | ( ( Read() & 0xff ) << 24 );
//
//                SkipFully( 6 );
//
//                var bitsPerSample = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 );
//
//                if ( bitsPerSample != 16 )
//                {
//                    throw new GdxRuntimeException( "WAV files must have 16 bits per sample: " + bitsPerSample );
//                }
//
//                SkipFully( fmtChunkLength - 16 );
//
//                dataRemaining = SeekToChunk( 'd', 'a', 't', 'a' );
//            }
//            catch ( Exception ex )
//            {
////                StreamUtils.closeQuietly( this );
//
//                throw new GdxRuntimeException( "Error reading WAV file: " + file, ex );
//            }
        }

        private int SeekToChunk( char c1, char c2, char c3, char c4 )
        {
//            while ( true )
//            {
//                var found = Read() == c1;
//
//                found &= Read() == c2;
//                found &= Read() == c3;
//                found &= Read() == c4;
//
//                var chunkLength = ( Read() & 0xff ) | ( ( Read() & 0xff ) << 8 ) | ( ( Read() & 0xff ) << 16 ) | ( ( Read() & 0xff ) << 24 );
//
//                if ( chunkLength == -1 )
//                {
//                    throw new IOException( "Chunk not found: " + c1 + c2 + c3 + c4 );
//                }
//
//                if ( found )
//                {
//                    return chunkLength;
//                }
//
//                SkipFully( chunkLength );
//            }

            return 0;
        }

        private void SkipFully( int count )
        {
//            while ( count > 0 )
//            {
//                long skipped = input.Skip( count );
//
//                if ( skipped <= 0 )
//                {
//                    throw new EndOfStreamException( "Unable to skip." );
//                }
//
//                count -= skipped;
//            }
        }

        public int Read( byte[] buffer )
        {
//            if ( dataRemaining == 0 )
//            {
//                return -1;
//            }

            var offset = 0;

//            do
//            {
//                int length = Math.Min( base.Read( buffer, offset, buffer.Length - offset ), dataRemaining );
//
//                if ( length == -1 )
//                {
//                    if ( offset > 0 )
//                    {
//                        return offset;
//                    }
//
//                    return -1;
//                }
//
//                offset        += length;
//                dataRemaining -= length;
//            }
//            while ( offset < buffer.Length );

            return offset;
        }
    }
}
