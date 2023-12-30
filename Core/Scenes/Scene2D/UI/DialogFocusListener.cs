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
public class DialogFocusListener : FocusListener
{
    private readonly Dialog _dialog;

    public DialogFocusListener( Dialog dialog )
    {
        _dialog = dialog;
    }

    public override void KeyboardFocusChanged( FocusListener.FocusEvent ev, Actor? actor, bool focused )
    {
        if ( !focused )
        {
            FocusChanged( ev );
        }
    }

    public override void ScrollFocusChanged( FocusListener.FocusEvent ev, Actor? actor, bool focused )
    {
        if ( !focused )
        {
            FocusChanged( ev );
        }
    }

    private void FocusChanged( FocusListener.FocusEvent ev )
    {
        if ( _dialog is { IsModal: true, Stage: not null }
          && ( _dialog.Stage.Root.Children.Size > 0 )
          && ( _dialog.Stage.Root.Children.Peek() == _dialog ) )
        {
            // Dialog is top most actor.
            Actor? newFocusedActor = ev.RelatedActor;

            if ( ( newFocusedActor != null )
              && !newFocusedActor.IsDescendantOf( _dialog )
              && !( newFocusedActor.Equals( _dialog.PreviousKeyboardFocus )
                 || newFocusedActor.Equals( _dialog.PreviousScrollFocus ) ) )
            {
                ev.Cancel();
            }
        }
    }
}
