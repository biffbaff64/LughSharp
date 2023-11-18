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

namespace LibGDXSharp.Core.Audio.JavazoomJL;

[PublicAPI]
public class OutputBuffer
{
    public const int BUFFERSIZE  = 2 * 1152; // max. 2 * 1152 samples per frame
    public const int MAXCHANNELS = 2;        // max. number of channels

    public byte[] Buffer { get; set; }

    private float? _replayGainScale;
    private int    _channels;
    private int[]  _channelPointer;
    private bool   _isBigEndian;

    public OutputBuffer( int channels, bool isBigEndian )
    {
        this._channels        = channels;
        this._isBigEndian     = isBigEndian;
        this.Buffer           = new byte[ BUFFERSIZE * channels ];
        this._channelPointer  = new int[ channels ];
        this._replayGainScale = 0.0f;

        Reset();
    }

    /// <summary>
    /// Takes a 16 Bit PCM sample.
    /// </summary>
    private void Append( int channel, int value )
    {
        byte firstByte;
        byte secondByte;

        if ( _isBigEndian )
        {
            firstByte  = ( byte )( ( value >>> 8 ) & 0xFF );
            secondByte = ( byte )( value & 0xFF );
        }
        else
        {
            firstByte  = ( byte )( value & 0xFF );
            secondByte = ( byte )( ( value >>> 8 ) & 0xFF );
        }

        Buffer[ _channelPointer[ channel ] ]     =  firstByte;
        Buffer[ _channelPointer[ channel ] + 1 ] =  secondByte;
        _channelPointer[ channel ]               += _channels * 2;
    }

    /// <summary>
    /// Takes 32 PCM samples.
    /// </summary>
    public void AppendSamples( int channel, float[] f )
    {
        int s;

        if ( _replayGainScale != null )
        {
            for ( var i = 0; i < 32; )
            {
                s = Clip( ( float )( f[ i++ ] * _replayGainScale ) );

                Append( channel, s );
            }
        }
        else
        {
            for ( var i = 0; i < 32; )
            {
                s = Clip( f[ i++ ] );

                Append( channel, s );
            }
        }
    }

    public int Reset()
    {
        try
        {
            var index = _channels - 1;

            return _channelPointer[ index ] - ( index * 2 );
        }
        finally
        {
            // Points to byte location, implicitely assuming 16 bit samples.
            for ( var i = 0; i < _channels; i++ )
            {
                _channelPointer[ i ] = i * 2;
            }
        }
    }

    public void SetReplayGainScale( float replayGainScale )
    {
        this._replayGainScale = replayGainScale;
    }

    public bool IsStereo() => _channelPointer[ 1 ] == 2;

    /// <summary>
    /// Clip to 16 bits.
    /// </summary>
    private int Clip( float sample )
    {
        return ( int )( sample > 32767.0f ? 32767 : sample < -32768.0f ? -32768 : sample );
    }
}
