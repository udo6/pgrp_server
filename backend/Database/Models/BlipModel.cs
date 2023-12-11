namespace Database.Models
{
	public class BlipModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int PositionId { get; set; }
		public int Sprite { get; set; }
		public int Color { get; set; }
		public bool ShortRange { get; set; }

		public BlipModel()
		{
			Name = string.Empty;
		}

		public BlipModel(string name, int positionId, int sprite, int color, bool shortRange)
		{
			Name = name;
			PositionId = positionId;
			Sprite = sprite;
			Color = color;
			ShortRange = shortRange;
		}
	}
}