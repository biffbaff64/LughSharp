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

namespace LibGDXSharp.Utils.Buffers;

[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
public abstract class FloatBuffer : Buffer
{
    protected float[]? Hb     { get; set; }
    protected int      Offset { get; set; }

    protected FloatBuffer(int mark, int pos, int lim, int cap, float[]? hb = null, int offset = 0)
        : base(mark, pos, lim, cap)
    {
        this.Hb     = hb;
        this.Offset = offset;
    }

    public float Get( int index )
    {
        return 0;
    }

    public float Get( int? vertices )
    {
        return 0;
    }

    public float Get( float[] vertices, int destOffset, int count )
    {
        return 0;
    }
}
