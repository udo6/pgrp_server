namespace Core.Models.NativeMenu
{
	public class NativeMenu
	{
		public string Title { get; set; }
		public List<NativeMenuItem> Items { get; set; }

		public NativeMenu()
		{
			Title = string.Empty;
			Items = new();
		}

		public NativeMenu(string title, List<NativeMenuItem> items)
		{
			Title = title;
			Items = items;
		}
	}
}