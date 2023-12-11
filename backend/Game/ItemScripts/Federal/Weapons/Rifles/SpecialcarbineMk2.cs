namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class SpecialcarbineMk2 : WeaponItemScript
	{
		public SpecialcarbineMk2() : base(185, 2526821735u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class SpecialcarbineMk2Ammo : AmmoItemScript
	{
		public SpecialcarbineMk2Ammo() : base(244, 2526821735u, 30, true)
		{
		}
	}
}