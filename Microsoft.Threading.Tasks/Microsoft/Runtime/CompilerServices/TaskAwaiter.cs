using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{

    /// <summary>Provides an awaiter for awaiting a <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public struct TaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
    {
        /// <summary>The default value to use for continueOnCapturedContext.</summary>
        internal const bool CONTINUE_ON_CAPTURED_CONTEXT_DEFAULT = true;
        /// <summary>Error message for GetAwaiter.</summary>
        private const string InvalidOperationException_TaskNotCompleted = "The task has not yet completed.";
        /// <summary>The task being awaited.</summary>
        private readonly Task m_task;

        /// <summary>Initializes the <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be awaited.</param>
        internal TaskAwaiter(Task task)
        {
            Contract.Assert(task != null);
            m_task = task;
        }

        /// <summary>Gets whether the task being awaited is completed.</summary>
        /// <remarks>This property is intended for compiler user rather than use directly in code.</remarks>
        /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
        public bool IsCompleted => m_task.IsCompleted;

        /// <summary>Schedules the continuation onto the <see cref="T:System.Threading.Tasks.Task" /> associated with this <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        public void OnCompleted(Action continuation)
        {
            OnCompletedInternal(m_task, continuation, true);
        }

        /// <summary>Schedules the continuation onto the <see cref="T:System.Threading.Tasks.Task" /> associated with this <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        public void UnsafeOnCompleted(Action continuation)
        {
            OnCompletedInternal(m_task, continuation, true);
        }

        /// <summary>Ends the await on the completed <see cref="T:System.Threading.Tasks.Task" />.</summary>
        /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <exception cref="T:System.InvalidOperationException">The task was not yet completed.</exception>
        /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
        /// <exception cref="T:System.Exception">The task completed in a Faulted state.</exception>
        public void GetResult() => ValidateEnd(m_task);

        /// <summary>
        /// Fast checks for the end of an await operation to determine whether more needs to be done
        /// prior to completing the await.
        /// </summary>
        /// <param name="task">The awaited task.</param>
        internal static void ValidateEnd(Task task)
        {
            if (task.Status == TaskStatus.RanToCompletion)
                return;
            HandleNonSuccess(task);
        }

        /// <summary>Handles validations on tasks that aren't successfully completed.</summary>
        /// <param name="task">The awaited task.</param>
        private static void HandleNonSuccess(Task task)
        {
            if (!task.IsCompleted)
            {
                try
                {
                    task.Wait();
                }
                catch { }
            }
            if (task.Status == TaskStatus.RanToCompletion)
                return;
            ThrowForNonSuccess(task);
        }

        /// <summary>Throws an exception to handle a task that completed in a state other than RanToCompletion.</summary>
        private static void ThrowForNonSuccess(Task task)
        {
            Contract.Assert(task.Status != TaskStatus.RanToCompletion);
            switch (task.Status)
            {
                case TaskStatus.Canceled:
                    throw new TaskCanceledException(task);
                case TaskStatus.Faulted:
                    throw PrepareExceptionForRethrow((task.Exception).InnerException);
                default:
                    throw new InvalidOperationException("The task has not yet completed.");
            }
        }

        /// <summary>Schedules the continuation onto the <see cref="T:System.Threading.Tasks.Task" /> associated with this <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
        /// <param name="task">The awaited task.</param>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <param name="continueOnCapturedContext">Whether to capture and marshal back to the current context.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        internal static void OnCompletedInternal(
          Task task,
          Action continuation,
          bool continueOnCapturedContext)
        {
            if (continuation == null)
                throw new ArgumentNullException(nameof(continuation));
            SynchronizationContext sc = continueOnCapturedContext ? SynchronizationContext.Current : null;
            if (sc != null && sc.GetType() != typeof(SynchronizationContext))
            {
                task.ContinueWith((param0 =>
                {
                    try
                    {
                        sc.Post(state => ((Action)state)(), continuation);
                    }
                    catch (Exception ex)
                    {
                        AsyncServices.ThrowAsync(ex, null);
                    }
                }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            else
            {
                TaskScheduler taskScheduler = continueOnCapturedContext ? TaskScheduler.Current : TaskScheduler.Default;
                if (task.IsCompleted)
                    Task.Factory.StartNew(s => ((Action)s)(), continuation, CancellationToken.None, TaskCreationOptions.None, taskScheduler);
                else if (taskScheduler != TaskScheduler.Default)
                    task.ContinueWith(_ => RunNoException(continuation), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, taskScheduler);
                else
                    task.ContinueWith((param0 =>
                    {
                        if (IsValidLocationForInlining)
                            RunNoException(continuation);
                        else
                            Task.Factory.StartNew(s => RunNoException((Action)s), continuation, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
                    }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
        }

        /// <summary>Invokes the delegate in a try/catch that will propagate the exception asynchronously on the ThreadPool.</summary>
        /// <param name="continuation"></param>
        private static void RunNoException(Action continuation)
        {
            try
            {
                continuation();
            }
            catch (Exception ex)
            {
                AsyncServices.ThrowAsync(ex, null);
            }
        }

        /// <summary>Whether the current thread is appropriate for inlining the await continuation.</summary>
        private static bool IsValidLocationForInlining
        {
            get
            {
                SynchronizationContext current = SynchronizationContext.Current;
                return (current == null || current.GetType() == typeof(SynchronizationContext)) && TaskScheduler.Current == TaskScheduler.Default;
            }
        }

        /// <summary>Copies the exception's stack trace so its stack trace isn't overwritten.</summary>
        /// <param name="exc">The exception to prepare.</param>
        internal static Exception PrepareExceptionForRethrow(Exception exc) => exc;
    }
}
