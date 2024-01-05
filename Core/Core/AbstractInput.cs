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

namespace LibGDXSharp.Core;

public abstract class AbstractInput : IInput
{

    private readonly List< int > _keysToCatch;

    /// <summary>
    /// </summary>
    protected AbstractInput()
    {
        PressedKeys     = new bool[ IInput.Keys.MAX_KEYCODE + 1 ];
        JustPressedKeys = new bool[ IInput.Keys.MAX_KEYCODE + 1 ];
        _keysToCatch    = new List< int >();
    }

    protected bool[]           PressedKeys     { get; set; }
    protected bool[]           JustPressedKeys { get; set; }
    protected bool             KeyJustPressed  { get; set; }
    protected int              PressedKeyCount { get; set; }
    public    IInputProcessor? InputProcessor  { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual bool IsKeyPressed( int key )
    {
        if ( key == IInput.Keys.ANY_KEY )
        {
            return PressedKeyCount > 0;
        }

        return key is >= 0 and <= IInput.Keys.MAX_KEYCODE && PressedKeys[ key ];
    }

    /// <summary>
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual bool IsKeyJustPressed( int key )
    {
        if ( key == IInput.Keys.ANY_KEY )
        {
            return KeyJustPressed;
        }

        return key is >= 0 and <= IInput.Keys.MAX_KEYCODE && JustPressedKeys[ key ];
    }

    /// <summary>
    /// </summary>
    /// <param name="keycode"></param>
    /// <param name="catchKey"></param>
    public virtual void SetCatchKey( int keycode, bool catchKey )
    {
        if ( !catchKey )
        {
            _keysToCatch.Remove( keycode );
        }
        else
        {
            _keysToCatch.Add( keycode );
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="keycode"></param>
    /// <returns></returns>
    public virtual bool IsCatchKey( int keycode ) => _keysToCatch.Contains( keycode );

    // ------------------------------------------------------------------------
    // Abstract methods to be implemented by any inheriting classes.
    // ------------------------------------------------------------------------

    public abstract float GetAccelerometerX();

    public abstract float GetAccelerometerY();

    public abstract float GetAccelerometerZ();

    public abstract float GetGyroscopeX();

    public abstract float GetGyroscopeY();

    public abstract float GetGyroscopeZ();

    public abstract int GetMaxPointers();

    public abstract int GetX( int pointer = 0 );

    public abstract int GetDeltaX( int pointer = 0 );

    public abstract int GetY( int pointer = 0 );

    public abstract int GetDeltaY( int pointer = 0 );

    public abstract bool IsTouched( int pointer = 0 );

    public abstract bool JustTouched();

    public abstract float GetPressure( int pointer = 0 );

    public abstract bool IsButtonPressed( int button );

    public abstract bool IsButtonJustPressed( int button );

    public abstract bool IsPeripheralAvailable( IInput.Peripheral peripheral );

    public abstract int GetRotation();

    public abstract IInput.Orientation GetNativeOrientation();

    public abstract void SetCursorCaught( bool caught );

    public abstract bool IsCursorCaught();

    public abstract void SetCursorPosition( int x, int y );

    public abstract void GetTextInput( IInput.ITextInputListener listener,
                                       string title,
                                       string text,
                                       string hint,
                                       IInput.OnscreenKeyboardType type = IInput.OnscreenKeyboardType.Default );

    public abstract void SetOnscreenKeyboardVisible( bool visible );

    public abstract void SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type );

    public abstract void Vibrate( int milliseconds );

    public abstract void Vibrate( long[] pattern, int repeat );

    public abstract void CancelVibrate();

    public abstract float GetAzimuth();

    public abstract float GetPitch();

    public abstract float GetRoll();

    public abstract void GetRotationMatrix( float[] matrix );

    public abstract long GetCurrentEventTime();

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCatchBackKey() => _keysToCatch.Contains( IInput.Keys.BACK );

    /// <summary>
    /// </summary>
    /// <param name="catchBack"></param>
    public virtual void SetCatchBackKey( bool catchBack ) => SetCatchKey( IInput.Keys.BACK, catchBack );

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual bool IsCatchMenuKey() => _keysToCatch.Contains( IInput.Keys.MENU );

    /// <summary>
    /// </summary>
    /// <param name="catchMenu"></param>
    public virtual void SetCatchMenuKey( bool catchMenu ) => SetCatchKey( IInput.Keys.MENU, catchMenu );
}
