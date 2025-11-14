using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Microsoft.Runtime.CompilerServices
{

    /// <summary>Provides an awaitable object that allows for configured awaits on <see cref="T:System.Threading.Tasks.Task" />.</summary>
    /// <remarks>This type is intended for compiler use only.</remarks>
    public readonly struct ConfiguredTaskAwaitable
    {
        /// <summary>The task being awaited.</summary>
        private readonly ConfiguredTaskAwaiter m_configuredTaskAwaiter;

        /// <summary>Initializes the <see cref="T:Microsoft.Runtime.CompilerServices.ConfiguredTaskAwaitable" />.</summary>
        /// <param name="task">The awaitable <see cref="T:System.Threading.Tasks.Task" />.</param>
        /// <param name="continueOnCapturedContext">
        /// true to attempt to marshal the continuation back to the original context captured; otherwise, false.
        /// </param>
        internal ConfiguredTaskAwaitable(Task task, bool continueOnCapturedContext)
        {
            Contract.Assert(task != null);
            m_configuredTaskAwaiter = new ConfiguredTaskAwaiter(task, continueOnCapturedContext);
        }

        /// <summary>Gets an awaiter for this awaitable.</summary>
        /// <returns>The awaiter.</returns>
        public ConfiguredTaskAwaiter GetAwaiter() => m_configuredTaskAwaiter;

        /// <summary>Provides an awaiter for a <see cref="T:Microsoft.Runtime.CompilerServices.ConfiguredTaskAwaitable" />.</summary>
        /// <remarks>This type is intended for compiler use only.</remarks>
        public readonly struct ConfiguredTaskAwaiter : ICriticalNotifyCompletion, INotifyCompletion
        {
            /// <summary>The task being awaited.</summary>
            private readonly Task m_task;
            /// <summary>Whether to attempt marshaling back to the original context.</summary>
            private readonly bool m_continueOnCapturedContext;

            /// <summary>Initializes the <see cref="T:Microsoft.Runtime.CompilerServices.ConfiguredTaskAwaiter" />.</summary>
            /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to await.</param>
            /// <param name="continueOnCapturedContext">
            /// true to attempt to marshal the continuation back to the original context captured
            /// when BeginAwait is called; otherwise, false.
            /// </param>
            internal ConfiguredTaskAwaiter(Task task, bool continueOnCapturedContext)
            {
                Contract.Assert(task != null);
                m_task = task;
                m_continueOnCapturedContext = continueOnCapturedContext;
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
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext);
            }

            /// <summary>Schedules the continuation onto the <see cref="T:System.Threading.Tasks.Task" /> associated with this <see cref="T:Microsoft.Runtime.CompilerServices.TaskAwaiter" />.</summary>
            /// <param name="continuation">The action to invoke when the await operation completes.</param>
            /// <exception cref="T:System.ArgumentNullException">The <paramref name="continuation" /> argument is null (Nothing in Visual Basic).</exception>
            /// <exception cref="T:System.InvalidOperationException">The awaiter was not properly initialized.</exception>
            /// <remarks>This method is intended for compiler user rather than use directly in code.</remarks>
            public void UnsafeOnCompleted(Action continuation)
            {
                TaskAwaiter.OnCompletedInternal(m_task, continuation, m_continueOnCapturedContext);
            }

            /// <summary>Ends the await on the completed <see cref="T:System.Threading.Tasks.Task" />.</summary>
            /// <returns>The result of the completed <see cref="T:System.Threading.Tasks.Task" />.</returns>
            /// <exception cref="T:System.NullReferenceException">The awaiter was not properly initialized.</exception>
            /// <exception cref="T:System.InvalidOperationException">The task was not yet completed.</exception>
            /// <exception cref="T:System.Threading.Tasks.TaskCanceledException">The task was canceled.</exception>
            /// <exception cref="T:System.Exception">The task completed in a Faulted state.</exception>
            public void GetResult() => TaskAwaiter.ValidateEnd(m_task);
        }
    }
}
