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


namespace LughSharp.Lugh.Audio.Maponus.Decoding.Decoders.LayerIII;

[PublicAPI]
public class GranuleInfo
{
    public int   BigValues           { get; set; }
    public int   BlockType           { get; set; }
    public int   Count1TableSelect   { get; set; }
    public int   GlobalGain          { get; set; }
    public int   MixedBlockFlag      { get; set; }
    public int   Part23Length        { get; set; }
    public int   Preflag             { get; set; }
    public int   Region0Count        { get; set; }
    public int   Region1Count        { get; set; }
    public int   ScaleFacCompress    { get; set; }
    public int   ScaleFacScale       { get; set; }
    public int[] SubblockGain        { get; } = new int[ 3 ];
    public int[] TableSelect         { get; } = new int[ 3 ];
    public int   WindowSwitchingFlag { get; set; }
}
