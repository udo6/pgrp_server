namespace Core
{
	public static class Config
	{
		public static readonly bool DevMode = false;

		public static class Inventory
		{
			public static readonly int DEFAULT_SLOTS = 6;
			public static readonly float DEFAULT_WEIGHT = 25f;

			public static readonly int BACKPACK_SLOTS = 16;
			public static readonly float BACKPACK_WEIGHT = 60f;
		}
	}
}