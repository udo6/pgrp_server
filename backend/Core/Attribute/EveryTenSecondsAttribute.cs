namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class EveryTenSecondsAttribute : System.Attribute
	{
		public EveryTenSecondsAttribute() { }
	}
}