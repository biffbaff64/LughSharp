// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects
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


namespace LughSharp.LibCore.Core;

/// <summary>
///     Base class for Input classes.
/// </summary>
[PublicAPI]
public abstract class AbstractInput : IInput
{
    /// <inheritdoc/>
    public IInputProcessor? InputProcessor { get; set; }

    protected bool[] PressedKeys     { get; set; } = new bool[ IInput.Keys.MAX_KEYCODE + 1 ];
    protected bool[] JustPressedKeys { get; set; } = new bool[ IInput.Keys.MAX_KEYCODE + 1 ];
    protected bool   KeyJustPressed  { get; set; } = false;
    protected int    PressedKeyCount { get; set; } = 0;

    private readonly List< int > _keysToCatch = new();

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    /// <summary>
    ///     Returns TRUE if the key identified by the supplied <see cref="IInput.Keys"/>
    ///     key code is pressed.
    /// </summary>
    public virtual bool IsKeyPressed( int key )
    {
        if ( key == IInput.Keys.ANY_KEY )
        {
            return PressedKeyCount > 0;
        }

        return key is >= 0 and <= IInput.Keys.MAX_KEYCODE && PressedKeys[ key ];
    }

    /// <summary>
    ///     Returns TRUE if the key identified by the supplied <see cref="IInput.Keys"/>
    ///     key code has <b>just</b> been pressed.
    /// </summary>
    public virtual bool IsKeyJustPressed( int key )
    {
        if ( key == IInput.Keys.ANY_KEY )
        {
            return KeyJustPressed;
        }

        return ( key is >= 0 and <= IInput.Keys.MAX_KEYCODE ) && JustPressedKeys[ key ];
    }

    // ------------------------------------------------------------------------

    #region catch keys
    
    //TODO: What are 'catch keys' in LibGDX???
    //TODO: Are these just for mobile devices???
    //TODO: Do I need to keep these???
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

    public virtual bool IsCatchKey( int keycode ) => _keysToCatch.Contains( keycode );

    public virtual bool IsCatchBackKey() => _keysToCatch.Contains( IInput.Keys.BACK );

    public virtual void SetCatchBackKey( bool catchBack ) => SetCatchKey( IInput.Keys.BACK, catchBack );

    public virtual bool IsCatchMenuKey() => _keysToCatch.Contains( IInput.Keys.MENU );

    public virtual void SetCatchMenuKey( bool catchMenu ) => SetCatchKey( IInput.Keys.MENU, catchMenu );

    #endregion catch keys

    // ------------------------------------------------------------------------
    // Abstract methods to be implemented by any inheriting classes.
    // ------------------------------------------------------------------------

    #region abstract methods

    // I'm pretty sure these are for mobile devices...
    public abstract float              GetAccelerometerX();
    public abstract float              GetAccelerometerY();
    public abstract float              GetAccelerometerZ();
    public abstract float              GetGyroscopeX();
    public abstract float              GetGyroscopeY();
    public abstract float              GetGyroscopeZ();
    public abstract int                GetMaxPointers();
    public abstract int                GetX( int pointer = 0 );
    public abstract int                GetDeltaX( int pointer = 0 );
    public abstract int                GetY( int pointer = 0 );
    public abstract int                GetDeltaY( int pointer = 0 );
    public abstract bool               IsTouched( int pointer = 0 );
    public abstract bool               JustTouched();
    public abstract float              GetPressure( int pointer = 0 );
    public abstract bool               IsButtonPressed( int button );
    public abstract bool               IsButtonJustPressed( int button );
    public abstract bool               IsPeripheralAvailable( IInput.Peripheral peripheral );
    public abstract int                GetRotation();
    public abstract IInput.Orientation GetNativeOrientation();
    public abstract void               SetCursorCaught( bool caught );
    public abstract bool               IsCursorCaught();
    public abstract void               SetCursorPosition( int x, int y );

    public abstract void GetTextInput( IInput.ITextInputListener listener,
                                       string title,
                                       string text,
                                       string hint,
                                       IInput.OnscreenKeyboardType type = IInput.OnscreenKeyboardType.Default );

    public abstract void  SetOnscreenKeyboardVisible( bool visible );
    public abstract void  SetOnscreenKeyboardVisible( bool visible, IInput.OnscreenKeyboardType type );
    public abstract void  Vibrate( int milliseconds );
    public abstract void  Vibrate( long[] pattern, int repeat );
    public abstract void  CancelVibrate();
    public abstract float GetAzimuth();
    public abstract float GetPitch();
    public abstract float GetRoll();
    public abstract void  GetRotationMatrix( float[] matrix );
    public abstract long  GetCurrentEventTime();

    #endregion abstract methods
}
