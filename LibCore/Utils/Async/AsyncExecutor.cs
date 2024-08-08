// /////////////////////////////////////////////////////////////////////////////
//  MIT License
// 
//  Copyright (c) 2024 Richard Ikin / Red 7 Projects and Contributors.
// 
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// /////////////////////////////////////////////////////////////////////////////

using System.Collections.Concurrent;

namespace LughSharp.LibCore.Utils.Async;

/// <summary>
/// Allows asnynchronous execution of AsyncTask instances on a separate thread.
/// Needs to be disposed via a call to Dispose() when no longer used, in which
/// case the executor waits for running tasks to finish. Scheduled but not yet
/// running tasks will not be executed.
/// </summary>
[PublicAPI]
public class AsyncExecutor : IDisposable
{
    private readonly CancellationTokenSource _cts;
    private readonly SemaphoreSlim           _semaphore;
    private readonly ConcurrentQueue< Task > _taskQueue;

    // ------------------------------------------------------------------------
    
    /// <summary>
    /// Creates a new AsynchExecutor that allows maxConcurrent Runnable instances to run in parallel.
    /// </summary>
    /// <param name="maxConcurrent"> The maximum allowed instances. </param>
    /// <param name="name"> The name of the threads. Default is "AsyncExecutor-Thread". </param>
    public AsyncExecutor( int maxConcurrent, string name = "AsyncExecutor-Thread" )
    {
        Logger.CheckPoint();

        _taskQueue = new ConcurrentQueue< Task >();
        _cts       = new CancellationTokenSource();
        _semaphore = new SemaphoreSlim( maxConcurrent );

        for ( var i = 0; i < maxConcurrent; i++ )
        {
            var thread = new Thread( () =>
            {
                while ( !_cts.Token.IsCancellationRequested )
                {
                    if ( _taskQueue.TryDequeue( out var task ) )
                    {
                        task.Start();
                        task.Wait();
                    }
                }
            } )
            {
                IsBackground = true,
                Name         = name
            };
            thread.Start();
        }
    }

    /// <summary>
    /// Submits a Task to be executed asynchronously.
    /// If maxConcurrent tasks are already running, the task will be queued.
    /// </summary>
    /// <param name="task"> The task to execute. </param>
    public AsyncResult< T > Submit< T >( IAsyncTask< T > task )
    {
        if ( _cts.IsCancellationRequested )
        {
            throw new InvalidOperationException( "Cannot run tasks on an executor that has been shutdown (disposed)" );
        }

        var tcs = new TaskCompletionSource< T >();

        var taskWrapper = new Task< T >( () =>
        {
            try
            {
                return task.Call() ?? throw new NullReferenceException();
            }
            catch ( Exception ex )
            {
                tcs.SetException( ex );

                throw;
            }
        }, _cts.Token );

        _taskQueue.Enqueue( taskWrapper );

        return new AsyncResult< T >( taskWrapper );
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _cts.Cancel();

        while ( _taskQueue.TryDequeue( out var _ ) )
        {
        }
    }
}
