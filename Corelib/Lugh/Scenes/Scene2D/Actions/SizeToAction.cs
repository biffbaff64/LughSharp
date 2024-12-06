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

using Corelib.Lugh.Utils.Exceptions;

namespace Corelib.Lugh.Scenes.Scene2D.Actions;

/// <summary>
/// Moves an actor from its current size to a specific size.
/// </summary>
[PublicAPI]
public class SizeToAction : TemporalAction
{
    public float StartWidth  { get; set; }
    public float StartHeight { get; set; }
    public float EndWidth    { get; set; }
    public float EndHeight   { get; set; }

    /// <inheritdoc />
    protected override void Begin()
    {
        if ( Target == null )
        {
            throw new GdxRuntimeException( "Cannot Begin with null Target Actor!" );
        }

        StartWidth  = Target.Width;
        StartHeight = Target.Height;
    }

    /// <inheritdoc />
    protected override void Update( float percent )
    {
        float width, height;

        if ( percent == 0 )
        {
            width  = StartWidth;
            height = StartHeight;
        }
        else if ( percent is 1.0f )
        {
            width  = EndWidth;
            height = EndHeight;
        }
        else
        {
            width  = StartWidth + ( ( EndWidth - StartWidth ) * percent );
            height = StartHeight + ( ( EndHeight - StartHeight ) * percent );
        }

        Target?.SetSize( width, height );
    }

    /// <summary>
    /// Sets the target width and height of this action. 
    /// </summary>
    /// <param name="width"> Target width. </param>
    /// <param name="height"> Target height. </param>
    public void SetSize( float width, float height )
    {
        EndWidth  = width;
        EndHeight = height;
    }
}
