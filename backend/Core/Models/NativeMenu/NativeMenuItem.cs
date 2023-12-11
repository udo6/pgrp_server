namespace Core.Models.NativeMenu
{
	public class NativeMenuItem
	{
		public string Label { get; set; }
		public string CallbackEvent { get; set; }
		public object[] CallbackArgs { get; set; }
		public bool Close { get; set; }

		public NativeMenuItem()
		{
			Label = string.Empty;
			CallbackEvent = string.Empty;
			CallbackArgs = Array.Empty<object>();
			Close = false;
		}

		public NativeMenuItem(string label, bool close, string callbackEvent, params object[] callbackArgs)
		{
			Label = label;
			CallbackEvent = callbackEvent;
			CallbackArgs = callbackArgs;
			Close = close;
		}
	}
}