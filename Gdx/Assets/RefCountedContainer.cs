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

namespace LibGDXSharp.Assets;

/// <summary>
///     A class that stores a reference to an object, as well as counts the number
///     of times it has been referenced. <see cref="RefCount" /> must be incremented
///     each time you start using the object, and decrement it after you're done
///     using it. AssetManager handles this automatically.
/// </summary>
[PublicAPI]
public class RefCountedContainer : IRefCountedContainer
{
    public RefCountedContainer( object obj )
    {
        Asset = obj ?? throw new ArgumentException( "Object must not be null" );
    }

    public int     RefCount { get; set; } = 1;
    public object? Asset    { get; set; }
}

internal interface IRefCountedContainer
{
    object? Asset    { get; }
    int     RefCount { get; set; }
}
