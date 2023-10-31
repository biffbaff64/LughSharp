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

namespace LibGDXSharp.Backends.Desktop;

[PublicAPI]
public class DefaultGLInput : AbstractInput, IGLInput
{
    private DesktopGLWindow?        _window;
    private IInputProcessor? _inputProcessor;
    private InputEventQueue  _eventQueue = new();
    private int              _mouseX;
    private int              _mouseY;
    private int              _mousePressed;
    private int              _deltaX;
    private int              _deltaY;
    private bool             _justTouched;
    private bool[]           _justPressedButtons = new bool[ 5 ];
    private char             _lastCharacter;

    public override float GetAccelerometerX()
    {
        return 0;
    }

    public override float GetAccelerometerY()
    {
        return 0;
    }

    public override float GetAccelerometerZ()
    {
        return 0;
    }

    public override float GetGyroscopeX()
    {
        return 0;
    }

    public override float GetGyroscopeY()
    {
        return 0;
    }

    public override float GetGyroscopeZ()
    {
        return 0;
    }

    public override int GetMaxPointers()
    {
        return 0;
    }

    public override int GetX( int pointer = 0 )
    {
        return 0;
    }

    public override int GetDeltaX( int pointer = 0 )
    {
        return 0;
    }

    public override int GetY( int pointer = 0 )
    {
        return 0;
    }

    public override int GetDeltaY( int pointer = 0 )
    {
        return 0;
    }

    public override bool IsTouched( int pointer = 0 )
    {
        return false;
    }

    public override bool JustTouched()
    {
        return false;
    }

    public override float GetPressure( int pointer = 0 )
    {
        return 0;
    }

    public override bool IsButtonPressed( int button )
    {
        return false;
    }

    public override bool IsButtonJustPressed( int button )
    {
        return false;
    }

    public override void SetInputProcessor( IInputProcessor processor )
    {
    }

    public override IInputProcessor GetInputProcessor()
    {
        return null;
    }

    public override bool IsPeripheralAvailable( IInput.Peripheral peripheral )
    {
        return false;
    }

    public override int GetRotation()
    {
        return 0;
    }

    public override IInput.Orientation GetNativeOrientation()
    {
        return IInput.Orientation.Landscape;
    }

    /// <remarks>Java LibGDX has this method named as SetCursorCatched(bool catched).</remarks>
    public override void SetCursorCaught( bool caught )
    {
    }

    /// <remarks>Java LibGDX has this method named as IsCursorCatched().</remarks>
    public override bool IsCursorCaught()
    {
        return false;
    }

    public override void SetCursorPosition( int x, int y )
    {
    }

    public override void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint )
    {
    }

    public override void GetTextInput( IInput.ITextInputListener listener, string title, string text, string hint, IInput.OnscreenKeyboardType type )
    {
    }

    public override void SetOnscreenKeyboardVisible( bool visible )
    {
    }

    public override void SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type )
    {
    }

    public override void Vibrate( int milliseconds )
    {
    }

    public override void Vibrate( long[] pattern, int repeat )
    {
    }

    public override void CancelVibrate()
    {
    }

    public override float GetAzimuth()
    {
        return 0;
    }

    public override float GetPitch()
    {
        return 0;
    }

    public override float GetRoll()
    {
        return 0;
    }

    public override void GetRotationMatrix( float[] matrix )
    {
    }

    public override long GetCurrentEventTime()
    {
        return 0;
    }

    public void WindowHandleChanged( long windowHandle )
    {
    }

    public void Update()
    {
    }

    public void PrepareNext()
    {
    }

    public void ResetPollingStates()
    {
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing,
    /// or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
    }
}
