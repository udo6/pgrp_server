namespace Game.ItemScripts.Weapons.Rifles
{
	public class Advancedrifle : WeaponItemScript
	{
		public Advancedrifle() : base(1, 2937143193u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AdvancedrifleAmmo : AmmoItemScript
	{
		public AdvancedrifleAmmo() : base(107, 2937143193u, 30)
		{
		}
	}

	public class AdvancedrifleSuppressor : AttatchmentItemScript
	{
		public AdvancedrifleSuppressor() : base(380, 0x837445AA, 2937143193)
		{
		}
	}

	public class AdvancedrifleScope : AttatchmentItemScript
	{
		public AdvancedrifleScope() : base(381, 0xAA2C45B4, 2937143193)
		{
		}
	}
}