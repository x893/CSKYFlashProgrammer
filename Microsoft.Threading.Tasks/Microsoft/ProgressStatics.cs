using System.Threading;

namespace Microsoft
{
	/// <summary>Holds static values for <see cref="T:Microsoft.Progress" />.</summary>
	/// <remarks>This avoids one static instance per type T.</remarks>
	internal static class ProgressStatics
	{
		/// <summary>A default synchronization context that targets the ThreadPool.</summary>
		internal static readonly SynchronizationContext DefaultContext = new SynchronizationContext();
	}
}
