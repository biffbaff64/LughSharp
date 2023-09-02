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

using JetBrains.Annotations;

namespace LibGDXSharp.Utils;

public interface IScaling
{
    /// <summary>
    /// Returns the size of the source scaled to the target.
    /// Note the same Vector2 instance is always returned and should
    /// never be cached.
    /// </summary>
    Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight );
}

[PublicAPI]
public class Scaling
{
    protected readonly static Vector2 Temp = new();

    public readonly static IScaling Fit      = new FitScalingAnonymousInnerClass();
    public readonly static IScaling Fill     = new FillScalingAnonymousInnerClass();
    public readonly static IScaling FillX    = new FillXScalingAnonymousInnerClass();
    public readonly static IScaling FillY    = new FillYScalingAnonymousInnerClass();
    public readonly static IScaling Stretch  = new StretchScalingAnonymousInnerClass();
    public readonly static IScaling StretchX = new StretchXScalingAnonymousInnerClass();
    public readonly static IScaling StretchY = new StretchYScalingAnonymousInnerClass();
    public readonly static IScaling None     = new NoScalingAnonymousInnerClass();

    /// <summary>
    /// Scales the source to fit the target while keeping the same aspect ratio.
    /// This may cause the source to be smaller than the target in one direction. 
    /// </summary>
    private class FitScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            var targetRatio = targetHeight / targetWidth;
            var sourceRatio = sourceHeight / sourceWidth;
            var scale       = targetRatio > sourceRatio ? targetWidth / sourceWidth : targetHeight / sourceHeight;

            Temp.X = sourceWidth * scale;
            Temp.Y = sourceHeight * scale;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target while keeping the same aspect ratio.
    /// This may cause the source to be larger than the target in one direction. 
    /// </summary>
    private class FillScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            var targetRatio = targetHeight / targetWidth;
            var sourceRatio = sourceHeight / sourceWidth;
            var scale       = targetRatio < sourceRatio ? targetWidth / sourceWidth : targetHeight / sourceHeight;

            Temp.X = sourceWidth * scale;
            Temp.Y = sourceHeight * scale;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target in the x direction while keeping
    /// the same aspect ratio. This may cause the source to be smaller or larger
    /// than the target in the y direction. 
    /// </summary>
    private class FillXScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            var scale = targetWidth / sourceWidth;

            Temp.X = sourceWidth * scale;
            Temp.Y = sourceHeight * scale;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target in the y direction while keeping the
    /// same aspect ratio. This may cause the source to be smaller or larger than
    /// the target in the x direction. 
    /// </summary>
    private class FillYScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            var scale = targetHeight / sourceHeight;

            Temp.X = sourceWidth * scale;
            Temp.Y = sourceHeight * scale;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target.
    /// This may cause the source to not keep the same aspect ratio.
    /// </summary>
    private class StretchScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            Temp.X = targetWidth;
            Temp.Y = targetHeight;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target in the x direction, without changing the
    /// y direction. This may cause the source to not keep the same aspect ratio. 
    /// </summary>
    private class StretchXScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            Temp.X = targetWidth;
            Temp.Y = sourceHeight;

            return Temp;
        }
    }

    /// <summary>
    /// Scales the source to fill the target in the y direction, without changing the
    /// x direction. This may cause the source to not keep the same aspect ratio. 
    /// </summary>
    private class StretchYScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            Temp.X = sourceWidth;
            Temp.Y = targetHeight;

            return Temp;
        }
    }

    /// <summary>
    /// The source is not scaled.
    /// </summary>
    private class NoScalingAnonymousInnerClass : IScaling
    {
        public Vector2 Apply( float sourceWidth, float sourceHeight, float targetWidth, float targetHeight )
        {
            Temp.X = sourceWidth;
            Temp.Y = sourceHeight;

            return Temp;
        }
    }
}