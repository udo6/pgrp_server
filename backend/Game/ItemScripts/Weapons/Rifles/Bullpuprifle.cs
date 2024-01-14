namespace Game.ItemScripts.Weapons.Rifles
{
	public class Bullpuprifle : WeaponItemScript
	{
		public Bullpuprifle() : base(51, 2132975508u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class BullpuprifleAmmo : AmmoItemScript
	{
		public BullpuprifleAmmo() : base(110, 2132975508u, 30)
		{
		}
	}

	public class BullpuprifleSuppressor : AttatchmentItemScript
	{
		public BullpuprifleSuppressor() : base(385, 0x837445AA, 2132975508u)
		{
		}
	}

	public class BullpuprifleScope : AttatchmentItemScript
	{
		public BullpuprifleScope() : base(386, 0xAA2C45B4, 2132975508u)
		{
		}
	}

	public class BullpuprifleGrip : AttatchmentItemScript
	{
		public BullpuprifleGrip() : base(387, 0xC164F53, 2132975508u)
		{
		}
	}
}