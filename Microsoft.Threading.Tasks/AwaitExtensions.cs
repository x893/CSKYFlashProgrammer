using Microsoft.Runtime.CompilerServices;
using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
///     Provides extension methods for threading-related types.
/// </summary>
public static class AwaitExtensions
{
	/// <summary>Cancels the <see cref="T:System.Threading.CancellationTokenSource" /> after the specified duration.</summary>
	/// <param name="source">The CancellationTokenSource.</param>
	/// <param name="dueTime">The due time in milliseconds for the source to be canceled.</param>
	public static void CancelAfter(this CancellationTokenSource source, int dueTime)
	{
		if (source == null)
			throw new NullReferenceException();
		if (dueTime < -1)
			throw new ArgumentOutOfRangeException(nameof(dueTime));
		Contract.EndContractBlock();
		Timer timer = null;
		timer = new Timer(state =>
		{
			timer.Dispose();
			TimerManager.Remove(timer);
			try
			{
				source.Cancel();
			}
			catch (ObjectDisposedException) { }
		}, null, -1, -1);
		TimerManager.Add(timer);
		timer.Change(dueTime, -1);
	}

	/// <summary>Cancels the <see cref="T:System.Threading.CancellationTokenSource" /> after the specified duration.</summary>
	/// <param name="source">The CancellationTokenSource.</param>
	/// <param name="dueTime">The due time for the source to be canceled.</param>
	public static void CancelAfter(this CancellationTokenSource source, TimeSpan dueTime)
	{
		long totalMilliseconds = (long)dueTime.TotalMilliseconds;
		if (totalMilliseconds < -1L || totalMilliseconds > (long)int.MaxValue)
			throw new ArgumentOutOfRangeException(nameof(dueTime));
		AwaitExtensions.CancelAfter(source, (int)totalMilliseconds);
	}

	/// <summary>Gets an awaiter used to await this <see cref="T:System.Threading.Tasks.Task" />.</summary>
	/// <param name="task">The task to await.</param>
	/// <returns>An awaiter instance.</returns>
	public static TaskAwaiter GetAwaiter(this Task task)
	{
		return task != null ? new TaskAwaiter(task) : throw new ArgumentNullException(nameof(task));
	}

	public static TaskAwaiter<TResult> GetAwaiter<TResult>(this Task<TResult> task)
	{
		return task != null ? new TaskAwaiter<TResult>(task) : throw new ArgumentNullException(nameof(task));
	}

	/// <summary>Creates and configures an awaitable object for awaiting the specified task.</summary>
	/// <param name="task">The task to be awaited.</param>
	/// <param name="continueOnCapturedContext">
	/// true to automatic marshag back to the original call site's current SynchronizationContext
	/// or TaskScheduler; otherwise, false.
	/// </param>
	/// <returns>The instance to be awaited.</returns>
	public static ConfiguredTaskAwaitable ConfigureAwait(
	  this Task task,
	  bool continueOnCapturedContext)
	{
		return task != null ? new ConfiguredTaskAwaitable(task, continueOnCapturedContext) : throw new ArgumentNullException(nameof(task));
	}

	public static ConfiguredTaskAwaitable<TResult> ConfigureAwait<TResult>(
	  this Task<TResult> task,
	  bool continueOnCapturedContext)
	{
		return task != null ? new ConfiguredTaskAwaitable<TResult>(task, continueOnCapturedContext) : throw new ArgumentNullException(nameof(task));
	}
}
