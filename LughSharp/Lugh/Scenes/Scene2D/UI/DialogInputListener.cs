// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / LughSharp Team.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// ///////////////////////////////////////////////////////////////////////////////

using LughSharp.Lugh.Scenes.Scene2D.Listeners;

namespace LughSharp.Lugh.Scenes.Scene2D.UI;

[PublicAPI]
public class DialogInputListener : InputListener
{
    private readonly Dialog  _dialog;
    private readonly object? _object;
    private readonly int     _thisKey;

    public DialogInputListener( Dialog d, int thisKey, object? o )
    {
        _dialog  = d;
        _thisKey = thisKey;
        _object  = o;
    }

    /// <inheritdoc />
    public override bool KeyDown( InputEvent? inputEvent, int keycode )
    {
        if ( keycode == _thisKey )
        {
            // Delay a frame to eat the keyTyped event.
            GdxApi.App.PostRunnable( () =>
            {
                _dialog.Result( _object );

                if ( !_dialog.CancelHide )
                {
                    _dialog.Hide();
                }

                _dialog.CancelHide = false;
            } );
        }

        return false;
    }
}
