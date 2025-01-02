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

using LughSharp.Lugh.Utils.Collections;

namespace LughSharp.Lugh.Core;

[PublicAPI]
public class InputMultiplexer : IInputProcessor
{
    public SnapshotArray< IInputProcessor > Processors { get; set; } = new( 4 );

    // ========================================================================
    
    /// <summary>
    /// Constructor.
    /// Creates a new InputMultiplexer. It will not contain any Input Processors,
    /// these will need adding seperately.
    /// </summary>
    public InputMultiplexer()
    {
    }

    /// <summary>
    /// Constructor.
    /// Creats a new InputMultiplexer. The supplied <see cref="IInputProcessor"/>(s)
    /// will be added to the multiplexer.
    /// </summary>
    public InputMultiplexer( params IInputProcessor[] processors )
    {
        foreach ( var inputProcessor in processors )
        {
            Processors.Add( inputProcessor );
        }
    }

    /// <inheritdoc />
    public bool KeyDown( int keycode )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].KeyDown( keycode ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool KeyUp( int keycode )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].KeyUp( keycode ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool KeyTyped( char character )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].KeyTyped( character ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchDown( int screenX, int screenY, int pointer, int button )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].TouchDown( screenX, screenY, pointer, button ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchUp( int screenX, int screenY, int pointer, int button )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].TouchUp( screenX, screenY, pointer, button ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool TouchDragged( int screenX, int screenY, int pointer )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].TouchDragged( screenX, screenY, pointer ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool MouseMoved( int screenX, int screenY )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].MouseMoved( screenX, screenY ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <inheritdoc />
    public bool Scrolled( float amountX, float amountY )
    {
        IInputProcessor[] items = Processors.Begin();

        try
        {
            for ( int i = 0, n = Processors.Size; i < n; i++ )
            {
                if ( items[ i ].Scrolled( amountX, amountY ) )
                {
                    return true;
                }
            }
        }
        finally
        {
            Processors.End();
        }

        return false;
    }

    /// <summary>
    /// Inserts an <see cref="IInputProcessor"/> into the list of processors.
    /// This processor will be inserted at the position specified by
    /// <paramref name="index"/>
    /// </summary>
    public void AddProcessor( int index, IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        Processors.Insert( index, processor );
    }

    /// <summary>
    /// Adds the specified <see cref="IInputProcessor"/> to the list of processors.
    /// </summary>
    public void AddProcessor( IInputProcessor processor )
    {
        if ( processor == null )
        {
            throw new NullReferenceException( "processor cannot be null" );
        }

        Processors.Add( processor );
    }

    /// <summary>
    /// Remove the <see cref="IInputProcessor"/> at the given index from
    /// the multiplexer.
    /// </summary>
    public void RemoveProcessor( int index )
    {
        Processors.RemoveAt( index );
    }

    /// <summary>
    /// Remove the first occurance of the specified <see cref="IInputProcessor"/>
    /// from the InputMultiplexer.
    /// </summary>
    public void RemoveProcessor( IInputProcessor processor )
    {
        Processors.Remove( processor );
    }

    /// <summary>
    /// Returns the number of <see cref="IInputProcessor"/>s in the list.
    /// </summary>
    /// <returns></returns>
    public int Size()
    {
        return Processors.Size;
    }

    /// <summary>
    /// Clears the list of Input Processors.
    /// </summary>
    public void Clear()
    {
        Processors.Clear();
    }

    /// <summary>
    /// Adds the given list of <see cref="IInputProcessor"/>s, which can be a
    /// single processor or multiple processors, to the multiplexer.
    /// </summary>
    public void AddProcessors( params IInputProcessor[] processorList )
    {
        Processors.Clear();
        Processors.AddAll( processorList );
    }

    /// <summary>
    /// Adds the given list of <see cref="IInputProcessor"/>s, which can be a
    /// single processor or multiple processors, to the multiplexer.
    /// </summary>
    public void AddProcessors( List< IInputProcessor > processorList )
    {
        Processors.Clear();
        Processors.AddAll( processorList );
    }
}
