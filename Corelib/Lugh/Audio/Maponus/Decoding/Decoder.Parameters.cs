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


using Exception = System.Exception;

namespace Corelib.Lugh.Audio.Maponus.Decoding;

public partial class Decoder
{
    /// <summary>
    /// The Parameters class presents the customizable aspects of the decoder.
    /// Instances of this class are not thread safe.
    /// </summary>
    [PublicAPI]
    public class Parameters : ICloneable
    {
        public virtual OutputChannels? OutputChannels { get; set; }

        /// <summary>
        /// Retrieves the equalizer settings that the decoder's equalizer will be
        /// initialized from. The Equalizer instance returned cannot be changed in
        /// real time to affect the decoder output as it is used only to initialize
        /// the decoders EQ settings. To affect the decoder's output in realtime,
        /// use the Equalizer returned from the getEqualizer() method on the decoder.
        /// </summary>
        /// <returns>
        /// The Equalizer used to initialize the EQ settings of the decoder.
        /// </returns>
        public virtual Equalizer? InitialEqualizerSettings => null;

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns> A new object that is a copy of this instance. </returns>
        public object Clone()
        {
            try
            {
                return MemberwiseClone();
            }
            catch ( Exception ex )
            {
                throw new ApplicationException( this + ": " + ex );
            }
        }
    }
}
