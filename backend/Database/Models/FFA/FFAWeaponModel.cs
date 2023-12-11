namespace Database.Models.FFA
{
	public class FFAWeaponModel
	{
		public int Id { get; set; }
		public int FFAId { get; set; }
		public uint WeaponHash { get; set; }

		public FFAWeaponModel()
		{
		}

		public FFAWeaponModel(int fFAId, uint weaponHash)
		{
			FFAId = fFAId;
			WeaponHash = weaponHash;
		}
	}
}