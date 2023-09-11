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

using LibGDXSharp.Maths;

namespace LibGDXSharp.G2D;

[PublicAPI]
public class Animation<T>
{
    /// <summary>
    /// Defines possible playback modes for an <see cref="Animation{T}"/>.
    /// </summary>
    public enum AnimMode
    {
        Normal,
        Reversed,
        Loop,
        Loop_Reversed,
        Loop_Pingpong,
        Loop_Random,
    }

    /// <summary>
    /// Length must not be modified without updating <see cref="_animationDuration"/>.
    /// See <see cref="SetKeyFrames(T[])"/>.
    /// </summary>
    private T[] _keyFrames = null!;

    private float _frameDuration;
    private float _animationDuration;
    private int   _lastFrameNumber;
    private float _lastStateTime;

    /// <summary>
    /// Constructor, storing the frame duration and key frames.
    /// </summary>
    /// <param name="frameDuration">the time between frames in seconds.</param>
    /// <param name="keyFrames">
    /// The objects representing the frames.
    /// If this Array is type-aware, <see cref="GetKeyFrames()"/> can return the
    /// correct type of array. Otherwise, it returns an object[].
    /// </param>
    public Animation( float frameDuration, List< T > keyFrames )
    {
        this._frameDuration = frameDuration;

        SetKeyFrames( keyFrames.ToArray() );
    }

    /// <summary>
    /// Constructor, storing the frame duration and key frames.
    /// </summary>
    /// <param name="frameDuration"> the time between frames in seconds.</param>
    /// <param name="keyFrames">
    /// The objects representing the frames. If this Array is type-aware,
    /// <see cref="GetKeyFrames()"/> can return the correct type of array.
    /// Otherwise, it returns an object[].
    /// </param>
    /// <param name="playMode"></param>
    public Animation( float frameDuration, List< T > keyFrames, AnimMode playMode )
        : this( frameDuration, keyFrames )
    {
        PlayMode = playMode;
    }

    /// <summary>
    /// Constructor, storing the frame duration and key frames.
    /// </summary>
    /// <param name="frameDuration"> the time between frames in seconds.</param>
    /// <param name="keyFrames"> the objects representing the frames.</param>
    public Animation( float frameDuration, T[] keyFrames )
    {
        this._frameDuration = frameDuration;
        SetKeyFrames( keyFrames );
    }

    /// <summary>
    /// Returns a frame based on the so called state time. This is the amount of
    /// seconds an object has spent in the state this Animation instance represents,
    /// e.g. running, jumping and so on. The mode specifies whether the animation is
    /// looping or not.
    /// </summary>
    /// <param name="stateTime">the time spent in the state represented by this animation.</param>
    /// <param name="looping"> whether the animation is looping or not.</param>
    /// <returns> the frame of animation for the given state time.</returns>
    public T GetKeyFrame( float stateTime, bool looping )
    {
        // we set the play mode by overriding the previous mode based on looping
        // parameter value
        AnimMode oldPlayMode = PlayMode;

        if ( looping && ( ( PlayMode == AnimMode.Normal ) || ( PlayMode == AnimMode.Reversed ) ) )
        {
            PlayMode = PlayMode == AnimMode.Normal ? AnimMode.Loop : AnimMode.Loop_Reversed;
        }
        else if ( !looping && !( ( PlayMode == AnimMode.Normal ) || ( PlayMode == AnimMode.Reversed ) ) )
        {
            PlayMode = PlayMode == AnimMode.Loop_Reversed ? AnimMode.Reversed : AnimMode.Loop;
        }

        T frame = GetKeyFrame( stateTime );
        PlayMode = oldPlayMode;

        return frame;
    }

    /// <summary>
    /// Returns a frame based on the so called state time. This is the amount
    /// of seconds an object has spent in the state this Animation instance
    /// represents, e.g. running, jumping and so on using the mode specified by
    /// <see cref="PlayMode"/> property.
    /// </summary>
    /// <param name="stateTime"> </param>
    /// <returns> the frame of animation for the given state time.</returns>
    public T GetKeyFrame( float stateTime )
    {
        var frameNumber = GetKeyFrameIndex( stateTime );

        return _keyFrames[ frameNumber ];
    }

    /** Returns the current frame number.
	 * @param stateTime
	 * @return current frame number */
    public int GetKeyFrameIndex( float stateTime )
    {
        if ( _keyFrames.Length == 1 ) return 0;

        var frameNumber = ( int )( stateTime / _frameDuration );

        switch ( PlayMode )
        {
            case AnimMode.Normal:
                frameNumber = Math.Min( _keyFrames.Length - 1, frameNumber );

                break;

            case AnimMode.Loop:
                frameNumber = frameNumber % _keyFrames.Length;

                break;

            case AnimMode.Loop_Pingpong:
                frameNumber = frameNumber % ( ( _keyFrames.Length * 2 ) - 2 );

                if ( frameNumber >= _keyFrames.Length )
                    frameNumber = _keyFrames.Length - 2 - ( frameNumber - _keyFrames.Length );

                break;

            case AnimMode.Loop_Random:
                var lastFrameNumber = ( int )( ( _lastStateTime ) / _frameDuration );

                frameNumber = lastFrameNumber != frameNumber
                    ? MathUtils.Random( _keyFrames.Length - 1 )
                    : this._lastFrameNumber;

                break;

            case AnimMode.Reversed:
                frameNumber = Math.Max( _keyFrames.Length - frameNumber - 1, 0 );

                break;

            case AnimMode.Loop_Reversed:
                frameNumber %= _keyFrames.Length;
                frameNumber =  _keyFrames.Length - frameNumber - 1;

                break;
        }

        _lastFrameNumber = frameNumber;
        _lastStateTime   = stateTime;

        return frameNumber;
    }

    /// <summary>
    /// Returns the keyframes[] array where all the frames of the
    /// animation are stored.
    /// </summary>
    /// <returns>
    /// The keyframes[] field. This array is an object[] if the animation
    /// was instantiated with an Array that was not type-aware.
    /// </returns>
    public T[] GetKeyFrames()
    {
        return _keyFrames;
    }

    protected void SetKeyFrames( T[] keyFrames )
    {
        this._keyFrames         = keyFrames;
        this._animationDuration = _keyFrames.Length * _frameDuration;
    }

    /// <summary>
    /// The animation play mode.
    /// </summary>
    public AnimMode PlayMode { get; set; } = AnimMode.Normal;

    /// <summary>
    /// Whether the animation would be finished if played without looping
    /// (PlayMode#NORMAL), given the state time.
    /// </summary>
    /// <param name="stateTime"> </param>
    /// <returns> whether the animation is finished.</returns>
    public bool IsAnimationFinished( float stateTime )
    {
        var frameNumber = ( int )( stateTime / _frameDuration );

        return ( _keyFrames.Length - 1 ) < frameNumber;
    }

    /// <summary>
    /// Sets duration a frame will be displayed.
    /// </summary>
    /// <param name="frameDuration">The animation frame duration in seconds</param>
    public void SetFrameDuration( float frameDuration )
    {
        this._frameDuration     = frameDuration;
        this._animationDuration = _keyFrames.Length * frameDuration;
    }

    /// <summary>
    /// </summary>
    /// <returns>the duration of a frame in seconds.</returns>
    public float FrameDuration() => _frameDuration;

    /// <summary>
    /// </summary>
    /// <returns>
    /// the duration of the entire animation, (number of frames x frame duration),
    /// in seconds.
    /// </returns>
    public float GetAnimationDuration()
    {
        return _animationDuration;
    }
}