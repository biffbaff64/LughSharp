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

using LibGDXSharp.Scenes.Listeners;

namespace LibGDXSharp.Scenes.Scene2D.UI;

[PublicAPI]
public class DialogInputListener : InputListener
{
    private readonly Dialog  _dialog;
    private readonly int     _thisKey;
    private readonly object? _object;

    public DialogInputListener( Dialog d, int thisKey, object? o )
    {
        this._dialog  = d;
        this._thisKey = thisKey;
        this._object  = o;
    }

    /// <inheritdoc />
    public override bool KeyDown( InputEvent inputEvent, int keycode )
    {
        if ( keycode == _thisKey )
        {
            // Delay a frame to eat the keyTyped event.
            Gdx.App.PostRunnable( () =>
            {
                void Run()
                {
                    _dialog.Result( _object );

                    if ( !_dialog.CancelHide )
                    {
                        _dialog.Hide();
                    }

                    _dialog.CancelHide = false;
                }
            } );
        }

        return false;
    }
}
