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

using LibGDXSharp.Scenes.Listeners;

namespace LibGDXSharp.Scenes.Scene2D.UI;

public class DialogChangeListener : ChangeListener
{
    private readonly Dialog _dialog;

    public DialogChangeListener( Dialog d ) => _dialog = d;

    /// <inheritdoc cref="ChangeListener.Changed" />
    public override void Changed( ChangeEvent ev, Actor? actor )
    {
        if ( ( _dialog.Values == null ) || ( actor == null ) )
        {
            return;
        }

        if ( !_dialog.Values!.ContainsKey( actor ) )
        {
            return;
        }

        while ( actor!.Parent != _dialog.ButtonTable )
        {
            actor = actor.Parent;
        }

        _dialog.Result( _dialog.Values[ actor ] );

        if ( !_dialog.CancelHide )
        {
            _dialog.Hide();
        }

        _dialog.CancelHide = false;
    }
}
