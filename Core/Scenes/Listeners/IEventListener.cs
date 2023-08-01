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

using LibGDXSharp.Scenes.Scene2D;

namespace LibGDXSharp.Scenes.Listeners;

/// <summary>
/// Low level interface for receiving events.
/// Typically there is a listener class for each specific event class.
/// </summary>
/// <see cref="InputListener"/>
/// <see cref="InputEvent"/>
public interface IEventListener
{
    /// <summary>
    /// Try to handle the given event, if it is applicable.
    /// </summary>
    /// <returns>
    /// True if the event should be considered as handled by scene2d.
    /// </returns>
    public bool Handle( Event e );
}