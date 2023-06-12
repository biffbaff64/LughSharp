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

namespace LibGDXSharp.Graphics;

public struct DisplayMode
{
    public int Width        { get; set; }
    public int Height       { get; set; }
    public int RefreshRate  { get; set; }
    public int BitsPerPixel { get; set; }

    public DisplayMode( int width, int height, int refreshRate, int bitsPerPixel )
    {
        this.Width        = width;
        this.Height       = height;
        this.RefreshRate  = refreshRate;
        this.BitsPerPixel = bitsPerPixel;
    }

    public new string ToString()
    {
        return Width + "x" + Height + ", bpp: " + BitsPerPixel + ", hz: " + RefreshRate;
    }
}