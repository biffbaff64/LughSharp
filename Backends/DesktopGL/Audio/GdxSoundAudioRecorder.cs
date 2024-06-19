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


using LughSharp.LibCore.Utils.Exceptions;

namespace LughSharp.Backends.DesktopGL.Audio;

[PublicAPI]
public class GdxSoundAudioRecorder : IAudioRecorder
{
    private TargetDataLine line;
    private byte[]         buffer = new byte[ 1024 * 4 ];

    public GdxSoundAudioRecorder( int samplingRate, bool isMono )
    {
        try
        {
            var format = new AudioFormat( AudioFormat.Encoding.PcmSigned,
                                          samplingRate,
                                          16,
                                          isMono ? 1 : 2,
                                          isMono ? 2 : 4,
                                          samplingRate,
                                          false );

            line = AudioSystem.getTargetDataLine( format );
            line.open( format, buffer.Length );
            line.start();
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( "Error creating JavaSoundAudioRecorder.", ex );
        }
    }

    public void Read( short[] samples, int offset, int numSamples )
    {
        if ( buffer.length < numSamples * 2 ) buffer = new byte[ numSamples * 2 ];

        int toRead = numSamples * 2;
        int read   = 0;

        while ( read != toRead )
            read += line.read( buffer, read, toRead - read );

        for ( int i = 0, j = 0; i < numSamples * 2; i += 2, j++ )
            samples[ offset + j ] = ( short ) ( ( buffer[ i + 1 ] << 8 ) | ( buffer[ i ] & 0xff ) );
    }

    public void Dispose()
    {
        line.close();
    }
}
