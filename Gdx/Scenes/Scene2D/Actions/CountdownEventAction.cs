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

namespace LibGDXSharp.Scenes.Scene2D.Actions;

/// <summary>
///     An EventAction that is complete once it receives X number of events.
/// </summary>
public class CountdownEventAction<T> : EventAction< T > where T : Event
{
    private readonly int _count;
    private          int _current;

    public CountdownEventAction( T eventClass, int count )
        : base( eventClass ) => _count = count;

    public override bool HandleDelegate( Event ev )
    {
        _current++;

        return _current >= _count;
    }
}
