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


namespace Corelib.LibCore.Audio.Maponus.Decoding;

/// <summary>
/// A Type-safe representation of the the supported output channel constants.
/// This class is immutable and, hence, is thread safe.
/// </summary>
[PublicAPI]
public class OutputChannels
{
    /// <summary>
    /// Enumeration of Audio output channels.
    /// </summary>
    [PublicAPI]
    public enum OutputChannelsEnum
    {
        BothChannels    = 0,
        LeftChannel     = 1,
        RightChannel    = 2,
        DownmixChannels = 3,
    }

    /// <summary>
    /// Flag to indicate output should include both channels.
    /// </summary>
    public const int BOTH_CHANNELS = 0;

    /// <summary>
    /// Flag to indicate output should include the left channel only.
    /// </summary>
    public const int LEFT_CHANNEL = 1;

    /// <summary>
    /// Flag to indicate output should include the right channel only.
    /// </summary>
    public const int RIGHT_CHANNEL = 2;

    /// <summary>
    /// Flag to indicate output is mono.
    /// </summary>
    public const int DOWNMIX_CHANNELS = 3;

    public static readonly OutputChannels Left    = new( LEFT_CHANNEL );
    public static readonly OutputChannels Right   = new( RIGHT_CHANNEL );
    public static readonly OutputChannels Both    = new( BOTH_CHANNELS );
    public static readonly OutputChannels DownMix = new( DOWNMIX_CHANNELS );

    private readonly int _outputChannels;

    // ========================================================================
    
    // private constructor for use when declaring new static instances.
    private OutputChannels( int channels )
    {
        _outputChannels = channels;

        if ( channels is < 0 or > 3 )
        {
            throw new ArgumentException( $"channels is wrong: {channels}" );
        }
    }

    /// <summary>
    /// Retrieves the code representing the desired output channels. Will be one of LEFT_CHANNEL,
    /// RIGHT_CHANNEL, BOTH_CHANNELS or DOWNMIX_CHANNELS.
    /// </summary>
    /// <returns>
    /// the channel code represented by this instance.
    /// </returns>
    public virtual int ChannelsOutputCode => _outputChannels;

    /// <summary>
    /// Retrieves the number of output channels for this channel output
    /// type. This will be 2 for BOTH_CHANNELS only, and 1 for all other types.
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
    /// Creates an OutputChannels instance corresponding to the given channel code.
    /// </summary>
    /// <param name="code"> one of the OutputChannels channel code constants. </param>
    /// <exception cref="ArgumentException"> if code is not a valid channel code.</exception>
    public static OutputChannels FromInt( int code )
    {
        return code switch
        {
            ( int ) OutputChannelsEnum.LeftChannel     => Left,
            ( int ) OutputChannelsEnum.RightChannel    => Right,
            ( int ) OutputChannelsEnum.BothChannels    => Both,
            ( int ) OutputChannelsEnum.DownmixChannels => DownMix,
            var _                                      => throw new ArgumentException( "Invalid channel code: " + code ),
        };
    }

    /// <inheritdoc />
    public override bool Equals( object? obj )
    {
        var equals = false;

        if ( obj is OutputChannels oc )
        {
            equals = oc._outputChannels == _outputChannels;
        }

        return equals;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _outputChannels;
    }
}
