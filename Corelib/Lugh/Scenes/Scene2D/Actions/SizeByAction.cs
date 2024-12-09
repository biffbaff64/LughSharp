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


namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Scales an actor by the the values in <see cref="AmountWidth"/>
/// and <see cref="AmountHeight"/>.
/// </summary>
[PublicAPI]
public class SizeByAction : RelativeTemporalAction
{
    public float AmountWidth  { get; set; }
    public float AmountHeight { get; set; }

    // ========================================================================
    
    /// <inheritdoc />
    protected override void UpdateRelative( float percentDelta )
    {
        Target?.SizeBy( AmountWidth * percentDelta, AmountHeight * percentDelta );
    }

    /// <summary>
    /// Sets the amounts by which to scale the width and height.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetAmount( float width, float height )
    {
        AmountWidth  = width;
        AmountHeight = height;
    }
}
