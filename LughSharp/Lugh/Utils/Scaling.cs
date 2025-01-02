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


namespace LughSharp.Lugh.Utils;

[PublicAPI]
public abstract class Scaling
{
    protected static readonly Vector2 Temp = new();

    public static readonly Scaling Fit      = new FitScaling();
    public static readonly Scaling Fill     = new FillScaling();
    public static readonly Scaling FillX    = new FillXScaling();
    public static readonly Scaling FillY    = new FillYScaling();
    public static readonly Scaling Stretch  = new StretchScaling();
    public static readonly Scaling StretchX = new StretchXScaling();
    public static readonly Scaling StretchY = new StretchYScaling();
    public static readonly Scaling None     = new NoScaling();

    // ============================================================================

    /// <summary>
    /// Returns the size of the source scaled to the target.
    /// Note the same Vector2 instance is always returned and should
    /// never be cached.
    /// </summary>
    public abstract Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight );
}

// ============================================================================

/// <summary>
/// Scales the source to fit the target while keeping the same aspect ratio.
/// This may cause the source to be smaller than the target in one direction.
/// </summary>
[PublicAPI]
internal class FitScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        var targetRatio = targetHeight / targetWidth;
        var sourceRatio = sourceHeight / sourceWidth;
        var scale       = targetRatio > sourceRatio ? targetWidth / sourceWidth : targetHeight / sourceHeight;

        Temp.X = sourceWidth * scale;
        Temp.Y = sourceHeight * scale;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target while keeping the same aspect ratio.
/// This may cause the source to be larger than the target in one direction.
/// </summary>
[PublicAPI]
internal class FillScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        var targetRatio = targetHeight / targetWidth;
        var sourceRatio = sourceHeight / sourceWidth;
        var scale       = targetRatio < sourceRatio ? targetWidth / sourceWidth : targetHeight / sourceHeight;

        Temp.X = sourceWidth * scale;
        Temp.Y = sourceHeight * scale;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target in the x direction while keeping the same aspect
/// ratio. This may cause the source to be smaller or larger than the target in the y direction.
/// </summary>
[PublicAPI]
internal class FillXScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        var scale = targetWidth / sourceWidth;

        Temp.X = sourceWidth * scale;
        Temp.Y = sourceHeight * scale;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target in the y direction while keeping the
/// same aspect ratio. This may cause the source to be smaller or larger than
/// the target in the x direction.
/// </summary>
[PublicAPI]
internal class FillYScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        var scale = targetHeight / sourceHeight;

        Temp.X = sourceWidth * scale;
        Temp.Y = sourceHeight * scale;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target.
/// This may cause the source to not keep the same aspect ratio.
/// </summary>
[PublicAPI]
internal class StretchScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        Temp.X = targetWidth;
        Temp.Y = targetHeight;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target in the x direction, without changing the
/// y direction. This may cause the source to not keep the same aspect ratio.
/// </summary>
[PublicAPI]
internal class StretchXScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        Temp.X = targetWidth;
        Temp.Y = sourceHeight;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// Scales the source to fill the target in the y direction, without changing the
/// x direction. This may cause the source to not keep the same aspect ratio.
/// </summary>
[PublicAPI]
internal class StretchYScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        Temp.X = sourceWidth;
        Temp.Y = targetHeight;

        return Temp;
    }
}

// ============================================================================

/// <summary>
/// The source is not scaled.
/// </summary>
[PublicAPI]
internal class NoScaling : Scaling
{
    public override Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
    {
        Temp.X = sourceWidth;
        Temp.Y = sourceHeight;

        return Temp;
    }
}
