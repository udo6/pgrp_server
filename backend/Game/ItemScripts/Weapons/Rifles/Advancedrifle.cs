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
		public AdvancedrifleSuppressor() : base(24, 2205435306, 2937143193)
		{
		}
	}
}