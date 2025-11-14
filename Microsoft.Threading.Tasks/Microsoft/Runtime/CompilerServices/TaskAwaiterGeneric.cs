using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{

    /// <summary>Provides an awaiter for awaiting a <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public readonly struct TaskAwaiter<TResult> : ICriticalNotifyCompletion, INotifyCompletion
    {
        /// <summary>The task being awaited.</summary>
        private readonly Task<TResult> m_task;

        internal TaskAwaiter(Task<TResult> task)
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
        /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        public void OnCompleted(Action continuation)
        {
            TaskAwaiter.OnCompletedInternal(m_task, continuation, true);
        }

        /// <summary>Schedules the continuation onto the <see cref="T:System.Threading.Tasks.Task" /> associated with this <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
        /// <param name="continuation">The action to invoke when the await operation completes.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
        /// <exception cref="T:System.InvalidOperationException">The awaiter was not properly initialized.</exception>
        /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
        public void UnsafeOnCompleted(Action continuation)
        {
            TaskAwaiter.OnCompletedInternal(m_task, continuation, true);
        }

        /// <summary>Ends the await on the completed <see cref="T:System.Threading.Tasks.Task" />.</summary>
        /// <returns>The result of the completed <see cref="T:System.Threading.Tasks.Task" />.</returns>
        /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
        /// <exception cref="T:System.InvalidOperationException">The task was not yet completed.</exception>
        /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
        /// <exception cref="T:System.Exception">The task completed in a Faulted state.</exception>
        public TResult GetResult()
        {
            TaskAwaiter.ValidateEnd(m_task);
            return m_task.Result;
        }
    }
}
