namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryFiveSecondsAttribute : System.Attribute
	{
		public EveryFiveSecondsAttribute() { }
	}
}