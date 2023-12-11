namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryMinuteAttribute : System.Attribute
	{
		public EveryMinuteAttribute() { }
	}
}