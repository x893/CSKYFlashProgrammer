using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace Microsoft
{

	/// <summary>
	/// Provides an IProgress{T} that invokes callbacks for each reported progress value.
	/// </summary>
	/// <typeparam name="T">Specifies the type of the progress report value.</typeparam>
	/// <remarks>
	/// Any handler provided to the constructor or event handlers registered with
	/// the <see cref="E:Microsoft.Progress.ProgressChanged" /> event are invoked through a
	/// <see cref="T:System.Threading.SynchronizationContext" /> instance captured
	/// when the instance is constructed.  If there is no current SynchronizationContext
	/// at the time of construction, the callbacks will be invoked on the ThreadPool.
	/// </remarks>
	public class Progress<T> : IProgress<T>
	{
		/// <summary>The synchronization context captured upon construction.  This will never be null.</summary>
		private readonly SynchronizationContext m_synchronizationContext;
		/// <summary>The handler specified to the constructor.  This may be null.</summary>
		private readonly Action<T> m_handler;
		/// <summary>A cached delegate used to post invocation to the synchronization context.</summary>
		private readonly SendOrPostCallback m_invokeHandlers;

		/// <summary>Initializes the <see cref="T:Microsoft.Progress" />.</summary>
		public Progress()
		{
			m_synchronizationContext = SynchronizationContext.Current ?? ProgressStatics.DefaultContext;
			Contract.Assert(m_synchronizationContext != null);
			m_invokeHandlers = new SendOrPostCallback(InvokeHandlers);
		}

		/// <summary>Initializes the <see cref="T:Microsoft.Progress" /> with the specified callback.</summary>
		/// <param name="handler">
		/// A handler to invoke for each reported progress value.  This handler will be invoked
		/// in addition to any delegates registered with the <see cref="E:Microsoft.Progress.ProgressChanged" /> event.
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="handler" /> is null (Nothing in Visual Basic).</exception>
		public Progress(Action<T> handler)
		  : this()
		{
			m_handler = handler ?? throw new ArgumentNullException(nameof(handler));
		}

		/// <summary>Raised for each reported progress value.</summary>
		/// <remarks>
		/// Handlers registered with this event will be invoked on the
		/// <see cref="T:System.Threading.SynchronizationContext" /> captured when the instance was constructed.
		/// </remarks>
		public event ProgressEventHandler<T> ProgressChanged;

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		protected virtual void OnReport(T value)
		{
			Action<T> handler = m_handler;
			ProgressEventHandler<T> progressChanged = ProgressChanged;
			if (handler == null && progressChanged == null)
				return;
			m_synchronizationContext.Post(m_invokeHandlers, value);
		}

		/// <summary>Reports a progress change.</summary>
		/// <param name="value">The value of the updated progress.</param>
		void IProgress<T>.Report(T value) => OnReport(value);

		/// <summary>Invokes the action and event callbacks.</summary>
		/// <param name="state">The progress value.</param>
		private void InvokeHandlers(object state)
		{
			T obj = (T)state;
			Action<T> handler = m_handler;
			ProgressEventHandler<T> progressChanged = ProgressChanged;
			handler?.Invoke(obj);

			if (progressChanged == null)
				return;
			progressChanged(this, obj);
		}
	}
}
