namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Advancedrifle : WeaponItemScript
	{
		public Advancedrifle() : base(273, 2937143193u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AdvancedrifleAmmo : AmmoItemScript
	{
		public AdvancedrifleAmmo() : base(233, 2937143193u, 30, true)
		{
		}
	}
}