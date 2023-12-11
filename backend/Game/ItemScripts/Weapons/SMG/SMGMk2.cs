namespace Game.ItemScripts.Weapons.SMG
{
	public class SMGMk2 : WeaponItemScript
	{
		public SMGMk2() : base(79, 2024373456, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class SMGMk2Ammo : AmmoItemScript
	{
		public SMGMk2Ammo() : base(127, 2024373456, 30)
		{
		}
	}
}