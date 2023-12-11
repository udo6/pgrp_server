using Core.Enums;

namespace Core.Models.Input
{
	public class InputClientData
	{
		public string Title { get; set; }
		public string Message { get; set; }
		public int Type { get; set; }
		public string CallbackEvent { get; set; }
		public object[] CallbackArgs { get; set; }

		public InputClientData(string title, string message, InputType type, string callback, params object[] args)
		{
			Title = title;
			Message = message;
			Type = (int)type;
			CallbackEvent = callback;
			CallbackArgs = args;
		}
	}
}