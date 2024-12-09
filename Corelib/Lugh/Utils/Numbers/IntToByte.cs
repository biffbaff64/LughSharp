// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / LughSharp Team
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

namespace Corelib.Lugh.Utils.Numbers;

[PublicAPI]
public interface IIntToByte
{
    int Int { get; set; }

    byte B0 { get; }
    byte B1 { get; }
    byte B2 { get; }
    byte B3 { get; }
}

[PublicAPI]
[StructLayout( LayoutKind.Explicit )]
public struct IntToByteLE : IIntToByte
{
    [FieldOffset( 0 )]
    public int IntVal;

    [FieldOffset( 0 )]
    public byte b0;

    [FieldOffset( 1 )]
    public byte b1;

    [FieldOffset( 2 )]
    public byte b2;

    [FieldOffset( 3 )]
    public byte b3;

    public int Int
    {
        get => IntVal;
        set => IntVal = value;
    }

    public byte B0 => b0;
    public byte B1 => b1;
    public byte B2 => b2;
    public byte B3 => b3;
}

[PublicAPI]
[StructLayout( LayoutKind.Explicit )]
public struct IntToByteBE : IIntToByte  //TODO:
{
    [FieldOffset( 0 )]
    public int IntVal;

    [FieldOffset( 0 )]
    public byte b0;

    [FieldOffset( 1 )]
    public byte b1;

    [FieldOffset( 2 )]
    public byte b2;

    [FieldOffset( 3 )]
    public byte b3;

    public int Int
    {
        get => IntVal;
        set => IntVal = value;
    }

    public byte B0 => b0;
    public byte B1 => b1;
    public byte B2 => b2;
    public byte B3 => b3;
}