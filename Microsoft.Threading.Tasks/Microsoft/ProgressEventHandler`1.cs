namespace Microsoft
{
	/// <summary>Event handler for progress reports.</summary>
	/// <typeparam name="T">Specifies the type of data for the progress report.</typeparam>
	/// <param name="sender">The sender of the report.</param>
	/// <param name="value">The reported value.</param>
	public delegate void ProgressEventHandler<T>(object sender, T value);
}