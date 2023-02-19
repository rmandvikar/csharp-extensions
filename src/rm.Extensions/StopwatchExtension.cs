using System.Diagnostics;

namespace rm.Extensions;

/// <summary>
/// Stopwatch extensions.
/// </summary>
public static class StopwatchExtension
{
	public static long ElapsedTicks(this Stopwatch sw)
	{
		return sw.ElapsedTicks;
	}
	public static long ElapsedMilliseconds(this Stopwatch sw)
	{
		return sw.ElapsedMilliseconds;
	}
	public static long ElapsedSeconds(this Stopwatch sw)
	{
		return sw.ElapsedMilliseconds / 1000;
	}
}
