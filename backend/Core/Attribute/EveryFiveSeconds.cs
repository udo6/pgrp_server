namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryFiveSeconds : System.Attribute
	{
		public EveryFiveSeconds() { }
	}
}