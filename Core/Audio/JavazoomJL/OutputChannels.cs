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
public class OutputChannels
{
    // Flag to indicate output should include both channels.
    public const int BOTH_CHANNELS = 0;

    // Flag to indicate output should include the left channel only.
    public const int LEFT_CHANNEL = 1;

    // Flag to indicate output should include the right channel only.
    public const int RIGHT_CHANNEL = 2;

    // Flag to indicate output is mono.
    public const int DOWNMIX_CHANNELS = 3;

    public readonly static OutputChannels Left    = new OutputChannels( LEFT_CHANNEL );
    public readonly static OutputChannels Right   = new OutputChannels( RIGHT_CHANNEL );
    public readonly static OutputChannels Both    = new OutputChannels( BOTH_CHANNELS );
    public readonly static OutputChannels Downmix = new OutputChannels( DOWNMIX_CHANNELS );

    public int Channels { get; set; }

    private OutputChannels( int channels )
    {
        Channels = channels;

        if ( ( channels < 0 ) || ( channels > 3 ) )
        {
            throw new ArgumentException( "channels" );
        }
    }

    /// <summary>
    /// Creates an <tt>OutputChannels</tt> instance corresponding to the given channel code.
    /// </summary>
    /// <param name="code"> one of the OutputChannels channel code constants. </param>
    /// <exception cref="ArgumentException"> if code is not a valid channel code. </exception>
    public OutputChannels FromInt( int code )
    {
        switch ( code )
        {
            case LEFT_CHANNEL:
                return Left;

            case RIGHT_CHANNEL:
                return Right;

            case BOTH_CHANNELS:
                return Both;

            case DOWNMIX_CHANNELS:
                return Downmix;

            default:
                throw new ArgumentException( "Invalid channel code: " + code );
        }
    }

    /// <summary>
    /// Retrieves the code representing the desired output channels. Will be
    /// one of LEFT_CHANNEL, RIGHT_CHANNEL, BOTH_CHANNELS or DOWNMIX_CHANNELS.
    /// </summary>
    /// <returns> the channel code represented by this instance. </returns>
    public int GetChannelsOutputCode()
    {
        return Channels;
    }

    public int ChannelCount() => Channels == BOTH_CHANNELS ? 2 : 1;

    public new bool Equals( object? o )
    {
        var equals = false;

        if ( o is OutputChannels oc )
        {
            equals = oc.Channels == Channels;
        }

        return equals;
    }
}
