namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandAttribute : System.Attribute
	{
		public string EventName { get; set; }
		public bool GreedyArg { get; set; }

		public CommandAttribute(string cmd, bool greedyArg = false)
		{
			EventName = cmd;
			GreedyArg = greedyArg;
		}
	}
}