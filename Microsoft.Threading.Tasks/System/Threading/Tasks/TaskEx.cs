using Microsoft.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace System.Threading.Tasks
{
	/// <summary>Provides methods for creating and manipulating tasks.</summary>
	public static class TaskEx
	{
		private const string ArgumentOutOfRange_TimeoutNonNegativeOrMinusOne = "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.";
		/// <summary>An already completed task.</summary>
		private static readonly Task s_preCompletedTask = FromResult(false);

		/// <summary>Creates a task that runs the specified action.</summary>
		/// <param name="action">The action to execute asynchronously.</param>
		/// <returns>A task that represents the completion of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
		public static Task Run(Action action) => Run(action, CancellationToken.None);

		/// <summary>Creates a task that runs the specified action.</summary>
		/// <param name="action">The action to execute.</param>
		/// <param name="cancellationToken">The CancellationToken to use to request cancellation of this task.</param>
		/// <returns>A task that represents the completion of the action.</returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="action" /> argument is null.</exception>
		public static Task Run(Action action, CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(action, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
		}

		public static Task<TResult> Run<TResult>(Func<TResult> function)
		{
			return Run(function, CancellationToken.None);
		}

		public static Task<TResult> Run<TResult>(
		  Func<TResult> function,
		  CancellationToken cancellationToken)
		{
			return Task.Factory.StartNew(function, cancellationToken, TaskCreationOptions.None, TaskScheduler.Default);
		}

		public static Task Run(Func<Task> function) => Run(function, CancellationToken.None);

		public static Task Run(Func<Task> function, CancellationToken cancellationToken)
		{
			return Run<Task>(function, cancellationToken).Unwrap();
		}

		public static Task<TResult> Run<TResult>(Func<Task<TResult>> function)
		{
			return Run(function, CancellationToken.None);
		}

		public static Task<TResult> Run<TResult>(
		  Func<Task<TResult>> function,
		  CancellationToken cancellationToken)
		{
			return Run<Task<TResult>>(function, cancellationToken).Unwrap();
		}

		/// <summary>Starts a Task that will complete after the specified due time.</summary>
		/// <param name="dueTime">The delay in milliseconds before the returned task completes.</param>
		/// <returns>The timed Task.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="dueTime" /> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.
		/// </exception>
		public static Task Delay(int dueTime) => Delay(dueTime, CancellationToken.None);

		/// <summary>Starts a Task that will complete after the specified due time.</summary>
		/// <param name="dueTime">The delay before the returned task completes.</param>
		/// <returns>The timed Task.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="dueTime" /> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.
		/// </exception>
		public static Task Delay(TimeSpan dueTime) => Delay(dueTime, CancellationToken.None);

		/// <summary>Starts a Task that will complete after the specified due time.</summary>
		/// <param name="dueTime">The delay before the returned task completes.</param>
		/// <param name="cancellationToken">A CancellationToken that may be used to cancel the task before the due time occurs.</param>
		/// <returns>The timed Task.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="dueTime" /> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.
		/// </exception>
		public static Task Delay(TimeSpan dueTime, CancellationToken cancellationToken)
		{
			long totalMilliseconds = (long)dueTime.TotalMilliseconds;
			if (totalMilliseconds < -1L || totalMilliseconds > int.MaxValue)
				throw new ArgumentOutOfRangeException(nameof(dueTime), "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.");
			Contract.EndContractBlock();
			return Delay((int)totalMilliseconds, cancellationToken);
		}

		/// <summary>Starts a Task that will complete after the specified due time.</summary>
		/// <param name="dueTime">The delay in milliseconds before the returned task completes.</param>
		/// <param name="cancellationToken">A CancellationToken that may be used to cancel the task before the due time occurs.</param>
		/// <returns>The timed Task.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// The <paramref name="dueTime" /> argument must be non-negative or -1 and less than or equal to Int32.MaxValue.
		/// </exception>
		public static Task Delay(int dueTime, CancellationToken cancellationToken)
		{
			if (dueTime < -1)
				throw new ArgumentOutOfRangeException(nameof(dueTime), "The timeout must be non-negative or -1, and it must be less than or equal to Int32.MaxValue.");
			Contract.EndContractBlock();
			if (cancellationToken.IsCancellationRequested)
				return new Task(() => { }, cancellationToken);
			if (dueTime == 0)
				return s_preCompletedTask;
			TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
			CancellationTokenRegistration ctr = new CancellationTokenRegistration();
			Timer timer = null;
			timer = new Timer(state =>
			{
				ctr.Dispose();
				timer.Dispose();
				tcs.TrySetResult(true);
				TimerManager.Remove(timer);
			}, null, -1, -1);
			TimerManager.Add(timer);
			if (cancellationToken.CanBeCanceled)
				ctr = cancellationToken.Register(() =>
				{
					timer.Dispose();
					tcs.TrySetCanceled();
					TimerManager.Remove(timer);
				});
			timer.Change(dueTime, -1);
			return tcs.Task;
		}

		/// <summary>Creates a Task that will complete only when all of the provided collection of Tasks has completed.</summary>
		/// <param name="tasks">The Tasks to monitor for completion.</param>
		/// <returns>A Task that represents the completion of all of the provided tasks.</returns>
		/// <remarks>
		/// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
		/// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
		/// Task will also be canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null reference.</exception>
		public static Task WhenAll(params Task[] tasks) => WhenAll((IEnumerable<Task>)tasks);

		public static Task<TResult[]> WhenAll<TResult>(params Task<TResult>[] tasks)
		{
			return WhenAll((IEnumerable<Task<TResult>>)tasks);
		}

		/// <summary>Creates a Task that will complete only when all of the provided collection of Tasks has completed.</summary>
		/// <param name="tasks">The Tasks to monitor for completion.</param>
		/// <returns>A Task that represents the completion of all of the provided tasks.</returns>
		/// <remarks>
		/// If any of the provided Tasks faults, the returned Task will also fault, and its Exception will contain information
		/// about all of the faulted tasks.  If no Tasks fault but one or more Tasks is canceled, the returned
		/// Task will also be canceled.
		/// </remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null reference.</exception>
		public static Task WhenAll(IEnumerable<Task> tasks)
		{
			return WhenAllCore(tasks, (Action<Task[], TaskCompletionSource<object>>)((completedTasks, tcs) => tcs.TrySetResult(null)));
		}

		public static Task<TResult[]> WhenAll<TResult>(IEnumerable<Task<TResult>> tasks)
		{
			return WhenAllCore(tasks.Cast<Task>(), (Action<Task[], TaskCompletionSource<TResult[]>>)((completedTasks, tcs) => tcs.TrySetResult(((IEnumerable<Task>)completedTasks).Select<Task, TResult>((Func<Task, TResult>)(t => ((Task<TResult>)t).Result)).ToArray<TResult>())));
		}

		private static Task<TResult> WhenAllCore<TResult>(
		  IEnumerable<Task> tasks,
		  Action<Task[], TaskCompletionSource<TResult>> setResultAction)
		{
			if (tasks == null)
				throw new ArgumentNullException(nameof(tasks));
			Contract.EndContractBlock();
			Contract.Assert(setResultAction != null);
			TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
			if (!(tasks is Task[] taskArray1))
				taskArray1 = tasks.ToArray();
			Task[] taskArray2 = taskArray1;
			if (taskArray2.Length == 0)
				setResultAction(taskArray2, tcs);
			else
				Task.Factory.ContinueWhenAll(taskArray2, completedTasks =>
				{
					List<Exception> targetList = null;
					bool flag = false;
					foreach (Task completedTask in completedTasks)
					{
						if (completedTask.IsFaulted)
							AddPotentiallyUnwrappedExceptions(ref targetList, (Exception)completedTask.Exception);
						else if (completedTask.IsCanceled)
							flag = true;
					}
					if (targetList != null && targetList.Count > 0)
						tcs.TrySetException(targetList);
					else if (flag)
						tcs.TrySetCanceled();
					else
						setResultAction(completedTasks, tcs);
				}, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
			return tcs.Task;
		}

		/// <summary>Creates a Task that will complete when any of the tasks in the provided collection completes.</summary>
		/// <param name="tasks">The Tasks to be monitored.</param>
		/// <returns>
		/// A Task that represents the completion of any of the provided Tasks.  The completed Task is this Task's result.
		/// </returns>
		/// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null reference.</exception>
		public static Task<Task> WhenAny(params Task[] tasks)
		{
			return WhenAny((IEnumerable<Task>)tasks);
		}

		/// <summary>Creates a Task that will complete when any of the tasks in the provided collection completes.</summary>
		/// <param name="tasks">The Tasks to be monitored.</param>
		/// <returns>
		/// A Task that represents the completion of any of the provided Tasks.  The completed Task is this Task's result.
		/// </returns>
		/// <remarks>Any Tasks that fault will need to have their exceptions observed elsewhere.</remarks>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="tasks" /> argument is null.</exception>
		/// <exception cref="T:System.ArgumentException">The <paramref name="tasks" /> argument contains a null reference.</exception>
		public static Task<Task> WhenAny(IEnumerable<Task> tasks)
		{
			if (tasks == null)
				throw new ArgumentNullException(nameof(tasks));
			Contract.EndContractBlock();
			TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>();
			TaskFactory factory = Task.Factory;
			if (!(tasks is Task[] taskArray))
				taskArray = tasks.ToArray();
			CancellationToken none = CancellationToken.None;
			TaskScheduler taskScheduler = TaskScheduler.Default;
			factory.ContinueWhenAny(taskArray, completed => tcs.TrySetResult(completed), none, TaskContinuationOptions.ExecuteSynchronously, taskScheduler);
			return tcs.Task;
		}

		public static Task<Task<TResult>> WhenAny<TResult>(params Task<TResult>[] tasks)
		{
			return WhenAny((IEnumerable<Task<TResult>>)tasks);
		}

		public static Task<Task<TResult>> WhenAny<TResult>(IEnumerable<Task<TResult>> tasks)
		{
			if (tasks == null)
				throw new ArgumentNullException(nameof(tasks));
			Contract.EndContractBlock();
			TaskCompletionSource<Task<TResult>> tcs = new TaskCompletionSource<Task<TResult>>();
			TaskFactory factory = Task.Factory;
			if (!(tasks is Task<TResult>[] taskArray))
				taskArray = tasks.ToArray();
			Func<Task<TResult>, bool> func = (completed => tcs.TrySetResult(completed));
			CancellationToken none = CancellationToken.None;
			TaskScheduler taskScheduler = TaskScheduler.Default;
			factory.ContinueWhenAny(taskArray, func, none, TaskContinuationOptions.ExecuteSynchronously, taskScheduler);
			return tcs.Task;
		}

		/// <summary>Creates an already completed <see cref="T:System.Threading.Tasks.Task" /> from the specified result.</summary>
		/// <param name="result">The result from which to create the completed task.</param>
		/// <returns>The completed task.</returns>
		public static Task<TResult> FromResult<TResult>(TResult result)
		{
			TaskCompletionSource<TResult> completionSource = new TaskCompletionSource<TResult>(result);
			completionSource.TrySetResult(result);
			return completionSource.Task;
		}

		/// <summary>Creates an awaitable that asynchronously yields back to the current context when awaited.</summary>
		/// <returns>
		/// A context that, when awaited, will asynchronously transition back into the current context.
		/// If SynchronizationContext.Current is non-null, that is treated as the current context.
		/// Otherwise, TaskScheduler.Current is treated as the current context.
		/// </returns>
		public static YieldAwaitable Yield() => new YieldAwaitable();

		/// <summary>Adds the target exception to the list, initializing the list if it's null.</summary>
		/// <param name="targetList">The list to which to add the exception and initialize if the list is null.</param>
		/// <param name="exception">The exception to add, and unwrap if it's an aggregate.</param>
		private static void AddPotentiallyUnwrappedExceptions(
		  ref List<Exception> targetList,
		  Exception exception)
		{
			AggregateException aggregateException = exception as AggregateException;
			Contract.Assert(exception != null);
			Contract.Assert(aggregateException == null || aggregateException.InnerExceptions.Count > 0);
			if (targetList == null)
				targetList = new List<Exception>();
			if (aggregateException != null)
				targetList.Add(aggregateException.InnerExceptions.Count == 1 ? exception.InnerException : exception);
			else
				targetList.Add(exception);
		}
	}
}
