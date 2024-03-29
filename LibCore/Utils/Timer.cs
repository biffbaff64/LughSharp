﻿// ///////////////////////////////////////////////////////////////////////////////
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


using Exception = System.Exception;
using Monitor = System.Threading.Monitor;

namespace LughSharp.LibCore.Utils;

/// <summary>
///     Executes tasks in the future on the main loop thread.
/// </summary>
public class Timer
{
    private readonly static object _threadLock = new();

    private static TimerThread? _thread;

    protected readonly List< Task? > tasks = new( 8 );

    public Timer()
    {
        Start();
    }

    /// <summary>
    ///     Timer instance singleton for general application wide usage.
    ///     Static methods on <see cref="Timer" /> make convenient use of this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Timer Instance()
    {
        lock ( _threadLock )
        {
            TimerThread? thread = Thread();

            if ( thread == null )
            {
                throw new GdxRuntimeException( "Thread instance is null!" );
            }

            return thread.instance ?? ( thread.instance = new Timer() );
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private static TimerThread Thread()
    {
        lock ( _threadLock )
        {
            if ( ( _thread != null ) && ( _thread.files == Gdx.Files ) )
            {
                return _thread;
            }

            _thread?.Dispose();
            _thread = new TimerThread();

            return _thread;
        }
    }

    /// <summary>
    ///     Schedules a task to occur once as soon as possible, but not sooner
    ///     than the start of the next frame.
    /// </summary>
    public Task PostTask( Task task )
    {
        return ScheduleTask( task, 0, 0, 0 );
    }

    /// <summary>
    ///     Schedules a task to occur once after the specified delay.
    /// </summary>
    public Task ScheduleTask( Task task, float delaySeconds )
    {
        return ScheduleTask( task, delaySeconds, 0, 0 );
    }

    /// <summary>
    ///     Schedules a task to occur once after the specified delay and then a
    ///     number of additional times at the specified interval.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="delaySeconds"></param>
    /// <param name="intervalSeconds"></param>
    /// <param name="repeatCount"> If negative, the task will repeat forever.</param>
    protected virtual Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds, int repeatCount = -1 )
    {
        lock ( _threadLock )
        {
            lock ( this )
            {
                lock ( task )
                {
                    if ( task.timer != null )
                    {
                        throw new ArgumentException( "The same task may not be scheduled twice." );
                    }

                    task.timer = this;

                    var timeMillis        = TimeUtils.NanoTime() / 1000000;
                    var executeTimeMillis = timeMillis + ( long )( delaySeconds * 1000 );

                    if ( _thread?.pauseTimeMillis > 0 )
                    {
                        executeTimeMillis -= timeMillis - _thread.pauseTimeMillis;
                    }

                    task.executeTimeMillis = executeTimeMillis;
                    task.intervalMillis    = ( long )( intervalSeconds * 1000 );
                    task.repeatCount       = repeatCount;

                    tasks.Add( task );
                }
            }

            Monitor.PulseAll( _threadLock );
        }

        return task;
    }

    /// <summary>
    ///     Starts the timer if it is not currently running.
    /// </summary>
    public void Start()
    {
        lock ( _threadLock )
        {
            if ( Thread().instances.Contains( this ) )
            {
                return;
            }

            Thread().instances.Add( this );

            Monitor.PulseAll( _threadLock );
        }
    }

    /// <summary>
    ///     Stops the timer, tasks will not be executed and time that passes
    ///     will not be applied to the task delays.
    /// </summary>
    public void Stop()
    {
        lock ( _threadLock )
        {
            Thread().instances.Remove( this );
        }
    }

    /// <summary>
    ///     Cancels all tasks.
    /// </summary>
    public void Clear()
    {
        for ( int i = 0, n = tasks.Count; i < n; i++ )
        {
            if ( tasks[ i ] == null )
            {
                continue;
            }

            lock ( tasks[ i ]! )
            {
                tasks[ i ]!.executeTimeMillis = 0;
                tasks[ i ]!.timer             = null;
            }
        }

        tasks.Clear();
    }

    /// <summary>
    ///     Returns true if the timer has no tasks in the queue.
    ///     Note that this can change at any time. Synchronize on the timer instance
    ///     to prevent tasks being added, removed, or updated.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        lock ( _threadLock )
        {
            return tasks.Count == 0;
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="timeMillis"></param>
    /// <param name="waitMillis"></param>
    /// <returns></returns>
    public long Update( long timeMillis, long waitMillis )
    {
        lock ( _threadLock )
        {
            for ( int i = 0, n = tasks.Count; i < n; i++ )
            {
                lock ( tasks[ i ]! )
                {
                    if ( tasks[ i ]?.executeTimeMillis > timeMillis )
                    {
                        waitMillis = Math.Min( waitMillis, tasks[ i ]!.executeTimeMillis - timeMillis );

                        continue;
                    }

                    if ( tasks[ i ]!.repeatCount == 0 )
                    {
                        tasks[ i ]!.timer = null;

                        tasks.RemoveAt( i );

                        i--;
                        n--;
                    }
                    else
                    {
                        tasks[ i ]!.executeTimeMillis = timeMillis + tasks[ i ]!.intervalMillis;

                        waitMillis = Math.Min( waitMillis, tasks[ i ]!.intervalMillis );

                        if ( tasks[ i ]!.repeatCount > 0 )
                        {
                            tasks[ i ]!.repeatCount--;
                        }
                    }

                    tasks[ i ]!.app!.PostRunnable( tasks[ i ]!.Run );
                }
            }

            return waitMillis;
        }
    }

    /// <summary>
    ///     Adds the specified delay to all tasks.
    /// </summary>
    /// <param name="delayMillis"></param>
    protected virtual void Delay( long delayMillis )
    {
        lock ( _threadLock )
        {
            for ( int i = 0, n = tasks.Count; i < n; i++ )
            {
                Task task = tasks[ i ]!;

                lock ( task )
                {
                    task.executeTimeMillis += delayMillis;
                }
            }
        }
    }

    // ------------------------------------------------------------------------

    /// <summary>
    ///     Schedules a <see cref="Task" /> on <see cref="Instance" />
    /// </summary>
    public static Task Post( Task task )
    {
        return Instance().PostTask( task );
    }

    /// <summary>
    ///     Schedules a <see cref="Task" /> on <see cref="Instance" />
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds )
    {
        return Instance().ScheduleTask( task, delaySeconds );
    }

    /// <summary>
    ///     Schedules a <see cref="Task" /> on <see cref="Instance" />
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds, float intervalSeconds )
    {
        return Instance().ScheduleTask( task, delaySeconds, intervalSeconds );
    }

    /// <summary>
    ///     Schedules a <see cref="Task" /> on <see cref="Instance" />
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds, float intervalSeconds, int repeatCount )
    {
        return Instance().ScheduleTask( task, delaySeconds, intervalSeconds, repeatCount );
    }

    // ================================================================================
    //  Companion Classes
    // ================================================================================

    /// <summary>
    ///     A <see cref="Runnable" /> that can be scheduled on a <see cref="Timer" />
    /// </summary>
    [PublicAPI]
    public abstract class Task
    {
        internal readonly IApplication? app;
        internal          long          executeTimeMillis;
        internal          long          intervalMillis;
        internal          int           repeatCount;
        internal volatile Timer?        timer;

        protected Task()
        {
            if ( Gdx.App == null )
            {
                throw new GdxRuntimeException( "Gdx.App not available!" );
            }

            app = Gdx.App;
        }

        /// <summary>
        ///     If this is the last time the task will be ran or the task is first
        ///     cancelled, it may be scheduled again in this method.
        /// </summary>
        public abstract void Run();

        /// <summary>
        ///     Cancels the task.
        ///     It will not be executed until it is scheduled again.
        ///     This method can be called at any time.
        /// </summary>
        public void Cancel()
        {
            if ( timer != null )
            {
                lock ( timer )
                {
                    lock ( this )
                    {
                        executeTimeMillis = 0;
                        timer             = null;

                        timer?.tasks.Remove( this );
                    }
                }
            }
            else
            {
                lock ( this )
                {
                    executeTimeMillis = 0;
                    timer             = null;
                }
            }
        }

        /// <summary>
        ///     Returns true if this task is scheduled to be executed in the future by a timer.
        ///     The execution time may be reached at any time after calling this method,
        ///     which may change the scheduled state.
        ///     <p>
        ///         To prevent the scheduled state from changing, synchronize on this task object, eg:
        ///         <code>
        ///     lock( task )
        ///     {
        ///         if ( !task.IsScheduled() )
        ///         {
        ///             . . . . 
        ///         }
        ///     }
        /// </code>
        ///     </p>
        /// </summary>
        /// <returns></returns>
        public bool IsScheduled()
        {
            return timer != null;
        }

        /// <summary>
        ///     Returns the time in milliseconds when this task will be executed next.
        /// </summary>
        public long GetExecuteTimeMillis()
        {
            return executeTimeMillis;
        }
    }

    // ------------------------------------------------------------------------
    // ------------------------------------------------------------------------

    [PublicAPI]
    public class TimerThread : ILifecycleListener
    {
        public readonly IFiles?       files;
        public readonly List< Timer > instances = new( capacity: 1 );
        public          Timer?        instance;
        public          long          pauseTimeMillis;

        public TimerThread()
        {
            files = Gdx.Files;

            Gdx.App.AddLifecycleListener( this );

            Resume();

            var thread = new Thread( Run )
            {
                Name = "Timer"
            };

            thread.Start();
        }

        public void Pause()
        {
            lock ( _threadLock )
            {
                pauseTimeMillis = TimeUtils.NanosToMillis();

                Monitor.PulseAll( _threadLock );
            }
        }

        public void Resume()
        {
            lock ( _threadLock )
            {
                var delayMillis = TimeUtils.NanosToMillis() - pauseTimeMillis;

                for ( int i = 0, n = instances.Count; i < n; i++ )
                {
                    instances[ i ].Delay( delayMillis );
                }

                pauseTimeMillis = 0;

                Monitor.PulseAll( _threadLock );
            }
        }

        public void Dispose()
        {
            Dispose( true );
        }

        public void Run()
        {
            lock ( _threadLock )
            {
                if ( ( _thread != this ) || ( files != Gdx.Files ) )
                {
                    Dispose();

                    return;
                }

                long waitMillis = 5000;

                if ( pauseTimeMillis == 0 )
                {
                    var timeMillis = TimeUtils.NanoTime() / 1000000;

                    for ( int i = 0, n = instances.Count; i < n; i++ )
                    {
                        try
                        {
                            waitMillis = instances[ i ].Update( timeMillis, waitMillis );
                        }
                        catch ( Exception ex )
                        {
                            throw new GdxRuntimeException( "Task failed: " + instances[ i ].GetType().Name, ex );
                        }
                    }
                }

                if ( ( _thread != this ) || ( files != Gdx.Files ) )
                {
                    Dispose();

                    return;
                }

                try
                {
                    if ( waitMillis > 0 )
                    {
                        Monitor.Wait( _threadLock, ( int )waitMillis );
                    }
                }
                catch ( ThreadInterruptedException )
                {
                    // Ignore
                }
            }

            Dispose();
        }

        private void Dispose( bool disposing )
        {
            if ( disposing )
            {
                lock ( _threadLock )
                {
                    if ( _thread == this )
                    {
                        _thread = null;
                    }

                    instances.Clear();

                    Monitor.PulseAll( _threadLock );
                }

                Gdx.App.RemoveLifecycleListener( this );
            }
        }
    }
}
