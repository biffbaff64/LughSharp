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


namespace Corelib.LibCore.Core;

/// <summary>
/// An adapter class for <see cref="IInputProcessor"/>.
/// You can derive from this and only override what you are interested in.
/// </summary>
[PublicAPI]
public class InputAdapter : IInputProcessor
{
    /// <inheritdoc />
    public virtual bool KeyDown( int keycode )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool KeyUp( int keycode )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool KeyTyped( char character )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool TouchDown( int screenX, int screenY, int pointer, int button )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool TouchUp( int screenX, int screenY, int pointer, int button )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool TouchDragged( int screenX, int screenY, int pointer )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool MouseMoved( int screenX, int screenY )
    {
        return false;
    }

    /// <inheritdoc />
    public virtual bool Scrolled( float amountX, float amountY )
    {
        return false;
    }
}
