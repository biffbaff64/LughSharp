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

using System.Diagnostics;

using LibGDXSharp.Scenes.Listeners;

namespace LibGDXSharp.Scenes.Scene2D.Actions;

[PublicAPI]
public class RemoveListenerAction : Action
{
    public IEventListener? Listener { get; set; }
    public bool            Capture  { get; set; }

    public override bool Act( float delta )
    {
        Debug.Assert( Listener != null, nameof( Listener ) + " != null" );

        if ( Capture )
        {
            Target?.RemoveCaptureListener( Listener );
        }
        else
        {
            Target?.RemoveListener( Listener );
        }

        return true;
    }

    public new void Reset()
    {
        base.Reset();
        Listener = null;
    }
}