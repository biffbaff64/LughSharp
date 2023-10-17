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

[PublicAPI]
public class HeapFloatBuffer : FloatBuffer
{
    public HeapFloatBuffer( int cap, int lim )
        : base( -1, 0, lim, cap, new float[cap], 0 )
    {
    }

    public HeapFloatBuffer( float[] buf, int off, int len )
        : base( -1, off, off + len, buf.Length, buf, 0 )
    {
    }

    protected HeapFloatBuffer( float[] buf, int mark, int pos, int lim, int cap, int off )
        : base( mark, pos, lim, cap, buf, off )
    {
    }
}
