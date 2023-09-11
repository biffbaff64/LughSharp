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

using LibGDXSharp.Utils.Collections;

namespace LibGDXSharp.Core;

[PublicAPI]
public class InputMultiplexer : IInputProcessor
{
    private readonly SnapshotArray< IInputProcessor > _processors;

    public InputMultiplexer()
    {
        _processors = new SnapshotArray< IInputProcessor >( 4 );
    }

    public InputMultiplexer( params IInputProcessor[] processors )
    {
        _processors = new SnapshotArray< IInputProcessor >();
        _processors.AddAll( processors );
    }

    public void AddProcessor( int index, IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        _processors.Insert( index, processor );
    }

    public void RemoveProcessor( int index )
    {
        _processors.RemoveAt( index );
    }

    public void AddProcessor( IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        _processors.Add( processor );
    }

    public void RemoveProcessor( IInputProcessor processor )
    {
        _processors.Remove( processor );
    }

    public int Size()
    {
        return _processors.Size;
    }

    public void Clear()
    {
        _processors.Clear();
    }

    public void SetProcessors( params IInputProcessor[] processorList )
    {
        this._processors.Clear();
        this._processors.AddAll( processorList );
    }

    public void SetProcessors( List< IInputProcessor > processorList )
    {
        this._processors.Clear();
        this._processors.AddAll( processorList.ToArray() );
    }

    public SnapshotArray< IInputProcessor > GetProcessors()
    {
        return _processors;
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
}