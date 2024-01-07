// ///////////////////////////////////////////////////////////////////////////////
// // Copyright [2023] [Richard Ikin]
// //
// // Licensed under the Apache License, Version 2.0 (the "License");
// // you may not use this file except in compliance with the License.
// // You may obtain a copy of the License at
// //
// // http: //www.apache.org/licenses/LICENSE-2.0
// //
// // Unless required by applicable law or agreed to in writing, software
// // distributed under the License is distributed on an "AS IS" BASIS,
// // WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// // See the License for the specific language governing permissions and
// // limitations under the License.
// ///////////////////////////////////////////////////////////////////////////////

namespace LibGDXSharp.Audio.MP3Sharp;

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
