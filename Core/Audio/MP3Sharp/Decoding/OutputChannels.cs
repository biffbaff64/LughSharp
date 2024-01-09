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

namespace LibGDXSharp.Audio.MP3Sharp.Decoding;

/// <summary>
///     A Type-safe representation of the the supported output channel constants.
///     This class is immutable and, hence, is thread safe.
/// </summary>
public class OutputChannels
{
    /// <summary>
    ///     Flag to indicate output should include both channels.
    /// </summary>
    public const int BOTH_CHANNELS = 0;

    /// <summary>
    ///     Flag to indicate output should include the left channel only.
    /// </summary>
    public const int LEFT_CHANNEL = 1;

    /// <summary>
    ///     Flag to indicate output should include the right channel only.
    /// </summary>
    public const int RIGHT_CHANNEL = 2;

    /// <summary>
    ///     Flag to indicate output is mono.
    /// </summary>
    public const int DOWNMIX_CHANNELS = 3;

    public readonly static OutputChannels Left    = new( LEFT_CHANNEL );
    public readonly static OutputChannels Right   = new( RIGHT_CHANNEL );
    public readonly static OutputChannels Both    = new( BOTH_CHANNELS );
    public readonly static OutputChannels DownMix = new( DOWNMIX_CHANNELS );

    private readonly int _outputChannels;

    private OutputChannels( int channels )
    {
        _outputChannels = channels;

        if ( channels is < 0 or > 3 )
        {
            throw new ArgumentException( $"channels is wrong: {channels}" );
        }
    }

    /// <summary>
    ///     Retrieves the code representing the desired output channels. Will be one of LEFT_CHANNEL,
    ///     RIGHT_CHANNEL, BOTH_CHANNELS or DOWNMIX_CHANNELS.
    /// </summary>
    /// <returns>
    ///     the channel code represented by this instance.
    /// </returns>
    public virtual int ChannelsOutputCode => _outputChannels;

    /// <summary>
    ///     Retrieves the number of output channels for this channel output
    ///     type. This will be 2 for BOTH_CHANNELS only, and 1 for all other types.
    /// </summary>
    public virtual int ChannelCount
    {
        get
        {
            var count = _outputChannels == BOTH_CHANNELS ? 2 : 1;

            return count;
        }
    }

    /// <summary>
    ///     Creates an OutputChannels instance corresponding to the given channel code.
    /// </summary>
    /// <param name="code"> one of the OutputChannels channel code constants. </param>
    /// <exception cref="ArgumentException"> if code is not a valid channel code.</exception>
    public static OutputChannels FromInt( int code ) => code switch
                                                        {
                                                            ( int )OutputChannelsEnum.LeftChannel => Left,
                                                            ( int )OutputChannelsEnum.RightChannel => Right,
                                                            ( int )OutputChannelsEnum.BothChannels => Both,
                                                            ( int )OutputChannelsEnum.DownmixChannels => DownMix,
                                                            _ => throw new ArgumentException( "Invalid channel code: " + code )
                                                        };

    public override bool Equals( object? obj )
    {
        var equals = false;

        if ( obj is OutputChannels oc )
        {
            equals = oc._outputChannels == _outputChannels;
        }

        return equals;
    }

    public override int GetHashCode() => _outputChannels;
}
