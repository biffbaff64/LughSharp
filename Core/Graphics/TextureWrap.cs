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

using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public class TextureWrap
{
    public readonly static TextureWrap MirroredRepeat = new( "MirroredRepeat", InnerEnum.MirroredRepeat, IGL20.GL_MIRRORED_REPEAT );
    public readonly static TextureWrap ClampToEdge    = new( "ClampToEdge", InnerEnum.ClampToEdge, IGL20.GL_CLAMP_TO_EDGE );
    public readonly static TextureWrap Repeat         = new( "Repeat", InnerEnum.Repeat, IGL20.GL_REPEAT );

    private readonly static List< TextureWrap > ValueList = new();

    static TextureWrap()
    {
        ValueList.Add( MirroredRepeat );
        ValueList.Add( ClampToEdge );
        ValueList.Add( Repeat );
    }

    public enum InnerEnum
    {
        MirroredRepeat,
        ClampToEdge,
        Repeat
    }

    public readonly  InnerEnum innerEnumValue;
    private readonly string    _nameValue;
    private static   int       _nextOrdinal = 0;

    public TextureWrap( string name, InnerEnum innerEnum, int glEnum )
    {
        this.GLEnum = glEnum;

        _nameValue     = name;
        OrdinalValue   = _nextOrdinal++;
        innerEnumValue = innerEnum;
    }

    public int GLEnum       { get; }
    public int OrdinalValue { get; set; }

    public static TextureWrap[] Values() => ValueList.ToArray();

    public static TextureWrap ValueOf( string name )
    {
        foreach ( TextureWrap enumInstance in TextureWrap.ValueList )
        {
            if ( enumInstance._nameValue == name )
            {
                return enumInstance;
            }
        }

        throw new System.ArgumentException( name );
    }

    public override string ToString()
    {
        return _nameValue;
    }
}