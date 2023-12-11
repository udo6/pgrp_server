namespace Game.ItemScripts.Weapons.Rifles
{
	public class SpecialcarbineMk2 : WeaponItemScript
	{
		public SpecialcarbineMk2() : base(59, 2526821735u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class SpecialcarbineMk2Ammo : AmmoItemScript
	{
		public SpecialcarbineMk2Ammo() : base(118, 2526821735u, 30)
		{
		}
	}
}