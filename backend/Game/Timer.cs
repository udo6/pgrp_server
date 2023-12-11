using Core.Attribute;
using System.Reflection;
using System.Timers;

namespace Game
{
	public class Timer
	{
		public static readonly List<MethodInfo> EveryFifteenMinuteActions = new();
		public static readonly List<MethodInfo> EveryMinuteActions = new();
		public static readonly List<MethodInfo> EveryTenSecondsActions = new();
		public static readonly List<MethodInfo> EveryFiveSecondsActions = new();

		[Initialize]
		public static void Initialize()
		{
			var fifteenminutes = new System.Timers.Timer();
			fifteenminutes.Interval =  900000;
			fifteenminutes.Elapsed += EveryFifteenMinutes;
			fifteenminutes.AutoReset = true;
			fifteenminutes.Start();

			var minute = new System.Timers.Timer();
			minute.Interval = 60000;
			minute.Elapsed += EveryMinute;
			minute.AutoReset = true;
			minute.Start();

			var tenSecond = new System.Timers.Timer();
			tenSecond.Interval = 10000;
			tenSecond.Elapsed += EveryTenSeconds;
			tenSecond.AutoReset = true;
			tenSecond.Start();

			var fiveSecond = new System.Timers.Timer();
			fiveSecond.Interval = 5000;
			fiveSecond.Elapsed += EveryFiveSeconds;
			fiveSecond.AutoReset = true;
			fiveSecond.Start();
		}

		public static void EveryFifteenMinutes(object? sender, ElapsedEventArgs e)
		{
			foreach (var action in EveryFifteenMinuteActions)
				action.Invoke(null, Array.Empty<object>());
		}

		public static void EveryMinute(object? sender, ElapsedEventArgs e)
		{
			foreach(var action in EveryMinuteActions)
				action.Invoke(null, Array.Empty<object>());
		}

		public static void EveryTenSeconds(object? sender, ElapsedEventArgs e)
		{
			foreach (var action in EveryTenSecondsActions)
				action.Invoke(null, Array.Empty<object>());
		}

		public static void EveryFiveSeconds(object? sender, ElapsedEventArgs e)
		{
			foreach (var action in EveryFiveSecondsActions)
				action.Invoke(null, Array.Empty<object>());
		}
	}
}