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
    private TargetDataLine _line;
    private byte[]         _buffer = new byte[ 1024 * 4 ];

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="samplingRate"></param>
    /// <param name="isMono"></param>
    /// <exception cref="GdxRuntimeException"></exception>
    public GdxSoundAudioRecorder( int samplingRate, bool isMono )
    {
        try
        {
            var format = new AudioFormat( AudioFormat.EncodingType.PcmSigned,
                                          samplingRate,
                                          16,
                                          isMono ? 1 : 2,
                                          isMono ? 2 : 4,
                                          samplingRate,
                                          false );

            _line = AudioSystem.GetTargetDataLine( format );
            _line.Open( format, _buffer.Length );
            _line.Start();
        }
        catch ( Exception ex )
        {
            throw new GdxRuntimeException( $"Error creating {this.GetType().Name}.", ex );
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="samples"></param>
    /// <param name="offset"></param>
    /// <param name="numSamples"></param>
    public void Read( short[] samples, int offset, int numSamples )
    {
        if ( _buffer.Length < ( numSamples * 2 ) )
        {
            _buffer = new byte[ numSamples * 2 ];
        }

        var toRead = numSamples * 2;
        var read   = 0;

        while ( read != toRead )
        {
            read += _line.Read( _buffer, read, toRead - read );
        }

        for ( int i = 0, j = 0; i < numSamples * 2; i += 2, j++ )
        {
            samples[ offset + j ] = ( short ) ( ( _buffer[ i + 1 ] << 8 ) | ( _buffer[ i ] & 0xff ) );
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _line.Close();
    }
}
