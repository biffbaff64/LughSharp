// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LibGDXSharp.Gdx.Utils.Xml;

public partial class XmlReader
{
    internal readonly static byte[] XmlActions =
    {
        0, 1, 0, 1, 1, 1, 2, 1, 3, 1, 4, 1, 5,
        1, 6, 1, 7, 2, 0, 6, 2, 1, 4, 2, 2, 4
    };

    internal readonly static byte[] XmlKeyOffsets =
    {
        0, 0, 4, 9, 14, 20, 26, 30, 35, 36, 37, 42, 46, 50, 51,
        52, 56, 57, 62, 67, 73, 79, 83, 88, 89, 90, 95, 99, 103,
        104, 108, 109, 110, 111, 112, 115
    };

    internal readonly static char[] XmlTransKeys =
    {
        ( char )32, ( char )60, ( char )9, ( char )13, ( char )32,
        ( char )47, ( char )62, ( char )9, ( char )13, ( char )32,
        ( char )47, ( char )62, ( char )9, ( char )13, ( char )32,
        ( char )47, ( char )61, ( char )62, ( char )9, ( char )13,
        ( char )32, ( char )47, ( char )61, ( char )62, ( char )9,
        ( char )13, ( char )32, ( char )61, ( char )9, ( char )13,
        ( char )32, ( char )34, ( char )39, ( char )9, ( char )13,
        ( char )34, ( char )34, ( char )32, ( char )47, ( char )62,
        ( char )9, ( char )13, ( char )32, ( char )62, ( char )9,
        ( char )13, ( char )32, ( char )62, ( char )9, ( char )13,
        ( char )39, ( char )39, ( char )32, ( char )60, ( char )9,
        ( char )13, ( char )60, ( char )32, ( char )47, ( char )62,
        ( char )9, ( char )13, ( char )32, ( char )47, ( char )62,
        ( char )9, ( char )13, ( char )32, ( char )47, ( char )61,
        ( char )62, ( char )9, ( char )13, ( char )32, ( char )47,
        ( char )61, ( char )62, ( char )9, ( char )13, ( char )32,
        ( char )61, ( char )9, ( char )13, ( char )32, ( char )34,
        ( char )39, ( char )9, ( char )13, ( char )34, ( char )34,
        ( char )32, ( char )47, ( char )62, ( char )9, ( char )13,
        ( char )32, ( char )62, ( char )9, ( char )13, ( char )32,
        ( char )62, ( char )9, ( char )13, ( char )60, ( char )32,
        ( char )47, ( char )9, ( char )13, ( char )62, ( char )62,
        ( char )39, ( char )39, ( char )32, ( char )9, ( char )13,
        ( char )0
    };

    internal readonly static byte[] XmlSingleLengths =
    {
        0, 2, 3, 3, 4, 4, 2, 3, 1, 1, 3, 2, 2, 1, 1, 2, 1, 3,
        3, 4, 4, 2, 3, 1, 1, 3, 2, 2, 1, 2, 1, 1, 1, 1, 1, 0
    };

    internal readonly static byte[] XmlRangeLengths =
    {
        0, 1, 1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 0, 1, 0, 1,
        1, 1, 1, 1, 1, 0, 0, 1, 1, 1, 0, 1, 0, 0, 0, 0, 1, 0
    };

    internal readonly static short[] XmlIndexOffsets =
    {
        0, 0, 4, 9, 14, 20, 26, 30, 35, 37, 39, 44, 48, 52, 54, 56, 60,
        62, 67, 72, 78, 84, 88, 93, 95, 97, 102, 106, 110, 112, 116, 118,
        120, 122, 124, 127
    };

    internal readonly static byte[] XmlIndicies =
    {
        0, 2, 0, 1, 2, 1, 1, 2, 3, 5, 6, 7, 5, 4, 9, 10, 1, 11, 9, 8, 13,
        1, 14, 1, 13, 12, 15, 16, 15, 1, 16, 17, 18, 16, 1, 20, 19, 22, 21,
        9, 10, 11, 9, 1, 23, 24, 23, 1, 25, 11, 25, 1, 20, 26, 22, 27, 29,
        30, 29, 28, 32, 31, 30, 34, 1, 30, 33, 36, 37, 38, 36, 35, 40, 41,
        1, 42, 40, 39, 44, 1, 45, 1, 44, 43, 46, 47, 46, 1, 47, 48, 49, 47,
        1, 51, 50, 53, 52, 40, 41, 42, 40, 1, 54, 55, 54, 1, 56, 42, 56, 1,
        57, 1, 57, 34, 57, 1, 1, 58, 59, 58, 51, 60, 53, 61, 62, 62, 1, 1, 0
    };

    internal readonly static byte[] XmlTransTargs =
    {
        1, 0, 2, 3, 3, 4, 11, 34, 5, 4, 11, 34, 5, 6, 7, 6, 7, 8, 13, 9, 10, 9,
        10, 12, 34, 12, 14, 14, 16, 15, 17, 16, 17, 18, 30, 18, 19, 26, 28, 20,
        19, 26, 28, 20, 21, 22, 21, 22, 23, 32, 24, 25, 24, 25, 27, 28, 27, 29,
        31, 35, 33, 33, 34
    };

    internal readonly static byte[] XmlTransActions =
    {
        0, 0, 0, 1, 0, 3, 3, 20, 1, 0, 0, 9, 0, 11, 11, 0, 0, 0, 0, 1, 17, 0, 13,
        5, 23, 0, 1, 0, 1, 0, 0, 0, 15, 1, 0, 0, 3, 3, 20, 1, 0, 0, 9, 0, 11, 11,
        0, 0, 0, 0, 1, 17, 0, 13, 5, 23, 0, 0, 0, 7, 1, 0, 0
    };
}
