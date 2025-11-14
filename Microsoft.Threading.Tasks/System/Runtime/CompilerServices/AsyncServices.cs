using System.Threading;

namespace System.Runtime.CompilerServices
{

    internal static class AsyncServices
    {
        /// <summary>Throws the exception on the ThreadPool.</summary>
        /// <param name="exception">The exception to propagate.</param>
        /// <param name="targetContext">The target context on which to propagate the exception.  Null to use the ThreadPool.</param>
        internal static void ThrowAsync(Exception exception, SynchronizationContext targetContext)
        {
            if (targetContext != null)
            {
                try
                {
                    targetContext.Post((state =>
                    {
                        throw PrepareExceptionForRethrow((Exception)state);
                    }), exception);
                    return;
                }
                catch (Exception ex)
                {
                    exception = new AggregateException(new Exception[2]
                    {
          exception,
          ex
                    });
                }
            }
            ThreadPool.QueueUserWorkItem(state =>
            {
                throw PrepareExceptionForRethrow((Exception)state);
            }, exception);
        }

        /// <summary>Copies the exception's stack trace so its stack trace isn't overwritten.</summary>
        /// <param name="exc">The exception to prepare.</param>
        internal static Exception PrepareExceptionForRethrow(Exception exc) => exc;
    }
}
