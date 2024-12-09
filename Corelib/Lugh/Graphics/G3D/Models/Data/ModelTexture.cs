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


namespace Corelib.Lugh.Graphics.G3D.Models.Data;

public class ModelTexture
{
    public const int USAGE_UNKNOWN      = 0;
    public const int USAGE_NONE         = 1;
    public const int USAGE_DIFFUSE      = 2;
    public const int USAGE_EMISSIVE     = 3;
    public const int USAGE_AMBIENT      = 4;
    public const int USAGE_SPECULAR     = 5;
    public const int USAGE_SHININESS    = 6;
    public const int USAGE_NORMAL       = 7;
    public const int USAGE_BUMP         = 8;
    public const int USAGE_TRANSPARENCY = 9;
    public const int USAGE_REFLECTION   = 10;

    public string?  ID            { get; set; }
    public string?  FileName      { get; set; }
    public Vector2? UVTranslation { get; set; }
    public Vector2? UVScaling     { get; set; }
    public int      Usage         { get; set; }
}
