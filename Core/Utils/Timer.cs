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

using Monitor = System.Threading.Monitor;

namespace LibGDXSharp.Utils;

/// <summary>
/// Executes tasks in the future on the main loop thread.
/// </summary>
[PublicAPI]
public class Timer
{
    private readonly static object ThreadLock = new();

    protected readonly List< Task? > tasks = new( 8 );

    private static TimerThread? _thread;

    public Timer()
    {
        Start();
    }

    /// <summary>
    /// Timer instance singleton for general application wide usage.
    /// Static methods on <see cref="Timer"/> make convenient use of this instance.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="GdxRuntimeException"></exception>
    public static Timer Instance()
    {
        lock ( ThreadLock )
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
        lock ( ThreadLock )
        {
            if ( ( _thread == null ) || ( _thread.files != Gdx.Files ) )
            {
                _thread?.Dispose();

                _thread = new TimerThread();
            }

            return _thread;
        }
    }

    /// <summary>
    /// Schedules a task to occur once as soon as possible, but not sooner
    /// than the start of the next frame.
    /// </summary>
    public Task PostTask( Task task )
    {
        return ScheduleTask( task, 0, 0, 0 );
    }

    /// <summary>
    /// Schedules a task to occur once after the specified delay.
    /// </summary>
    public Task ScheduleTask( Task task, float delaySeconds )
    {
        return ScheduleTask( task, delaySeconds, 0, 0 );
    }

    /// <summary>
    /// Schedules a task to occur once after the specified delay and then repeatedly
    /// at the specified interval until cancelled.
    /// </summary>
    public Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds )
    {
        return ScheduleTask( task, delaySeconds, intervalSeconds, -1 );
    }

    /// <summary>
    /// Schedules a task to occur once after the specified delay and then a
    /// number of additional times at the specified interval.
    /// </summary>
    /// <param name="task"></param>
    /// <param name="delaySeconds"></param>
    /// <param name="intervalSeconds"></param>
    /// <param name="repeatCount"> If negative, the task will repeat forever.</param>
    protected virtual Task ScheduleTask( Task task, float delaySeconds, float intervalSeconds, int repeatCount )
    {
        lock ( ThreadLock )
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

            Monitor.PulseAll( ThreadLock );
        }

        return task;
    }

    /// <summary>
    /// Starts the timer if it is not currently running.
    /// </summary>
    public void Start()
    {
        lock ( ThreadLock )
        {
            if ( Thread().instances.Contains( this ) )
            {
                return;
            }

            Thread().instances.Add( this );

            Monitor.PulseAll( ThreadLock );
        }
    }

    /// <summary>
    /// Stops the timer, tasks will not be executed and time that passes
    /// will not be applied to the task delays.
    /// </summary>
    public void Stop()
    {
        lock ( ThreadLock )
        {
            Thread().instances.Remove( this );
        }
    }

    /// <summary>
    /// Cancels all tasks.
    /// </summary>
    public void Clear()
    {
        for ( int i = 0, n = tasks.Count; i < n; i++ )
        {
            if ( tasks[ i ] != null )
            {
                lock ( tasks[ i ]! )
                {
                    tasks[ i ]!.executeTimeMillis = 0;
                    tasks[ i ]!.timer             = null;
                }
            }
        }

        tasks.Clear();
    }

    /// <summary>
    /// Returns true if the timer has no tasks in the queue.
    /// Note that this can change at any time. Synchronize on the timer instance
    /// to prevent tasks being added, removed, or updated.
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        lock ( ThreadLock )
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
        lock ( ThreadLock )
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
    /// Adds the specified delay to all tasks.
    /// </summary>
    /// <param name="delayMillis"></param>
    protected virtual void Delay( long delayMillis )
    {
        lock ( ThreadLock )
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

    /// <summary>
    /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
    /// </summary>
    public static Task Post( Task task )
    {
        return Instance().PostTask( task );
    }

    /// <summary>
    /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds )
    {
        return Instance().ScheduleTask( task, delaySeconds );
    }

    /// <summary>
    /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds, float intervalSeconds )
    {
        return Instance().ScheduleTask( task, delaySeconds, intervalSeconds );
    }

    /// <summary>
    /// Schedules a <see cref="Task"/> on <see cref="Instance"/>
    /// </summary>
    public static Task Schedule( Task task, float delaySeconds, float intervalSeconds, int repeatCount )
    {
        return Instance().ScheduleTask( task, delaySeconds, intervalSeconds, repeatCount );
    }

    // ================================================================================
    //  Companion Classes
    // ================================================================================

    /// <summary>
    /// A <see cref="Runnable"/> that can be scheduled on a <see cref="Timer"/>
    /// </summary>
    [PublicAPI]
    public abstract class Task
    {
        internal volatile Timer?        timer;
        internal readonly IApplication? app;
        internal          long          executeTimeMillis;
        internal          long          intervalMillis;
        internal          int           repeatCount;

        protected Task()
        {
            if ( Gdx.App == null )
            {
                throw new GdxRuntimeException( "Gdx.App not available!" );
            }

            app = Gdx.App;
        }

        /// <summary>
        /// If this is the last time the task will be ran or the task is first
        /// cancelled, it may be scheduled again in this method.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Cancels the task.
        /// It will not be executed until it is scheduled again.
        /// This method can be called at any time.
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
                        this.timer        = null;

                        timer?.tasks.Remove( this );
                    }
                }
            }
            else
            {
                lock ( this )
                {
                    executeTimeMillis = 0;
                    this.timer        = null;
                }
            }
        }

        /// <summary>
        /// Returns true if this task is scheduled to be executed in the future by a timer.
        /// The execution time may be reached at any time after calling this method,
        /// which may change the scheduled state.
        /// <p>
        /// To prevent the scheduled state from changing, synchronize on this task object, eg:
        /// <code>
        ///     lock( task )
        ///     {
        ///         if ( !task.IsScheduled() )
        ///         {
        ///             . . . . 
        ///         }
        ///     }
        /// </code>
        /// </p>
        /// </summary>
        /// <returns></returns>
        public bool IsScheduled()
        {
            return timer != null;
        }

        /// <summary>
        /// Returns the time in milliseconds when this task will be executed next.
        /// </summary>
        public long GetExecuteTimeMillis()
        {
            return executeTimeMillis;
        }
    }

    [PublicAPI]
    public class TimerThread : ILifecycleListener, IDisposable
    {
        public readonly List< Timer > instances = new( capacity: 1 );
        public readonly IFiles?       files;
        public          Timer?        instance;
        public          long          pauseTimeMillis;

        public TimerThread()
        {
            files = Gdx.Files;

            Gdx.App.AddLifecycleListener( this );

            Resume();

            var thread = new Thread( this.Run )
            {
                Name = "Timer"
            };

            thread.Start();
        }

        public void Run()
        {
            lock ( ThreadLock )
            {
                if ( ( _thread != this ) || ( files != Gdx.Files ) )
                {
                    goto exitlabel;
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
                    goto exitlabel;
                }

                try
                {
                    if ( waitMillis > 0 )
                    {
                        Monitor.Wait( ThreadLock, ( int )waitMillis );
                    }
                }
                catch ( ThreadInterruptedException )
                {
                    // Ignore
                }
            }

            exitlabel:

            Dispose();
        }

        public void Pause()
        {
            lock ( ThreadLock )
            {
                pauseTimeMillis = TimeUtils.NanosToMillis();

                Monitor.PulseAll( ThreadLock );
            }
        }

        public void Resume()
        {
            lock ( ThreadLock )
            {
                var delayMillis = TimeUtils.NanosToMillis() - pauseTimeMillis;

                for ( int i = 0, n = instances.Count; i < n; i++ )
                {
                    instances[ i ].Delay( delayMillis );
                }

                pauseTimeMillis = 0;

                Monitor.PulseAll( ThreadLock );
            }
        }

        public void Dispose()
        {
            lock ( ThreadLock )
            {
                if ( _thread == this )
                {
                    _thread = null;
                }

                instances.Clear();

                Monitor.PulseAll( ThreadLock );
            }

            Gdx.App.RemoveLifecycleListener( this );
        }
    }
}
