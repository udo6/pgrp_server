namespace Core.Models.Player
{
	public class WeaponModel
	{
		public uint Hash { get; set; }
		public byte TintIndex { get; set; }
		public List<uint> Components { get; set; }

		public WeaponModel()
		{
			Components = new();
		}

		public WeaponModel(uint hash, byte tintIndex, List<uint> components)
		{
			Hash = hash;
			TintIndex = tintIndex;
			Components = components;
		}
	}
}
