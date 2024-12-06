// ///////////////////////////////////////////////////////////////////////////////
// MIT License
//
// Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
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


namespace Corelib.Lugh.Scenes.Scene2D.UI;

/// <summary>
/// Manages a group of buttons to enforce a minimum and maximum number of checked
/// buttons. This enables "radio button" functionality and more. A button may only
/// be in one group at a time.
/// <para>
/// The <see cref="CanCheck(T, bool)"/> method can be overridden to control
/// if a button check or uncheck is allowed.
/// </para>
/// </summary>
[PublicAPI]
public class ButtonGroup< T > where T : Button
{
    private readonly List< T > _buttons        = new();
    private readonly List< T > _checkedButtons = new( 1 );
    private          T         _lastChecked    = null!;
    private          int       _maxCheckCount  = 1;

    private int  _minCheckCount;
    private bool _uncheckLast = true;

    // ========================================================================

    public ButtonGroup()
    {
        _minCheckCount = 1;
    }

    public ButtonGroup( params T[] buttons )
    {
        _minCheckCount = 0;

        Add( buttons );

        _minCheckCount = 1;
    }

    public void Add( T button )
    {
        ArgumentNullException.ThrowIfNull( button );

        button.ButtonGroup = null!;

        var shouldCheck = button.IsChecked || ( _buttons.Count < _minCheckCount );

        button.SetChecked( false );
        button.ButtonGroup = ( this as ButtonGroup< Button > )!;

        _buttons.Add( button );

        button.SetChecked( shouldCheck );
    }

    public void Add( params T[] buttons )
    {
        if ( buttons == null )
        {
            throw new ArgumentException( "buttons cannot be null." );
        }

        for ( int i = 0, n = buttons.Length; i < n; i++ )
        {
            Add( buttons[ i ] );
        }
    }

    public void Remove( T button )
    {
        if ( button == null )
        {
            throw new ArgumentException( "button cannot be null." );
        }

        button.ButtonGroup = null!;

        _buttons.Remove( button );
        _checkedButtons.Remove( button );
    }

    public void Remove( params T[] buttons )
    {
        if ( buttons == null )
        {
            throw new ArgumentException( "buttons cannot be null." );
        }

        for ( int i = 0, n = buttons.Length; i < n; i++ )
        {
            Remove( buttons[ i ] );
        }
    }

    public void Clear()
    {
        _buttons.Clear();
        _checkedButtons.Clear();
    }

    /// <summary>
    /// Sets the first <see cref="TextButton"/> with the specified text to checked.
    /// </summary>
    public void SetChecked( string text )
    {
        if ( text == null )
        {
            throw new ArgumentException( "text cannot be null." );
        }

        for ( int i = 0, n = _buttons.Count; i < n; i++ )
        {
            Button button = _buttons[ i ];

            if ( ( button.GetType() == typeof( TextButton ) )
              && text.Equals( ( ( TextButton ) button ).GetText() ) )
            {
                button.SetChecked( true );

                return;
            }
        }
    }

    /// <summary>
    /// Called when a button is checked or unchecked. If overridden, generally
    /// changing button checked states should not be done from within this method.
    /// </summary>
    /// <returns> True if the new state should be allowed. </returns>
    public bool CanCheck( T button, bool newState )
    {
        if ( button.IsChecked == newState )
        {
            return false;
        }

        if ( !newState )
        {
            // Keep button checked to enforce minCheckCount.
            if ( _checkedButtons.Count <= _minCheckCount )
            {
                return false;
            }

            _checkedButtons.Remove( button );
        }
        else
        {
            // Keep button unchecked to enforce maxCheckCount.
            if ( ( _maxCheckCount != -1 ) && ( _checkedButtons.Count >= _maxCheckCount ) )
            {
                if ( _uncheckLast )
                {
                    var old = _minCheckCount;

                    _minCheckCount = 0;
                    _lastChecked.SetChecked( false );
                    _minCheckCount = old;
                }
                else
                {
                    return false;
                }
            }

            _checkedButtons.Add( button );
            _lastChecked = button;
        }

        return true;
    }

    /// <summary>
    /// Sets all buttons' <see cref="Button.IsChecked"/> to false, regardless
    /// of <see cref="SetMinCheckCount(int)"/>.
    /// </summary>
    public void UncheckAll()
    {
        var old = _minCheckCount;

        _minCheckCount = 0;

        for ( int i = 0, n = _buttons.Count; i < n; i++ )
        {
            var button = _buttons[ i ];
            button.SetChecked( false );
        }

        _minCheckCount = old;
    }

    /// <summary>
    /// Returns the first checked button, or null.
    /// </summary>
    public T? GetChecked()
    {
        return _checkedButtons.Count > 0 ? _checkedButtons[ 0 ] : null;
    }

    /// <summary>
    /// Returns the first checked button index, or -1.
    /// </summary>
    public int GetCheckedIndex()
    {
        if ( _checkedButtons.Count > 0 )
        {
            return _buttons.IndexOf( _checkedButtons[ 0 ] );
        }

        return -1;
    }

    public List< T > GetAllChecked()
    {
        return _checkedButtons;
    }

    public List< T > GetButtons()
    {
        return _buttons;
    }

    /// <summary>
    /// Sets the minimum number of buttons that must be checked. Default is 1.
    /// </summary>
    public void SetMinCheckCount( int minCheckCount )
    {
        _minCheckCount = minCheckCount;
    }

    /// <summary>
    /// Sets the maximum number of buttons that can be checked.
    /// Set to -1 for no maximum. Default is 1.
    /// </summary>
    public void SetMaxCheckCount( int maxCheckCount )
    {
        if ( maxCheckCount == 0 )
        {
            maxCheckCount = -1;
        }

        _maxCheckCount = maxCheckCount;
    }

    /// <summary>
    /// If true, when the maximum number of buttons are checked and an
    /// additional button is checked, the last button to be checked
    /// is unchecked so that the maximum is not exceeded. If false,
    /// additional buttons beyond the maximum are not allowed to be
    /// checked. Default is true.
    /// </summary>
    public void SetUncheckLast( bool uncheckLast )
    {
        _uncheckLast = uncheckLast;
    }
}
