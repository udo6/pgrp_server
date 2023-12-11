using Core.Enums;

namespace Core.Attribute
{
	[AttributeUsage(AttributeTargets.Method)]
	public class ServerEventAttribute : System.Attribute
	{
		public ServerEventType EventType { get; set; }

		public ServerEventAttribute(ServerEventType type)
		{
			EventType = type;
		}
	}
}