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

namespace LibGDXSharp.GdxCore.Utils.Pooling;

/// <summary>
/// Pool that creates new instances of a type using reflection.
/// The type must have a zero argument constructor.
/// </summary>
/// <typeparam name="T"></typeparam>
[SuppressMessage( "ReSharper", "MemberCanBeInternal" )]
[SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" )]
public class ReflectionPool<T> : Pool<T>
{
    public ReflectionPool( int initialCapacity = 16, int max = int.MaxValue )
        : base( initialCapacity, max )
    {
    }

    protected new T NewObject()
    {
        return Activator.CreateInstance< T >();
    }
}