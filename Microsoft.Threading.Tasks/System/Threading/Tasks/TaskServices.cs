using System.ComponentModel;

namespace System.Threading.Tasks
{

    internal class TaskServices
    {
        /// <summary>Returns a canceled task.</summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The canceled task.</returns>
        public static Task FromCancellation(CancellationToken cancellationToken)
        {
            return cancellationToken.IsCancellationRequested ? new Task((Action)(() => { }), cancellationToken) : throw new ArgumentOutOfRangeException(nameof(cancellationToken));
        }

        /// <summary>Returns a canceled task.</summary>
        /// <typeparam name="TResult">Specifies the type of the result.</typeparam>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The canceled task.</returns>
        public static Task<TResult> FromCancellation<TResult>(CancellationToken cancellationToken)
        {
            return cancellationToken.IsCancellationRequested ? new Task<TResult>((Func<TResult>)(() => default(TResult)), cancellationToken) : throw new ArgumentOutOfRangeException(nameof(cancellationToken));
        }

        public static void HandleEapCompletion<T>(
          TaskCompletionSource<T> tcs,
          bool requireMatch,
          AsyncCompletedEventArgs e,
          Func<T> getResult,
          Action unregisterHandler)
        {
            if (requireMatch)
            {
                if (e.UserState != tcs)
                    return;
            }
            try
            {
                unregisterHandler();
            }
            finally
            {
                if (e.Cancelled)
                    tcs.TrySetCanceled();
                else if (e.Error != null)
                    tcs.TrySetException(e.Error);
                else
                    tcs.TrySetResult(getResult());
            }
        }
    }
}
