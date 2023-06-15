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

namespace LibGDXSharp.Scenes.Scene2D.UI;

/// Manages a group of buttons to enforce a minimum and maximum number of checked
/// buttons. This enables "radio button" functionality and more. A button may only
/// be in one group at a time.
/// <para>
/// The <see cref="CanCheck(T, bool)"/> method can be overridden to control
/// if a button check or uncheck is allowed.
/// </para>
public class ButtonGroup<T> where T : Button
{
    private readonly List< T > _buttons        = new();
    private readonly List< T > _checkedButtons = new( 1 );

    private int  _minCheckCount;
    private int  _maxCheckCount = 1;
    private bool _uncheckLast   = true;
    private T    _lastChecked   = null!;

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
        if ( button == null ) throw new ArgumentException( "button cannot be null." );

        button.ButtonGroup = null;

        var shouldCheck = ( button.IsChecked || ( _buttons.Count < _minCheckCount ) );

        button.SetChecked( false );
        button.ButtonGroup = this;
        _buttons.Add( button );
        button.SetChecked( shouldCheck );
    }

    public void Add( params T[] buttons )
    {
        if ( buttons == null ) throw new ArgumentException( "buttons cannot be null." );

        for ( int i = 0, n = buttons.Length; i < n; i++ )
        {
            Add( buttons[ i ] );
        }
    }

    public void Remove( T button )
    {
        if ( button == null ) throw new ArgumentException( "button cannot be null." );

        button.ButtonGroup = null;
        
        _buttons.Remove( button );
        _checkedButtons.Remove( button );
    }

    public void Remove( params T[] buttons )
    {
        if ( buttons == null ) throw new ArgumentException( "buttons cannot be null." );

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
        if ( text == null ) throw new ArgumentException( "text cannot be null." );

        for ( int i = 0, n = _buttons.Count; i < n; i++ )
        {
            Button button = _buttons[ i ];

            if ( ( button.GetType() == typeof( TextButton ) )
                 && text.Equals( ( ( TextButton )button ).GetText() ) )
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
    protected bool CanCheck( T button, bool newState )
    {
        if ( button.IsChecked == newState ) return false;

        if ( !newState )
        {
            // Keep button checked to enforce minCheckCount.
            if ( _checkedButtons.Count <= _minCheckCount ) return false;

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
            T button = _buttons[ i ];
            button.SetChecked( false );
        }

        _minCheckCount = old;
    }

    /// <returns> The first checked button, or null. </returns>
    public T? GetChecked()
    {
        return _checkedButtons.Count > 0 ? _checkedButtons[ 0 ] : null;
    }

    /// <returns> The first checked button index, or -1. </returns>
    public int GetCheckedIndex()
    {
        if ( _checkedButtons.Count > 0 ) return _buttons.IndexOf( _checkedButtons[ 0 ] );

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
        this._minCheckCount = minCheckCount;
    }

    /// <summary>
    /// Sets the maximum number of buttons that can be checked.
    /// Set to -1 for no maximum. Default is 1.
    /// </summary>
    public void SetMaxCheckCount( int maxCheckCount )
    {
        if ( maxCheckCount == 0 ) maxCheckCount = -1;
        
        this._maxCheckCount = maxCheckCount;
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
        this._uncheckLast = uncheckLast;
    }
}
