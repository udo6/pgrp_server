namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryFifteenMinutesAttribute : System.Attribute
	{
		public EveryFifteenMinutesAttribute() { }
	}
}