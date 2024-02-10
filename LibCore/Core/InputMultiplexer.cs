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


using LibGDXSharp.LibCore.Utils.Collections;

namespace LibGDXSharp.LibCore.Core;

public class InputMultiplexer : IInputProcessor
{
    private readonly SnapshotArray< IInputProcessor > _processors = new( 4 );

    public InputMultiplexer()
    {
    }

    public InputMultiplexer( params IInputProcessor[] processors )
    {
        foreach ( IInputProcessor inputProcessor in processors )
        {
            _processors.Add( inputProcessor );
        }
    }

    public bool KeyDown( int keycode )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.KeyDown( keycode ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool KeyUp( int keycode )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.KeyUp( keycode ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool KeyTyped( char character )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.KeyTyped( character ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchDown( int screenX, int screenY, int pointer, int button )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.TouchDown( screenX, screenY, pointer, button ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchUp( int screenX, int screenY, int pointer, int button )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.TouchUp( screenX, screenY, pointer, button ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchDragged( int screenX, int screenY, int pointer )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.TouchDragged( screenX, screenY, pointer ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool MouseMoved( int screenX, int screenY )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.MouseMoved( screenX, screenY ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool Scrolled( float amountX, float amountY )
    {
        IInputProcessor?[] items = _processors.Begin();

        try
        {
            for ( int i = 0, n = _processors.Size; i < n; i++ )
            {
                if ( items[ i ]!.Scrolled( amountX, amountY ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            _processors.End();
        }

        return false;
    }

    public void AddProcessor( int index, IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        _processors.Insert( index, processor );
    }

    public void RemoveProcessor( int index ) => _processors.RemoveAt( index );

    public void AddProcessor( IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        _processors.Add( processor );
    }

    public void RemoveProcessor( IInputProcessor processor ) => _processors.Remove( processor );

    public int Size() => _processors.Size;

    public void Clear() => _processors.Clear();

    public void SetProcessors( params IInputProcessor[] processorList )
    {
        _processors.Clear();

        foreach ( IInputProcessor inputProcessor in processorList )
        {
            _processors.Add( inputProcessor );
        }
    }

    public void SetProcessors( List< IInputProcessor > processorList )
    {
        _processors.Clear();

        foreach ( IInputProcessor inputProcessor in processorList )
        {
            _processors.Add( inputProcessor );
        }
    }

    public SnapshotArray< IInputProcessor > GetProcessors() => _processors;
}
