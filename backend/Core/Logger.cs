namespace Core
{
	public static class Logger
	{
		public static void LogInfo(string message)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine($"[+] {message}");
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void LogWarn(string message)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($"[+] {message}");
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static void LogFatal(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"[+] {message}");
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}