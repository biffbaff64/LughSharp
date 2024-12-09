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

namespace Corelib.Lugh.Graphics;

/// <summary>
/// Represents a mouse cursor.
/// Create a cursor via <see cref="IGraphics.NewCursor"/>.
/// To set the cursor use <see cref="IGraphics.SetCursor(ICursor)"/>.
/// To use one of the system cursors, call <see cref="IGraphics.SetSystemCursor"/>.
/// </summary>
[PublicAPI]
public interface ICursor
{
    /// <summary>
    /// Available system mouse cursor types.
    /// </summary>
    [PublicAPI]
    public enum SystemCursor : int
    {
        Arrow,
        Ibeam,
        Crosshair,
        Hand,
        HorizontalResize,
        VerticalResize,
    }
}
