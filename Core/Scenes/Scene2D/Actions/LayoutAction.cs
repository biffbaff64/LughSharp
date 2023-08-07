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

using LibGDXSharp.Scenes.Scene2D.Utils;
using LibGDXSharp.Utils;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

public class LayoutAction : Action
{
    public bool Enabled { get; set; }

    public void SetTarget( Actor actor )
    {
        if ( ( actor != null ) && ( actor is not ILayout ) )
        {
            throw new GdxRuntimeException( "Actor must implement layout: " + actor );
        }

        base.Target = actor;
    }

    /// <summary>
    /// Updates the action based on time.
    /// Typically this is called each frame by <see cref="Actor"/>.
    /// </summary>
    /// <param name="delta">Time in seconds since the last frame.</param>
    /// <returns>
    /// true if the action is done. This method may continue to be called after
    /// the action is done.
    /// </returns>
    public override bool Act( float delta )
    {
        if ( base.Target == null ) return false;
        
        ( ( ILayout )base.Target ).LayoutEnabled = Enabled;

        return true;
    }
}
